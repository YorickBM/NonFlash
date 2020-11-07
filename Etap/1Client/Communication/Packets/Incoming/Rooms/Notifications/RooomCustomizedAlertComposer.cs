using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Rooms.Notifications
{
    class RoomCustomizedAlertComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x0 = Packet.PopInt();
            string message = Packet.PopString();
        }
    }
}