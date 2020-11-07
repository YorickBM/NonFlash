using Retro.Communication.Packets.Outgoing;
using Retro.Communication.Packets.Outgoing.Misc;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Misc
{
    class LatencyTestEvent : ServerPacket
    {
        public LatencyTestEvent(GameClient Session, int delay) : base(ServerPacketHeader.LatencyTestMessageEvent)
        {
            base.WriteInteger(delay);
        }
    }
}
