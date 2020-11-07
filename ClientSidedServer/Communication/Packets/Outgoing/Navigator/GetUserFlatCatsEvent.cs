using System.Collections.Generic;
using Retro.Hotel.Navigator;
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Navigator;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = RetroEnvironment.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}