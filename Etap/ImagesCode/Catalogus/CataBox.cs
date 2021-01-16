using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Engine.Catalogus
{
    class CataBox
    {

        Image crnTL, crnTR, crnBL, crnBR;
        Image edgeT, edgeL, edgeB, edgeR;
        Image filler;

        Vector2i size;

        public CataBox(ContentManager content, Vector2i size)
        {
            crnTL = new Image(content, "Menu/Catalogus/Box/crnT", Vector2.Zero, 1, 0, SpriteEffects.None);
            crnTR = new Image(content, "Menu/Catalogus/Box/crnT", Vector2.Zero, 1, 0, SpriteEffects.FlipHorizontally);
            crnBL = new Image(content, "Menu/Catalogus/Box/crnB", Vector2.Zero, 1, 0, SpriteEffects.None);
            crnBR = new Image(content, "Menu/Catalogus/Box/crnB", Vector2.Zero, 1, 0, SpriteEffects.FlipHorizontally);

            filler = new Image(content, "Menu/Catalogus/Box/filler", Vector2.Zero, 1, 0, SpriteEffects.None);

            edgeT = new Image(content, "Menu/Catalogus/Box/edgeT", Vector2.Zero, 1, 0, SpriteEffects.None);
            edgeB = new Image(content, "Menu/Catalogus/Box/edgeB", Vector2.Zero, 1, 0, SpriteEffects.None);
            edgeL = new Image(content, "Menu/Catalogus/Box/edgeL", Vector2.Zero, 1, 0, SpriteEffects.None);
            edgeR = new Image(content, "Menu/Catalogus/Box/edgeL", Vector2.Zero, 1, 0, SpriteEffects.FlipHorizontally);

            this.size = size;
        }

        public Vector2i GetSize() { return size; }

        public void UnloadContent()
        {
            crnTL.UnloadContent();
            crnTR.UnloadContent();
            crnBL.UnloadContent();
            crnBR.UnloadContent();

            filler.UnloadContent();

            edgeT.UnloadContent();
            edgeB.UnloadContent();
            edgeL.UnloadContent();
            edgeR.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2i position, float depth = 0.9f)
        {
            crnTL.Draw(spriteBatch, position + new Vector2i(0, 0), depth);
            crnTR.Draw(spriteBatch, position + new Vector2i(size.X - crnTR.GetTexture().Width, 0), depth);
            crnBL.Draw(spriteBatch, position + new Vector2i(0, size.Y - crnBL.GetTexture().Height), depth);
            crnBR.Draw(spriteBatch, position + new Vector2i(size.X - crnTR.GetTexture().Width, size.Y - crnBL.GetTexture().Height), depth);

            filler.Draw(spriteBatch, position + new Vector2i(edgeL.GetTexture().Width, edgeT.GetTexture().Height), depth - 0.01f);

            edgeT.Draw(spriteBatch, position + new Vector2i(crnTL.GetTexture().Width, 0), depth);
            edgeB.Draw(spriteBatch, position + new Vector2i(crnTL.GetTexture().Width, size.Y - edgeB.GetTexture().Height), depth);
            edgeL.Draw(spriteBatch, position + new Vector2i(0, crnTL.GetTexture().Height), depth);
            edgeR.Draw(spriteBatch, position + new Vector2i(size.X - edgeR.GetTexture().Width, crnTL.GetTexture().Height), depth);
        }

        public void Update(GameTime gameTime)
        {
            crnTL.Update(gameTime);
            crnTR.Update(gameTime);
            crnBL.Update(gameTime);
            crnBR.Update(gameTime);

            edgeT.SetSourceSize(new Vector2i(size.X - (crnTL.GetTexture().Width * 2), 3));
            edgeT.Update(gameTime);

            edgeB.SetSourceSize(new Vector2i(size.X - (crnBL.GetTexture().Width * 2), 4));
            edgeB.Update(gameTime);

            edgeL.SetSourceSize(new Vector2i(3, size.Y - (crnTL.GetTexture().Height * 2) - 1));
            edgeL.Update(gameTime);

            edgeR.SetSourceSize(new Vector2i(3, size.Y - (crnTR.GetTexture().Height * 2) - 1));
            edgeR.Update(gameTime);

            filler.SetSourceSize(new Vector2i(size.X - (edgeL.SourceRect.Width * 2), size.Y - (edgeT.SourceRect.Height * 2) - 1));
            filler.Update(gameTime);
        }

    }
}
