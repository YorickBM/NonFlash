using System.Linq;
using Retro.Hotel.Rooms;
using Retro.Hotel.Cache.Type;

namespace Retro.Communication.Packets.Outgoing.Rooms.Settings
{
	class GetRoomBannedUsersComposer : ServerPacket
    {
        public GetRoomBannedUsersComposer(Room Instance)
            : base(ServerPacketHeader.GetRoomBannedUsersMessageComposer)
        {
			WriteInteger(Instance.Id);

			WriteInteger(Instance.GetBans().BannedUsers().Count);//Count
            foreach (int Id in Instance.GetBans().BannedUsers().ToList())
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
