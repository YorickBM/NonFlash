
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;

namespace Util.ButtonTypes
{
    class HoverButton : Button
    {
        internal Color clickColor { get; set; }
        internal Color originColor { get; set; }

        public HoverButton(ContentManager content, String path, Vector2i frames, MyAction action, Color clickColor) : base(content, path, frames, action)
        {
            this.Enable();
            base.Show();
            this.clickColor = clickColor;
            originColor = color;
        }

        internal bool isHovered = false;

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle((int)base.Position.X, (int)base.Position.Y, (int)framesDimensions.X, (int)framesDimensions.Y);

            if (enabled)
            {
                if (rectangle.Contains(mousePoint))
                {
                    isHovered = true;
                    isClicked = mouseState.LeftButton == ButtonState.Pressed;
                }
                else
                {
                    isHovered = false;
                    isClicked = false;
                }

                if (isHovered && !isClicked)
                {
                    if (color != originColor) color = originColor;
                    if (!t.Enabled)
                        loadFrame(2);
                }
                else if (isClicked)
                {
                    loadFrame(1);
                    if (!t.Enabled)
                    {
                        clickAction.Invoke();
                        t.Enabled = true;
                        if (clickColor != null)
                            color = clickColor;
                    }
                }
                else
                {
                    if (!t.Enabled)
                    {
                        if (color != originColor) color = originColor;
                        loadFrame(0);
                    }
                }
            }
        }
    }
}
