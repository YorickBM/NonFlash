using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    class ScrollView
    {
        private Vector2i position, scollSize, scrolled;
        private Rectangle view;
        private RenderTarget2D viewTexture;
        private List<Image> images;
        private SpriteBatch tempBatch;

        public ScrollView(ContentManager content, Vector2i position, Vector2i size, params Image[] images)
        {
            this.position = position;
            this.scollSize = size;
            this.scrolled = new Vector2i(0, 0);
            this.view = new Rectangle(position.X + scrolled.X, position.Y + scrolled.Y, scollSize.X, scollSize.Y);

            this.images = images.ToList();
            Vector2i totalSize = new Vector2i(0, 0);
            foreach (Image img in images) totalSize += new Vector2i(img.Size.X, img.Size.Y);

            viewTexture = new RenderTarget2D(GameScreenManager.Instance.GraphicsDevice, GameScreenManager.Instance.Dimensions.X, GameScreenManager.Instance.Dimensions.Y);
            tempBatch = new SpriteBatch(GameScreenManager.Instance.GraphicsDevice);

            Console.WriteLine("Size To Spare: " + (totalSize - size).X + ";" + (totalSize - size).Y);
        }

        public virtual void UnloadContent()
        {
            viewTexture.Dispose();
            foreach (Image img in images) img.UnloadContent();

            tempBatch.Dispose();
        }

        public virtual void Update(GameTime gameTime)
        {
            //foreach (Image img in images) img.SetPosition(position);

            this.view = new Rectangle(position.X + scrolled.X, position.Y + scrolled.Y, scollSize.X, scollSize.Y);
        }

        public void createTexture(float depthLayer)
        {
            GameScreenManager.Instance.GraphicsDevice.SetRenderTarget(viewTexture);
            tempBatch.Begin();

            foreach (Image img in images) img.Draw(tempBatch, img.originPosition + position + new Vector2i(0, 0), depthLayer);

            tempBatch.End();
            GameScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float depthLayer = 0.97f)
        {
            createTexture(depthLayer);
            spriteBatch.Draw(viewTexture, position, view, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depthLayer);
        }

        public void SetViewSize(Vector2i size)
        {
            scollSize = size;
        }

        internal void SetScrolled(Vector2i vector2i)
        {
            scrolled = vector2i;
        }

        internal void SetPosition(Vector2i vector2i)
        {
            position = vector2i;
        }
    }
}
