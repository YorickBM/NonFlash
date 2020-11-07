using System.Collections.Generic;
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Navigator;
using Retro.Hotel.Navigator;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = RetroEnvironment.GetGame().GetNavigator().GetEventCategories();

            Session.SendMessage(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}