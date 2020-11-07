using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    public class ShoutEvent : ServerPacket
    {
        public ShoutEvent() : base (ServerPacketHeader.ShoutMessageEvent)
        {
        }
    }
}