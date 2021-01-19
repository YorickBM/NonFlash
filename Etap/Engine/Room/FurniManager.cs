using Etap.Communication.Packets.Outgoing.Rooms.Engine;
using Etap.ImagesCode;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Util;
using Util.ButtonTypes;

namespace Etap.Engine.Room
{
    class FurniManager
    {
        Dictionary<int, BaseItem> baseItems;

        Meubi selectedItem;
        bool hasRights, buyOne; //TODO: Fix buy one from cata button
        Vector2i minimiumSize, size;
        System.Timers.Timer delay;

        HoverButton moveBtn, rotateBtn, pickupBtn, useBtn;
        Button close;
        Font moveFnt, rotateFnt, pickupFnt, useFnt, titleFnt, descFnt, userFnt;
        Image crnLT, crnLB, crnRT, crnRB, edgeT, edgeB, backdrop, divider1, divider2, divider3, preview, userProfile;

        public FurniManager(ContentManager content)
        {
            minimiumSize = new Vector2i(190, 160);
            size = minimiumSize;

            buyOne = false;
            hasRights = false;

            selectedItem = null;
            baseItems = new Dictionary<int, BaseItem>();

            delay = new System.Timers.Timer(200);
            delay.Elapsed += Delay_Elapsed;

            useBtn = new HoverButton(content, "Menu/Furni/button", new Vector2i(3, 1), () => { SelectItem(selectedItem);  }, Color.White);
            pickupBtn = new HoverButton(content, "Menu/Furni/button", new Vector2i(3, 1), 
                () => {
                    RetroEnvironment.GetGame().GetClientManager().SendPacket(new PickupObjectEvent(selectedItem.GetItemId()));
                    SelectItem(null);
                }, Color.White);
            rotateBtn = new HoverButton(content, "Menu/Furni/button", new Vector2i(3, 1), 
                () => { 
                    SelectItem(selectedItem); 
                    RetroEnvironment.GetGame().GetClientManager().SendPacket(new MoveObjectEvent(selectedItem.GetItemId(), (int)selectedItem.GetCoordinate().X, (int)selectedItem.GetCoordinate().Z, (selectedItem.GetRotation() * 2) + 2)); 
                }, Color.White);
            moveBtn = new HoverButton(content, "Menu/Furni/button", new Vector2i(3, 1), 
                () => { 
                    GameScreenManager.Instance.GetRoomManager().SetMoveGhostItem(selectedItem);
                    SelectItem(null);
                }, Color.White);

            close = new Button(content, "Menu/Furni/close", new Vector2i(1, 1), () => { SelectItem(null); });

            crnLT = new Image(content, "Menu/Furni/crn", Vector2.Zero, 1, 0, SpriteEffects.None);
            crnLB = new Image(content, "Menu/Furni/crnB", Vector2.Zero, 1, 0, SpriteEffects.None);
            crnRT = new Image(content, "Menu/Furni/crn", Vector2.Zero, 1, 0, SpriteEffects.FlipHorizontally);
            crnRB = new Image(content, "Menu/Furni/crnB", Vector2.Zero, 1, 0, SpriteEffects.FlipHorizontally);

            edgeT = new Image(content, "Menu/Furni/backdrop", new Vector2(minimiumSize.X - (2 * crnLT.SourceRect.Width), 6));
            edgeB = new Image(content, "Menu/Furni/backdrop", new Vector2(minimiumSize.X - (2 * crnLT.SourceRect.Width), 6));
            backdrop = new Image(content, "Menu/Furni/backdrop", new Vector2(minimiumSize.X, minimiumSize.Y - (2 * edgeT.SourceRect.Height)));
            divider1 = new Image(content, "Menu/Furni/divider", new Vector2(minimiumSize.X - 20, 1));
            userProfile = new Image(content, "Menu/Furni/user", Vector2.Zero);
            divider2 = divider1.Clone() as Image;
            divider3 = divider1.Clone() as Image;

            titleFnt = new Font(content, "Fonts/Furni/title", "N/A", Color.White);
            descFnt = new Font(content, "Fonts/Furni/desc", "N/A", Color.White);
            userFnt = new Font(content, "Fonts/Furni/user", "N/A", Color.White);

            useFnt = new Font(content, "Fonts/Furni/button", "furni.button.use", Color.White);
            pickupFnt = new Font(content, "Fonts/Furni/button", "furni.button.remove", Color.White);
            rotateFnt = new Font(content, "Fonts/Furni/button", "furni.button.rotate", Color.White);
            moveFnt = new Font(content, "Fonts/Furni/button", "furni.button.move", Color.White);
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e) { delay.Enabled = false; }

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

        int previewAreaHeight = 70;
        public void Draw(SpriteBatch spriteBatch, float depth = 1f) 
        {
            Vector2i bottomRightPos = GameScreenManager.Instance.Dimensions - new Vector2i(7, 7 + GameScreenManager.Instance.GetOverlayRenderer().GetHeight());
            Vector2i topLeftMenuPos = bottomRightPos - size;
            if (hasRights) topLeftMenuPos -= new Vector2i(0, 25 + 7);

            if (selectedItem != null)
            {
                crnLT.Draw(spriteBatch, topLeftMenuPos + new Vector2i(0, 0), depth - 0.02f);
                edgeT.Draw(spriteBatch, topLeftMenuPos + new Vector2i(crnLT.SourceRect.Width, 0), depth - 0.02f);
                crnRT.Draw(spriteBatch, topLeftMenuPos + new Vector2i(crnLT.SourceRect.Width + edgeT.SourceRect.Width, 0), depth - 0.02f);
             backdrop.Draw(spriteBatch, topLeftMenuPos + new Vector2i(0, edgeT.SourceRect.Height), depth - 0.02f);
                crnLB.Draw(spriteBatch, topLeftMenuPos + new Vector2i(0, size.Y - crnLB.SourceRect.Height), depth - 0.02f);
                edgeB.Draw(spriteBatch, topLeftMenuPos + new Vector2i(crnLB.SourceRect.Width, size.Y - edgeB.SourceRect.Height), depth - 0.02f);
                crnRB.Draw(spriteBatch, topLeftMenuPos + new Vector2i(crnLB.SourceRect.Width + edgeB.SourceRect.Width, size.Y - crnRB.SourceRect.Height), depth - 0.02f);

                close.Draw(spriteBatch, topLeftMenuPos + new Vector2i(size.X - 8 - close.SourceRect.Width, 8), depth);
                titleFnt.Draw(spriteBatch, topLeftMenuPos + new Vector2i(12, 12), depth - 0.01f);
                divider1.Draw(spriteBatch, topLeftMenuPos + new Vector2i(10, 30), depth - 0.01f);

                preview.Draw(spriteBatch, topLeftMenuPos + new Vector2i(10, 36) + new Vector2i((size.X / 2) - (preview.GetTexture().Width / 2), (previewAreaHeight / 2) - (preview.GetTexture().Height / 2)), depth - 0.01f);
                divider2.Draw(spriteBatch, topLeftMenuPos + new Vector2i(10, 42 + previewAreaHeight), depth - 0.01f);

                descFnt.Draw(spriteBatch, topLeftMenuPos + new Vector2i(12, 42 + previewAreaHeight) + new Vector2i(0, (24 / 2) - (descFnt.measureString().Y / 2)), depth - 0.01f);
                divider3.Draw(spriteBatch, topLeftMenuPos + new Vector2i(10, 66 + previewAreaHeight), depth - 0.01f);

                userProfile.Draw(spriteBatch, topLeftMenuPos + new Vector2i(10, 66 + previewAreaHeight) + new Vector2i(0, (24 / 2) - (userProfile.SourceRect.Height / 2)), depth - 0.01f);
                userFnt.Draw(spriteBatch, topLeftMenuPos + new Vector2i(30, 66 + previewAreaHeight) + new Vector2i(0, (24 / 2) - (userFnt.measureString().Y / 2)), depth - 0.01f);
            }

            if (hasRights && selectedItem != null)
            {
                Vector2i rightTopButtonPosition = bottomRightPos;
                Vector2i offset = new Vector2i(0, 25);

                if (selectedItem.GetData().interactions > 1)
                {
                    offset += new Vector2i(useBtn.SourceRect.Width, 0);
                    useBtn.Draw(spriteBatch, rightTopButtonPosition - offset, depth - 0.02f);
                    useFnt.Draw(spriteBatch, rightTopButtonPosition - offset + new Vector2i((useBtn.SourceRect.Width / 2) - (useFnt.measureString().X / 2), (useBtn.SourceRect.Height / 2) - (useFnt.measureString().Y / 2)), depth - 0.01f);
                    offset += new Vector2i(10, 0);
                }

                offset += new Vector2i(pickupBtn.SourceRect.Width, 0);
                pickupBtn.Draw(spriteBatch, rightTopButtonPosition - offset, depth - 0.02f);
                pickupFnt.Draw(spriteBatch, rightTopButtonPosition - offset + new Vector2i((pickupBtn.SourceRect.Width / 2) - (pickupFnt.measureString().X / 2), (pickupBtn.SourceRect.Height / 2) - (pickupFnt.measureString().Y / 2)), depth - 0.01f);
                offset += new Vector2i(10, 0);

                if (selectedItem.GetData().rotations > 1)
                {
                    offset += new Vector2i(rotateBtn.SourceRect.Width, 0);
                    rotateBtn.Draw(spriteBatch, rightTopButtonPosition - offset, depth - 0.02f);
                    rotateFnt.Draw(spriteBatch, rightTopButtonPosition - offset + new Vector2i((rotateBtn.SourceRect.Width / 2) - (rotateFnt.measureString().X / 2), (rotateBtn.SourceRect.Height / 2) - (rotateFnt.measureString().Y / 2)), depth - 0.01f);
                    offset += new Vector2i(10, 0);
                }

                offset += new Vector2i(moveBtn.SourceRect.Width, 0);
                moveBtn.Draw(spriteBatch, rightTopButtonPosition - offset, depth - 0.02f);
                moveFnt.Draw(spriteBatch, rightTopButtonPosition - offset + new Vector2i((moveBtn.SourceRect.Width / 2) - (moveFnt.measureString().X / 2), (moveBtn.SourceRect.Height / 2) - (moveFnt.measureString().Y / 2)), depth - 0.01f);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (selectedItem != null)
            {
                close.Update(gameTime);

                //TODO: Open user profile button
            }

            if (hasRights && selectedItem != null)
            {
                useBtn.Update(gameTime);
                pickupBtn.Update(gameTime);
                rotateBtn.Update(gameTime);
                moveBtn.Update(gameTime);
            }
        }
        public void UnloadContent()
        {
            moveBtn.UnloadContent();
            rotateBtn.UnloadContent();
            pickupBtn.UnloadContent();
            useBtn.UnloadContent();

            close.UnloadContent();

            crnLT.UnloadContent();
            crnLB.UnloadContent();
            crnRT.UnloadContent();
            crnRB.UnloadContent();

            edgeT.UnloadContent();
            edgeB.UnloadContent();
            backdrop.UnloadContent();

            close.UnloadContent();
            titleFnt.UnloadContent();
            divider1.UnloadContent();

            preview.UnloadContent();
            divider2.UnloadContent();

            descFnt.UnloadContent();
            divider3.UnloadContent();

            userFnt.UnloadContent();
            useFnt.UnloadContent();
            pickupFnt.UnloadContent();
            rotateFnt.UnloadContent();
            moveFnt.UnloadContent();
        }

        public void SelectItem(Meubi meubi) { if(!delay.Enabled) selectedItem = meubi; if (meubi != null) delay.Start(); Update(); }
        public void SetRights(bool value) { hasRights = value; }

        public void Update()
        {
            if (selectedItem == null) return;

            titleFnt.SetText(selectedItem.GetData().name);
            preview = selectedItem.GetFirstTexture().Clone() as Image;

            if ((preview.GetTexture().Height + 20) > previewAreaHeight)
            {
                int oldPreviewHeight = previewAreaHeight;
                previewAreaHeight = preview.GetTexture().Height + 20;
                size += new Vector2i(0, (previewAreaHeight - oldPreviewHeight));

                backdrop.SetSourceSize(new Vector2i(backdrop.SourceRect.Width, backdrop.SourceRect.Height + (previewAreaHeight - oldPreviewHeight)));
            }

            descFnt.SetText(selectedItem.GetData().description);
            userFnt.SetText(selectedItem.GetOwnerName());

            useFnt.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(useFnt.GetOriginalText()));
            pickupFnt.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(pickupFnt.GetOriginalText()));
            rotateFnt.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(rotateFnt.GetOriginalText()));
            moveFnt.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(moveFnt.GetOriginalText()));
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
                                else if (r == 3)
                                {
                                    textures.Add(new MeubiData(r, i, x, y), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + data.classname + "/" + 2 + "_" + i + "_" + x + "_" + y, Vector2.Zero, 1f, 0, SpriteEffects.FlipHorizontally));
                                    if (!ColorData.Compare(data.color, 0, 0, 0))
                                        textures.Add(new MeubiData(r, i, x, y, true), new Image(GameScreenManager.Instance.GetContentManager(), "Client/Items/" + data.classname + "/" + 2 + "_" + i + "_" + x + "_" + y + "_C", Vector2.Zero, 1f, 0, SpriteEffects.FlipHorizontally).SetColor(data.color));
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
