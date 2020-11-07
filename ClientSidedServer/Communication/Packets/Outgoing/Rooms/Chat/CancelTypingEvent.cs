using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Rooms.Chat
{
    public class CancelTypingEvent : ServerPacket
    {
        public CancelTypingEvent() : base(ServerPacketHeader.CancelTypingMessageEvent)
        {

        }
    }
}