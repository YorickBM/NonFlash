using Retro.Communication.Packets.Outgoing;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Rooms.Connection
{
    public class OpenFlatConnectionEvent : ServerPacket
    {
        public OpenFlatConnectionEvent() : base (ServerPacketHeader.OpenFlatConnectionMessageEvent)
        {

        }
    }
}