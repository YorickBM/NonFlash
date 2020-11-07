using Etap.Communication.Packets.Outgoing;
using Etap.Communication.Packets.Outgoing.Misc;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Misc
{
    class LatencyTestEvent : ServerPacket
    {
        public LatencyTestEvent(GameClient Session, int delay) : base(ServerPacketHeader.LatencyTestMessageEvent)
        {
            base.WriteInteger(delay);
        }
    }
}
