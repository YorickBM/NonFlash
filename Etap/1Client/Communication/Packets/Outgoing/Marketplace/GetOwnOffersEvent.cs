using Retro.Communication.Packets.Outgoing.Marketplace;

namespace Retro.Communication.Packets.Incoming.Marketplace
{
    class GetOwnOffersEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new MarketPlaceOwnOffersComposer(Session.GetHabbo().Id));
        }
    }
}
