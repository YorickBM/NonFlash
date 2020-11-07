using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Rooms.Notifications
{
	class RoomAlertComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x0 = Packet.PopInt();
            string message = Packet.PopString();
        }
    }
}