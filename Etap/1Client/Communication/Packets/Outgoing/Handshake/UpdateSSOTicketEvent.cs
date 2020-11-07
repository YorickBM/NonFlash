
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Outgoing.Handshake;
using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    public class UpdateSSOTicketEvent : ServerPacket
    {
        public UpdateSSOTicketEvent(string ticket) : base(ServerPacketHeader.UpdateSSOTicketMessageEvent)
        {
            base.WriteString(ticket);
        }
    }
}