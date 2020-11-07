using Etap.Communication.Encryption;
using Etap.Communication.Encryption.Crypto.Prng;
using Etap.Communication.Packets.Outgoing;
using Etap.Communication.Packets.Outgoing.Handshake;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    public class GenerateSecretKeyEvent : ServerPacket
    {
        public GenerateSecretKeyEvent(string PrimeKey) : base(ServerPacketHeader.GenerateSecretKeyMessageEvent)
        {
            base.WriteString(PrimeKey);
        }
    }
}