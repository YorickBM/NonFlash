﻿namespace Retro.Communication.Packets.Outgoing.Quests
{
	class QuestCompletedCompser : ServerPacket
    {
        public QuestCompletedCompser()
            : base(ServerPacketHeader.QuestCompletedMessageComposer)
        {

        }
    }
}