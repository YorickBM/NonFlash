using Retro.Communication.Packets.Incoming;
using Retro.Communication.Packets.Incoming.Handshake;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Handshake
{
    public class InitCryptoComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string PrimeKey = Packet.PopString(); //Public?
            string GeneratorKey = Packet.PopString(); //Private?
        }
    }
}