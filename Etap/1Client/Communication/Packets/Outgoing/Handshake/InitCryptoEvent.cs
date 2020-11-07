
using Etap.Communication.Encryption;
using Etap.Communication.Packets.Outgoing;
using Etap.Communication.Packets.Outgoing.Handshake;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    public class InitCryptoEvent : ServerPacket
    {
        public InitCryptoEvent() : base(ServerPacketHeader.InitCryptoMessageEvent)
        {
        }
    }
}