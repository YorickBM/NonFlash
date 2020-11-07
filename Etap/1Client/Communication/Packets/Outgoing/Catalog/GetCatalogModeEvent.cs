namespace Retro.Communication.Packets.Incoming.Catalog
{
	class GetCatalogModeEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string PageMode = Packet.PopString();
        }
    }
}
