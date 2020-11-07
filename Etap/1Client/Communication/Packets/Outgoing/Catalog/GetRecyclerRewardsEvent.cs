using Retro.Communication.Packets.Outgoing.Catalog;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    public class GetRecyclerRewardsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new RecyclerRewardsComposer());
        }
    }
}