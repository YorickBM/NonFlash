using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap
{
    public class RoomFurni
    {
        public string id;
        public string user_id;
        public string room_id;
        public string furni_id;

        public string extra_data, limited_number, limited_stack, VinkingThiago;

        public Vector3 position;
        public string rot;

        public RoomFurni(Dictionary<string, string> list)
        {
            list.TryGetValue("id", out id);
            list.TryGetValue("user_id", out user_id);
            list.TryGetValue("room_id", out room_id);
            list.TryGetValue("base_item", out furni_id);

            string x, y, z = "";
            list.TryGetValue("x", out x);
            list.TryGetValue("y", out y);
            list.TryGetValue("z", out z);
            position = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));

            list.TryGetValue("rot", out rot);

            list.TryGetValue("extra_data", out extra_data);
            list.TryGetValue("limited_number", out limited_number);
            list.TryGetValue("limited_stack", out limited_stack);
            list.TryGetValue("VinkingThiago", out VinkingThiago);
        }
    }
}
