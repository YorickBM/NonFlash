using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.ButtonTypes
{
    class ToggleButton : Button
    {
        public ToggleButton(ContentManager content, string path, Vector2i frames, MyAction action) : base(content, path, frames, action)
        {
            this.Enable();
            base.Show();
            loadFrame(0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal int activeFrame = 0;
        public void Toggle()
        {
            activeFrame += 1;
            activeFrame = loadFrame(activeFrame);
        }
    }
}
