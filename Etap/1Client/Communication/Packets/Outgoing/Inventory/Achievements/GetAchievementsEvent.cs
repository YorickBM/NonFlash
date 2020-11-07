
using System.Linq;


using Retro.Communication.Packets.Outgoing.Inventory.Achievements;

namespace Retro.Communication.Packets.Incoming.Inventory.Achievements
{
    class GetAchievementsEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new AchievementsComposer(Session, RetroEnvironment.GetGame().GetAchievementManager()._achievements.Values.ToList()));
        }
    }
}
