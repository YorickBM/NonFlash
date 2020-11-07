using Retro.Hotel.GameClients;
using Retro.Hotel.Groups.Forums;

namespace Retro.Communication.Packets.Outgoing.Groups
{
	class ThreadUpdatedComposer : ServerPacket
    {
        public ThreadUpdatedComposer(GameClient Session, GroupForumThread Thread)
            : base(ServerPacketHeader.ThreadUpdatedMessageComposer)
        {
			WriteInteger(Thread.ParentForum.Id);

            Thread.SerializeData(Session, this);
        }
    }
}
