using Retro.Communication.Packets.Outgoing;
using Retro.Hotel.GameClients;


namespace Retro.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : ServerPacket
    {
        public GetClientVersionEvent() : base(ServerPacketHeader.GetClientVersionMessageEvent)
        {
            base.WriteString("PRODUCTION-NonFlash-Q3QslPaKxHH841gLfQcA");
        }
    }
}