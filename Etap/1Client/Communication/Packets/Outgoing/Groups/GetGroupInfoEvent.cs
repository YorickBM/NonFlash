using Retro.Hotel.Groups;
using Retro.Communication.Packets.Outgoing.Groups;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class GetGroupInfoEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            bool NewWindow = Packet.PopBoolean();

            Group Group = null;
            if (!RetroEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            Session.SendMessage(new GroupInfoComposer(Group, Session, NewWindow));     
        }
    }
}
