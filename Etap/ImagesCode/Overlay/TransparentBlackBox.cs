using Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overlay
{
    class TransparentBlackBox
    {

        private Image backdrop;

        private Image edgeLeft;
        private Image edgeRight;
        private Image edgeBottom;

        private Image bottomLeft;
        private Image bottomRight;

        private Vector2 Size;

        public TransparentBlackBox(ContentManager content)
        {
            backdrop = new Image(content, "Overlay/OverlayBackGround", Vector2.Zero);

            edgeLeft = new Image(content, "Overlay/OverlayEdge", Vector2.Zero);
            edgeRight = new Image(content, "Overlay/OverlayEdge", Vector2.Zero);
            edgeBottom = new Image(content, "Overlay/OverlayBottom", Vector2.Zero);

            bottomLeft = new Image(content, "Overlay/OverlayCornerBottomLeft", Vector2.Zero);
            bottomRight = new Image(content, "Overlay/OverlayCornerBottomLeft", Vector2.Zero);
        }

        public void LoadContent(Vector2 size, bool Top, bool Bottom, bool Right, bool Left)
        {
            backdrop.resize((int)size.X - 3, (int)size.Y - 4);
            edgeLeft.resize(3, (int)size.Y - (int)bottomLeft.dimensions.Y);
            edgeRight.resize(3, (int)size.Y - (int)bottomLeft.dimensions.Y);
            edgeBottom.resize((int)size.X - (int)(bottomLeft.dimensions.X * 2), 4);

            Size = size;
        }

        public void UnloadContent()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, float layerDepth = 0f)
        {
            backdrop.Draw(spriteBatch, offset, layerDepth);
            edgeLeft.Draw(spriteBatch, offset, layerDepth);
            bottomLeft.Draw(spriteBatch, offset + new Vector2(0, Size.Y - bottomLeft.dimensions.Y), layerDepth);
            edgeBottom.Draw(spriteBatch, offset + new Vector2(bottomLeft.dimensions.X, Size.Y - bottomLeft.dimensions.Y + 2), layerDepth);
            bottomRight.Draw(spriteBatch, offset + new Vector2(Size.X - bottomLeft.dimensions.X, Size.Y - bottomLeft.dimensions.Y), layerDepth, SpriteEffects.FlipHorizontally);
            edgeRight.Draw(spriteBatch, offset + new Vector2(Size.X - bottomLeft.dimensions.X + 2, 0), layerDepth, SpriteEffects.FlipHorizontally);
        }

        public void Update(GameTime gametime)
        {

        }
    }
}
