using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Util;

namespace Etap.Engine.Room
{
    class FurniManager
    {
        Dictionary<int, BaseItem> baseItems;

        public FurniManager()
        {
            baseItems = new Dictionary<int, BaseItem>();
        }

        public BaseItem RegisterFurni(int spriteId)
        {
            BaseItem itm = null;
            if (baseItems.ContainsKey(spriteId)) {
                baseItems.TryGetValue(spriteId, out itm);
                return itm;
            }

            itm = new BaseItem(spriteId);
            baseItems.Add(spriteId, itm);

            return itm;
        }
    }

    class BaseItem : ICloneable
    {
        int spriteId;
        Furnitype data;

        Dictionary<MeubiData, Image> textures;
        Dictionary<MeubiData, Vector2> offsetData;

        int attemptAtReload = 0;

        public BaseItem(BaseItem item)
        {
            spriteId = item.getId();
            data = item.getTypeData();

            textures = new Dictionary<MeubiData, Image>();
            foreach (KeyValuePair<MeubiData, Image> pair in item.getTextures()) textures.Add(pair.Key.Clone() as MeubiData, pair.Value.Clone() as Image);
            offsetData = new Dictionary<MeubiData, Vector2>();
            foreach (KeyValuePair<MeubiData, Vector2> pair in item.getOffsets()) offsetData.Add(pair.Key.Clone() as MeubiData, pair.Value);
        }
        public BaseItem(int id)
        {
            textures = new Dictionary<MeubiData, Image>();
            offsetData = new Dictionary<MeubiData, Vector2>();

            spriteId = id;
            SetSpriteId(id);
        }

        public int getId() { return this.spriteId; }
        public Furnitype getTypeData() { return this.data; }

        public Dictionary<MeubiData, Image> getTextures() { return this.textures; }
        public Dictionary<MeubiData, Vector2> getOffsets() { return this.offsetData; }

        public void SetSpriteId(int id)
        {
            data = GameScreenManager.Instance.GetFurniTypeBySpriteId(id);

            if(data == null)
            {
                Logger.Warn("Sprite Id not found:", id);
                return;
            }
            try
            {
                if (data.id == 0)
                    return;

                for (int r = 0; r < data.rotations; r++)
                    for (int i = 0; i < data.interactions; i++)
                        for (int x = 0; x < data.xdim; x++)
                            for (int y = 0; y < data.ydim; y++)
                            {
                                if (File.Exists(@"Content/Client/Items/" + data.classname + "/" + r + "_" + i + "_" + x + "_" + y + ".xnb"))
                                {
                                    textures.Add(new MeubiData(r, i, x, y), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + data.classname + "/" + r + "_" + i + "_" + x + "_" + y, Vector2.Zero));
                                    if (!ColorData.Compare(data.color, 0, 0, 0)) {
                                        textures.Add(new MeubiData(r, i, x, y, true), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + data.classname + "/" + r + "_" + i + "_" + x + "_" + y + "_C", Vector2.Zero).SetColor(data.color));
                                    }
                                }
                                else if (r == 1)
                                {
                                    textures.Add(new MeubiData(r, i, x, y), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + data.classname + "/" + 0 + "_" + i + "_" + x + "_" + y, Vector2.Zero, 1f, 0, SpriteEffects.FlipHorizontally));
                                    if (!ColorData.Compare(data.color, 0, 0, 0))
                                        textures.Add(new MeubiData(r, i, x, y, true), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + data.classname + "/" + 0 + "_" + i + "_" + x + "_" + y + "_C", Vector2.Zero, 1f, 0, SpriteEffects.FlipHorizontally).SetColor(data.color));
                                }
                                //else Console.WriteLine("Texture not found!");
                                //else textures.Add(new MeubiData(r, i, x, y), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + type.classname + "/" + 0 + "_" + 0 + "_" + x + "_" + y, Vector2.Zero));
                            }

                if (File.Exists(@"Content/Client/Items/" + data.classname + "/offsets.txt"))
                {
                    string text = File.ReadAllText(@"Content/Client/Items/" + data.classname + "/offsets.txt");
                    string[] files = text.Split('\n');

                    foreach (string line in files)
                    {
                        string[] data = line.Split(';');
                        string[] meubiData = data[2].Split('_');

                        try
                        {
                            if (meubiData[4] != null) offsetData.Add(new MeubiData(int.Parse(meubiData[0]), int.Parse(meubiData[1]), int.Parse(meubiData[2]), int.Parse(meubiData[3]), true), new Vector2(int.Parse(data[0]), int.Parse(data[1])));

                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            offsetData.Add(new MeubiData(int.Parse(meubiData[0]), int.Parse(meubiData[1]), int.Parse(meubiData[2]), int.Parse(meubiData[3])), new Vector2(int.Parse(data[0]), int.Parse(data[1])));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var t = new Thread(() => SetSpriteId(id));
                //if (textureLoadIssue) Thread.Sleep(100);
                //else textureLoadIssue = true;

                if (attemptAtReload++ < 10) t.Start();
                else Logger.Error("Could not load Sprite: ", id, " afther ", attemptAtReload, " attempts.\n" + ex);
            }
        }
        public object Clone()
        {
            return new BaseItem(this);
        }
    }
}
