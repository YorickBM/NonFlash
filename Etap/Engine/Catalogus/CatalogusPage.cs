using Engine.Furniture;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Engine.Catalogus
{
    class CatalogusPage
    {
        public int pageId;
        public int parentId;
        public string caption;
        public string pageLink;
        public int iconId;
        public bool visible;

        private Font pageName;
        private Image icon;
        private Rectangle area;

        internal bool isCollapsed;
        internal List<IPageItem> items;

        public CatalogusPage(int pageId, int parentId, string caption, string pageLink, int icon, bool visible)
        {
            this.pageId = pageId;
            this.parentId = parentId;
            this.caption = caption;
            this.pageLink = pageLink;
            this.iconId = icon;
            this.visible = visible;
            this.isCollapsed = true;

            items = new List<IPageItem>();
        }

        public CatalogusPage AddItems(params IPageItem[] items)
        {
            this.items.AddRange(items);
            return this;
        }

        public CatalogusPage AddItem(IPageItem item)
        {
            this.items.Add(item);
            return this;
        }

        public IPageItem GetItemById(int itemId)
        {
            foreach(IPageItem itm in items)
                if (itm.GetItemId() == itemId) return itm;

            return null;
        }

        internal void CleanUp()
        {
            items.Clear();
        }
    }
}
