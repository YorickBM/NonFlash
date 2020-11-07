using System.Linq;
using System.Collections.Generic;
using Retro.Hotel.Rooms;
using Retro.Hotel.Users;
using Retro.Communication.Packets.Outgoing.Users;
using Retro.Communication.Packets.Outgoing.Navigator;
using Retro.Communication.Packets.Outgoing.Rooms.Engine;
using Retro.Database.Interfaces;
using Retro.Communication.Packets.Outgoing.Rooms.Session;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using System;
using System.Data;

namespace Retro.Communication.Packets.Incoming.Users
{
    class ChangeNameEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (User == null)
                return;

            string NewName = Packet.PopString();
            string OldName = Session.GetHabbo().Username;

            if (NewName == OldName)
            {
                Session.GetHabbo().ChangeName(OldName);
                Session.SendMessage(new UpdateUsernameComposer(NewName));
                return;
            }

            if (!CanChangeName(Session.GetHabbo()))
            {
                Session.SendNotification("Well, it looks like currently you can not change your username!");
                return;
            }

            if (Session.GetHabbo().Rank > 0)
            {
                DataRow presothiago = null;
                using (var dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Presidio FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                    presothiago = dbClient.getRow();
                }

                if (Convert.ToBoolean(presothiago["Presidio"]) == true)
                {
                    if (Session.GetHabbo().Rank > 0)
                    {
                        string thiago = Session.GetHabbo().Look;
                        Session.SendMessage(new RoomNotificationComposer("police_announcement", "message", "You're stuck and you can not change your name."));
                        return;
                    }
                }
            }

            bool InUse = false;
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `users` WHERE `username` = @name LIMIT 1");
                dbClient.AddParameter("name", NewName);
                InUse = dbClient.getInteger() == 1;
            }

            char[] Letters = NewName.ToLower().ToCharArray();
            string AllowedCharacters = "abcdefghijklmnopqrstuvwxyz.,_-;:?!1234567890";

            foreach (char Chr in Letters)
            {
                if (!AllowedCharacters.Contains(Chr))
                {
                    return;
                }
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool") && NewName.ToLower().Contains("mod") || NewName.ToLower().Contains("adm") || NewName.ToLower().Contains("admin")
                || NewName.ToLower().Contains("m0d") || NewName.ToLower().Contains("mob") || NewName.ToLower().Contains("m0b"))
                return;
            else if (!NewName.ToLower().Contains("mod") && (Session.GetHabbo().Rank == 2 || Session.GetHabbo().Rank == 3))
                return;
            else if (NewName.Length > 15)
                return;
            else if (NewName.Length < 3)
                return;
            else if (InUse)
                return;
            else
            {
                if (!RetroEnvironment.GetGame().GetClientManager().UpdateClientUsername(Session, OldName, NewName))
                {
                    Session.SendNotification("Oops! There was a problem updating your username.");
                    return;
                }

                Session.GetHabbo().ChangingName = false;

                Room.GetRoomUserManager().RemoveUserFromRoom(Session, true, false);

                Session.GetHabbo().ChangeName(NewName);
                Session.GetHabbo().GetMessenger().OnStatusChanged(true);

                Session.SendMessage(new UpdateUsernameComposer(NewName));
                Room.SendMessage(new UserNameChangeComposer(Room.Id, User.VirtualId, NewName));

                using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `logs_client_namechange` (`user_id`,`new_name`,`old_name`,`timestamp`) VALUES ('" + Session.GetHabbo().Id + "', @name, '" + OldName + "', '" + RetroEnvironment.GetUnixTimestamp() + "')");
                    dbClient.AddParameter("name", NewName);
                    dbClient.RunQuery();
                }

                ICollection<RoomData> Rooms = Session.GetHabbo().UsersRooms;
                foreach (RoomData Data in Rooms)
                {
                    if (Data == null)
                        continue;

                    Data.OwnerName = NewName;
                }

                foreach (Room UserRoom in RetroEnvironment.GetGame().GetRoomManager().GetRooms().ToList())
                {
                    if (UserRoom == null || UserRoom.RoomData.OwnerName != NewName)
                        continue;

                    UserRoom.OwnerName = NewName;
                    UserRoom.RoomData.OwnerName = NewName;

                    UserRoom.SendMessage(new RoomInfoUpdatedComposer(UserRoom.RoomId));
                }

                RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_Name", 1);

               Session.SendMessage(new RoomForwardComposer(Room.Id));
            }
        }

        private static bool CanChangeName(Habbo Habbo)
        {

            if (Habbo.Rank == 1 && Habbo.VIPRank == 0 && Habbo.LastNameChange == 0)
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 1 && (Habbo.LastNameChange == 0 || (RetroEnvironment.GetUnixTimestamp() + 604800) > Habbo.LastNameChange))
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 2 && (Habbo.LastNameChange == 0 || (RetroEnvironment.GetUnixTimestamp() + 86400) > Habbo.LastNameChange))
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 3)
                return true;
            else if (Habbo.GetPermissions().HasRight("mod_tool"))
                return true;

            return false;
        }
    }
}