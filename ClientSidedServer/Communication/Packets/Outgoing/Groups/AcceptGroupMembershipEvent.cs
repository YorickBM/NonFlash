
using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Groups;

using Retro.Communication.Packets.Outgoing.Messenger;
using Retro.Hotel.Users;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class AcceptGroupMembershipEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if ((Session.GetHabbo().Id != Group.CreatorId && !Group.IsAdmin(Session.GetHabbo().Id)) && !Session.GetHabbo().GetPermissions().HasRight("fuse_group_accept_any"))
                return;

            if (!Group.HasRequest(UserId))
                return;

            Habbo Habbo = RetroEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Er is een fout opgetreden bij het ophalen van deze gebruiker!");
                return;
            }

            Group.HandleRequest(UserId, true);

            if (Group.HasChat)
            {
                var Client = RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(Group, 1));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 4));
        }
    }
}