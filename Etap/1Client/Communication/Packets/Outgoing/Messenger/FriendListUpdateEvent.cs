namespace Etap.Communication.Packets.Outgoing.Messenger
{
    class FriendListUpdateEvent : ServerPacket
    {
        public FriendListUpdateEvent() : base(ServerPacketHeader.FriendListUpdateMessageEvent)
        {
            
        }
    }
}
