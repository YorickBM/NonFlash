using Etap;
using Etap.Communication.Packets.Outgoing.Inventory.Furni;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Util;

namespace Engine.Inventory
{
    class InventoryManager
    {
        InventoryContent Inventory;

        Dictionary<int, Furni> furnis;
        Dictionary<IInventoryItem, int> items;
        Furni selectedItem;

        Timer UpdateDelay;
        bool firstSelect;

        public InventoryManager(ContentManager content)
        {
            Inventory = new InventoryContent(content, new Vector2i(0, 0), new Vector2i(490, 342));
            items = new Dictionary<IInventoryItem, int>();
            furnis = new Dictionary<int, Furni>();

            UpdateDelay = new Timer(50);
            UpdateDelay.Elapsed += UpdateDelay_Elapsed;

            firstSelect = true;
        }

        private void UpdateDelay_Elapsed(object sender, ElapsedEventArgs e)
        {
            Inventory.UploadFurniture(items);
            UpdateDelay.Enabled = false;
            if (firstSelect)
            {
                Inventory.invItemSections.First().ActivateItem();
                firstSelect = false;
            }
        }

        public void Toggle()
        {
            if (Inventory.isOpen()) Close();
            else Open();
        }
        internal void Open()
        {
            firstSelect = true;
            Inventory.Initialize();
        }
        internal void Close()
        {
            Inventory.Close();
        }

        internal List<InventoryItemSection> GetAllItemSections()
        {
            return Inventory.invItemSections;
        }

        internal void ForceUpdateItems()
        {
            Inventory.ForceUpdateItems();
        }

        public void UnloadContent()
        {
            Inventory.UnloadContent();
        }

        public void SelectItem(int itemId)
        {
            furnis.TryGetValue(itemId, out selectedItem);
        }

        public void Reset()
        {
            furnis.Clear();
            items.Clear();
            selectedItem = null;
            firstSelect = true;
        }

        public int GetItemAmount(string className)
        {
            foreach (KeyValuePair<IInventoryItem, int> itm in items)
            {
                if (itm.Key.GetFurniClass().Equals(className))
                    return itm.Value;
            }
            return -1;
        }

        public void AddFurni(Furni item)
        {
            bool found = false;

            furnis.Add(item.GetItemId(), item);

            if (item.CanStack())
            {
                foreach (KeyValuePair<IInventoryItem, int> itm in items)
                {
                    if (itm.Key.GetFurniClass().Equals(item.GetFurniClass()))
                    {
                        items.Remove(itm.Key);
                        items.Add(itm.Key, itm.Value + 1);

                        found = true;
                        return;
                    }
                }
            }

            if(!found)
                items.Add(item, 1);

            if (!UpdateDelay.Enabled) UpdateDelay.Start();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Inventory.isOpen())
            {
                Inventory.Draw(spriteBatch);
                if (items.Count > 0) Inventory.DrawItems(spriteBatch);
                else Inventory.DrawNoItems(spriteBatch);

                if (selectedItem != null) Inventory.DrawSelectedItem(spriteBatch, selectedItem);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Inventory.isOpen())
            {
                Inventory.Update(gameTime);
                if (items.Count > 0) Inventory.UpdateItems(gameTime);
                else Inventory.UpdateNoItems(gameTime);

                if (selectedItem != null) Inventory.UpdateSelectedItem(gameTime, selectedItem);
            }
        }
    }
}
