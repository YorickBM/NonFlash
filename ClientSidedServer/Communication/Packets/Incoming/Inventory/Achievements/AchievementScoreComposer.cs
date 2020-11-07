using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Inventory.Achievements
{
	class AchievementScoreComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int achievementScore = Packet.PopInt();
            Logger.DebugWarn("Achievement Score Composer ->", achievementScore);
        }
    }
}
