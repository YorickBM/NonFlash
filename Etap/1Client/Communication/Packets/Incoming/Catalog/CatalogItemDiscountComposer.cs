using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Retro.Communication.Packets.Outgoing.Catalog
{
	class CatalogItemDiscountComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int max = Packet.PopInt();

            int x0 = Packet.PopInt();
            int x1 = Packet.PopInt();
            int x2 = Packet.PopInt();

            int count = Packet.PopInt();

            int x3 = Packet.PopInt();
            int x4 = Packet.PopInt();
        }
    }
}