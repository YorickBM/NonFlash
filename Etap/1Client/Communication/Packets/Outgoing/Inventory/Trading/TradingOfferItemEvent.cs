using Retro.Hotel.Rooms;
using Retro.Hotel.Items;
using Retro.Hotel.Rooms.Trading;

namespace Retro.Communication.Packets.Incoming.Inventory.Trading
{
    class TradingOfferItemEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CanTradeInRoom)
                return;

            Trade Trade = Room.GetUserTrade(Session.GetHabbo().Id);
            if (Trade == null)
                return;

            Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Packet.PopInt());
            if (Item == null)
                return;

            Trade.OfferItem(Session.GetHabbo().Id, Item);
        }
    }
}