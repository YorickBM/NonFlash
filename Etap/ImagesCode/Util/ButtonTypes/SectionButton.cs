using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.ButtonTypes
{
    class SectionButton : Button
    {
        public SectionButton(ContentManager content, string path, Vector2i frames, MyAction action) : base(content, path, frames, action)
        {

        }
    }
}
