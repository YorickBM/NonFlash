using Retro.Communication.Packets.Incoming;
using Retro.Hotel.GameClients;
using Retro.Utilities;

namespace Retro.Communication.Packets.Outgoing.Inventory.Achievements
{
    class AchievementProgressedComposer : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int id = Packet.PopInt(); // Unknown (ID?)
            int tragetLevel = Packet.PopInt(); // Target level
            string nameordescorbadge = Packet.PopString(); // Target name/desc/badge
            int progressReq = Packet.PopInt(); // Progress req/target 
            int rewardPixels = Packet.PopInt(); // Reward in Pixels
            int rewardAchScore = Packet.PopInt(); // Reward Ach Score
            int i0 = Packet.PopInt(); // ?
            int currentProgress = Packet.PopInt(); // Current progress
            bool completed = Packet.PopBoolean(); // Set 100% completed(??)
            string catagorie = Packet.PopString(); // Category
            string s0 = Packet.PopString(); // Empty String
            int levelAmount = Packet.PopInt(); // Total amount of levels 
            int i1 = Packet.PopInt();

            Logger.DebugWarn("Achievement Progressed Composer ->", id, tragetLevel, nameordescorbadge, progressReq, rewardPixels, rewardAchScore,
                i0, currentProgress, completed, catagorie, s0, levelAmount, i1);
        }
    }
}
