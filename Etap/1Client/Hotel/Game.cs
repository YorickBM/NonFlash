using log4net;
using System;
using Etap.Communication.Packets;
using System.Threading;
using Etap.Core;
using Etap.Hotel.GameClients;
using Etap.Hotel.Games;

namespace Etap.HabboHotel
{
    public class Game
    {
        private static readonly ILog log = LogManager.GetLogger("Habbie.Hotel.Game");
        internal bool ClientManagerCycleEnded, RoomManagerCycleEnded;
        private readonly PacketManager _packetManager;
        private readonly GameClientManager _clientManager = null;
        private readonly GameDataManager _gameDataManager;
		private Thread _gameLoop;
        internal static bool GameLoopEnabled = true;
        public static int SessionUserRecord;
        internal bool GameLoopActiveExt { get; private set; }

        public Game()
        {
            //Logger.Info();
            //Logger.Info("» " + RetroEnvironment.PrettyVersion + " starten");
            //Logger.Info();

            SessionUserRecord = 0;
            // Run Extra Settings
           // BotFrankConfig.RunBotFrank();
            ExtraSettings.RunExtraSettings();

            // Run Catalog Settings
            //CatalogSettings.RunCatalogSettings();

            // Run Notification Settings
            NotificationSettings.RunNotiSettings();

            _packetManager = new PacketManager();
            _clientManager = new GameClientManager();

            _gameDataManager = new GameDataManager();
        }

        public void ContinueLoading()
        {
            ServerStatusUpdater.Init();
			StartGameLoop();
        }

        public void StartGameLoop()
        {
            GameLoopActiveExt = true;
			_gameLoop = new Thread(GameCycle)
			{
				Name = "Game Loop"
			};
			_gameLoop.Start();

        }

        private void GameCycle()
        {
            ServerStatusUpdater.StartProcessing();

            while (GameLoopActiveExt)
            {
                if (GameLoopEnabled)
                    try
                    {
                        RoomManagerCycleEnded = false;
                        ClientManagerCycleEnded = false;
                        _clientManager.OnCycle();
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.LogCriticalException(ex);
                    }
                Thread.Sleep(25);
            }
        }

        public void StopGameLoop()
        {
            GameLoopActiveExt = false;
            while (!ClientManagerCycleEnded) //!RoomManagerCycleEnded ||  //No room Manager yet
            {
                Thread.Sleep(25);
            }
        }

        public void Destroy()
        {
            GetClientManager();
            log.WarnFormat("Client Manager destroyed", "Habbie.Hotel.Game", ConsoleColor.DarkYellow);
        }


        public PacketManager GetPacketManager()
        {
            return _packetManager;
        }

        public GameClientManager GetClientManager()
        {
            return _clientManager;
        }

        public GameDataManager GetGameDataManager()
        {
            return _gameDataManager;
        }
    }
}