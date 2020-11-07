using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.Hotel.GameClients;
using Etap.Utilities;
using System.Collections.Generic;

namespace Etap.Communication.Packets.Incoming.Navigator
{
	class UserFlatCatsComposer : IPacketEvent
    {
        /*public UserFlatCatsComposer(ICollection<SearchResultList> Categories, int Rank)
            : base(ServerPacketHeader.UserFlatCatsMessageComposer)
        {
			WriteInteger(Categories.Count);
            foreach (SearchResultList Cat in Categories)
            {
				WriteInteger(Cat.Id);
				WriteString(Cat.PublicName);
				WriteBoolean(Cat.RequiredRank <= Rank);
				WriteBoolean(false);
				WriteString("");
				WriteString("");
				WriteBoolean(false);
            }
        }*/

        public void Parse(GameClient session, ClientPacket packet)
        {
            int count = packet.PopInt();

            for(int i = 0; i < count; i++)
            {
                int catId = packet.PopInt();
                string catPublicName = packet.PopString();
                bool hasRequiredRank = packet.PopBoolean();
                bool x = packet.PopBoolean();
                string x1 = packet.PopString();
                string x2 = packet.PopString();
                bool x3 = packet.PopBoolean();

                Logger.Debug(catId, "-", catPublicName, "-", hasRequiredRank);
                session.SendPacket(new NavigatorSearchEvent(catPublicName, "MY_ROOMS"));
            }
        }
    }
}