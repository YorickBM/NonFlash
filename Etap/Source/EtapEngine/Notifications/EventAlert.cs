using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap
{
    public class EventAlert : MenuWithNoButton
    {
        public EventAlert(Vector2 position, String title, String content)
        {
            base.Position(position);
            base.Size(new Vector2(300, 155));
            base.Title(title);
            base.Content(content);
        }

        public EventAlert(Vector2 position, String title, String content, String imageUrl)
        {
            base.Position(new Vector2(10, 10));
            base.Size(new Vector2(300, 155));
            base.Title(title);
            base.Content(content);
            base.ImageUrl(imageUrl);
        }

        public EventAlert(Vector2 position, String title, String content, String imageUrl, String CustomBtnPath)
        {
            base.Position(new Vector2(10, 10));
            base.Size(new Vector2(300, 155));
            base.Title(title);
            content += "Er is een nieuw evenement gestart in Habbie!\n\nBij dit evenement kan je allerlei prijzen winnen,\nnieuwe vrienden ontdekken en plezier beleven!\n\nEvenement: <b>X</b>\nDoor: <b>X</b>\n\nAls je wilt deelnemen, aan dit evenement kan je op\n de onderstaande knop drukken!";
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

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
