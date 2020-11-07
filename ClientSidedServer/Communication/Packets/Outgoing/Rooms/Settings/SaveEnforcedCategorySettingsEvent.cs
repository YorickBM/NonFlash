using Retro.Hotel.Rooms;
using Retro.Hotel.Navigator;

namespace Retro.Communication.Packets.Incoming.Rooms.Settings
{
	class SaveEnforcedCategorySettingsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Packet.PopInt(), out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            int CategoryId = Packet.PopInt();
            int TradeSettings = Packet.PopInt();

            if (TradeSettings < 0 || TradeSettings > 2)
                TradeSettings = 0;

            SearchResultList SearchResultList = null;
            if (!RetroEnvironment.GetGame().GetNavigator().TryGetSearchResultList(CategoryId, out SearchResultList))
            {
                CategoryId = 36;
            }

            if (SearchResultList.CategoryType != NavigatorCategoryType.CATEGORY || SearchResultList.RequiredRank > Session.GetHabbo().Rank)
            {
                CategoryId = 36;
            }
        }
    }
}
