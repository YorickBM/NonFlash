using System.Linq;

using Retro.Hotel.Rooms;
using Retro.Hotel.Cache.Type;

namespace Retro.Communication.Packets.Outgoing.Rooms.Settings
{
	class RoomRightsListComposer : ServerPacket
    {
        public RoomRightsListComposer(Room Instance)
            : base(ServerPacketHeader.RoomRightsListMessageComposer)
        {
			WriteInteger(Instance.Id);

			WriteInteger(Instance.UsersWithRights.Count);
            foreach (int Id in Instance.UsersWithRights.ToList())
            {
                UserCache Data = RetroEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                if (Data == null)
                {
					WriteInteger(0);
					WriteString("Unknown Error");
                }
                else
                {
					WriteInteger(Data.Id);
					WriteString(Data.Username);
                }
            }
        }
    }
}
