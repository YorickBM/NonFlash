using Engine.Inventory;
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
using Util;
using Util.ButtonTypes;
using Util.Combiner;

namespace Engine.Inventory
{
    class InventoryItemSection : ICombinable
    {
        private List<Image> images;
        private List<Font> fonts;
        private Vector2i size, localPosition;

        private List<Image> headerT;
        private List<Font> headerF;

        private Image icon, backdrop, backdropSelected;
        private Font amountT;

        internal IInventoryItem item;
        internal int amount;
        private Timer delay;

        private bool isClicked = false;

        public InventoryItemSection(ContentManager content, int amount, IInventoryItem item, Vector2i position, Vector2i size, out int bodySize) {
            images = new List<Image>();
            fonts = new List<Font>();
            headerT = new List<Image>();
            headerF = new List<Font>();

            this.size = size;
            this.localPosition = position;
            this.item = item;
            this.amount = amount;

            #region Header
            String classname = item.GetFurniClass();

            Logger.Debug("Generating header for:", classname);

            if (classname != String.Empty)
            {
                if (File.Exists(@"Content/Client/Items/" + classname + "/icon.xnb"))
                    GameScreenManager.Instance.GetCatalogusManager().catalogusImageManager.GetFurniIcon(classname, out icon);
                else icon = new Image(content, @"Client/Items/CantFindTextureTextures/item_small", Vector2.Zero);
            }
            else
            {
                icon = new Image(content, @"Client/Items/CantFindTextureTextures/item_small", Vector2.Zero);
            }
            icon.SetPosition(position + new Vector2i(size.X / 2 - icon.dimensions.X / 2, size.Y / 2 - icon.dimensions.Y / 2));

            amountT = new Font(content, "Fonts/Inventory/itemAmount", amount.ToString(), new Color(47, 105, 130));
            if(amount > 1) headerF.Add(amountT);

            backdrop = new Image(content, "Menu/Inventory/Furni/backdrop", Vector2.Zero);
            backdropSelected = new Image(content, "Menu/Inventory/Furni/backdropSelected", Vector2.Zero);

            headerT.Add(backdrop);
            headerT.Add(icon);
            #endregion

            delay = new Timer(200);
            delay.Elapsed += Delay_Elapsed;


            bodySize = size.Y;
        }

        internal void Select()
        {
            if (headerT.Contains(backdrop)) headerT.Remove(backdrop);
            if (!headerT.Contains(backdropSelected)) headerT.Add(backdropSelected);

            if(headerT.Contains(icon)) headerT.Remove(icon);
            headerT.Add(icon);
        }
        internal void Deselect()
        {
            if (headerT.Contains(backdropSelected)) headerT.Remove(backdropSelected);
            if (!headerT.Contains(backdrop)) headerT.Add(backdrop);

            if (headerT.Contains(icon)) headerT.Remove(icon);
            headerT.Add(icon);
        }

        internal void UpdatePosition(Vector2i position, out int bodySize)
        {
            this.localPosition = position;
            bodySize = size.Y;

            icon.SetPosition(position + new Vector2i(size.X / 2 - icon.dimensions.X / 2, size.Y / 2 - icon.dimensions.Y / 2));
            amountT.SetPosition(position + new Vector2i(size.X - amountT.measureString().X - 4, 6));

            backdrop.SetPosition(position + new Vector2i(2, 2));
            backdropSelected.SetPosition(position + new Vector2i(0, 0));

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
                    ActivateItem();
                    delay.Enabled = true;
                }
            }
        }
        internal void ActivateItem()
        {
            Logger.Debug("Active Item:", item.GetFurniClass());

            GameScreenManager.Instance.GetInventoryManager().SelectItem(item.GetFurniClass());
            GameScreenManager.Instance.GetInventoryManager().GetAllItemSections().ForEach(s => s.Deselect());
            this.Select();
            GameScreenManager.Instance.GetInventoryManager().ForceUpdateItems();
        }
        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        internal void CleanUp()
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
        internal void UnloadContent()
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
