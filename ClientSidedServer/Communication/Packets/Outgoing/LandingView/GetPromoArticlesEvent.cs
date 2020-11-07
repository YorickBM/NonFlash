using System.Collections.Generic;
using Retro.Hotel.LandingView.Promotions;
using Retro.Communication.Packets.Outgoing.LandingView;

namespace Retro.Communication.Packets.Incoming.LandingView
{
    class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = RetroEnvironment.GetGame().GetLandingManager().GetPromotionItems();
            Session.SendMessage(new PromoArticlesComposer(LandingPromotions));
            RetroEnvironment.GetGame().GetLandingManager().LoadHallOfFame();
        }
    }
}
