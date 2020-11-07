using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap
{
    public class FloorPlan
    {
        public string id;

        public Vector3 DoorPos;
        public string door_dir;

        public string heightmap;
        public string public_items;

        public string poolmap;
        public string custom;

        public string wall_height;

        public FloorPlan(Dictionary<string, string> list)
        {
            //Get Stuff From Database
            list.TryGetValue("id", out id);
            list.TryGetValue("door_dir", out door_dir);
            list.TryGetValue("heightmap", out heightmap);
            list.TryGetValue("public_items", out public_items);
            list.TryGetValue("poolmap", out poolmap);
            list.TryGetValue("custom", out custom);
            list.TryGetValue("wall_height", out wall_height);

            string x, y, z = "";
            list.TryGetValue("door_x", out x);
            list.TryGetValue("door_y", out y);
            list.TryGetValue("door_z", out z);

            DoorPos = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        }
    }
}
