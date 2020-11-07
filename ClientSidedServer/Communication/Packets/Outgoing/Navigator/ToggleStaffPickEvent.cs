using Retro.Hotel.Navigator;
using Retro.Hotel.GameClients;
using Retro.Database.Interfaces;
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Navigator;
using Retro.Communication.Packets.Outgoing.Rooms.Settings;

namespace Retro.Communication.Packets.Incoming.Navigator
{
    class ToggleStaffPickEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            GameClient TargetClient = RetroEnvironment.GetGame().GetClientManager().GetClientByUsername(session.GetHabbo().CurrentRoom.OwnerName);
            if (!session.GetHabbo().GetPermissions().HasRight("room.staff_picks.management"))
            {
                session.SendWhisper("Hmmm, Je hebt niet de benodige permissie! Dus waarom heb je dan de knop :?");
                return;
            }

            Room room = null;
            if (!RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(packet.PopInt(), out room))
                return;

            StaffPick staffPick = null;
            if (!RetroEnvironment.GetGame().GetNavigator().TryGetStaffPickedRoom(room.Id, out staffPick))
            {
                if (RetroEnvironment.GetGame().GetNavigator().TryAddStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("INSERT INTO `navigator_staff_picks` (`room_id`,`image`) VALUES (@roomId, null)");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                    RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(TargetClient, "ACH_Spr", 1, false);
                }
            }
            else
            {
                if (RetroEnvironment.GetGame().GetNavigator().TryRemoveStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `navigator_staff_picks` WHERE `room_id` = @roomId LIMIT 1");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            room.SendMessage(new RoomSettingsSavedComposer(room.RoomId));
            room.SendMessage(new RoomInfoUpdatedComposer(room.RoomId));
        }
    }
}