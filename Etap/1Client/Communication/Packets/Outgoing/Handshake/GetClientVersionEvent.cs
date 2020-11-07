using Etap.Communication.Packets.Outgoing;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : ServerPacket
    {
        public GetClientVersionEvent() : base(ServerPacketHeader.GetClientVersionMessageEvent)
        {
            base.WriteString("PRODUCTION-NonFlash-Q3QslPaKxHH841gLfQcA");
        }
    }
}