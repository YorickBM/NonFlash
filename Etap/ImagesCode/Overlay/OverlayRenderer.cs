using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Overlay
{
    class OverlayRenderer
    {
        private BasicInformationContent bic;
        private ToolbarContent tbc;
        private ContentManager content;

        private bool canRender = false;
        public void Show() { canRender = true; }
        public void Hide() { canRender = false; }
        public int GetHeight() { return 50; }

        public OverlayRenderer(ContentManager content)
        {
            this.content = content;
            bic = new BasicInformationContent(content, -195);
            tbc = new ToolbarContent(content, 0, -GetHeight());
        }

        public void UnloadContent()
        {
            bic.UnloadContent();
            tbc.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (canRender)
            {
                bic.Draw(spriteBatch, new Vector2(GameScreenManager.Instance.Dimensions.X, 0));
                tbc.Draw(spriteBatch, new Vector2(0, GameScreenManager.Instance.Dimensions.Y));
            }
        }

        public void Update(GameTime gameTime)
        {
            bic.Update(gameTime);
            tbc.Update(gameTime);
        }
    }
}
