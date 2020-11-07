using Etap.Communication.Packets;
using Etap.Communication.Packets.Incoming;
using Etap.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Inventory.Achievements
{
    class AchievementUnlockedComposer : IPacketEvent
    {
        /*public AchievementUnlockedComposer(Achievement Achievement, int Level, int PointReward, int PixelReward)
            : base(ServerPacketHeader.AchievementUnlockedMessageComposer)
        {
			WriteInteger(Achievement.Id); // Achievement ID
			WriteInteger(Level); // Achieved level
			WriteInteger(144); // Unknown. Random useless number.
			WriteString(Achievement.GroupName + Level); // Achieved name
			WriteInteger(PointReward); // Point reward
			WriteInteger(PixelReward); // Pixel reward
			WriteInteger(0); // Unknown.
			WriteInteger(10); // Unknown.
			WriteInteger(21); // Unknown. (Extra reward?)
			WriteString(Level > 1 ? Achievement.GroupName + (Level - 1) : string.Empty);
			WriteString(Achievement.Category);
			WriteBoolean(true);
        }*/
        public void Parse(GameClient Session, ClientPacket Packet)
        {
			int achievmentId = Packet.PopInt();
			int level = Packet.PopInt();
			int x0 = Packet.PopInt(); //144
			string achievementName = Packet.PopString();
			int pointReward = Packet.PopInt();
			int pixelReward = Packet.PopInt();
			int x1 = Packet.PopInt(); //0
			int x2 = Packet.PopInt(); //10
			int x3 = Packet.PopInt(); //21
			string prevAchievmentName = Packet.PopString();
			string catagory = Packet.PopString();
			bool x4 = Packet.PopBoolean(); //true
		}
    }
}
