using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using System.Collections.Generic;

namespace Retro.Communication.Packets.Incoming.Inventory.Achievements
{
	class BadgeDefinitionsComposer: IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int count = Packet.PopInt();

            for(int i = 0; i < count; i++)
            {
                string achievementName = Packet.PopString();
                int levelsCount = Packet.PopInt();
                for(int y = 0; y < levelsCount; y++)
                {
                    int level = Packet.PopInt();
                    int nextLvlRequirement = Packet.PopInt();
                }
            }

            int x0 = Packet.PopInt();
        }
    }
}
