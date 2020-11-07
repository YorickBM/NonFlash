using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Handshake
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
            }
        }
    }
}