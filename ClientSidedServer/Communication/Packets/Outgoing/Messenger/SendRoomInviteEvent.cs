
using System.Collections.Generic;
using Retro.Core;
using Retro.Utilities;
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Messenger;
using Retro.Database.Interfaces;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class SendRoomInviteEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendNotification("Oops, your muted - you can not send room invitations.");
                return;
            }

            int Amount = Packet.PopInt();
            if (Amount > 500)
                return; // don't send at all

            List<int> Targets = new List<int>();
            for (int i = 0; i < Amount; i++)
            {
                int uid = Packet.PopInt();
                if (i < 100) // limit to 100 people, keep looping until we fulfil the request though
                {
                    Targets.Add(uid);
                }
            }

            string Message = StringCharFilter.Escape(Packet.PopString());
            if (Message.Length > 121)
                Message = Message.Substring(0, 121);

            string word;
            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Message, out word))
            {
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= 1)
                {
                    Session.GetHabbo().TimeMuted = 25;
                    Session.SendNotification("You have been silenced for divulging a Hotel! " + Session.GetHabbo().BannedPhraseCount + "/3");
                    RetroEnvironment.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Discloser Alert:",
                        "Attention, you mentioned the word <b>" + word.ToUpper() + "</b><br><br><b>Phrase:</b><br><i>" + Message +
                        "</i>.<br><br><b>Tipo</b><br>Spam for chatting.\r\n" + "- This User: <b>" +
                        Session.GetHabbo().Username + "</b>", NotificationSettings.NOTIFICATION_FILTER_IMG, "", ""));
                }
                if (Session.GetHabbo().BannedPhraseCount >= 3)
                {
                    RetroEnvironment.GetGame().GetModerationManager().BanUser("System", Hotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Banned for spamming with the phrase (" + Message + ")", (RetroEnvironment.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                return;
            }

            foreach (int UserId in Targets)
            {
                if (!Session.GetHabbo().GetMessenger().FriendshipExists(UserId))
                    continue;

                GameClient Client = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().AllowMessengerInvites == true || Client.GetHabbo().AllowConsoleMessages == false)
                    continue;

                Client.SendMessage(new RoomInviteComposer(Session.GetHabbo().Id, Message));

            }

            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `chatlogs_console_invitations` (`user_id`,`message`,`timestamp`) VALUES ('" + Session.GetHabbo().Id + "', @message, UNIX_TIMESTAMP())");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();
            }
        }
    }
}