using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etap.Source.EtapEngine.Notifications
{
    public class HotelAlert : MenuWithButton
    {
        public HotelAlert(Vector2 position, String title, String content)
        {
            base.Position(position);
            base.Size(new Vector2(300, 155));
            base.Title(title);
            base.Content(content);
        }

        public HotelAlert(Vector2 position, String title, String content, String imageUrl)
        {
            base.Position(new Vector2(10, 10));
            base.Size(new Vector2(300, 155));
            base.Title(title);
            base.Content(content);
            base.ImageUrl(imageUrl);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
