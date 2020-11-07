using Etap.Communication.Packets.Outgoing;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Rooms.Connection
{
    public class OpenFlatConnectionEvent : ServerPacket
    {
        public OpenFlatConnectionEvent() : base (ClientPacketHeader.OpenFlatConnectionMessageComposer)
        {

        }
    }
}