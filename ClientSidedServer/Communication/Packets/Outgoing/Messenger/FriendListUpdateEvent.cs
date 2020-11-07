using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Messenger
{
    class FriendListUpdateEvent : ServerPacket
    {
        public FriendListUpdateEvent() : base(ServerPacketHeader.FriendListUpdateMessageEvent)
        {
            
        }
    }
}
