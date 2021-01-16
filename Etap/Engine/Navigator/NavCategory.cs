using Etap.Engine.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Navigator
{
    public class NavCategory
    {
        public List<RoomData> Rooms;
        public string Identifyer;
        public string Name;
        public bool isCollapsed;
        public int ViewMode;

        public NavCategory(string identifyer, string name, bool collapsed, int viewMode)
        {
            Identifyer = identifyer;
            Name = name;
            isCollapsed = collapsed;
            ViewMode = viewMode;
            Rooms = new List<RoomData>();
        }

        public void AddRoom(RoomData room)
        {
            Rooms.Add(room);
        }

        public RoomData GetRoomById(int id)
        {
            IEnumerable<RoomData> thing = (from room in this.Rooms
                                           where room.RoomId == id
                                           orderby room.UsersNow descending
                                           select room);
            return thing.First();
        }
    }
}
