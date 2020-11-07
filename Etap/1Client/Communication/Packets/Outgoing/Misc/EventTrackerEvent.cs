namespace Etap.Communication.Packets.Outgoing.Misc
{
    class EventTrackerEvent : ServerPacket
    {
        public EventTrackerEvent() : base(ServerPacketHeader.EventTrackerMessageEvent)
        {

        }
    }
}
