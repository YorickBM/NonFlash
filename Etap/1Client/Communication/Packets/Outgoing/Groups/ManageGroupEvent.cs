using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Groups;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class ManageGroupEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Group.CreatorId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("group_management_override"))
                return;

            Session.SendMessage(new ManageGroupComposer(Group, Group.Badge.Replace("b", "").Split('s')));
        }
    }
}
