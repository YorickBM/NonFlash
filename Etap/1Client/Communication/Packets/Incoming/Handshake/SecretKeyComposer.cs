using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
    public class SecretKeyComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
        }
    }
}