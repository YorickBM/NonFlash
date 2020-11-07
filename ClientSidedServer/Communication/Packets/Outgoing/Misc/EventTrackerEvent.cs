using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Misc
{
    class EventTrackerEvent : ServerPacket
    {
        public EventTrackerEvent() : base(ServerPacketHeader.EventTrackerMessageEvent)
        {

        }
    }
}
