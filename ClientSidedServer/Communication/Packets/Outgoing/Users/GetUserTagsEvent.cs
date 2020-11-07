
using Retro.Communication.Packets.Outgoing.Users;

namespace Retro.Communication.Packets.Incoming.Users
{
	class GetUserTagsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();

            Session.SendMessage(new UserTagsComposer(Session, UserId));
        }
    }
}
