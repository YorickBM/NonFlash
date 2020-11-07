using Retro.Hotel.Catalog;
using Retro.Communication.Packets.Outgoing;

namespace Retro.Communication.Packets.Incoming.Inventory.Purse
{
    class GetHabboClubWindowEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            CatalogPage page = RetroEnvironment.GetGame().GetCatalog().TryGetPageByTemplate("vip_buy");
            if (page == null)
                return;

            ServerPacket Message = new ServerPacket(ServerPacketHeader.GetClubComposer);
            Message.WriteInteger(page.Items.Values.Count);

            foreach (CatalogItem catalogItem in page.Items.Values)
            {
                catalogItem.SerializeClub(Message, Session);
            }

            Message.WriteInteger(Packet.PopInt());

            Session.SendMessage(Message);
        }
    }
}
