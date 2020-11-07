using System.Collections.Generic;

using Retro.Hotel.Games;

namespace Retro.Communication.Packets.Outgoing.GameCenter
{
	class GameListComposer : ServerPacket
    {
        public GameListComposer(ICollection<GameData> Games)
            : base(ServerPacketHeader.GameListMessageComposer)
        {
			WriteInteger(RetroEnvironment.GetGame().GetGameDataManager().GetCount());//Game count
            foreach (GameData Game in Games)
            {
				WriteInteger(Game.GameId);
				WriteString(Game.GameName);
				WriteString(Game.ColourOne);
				WriteString(Game.ColourTwo);
				WriteString(Game.ResourcePath);
				WriteString(Game.StringThree);
            }
        }
    }
}
