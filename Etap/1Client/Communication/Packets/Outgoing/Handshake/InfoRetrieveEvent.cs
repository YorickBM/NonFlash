
using Etap.Hotel.GameClients;
using Etap.Communication.Packets.Outgoing.Handshake;
using Etap.Communication.Packets.Outgoing;

namespace Etap.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : ServerPacket
    {
        public InfoRetrieveEvent() : base(ServerPacketHeader.InfoRetrieveMessageEvent)
        {
        }
    }
}