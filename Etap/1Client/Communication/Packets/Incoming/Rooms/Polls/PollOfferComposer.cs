﻿using Retro.Hotel.Rooms.Polls;

namespace Retro.Communication.Packets.Outgoing.Rooms.Polls
{
    class PollOfferComposer : ServerPacket
    {
        public PollOfferComposer(RoomPoll poll)
            : base(ServerPacketHeader.PollOfferMessageComposer)
        {
			WriteInteger(poll.Id);
			WriteString(RoomPollTypeUtility.GetRoomPollType(poll.Type).ToUpper());
			WriteString(poll.Headline);
			WriteString(poll.Summary);
        }
    }
}