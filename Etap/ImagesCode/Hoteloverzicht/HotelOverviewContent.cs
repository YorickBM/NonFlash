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

namespace Hoteloverzicht
{
    class HotelOverviewContent
    {

        private Image left { get; set; }
        private Image right { get; set; }
        private Image backdrop { get; set; }
        private Image filler { get; set; }

        public HotelOverviewContent(ContentManager content, Vector2 position, int offsetX = 0, int offsetY = 0)
        {
            left = new Image(content, "HotelOverzicht/background_left", Vector2.Zero);
            right = new Image(content, "HotelOverzicht/background_right", Vector2.Zero);
            backdrop = new Image(content, "HotelOverzicht/Backdrop", Vector2.Zero);
            filler = new Image(content, "Client/ToolBarBottom/ToolBar/Background/Filler", new Vector2(GameScreenManager.Instance.Dimensions.X, 50));
        }

        public void UnloadContent()
        {
            left.UnloadContent();
            right.UnloadContent();
            backdrop.UnloadContent();
            filler.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(inOverview)
            {
                backdrop.Draw(spriteBatch, new Vector2(0, 0), 0f);
                left.Draw(spriteBatch, new Vector2(0, GameScreenManager.Instance.Dimensions.Y - left.dimensions.Y - 40), 0.01f);
                right.Draw(spriteBatch, new Vector2(GameScreenManager.Instance.Dimensions.X - right.dimensions.X,
                    GameScreenManager.Instance.Dimensions.Y - right.dimensions.Y - 40), 0.01f);
                filler.Draw(spriteBatch, new Vector2(0, GameScreenManager.Instance.Dimensions.Y - filler.SourceRect.Size.Y), 0.02f);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (inOverview)
            {
                if (GameScreenManager.Instance.Dimensions.X != filler.dimensions.X)
                    filler.resize(GameScreenManager.Instance.Dimensions.X, 50);
                if (GameScreenManager.Instance.Dimensions.X != backdrop.dimensions.X || GameScreenManager.Instance.Dimensions.Y != backdrop.dimensions.Y)
                    backdrop.resize(GameScreenManager.Instance.Dimensions.X, GameScreenManager.Instance.Dimensions.Y);
            }
        }

        public bool inOverview = false;
        public bool isInOverview() { return inOverview; }
        public void ExitOverview() { inOverview = false; }
        public void EnterOverview() { inOverview = true; }
    }
}
