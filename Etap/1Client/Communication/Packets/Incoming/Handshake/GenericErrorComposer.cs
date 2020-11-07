using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
    class GenericErrorComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
        }
    }
}
