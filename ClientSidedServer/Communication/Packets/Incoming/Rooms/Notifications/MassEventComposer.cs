using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Rooms.Notifications
{
    class MassEventComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string message = Packet.PopString();
        }
    }
}