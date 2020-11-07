using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Moderation
{
    class CloseTicketEvent : ServerPacket
    {
        public CloseTicketEvent(int lvl, int TicketId) : base(ServerPacketHeader.CloseTicketMesageEvent)
        {
            base.WriteInteger(lvl);
            base.WriteInteger(0);
            base.WriteInteger(TicketId);
        }
    }
}