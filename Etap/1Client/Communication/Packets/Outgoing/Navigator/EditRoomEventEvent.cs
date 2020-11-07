using System;
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Engine;
using Retro.Database.Interfaces;


namespace Retro.Communication.Packets.Incoming.Navigator
{
    class EditRoomEventEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int RoomId = Packet.PopInt();
            string word;
            string Name = Packet.PopString();
            Name = RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Desc = Packet.PopString();
            Desc = RetroEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Desc, out word) ? "Spam" : Desc;

            RoomData Data = RetroEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Data == null)
                return;

            if (Data.OwnerId != Session.GetHabbo().Id)
                return;//HAX

            if (Data.Promotion == null)
            {
                Session.SendNotification("Oops, it looks like there's not a promotion in this room! ");
                return;
            }

            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `room_promotions` SET `title` = @title, `description` = @desc WHERE `room_id` = " + RoomId + " LIMIT 1");
                dbClient.AddParameter("title", Name);
                dbClient.AddParameter("desc", Desc);
                dbClient.RunQuery();
            }

            Room Room;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Convert.ToInt32(RoomId), out Room))
                return;

            Data.Promotion.Name = Name;
            Data.Promotion.Description = Desc;
            Room.SendMessage(new RoomEventComposer(Data, Data.Promotion));
        }
    }
}
