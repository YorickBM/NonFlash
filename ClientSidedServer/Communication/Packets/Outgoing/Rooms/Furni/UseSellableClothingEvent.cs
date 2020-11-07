
using Retro.Hotel.Items;
using Retro.Hotel.Rooms;
using Retro.Hotel.Catalog.Clothing;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Retro.Database.Interfaces;

namespace Retro.Communication.Packets.Incoming.Rooms.Furni
{
	class UseSellableClothingEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            int ItemId = Packet.PopInt();

            Item Item = Room.GetRoomItemHandler().GetItem(ItemId);
            if (Item == null)
                return;

            if (Item.Data == null)
                return;

            if (Item.UserID != Session.GetHabbo().Id)
                return;

            if (Item.Data.InteractionType != InteractionType.PURCHASABLE_CLOTHING)
            {
                Session.SendNotification("Oops, it got bad to and on, call a staff!");
                return;
            }

            if (Item.Data.BehaviourData == 0)
            {
                Session.SendNotification("Oops, this article has no clothing settings, please report this!");
                return;
            }

            ClothingItem Clothing = null;
            if (!RetroEnvironment.GetGame().GetCatalog().GetClothingManager().TryGetClothing(Item.Data.ClothingId, out Clothing))
            {
                Session.SendNotification("Oops! this piece of clothing was not found!");
                return;
            }

            //Quickly delete it from the database.
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @ItemId LIMIT 1");
                dbClient.AddParameter("ItemId", Item.Id);
                dbClient.RunQuery();
            }

            //Remove the item.
            Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);

            Session.GetHabbo().GetClothing().AddClothing(Clothing.ClothingName, Clothing.PartIds);
            Session.SendMessage(new FigureSetIdsComposer(Session.GetHabbo().GetClothing().GetClothingParts));
            Session.SendMessage(new RoomNotificationComposer("figureset.redeemed.success"));
            Session.SendWhisper("For some reason you can not see your new clothes, try again!");
        }
    }
}
