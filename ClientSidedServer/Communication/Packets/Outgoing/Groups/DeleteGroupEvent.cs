using System;
using Retro.Database.Interfaces;
using Retro.Hotel.Groups;
using Retro.Hotel.Rooms;
using Retro.Communication.Packets.Outgoing.Messenger;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class DeleteGroupEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(Packet.PopInt(), out Group))
            {
                Session.SendNotification("Oops! We konden de groep niet vinden!");
                return;
            }

            if (Group.CreatorId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("group_delete_override"))//Maybe a FUSE check for staff override?
            {
                Session.SendNotification("Oops! alleen de eigenaar kan het verwijderen!");
                return;
            }

            if (Group.MemberCount >= Convert.ToInt32(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("group.delete.member.limit")) && !Session.GetHabbo().GetPermissions().HasRight("group_delete_limit_override"))
            {
                Session.SendNotification("Ops je groep heeft het maximum aantal leden al overschreden! (" + Convert.ToInt32(RetroEnvironment.GetGame().GetSettingsManager().TryGetValue("group.delete.member.limit")) + ") dat een groep kan overschrijden voordat ze in aanmerking komen voor eliminatie. Vraag om hulp van een medewerker.");
                return;
            }

            Room Room = RetroEnvironment.GetGame().GetRoomManager().LoadRoom(Group.RoomId);

            if (Room != null)
            {
                Room.Group = null;
                Room.RoomData.Group = null;//I'm not sure if this is needed or not, becauseof inheritance, but oh well.
            }

            //Remove it from the cache.
            RetroEnvironment.GetGame().GetGroupManager().DeleteGroup(Group.Id);

            //Now the :S stuff.
            using (IQueryAdapter dbClient = RetroEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("DELETE FROM `groups` WHERE `id` = '" + Group.Id + "'");
                dbClient.runFastQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + Group.Id + "'");
                dbClient.runFastQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + Group.Id + "'");
                dbClient.runFastQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + Group.Id + "' LIMIT 1");
                dbClient.runFastQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + Group.Id + "' LIMIT 1");
                dbClient.runFastQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + Group.Id + "'");
            }

            bool forumEnabled = Group.ForumEnabled;
            if (forumEnabled)
            {
                RetroEnvironment.GetGame().GetGroupForumManager().RemoveGroup(Group);
                return;
            }

            //Unload it last.
            RetroEnvironment.GetGame().GetRoomManager().UnloadRoom(Room.Id);

            var Client = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
            if (Client != null)
            {
                Client.SendMessage(new FriendListUpdateComposer(Group, -1));
                Client.SendMessage(new FriendListUpdateComposer(-Group.Id));
            }

            //Say hey!
            Session.SendNotification("U heeft de groep succesvol verwijderd.");
        }
    }
}
