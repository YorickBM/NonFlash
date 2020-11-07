
using Etap.Hotel.GameClients;
using log4net;
using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    public class SSOTicketEvent : ServerPacket
    {
        public SSOTicketEvent(string AuthTicket) : base(ServerPacketHeader.SSOTicketMessageEvent)
        {
            base.WriteString(AuthTicket);
        }
    }
}