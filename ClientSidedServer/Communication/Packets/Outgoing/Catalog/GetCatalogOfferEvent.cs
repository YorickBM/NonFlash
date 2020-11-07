using Retro.Hotel.Catalog;
using Retro.Communication.Packets.Outgoing.Catalog;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!RetroEnvironment.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = RetroEnvironment.GetGame().GetCatalog().ItemOffers[OfferId];

            CatalogPage Page;
            if (!RetroEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank  >  Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

            CatalogItem Item = null;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            Item = (CatalogItem)Page.ItemOffers[OfferId];
            if (Item != null)
                Session.SendMessage(new CatalogOfferComposer(Item));
        }
    }
}
