

using Retro.Hotel.Games;


namespace Retro.Communication.Packets.Incoming.GameCenter
{
    class Game2GetWeeklyLeaderboardEvent : IPacketEvent
    {
        public void Parse(Hotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();

            GameData GameData = null;
            if (RetroEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
                //Code
            }
        }
    }
}
