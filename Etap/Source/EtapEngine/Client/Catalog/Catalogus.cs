using Etap.Hotel.GameClients;
using Etap.Source.EtapEngine.Layouts;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Etap.Source.EtapEngine.Client.Catalog
{
    public class Catalogus
    {
        private List<CatalogDeal> _deals;
        private List<CatalogItem> _items;
        private List<CatalogPage> _pages;

        private List<CatalogPage> _rootPages;
        private Dictionary<CatalogPage, int> _pageById;
        private List<Image> _RootPageImages;
        private List<Image> _ContentImages;

        public List<CatalogPage> CatalogPages;
        public int amountPages = 0;
        public bool NeedUpdate = false;

        private MenuManager _MenuManager;

        private GameClient _session;
        public void SetSession(GameClient session)
        {
            _session = session;
        }

        public Catalogus()
        {
            _deals = new List<CatalogDeal>();
            _items = new List<CatalogItem>();
            _pages = new List<CatalogPage>();

            _rootPages = new List<CatalogPage>();
            _pageById = new Dictionary<CatalogPage, int>();
            _RootPageImages = new List<Image>();
            _ContentImages = new List<Image>();

            _MenuManager = new MenuManager();
            //_layout = layout;
        }

        public void UpdatePages(List<CatalogPage> pages, int amountRootPages = 1)
        {
            _pages = pages;
            Logger.Debug("Updating pages");

            //Sort the Pages
            foreach (CatalogPage page in pages)
            {
                if (!page.Visible)
                    continue;

                if(page.ParentId == -1)
                {
                    _rootPages.Add(page);
                    Logger.Info(page.Caption, " - ", page.Id, ", -1");
                }
                else {
                    _pageById.Add(page, page.ParentId);
                    Logger.Info(page.Caption, " - ", page.Id);
                }
            }

            //Calculate size for every Root Page Tab
            int amountRoot = _rootPages.Count();
            float width = _session.GetCatalogusLayout().GetSize().X;
            float sizePerRootPage = (width / amountRoot) - 16;
            int it = 0;

            Vector2 offset = new Vector2(8, 38);
            foreach(CatalogPage rootPage in _rootPages)
            {
                Image image = new Image(null);
                if (amountRoot == 1)
                    image.Path = "Menu/Catalogus/Header/1ItemsButton";
                else if (amountRoot == 2)
                    image.Path = "Menu/Catalogus/Header/2ItemsButton";
                else if(amountRoot == 3)
                    image.Path = "Menu/Catalogus/Header/3ItemsButton";
                else if(amountRoot == 4)
                    image.Path = "Menu/Catalogus/Header/4ItemsButton";
                else if (amountRoot == 5)
                    image.Path = "Menu/Catalogus/Header/5ItemsButton";
                else if(amountRoot == 6)
                    image.Path = "Menu/Catalogus/Header/6ItemsButton";
                image.Position = _session.GetCatalogusLayout().GetPosition() + offset;
                image.Effects = "SpriteSheetEffect";
                image.EffectIsActive = true;
                image.IsAnimated = false;
                image.LoadContent();
                _RootPageImages.Add(image);

                #region Text
                Image rimageT = new Image(null);
                rimageT.Text = rootPage.Caption + "";
                rimageT.FontName = String.Empty;
                rimageT.Scale = new Vector2(1, 1);
                rimageT.setColor(Color.Black);

                rimageT.Position = _session.GetCatalogusLayout().GetPosition() + offset + new Vector2(sizePerRootPage / 2, 12);
                //rimageT.LoadContent();
                //_RootPageImages.Add(rimageT);
                #endregion

                //Add offset of 1 tab
                offset.X += sizePerRootPage;
            }

            Vector2 off = new Vector2(8, 192);
            //TopAndBottom(ref off, new Vector2(184, 433), "Menu/Catalogus/Content/", new Vector2(5, 5), new Vector2(3, 3), ref _ContentImages);

            _MenuManager.LoadContent(String.Empty);
        }

        private void TopAndBottom(ref Vector2 offset, Vector2 size, String mainPath, Vector2 cornerSize, Vector2 sideSize, ref List<Image> list)
        {
            #region Center Background
            Image rimage = new Image(null);
            rimage.Path = mainPath + "ContentBackground";

            Vector2 pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + sideSize;

            Vector2 scl = new Vector2(size.X - cornerSize.X, size.Y - sideSize.Y);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Left Corner
            rimage = new Image(null);
            rimage.Path = mainPath + "CornerLeft";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset;

            scl = new Vector2(1, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Right Corner
            rimage = new Image(null);
            rimage.Path = mainPath + "CornerRight";

            pos = _session.GetCatalogusLayout().GetPosition();
            rimage.Position = pos + offset + new Vector2(size.X - cornerSize.X, 0);

            scl = new Vector2(1, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Top Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeTop";

            pos = _session.GetCatalogusLayout().GetPosition();
            rimage.Position = pos + offset + new Vector2(cornerSize.X, 0);

            scl = new Vector2(size.X - (2 * cornerSize.X), 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Left Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeLeft";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + new Vector2(0, cornerSize.Y);

            scl = new Vector2(1, size.Y - (cornerSize.Y * 2));
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Right Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeRight";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + new Vector2(size.X - sideSize.X, cornerSize.Y);

            scl = new Vector2(1, size.Y - (cornerSize.Y * 2));
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Bottom Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeBottom";

            pos = _session.GetCatalogusLayout().GetPosition();
            rimage.Position = pos + offset + new Vector2(cornerSize.X, -cornerSize.Y) + new Vector2(0, size.Y + sideSize.Y);

            scl = new Vector2(size.X - (cornerSize.X * 2), 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Left Corner Bottom
            rimage = new Image(null);
            rimage.Path = mainPath + "CornerLeftBottom";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + new Vector2(0, size.Y - cornerSize.Y);

            scl = new Vector2(1, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Right Corner Bottom
            rimage = new Image(null);
            rimage.Path = mainPath + "CornerRightBottom";

            pos = _session.GetCatalogusLayout().GetPosition();
            rimage.Position = pos + offset + new Vector2(size.X - cornerSize.X, size.Y - cornerSize.Y);

            scl = new Vector2(1, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
        }
        private void TopAndSides(ref Vector2 offset, Vector2 size, String mainPath, Vector2 cornerSize, Vector2 sideSize, ref List<Image> list)
        {
            #region Center Background
            Image rimage = new Image(null);
            rimage.Path = mainPath + "ContentBackground";

            Vector2 pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + sideSize;

            Vector2 scl = new Vector2(size.X - cornerSize.X, size.Y + sideSize.Y);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            _RootPageImages.Add(rimage);
            #endregion
            #region Left Corner
            rimage = new Image(null);
            rimage.Path = mainPath + "CornerLeft";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset;

            scl = new Vector2(1, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            _RootPageImages.Add(rimage);
            #endregion
            #region Right Corner
            rimage = new Image(null);
            rimage.Path = mainPath + "CornerRight";

            pos = _session.GetCatalogusLayout().GetPosition();
            rimage.Position = pos + offset + new Vector2(size.X - cornerSize.X, 0);

            scl = new Vector2(1, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            _RootPageImages.Add(rimage);
            #endregion
            #region Top Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeTop";

            pos = _session.GetCatalogusLayout().GetPosition();
            rimage.Position = pos + offset + new Vector2(cornerSize.X, 0);

            scl = new Vector2(size.X - 12, 1);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            _RootPageImages.Add(rimage);
            #endregion
            #region Left Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeLeft";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + new Vector2(0, cornerSize.Y);

            scl = new Vector2(1, size.Y);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion
            #region Right Edge
            rimage = new Image(null);
            rimage.Path = mainPath + "EdgeRight";

            pos = _session.GetCatalogusLayout().GetPosition();
            //Modify Position
            rimage.Position = pos + offset + new Vector2(size.X - sideSize.X, cornerSize.Y);

            scl = new Vector2(1, size.Y);
            //Modify Scale
            rimage.Scale = scl;

            rimage.LoadContent();
            list.Add(rimage);
            #endregion

        }

        public void Open(CloseLayout layout) 
        {
            layout.Open();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //foreach (Image img in _RootPageImages)
                //img.Draw(spriteBatch);

            //_MenuManager.Draw(spriteBatch);

           //foreach (Image img in _ContentImages)
                //img.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            //foreach (Image img in _RootPageImages)
                //img.Update(gameTime);

            //_MenuManager.Update(gameTime);

            //foreach (Image img in _ContentImages)
                //img.Update(gameTime);

            if(NeedUpdate)
            {
                NeedUpdate = false;
                UpdatePages(CatalogPages, amountPages);
            }
        }

        public void UnloadContent()
        {
            _session.GetCatalogusLayout().UnloadContent();

            foreach (Image img in _RootPageImages)
                img.UnloadContent();

            _MenuManager.UnloadContent();

            foreach (Image img in _ContentImages)
                img.UnloadContent();
        }
        public void LoadContent()
        {
            _session.GetCatalogusLayout().LoadContent();
        }
    }
}
