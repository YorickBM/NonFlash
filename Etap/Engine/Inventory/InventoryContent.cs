using Etap;
using Etap.Communication.Packets.Outgoing.Inventory.Furni;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.ButtonTypes;
using Util.Combiner;

namespace Engine.Inventory
{
    class InventoryContent : CloseUI
    {
        TopButtonsContainer rootContainer;

        HoverConstructedButton openCataButton;
        Image noItemsImg;
        Font noItemsHead, noItemsBody;

        ScrollView itemDisplay;
        internal List<InventoryItemSection> invItemSections;

        Image canTrade, cantTrade, canRecycle, cantRecycle;
        Font selectedItem, amountTrade, amountRecycle;
        HoverConstructedButton placeItemButton;

        public InventoryContent(ContentManager content, Vector2i position, Vector2i size, int offsetX = 0, int offsetY = 0) : base(content, position, size, "Inventory", offsetX, offsetY)
        {
            invItemSections = new List<InventoryItemSection>();

            rootContainer = new TopButtonsContainer(content);
            rootContainer.SetWidth(GetSize().X);

            rootContainer.AddButton(4, () =>
            {
                RetroEnvironment.GetGame().GetClientManager().SendPacket(new RequestFurniInventoryEvent());
                GameScreenManager.Instance.GetInventoryManager().Reset();

                foreach (InMenuButton btn in rootContainer.GetButtons()) btn.Deselect();
            }, "inventory.toplevelview.furni");
            rootContainer.AddButton(4, () =>
            {
                Logger.Debug("Opening Pets");
                GameScreenManager.Instance.GetInventoryManager().Reset();

                foreach (InMenuButton btn in rootContainer.GetButtons()) btn.Deselect();
            }, "inventory.toplevelview.pets");
            rootContainer.AddButton(4, () =>
            {
                Logger.Debug("Opening Badges");
                GameScreenManager.Instance.GetInventoryManager().Reset();

                foreach (InMenuButton btn in rootContainer.GetButtons()) btn.Deselect();
            }, "inventory.toplevelview.badges");
            rootContainer.AddButton(4, () =>
            {
                Logger.Debug("Opening Bots");
                GameScreenManager.Instance.GetInventoryManager().Reset();

                foreach (InMenuButton btn in rootContainer.GetButtons()) btn.Deselect();
            }, "inventory.toplevelview.bots");

            noItemsImg = new Image(content, "Menu/Inventory/noItems", Vector2.Zero);
            noItemsHead = new Font(content, "Fonts/Inventory/EmptyHead", "inventory.empty.head", new Color(222,8,8));
            noItemsBody = new Font(content, "Fonts/Inventory/EmptyBody", "inventory.empty.body", new Color(18, 18, 18));
            openCataButton = new HoverConstructedButton(content, () => { GameScreenManager.Instance.GetCatalogusManager().Open(); }, new Vector2i(140, 50),
                HoverConstructedButtonType.BASIC1,
                new Color[] { new Color(243, 243, 243), new Color(225, 225, 225), new Color(255, 255, 255) },
                new Color[] { new Color(217, 217, 217), new Color(163, 163, 163), new Color(238, 238, 238) },
                "inventory.button.goto.cata", "Fonts/Inventory/EmptyButton");

            itemDisplay = new ScrollView(content, new Vector2i(0, 0), new Vector2i(285, 235), "menu/Navigator/backdrop", null);

            canTrade = new Image(content, "Menu/Inventory/Furni/canTrade", Vector2.Zero);
            cantTrade = new Image(content, "Menu/Inventory/Furni/cantTrade", Vector2.Zero);
            canRecycle = new Image(content, "Menu/Inventory/Furni/canRecycle", Vector2.Zero);
            cantRecycle = new Image(content, "Menu/Inventory/Furni/cantRecycle", Vector2.Zero);

            selectedItem = new Font(content, "Fonts/Inventory/selectedItem", "inventory.item.text.noselected", new Color(0,0,0));
            amountTrade = new Font(content, "Fonts/Inventory/amountTrade", "0", new Color(0, 0, 0));
            amountRecycle = new Font(content, "Fonts/Inventory/amountRecycle", "0", new Color(0, 0, 0));

            placeItemButton = new HoverConstructedButton(content, () => 
            {
                GameScreenManager.Instance.GetRoomManager().DisplayGhostItem(GameScreenManager.Instance.GetInventoryManager().GetSelectedItem());
            }, new Vector2i(150, 22), 
                HoverConstructedButtonType.BASIC1,
                new Color[] { new Color(243, 243, 243), new Color(225, 225, 225), new Color(255, 255, 255) },
                new Color[] { new Color(217, 217, 217), new Color(163, 163, 163), new Color(238, 238, 238) },
                "inventory.item.button.placeInRoom", "Fonts/Inventory/placeButton");
        }

        public void Initialize()
        {
            rootContainer.GetButtons().First().Trigger();
            rootContainer.GetButtons().First().SetActive(true);
            Open();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rootContainer.Update(gameTime, size, headerHeight, edgeBottom.dimensions.Y);
        }
        public void ForceUpdateItems()
        {
            itemDisplay.texturesChanged = true;
        }

        public override void Draw(SpriteBatch spriteBatch, float depth = 0.91f)
        {
            base.Draw(spriteBatch, depth - 0.01f);
            rootContainer.Draw(spriteBatch, position, offset, headerHeight, depth);
        }

        public void DrawSelectedItem(SpriteBatch spriteBatch, Furni item, float depth = 0.92f)
        {
            if (item.CanTrade()) canTrade.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X, 0), depth);
            else cantTrade.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X, 0), depth);

            if (item.CanRecycle()) canRecycle.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X, 3 + canTrade.GetTexture().Height), depth);
            else cantRecycle.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X, 3 + canTrade.GetTexture().Height), depth);

            if (item.CanTrade()) amountTrade.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X + canTrade.GetTexture().Width + 3, 0), depth);
            if (item.CanRecycle()) amountRecycle.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X + canRecycle.GetTexture().Width + 3, 3 + canTrade.GetTexture().Height), depth);

            selectedItem.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X, itemDisplay.GetSize().Y - placeItemButton.GetSize().Y - selectedItem.measureString().Y - 10), depth);
            placeItemButton.Draw(spriteBatch, itemDisplay.GetPosition() + new Vector2i(12 + itemDisplay.GetSize().X, itemDisplay.GetSize().Y - placeItemButton.GetSize().Y), depth);
        }
        public void UpdateSelectedItem(GameTime gameTime, Furni item)
        {
            if (item.CanTrade()) canTrade.Update(gameTime);
            else cantTrade.Update(gameTime);

            if (item.CanRecycle()) canRecycle.Update(gameTime);
            else cantRecycle.Update(gameTime);

            selectedItem.SetText(item.GetFurniName());
            placeItemButton.Update(gameTime);

            amountTrade.SetText(GameScreenManager.Instance.GetInventoryManager().GetItemAmount(item.GetFurniClass()).ToString());
            amountTrade.Update(gameTime);

            amountRecycle.SetText(GameScreenManager.Instance.GetInventoryManager().GetItemAmount(item.GetFurniClass()).ToString());
            amountRecycle.Update(gameTime);
        }

        public void DrawNoItems(SpriteBatch spriteBatch, float depth = 0.92f)
        {
            Vector2i bodySize = new Vector2i(size.X, size.Y - (headerHeight + rootContainer.GetHeight()));

            noItemsImg.Draw(spriteBatch, position + 
                new Vector2i((bodySize.X / 4) - (noItemsImg.GetTexture().Width / 2), (bodySize.Y / 2) - (noItemsImg.GetTexture().Height / 2)) + 
                new Vector2i(0, headerHeight + rootContainer.GetHeight()), depth);

            noItemsHead.Draw(spriteBatch, position + 
                new Vector2i(0, (bodySize.Y / 2) - (noItemsImg.GetTexture().Height / 2)) + 
                new Vector2i((bodySize.X / 2) + 20, headerHeight + rootContainer.GetHeight()), depth);

            noItemsBody.Draw(spriteBatch, position +
                new Vector2i(0, (bodySize.Y / 2) - (noItemsImg.GetTexture().Height / 2)) +
                new Vector2i((bodySize.X / 2) + 20, noItemsHead.measureString().Y + headerHeight + rootContainer.GetHeight()), depth);

            openCataButton.Draw(spriteBatch, position + new Vector2i(0, bodySize.Y - openCataButton.GetSize().Y - 10) +
                new Vector2i((bodySize.X / 2) + 20, headerHeight + rootContainer.GetHeight()), depth);
        }
        public void UpdateNoItems(GameTime gameTime)
        {
            noItemsImg.Update(gameTime);

            noItemsHead.SetText(FontUtil.SplitToLines(RetroEnvironment.GetLanguageManager().TryGetValue(noItemsHead.GetOriginalText()), 30));
            noItemsHead.Update(gameTime);

            noItemsBody.SetText(FontUtil.SplitToLines(RetroEnvironment.GetLanguageManager().TryGetValue(noItemsBody.GetOriginalText()), 30));
            noItemsBody.Update(gameTime);

            openCataButton.Update(gameTime);
        }

        public void DrawItems(SpriteBatch spriteBatch, float depth = 0.92f)
        {
            itemDisplay.Draw(spriteBatch, depth - 0.01f);
        }
        public void UpdateItems(GameTime gameTime)
        {
            itemDisplay.Update(gameTime, position + new Vector2i(12, headerHeight + rootContainer.GetHeight() + 30));

            try
            {
                foreach (InventoryItemSection section in invItemSections)
                    section.Update(gameTime, itemDisplay.GetPosition() - itemDisplay.GetScrolled(), ref itemDisplay);
            } catch
            {
                Logger.Warn("Can not render a Inventory Body!! Soo loading???!");
            }

            if (itemDisplay.texturesChanged)
            {
                int cell = 0;
                int row = 0;

                Vector2i offset = new Vector2i(0, 0);
                for (int i = 0; i < invItemSections.Count(); i++)
                {
                    int bodySize = 0;
                    InventoryItemSection section = invItemSections[i];
                    section.UpdatePosition(offset, out bodySize);
                    offset += new Vector2i(2 + 42, 0);

                    if (cell++ >= 6)
                    {
                        cell = 0;
                        row++;
                        offset = new Vector2i(0, (row * (42 + 2)) - 2);
                    }
                }
                SectionCombiner combiner = new SectionCombiner();
                itemDisplay.SetContent(combiner.AddSections(invItemSections.ToArray()).GetImages());
                itemDisplay.SetText(combiner.AddSections(invItemSections.ToArray()).GetFonts());
                itemDisplay.texturesChanged = false;
            }
        }
        internal void UploadFurniture(Dictionary<IInventoryItem, int> items)
        {
            invItemSections.Clear();
            itemDisplay.CleanUp();
            Logger.Debug("Loading inventory items (" + items.Count() + ")");
            SectionCombiner combiner = new SectionCombiner();

            int cell = 0;
            int row = 0;
            Vector2i specialOffset = new Vector2i(0, 0);

            foreach (KeyValuePair<IInventoryItem, int> itemD in items)
            {
                IInventoryItem item = itemD.Key;
                int amount = itemD.Value;

                int bodySize = 0;
                invItemSections.Add(new InventoryItemSection(GameScreenManager.Instance.GetContentManager(), amount, item, specialOffset, new Vector2i(42, 42), out bodySize));

                specialOffset += new Vector2i(2 + 42, 0);
                if (cell++ >= 6)
                {
                    cell = 0;
                    row++;
                    specialOffset = new Vector2i(0, (row * (bodySize + 2)) - 2);
                }
            }

            itemDisplay.SetContent(combiner.AddSections(invItemSections.ToArray()).GetImages());
            itemDisplay.SetText(combiner.AddSections(invItemSections.ToArray()).GetFonts());
            itemDisplay.texturesChanged = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            rootContainer.UnloadContent();

            noItemsImg.UnloadContent();
            noItemsHead.UnloadContent();
            noItemsBody.UnloadContent();
            openCataButton.UnloadContent();

            itemDisplay.UnloadContent();
            invItemSections.Clear();

            canTrade.UnloadContent();
            cantTrade.UnloadContent();
            canRecycle.UnloadContent();
            cantRecycle.UnloadContent();

            selectedItem.UnloadContent();
            amountTrade.UnloadContent();
            amountRecycle.UnloadContent();

            placeItemButton.UnloadContent();
        }
    }
}
