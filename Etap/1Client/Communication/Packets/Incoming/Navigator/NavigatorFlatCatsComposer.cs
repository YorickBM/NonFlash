using Etap.Hotel.GameClients;
using Etap.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Etap.Communication.Packets.Incoming.Navigator
{
	class NavigatorFlatCatsComposer : IPacketEvent
    {
        /*public NavigatorFlatCatsComposer(ICollection<SearchResultList> Categories, int Rank)
            : base(ServerPacketHeader.NavigatorFlatCatsMessageComposer)
        {
			WriteInteger(Categories.Count);
            foreach (SearchResultList Category in Categories.ToList())
            {
				WriteInteger(Category.Id);
				WriteString(Category.PublicName);
				WriteBoolean(true);//TODO
            }
        }*/

        public void Parse(GameClient Session, ClientPacket packet)
        {
            int count = packet.PopInt();

            for (int i = 0; i < count; i++)
            {
                int catId = packet.PopInt();
                string catPublicName = packet.PopString();
                bool hasRequiredRank = packet.PopBoolean(); //Always True

                Logger.Debug(catId, "-", catPublicName, "-", hasRequiredRank);
            }
        }
    }
}