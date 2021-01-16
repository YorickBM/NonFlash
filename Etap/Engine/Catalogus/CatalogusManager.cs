using Engine.Furniture;
using Etap;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Retro.Communication.Packets.Incoming.Catalog;
using Retro.Communication.Packets.Outgoing.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Util;
using Util.ButtonTypes;

namespace Engine.Catalogus
{
    class CatalogusManager
    {
        CatalogusContent Catalogus;
        Timer delay;
        IPageItem selectedItem;

        Font pageNameFont;

        bool OnlyFirstTime = false, isInitailized = false;
        public Dictionary<CatalogusPage, int> pages;
        //public List<CatalogusPromotion> promotions
        public CatalogusPromotion[] promotions;

        public CatalogusImageManager catalogusImageManager;

        private string mode;
        private string pageTemplate;
        private int openPageId = -1;
        private int rootPageId = -1;

        public CatalogusManager(ContentManager content)
        {
            pages = new Dictionary<CatalogusPage, int>();
            promotions = new CatalogusPromotion[4];// new List<CatalogusPromotion>();
            promotions[0] = new CatalogusPromotion(content);
            promotions[1] = new CatalogusPromotion(content);
            promotions[2] = new CatalogusPromotion(content);
            promotions[3] = new CatalogusPromotion(content);

            mode = String.Empty;
            pageTemplate = String.Empty;
            selectedItem = null;

            pageNameFont = new Font(content, "Fonts/cataPageName", "", new Color(106, 106, 106));

            Catalogus = new CatalogusContent(content, new Vector2i(0, 0), new Vector2i(425, 300));
            catalogusImageManager = new CatalogusImageManager(content);
        }

        public Font GetPageFont() { return pageNameFont; }

        public void ToggleCatalogus()
        {
            if (Catalogus.isOpen())
            {
                Catalogus.Close();
            }
            else
            {
                RetroEnvironment.GetGame().GetClientManager().SendPacket(new GetCatalogIndexEvent());
            }

        }

        internal void Initialize(string mode)
        {
            if(!isInitailized)
            {
                this.mode = mode;
                isInitailized = true;
                Catalogus.CreatePages(pages);
            }
        }

        internal void UpdateCatalogus()
        {
            
        }

        internal void ShowCatalogus()
        {
            Catalogus.Open();
        }

        internal void Open()
        {
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new GetCatalogIndexEvent());
        }

        internal CatalogusPage GetPage(int id)
        {
            IEnumerable<CatalogusPage> posPages = (from page in pages where page.Key.pageId == id orderby page.Value descending select page.Key);
            if (posPages.Count() > 0)
                return posPages.First();
            else return null;
        }

        internal void LoadPageItems(int id)
        {
            Catalogus.LoadNewPageItems(GetPage(id));
        }

        bool itemIsSelected = false;
        internal void SelectItem(int pageId, int itemId)
        {
            itemIsSelected = false;
            selectedItem = null;

            if (openPageId == pageId && itemId != -1)
            {
                IPageItem item = GetPage(pageId).GetItemById(itemId);
                if (item != null)
                {
                    itemIsSelected = true;
                    selectedItem = item;
                }
            }
        }

        internal IPageItem GetSelectedItem() { return selectedItem; }

        internal void OpenPage(int id)
        {
            IEnumerable<CatalogusPage> rootPage = (from page in pages where page.Value == -1 && page.Key.pageId == id orderby page.Value descending select page.Key);
            if (rootPage.Count() > 0)
            {
                if(rootPageId != rootPage.First().pageId) Catalogus.LoadNewTree(rootPage.First().pageId, ref pages);
                rootPageId = rootPage.First().pageId;
            }

            RetroEnvironment.GetGame().GetClientManager().SendPacket(new GetCatalogPageEvent(id));
            if (rootPage.Count() <= 0) openPageId = id;
            if (rootPage.Count() <= 0) SelectItem(id, -1);
        }
        internal void OpenPage(string link)
        {
            IEnumerable<CatalogusPage> linkedPage = (from page in pages where page.Key.pageLink.Equals(link) orderby page.Value descending select page.Key);
            if (linkedPage.Count() > 0)
            {
                OpenPage(linkedPage.First().pageId);
            } else Logger.Warn("Could not open page link:", link);
        }

        public int GetActivePage()
        {
            return openPageId;
        }

        internal void SetSize(int width, int height)
        {
            Catalogus.SetSize(width, height);
        }

        internal void SetPosition(int posX, int posY)
        {
            if (OnlyFirstTime)
            {
                OnlyFirstTime = false;
                Catalogus.SetPosition(new Vector2i(posX, posY));
            }
        }

        internal String GetCataMode() { return mode; }
        internal void SetTemplate(string template) { this.pageTemplate = template; }

        internal void SetBanner(string banner) {
            Image imgBanner;
            catalogusImageManager.GetBanner(banner, out imgBanner);
            Catalogus.SetBanner(imgBanner);
        }
        internal void SetTeaser(string teaser) { }

        internal void AddPromotion(string title, string pageLink, string image, int promotionNum)
        {
            promotions[promotionNum].SetTitle(title);
            promotions[promotionNum].SetPageLink(pageLink);
            promotions[promotionNum].SetImage(image);

            if (promotionNum == 0) promotions[promotionNum].SetSize(new Vector2i(184, 460));
            else promotions[promotionNum].SetSize(new Vector2i(356, 126));
        }

        internal void Update(GameTime gameTime)
        {
            if (Catalogus.isOpen())
            {
                Catalogus.Update(gameTime, openPageId, ref pages);

                Catalogus.UpdateVoucher(gameTime, this.pageTemplate.Equals("frontpage4"));
                if (this.pageTemplate.Equals("frontpage4"))
                    foreach (CatalogusPromotion promo in promotions)
                        promo.Update(gameTime);

                if (itemIsSelected && selectedItem != null) Catalogus.UpdateItemSelected(gameTime, selectedItem);
            }
        }

        internal void UnloadContent()
        {
            Catalogus.UnloadContent();
            foreach (CatalogusPromotion promo in promotions)
                promo.Unloadcontent();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (Catalogus.isOpen())
            {
                Catalogus.Draw(spriteBatch);

                if (this.pageTemplate.Equals("frontpage4"))
                {
                    Catalogus.DrawPromotions(spriteBatch, promotions);
                    Catalogus.DrawVoucher(spriteBatch);
                }
                else
                {
                    if (this.pageTemplate.Equals("default_3x3"))
                    {
                        Catalogus.DrawDefault3x3(spriteBatch);

                        if (!itemIsSelected) Catalogus.DrawNoItemSelected(spriteBatch);
                        else if (selectedItem != null) Catalogus.DrawItemIsSelected(spriteBatch, selectedItem);
                        else Catalogus.DrawNoItemSelected(spriteBatch);
                    }
                    Catalogus.DrawPageTree(spriteBatch, rootPageId);
                }
            }
        }
    }
}
