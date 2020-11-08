using Etap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Util.Button;

namespace Util.ButtonTypes
{

    class DropdownItem
    {
        public string text;
        public MyAction action;

        public DropdownItem(string text, MyAction clickAction)
        {
            this.text = text;
            action = clickAction;
        }
    }

    class DropdownButton
    {
        private Image arrow, dotline;
        private Image cornerLeftTop, cornerLeftBottom, cornerRightTop, cornerRightBottom;
        private Image edgeLeft, edgeRight, edgTop, edgeBottom, backdrop;
        private Image hover, active;

        private Vector2i position, size, offset;
        private Font display;

        private List<DropdownItem> items;
        private List<Font> itemsText;
        private Timer delay;

        private int selectedIndex = 0;
        private int hoverIndex = 0;
        private bool clicked = false;

        public DropdownButton(ContentManager content, int width, Vector2i pos, params DropdownItem[] items) : this(content, width, pos, "Menu/Buttons/Dropdown/", items) { }
        public DropdownButton(ContentManager content, int width, Vector2i pos, string file, params DropdownItem[] items)
        {
            cornerLeftTop = new Image(content, file + "CornerLeftTop", Vector2.Zero);
            cornerLeftBottom = new Image(content, file + "CornerLeftBottom", Vector2.Zero);
            cornerRightTop = new Image(content, file + "CornerRightTop", Vector2.Zero);
            cornerRightBottom = new Image(content, file + "CornerRightBottom", Vector2.Zero);

            position = pos;
            offset = new Vector2i(0, 0);
            size = new Vector2i(width, 24);

            edgeLeft = new Image(content, file + "EdgeLeft", new Vector2(2, size.Y - cornerLeftTop.dimensions.Y - cornerLeftBottom.dimensions.Y));
            edgeRight = new Image(content, file + "EdgeRight", new Vector2(2, size.Y - cornerLeftTop.dimensions.Y - cornerLeftBottom.dimensions.Y));

            edgTop = new Image(content, file + "EdgeTop", new Vector2(width - cornerLeftTop.dimensions.X - cornerRightTop.dimensions.X, 2));
            edgeBottom = new Image(content, file + "EdgeBottom", new Vector2(width - cornerLeftBottom.dimensions.X - cornerRightBottom.dimensions.X, 2));
            backdrop = new Image(content, file + "Backdrop", size - new Vector2(edgeLeft.dimensions.X + edgeRight.dimensions.X, edgTop.dimensions.Y + edgeBottom.dimensions.Y));

            arrow = new Image(content, file + "Arrow", Vector2.Zero);
            dotline = new Image(content, file + "Dotline", Vector2.Zero);

            active = new Image(content, file + "Selected", Vector2.Zero);
            hover = new Image(content, file + "HoverActive", Vector2.Zero);

            this.items = new List<DropdownItem>();
            this.itemsText = new List<Font>();
            delay = new Timer(100);
            delay.Elapsed += new ElapsedEventHandler(delay_reset);

            foreach (DropdownItem item in items) this.items.Add(item);
            foreach (DropdownItem item in this.items) this.itemsText.Add(new Font(content, "Fonts/DropdownButton", item.text, Color.Black));

            display = new Font(content, "Fonts/DropdownButton", items[selectedIndex].text, Color.Black);
        }

        public virtual void UnloadContent()
        {
            cornerLeftTop.UnloadContent();
            cornerLeftBottom.UnloadContent();
            cornerRightTop.UnloadContent();
            cornerRightBottom.UnloadContent();
            edgeLeft.UnloadContent();
            edgeRight.UnloadContent();
            edgTop.UnloadContent();
            edgeBottom.UnloadContent();
            backdrop.UnloadContent();
            arrow.UnloadContent();
            dotline.UnloadContent();

            active.UnloadContent();
            hover.UnloadContent();

            display.UnloadContent();
            foreach (Font font in itemsText) font.UnloadContent();
        }

        private bool isHover;
        public virtual void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (!clicked)
            {
                var rectangle = new Rectangle(position.X, position.Y, size.X, size.Y);

                if(rectangle.Contains(mousePoint))
                {
                    if (rectangle.Contains(mousePoint))
                    {
                        isHover = true;
                        if(delay.Enabled == false && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            delay.Start();
                            clicked = true;
                        }
                    }
                    else
                    {
                        isHover = false;
                    }
                }

                edgeRight.resize(2, size.Y - cornerLeftTop.dimensions.Y - cornerLeftBottom.dimensions.Y);
                edgeLeft.resize(2, size.Y - cornerLeftTop.dimensions.Y - cornerLeftBottom.dimensions.Y);
                backdrop.resize(size.X - (edgeLeft.dimensions.X + edgeRight.dimensions.X), size.Y - (edgTop.dimensions.Y + edgeBottom.dimensions.Y));
                offset = new Vector2i(0, 0);
                hoverIndex = -1;
            } else
            {
                var rectangle = new Rectangle(position.X, position.Y, size.X + offset.X, size.Y + offset.Y);
                if (!rectangle.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed) clicked = false;
                if (rectangle.Contains(mousePoint))  {
                    hoverIndex = -1;
                    for (int i = 0; i < items.Count(); i++)
                    {
                        var recItem = new Rectangle(position.X, position.Y + (16 * i), size.X, 16);
                        if (recItem.Contains(mousePoint)) {
                            if(delay.Enabled == false && mouseState.LeftButton == ButtonState.Pressed)
                            {
                                clicked = false;
                                selectedIndex = i;
                                items[selectedIndex].action.Invoke();
                                delay.Start();
                            }
                            hoverIndex = i;
                        }
                    }
                }  else hoverIndex = -1;

                active.Update(gameTime);
                hover.Update(gameTime);

                offset = new Vector2i(0, 16 * (items.Count - 1));
                edgeRight.resize(2, size.Y + offset.Y - cornerLeftTop.dimensions.Y - cornerLeftBottom.dimensions.Y);
                edgeLeft.resize(2, size.Y + offset.Y - cornerLeftTop.dimensions.Y - cornerLeftBottom.dimensions.Y);
                backdrop.resize((size.X + offset.X) - (edgeLeft.dimensions.X + edgeRight.dimensions.X), (size.Y + offset.Y) - (edgTop.dimensions.Y + edgeBottom.dimensions.Y));
            }

            //Basic Updates
            cornerLeftTop.Update(gameTime);
            cornerLeftBottom.Update(gameTime);
            cornerRightTop.Update(gameTime);
            cornerRightBottom.Update(gameTime);
            edgeLeft.Update(gameTime);
            edgeRight.Update(gameTime);
            edgTop.Update(gameTime);
            edgeBottom.Update(gameTime);
            backdrop.Update(gameTime);
            arrow.Update(gameTime);
            dotline.Update(gameTime);

            if (RetroEnvironment.GetLanguageManager() != null) display.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(items[selectedIndex].text));
            display.Update(gameTime);

            if (RetroEnvironment.GetLanguageManager() != null) for (int i = 0; i < items.Count(); i++) itemsText[i].SetText(RetroEnvironment.GetLanguageManager().TryGetValue(items[i].text));
            foreach (Font font in itemsText) font.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float depthLayer = 0.96f)
        {
            backdrop.Draw(spriteBatch, position + new Vector2i(edgeLeft.dimensions.X, edgTop.dimensions.Y), depthLayer, SpriteEffects.None);
            cornerLeftTop.Draw(spriteBatch, position, depthLayer, SpriteEffects.None);
            cornerLeftBottom.Draw(spriteBatch, offset + position + new Vector2i(0, size.Y - cornerLeftBottom.dimensions.Y), depthLayer, SpriteEffects.None);
            cornerRightTop.Draw(spriteBatch, position + new Vector2i(size.X - cornerRightTop.dimensions.X, 0), depthLayer, SpriteEffects.None);
            cornerRightBottom.Draw(spriteBatch, offset + position + new Vector2i(size.X - cornerRightBottom.dimensions.X, size.Y - cornerRightBottom.dimensions.Y), depthLayer, SpriteEffects.None);
            edgeLeft.Draw(spriteBatch, position + new Vector2i(0, cornerLeftTop.dimensions.Y), depthLayer, SpriteEffects.None);
            edgeRight.Draw(spriteBatch, position + new Vector2i(size.X - edgeRight.dimensions.X, cornerRightTop.dimensions.Y), depthLayer, SpriteEffects.None);
            edgTop.Draw(spriteBatch, position + new Vector2i(cornerLeftTop.dimensions.X, 0), depthLayer, SpriteEffects.None);
            edgeBottom.Draw(spriteBatch, offset + position + new Vector2i(cornerLeftBottom.dimensions.X, size.Y - edgeBottom.dimensions.Y), depthLayer, SpriteEffects.None);

            if (!clicked)
            {
                dotline.Draw(spriteBatch, position + new Vector2i(size.X - 20 - dotline.dimensions.X, 4), depthLayer + 0.01f, SpriteEffects.None);
                arrow.Draw(spriteBatch, position + new Vector2i(size.X - 7 - arrow.dimensions.X, 10), depthLayer + 0.01f, SpriteEffects.None);

                display.Draw(spriteBatch, position + new Vector2i(10, size.Y / 2 - display.measureString().Y / 2), depthLayer + 0.01f, SpriteEffects.None);
            }
            else
            {
                Vector2i personalOffsetFont = new Vector2i(0, 0);
                Vector2i activeIndexPos = new Vector2i(0, 0);
                int index = 0;
                foreach (Font font in itemsText)
                {
                    if (index++ == selectedIndex) activeIndexPos = personalOffsetFont;
                    font.Draw(spriteBatch, personalOffsetFont +  position + new Vector2i(10, size.Y / 2 - font.measureString().Y / 2), depthLayer + 0.02f, SpriteEffects.None);
                    personalOffsetFont += new Vector2i(0, 16);
                }

                active.Draw(spriteBatch, position + activeIndexPos + new Vector2i(4, cornerLeftTop.dimensions.X), depthLayer + 0.015f, SpriteEffects.None);
                if(hoverIndex >= 0)
                    hover.Draw(spriteBatch, position + new Vector2i(0, 16 * hoverIndex) + new Vector2i(4, cornerLeftTop.dimensions.X), depthLayer + 0.015f, SpriteEffects.None);
            }
        }

        public void SetPosition(Vector2i pos)
        {
            position = pos;
        }

        private void delay_reset(object sender, ElapsedEventArgs e)
        {
            delay.Enabled = false;
        }
    }
}
