using System.Collections.Generic;

namespace Etap.Communication.Packets.Outgoing.Navigator
{
    public class GetUserFlatCatsEvent : ServerPacket
    {
        /*public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = RetroEnvironment.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }*/

        public GetUserFlatCatsEvent() : base(ServerPacketHeader.GetUserFlatCatsMessageEvent)
        {

        }
    }
}