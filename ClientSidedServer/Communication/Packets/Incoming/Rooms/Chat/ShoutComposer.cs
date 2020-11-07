using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Rooms.Chat
{
    public class ShoutComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int virtualId = Packet.PopInt();
            string message = Packet.PopString();
            int emotion = Packet.PopInt();
            int colour = Packet.PopInt();

            int x0 = Packet.PopInt();
            int x1 = Packet.PopInt();
        }
    }
}