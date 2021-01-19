using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using static Util.Button;

namespace Etap.Engine.Room
{
    class MeubiData : ICloneable
    {
        public int r;
        public int i;
        public int x;
        public int y;
        public bool c;

        public MeubiData(int rotation, int interaction, int xDim, int yDim, bool colored = false)
        {
            r = rotation;
            i = interaction;
            x = xDim;
            y = yDim;
            c = colored;
        }

        public object Clone()
        {
            return new MeubiData(r, i, x, y, c);
        }

        public static bool operator ==(MeubiData data, MeubiData data1)
        {
            return data.r == data1.r && data.i == data1.i && data.x == data1.x && data.y == data1.y && data.c == data1.c;
        }
        public static bool operator !=(MeubiData data, MeubiData data1)
        {
            return data.r != data1.r || data.i != data1.i || data.x != data1.x || data.y != data1.y && data.c != data1.c;
        }
        public static implicit operator string(MeubiData data)
        {
            return "(" + data.r + "_" + data.i + "_" + data.x + "_" + data.y + "_" + data.c + ")";
        }
    }

    class Meubi
    {
        Coordinate location;
        Vector2 locationVec;
        Image noTexture;
        Image shadow;
        BaseItem baseItem;
        Timer delay;

        MyAction onUse;
        int ownerId = -1;
        string ownerName = String.Empty;
        bool doDraw;

        int rotation = 0;
        int interaction = 0;
        int itemId;
        Vector2 typeOffset = new Vector2(-1, 33);
        IEnumerable<MeubiData> activeTextures;

        public Meubi(Coordinate cord, int itemId = 0)
        {
            location = cord;
            noTexture = new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/CantFindTextureTextures/item", Vector2.Zero);

            this.itemId = itemId;
            doDraw = true;
            baseItem = null;
            locationVec = new Vector2(0, 0);

            delay = new Timer(200);
            delay.Elapsed += Delay_Elapsed;

            onUse = () => { };
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }

        public Meubi(Coordinate cord, Vector2 vec, int itemId = 0)
        {
            location = cord;
            noTexture = new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/CantFindTextureTextures/item", Vector2.Zero);

            this.itemId = itemId;
            baseItem = null;
            locationVec = vec;
        }

        public void SetItemId(int id) { itemId = id; }
        public void SetInteractionState(int state)
        {
            interaction = (state - 1);
            if (interaction < 0) interaction = 0;
            try { if (interaction >= baseItem.getTypeData().interactions) interaction = 0; } catch { Logger.Error("Could not update interactionState for:", itemId); interaction = 0; }
            UpdateTextures();
        }
        public void SetRotationState(int state)
        {
            rotation = (state / 2);
            if (rotation < 0) rotation = 0;
            try { if (rotation >= baseItem.getTypeData().rotations) rotation = 0; } catch { Logger.Error("Could not update rotationState for:", itemId); rotation = 0; }
            UpdateTextures();
        }

        internal void Update(GameTime gameTime, Vector2 offset)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            Vector2 offsetExtra = Vector2.Zero;
            if (activeTextures == null || baseItem == null || baseItem.getTextures().Count() < 1) offsetExtra = typeOffset;
            else if (activeTextures.Count() > 0) if (!GetOffetByMeubiData(activeTextures.First(), out offsetExtra)) offsetExtra = new Vector2((int)baseItem.getTypeData().offsetX, (int)baseItem.getTypeData().offsetY);
            Vector2 deduct = new Vector2(0, GetActiveTexture().Size.Y);

            Rectangle hitbox = new Rectangle(
                (int)(GetCoordinate().twoDPosition().X + offset.X + offsetExtra.X - deduct.X), 
                (int)(GetCoordinate().twoDPosition().Y + offset.Y + offsetExtra.Y - deduct.Y), 
                GetActiveTexture().GetTexture().Width, 
                GetActiveTexture().GetTexture().Height);

            if (hitbox.Contains(mousePoint))
            {
                bool isClicked = mouseState.LeftButton == ButtonState.Pressed;
                if (isClicked && !delay.Enabled)
                    if(CheckPixelNotTransparent(mousePoint.ToVector2(), offset + offsetExtra - deduct))
                    {
                        delay.Start();
                        GameScreenManager.Instance.GetFurniManager().SelectItem(this);
                    }
            }
        }

        private bool CheckPixelNotTransparent(Vector2 mousePosition, Vector2 offset)
        {
            Vector2 pixelPosition = Vector2.Zero;
            uint[] PixelData = new uint[1];

            // Get Mouse position relative to top left of Texture
            pixelPosition = mousePosition - (GetCoordinate().twoDPosition() + offset);

            if (pixelPosition.X >= 0 && pixelPosition.X < GetActiveTexture().GetTexture().Width &&
                pixelPosition.Y >= 0 && pixelPosition.Y < GetActiveTexture().GetTexture().Height) {

                // Get the Texture Data within the Rectangle coords, in this case a 1 X 1 rectangle
                // Store the data in pixelData Array
                GetActiveTexture().GetTexture().GetData<uint>(0, new Rectangle((int)pixelPosition.X, (int)pixelPosition.Y, (1), (1)), PixelData, 0, 1);

                // Check if pixel in Array is non Alpha, give or take 20
                if (((PixelData[0] & 0xFF000000) >> 24) > 20)
                    return true;
                return false;
            }
            return false;
        }

        public Image GetActiveTexture()
        {
            if (activeTextures == null || baseItem == null || baseItem.getTextures().Count() < 1) return noTexture;

            if (activeTextures.Count() > 0)
            {
                Image s;
                baseItem.getTextures().TryGetValue(activeTextures.First(), out s);
                if (s == null) return noTexture;
                else return s;
            }

            return noTexture;
        }

        public void Hide()
        {
            doDraw = false;
        }
        public void Show()
        {
            doDraw = true;
        }

        public Image GetFirstTexture()
        {
            if (baseItem == null || baseItem.getTextures().Count() < 1) return noTexture;
            return baseItem.getTextures().First().Value;
        }
        public void UpdateTextures()
        {
            activeTextures = (from texture in baseItem.getTextures() where texture.Key.r == rotation && texture.Key.i == interaction select texture.Key);
        }

        //r_i_x_y(_l) -> rotation.interaction,width,length(,layer)
        public void SetBaseItem(BaseItem baseItem)
        {
            this.baseItem = baseItem;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 offset, float extraDepth = 0f)
        {
            if (!doDraw) return;

            if (baseItem.getTextures().Count() < 1 || baseItem == null)
            {
                Vector2 deduct = new Vector2(0, noTexture.Size.Y);
                noTexture.Draw(spriteBatch, GetCoordinate().twoDPosition() - deduct + offset + typeOffset, GetCoordinate().Depth() + extraDepth);
                Logger.DebugWarn("No Base Item");
            }
            else
            {
                Vector2 offsetExtra = new Vector2(baseItem.getTypeData().offsetX, baseItem.getTypeData().offsetY);

                if (activeTextures.Count() < 1) {
                    Vector2 deduct = new Vector2(0, noTexture.Size.Y);
                    noTexture.Draw(spriteBatch, GetCoordinate().twoDPosition() - deduct + offset + typeOffset, GetCoordinate().Depth() + extraDepth); 
                } //else Console.WriteLine("Textures Found To Render");

                activeTextures.ToList().ForEach(z => {
                    try
                    {
                        Image s;
                        if (!baseItem.getTextures().TryGetValue(z, out s)) { Logger.DebugWarn("No Texture found for: ", z); }
                        if (!GetOffetByMeubiData(z, out offsetExtra)) offsetExtra = new Vector2((int)baseItem.getTypeData().offsetX, (int)baseItem.getTypeData().offsetY);

                        float depth = GetCoordinate().Depth();
                        if (z.c) depth += 0.01f;

                        Vector2 deduct = new Vector2(0, s.Size.Y);
                         s.Draw(spriteBatch, GetCoordinate().twoDPosition() - deduct + offset + offsetExtra, depth + extraDepth);

                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                });
            }//*/
        }

        internal Furnitype GetData()
        {
            return baseItem.getTypeData();
        }

        internal int GetOwnerId()
        {
            return ownerId;
        }
        internal void SetOwnerId(int value)
        {
            ownerId = value;
        }
        internal string GetOwnerName()
        {
            return ownerName;
        }
        internal void SetOwnerName(string value)
        { 
            ownerName = value;
        }

        internal int GetRotation()
        {
            return rotation;
        }

        internal int GetSpriteId()
        {
            return baseItem.getId();
        }

        internal int GetItemId()
        {
            return itemId;
        }

        public void UnloadContent()
        {
            noTexture.UnloadContent();
            foreach (Image img in baseItem.getTextures().Values)
                img.UnloadContent();
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
        public Coordinate GetCoordinate() { return location; }
        public void SetCoordinate(Coordinate cord) { location = cord; }

        public Vector2 GetLocationVector() { return locationVec; }
        public void SetLocationVector(Vector2 vec) { locationVec = vec; }
    }
}
