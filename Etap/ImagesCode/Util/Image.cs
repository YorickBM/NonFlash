using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    class Image : ICloneable
    {
        private Texture2D img { get; set; }

        internal Rectangle SourceRect { get; set; }
        internal Vector2 Origin { get; set; }
        internal float Scale { get; set; }
        internal float Rotation { get; set; }

        internal Vector2i dimensions { get; set; }
        internal Vector2 Position { get; set; }
        internal Vector2 originPosition { get; set; }
        internal Vector2 Size { get; set; }
        private Color color = Color.White;

        bool hiddenB;
        public virtual void Show() { hiddenB = false; }
        public virtual void Hide() { hiddenB = true; }
        public bool isHidden() { return hiddenB; }

        private SpriteEffects initEffect;
        public Image(Image imageC)
        {
            img = imageC.img;
            dimensions = imageC.dimensions;
            initEffect = imageC.initEffect;
            Origin = imageC.Origin;
            Scale = imageC.Scale;
            Rotation = imageC.Rotation;
            Position = imageC.Position;
            originPosition = imageC.originPosition;
            Size = imageC.Size;
            SourceRect = imageC.SourceRect;

            this.SetColor(imageC.GetColor());

            this.Show();
        }
        public Image(ContentManager content, String path, Vector2 size, float scale = 1.0f, float rotation = 0, SpriteEffects effect = SpriteEffects.None)
        {
            img = content.Load<Texture2D>(path);
            dimensions = new Vector2i(img.Width, img.Height);
            if (size != Vector2.Zero){
                Size = size;
                SourceRect = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            } else {
                Size = new Vector2(img.Width, img.Height);
                SourceRect = new Rectangle(0, 0, (int)img.Width, (int)img.Height); 
            }

            initEffect = effect;
            Origin = Vector2.Zero;
            Scale = scale;
            Rotation = rotation;
            Position = Vector2.Zero;
            originPosition = Position;
            //img.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            this.Show();
        }

        internal void SetSize(Vector2i s)
        {
            Size = s;
        }

        internal void SetSourceSize(Vector2i s)
        {
            SourceRect = new Rectangle(0, 0, s.X, s.Y);
        }

        public Image(ContentManager content, RenderTarget2D texture, Vector2 size, float scale = 1.0f, float rotation = 0, SpriteEffects effect = SpriteEffects.None)
        {
            img = texture;
            dimensions = new Vector2i(img.Width, img.Height);

            if (size != Vector2.Zero)
            {
                Size = size;
                SourceRect = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            }
            else
            {
                Size = new Vector2(img.Width, img.Height);
                SourceRect = new Rectangle(0, 0, (int)img.Width, (int)img.Height);
            }

            initEffect = effect;
            Origin = Vector2.Zero;
            Scale = scale;
            Rotation = rotation;
            Position = Vector2.Zero;
            originPosition = Position;

            this.Show();
        }

        public Texture2D GetTexture() { return img; }

        public void AddPosition(Vector2 pos)
        {
            Position += pos;
            originPosition = Position;
        }
        public void SetPosition(Vector2 pos)
        {
            Position = pos;
            originPosition = Position;
        }

        public Image SetColor(Color color)
        {
            this.color = color;
            //Logger.Debug("Color Set To:", this.color.ToString());
            return this;
        }
        public Image SetColor(ColorData color)
        {
            this.color = new Color(color.r, color.g, color.b);
            //Logger.Debug("Color Set To:", this.color.ToString());
            return this;
        }

        public Color GetColor() { return color; }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float depth = 0, SpriteEffects effects = SpriteEffects.None)
        {
            Position = pos;
            Draw(spriteBatch, effects, depth);
        }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects, float depth)
        {
            if(!hiddenB)
                if(initEffect == effects || initEffect == SpriteEffects.None || effects != SpriteEffects.None)
                    spriteBatch.Draw(img, Position, SourceRect, this.color, Rotation, Origin, Scale, effects, depth);
                else
                    spriteBatch.Draw(img, Position, SourceRect, this.color, Rotation, Origin, Scale, initEffect, depth);
        }

        public void UnloadContent()
        {
            img.Dispose();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void resize(Vector2i s)
        {
            SourceRect = new Rectangle(0, 0, s.X, s.Y);
        }
        public void resize(int width, int height) 
        {
            SourceRect = new Rectangle(0, 0, width, height);
        }
        public void resizeCenter(int width, int height)
        {
            SourceRect = new Rectangle((int)((img.Width - (width / Scale)) / 2), (int)((img.Height - (height / Scale)) / 2), (int)(width / Scale), (int)(height / Scale));
        }

        public object Clone()
        {
            return new Image(this);
        }
    }
}
