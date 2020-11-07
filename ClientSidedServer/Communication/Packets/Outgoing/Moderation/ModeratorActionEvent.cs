using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Moderation;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class ModeratorActionEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_caution"))
                return;

            if (!Session.GetHabbo().InRoom)
                return;

            Room CurrentRoom = Session.GetHabbo().CurrentRoom;
            if (CurrentRoom == null)
                return;

            int AlertMode = Packet.PopInt(); 
            string AlertMessage = Packet.PopString();
            bool IsCaution = AlertMode != 3;

            AlertMessage = IsCaution ? "Precaution of the Moderator:\n\n" + AlertMessage : "Message from Moderator:\n\n" + AlertMessage;
            Session.GetHabbo().CurrentRoom.SendMessage(new BroadcastMessageAlertComposer(AlertMessage));
        }
    }
}
