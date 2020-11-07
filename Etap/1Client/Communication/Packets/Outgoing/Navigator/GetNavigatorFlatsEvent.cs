using System.Collections.Generic;

namespace Etap.Communication.Packets.Outgoing.Navigator
{
    class GetNavigatorFlatsEvent : ServerPacket
    {
        /*public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = RetroEnvironment.GetGame().GetNavigator().GetEventCategories();

            Session.SendMessage(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }*/

        public GetNavigatorFlatsEvent() : base(ServerPacketHeader.GetEventCategoriesMessageEvent)
        {

        }
    }
}