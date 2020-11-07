
using Retro.Hotel.GameClients;
using log4net;
using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Handshake
{
    public class SSOTicketEvent : ServerPacket
    {
        public SSOTicketEvent(string AuthTicket) : base(ServerPacketHeader.SSOTicketMessageEvent)
        {
            base.WriteString(AuthTicket);
        }
    }
}