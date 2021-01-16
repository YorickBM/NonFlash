using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ButtonTypes;

namespace Util
{
    class CloseAndInfoUI
    {
        internal Vector2i offset, size;
        internal Vector2i position = new Vector2i(0, 0);
        internal Image leftCornerTop, rightCornerTop, edgeTop, topLeftEdge, topRightEdge;
        internal Image leftCornerBottom, rightCornerBottom, edgeBottom, bottomLeftEdge, bottomRightEdge, bottomBackground;
        internal Font title;
        internal HoverButton CloseB, InfoB;
        internal int headerHeight = 31;

        public CloseAndInfoUI(ContentManager content, Vector2i position, Vector2i size, string title, int offsetX = 0, int offsetY = 0)
        {
            offset = new Vector2i(offsetX, offsetY);
            this.position = position;
            this.size = size;
            leftCornerTop = new Image(content, "Menu/TopBar/TopBarLeftCorner", Vector2.Zero);
            rightCornerTop = new Image(content, "Menu/TopBar/TopBarLeftCorner", Vector2.Zero);
            edgeTop = new Image(content, "Menu/TopBar/TopBarMidLayer", new Vector2(1, 31));
            topLeftEdge = new Image(content, "Menu/TopBar/TopBarLeftLayer", Vector2.Zero);
            topRightEdge = new Image(content, "Menu/TopBar/TopBarLeftLayer", Vector2.Zero);

            leftCornerBottom = new Image(content, "Menu/ContentBar/ContentBarLeftCorner", Vector2.Zero);
            rightCornerBottom = new Image(content, "Menu/ContentBar/ContentBarLeftCorner", Vector2.Zero);
            bottomBackground = new Image(content, "Menu/ContentBar/ContentBarMidLayer", Vector2.Zero);
            edgeBottom = new Image(content, "Menu/ContentBar/ContentBarDownLayer", Vector2.Zero);
            bottomLeftEdge = new Image(content, "Menu/ContentBar/ContentBarLeftLayer", Vector2.Zero);
            bottomRightEdge = new Image(content, "Menu/ContentBar/ContentBarLeftLayer", Vector2.Zero);

            CloseB = new HoverButton(content, "Menu/Buttons/XBttn", new Vector2i(3, 1), () => { Close(); }, Color.White);
            InfoB = new HoverButton(content, "Menu/Buttons/IBttn", new Vector2i(3, 1), () => { Console.WriteLine("Open Info Menu"); }, Color.White);

            this.title = new Font(content, "Fonts/UbuntuRegular", title, Color.White);
            allowRender = false;
        }

        internal Vector2i GetSize() { return size; }

        internal void SetSize(int width, int height)
        {
            size = new Vector2i(width, height);
        }

        private bool allowRender;
        public void Close()
        {
            allowRender = false;
        }
        public void Open()
        {
            allowRender = true;
        }
        public void SetHeaderName(string msg)
        {
            title.SetText(msg);
        }

        public bool isOpen() { return allowRender; }

        public virtual void UnloadContent()
        {
            leftCornerTop.UnloadContent();
            rightCornerTop.UnloadContent();
            edgeTop.UnloadContent();
            topLeftEdge.UnloadContent();
            topRightEdge.UnloadContent();

            leftCornerBottom.UnloadContent();
            rightCornerBottom.UnloadContent();
            bottomBackground.UnloadContent();
            edgeBottom.UnloadContent();
            bottomLeftEdge.UnloadContent();
            bottomRightEdge.UnloadContent();

            CloseB.UnloadContent();
            InfoB.UnloadContent();

            title.UnloadContent();
        }

        bool grabbed;
        Point prevMousePoint;
        public Vector2i move()
        {
            Vector2i movement = new Vector2i(0, 0);
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)headerHeight);

            if (rectangle.Contains(mousePoint))
                grabbed = mouseState.LeftButton == ButtonState.Pressed;

            if (mouseState.LeftButton != ButtonState.Pressed && grabbed)
            {
                grabbed = false;
            }

            if (grabbed && prevMousePoint != null)
            {
                Point adjust = mousePoint - prevMousePoint;
                movement = new Vector2i(adjust.X, adjust.Y);
            }

            prevMousePoint = mousePoint;

            if (new Rectangle(0, 0, GameScreenManager.Instance.Dimensions.X, GameScreenManager.Instance.Dimensions.Y).Contains(new Rectangle((int)position.X + movement.X, (int)position.Y + movement.Y, (int)size.X, (int)size.Y)))
                return movement;
            else
                return new Vector2i(0, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            position += move();

            bottomBackground.resize(size.X - (bottomLeftEdge.dimensions.X * 2), size.Y - headerHeight - edgeBottom.dimensions.Y);
            bottomBackground.Update(gameTime);

            leftCornerTop.Update(gameTime);
            rightCornerTop.Update(gameTime);

            edgeTop.resize(size.X - (leftCornerTop.dimensions.X * 2) + 2, headerHeight);
            edgeTop.Update(gameTime);

            topLeftEdge.resize(topLeftEdge.dimensions.X, headerHeight - leftCornerTop.dimensions.Y);
            topLeftEdge.Update(gameTime);

            topRightEdge.resize(topLeftEdge.dimensions.X, headerHeight - leftCornerTop.dimensions.Y);
            topRightEdge.Update(gameTime);

            leftCornerBottom.Update(gameTime);
            rightCornerBottom.Update(gameTime);

            edgeBottom.resize(size.X - (leftCornerBottom.dimensions.X * 2) + 2, edgeBottom.dimensions.Y);
            edgeBottom.Update(gameTime);

            bottomLeftEdge.resize(bottomLeftEdge.dimensions.X, size.Y - headerHeight - leftCornerBottom.dimensions.Y);
            bottomLeftEdge.Update(gameTime);

            bottomRightEdge.resize(bottomLeftEdge.dimensions.X, size.Y - headerHeight - leftCornerBottom.dimensions.Y);
            bottomRightEdge.Update(gameTime);

            CloseB.Update(gameTime);
            InfoB.Update(gameTime);

            title.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (allowRender)
            {
                bottomBackground.Draw(spriteBatch, (position + offset + new Vector2i(bottomLeftEdge.dimensions.X, headerHeight)), 0.8f);

                leftCornerTop.Draw(spriteBatch, (position + offset), 0.9f);
                rightCornerTop.Draw(spriteBatch, (position + offset + new Vector2i(size.X - rightCornerTop.dimensions.X, 0)), 0.9f, SpriteEffects.FlipHorizontally);
                edgeTop.Draw(spriteBatch, (position + offset + new Vector2i(leftCornerTop.dimensions.X - 1, 0)), 0.9f);

                topLeftEdge.Draw(spriteBatch, (position + offset + new Vector2i(0, leftCornerTop.dimensions.Y)), 0.9f);
                topRightEdge.Draw(spriteBatch, (position + offset + new Vector2i(size.X - rightCornerTop.dimensions.X + 1, leftCornerTop.dimensions.Y)), 0.9f, SpriteEffects.FlipHorizontally);

                leftCornerBottom.Draw(spriteBatch, (position + offset + new Vector2i(0, size.Y - leftCornerBottom.dimensions.Y)), 0.9f);
                rightCornerBottom.Draw(spriteBatch, (position + offset + new Vector2i(size.X - rightCornerTop.dimensions.X, size.Y - leftCornerBottom.dimensions.Y)), 0.9f, SpriteEffects.FlipHorizontally);
                edgeBottom.Draw(spriteBatch, (position + offset + new Vector2i(leftCornerBottom.dimensions.X - 1, size.Y - edgeBottom.dimensions.Y)), 0.9f);
                bottomLeftEdge.Draw(spriteBatch, (position + offset + new Vector2i(0, headerHeight)), 0.9f);
                bottomRightEdge.Draw(spriteBatch, (position + offset + new Vector2i(size.X - bottomRightEdge.dimensions.X, headerHeight)), 0.9f, SpriteEffects.FlipHorizontally);

                CloseB.Enable();
                InfoB.Enable();
                CloseB.Draw(spriteBatch, (position + offset + new Vector2i(size.X - leftCornerTop.dimensions.X - (CloseB.dimensions.X / 2) + 8, 2 + (int)(headerHeight / 2 - CloseB.dimensions.Y / 2))), 0.91f);
                InfoB.Draw(spriteBatch, (position + offset + new Vector2i(size.X - leftCornerTop.dimensions.X - 5 - CloseB.dimensions.X + 18, 2 + (int)(headerHeight / 2 - InfoB.dimensions.Y / 2))), 0.91f);

                title.Draw(spriteBatch, position + offset + new Vector2((int)(size.X / 2 - title.measureString().X / 2), 2 + (int)(headerHeight / 2 - title.measureString().Y / 2)), 0.93f);
            } else
            {
                CloseB.Disable();
                InfoB.Disable();
            }
        }

        internal void SetPosition(Vector2i vector2i)
        {
            position = vector2i;
        }
        internal void AddPosition(Vector2i vector2i)
        {
            position = position + vector2i;
            Console.WriteLine(position);
        }
    }
}
