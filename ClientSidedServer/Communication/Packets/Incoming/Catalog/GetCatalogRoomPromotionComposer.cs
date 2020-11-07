﻿using System.Collections.Generic;

using Retro.Hotel.Rooms;

namespace Retro.Communication.Packets.Outgoing.Catalog
{
	class GetCatalogRoomPromotionComposer : ServerPacket
    {
        public GetCatalogRoomPromotionComposer(List<RoomData> UsersRooms)
            : base(ServerPacketHeader.PromotableRoomsMessageComposer)
        {
			WriteBoolean(true);//wat
			WriteInteger(UsersRooms.Count);//Count of rooms?
            foreach (RoomData Room in UsersRooms)
            {
				WriteInteger(Room.Id);
				WriteString(Room.Name);
				WriteBoolean(true);
            }
        }
    }
}
