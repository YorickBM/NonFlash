using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Handshake
{
    class UserPerksComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt(); // Count
            for(int i = 0; i < count; i++)
            {
                string permission = Packet.PopString();
                string requirement = Packet.PopString();
                bool x = Packet.PopBoolean();
                Logger.DebugWarn(permission);
            }
        }
    }
}