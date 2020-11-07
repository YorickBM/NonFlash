
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Handshake;
using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Handshake
{
    public class UniqueIDEvent : ServerPacket
    {
        public UniqueIDEvent() : base(ServerPacketHeader.UniqueIDMessageEvent)
        {
            base.WriteString("Junk");
            base.WriteString("~4287364c2c9bd80c491cdaa66696137a");
        }
    }
}