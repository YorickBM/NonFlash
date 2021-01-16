using Engine.Furniture;
using Engine.Navigator;
using Etap;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Retro.Communication.Packets.Outgoing.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.ButtonTypes;
using Util.Combiner;
using Util.Combiner.Sections;
using Util.Textfield;

namespace Engine.Catalogus
{
    class CatalogusContent : CloseAndInfoUI
    {
        TopButtonsContainer rootContainer;

        TextField textField;

        Image imgBanner;
        Image imgBannerLayer;
        Image imgBannerIcon;
        Image imgPageTeaser;

        List<CataPageSection> cataPageSections = new List<CataPageSection>();
        List<CataItemSection> cataItemSections = new List<CataItemSection>();

        ScrollView pageTree;
        ScrollView pageItems;

        CataBox tree;
        CataBox items;
        CataBox selectItem;

        HoverConstructedButton voucherButton;
        HoverConstructedButton present, buy;
        Image presentDisabled;

        Image voucherBackdrop;
        Font selectItemText, presentDisabledText;
        Font Amount, Price;

        Font priceOneText, priceTwoText, amountText;
        Image priceCredits, priceDuckets, priceDiamonds, priceGotw;

        public CatalogusContent(ContentManager content, Vector2i position, Vector2i size, int offsetX = 0, int offsetY = 0) : base(content, position, size, "Catalogus", offsetX, offsetY)
        {
            rootContainer = new TopButtonsContainer(content);
            voucherButton = new HoverConstructedButton(content, () => { RetroEnvironment.GetGame().GetClientManager().SendPacket(new RedeemVoucherEvent("Test")); }, new Vector2i(67, 22), 
                HoverConstructedButtonType.BASIC1,
                new Color [] { new Color(243, 243, 243), new Color(225, 225, 225), new Color(255, 255, 255) }, 
                new Color[] { new Color(217, 217, 217), new Color(163, 163, 163), new Color(238, 238, 238) }, 
                "catalogue.voucher.button");

            textField = new TextField(content, "Fonts/Arial", new Rectangle(50, 50, 216, 25), GameScreenManager.Instance.GraphicsDevice);
            textField.SetEdgeColor(new Color(32,74,95));

            imgBannerLayer = new Image(content, "catalogue/bannerLayer", Vector2.Zero);
            voucherBackdrop = new Image(content, "catalogue/voucher", Vector2.Zero);

            pageTree = new ScrollView(content, new Vector2i(0, 0), new Vector2i(159, 424), "menu/Catalogus/backdrop", null);
            pageItems = new ScrollView(content, new Vector2i(0, 0), new Vector2i(335, 146), "menu/Catalogus/backdrop", null);

            tree = new CataBox(content, new Vector2i(184, 433));
            items = new CataBox(content, new Vector2i(360, 155));
            selectItem = new CataBox(content, new Vector2i(360, 30));
            selectItemText = new Font(content, "Fonts/Catalogus/NoItemSelected", "catalogus.page.noitemselected", new Color(102, 102, 102));

            present = new HoverConstructedButton(content, () => { Logger.Error("Send User Message of that inplemented"); }, new Vector2i(170, 24),
                HoverConstructedButtonType.BASIC1,
                new Color[] { new Color(243, 243, 243), new Color(225, 225, 225), new Color(255, 255, 255) },
                new Color[] { new Color(217, 217, 217), new Color(163, 163, 163), new Color(238, 238, 238) },
                "catalogue.item.button.present", "Fonts/Catalogus/PresentButton");

            buy = new HoverConstructedButton(content, () => { RetroEnvironment.GetGame().GetClientManager().SendPacket(new PurchaseFromCatalogEvent(GameScreenManager.Instance.GetCatalogusManager().GetActivePage(), GameScreenManager.Instance.GetCatalogusManager().GetSelectedItem().GetItemId(), GameScreenManager.Instance.GetCatalogusManager().GetSelectedItem().ExtraData(), 10)); }, new Vector2i(170, 24),
                HoverConstructedButtonType.GREEN,
                new Color[] { new Color(0, 161, 0), new Color(0, 161, 0), new Color(0, 161, 0) },
                new Color[] { new Color(0, 144, 0), new Color(0, 144, 0), new Color(0, 144, 0) },
                "catalogue.item.button.buy", "Fonts/Catalogus/BuyButton");

            presentDisabled = new Image(content, "Menu/Catalogus/Extras/presentDisabled", Vector2.Zero);
            presentDisabledText = new Font(content, "Fonts/Catalogus/PresentButton", "catalogue.item.button.present", new Color(163, 163, 163));

            Amount = new Font(content, "Fonts/Catalogus/ItemDetails", "catalogue.item.details.amount", new Color(106, 106, 106));
            Price = new Font(content, "Fonts/Catalogus/ItemDetails", "catalogue.item.details.price", new Color(106, 106, 106));

            priceOneText = new Font(content, "Fonts/Catalogus/CurrencyTitle", "0", new Color(11, 11, 11));
            priceTwoText = new Font(content, "Fonts/Catalogus/CurrencyTitle", "0", new Color(11, 11, 11));
            amountText = new Font(content, "Fonts/Catalogus/CurrencyTitle", "0", new Color(11, 11, 11));

            priceCredits = new Image(content, @"Menu/Catalogus/Items/credits", Vector2.Zero);
            priceDiamonds = new Image(content, @"Menu/Catalogus/Items/diamonds", Vector2.Zero);
            priceDuckets = new Image(content, @"Menu/Catalogus/Items/duckets", Vector2.Zero);
            priceGotw = new Image(content, @"Menu/Catalogus/Items/gotw", Vector2.Zero);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            rootContainer.UnloadContent();
            textField.UnloadContent();
            voucherButton.UnloadContent();

            imgBannerLayer.UnloadContent();
            if (imgBanner != null) imgBanner.UnloadContent();

            pageTree.UnloadContent();
            pageItems.UnloadContent();

            tree.UnloadContent();
            items.UnloadContent();
            selectItem.UnloadContent();
            selectItemText.UnloadContent();
        }

        public void Update(GameTime gameTime, int parentId, ref Dictionary<CatalogusPage, int> pages)
        {
            if (isOpen())
            {
                base.Update(gameTime);
                rootContainer.Update(gameTime, size, headerHeight, edgeBottom.dimensions.Y);

                tree.Update(gameTime);
                items.Update(gameTime);
                selectItem.Update(gameTime);

                selectItemText.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(selectItemText.GetOriginalText()));
                selectItemText.Update(gameTime);

                imgBannerLayer.Update(gameTime);
                if (imgBanner != null)
                {
                    imgBanner.Scale = 2f;
                    imgBanner.Update(gameTime);
                    imgBanner.resizeCenter(size.X - 2, 90);
                }

                pageTree.Update(gameTime, position + new Vector2i(12, headerHeight + rootContainer.GetHeight() + 95 + 38));
                pageItems.Update(gameTime, position + new Vector2i(202 + 4, headerHeight + rootContainer.GetHeight() + 95 + 251 + 3));

                try
                {
                    foreach (CataPageSection section in cataPageSections)
                        section.Update(gameTime, pageTree.GetPosition() - pageTree.GetScrolled(), ref pageTree);

                    foreach (CataItemSection section in cataItemSections)
                        section.Update(gameTime, pageItems.GetPosition() - pageItems.GetScrolled(), ref pageItems);
                }
                catch
                {
                    Logger.Warn("Can not render a Catalogus Body!! Soo loading???!");
                }

                if (pageTree.texturesChanged)
                {
                    Vector2i offset = new Vector2i();
                    for (int i = 0; i < cataPageSections.Count(); i++)
                    {
                        int bodySize = 0;
                        CataPageSection section = cataPageSections[i];
                        section.UpdatePosition(offset, out bodySize);
                        offset += new Vector2i(0, cataPageSections[i].GetSize().Y + 5);
                        if (!cataPageSections[i].page.isCollapsed) offset += new Vector2i(0, bodySize);
                    }
                    SectionCombiner combiner = new SectionCombiner();
                    pageTree.SetContent(combiner.AddSections(cataPageSections.ToArray()).GetImages());
                    pageTree.SetText(combiner.AddSections(cataPageSections.ToArray()).GetFonts());
                    pageTree.texturesChanged = false;
                }
                if (pageItems.texturesChanged)
                {
                    int cell = 0;
                    int row = 0;
                    int maxBody = 0;
                    Vector2i offset = new Vector2i(0, 0);
                    for (int i = 0; i < cataItemSections.Count(); i++)
                    {
                        int bodySize = 0;
                        CataItemSection section = cataItemSections[i];
                        section.UpdatePosition(offset, out bodySize);
                        offset += new Vector2i(3 + 53, 0);
                        if (maxBody < bodySize) maxBody = bodySize;

                        if (cell++ >= 6)
                        {
                            cell = 0;
                            row++;
                            offset = new Vector2i(0, row * maxBody);
                        }
                    }
                    SectionCombiner combiner = new SectionCombiner();
                    pageItems.SetContent(combiner.AddSections(cataItemSections.ToArray()).GetImages());
                    pageItems.SetText(combiner.AddSections(cataItemSections.ToArray()).GetFonts());
                    pageItems.texturesChanged = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isOpen())
            {
                base.Draw(spriteBatch);
                rootContainer.Draw(spriteBatch, position, offset, headerHeight, 0.9f);

                if (imgBanner != null)
                {
                    imgBanner.Draw(spriteBatch, position + offset + new Vector2i(1, headerHeight + rootContainer.GetHeight()), 0.94f);
                    imgBannerLayer.Draw(spriteBatch, position + offset + new Vector2i(1, headerHeight + rootContainer.GetHeight()), 0.95f);
                }
            }
        }

        internal void DrawPageTree(SpriteBatch spriteBatch, int parentId, float depth = 0.92f) {
            if (parentId == -1) //No page has been opened yet.
                return;

            pageTree.Draw(spriteBatch, depth);
            tree.Draw(spriteBatch, position + new Vector2i(8, headerHeight + rootContainer.GetHeight() + 95 + 34), depth - 0.01f);
        }

        internal void LoadNewPageItems(CatalogusPage page)
        {
            cataItemSections.Clear();
            pageItems.CleanUp();
            Logger.Debug("Loading page items (" + page.items.Count() + ")", cataItemSections.Count());
            SectionCombiner combiner = new SectionCombiner();

            int cell = 0;
            int row = 0;
            Vector2i specialOffset = new Vector2i(0, 0);

            foreach (IPageItem item in page.items)
            {
                int maxBody = 0;
                int height = 62;
                if (item.IsDubbelPriced()) height = 74;

                int bodySize = 0;
                cataItemSections.Add(new CataItemSection(GameScreenManager.Instance.GetContentManager(), page.pageId, item, specialOffset, new Vector2i(53, height), out bodySize));
                if (maxBody < bodySize) maxBody = bodySize;

                specialOffset += new Vector2i(3 + 53, 0);
                if(cell++ >= 6)
                {
                    cell = 0;
                    row++;
                    specialOffset = new Vector2i(0, row * maxBody);
                }
            }

            //foreach (CataItemSection section in cataItemSection) section.UploadTextures();
            pageItems.SetContent(combiner.AddSections(cataItemSections.ToArray()).GetImages());
            pageItems.SetText(combiner.AddSections(cataItemSections.ToArray()).GetFonts());
            pageItems.texturesChanged = true;
        }

        internal void DrawDefault3x3(SpriteBatch spriteBatch, float depth = 0.92f)
        {
            pageItems.Draw(spriteBatch, depth);
            items.Draw(spriteBatch, position + new Vector2i(202, headerHeight + rootContainer.GetHeight() + 5 + 90 + 251), depth - 0.01f);
        }

        internal void DrawNoItemSelected(SpriteBatch spriteBatch, float depth = 0.92f)
        {
            selectItem.Draw(spriteBatch, position + new Vector2i(202, headerHeight + rootContainer.GetHeight() + 5 + 90 + 436), depth - 0.01f);
            selectItemText.Draw(spriteBatch, new Vector2i((selectItem.GetSize().X / 2) - (selectItemText.measureString().X / 2), (selectItem.GetSize().Y / 2) - (selectItemText.measureString().Y / 2)) + position + new Vector2i(202, headerHeight + rootContainer.GetHeight() + 5 + 90 + 436), depth - 0.01f);
        }
        internal void DrawItemIsSelected(SpriteBatch spriteBatch, IPageItem item, float depth = 0.92f)
        {
            if (item.CanGift()) present.Draw(spriteBatch, position + new Vector2i(207, headerHeight + rootContainer.GetHeight() + 5 + 90 + 439), depth);
            else {
                presentDisabled.Draw(spriteBatch, position + new Vector2i(207, headerHeight + rootContainer.GetHeight() + 5 + 90 + 439), depth - 0.01f);
                presentDisabledText.Draw(spriteBatch, new Vector2i((presentDisabled.Size.X / 2) - (presentDisabledText.measureString().X / 2), (presentDisabled.Size.Y / 2) - (presentDisabledText.measureString().Y / 2)) + position + new Vector2i(207, headerHeight + rootContainer.GetHeight() + 5 + 90 + 439), depth);
            }
            buy.Draw(spriteBatch, position + new Vector2i(387, headerHeight + rootContainer.GetHeight() + 5 + 90 + 439), depth);

            Amount.Draw(spriteBatch, position + new Vector2i(207, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (Amount.measureString().Y / 2))), depth);
            Price.Draw(spriteBatch, position + new Vector2i(387, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (Price.measureString().Y / 2))), depth);

            amountText.Draw(spriteBatch, position + new Vector2i(207 + Amount.measureString().X + 20, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (amountText.measureString().Y / 2))), depth);

            if (item.IsDubbelPriced())
            {
                priceOneText.Draw(spriteBatch, position + new Vector2i(536 - priceCredits.GetTexture().Width - priceTwoText.measureString().X - 5 - priceOneText.measureString().X, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceOneText.measureString().Y / 2))), depth);
                priceCredits.Draw(spriteBatch, position + new Vector2i(536 - priceCredits.GetTexture().Width - priceTwoText.measureString().X, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceCredits.GetTexture().Height / 2))), depth);


                priceTwoText.Draw(spriteBatch, position + new Vector2i(536 - priceTwoText.measureString().X, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceTwoText.measureString().Y / 2))), depth);
                switch (item.GetPricesTypes()[1])
                {
                    case PriceTypes.DUCKETS:
                        priceDuckets.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceDuckets.GetTexture().Height / 2))), depth);
                        break;
                    case PriceTypes.DIAMONDS:
                        priceDiamonds.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceDiamonds.GetTexture().Height / 2))), depth);
                        break;
                    case PriceTypes.GOTW:
                        priceGotw.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceGotw.GetTexture().Height / 2))), depth);
                        break;
                    default:
                        Logger.Error("Unknown price type:", item.GetPricesTypes()[0].ToString());
                        break;
                }

            } else
            {
                priceOneText.Draw(spriteBatch, position + new Vector2i(536 - priceOneText.measureString().X, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33/2) - (priceOneText.measureString().Y / 2))), depth);
                
                switch (item.GetPricesTypes()[0])
                {
                    case PriceTypes.CREDITS:
                        priceCredits.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceCredits.GetTexture().Height / 2))), depth);
                        break;
                    case PriceTypes.DUCKETS:
                        priceDuckets.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceDuckets.GetTexture().Height / 2))), depth);
                        break;
                    case PriceTypes.DIAMONDS:
                        priceDiamonds.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceDiamonds.GetTexture().Height / 2))), depth);
                        break;
                    case PriceTypes.GOTW:
                        priceGotw.Draw(spriteBatch, position + new Vector2i(538, headerHeight + rootContainer.GetHeight() + 5 + 90 + 406 + ((33 / 2) - (priceGotw.GetTexture().Height / 2))), depth);
                        break;
                    default:
                        Logger.Error("Unknown price type:", item.GetPricesTypes()[0].ToString());
                        break;
                }
            }
        }
        internal void UpdateItemSelected(GameTime gameTime, IPageItem item)
        {
            if (item.CanGift()) present.Update(gameTime);
            else {
                presentDisabled.Update(gameTime);
                presentDisabledText.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(presentDisabledText.GetOriginalText()));
                presentDisabledText.Update(gameTime);
            }

            buy.Update(gameTime);

            Amount.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(Amount.GetOriginalText()));
            Price.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(Price.GetOriginalText()));
            amountText.SetText("10");
            amountText.Update(gameTime);
            Amount.Update(gameTime);
            Price.Update(gameTime);

            priceOneText.SetText((item.GetPrices()[0] * int.Parse(amountText.GetText())).ToString());
            priceTwoText.SetText("+ " + (item.GetPrices()[1] * int.Parse(amountText.GetText())).ToString());
            priceOneText.Update(gameTime);
            priceTwoText.Update(gameTime);
        }

        internal void LoadNewTree(int parentId, ref Dictionary<CatalogusPage, int> pages)
        {
            cataPageSections.Clear();
            Logger.Debug("Loading a new root tree for catalogus!");
            SectionCombiner combiner = new SectionCombiner();

            IEnumerable<CatalogusPage> pagesToAdd = (from page in pages where page.Value == parentId select page.Key);
            if (pagesToAdd.Count() > 0)
            {
                Vector2i specialOffset = new Vector2i(0, 0);
                foreach (CatalogusPage page in pagesToAdd)
                {
                    if (page.visible) {
                        IEnumerable<CatalogusPage> subPages = (from subPage in pages where subPage.Key.parentId == page.pageId select subPage.Key);
                        int bodySize = 0;
                        cataPageSections.Add(new CataPageSection(
                            GameScreenManager.Instance.GetContentManager(),
                            page,
                            specialOffset,
                            new Vector2i(177, 20),
                            out bodySize,
                            subPages));
                        specialOffset += new Vector2i(0, bodySize);
                    } else
                    {
                        Logger.Debug(page.caption, "is not visible.");
                    }
                }
            }

            foreach (CataPageSection section in cataPageSections) section.UploadTextures();
            pageTree.SetContent(combiner.AddSections(cataPageSections.ToArray()).GetImages());
            pageTree.SetText(combiner.AddSections(cataPageSections.ToArray()).GetFonts());
            pageTree.texturesChanged = true;
        }

        internal void CreatePages(Dictionary<CatalogusPage, int> pages)
        {
            rootContainer.SetWidth(GetSize().X);
            rootContainer.Reset();
            IEnumerable<CatalogusPage> rootPages = (from page in pages where page.Value == -1 orderby page.Value descending select page.Key);

            foreach (CatalogusPage page in rootPages)
            {
                rootContainer.AddButton(rootPages.Count(), () =>
                {
                    foreach (InMenuButton menuButton in rootContainer.GetButtons())
                        menuButton.Deselect();

                    GameScreenManager.Instance.GetCatalogusManager().OpenPage(page.pageId);
                    Console.WriteLine(page.pageId);
                }, page.caption, false);

            }
            rootContainer.GetButtons().First().SetActive(true, true);
        }

        private int bannerOffset = 90;
        internal void DrawVoucher(SpriteBatch spriteBatch)
        {
            Vector2i specialOffset = new Vector2i(10, bannerOffset + headerHeight + 6 + rootContainer.GetHeight());

            voucherBackdrop.Draw(spriteBatch, offset + position + new Vector2i(200, 399) + specialOffset, 0.91f);
            textField.Draw(spriteBatch, offset + position + new Vector2i(200 + 10, 399 + 25) + specialOffset, 0.92f);
            voucherButton.Draw(spriteBatch, offset + position + new Vector2i(200 + 269, 399 + 25) + specialOffset, 0.92f);
        }
        internal void UpdateVoucher(GameTime gameTime, bool doUpdate)
        {
            if(doUpdate)
            {
                voucherBackdrop.Update(gameTime);
                voucherButton.Update(gameTime);
                textField.Update(gameTime);
            }
        }

        internal void DrawPromotions(SpriteBatch spriteBatch, CatalogusPromotion[] promotions)
        {
            int num = 0;
            int bannerOffset = 90;

            Vector2i specialOffset = new Vector2i(10, bannerOffset + headerHeight + 6 + rootContainer.GetHeight()); //34 container size, 90 banner size, 6 offset of banner to promos
            foreach (CatalogusPromotion promo in promotions)
            {
                promo.SetPosition(specialOffset + offset + position);
                promo.Draw(spriteBatch, 0.91f);

                if (num == 0) specialOffset += new Vector2i(promo.GetSize().X + 8, 0);
                else if (num > 0) specialOffset += new Vector2i(0, promo.GetSize().Y + 7);
                num++;
            }
        }
        internal void SetBanner(Image img)
        {
            this.imgBanner = img;
        }
    }
}
