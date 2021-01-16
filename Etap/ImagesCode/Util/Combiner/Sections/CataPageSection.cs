using Engine.Catalogus;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Util.ButtonTypes;

namespace Util.Combiner.Sections
{
    class CataPageSection : ICombinable
    {
        private List<Image> images;
        private List<Font> fonts;
        private Vector2i size, localPosition;

        private List<Image> bodyT, headerT;
        private List<Font> bodyF, headerF;

        private Image collapsed, open, icon;
        private List<Row> rows;

        internal CatalogusPage page;
        private Timer delay;
        private bool hasSubPages;

        int rowNum = 0;
        int BodySize = 0;

        private bool isClicked = false;

        public CataPageSection(ContentManager content, CatalogusPage page, Vector2i position, Vector2i size, out int bodySize, IEnumerable<CatalogusPage> subPages)
        {
            images = new List<Image>();
            fonts = new List<Font>();
            bodyT = new List<Image>();
            headerT = new List<Image>();
            bodyF = new List<Font>();
            headerF = new List<Font>();
            rows = new List<Row>();

            this.size = size;
            this.localPosition = position;
            this.page = page;
            delay = new Timer(200);
            delay.Elapsed += Delay_Elapsed;

            try
            {

                #region Section Body

                rowNum = 0;
                Logger.Debug("Generating Body for:", page.caption);
                foreach (CatalogusPage subPage in subPages)
                {
                    try
                    {
                        Row row = new Row(content, rowNum++, position, size, subPage);
                        row.hidden = subPage.isCollapsed;
                        bodyF.AddRange(row.GetFonts());
                        bodyT.AddRange(row.GetImages());
                        rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Following page could not be displayed:", subPage.caption, "(" + subPage.pageId + ")");
                        Logger.Error(ex);
                    }
                }

                #endregion

                #region Section Header
                Logger.Debug("Generating header for:", page.caption);

                Font title = new Font(content, "Fonts/Catalogus/PageTitle", page.caption, new Color(109, 109, 109)); //125, 216, 246
                title.SetPosition(position + new Vector2i(23, size.Y / 2 - title.measureString().Y / 2));
                headerF.Add(title);

                collapsed = new Image(content, "Menu/Catalogus/Section/openClose", new Vector2i(10, 10));
                collapsed.SourceRect = new Rectangle((int)0 + (int)collapsed.Size.X, (int)0, (int)collapsed.Size.X, (int)collapsed.Size.Y);
                collapsed.SetPosition(position + new Vector2i(6, size.Y / 2 - collapsed.dimensions.Y / 2) + new Vector2i(title.measureString().X, 0));

                open = new Image(content, "Menu/Catalogus/Section/openClose", new Vector2i(10, 10));
                open.SetPosition(position + new Vector2i(6, size.Y / 2 - open.dimensions.Y / 2) + new Vector2i(title.measureString().X, 0));

                if (subPages.Count() > 0) {
                    if (!page.isCollapsed) headerT.Add(open);
                    else headerT.Add(collapsed);
                    hasSubPages = true;
                } else
                {
                    hasSubPages = false;
                }

                try
                {
                    if (File.Exists(@"Content/catalogue/icons/icon_" + page.iconId.ToString() + ".xnb")) icon = new Image(content, "catalogue/icons/icon_" + page.iconId.ToString(), Vector2.Zero);
                    else icon = new Image(content, "catalogue/icons/icon_1", Vector2.Zero);
                }
                catch (Exception ex)
                {
                    icon = GameScreenManager.Instance.catalogusnotFoundIcon.Clone() as Image;
                }
                icon.SetPosition(position + new Vector2i(6, size.Y / 2 - icon.dimensions.Y / 2));
                headerT.Add(icon);

                #endregion

            } catch(Exception ex)
            {
                Logger.Error("Error on creating catalogus page section:", page.caption);
            }

            bodySize = rowNum * 20;
            BodySize = bodySize;
        }


        internal void UpdatePosition(Vector2i position, out int bodySize)
        {
            this.localPosition = position;
            bodySize = 0;

            Font title = headerF[0];
            title.SetPosition(position + new Vector2i(28, size.Y / 2 - title.measureString().Y / 2));
            open.SetPosition(position + new Vector2i(144, size.Y / 2 - open.dimensions.Y / 2));
            collapsed.SetPosition(position + new Vector2i(144, size.Y / 2 - collapsed.dimensions.Y / 2));
            icon.SetPosition(position + new Vector2i(2, size.Y / 2 - icon.dimensions.Y / 2));

            int rowNum = 0;
            UpdateRows(rowNum, position, size);

            if (!page.isCollapsed) bodySize = BodySize;
            UploadTextures();
        }

        private void UpdateRows(int rowNum, Vector2i position, Vector2i size)
        {
            BodyCleanUp();
            foreach (Row row in rows)
            {
                row.CleanUp();
                row.Update(rowNum++, position, size);
                if (!page.isCollapsed) bodyF.AddRange(row.GetFonts());
                if (!page.isCollapsed) bodyT.AddRange(row.GetImages());

                if (page.isCollapsed) row.hidden = true;
                else row.hidden = false;
            }
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        public void BodyCleanUp()
        {
            //foreach (Image img in bodyT) img.UnloadContent();
            //foreach (Font fnt in bodyF) fnt.UnloadContent();
            bodyT.Clear();
            bodyF.Clear();
        }
        public void CleanUp()
        {
            //foreach (Image img in images) img.UnloadContent();
            //foreach (Font fnt in fonts) fnt.UnloadContent();
            images.Clear();
            fonts.Clear();
        }

        internal void UploadTextures()
        {
            CleanUp();

            //Logger.DebugWarn("Uploaded textures!");

            images.AddRange(headerT);
            if (!page.isCollapsed) images.AddRange(bodyT);
            fonts.AddRange(headerF);
            if (!page.isCollapsed) fonts.AddRange(bodyF);
        }

        public void Update(GameTime gameTime, Vector2i offset, ref ScrollView view)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle(localPosition.X + offset.X, localPosition.Y + offset.Y, size.X, size.Y);
            var rectangleV = new Rectangle(view.GetPosition().X, view.GetPosition().Y, view.GetViewSize().X, view.GetViewSize().Y);

            if (rectangle.Contains(mousePoint) && rectangleV.Contains(mousePoint))
            {
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                isClicked = false;
            }

            if (isClicked)
            {
                if (!delay.Enabled)
                {
                    if (hasSubPages)
                    {
                        page.isCollapsed = !page.isCollapsed;
                    }else
                    {
                        GameScreenManager.Instance.GetCatalogusManager().OpenPage(page.pageId);
                    }

                    delay.Enabled = true;
                }
            }

            foreach (Row row in rows) row.LogisticUpdate(gameTime, offset, localPosition, size, ref view);

            #region Image Updator on Category Settings
            if (!page.isCollapsed)
            {
                if (headerT.Contains(collapsed)) headerT.Remove(collapsed);
                if (!headerT.Contains(open)) view.texturesChanged = true;
                if (!headerT.Contains(open))
                {
                    view.texturesChanged = true;
                    if(hasSubPages) headerT.Add(open);
                    UploadTextures();
                }
            }
            else
            {
                if (headerT.Contains(open)) headerT.Remove(open);
                if (!headerT.Contains(collapsed))
                {
                    view.texturesChanged = true;
                    if (hasSubPages) headerT.Add(collapsed);
                    UploadTextures();
                }
            }
            #endregion
        }

        public void UnloadContent()
        {
            foreach (SectionButton btn in images)
                btn.UnloadContent();
        }

        public Font[] GetFonts()
        {
            return fonts.ToArray();
        }
        public Image[] GetImages()
        {
            return images.ToArray();
        }
        public Vector2i GetSize()
        {
            return size;
        }
    }

    class Row : ICombinable
    {
        List<Image> images;
        List<Font> fonts;

        private Image icon;
        private Font pageName;
        private Rectangle area;

        private CatalogusPage page;
        private Timer timer;

        private int rowN;
        public bool hidden;
        private bool isClicked = false;

        public Row(ContentManager content, int rowNum, Vector2i position, Vector2i size, CatalogusPage page)
        {
            images = new List<Image>();
            fonts = new List<Font>();
            this.page = page;
            timer = new Timer(200);
            timer.Elapsed += Timer_Elapsed;
            rowN = rowNum;
            hidden = true;

            pageName = new Font(content, "Fonts/Catalogus/PageTitle", page.caption, new Color(109, 109, 109));
            area = new Rectangle(0, 0, 177, 21);
            try
            {
                if (File.Exists(@"Content/catalogue/icons/icon_" + icon.ToString() + ".xnb")) this.icon = new Image(content, "catalogue/icons/icon_" + icon.ToString(), Vector2.Zero);
                else this.icon = new Image(content, "catalogue/icons/icon_1", Vector2.Zero);
            }
            catch (Exception ex)
            {
                this.icon = GameScreenManager.Instance.catalogusnotFoundIcon.Clone() as Image;
            }
        }

        public void LogisticUpdate(GameTime gameTime, Vector2i offset, Vector2i position, Vector2i size, ref ScrollView view)
        {
            int offsetPosX = 4;
            int offsetPosY = (20 * rowN) + (int)29;

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle(position.X + offset.X + offsetPosX, position.Y + offset.Y + offsetPosY, size.X, (int)area.Size.Y);
            var rectangleV = new Rectangle(view.GetPosition().X, view.GetPosition().Y, view.GetViewSize().X, view.GetViewSize().Y);

            if (rectangle.Contains(mousePoint) && !hidden && rectangleV.Contains(mousePoint))
            {
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                isClicked = false;
            }

            if (isClicked)
            {
                if (!timer.Enabled)
                {
                    Logger.Debug("Opening Page:", page.caption);
                    GameScreenManager.Instance.GetCatalogusManager().OpenPage(page.pageId);
                    timer.Start();
                }
            }

            pageName.Update(gameTime);
        }

        public void Update(int rowNum, Vector2i position, Vector2i size)
        {
            rowN = rowNum;
            int offsetPosX = 30;
            int offsetPosY = (21 * rowNum) + (int)21;

            pageName.SetPosition(position + new Vector2i(offsetPosX, offsetPosY) + new Vector2i(28, (area.Height / 2) - (pageName.measureString().Y / 2)));
            fonts.Add(pageName);

            icon.SetPosition(position + new Vector2i(offsetPosX, offsetPosY) + new Vector2i(0, (area.Height / 2) - (icon.GetTexture().Height / 2)));
            images.Add(icon);
        }

        public void CleanUp()
        {
            //foreach (Image img in images) img.UnloadContent();
            //foreach (Font fnt in fonts) fnt.UnloadContent();
            images.Clear();
            fonts.Clear();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
        }

        public Font[] GetFonts()
        {
            return fonts.ToArray();
        }
        public Image[] GetImages()
        {
            return images.ToArray();
        }
    }
}
