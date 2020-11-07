using Etap.Communication.Packets.Outgoing;
using Retro.Communication.Packets.Outgoing.Catalog;

namespace Retro.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : ServerPacket
    {
        public GetCatalogIndexEvent() : base(ServerPacketHeader.GetCatalogIndexMessageEvent)
        {

        }
    }
}