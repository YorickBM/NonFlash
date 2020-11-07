using System;
using Retro.Hotel.Rooms;
using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Groups;
using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Communication.Packets.Outgoing.Rooms.Session;
using Retro.Communication.Packets.Outgoing.Inventory.Purse;
using Retro.Communication.Packets.Outgoing.Moderation;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class PurchaseGroupEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket packet)
        {
            string word;
            string Name = packet.PopString();
            Name = RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Description = packet.PopString();
            Description = RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Description, out word) ? "Spam" : Description;
            int RoomId = packet.PopInt();
            int Colour1 = packet.PopInt();
            int Colour2 = packet.PopInt();
            int Unknown = packet.PopInt();

            int groupCost = Convert.ToInt32(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("catalog.group.purchase.cost"));

            if (Session.GetHabbo().Credits < groupCost)
            {
                Session.SendMessage(new BroadcastMessageAlertComposer("Um grupo custa " + groupCost + " creditos! E você tem " + Session.GetHabbo().Credits + "!"));
                return;
            }
            else
            {
                Session.GetHabbo().Credits -= groupCost;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }

            RoomData Room = RetroEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Room == null || Room.OwnerId != Session.GetHabbo().Id || Room.Group != null)
                return;

            string Badge = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                Badge += BadgePartUtility.WorkBadgeParts(i == 0, packet.PopInt().ToString(), packet.PopInt().ToString(), packet.PopInt().ToString());
            }

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryCreateGroup(Session.GetHabbo(), Name, Description, RoomId, Badge, Colour1, Colour2, out Group))
            {
                Session.SendNotification("Er is een fout opgetreden bij het maken van deze groep. \n\nProbeer het opnieuw.Als je dit bericht meerdere keren ontvangt, neem dan contact op met de moderator.\r\r");
                return;
            }

            Session.SendMessage(new PurchaseOKComposer());

            Room.Group = Group;

            if (Session.GetHabbo().CurrentRoomId != Room.Id)
                Session.SendMessage(new RoomForwardComposer(Room.Id));

            Session.SendMessage(new NewGroupInfoComposer(RoomId, Group.Id));
        }
    }
}