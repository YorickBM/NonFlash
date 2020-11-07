using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Handshake
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