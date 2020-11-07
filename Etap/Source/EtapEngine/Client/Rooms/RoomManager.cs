using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap
{
    public class RoomManager
    {
        private static RoomManager instance;
        public static RoomManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoomManager();
                }
                return instance;
            }
        }

        public void TryGetRoom(int id, out Room room)
        {
            ServerClass.Instance.RoomList.TryGetValue(id + "", out room);
        }

        public void CoordsToPos(Vector3 coords, out Vector2 pos)
        {
            pos = new Vector2(coords.X + coords.Z, coords.Y + coords.Z);
        }
    }
}
