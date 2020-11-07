using System;
using Retro.Hotel.Rooms;
using Retro.Hotel.Users.Inventory.Bots;
using Retro.Communication.Packets.Outgoing.Inventory.Bots;
using Retro.Database.Interfaces;

namespace Retro.Communication.Packets.Incoming.Rooms.AI.Bots
{
    class PickUpBotEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            

            if (!Session.GetHabbo().InRoom)
                return;

            int BotId = Packet.PopInt();
            if (BotId == 0)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser BotUser = null;
            if (!Room.GetRoomUserManager().TryGetBot(BotId, out BotUser))
                return;

            if (Session.GetHabbo().Id != BotUser.BotData.ownerID && !Session.GetHabbo().GetPermissions().HasRight("bot_place_any_override"))
            {
                Session.SendWhisper("You can only pick your own bots!");
                return;
            }

            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `bots` SET `room_id` = '0' WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("id", BotId);
                dbClient.RunQuery();
            }


            Room.GetGameMap().RemoveUserFromMap(BotUser, new System.Drawing.Point(BotUser.X, BotUser.Y));

            Session.GetHabbo().GetInventoryComponent().TryAddBot(new Bot(Convert.ToInt32(BotUser.BotData.Id), Convert.ToInt32(BotUser.BotData.ownerID), BotUser.BotData.Name, BotUser.BotData.Motto, BotUser.BotData.Look, BotUser.BotData.Gender));
            Session.SendMessage(new BotInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetBots()));
            Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
        }
    }
}
