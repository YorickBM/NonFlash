using Retro.Communication.Packets.Outgoing.Groups;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Groups
{
    class PostGroupContentEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var ForumId = Packet.PopInt();
            var ThreadId = Packet.PopInt();
            var Caption = Packet.PopString();
            var Message = Packet.PopString();

            var Forum = RetroEnvironment.GetGame().GetGroupForumManager().GetForum(ForumId);
            if (Forum == null)
            {
                Session.SendNotification("Oops! Dit forum bestaat niet!");
                return;
            }

            var IsNewThread = ThreadId == 0;
            if (IsNewThread)
            {
                var Thread = Forum.CreateThread(Session.GetHabbo().Id, Caption);
                var Post = Thread.CreatePost(Session.GetHabbo().Id, Message);

                Session.SendMessage(new ThreadCreatedComposer(Session, Thread));
                //Session.SendMessage(new PostUpdatedComposer(Session, Post));
                //Session.SendMessage(new ThreadReplyComposer(Session, Post));

            }
            else
            {
                var Thread = Forum.GetThread(ThreadId);
                if (Thread == null)
                {
                    Session.SendNotification("Oops! Het onderwerp van dit forum bestaat niet!");
                    return;
                }

                var Post = Thread.CreatePost(Session.GetHabbo().Id, Message);
                Session.SendMessage(new ThreadReplyComposer(Session, Post));
            }


        }
    }
}
