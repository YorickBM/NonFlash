using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Rooms.Notifications
{
    class MassEventComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string message = Packet.PopString();
        }
    }
}