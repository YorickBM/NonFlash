using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Outgoing.BuildersClub
{
    class BuildersClubMembershipComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int maxValue = Packet.PopInt();
            int i0 = Packet.PopInt();
            int i1 = Packet.PopInt();
            int minValue = Packet.PopInt();
        }
    }
}
