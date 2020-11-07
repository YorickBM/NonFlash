using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    class FurniMaticPageEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null) return;
            var response = new ServerPacket(ServerPacketHeader.FurniMaticNoRoomError);
            response.WriteInteger(1);
            response.WriteInteger(0);
            Session.SendMessage(response);
        }
    }
}