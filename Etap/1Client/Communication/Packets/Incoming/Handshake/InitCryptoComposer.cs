using Etap.Communication.Packets.Incoming;
using Etap.Communication.Packets.Incoming.Handshake;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Handshake
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