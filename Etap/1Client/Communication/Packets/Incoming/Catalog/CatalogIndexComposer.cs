using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;
using System.Collections.Generic;

namespace Retro.Communication.Packets.Outgoing.Catalog
{
    public class CatalogIndexComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            /* int width = Packet.PopInt();
            int height = Packet.PopInt();

            int x = Packet.PopInt();
            int y = Packet.PopInt();

            Session.initCata(width, height, x, y);

            List<CatalogPage> pages = new List<CatalogPage>();
            int amountPages = loadRoot(Packet);

            Logger.DebugWarn(amountPages, " <- pages");

            for(int i = 0; i < amountPages; i++)
            {
                loadPage(Packet, -1, ref pages);
            }
            
            bool x0 = Packet.PopBoolean(); //false
            string x1 = Packet.PopString(); // NORMAL

            //Session.GetCatalogus().CatalogPages = pages;
            //Session.GetCatalogus().amountPages = amountPages;
            //Session.GetCatalogus().NeedUpdate = true;
        }

        private int loadRoot(ClientPacket Packet)
        {
            bool visble = Packet.PopBoolean(); //true
            int icon = Packet.PopInt(); // 0
            int pageId = Packet.PopInt(); // -1
            string pageLink = Packet.PopString(); // root
            string caption = Packet.PopString(); // String.empty
            int itemOffers = Packet.PopInt(); // 0
            int treeSize = Packet.PopInt(); //Tree Size

            return treeSize;
        }

        private void loadPage(ClientPacket Packet, int parentId, ref List<CatalogPage> pages)
        {
            Dictionary<int, CatalogItem> items = new Dictionary<int, CatalogItem>();

            bool visible = Packet.PopBoolean();
            int icon = Packet.PopInt();
            int pageId = Packet.PopInt(); // -1
            string pageLink = Packet.PopString();
            string caption = Packet.PopString();
            int itemOffers = Packet.PopInt();
            for (int i = 0; i < itemOffers; i++) {
                int offerKey = Packet.PopInt();
                items.Add(offerKey, null);
            }
            int treeSize = Packet.PopInt();

            for (int i = 0; i < treeSize; i++)
            {
                loadPage(Packet, pageId, ref pages);
            }

            Logger.Debug(caption, ", ", pageId, ", ", parentId);

            CatalogPage pg = new CatalogPage(pageId, parentId, caption, pageLink, icon, visible, true, "", "", "", null, null, items);
            pages.Add(pg);*/
        }
    }
}