
using Retro.Communication.Packets.Outgoing.GameCenter;

namespace Retro.Communication.Packets.Incoming.GameCenter
{
    class GetPlayableGamesEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();

            Session.SendMessage(new GameAccountStatusComposer(GameId));
            Session.SendMessage(new PlayableGamesComposer(GameId));
            Session.SendMessage(new GameAchievementListComposer(Session, RetroEnvironment.GetGame().GetAchievementManager().GetGameAchievements(GameId), GameId));
        }
    }
}