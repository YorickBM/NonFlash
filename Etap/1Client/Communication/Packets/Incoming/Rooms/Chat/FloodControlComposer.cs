using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Etap.Communication.Packets.Incoming.Rooms.Chat
{
    public class FloodControlComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int floodTime = Packet.PopInt();
        }
    }
}