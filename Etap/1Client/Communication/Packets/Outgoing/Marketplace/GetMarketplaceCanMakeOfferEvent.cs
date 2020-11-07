using Retro.Communication.Packets.Outgoing.Marketplace;

namespace Retro.Communication.Packets.Incoming.Marketplace
{
    class GetMarketplaceCanMakeOfferEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new MarketplaceCanMakeOfferResultComposer((Session.GetHabbo().TradingLockExpiry > 0 ? 6 : 1)));
        }
    }
}