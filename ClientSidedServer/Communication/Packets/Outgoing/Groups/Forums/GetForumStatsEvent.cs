using Retro.Communication.Packets.Outgoing.Groups;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.GameClients;
using Retro.Hotel.Groups.Forums;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var GroupForumId = Packet.PopInt();

            GroupForum Forum;
            if (!RetroEnvironment.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out Forum))
            {
                RetroEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("forums_thread_hidden", "Het forum dat u probeert te openen bestaat niet meer.", ""));
                return;
            }

            Session.SendMessage(new ForumDataComposer(Forum, Session));

        }
    }
}
