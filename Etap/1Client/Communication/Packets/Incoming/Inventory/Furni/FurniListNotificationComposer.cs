using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Inventory.Furni
{
    class FurniListNotificationComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int x0 = Packet.PopInt();
            int type = Packet.PopInt();
            int x1 = Packet.PopInt();
            int id = Packet.PopInt();
        }
    }
}