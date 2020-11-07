using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
    public class AuthenticationOKComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            RetroEnvironment.ConnectionIsSucces = true;
            RetroEnvironment.InitializeIntervalTimers();
        }
    }
}