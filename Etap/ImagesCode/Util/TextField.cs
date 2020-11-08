using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    class TextField
    {
        public TextField(Font font, Vector2i pos)
        {

        }

        public virtual void UnloadContent() {
        }

        public virtual void Update(GameTime gamTime) { 
        }

        public virtual void Draw(SpriteBatch spriteBatch) { 
        }

        //https://community.monogame.net/t/text-input-box-again/9454/7
        //Game.Window.TextInput EVENT
        //https://github.com/UnterrainerInformatik/Monogame-Textbox
    }
}
