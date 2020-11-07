using System;
using System.Data;
using System.Collections.Generic;
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Moderation;
using Retro.Database.Interfaces;
using Retro.Hotel.Users;
using Retro.Hotel.Rooms.Chat.Logs;

namespace Retro.Communication.Packets.Incoming.Moderation
{
    class GetModeratorRoomChatlogEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int Junk = Packet.PopInt();
            int RoomId = Packet.PopInt();

            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(RoomId, out Room))
            {
                return;
            }

            RetroEnvironment.GetGame().GetChatManager().GetLogs().FlushAndSave();

            List<ChatlogEntry> Chats = new List<ChatlogEntry>();

            DataTable Data = null;
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `chatlogs` WHERE `room_id` = @id ORDER BY `id` DESC LIMIT 100");
                dbClient.AddParameter("id", RoomId);
                Data = dbClient.getTable();

                if (Data != null)
                {
                    foreach (DataRow Row in Data.Rows)
                    {
                        Habbo Habbo = RetroEnvironment.GetHabboById(Convert.ToInt32(Row["user_id"]));

                        if (Habbo != null)
                        {
                            Chats.Add(new ChatlogEntry(Convert.ToInt32(Row["user_id"]), RoomId, Convert.ToString(Row["message"]), Convert.ToDouble(Row["timestamp"]), Habbo));
                        }
                    }
                }
            }

            Session.SendMessage(new ModeratorRoomChatlogComposer(Room, Chats));
        }
    }
}