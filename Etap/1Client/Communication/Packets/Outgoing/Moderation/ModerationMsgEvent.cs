using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class ModerationMsgEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_alert"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();

            GameClient Client = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null)
                return;

            Client.SendNotification(Message);
        }
    }
}
