using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Engine.Room
{
    public class RoomData
    {
        private int roomId;
        private string name;
        private int ownerId;
        private string ownerName;
        private int acces;
        private int usersNow;
        private int usersMax;
        private string desc;
        private int tradeSettings;
        private int score;
        private int topRated;
        private int category;
        private List<string> tags;
        private int roomType;
        private string image;
        private Group group;
        private RoomEvent roomEvent;

        public RoomData(int roomId, string name, int ownerId, string ownerName, int acces, int usersNow, int usersMax, string desc, int tradeSettings, int score, int topRated, int category, List<string> tags, int roomType, string image, Group group, RoomEvent roomEvent)
        {
            this.RoomId = roomId;
            this.name = name;
            this.ownerId = ownerId;
            this.ownerName = ownerName;
            this.acces = acces;
            this.usersNow = usersNow;
            this.usersMax = usersMax;
            this.desc = desc;
            this.tradeSettings = tradeSettings;
            this.score = score;
            this.topRated = topRated;
            this.category = category;
            this.tags = tags;
            this.roomType = roomType;
            this.image = image;
            this.group = group;
            this.roomEvent = roomEvent;
        }

        public int RoomId { get => roomId; set => roomId = value; }
        public string Name { get => name; set => name = value; }
        public int OwnerId { get => ownerId; set => ownerId = value; }
        public string OwnerName { get => ownerName; set => ownerName = value; }
        public int Acces { get => acces; set => acces = value; }
        public int UsersNow { get => usersNow; set => usersNow = value; }
        public int UsersMax { get => usersMax; set => usersMax = value; }
        public string Desc { get => desc; set => desc = value; }
        public int TradeSettings { get => tradeSettings; set => tradeSettings = value; }
        public int Score { get => score; set => score = value; }
        public int TopRated { get => topRated; set => topRated = value; }
        public int Category { get => category; set => category = value; }
        public List<string> Tags { get => tags; set => tags = value; }
        public int RoomType { get => roomType; set => roomType = value; }
        public string Image { get => image; set => image = value; }
        public Group Group { get => group; set => group = value; }
        public RoomEvent RoomEvent { get => roomEvent; set => roomEvent = value; }
    }
}
