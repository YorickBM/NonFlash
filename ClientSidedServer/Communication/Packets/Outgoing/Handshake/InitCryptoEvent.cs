
using Retro.Communication.Encryption;
using Retro.Communication.Packets.Outgoing;
using Retro.Communication.Packets.Outgoing.Handshake;

namespace Retro.Communication.Packets.Incoming.Handshake
{
    public class InitCryptoEvent : ServerPacket
    {
        public InitCryptoEvent() : base(ServerPacketHeader.InitCryptoMessageEvent)
        {
        }
    }
}