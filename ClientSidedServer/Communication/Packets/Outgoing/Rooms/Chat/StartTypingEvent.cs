using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Rooms.Chat
{
    public class StartTypingEvent : ServerPacket
    {
        public StartTypingEvent() : base (ServerPacketHeader.StartTypingMessageEvent)
        {

        }
    }
}