using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using log4net;
using Etap.Utilities;

namespace Etap.Hotel.Games
{
    public class GameDataManager
    {
        private static readonly ILog log = LogManager.GetLogger("Habbie.Hotel.Games.GameDataManager");

        private readonly Dictionary<int, GameData> _games;

        public GameDataManager()
        {
            _games = new Dictionary<int, GameData>();

            Init();
        }

        public void Init()
        {
            if (_games.Count > 0)
                _games.Clear();

            Logger.Info("GameData Manager -> LOADED!");
        }

        public bool TryGetGame(int GameId, out GameData GameData)
        {
            if (_games.TryGetValue(GameId, out GameData))
                return true;
            return false;
        }

        public int GetCount()
        {
            int GameCount = 0;
            foreach (GameData Game in _games.Values.ToList())
            {
                if (Game.GameEnabled)
                    GameCount += 1;
            }
            return GameCount;
        }

        public ICollection<GameData> GameData
        {
            get
            {
                return _games.Values;
            }
        }
    }
}