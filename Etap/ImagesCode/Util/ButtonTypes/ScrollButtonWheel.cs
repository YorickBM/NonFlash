using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.ButtonTypes
{
    class ScrollButtonWheel : HoverButton
    {
        public ScrollButtonWheel(ContentManager content, MyAction action) : base(content, "Menu/Buttons/Scroller/arrowButton", new Vector2i(3, 1), action, Color.White)
        {

        }
    }
}
