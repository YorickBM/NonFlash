using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Rooms.Chat
{
    public class FloodControlComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int floodTime = Packet.PopInt();
        }
    }
}