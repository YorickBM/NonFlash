using System.Collections.Generic;
using System.Linq;
using Etap.Engine.Room;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Incoming.Navigator
{
    class NavigatorSearchResultSetComposer : IPacketEvent
    {
        /*public NavigatorSearchResultSetComposer(string Category, string Data, ICollection<SearchResultList> SearchResultLists, GameClient Session, int GoBack = 1, int FetchLimit = 25)
            : base(ServerPacketHeader.NavigatorSearchResultSetMessageComposer)
        {
			WriteString(Category);//Search code.
			WriteString(Data);//Text?

			WriteInteger(SearchResultLists.Count);//Count
            foreach (SearchResultList SearchResult in SearchResultLists.ToList())
            {
				WriteString(SearchResult.CategoryIdentifier);
				WriteString(SearchResult.PublicName);
				WriteInteger(NavigatorSearchAllowanceUtility.GetIntegerValue(SearchResult.SearchAllowance) != 0 ? GoBack : NavigatorSearchAllowanceUtility.GetIntegerValue(SearchResult.SearchAllowance));//0 = nothing, 1 = show more, 2 = back Action allowed.
				WriteBoolean(false);//True = minimized, false = open.
				WriteInteger(SearchResult.ViewMode == NavigatorViewMode.REGULAR ? 0 : SearchResult.ViewMode == NavigatorViewMode.THUMBNAIL ? 1 : 0);//View mode, 0 = tiny/regular, 1 = thumbnail

                NavigatorHandler.Search(this, SearchResult, Data, Session, FetchLimit);
            }
        }*/

        public void Parse(GameClient session, ClientPacket packet)
        {
            string searchCode = packet.PopString();
            string text = packet.PopString();

            int count = packet.PopInt();
            Logger.Debug("Count:", count, "Text:", text, "SearchCode:", searchCode);

            for(int i = 0; i < count; i++)
            {
                string categoryIdentifier = packet.PopString();
                string publicName = packet.PopString();
                int x = packet.PopInt();
                bool catOpen = packet.PopBoolean();
                int viewMode = packet.PopInt();

                int countRooms = packet.PopInt();
                Logger.Debug("Count:", countRooms, "CatIdentifier:", categoryIdentifier, "PublicName:", publicName, "Open?:", catOpen, "ViewMode:", viewMode);
                for (int z = 0; z < countRooms; z++)
                {
                    try
                    {
                        int roomId = packet.PopInt();
                        string name = packet.PopString();
                        int ownerId = packet.PopInt();
                        string ownerName = packet.PopString();
                        int acces = packet.PopInt();
                        int usersNow = packet.PopInt();
                        int usersMax = packet.PopInt();
                        string desc = packet.PopString();
                        int tradeSettings = packet.PopInt();
                        int score = packet.PopInt();
                        int topRated = packet.PopInt(); //Always 0
                        int category = packet.PopInt();
                        
                        int tagsCount = packet.PopInt();
                        List<string> tags = new List<string>();
                        for (int j = 0; j < tagsCount; j++)
                        {
                            string tag = packet.PopString();
                            tags.Add(tag);
                        }

                        int roomType = packet.PopInt();

                        bool img = packet.PopBoolean();
                        string image = "";
                        if (img)
                        {
                            image = packet.PopString();
                            Logger.Debug("image: ", image);
                        }

                        bool grp = packet.PopBoolean();
                        Group group = null;
                        if (grp)
                        {
                            int groupId = packet.PopInt();
                            string groupName = packet.PopString();
                            string badge = packet.PopString();
                            group = new Group(groupId, groupName, badge, roomId);
                        }

                        bool prmo = packet.PopBoolean();
                        RoomEvent roomEvent = null;
                        if (prmo)
                        {
                            string promoName = packet.PopString();
                            string promoDesc = packet.PopString();
                            int minLeft = packet.PopInt();
                            roomEvent = new RoomEvent(promoName, promoDesc, minLeft);
                        }

                        session.AddRoom(searchCode, categoryIdentifier, new RoomData(roomId, name, ownerId, ownerName, acces, usersNow, usersMax, desc, tradeSettings, score, topRated, category, tags, roomType, image, group, roomEvent));
                    }catch
                    {
                        Logger.Error("Error on parsing a room....");
                    }
                }
            }
        }
    }
}
