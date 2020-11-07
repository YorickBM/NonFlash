using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Rooms.Connection
{
    public class GoToFlatEvent : ServerPacket
    {
        public GoToFlatEvent() : base(ServerPacketHeader.GoToFlatMessageEvent)
        {

        }
    }
}
