
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;

namespace Util
{
    class Button : Image
    {
        public delegate void MyAction();

        public Button(ContentManager content, String path, Vector2i frames, MyAction action) : base(content, path, Vector2.Zero)
        {
            this.frames = frames;
            framesDimensions = new Vector2i((int)(dimensions.X / frames.X), (int)(dimensions.Y / frames.Y));

            if (frames.X > 1) {
                frameRotX = true;
                frameRotY = false;
                offsetRect = base.dimensions.X / frames.X;
            } 
            if (frames.Y > 1) {
                frameRotX = false;
                frameRotY = true;
                offsetRect = base.dimensions.Y / frames.Y;
            }

            loadFrame(0);

            clickAction = action;
            t = new Timer(100);
            t.Elapsed += new ElapsedEventHandler(t_reset);

            this.Enable();
            base.Show();
        }

        internal bool frameRotX = false;
        internal bool frameRotY = false;
        internal float offsetRect = 0f;
        internal Vector2i frames;
        internal Vector2i framesDimensions;

        internal bool enabled;
        public void Disable(bool hide = false) { enabled = false; if (hide) Hide(); }
        public void Enable() { enabled = true;  Show(); }
        public bool isEnabled() { return enabled; }

        
        public override void Hide()
        {
            base.Hide();
        }

        public override void Show()
        {
            base.Show();
        }

        internal bool isClicked = false;
        internal MyAction clickAction;
        internal Timer t;
        internal int ActiveFrame = 0;

        public override void Draw(SpriteBatch spriteBatch, Vector2 pos, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            base.Draw(spriteBatch, pos, depth, effect);
        }

        public virtual new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle((int)base.Position.X, (int)base.Position.Y, (int)framesDimensions.X, (int)framesDimensions.Y);

            if (enabled)
            {
                if (rectangle.Contains(mousePoint))
                {
                    isClicked = mouseState.LeftButton == ButtonState.Pressed;
                }
                else
                {
                    isClicked = false;
                }

                if (isClicked)
                {
                    if (!t.Enabled)
                    {
                        clickAction.Invoke();
                        t.Enabled = true;
                    }
                }
            }
        }

        public int loadFrame(int num)
        {
            if (frameRotX) {
                if (num >= frames.X) num = 0;
                base.SourceRect = new Rectangle((int)(offsetRect * num), 0, framesDimensions.X, framesDimensions.Y);
            }
            if (frameRotY) {
                if (num >= frames.Y) num = 0;
                base.SourceRect = new Rectangle(0, (int)(offsetRect * num), framesDimensions.X, framesDimensions.Y);
            }
            ActiveFrame = num;
            return num;
        }

        public void ResizeFrame(Vector2i newSize)
        {
            framesDimensions = newSize;
        }

        void t_reset(object sender, ElapsedEventArgs e)
        {
            t.Enabled = false;
        }
    }
}
