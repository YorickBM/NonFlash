using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Handshake
{
	class SetUniqueIdComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Id = Packet.PopString();
            Logger.DebugWarn(Id);
        }
    }
}
