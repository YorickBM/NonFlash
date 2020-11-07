
using Retro.Core;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Data;
using System;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class SendMsgEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            int userId = Packet.PopInt();
            if (userId == 0 || userId == Session.GetHabbo().Id)
                return;

            string message = Packet.PopString();
            if (string.IsNullOrWhiteSpace(message)) return;
            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendWhisper("Oops, you've been silenced for 15 seconds, you can not send messages during this time.");
                return;
            }

            string word;
            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(message, out word))
            {
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= 1)
                {

					Session.GetHabbo().TimeMuted = 25;
                    Session.SendNotification("You have been silenced for divulging a Hotel! " + Session.GetHabbo().BannedPhraseCount + "/3");
                    RetroEnvironment.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Discloser Alert:",
                        "Attention, you mentioned the word <b>" + word.ToUpper() + "</b><br><br><b>Phrase:</b><br><i>" + message +
                        "</i>.<br><br><b>Tipo</b><br>Spam for chatting.\r\n" + "- This User: <b>" +
                        Session.GetHabbo().Username + "</b>", NotificationSettings.NOTIFICATION_FILTER_IMG, "", ""));
				}
                if (Session.GetHabbo().BannedPhraseCount >= 5)
                {
                    RetroEnvironment.GetGame().GetModerationManager().BanUser("System", Hotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Banned for spamming with the phrase (" + message + ")", (RetroEnvironment.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                return;
            }

            if (Session.GetHabbo().Rank > 0)
            {
                DataRow presothiago = null;
                using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Presidio FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                    presothiago = dbClient.getRow();
                }

                if (Convert.ToBoolean(presothiago["Presidio"]) == true)
                {
                    if (Session.GetHabbo().Rank > 0)
                    {
                        string thiago = Session.GetHabbo().Look;
                        Session.SendMessage(new RoomNotificationComposer("police_announcement", "message", "Você esta preso e não pode enviar mensagens."));
                        return;
                    }
                }
            }

            Session.GetHabbo().GetMessenger().SendInstantMessage(userId, message);

        }
    }
}