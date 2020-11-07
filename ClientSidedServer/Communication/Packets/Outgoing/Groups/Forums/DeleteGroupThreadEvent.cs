using Retro.Communication.Packets.Outgoing.Groups;
using Retro.Communication.Packets.Outgoing.Rooms.Notifications;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class DeleteGroupThreadEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var int1 = Packet.PopInt();
            var int2 = Packet.PopInt();
            var int3 = Packet.PopInt();

            var forum = RetroEnvironment.GetGame().GetGroupForumManager().GetForum(int1);

            if (forum == null)
            {
                Session.SendNotification("Het forum bestaat niet!");
                return;
            }

            if (forum.Settings.GetReasonForNot(Session, forum.Settings.WhoCanModerate) != "")
            {
                Session.SendNotification("U hebt niet het recht om dit forum te verwijderen!");
                return;
            }

            var thread = forum.GetThread(int2);
            if (thread == null)
            {
                Session.SendNotification("Thema bestaat niet!");
                return;
            }

            thread.DeletedLevel = int3 / 10;

            thread.DeleterUserId = thread.DeletedLevel != 0 ? Session.GetHabbo().Id : 0;

            thread.Save();

            Session.SendMessage(new ThreadsListDataComposer(forum, Session));

            if (thread.DeletedLevel != 0)
                Session.SendMessage(new RoomNotificationComposer("forums.thread.hidden"));
            else
                Session.SendMessage(new RoomNotificationComposer("forums.thread.restored"));
        }
    }
}
