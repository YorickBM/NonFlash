using System;
using System.Threading;
using System.Diagnostics;
using log4net;
using Retro.HabboHotel;
using Retro.Utilities;

namespace Retro.Core
{
    public class ServerStatusUpdater : IDisposable
    {
        private static ILog log = LogManager.GetLogger("Habbie.Core.ServerStatusUpdater");
        private const int UPDATE_IN_SECS = 15;
        public static int _userPeak;

        private static string _lastDate;

        private static bool isExecuted;

        private static Stopwatch lowPriorityProcessWatch;
        private static Timer _mTimer;

        public static void Init()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = RetroEnvironment.PrettyVersion + " - 0 users online - 0 rooms loaded - 0 day(s) 0 hour(s) uptime";
            Logger.Info("Server Status Updater has been started.");
            Console.ResetColor();
            
            lowPriorityProcessWatch = new Stopwatch();
            lowPriorityProcessWatch.Start();
        }

        public static void StartProcessing()
        {
            _mTimer = new Timer(Process, null, 0, 10000);
        }

        internal static void Process(object caller)
        {

        }

        public void Dispose()
        {
            _mTimer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}