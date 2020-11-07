using Retro.Hotel.Users;
using Retro.Hotel.Rooms;
using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Groups;
using Retro.Communication.Packets.Outgoing.Rooms.Permissions;



namespace Retro.Communication.Packets.Incoming.Groups
{
    class GiveAdminRightsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId || !Group.IsMember(UserId))
                return;

            Habbo Habbo = RetroEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Er is een fout opgetreden bij het vinden van deze gebruiker!");
                return;
            }

            Group.MakeAdmin(UserId);
          
            Room Room = null;
            if (RetroEnvironment.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (!User.Statusses.ContainsKey("flatctrl 3"))
                        User.SetStatus("flatctrl 3", "");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(new YouAreControllerComposer(3));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 1));
        }
    }
}
