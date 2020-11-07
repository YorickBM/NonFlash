﻿
using Retro.Hotel.Items;

namespace Retro.Communication.Packets.Outgoing.Rooms.Engine
{
	class ObjectRemoveComposer : ServerPacket
    {
        public ObjectRemoveComposer(Item Item, int UserId)
            : base(ServerPacketHeader.ObjectRemoveMessageComposer)
        {
			WriteString(Item.Id.ToString());
			WriteBoolean(false);
			WriteInteger(UserId);
			WriteInteger(0);
        }
    }
}