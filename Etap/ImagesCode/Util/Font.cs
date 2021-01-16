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
    class Font : ICloneable
    {
        internal SpriteFont font { get; set; }
        internal Vector2 Position { get; set; }
        internal Color Color { get; set; }
        internal String message { get; set; }
        internal String messageOrginal { get; set; }
        internal Vector2 originPosition { get; set; }

        public Font(Font fnt)
        {
            this.font = fnt.font;
            this.Position = fnt.Position;
            this.Color = fnt.Color;
            this.message = fnt.message;
            this.messageOrginal = fnt.messageOrginal;
            this.originPosition = fnt.originPosition;
        }

        public Font(ContentManager content, String path, String text) : this(content, path, text, Color.White, Vector2.Zero)
        { }

        public Font(ContentManager content, String path, String text, Color color) : this(content, path, text, color, Vector2.Zero)
        { }

        public Font(ContentManager content, String path, String text, Color color, Vector2 position)
        {
            font = content.Load<SpriteFont>(path);
            Position = position;
            message = text;
            messageOrginal = text;
            Color = color;
        }

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

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos, float depthLayer = 0f, SpriteEffects effects = SpriteEffects.None)
        {
            Position = pos;
            Draw(spriteBatch, effects, depthLayer);
        }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects, float depthLayer = 0f)
        {
            if (!hiddenB)
                spriteBatch.DrawString(font, message, new Vector2((int)Position.X, (int)Position.Y), Color, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, depthLayer);
        }

        public void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public bool setTextedIsUsed = false;
        public void SetText(String text)
        {
            if(!setTextedIsUsed)
            {
                setTextedIsUsed = true;
            }
            message = text;
        }
        public void SetColor(Color color)
        {
            Color = color;
        }

        public Vector2 measureString()
        {
            return font.MeasureString(message);
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public string GetText()
        {
            return message;
        }

        public string GetOriginalText()
        {
            return messageOrginal;
        }

        bool hiddenB;
        public void Show() { hiddenB = false; }
        public void Hide() { hiddenB = true; }
        public bool isHidden() { return hiddenB; }

        public object Clone()
        {
            return new Font(this);
        }
    }
}
