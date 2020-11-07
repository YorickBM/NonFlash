using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using Retro.Hotel.Rooms;
using Retro.Hotel.Users.Inventory.Bots;
using Retro.Communication.Packets.Outgoing.Inventory.Bots;
using Retro.Database.Interfaces;
using Retro.Hotel.Rooms.AI.Speech;
using Retro.Hotel.Rooms.AI;


namespace Retro.Communication.Packets.Incoming.Rooms.AI.Bots
{
    class PlaceBotEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            int BotId = Packet.PopInt();
            int X = Packet.PopInt();
            int Y = Packet.PopInt();

            if (!Room.GetGameMap().CanWalk(X, Y, false) || !Room.GetGameMap().ValidTile(X, Y))
            {
                Session.SendNotification("You can not put a bot here!");
                return;
            }

            Bot Bot = null;
            if (!Session.GetHabbo().GetInventoryComponent().TryGetBot(BotId, out Bot))
                return;

            int BotCount = 0;
            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.IsPet || !User.IsBot)
                    continue;

                BotCount += 1;
            }

            if (BotCount >= 25 && !Session.GetHabbo().GetPermissions().HasRight("bot_place_any_override"))
            {
                Session.SendNotification("Sorry, but you can only put 25 bots in the room!");
                return;
            }

            //TODO: Hmm, maybe not????
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `bots` SET `room_id` = @roomId, `x` = @CoordX, `y` = @CoordY WHERE `id` = @BotId LIMIT 1");
                dbClient.AddParameter("roomId", Room.RoomId);
                dbClient.AddParameter("BotId", Bot.Id);
                dbClient.AddParameter("CoordX", X);
                dbClient.AddParameter("CoordY", Y);
                dbClient.RunQuery();
            }

            List<RandomSpeech> BotSpeechList = new List<RandomSpeech>();

            //TODO: Grab data?
            DataRow GetData = null;
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `ai_type`,`rotation`,`walk_mode`,`automatic_chat`,`speaking_interval`,`mix_sentences`,`chat_bubble` FROM `bots` WHERE `id` = @BotId LIMIT 1");
                dbClient.AddParameter("BotId", Bot.Id);
                GetData = dbClient.getRow();

                dbClient.SetQuery("SELECT * FROM `bots_speech` WHERE `bot_id` = @BotId ORDER BY id ASC");
                dbClient.AddParameter("BotId", Bot.Id);
                DataTable BotSpeech = dbClient.getTable();

                foreach (DataRow Speech in BotSpeech.Rows)
                {
                    BotSpeechList.Add(new RandomSpeech(Convert.ToString(Speech["text"]), Bot.Id));
                }
            }

            RoomUser BotUser = Room.GetRoomUserManager().DeployBot(new RoomBot(Bot.Id, Session.GetHabbo().CurrentRoomId, Convert.ToString(GetData["ai_type"]), Convert.ToString(GetData["walk_mode"]), Bot.Name, "", Bot.Figure, X, Y, 0, 4, 0, 0, 0, 0, ref BotSpeechList, "", 0, Bot.OwnerId, RetroEnvironment.EnumToBool(GetData["automatic_chat"].ToString()), Convert.ToInt32(GetData["speaking_interval"]), RetroEnvironment.EnumToBool(GetData["mix_sentences"].ToString()), Convert.ToInt32(GetData["chat_bubble"])), null);
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `bots` WHERE `id` = @BotId ");
                dbClient.AddParameter("BotId", Bot.Id);
                DataTable BotSpeechData = dbClient.getTable();
                foreach (DataRow SpeechData in BotSpeechData.Rows)
                {
                    BotUser.Chat("Heey!", false, (Convert.ToInt32(SpeechData["chat_bubble"])));
                }
             
            }

            Room.GetGameMap().UpdateUserMovement(new System.Drawing.Point(X,Y), new System.Drawing.Point(X, Y), BotUser);


            Bot ToRemove = null;
            if (!Session.GetHabbo().GetInventoryComponent().TryRemoveBot(BotId, out ToRemove))
            {
                Console.WriteLine("Error on removing bot: " + ToRemove.Id);
                return;
            }
            Session.SendMessage(new BotInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetBots()));
        }
    }
}
