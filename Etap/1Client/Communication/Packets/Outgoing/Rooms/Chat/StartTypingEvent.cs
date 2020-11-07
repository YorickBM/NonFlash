using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    public class StartTypingEvent : ServerPacket
    {
        public StartTypingEvent() : base (ServerPacketHeader.StartTypingMessageEvent)
        {

        }
    }
}