

using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Rooms.Notifications
{
    class RoomErrorNotifComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int errorCode = Packet.PopInt();
        }
    }
}
