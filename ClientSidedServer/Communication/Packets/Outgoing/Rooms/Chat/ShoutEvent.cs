using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Rooms.Chat
{
    public class ShoutEvent : ServerPacket
    {
        public ShoutEvent() : base (ServerPacketHeader.ShoutMessageEvent)
        {
        }
    }
}