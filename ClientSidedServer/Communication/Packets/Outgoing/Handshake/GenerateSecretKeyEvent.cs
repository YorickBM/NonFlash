using Retro.Communication.Encryption;
using Retro.Communication.Encryption.Crypto.Prng;
using Retro.Communication.Packets.Outgoing;
using Retro.Communication.Packets.Outgoing.Handshake;

namespace Retro.Communication.Packets.Incoming.Handshake
{
    public class GenerateSecretKeyEvent : ServerPacket
    {
        public GenerateSecretKeyEvent(string PrimeKey) : base(ServerPacketHeader.GenerateSecretKeyMessageEvent)
        {
        }
    }
}