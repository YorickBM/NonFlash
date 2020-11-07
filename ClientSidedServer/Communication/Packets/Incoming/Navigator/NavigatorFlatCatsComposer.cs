using Retro.Hotel.Navigator;
using System.Collections.Generic;
using System.Linq;

namespace Retro.Communication.Packets.Outgoing.Navigator
{
	class NavigatorFlatCatsComposer : ServerPacket
    {
        public NavigatorFlatCatsComposer(ICollection<SearchResultList> Categories, int Rank)
            : base(ServerPacketHeader.NavigatorFlatCatsMessageComposer)
        {
			WriteInteger(Categories.Count);
            foreach (SearchResultList Category in Categories.ToList())
            {
				WriteInteger(Category.Id);
				WriteString(Category.PublicName);
				WriteBoolean(true);//TODO
            }
        }
    }
}