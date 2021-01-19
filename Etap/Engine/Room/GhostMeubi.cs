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
using System.Timers;
using Util;
using static Util.Button;

namespace Etap.Engine.Room
{
    class GhostMeubi
    {
        Image noTexture;
        Image icon;
        BaseItem baseItem;

        Timer delay;
        MyAction close;
        MyAction place;

        int rotation;
        int itemId;
        int spriteId;
        bool alive;

        Vector2 typeOffset;
        IEnumerable<MeubiData> activeTextures;
        Coordinate placeLocation;

        public GhostMeubi(ContentManager content, int itemId, int spriteId, MyAction onClose, MyAction onPlace) {
            this.alive = true;
            noTexture = new Image(content, "Client/Items/CantFindTextureTextures/item", Vector2.Zero);

            this.rotation = 0;
            this.itemId = itemId;
            this.spriteId = spriteId;
            this.baseItem = GameScreenManager.Instance.GetFurniManager().RegisterFurni(spriteId).Clone() as BaseItem;

            this.close = onClose;
            this.place = onPlace;

            if (File.Exists(@"Content/Client/Items/" + baseItem.getTypeData().classname + "/icon.xnb"))
                GameScreenManager.Instance.GetCatalogusManager().catalogusImageManager.GetFurniIcon(baseItem.getTypeData().classname, out icon);
            else icon = new Image(content, @"Client/Items/CantFindTextureTextures/item_small", Vector2.Zero);

            this.typeOffset = new Vector2(-1, 33);

            noTexture.SetColor(new Color(255, 255, 255, 0.5f));
            foreach (Image img in baseItem.getTextures().Values)
                img.SetColor(new Color(255, 255, 255, 0.5f));

            SetRotationState(0);
            delay = new Timer(200);
            delay.Elapsed += Delay_Elapsed;
            delay.Start(); //Make it so they do not insta place
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        public void SetRotationState(int state)
        {
            rotation = (state - 1);
            if (rotation < 0) rotation = 0;
            try { if (rotation >= baseItem.getTypeData().rotations) rotation = 0; } catch { Logger.Error("Could not update rotationState for:", itemId); rotation = 0; }
            UpdateTextures();
        }
        public void UpdateTextures()
        {
            activeTextures = (from texture in baseItem.getTextures() where texture.Key.r == rotation && texture.Key.i == 0 select texture.Key);
        }
        public bool GetOffetByMeubiData(MeubiData data, out Vector2 offset)
        {
            offset = new Vector2(0, 0);
            foreach (KeyValuePair<MeubiData, Vector2> pair in baseItem.getOffsets())
            {
                if (pair.Key == data)
                {
                    offset = pair.Value;
                    return true;
                }
            }
            return false;
        }

        public void UnloadContent()
        {
            noTexture.UnloadContent();
            foreach (Image img in baseItem.getTextures().Values)
                img.UnloadContent();
        }
        public void Update(GameTime gameTime, Vector2 roomOffset, Floor floorDesign)
        {
            if (GameScreenManager.Instance.GetInventoryManager().IsOpen())
                this.alive = false;

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            Tile mouseTile = floorDesign.GetMouseTile(roomOffset);

            bool isClicked = mouseState.LeftButton == ButtonState.Pressed;

            if (mouseTile != null)
            {
                if(isClicked && !delay.Enabled)
                    PlaceItem();
            } else
            {
                if(isClicked && !delay.Enabled)
                    CancelItem();
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 roomOffset, Floor floorDesign, float extraDepth = 0f)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            Tile mouseTile = floorDesign.GetMouseTile(roomOffset);

            if(mouseTile != null)
            {
                DrawBaseItem(spriteBatch, mouseTile.GetCoordinate().twoDPosition(), roomOffset, extraDepth);
            } else
            {
                DrawIcon(spriteBatch, mousePoint.ToVector2(), roomOffset, extraDepth);
            }
        }

        public void PlaceItem()
        {
            delay.Start();
            place.Invoke();
        }
        public void CancelItem()
        {
            delay.Start();
            close.Invoke();
            alive = false;
        }

        private void DrawBaseItem(SpriteBatch spriteBatch, Vector2 position, Vector2 offset, float extraDepth)
        {
            if (baseItem.getTextures().Count() < 1 || baseItem == null)
            {
                Vector2 deduct = new Vector2(0, noTexture.Size.Y);
                noTexture.Draw(spriteBatch, position - deduct + offset + typeOffset, extraDepth);
                Logger.DebugWarn("No Base Item");
            }
            else
            {
                Vector2 offsetExtra = new Vector2(baseItem.getTypeData().offsetX, baseItem.getTypeData().offsetY);

                if (activeTextures.Count() < 1)
                {
                    Vector2 deduct = new Vector2(0, noTexture.Size.Y);
                    noTexture.Draw(spriteBatch, position - deduct + offset + typeOffset, extraDepth);
                } //else Console.WriteLine("Textures Found To Render");

                activeTextures.ToList().ForEach(z => {
                    try
                    {
                        Image s;
                        if (!baseItem.getTextures().TryGetValue(z, out s)) { Logger.DebugWarn("No Texture found for: ", z); }
                        if (!GetOffetByMeubiData(z, out offsetExtra)) offsetExtra = new Vector2((int)baseItem.getTypeData().offsetX, (int)baseItem.getTypeData().offsetY);

                        Vector2 deduct = new Vector2(0, s.Size.Y);
                        s.Draw(spriteBatch, position - deduct + offset + offsetExtra, extraDepth);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                });
            }//*/
        }
        private void DrawIcon(SpriteBatch spriteBatch, Vector2 position, Vector2 offset, float extraDepth)
        {
            icon.Draw(spriteBatch, position - new Vector2(icon.GetTexture().Width / 2, icon.GetTexture().Height), extraDepth);
        }

        public bool IsAlive()
        {
            return this.alive;
        }
        public int GetItemId()
        {
            return itemId;
        }
        public int GetRotation()
        {
            return rotation;
        }
    }
}
