using System.Linq;
using System.Collections.Generic;

using Retro.Utilities;
using Retro.Hotel.Users.Messenger;

using Retro.Communication.Packets.Outgoing.Messenger;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class HabboSearchEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            string Query = StringCharFilter.Escape(Packet.PopString().Replace("%", ""));
            if (Query.Length < 1 || Query.Length > 100)
                return;

            List<SearchResult> Friends = new List<SearchResult>();
            List<SearchResult> OthersUsers = new List<SearchResult>();

            List<SearchResult> Results = SearchResultFactory.GetSearchResult(Query);
            foreach (SearchResult Result in Results.ToList())
            {
                if (Session.GetHabbo().GetMessenger().FriendshipExists(Result.UserId))
                    Friends.Add(Result);
                else
                    OthersUsers.Add(Result);
            }

            Session.SendMessage(new HabboSearchResultComposer(Friends, OthersUsers));
        }
    }
}