﻿using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ButtonTypes;

namespace Util
{
    class ScrollView
    {
        private Vector2i position, scollSize, scrolled, scrollSpace;
        private Rectangle view;
        private RenderTarget2D viewTexture;
        private List<Button> images;
        private SpriteBatch tempBatch;
        private ScrollButtonArrow btnTop, btnBottom;
        private Image backgroundScroller, backgrnd;

        private int scrollWheelPrevValue;

        public ScrollView(ContentManager content, Vector2i position, Vector2i size, string Background, params Button[] images)
        {
            this.position = position;
            this.scollSize = size;
            this.scrolled = new Vector2i(0, 0);
            this.view = new Rectangle(position.X + scrolled.X, position.Y + scrolled.Y, scollSize.X, scollSize.Y);

            this.images = images.ToList();
            Vector2i totalSize = new Vector2i(size.X, 0);
            foreach (Image img in images)
            {
                if (img.Position.X + img.Size.X > totalSize.X) totalSize.X = (int)Math.Floor(img.Position.X + img.Size.X);
                if (img.Position.Y + img.Size.Y > totalSize.Y) totalSize.Y = (int)Math.Floor(img.Position.Y + img.Size.Y);
            }

            scrollSpace = totalSize - size;

            viewTexture = new RenderTarget2D(GameScreenManager.Instance.GraphicsDevice, GameScreenManager.Instance.Dimensions.X, GameScreenManager.Instance.Dimensions.Y);
            tempBatch = new SpriteBatch(GameScreenManager.Instance.GraphicsDevice);

            btnTop = new ScrollButtonArrow(content, () => { SetScrolled(scrolled - new Vector2i(scrollSpace.X * 0.16f, scrollSpace.Y * 0.16f)); });
            btnBottom = new ScrollButtonArrow(content, () => { SetScrolled(scrolled + new Vector2i(scrollSpace.X * 0.16f, scrollSpace.Y * 0.16f)); });
            backgroundScroller = new Image(content, "Menu/Buttons/Scroller/box", new Vector2(17, size.Y - btnTop.framesDimensions.Y - btnBottom.framesDimensions.Y));

            backgrnd = new Image(content, Background, totalSize);
            var mouseState = Mouse.GetState();
            scrollWheelPrevValue = mouseState.ScrollWheelValue;
        }

        public virtual void UnloadContent()
        {
            viewTexture.Dispose();
            foreach (Image img in images) img.UnloadContent();

            tempBatch.Dispose();

            btnBottom.UnloadContent();
            btnTop.UnloadContent();
            backgroundScroller.UnloadContent();

            backgrnd.UnloadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            this.view = new Rectangle(position.X + scrolled.X, position.Y + scrolled.Y, scollSize.X, scollSize.Y);

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            if (view.Contains(mousePoint))
            {
                if(mouseState.ScrollWheelValue != scrollWheelPrevValue)
                {
                    int change = scrollWheelPrevValue - mouseState.ScrollWheelValue;
                    if (change > 0) SetScrolled(scrolled + new Vector2i(scrollSpace.X * 0.16f, scrollSpace.Y * 0.16f));
                    else SetScrolled(scrolled - new Vector2i(scrollSpace.X * 0.16f, scrollSpace.Y * 0.16f));

                }
            }
            scrollWheelPrevValue = mouseState.ScrollWheelValue;

            btnTop.Update(gameTime);
            btnBottom.Update(gameTime);
            backgroundScroller.Update(gameTime);

            backgrnd.Update(gameTime);
        }

        public void createTexture(float depthLayer)
        {
            GameScreenManager.Instance.GraphicsDevice.SetRenderTarget(viewTexture);
            tempBatch.Begin();

            backgrnd.Draw(tempBatch, backgrnd.originPosition + position + new Vector2i(0, 0), depthLayer);
            foreach (Image img in images) img.Draw(tempBatch, img.originPosition + position + new Vector2i(0, 0), depthLayer);

            tempBatch.End();
            GameScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float depthLayer = 0.97f)
        {
            createTexture(depthLayer);
            spriteBatch.Draw(viewTexture, position, view, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depthLayer);

            btnTop.Draw(spriteBatch, position + new Vector2i(scollSize.X, 0), depthLayer);
            btnBottom.Draw(spriteBatch, position + new Vector2i(scollSize.X, scollSize.Y - btnBottom.framesDimensions.Y), depthLayer, SpriteEffects.FlipVertically);
            backgroundScroller.Draw(spriteBatch, position + new Vector2i(scollSize.X, btnTop.framesDimensions.Y), depthLayer);
        }

        public void SetViewSize(Vector2i size)
        {
            scollSize = size;
        }

        internal void SetScrolled(Vector2i vector2i)
        {
            if (vector2i.X > scrollSpace.X) vector2i.X = scrollSpace.X;
            if (vector2i.Y > scrollSpace.Y) vector2i.Y = scrollSpace.Y;
            if (vector2i.X < 0) vector2i.X = 0;
            if (vector2i.Y < 0) vector2i.Y = 0;
            scrolled = vector2i;
        }

        internal void SetPosition(Vector2i vector2i)
        {
            position = vector2i;
        }
    }
}
