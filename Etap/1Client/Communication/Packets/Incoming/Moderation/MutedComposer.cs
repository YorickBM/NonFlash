﻿using System;

namespace Retro.Communication.Packets.Outgoing.Moderation
{
	class MutedComposer : ServerPacket
    {
        public MutedComposer(Double TimeMuted)
            : base(ServerPacketHeader.MutedMessageComposer)
        {
			WriteInteger(Convert.ToInt32(TimeMuted));
        }
    }
}
