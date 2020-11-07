using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Handshake
{
	class SetUniqueIdComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Logger.DebugWarn(Packet.PopString());
        }
    }
}
