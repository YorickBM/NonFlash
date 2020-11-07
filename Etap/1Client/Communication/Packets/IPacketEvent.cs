using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient session, ClientPacket packet);
    }
}