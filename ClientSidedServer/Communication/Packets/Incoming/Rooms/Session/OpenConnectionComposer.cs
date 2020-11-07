using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Incoming.Rooms.Session
{
	class OpenConnectionComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Logger.Debug("Open Flat Connection?!?!?");
        }
    }
}
