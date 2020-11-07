using System.Linq;
using System.Collections.Generic;

using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Groups;
using Retro.Hotel.Cache.Type;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class GetGroupMembersEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int Page = Packet.PopInt();
            string SearchVal = Packet.PopString();
            int RequestType = Packet.PopInt();

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            List<UserCache> Members = new List<UserCache>();

            switch (RequestType)
            {
                case 0:
                    {
                        List<int> MemberIds = Group.GetAllMembers;
                        foreach (int Id in MemberIds.ToList())
                        {
                            UserCache GroupMember = RetroEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                            if (GroupMember == null)
                                continue;

                            if (!Members.Contains(GroupMember))
                                Members.Add(GroupMember);
                        }
                        break;
                    }

                case 1:
                    {
                        List<int> AdminIds = Group.GetAdministrators;
                        foreach (int Id in AdminIds.ToList())
                        {
                            UserCache GroupMember = RetroEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                            if (GroupMember == null)
                                continue;

                            if (!Members.Contains(GroupMember))
                                Members.Add(GroupMember);
                        }
                        break;
                    }

                case 2:
                    {
                        List<int> RequestIds = Group.GetRequests;
                        foreach (int Id in RequestIds.ToList())
                        {
                            UserCache GroupMember = RetroEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                            if (GroupMember == null)
                                continue;

                            if (!Members.Contains(GroupMember))
                                Members.Add(GroupMember);
                        }
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(SearchVal))
                Members = Members.Where(x => x.Username.StartsWith(SearchVal)).ToList();

            int StartIndex = ((Page - 1) * 14 + 14);
            int FinishIndex = Members.Count;

            Session.SendMessage(new GroupMembersComposer(Group, Members.Skip(StartIndex).Take(FinishIndex - StartIndex).ToList(), Members.Count, Page, (Group.CreatorId == Session.GetHabbo().Id || Group.IsAdmin(Session.GetHabbo().Id)), RequestType, SearchVal));
        }
    }
}