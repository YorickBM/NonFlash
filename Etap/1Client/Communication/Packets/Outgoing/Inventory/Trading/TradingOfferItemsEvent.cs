using System.Linq;
using System.Collections.Generic;

using Retro.Hotel.Rooms;
using Retro.Hotel.Items;
using Retro.Hotel.Rooms.Trading;

namespace Retro.Communication.Packets.Incoming.Inventory.Trading
{
    class TradingOfferItemsEvent : IPacketEvent
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

            int Amount = Packet.PopInt();

            Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Packet.PopInt());
            if (Item == null)
                return;

            List<Item> AllItems = Session.GetHabbo().GetInventoryComponent().GetItems.Where(x => x.Data.Id == Item.Data.Id).Take(Amount).ToList();
            foreach (Item I in AllItems)
            {
                Trade.OfferItem(Session.GetHabbo().Id, I);
            }
        }
    }
}