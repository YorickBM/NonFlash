using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Rooms.Connection
{
    public class GoToFlatEvent : ServerPacket
    {
        public GoToFlatEvent() : base(ServerPacketHeader.GoToFlatMessageEvent)
        {

        }
    }
}
