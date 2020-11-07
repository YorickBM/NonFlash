using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    public class CancelTypingEvent : ServerPacket
    {
        public CancelTypingEvent() : base(ServerPacketHeader.CancelTypingMessageEvent)
        {

        }
    }
}