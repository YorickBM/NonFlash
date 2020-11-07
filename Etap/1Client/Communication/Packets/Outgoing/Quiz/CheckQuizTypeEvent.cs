using Retro.Hotel.GameClients;

namespace Retro.Communication.Packets.Incoming.Quiz
{
	class CheckQuizTypeEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            RetroEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SafetyQuizGraduate", 1, false);
        }
    }
}
