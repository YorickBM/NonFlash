using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Etap
{
    public class RoomScreen : GameScreen
    {
        [XmlElement("Images")]
        public List<Image> Images;

        Room Room;
        Overlay Overlay;

        public RoomScreen()
        {
            int id = 3;
            RoomManager.Instance.TryGetRoom(id, out Room);
            Room.CreateFloor();
        }

        public override void LoadContent()
        {
            Overlay = LoadedContent.Instance.Overlay;

            base.LoadContent();
            //foreach (Image img in Images)
            //{
            //    img.LoadContent();
            //    img.Position = new Vector2((int)(img.Position.X + ((ScreenManager.Instance.Dimensions.X - img.getWidth()) / 2)), (int)(img.Position.Y + ((ScreenManager.Instance.Dimensions.Y - img.getHeight()) / 2)));
            //}
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            //foreach (Image img in Images) img.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            //foreach (Image img in Images) img.Update(gameTime);
            Overlay.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //foreach (Image img in Images) img.Draw(spriteBatch);
            Overlay.Draw(spriteBatch);
        }
    }
}
