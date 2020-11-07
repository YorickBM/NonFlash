using System.Collections.Generic;

namespace Etap.Communication.Packets.Outgoing.Navigator
{
    public class NavigatorSearchEvent : ServerPacket
    {
        public NavigatorSearchEvent(string category, string search) : base (ServerPacketHeader.NavigatorSearchMessageEvent)
        {
            base.WriteString(category);
            base.WriteString(search);
        }

        /*public void Parse(Hotel.GameClients.GameClient session, ClientPacket packet)
        {
            string Category = packet.PopString();
            string Search = packet.PopString();

            ICollection<SearchResultList> Categories = new List<SearchResultList>();

            if (!string.IsNullOrEmpty(Search))
            {
                SearchResultList QueryResult = null;
                if (RetroEnvironment.GetGame().GetNavigator().TryGetSearchResultList(0, out QueryResult))
                {
                    Categories.Add(QueryResult);
                }
            }
            else
            {
                Categories = RetroEnvironment.GetGame().GetNavigator().GetCategorysForSearch(Category);
                if (Categories.Count == 0)
                {
                    //Are we going in deep?!
                    Categories = RetroEnvironment.GetGame().GetNavigator().GetResultByIdentifier(Category);
                    if (Categories.Count > 0)
                    {
                        session.SendMessage(new NavigatorSearchResultSetComposer(Category, Search, Categories, session, 2, 100));
                        return;
                    }
                }
            }

            session.SendMessage(new NavigatorSearchResultSetComposer(Category, Search, Categories, session));
        } */

    }
}
