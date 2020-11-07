using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Rooms.Permissions;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;
using Retro.Database.Interfaces;
using Retro.Hotel.Cache.Type;

namespace Retro.Communication.Packets.Incoming.Rooms.Action
{
    class AssignRightsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            int UserId = Packet.PopInt();

            Room Room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (Room.UsersWithRights.Contains(UserId))
            {
                Session.SendNotification(RetroEnvironment.GetGame().GetLanguageManager().TryGetValue("room.rights.user.has_rights"));
                return;
            }

            Room.UsersWithRights.Add(UserId);

            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("INSERT INTO `room_rights` (`room_id`,`user_id`) VALUES ('" + Room.RoomId + "','" + UserId + "')");
            }

            RoomUser RoomUser = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
            if (RoomUser != null && !RoomUser.IsBot)
            {
                RoomUser.SetStatus("flatctrl 1", "");
                RoomUser.UpdateNeeded = true;
                if (RoomUser.GetClient() != null)
                    RoomUser.GetClient().SendMessage(new YouAreControllerComposer(1));

                Session.SendMessage(new FlatControllerAddedComposer(Room.RoomId, RoomUser.GetClient().GetHabbo().Id, RoomUser.GetClient().GetHabbo().Username));
            }
            else
            {
                UserCache User = RetroEnvironment.GetGame().GetCacheManager().GenerateUser(UserId);
                if (User != null)
                    Session.SendMessage(new FlatControllerAddedComposer(Room.RoomId, User.Id, User.Username));
            }
        }
    }
}
