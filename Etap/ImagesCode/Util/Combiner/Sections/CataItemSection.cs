using Engine.Furniture;
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
using System.Timers;
using Util.ButtonTypes;

namespace Util.Combiner.Sections
{
    class CataItemSection : ICombinable
    {
        private List<Image> images;
        private List<Font> fonts;
        private Vector2i size, localPosition;

        private List<Image> headerT;
        private List<Font> headerF;

        private Image icon, smallCredit, smallDucket, smallDiamond, smallGotw;

        internal IPageItem item;
        internal int pageId;
        private Timer delay;

        private bool isClicked = false;

        public CataItemSection(ContentManager content, int pageId, IPageItem item, Vector2i position, Vector2i size, out int bodySize)
        {
            images = new List<Image>();
            fonts = new List<Font>();
            headerT = new List<Image>();
            headerF = new List<Font>();

            this.size = size;
            this.localPosition = position;
            this.item = item;
            this.pageId = pageId;
            delay = new Timer(200);
            delay.Elapsed += Delay_Elapsed;

            #region Section Header
            String classname = String.Empty;
            if (GameScreenManager.Instance.GetFurniTypeBySpriteId(item.GetSpriteId()) != null) classname = GameScreenManager.Instance.GetFurniTypeBySpriteId(item.GetSpriteId()).classname;

            Logger.Debug("Generating header for:", classname);

            if (classname != String.Empty)
            {
                if (File.Exists(@"Content/Client/Items/" + classname + "/icon.xnb"))
                    GameScreenManager.Instance.GetCatalogusManager().catalogusImageManager.GetFurniIcon(classname, out icon);
                else icon = new Image(content, @"Client/Items/CantFindTextureTextures/item_small", Vector2.Zero);
            } else
            {
                icon = new Image(content, @"Client/Items/CantFindTextureTextures/item_small", Vector2.Zero);
            }
            icon.SetPosition(position + new Vector2i(size.X / 2 - icon.dimensions.X / 2, size.Y / 2 - icon.dimensions.Y / 2));
            headerT.Add(icon);

            smallCredit = new Image(content, @"Menu/Catalogus/Items/Small/credits", Vector2.Zero);
            smallDiamond = new Image(content, @"Menu/Catalogus/Items/Small/diamonds", Vector2.Zero);
            smallDucket = new Image(content, @"Menu/Catalogus/Items/Small/duckets", Vector2.Zero);
            smallGotw = new Image(content, @"Menu/Catalogus/Items/Small/gotw", Vector2.Zero);

            Font priceOne = new Font(content, "Fonts/Catalogus/CurrencyTitle", item.GetPrices()[0].ToString(), new Color(11, 11, 11)); //125, 216, 246
            priceOne.SetPosition(position + new Vector2i(34 - priceOne.measureString().X, 41 + ((11 / 2) - (priceOne.measureString().Y / 2))));
            headerF.Add(priceOne);

            Font priceTwo = new Font(content, "Fonts/Catalogus/CurrencyTitle", "+ 0", new Color(11, 11, 11)); //125, 216, 246
            priceTwo.SetPosition(position + new Vector2i(34 - priceTwo.measureString().X, 55 + ((11 / 2) - (priceOne.measureString().Y / 2))));

            switch (item.GetPricesTypes()[0])
            {
                case PriceTypes.CREDITS:
                    headerT.Add(smallCredit);
                    break;
                case PriceTypes.DUCKETS:
                    headerT.Add(smallDucket);
                    break;
                case PriceTypes.DIAMONDS:
                    headerT.Add(smallDiamond);
                    break;
                case PriceTypes.GOTW:
                    headerT.Add(smallGotw);
                    break;
                default:
                    Logger.Error("Unknown price type:", item.GetPricesTypes()[0].ToString());
                    break;
            }

            if (item.IsDubbelPriced())
            {
                headerF.Add(priceTwo);

                switch (item.GetPricesTypes()[1])
                {
                    case PriceTypes.DUCKETS:
                        headerT.Add(smallDucket);
                        break;
                    case PriceTypes.DIAMONDS:
                        headerT.Add(smallDiamond);
                        break;
                    case PriceTypes.GOTW:
                        headerT.Add(smallGotw);
                        break;
                    default:
                        Logger.Error("Unknown price type:", item.GetPricesTypes()[1].ToString());
                        break;
                }
            }

            #endregion

            bodySize = size.Y;
        }

        internal void UpdatePosition(Vector2i position, out int bodySize)
        {
            this.localPosition = position;
            bodySize = size.Y;

            icon.SetPosition(position + new Vector2i(53 / 2 - icon.dimensions.X / 2, 38 / 2 - icon.dimensions.Y / 2));

            Font priceOne = headerF[0];
            priceOne.SetText(item.GetPrices()[0].ToString());
            priceOne.SetPosition(position + new Vector2i(34 - priceOne.measureString().X, 41 + ((11 / 2) - (priceOne.measureString().Y / 2))));

            switch (item.GetPricesTypes()[0])
            {
                case PriceTypes.CREDITS:
                    smallCredit.SetPosition(position + new Vector2i(37, 41 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                    break;
                case PriceTypes.DUCKETS:
                    smallDucket.SetPosition(position + new Vector2i(37, 41 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                    break;
                case PriceTypes.DIAMONDS:
                    smallDiamond.SetPosition(position + new Vector2i(37, 41 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                    break;
                case PriceTypes.GOTW:
                    smallGotw.SetPosition(position + new Vector2i(37, 41 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                    break;
            }

            if (item.IsDubbelPriced())
            {
                Font priceTwo = headerF[1];
                priceTwo.SetText("+ " + item.GetPrices()[1].ToString());
                priceTwo.SetPosition(position + new Vector2i(34 - priceTwo.measureString().X, 55 + ((11 / 2) - (priceTwo.measureString().Y / 2))));

                switch (item.GetPricesTypes()[1])
                {
                    case PriceTypes.DUCKETS:
                        smallDucket.SetPosition(position + new Vector2i(37, 55 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                        break;
                    case PriceTypes.DIAMONDS:
                        smallDiamond.SetPosition(position + new Vector2i(37, 55 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                        break;
                    case PriceTypes.GOTW:
                        smallGotw.SetPosition(position + new Vector2i(37, 55 + ((11 / 2) - (smallCredit.GetTexture().Height / 2))));
                        break;
                }
            }

            UploadTextures();
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
                    //Select the item in top menu
                    Logger.Debug(pageId, item.GetItemId());
                    GameScreenManager.Instance.GetCatalogusManager().SelectItem(pageId, item.GetItemId());
                    delay.Enabled = true;
                }
            }
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        public void CleanUp()
        {
            images.Clear();
            fonts.Clear();
        }

        internal void UploadTextures()
        {
            CleanUp();

            images.AddRange(headerT);
            fonts.AddRange(headerF);
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
}
