using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;
using Etap.Utilities;

namespace Etap.Communication.Packets.Outgoing.Inventory.Achievements
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
        }
    }
}
