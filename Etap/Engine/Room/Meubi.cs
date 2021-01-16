using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Util;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etap.ImagesCode;
using System.IO;
using System.Threading;
using Etap.Utilities;

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
        Image noTexture;
        Image shadow;
        BaseItem baseItem;

        int rotation = 0;
        int interaction = 0;
        int itemId;
        Vector2 typeOffset = new Vector2(-1, 33);
        IEnumerable<MeubiData> activeTextures;

        public Meubi(Coordinate cord, int id = 0)
        {
            location = cord;
            noTexture = new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/CantFindTextureTextures/item", Vector2.Zero);

            itemId = id;
            baseItem = null;
        }

        public void SetItemId(int id) { itemId = id; }
        public void SetInteractionState(int state)
        {
            interaction = (state - 1);
            if (interaction < 0) interaction = 0;
            try { if (interaction >= baseItem.getTypeData().interactions) interaction = 0; } catch (Exception ex) { Logger.Error("Could not update interactionState for:", itemId); interaction = 0; }
            UpdateTextures();
        }
        public void SetRotationState(int state)
        {
            rotation = (state - 1);
            if (rotation < 0) rotation = 0;
            try { if (rotation >= baseItem.getTypeData().rotations) rotation = 0; } catch (Exception ex) { Logger.Error("Could not update rotationState for:", itemId); rotation = 0; }
            UpdateTextures();
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
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (baseItem.getTextures().Count() < 1 || baseItem == null)
            {
                Vector2 deduct = new Vector2(0, noTexture.Size.Y);
                noTexture.Draw(spriteBatch, GetCoordinate().twoDPosition() - deduct + offset + typeOffset, GetCoordinate().Depth());
                //Console.WriteLine("No Base Item");
            }
            else
            {
                Vector2 offsetExtra = new Vector2(baseItem.getTypeData().offsetX, baseItem.getTypeData().offsetY);

                if (activeTextures.Count() < 1) {
                    Vector2 deduct = new Vector2(0, noTexture.Size.Y);
                    noTexture.Draw(spriteBatch, GetCoordinate().twoDPosition() - deduct + offset + typeOffset, GetCoordinate().Depth()); 
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
                        s.Draw(spriteBatch, GetCoordinate().twoDPosition() - deduct + offset + offsetExtra, depth);

                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
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
    }
}
