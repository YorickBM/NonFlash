
using Retro.Hotel.GameClients;
using Retro.Communication.Packets.Outgoing.Handshake;
using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : ServerPacket
    {
        public InfoRetrieveEvent() : base(ServerPacketHeader.InfoRetrieveMessageEvent)
        {
        }
    }
}