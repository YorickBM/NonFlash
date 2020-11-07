using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Reflection;
using Retro.Core;
using Retro.HabboHotel;
using Retro.Communication.RCON;
using Retro.Communication.ConnectionManager;
using Retro.Utilities;
using log4net;
using Retro.Communication.Encryption.Keys;
using Retro.Communication.Encryption;
using System.Threading;
using Retro.Communication.Packets.Incoming.Misc;
using Retro.Communication.Packets.Incoming.Moderation;
using Retro.Communication.Packets.Incoming.Handshake;
using System.Timers;
using Retro.Communication.Packets.Incoming.Messenger;
using Retro.Communication.Packets.Incoming.Users;

namespace Retro
{
    public static class RetroEnvironment
    {
        private static readonly ILog log = LogManager.GetLogger("Retro.RetroEnvironment");
        public static string HotelName = "habbie";
        public static string Licenseto;
        public static bool IsLive;
        public static string CurrentTime = DateTime.Now.ToString("hh:mm:ss tt" + "- Habbie ");
        public const string PrettyVersion = "Habbie Emulator";
        public const string PrettyBuild = "NonFlash";
        public const string ServerVersion = "0.0.1";
        public const string VersionBios = "YorickPrm";
        public const string LastUpdate = " 05/05/2019 ";

        private static Encoding _defaultEncoding;
        public static CultureInfo CultureInfo;

        internal static object UnixTimeStampToDateTime(double timestamp)
        {
            throw new NotImplementedException();
        }

        private static Game _game;
        private static ConfigurationData _configuration;
        private static ConnectionHandling _connectionManager;
        private static RCONSocket _rcon;

        // TODO: Get rid?
        public static bool Event = false;
        public static DateTime lastEvent;
        public static DateTime ServerStarted;

        private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });

        public static string SWFRevision = "";
        //public static int Quartovip;
        //public static int Prisao;

        public static bool ShowDebugWarn = true;
        public static bool ConnectionIsSucces = false;
        public static void RequestStarterData()
        {
            Logger.DebugWarn("Requesting Starter Data");
            //All the recieve package events on bootup
            //GetClientVersionEvent,InitCryptoEvent,GenerateSecretKeyEvent,
            //ClientVariablesEvent, UniqueIDEvent, SSOTicketEvent

            ///Atm geen Encryptie want kan de CipherPublicKey niet uitvogelen :?
            ///GetGame().GetClientManager().SendPacket(new InitCryptoEvent());
            ///GetGame().GetClientManager().SendPacket(new GenerateSecretKeyEvent("CipherPublicKey"));
            GetGame().GetClientManager().SendPacket(new SSOTicketEvent("SSO-NONFLASH-37"));
            GetGame().GetClientManager().SendPacket(new GetClientVersionEvent());
            GetGame().GetClientManager().SendPacket(new InfoRetrieveEvent());
            GetGame().GetClientManager().SendPacket(new UniqueIDEvent());
        }
        public static System.Timers.Timer SecTimer30 = new System.Timers.Timer();
        public static System.Timers.Timer SecTimer60 = new System.Timers.Timer();
        public static System.Timers.Timer SecTimer120 = new System.Timers.Timer();
        public static void InitializeIntervalTimers()
        {
            SecTimer30.Elapsed += new ElapsedEventHandler(sec30);
            SecTimer30.Interval = 30000;
            SecTimer30.Enabled = true;

            SecTimer60.Elapsed += new ElapsedEventHandler(sec60);
            SecTimer60.Interval = 60000;
            SecTimer60.Enabled = true;

            SecTimer120.Elapsed += new ElapsedEventHandler(sec120);
            SecTimer120.Interval = 120000;
            SecTimer120.Enabled = true;
        }
        private static void sec30(object source, ElapsedEventArgs e)
        {
            GetGame().GetClientManager().SendPacket(new PingEvent());
        }
        private static void sec60(object source, ElapsedEventArgs e)
        {
            GetGame().GetClientManager().SendPacket(new FriendListUpdateEvent());
        }
        private static void sec120(object source, ElapsedEventArgs e)
        {
            GetGame().GetClientManager().SendPacket(new ScrGetUserInfoEvent());
        }

        static string location = System.Reflection.Assembly.GetEntryAssembly().Location;
        public static void Initialize()
        {
            ServerStarted = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Title = "Loading " + PrettyVersion;
            _defaultEncoding = Encoding.Default;

            Console.WriteLine("");
            Console.WriteLine("");

            CultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
            try
            {
                Logger.Info("Loading ConfigFile");
                _configuration = new ConfigurationData(location.Substring(0, location.Length - 9) + @"\Configuration\Config.ini", false);

                Logger.Info("Initializing Encryption");
                //Have our encryption ready.
                HabboEncryptionV2.Initialize(new RSAKeys());

                Logger.Info("Setting up RCON Socket");
                //Make sure MUS is working.
                _rcon = new RCONSocket(GetConfig().data["mus.tcp.bindip"], int.Parse(GetConfig().data["mus.tcp.port"]), GetConfig().data["mus.tcp.allowedaddr"].Split(Convert.ToChar(";")));

                Logger.Info("Setting up ConnectionManager");
                //Accept connections.
                _connectionManager = new ConnectionHandling(int.Parse(GetConfig().data["game.tcp.port"]), GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                _connectionManager.Init();


                log.Debug("Loading Game");
                _game = new Game();
                _game.ContinueLoading();

                //_game.StartGameLoop();
                TimeSpan TimeUsed = DateTime.Now - ServerStarted;

                //Quartovip = int.Parse(GetConfig().data["Quartovip"]);
                //Prisao = int.Parse(GetConfig().data["Prisao"]);

                Console.ForegroundColor = ConsoleColor.Green;
                Logger.Info("" + PrettyVersion + " -> READY! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");
                Console.ResetColor();
                IsLive = true;
                Console.ForegroundColor = ConsoleColor.Gray;

            }
            catch (KeyNotFoundException e)
            {
                Logger.Info("Please check your configuration file - some values appear to be missing.");
                Logger.Info("Press any key to shut down ...");
                ExceptionLogger.LogException(e);
                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (InvalidOperationException e)
            {
                Logger.Info("Failed to initialize " + PrettyVersion + ":" + e.Message);
                Logger.Info("Press any key to shut down ...");
                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (Exception e)
            {
                Logger.Info("Fatal error during startup:" + e);
                Logger.Info("Press any key to shut down ...");

                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        public static string BoolToEnum(bool Bool)
        {
            return (Bool == true ? "1" : "0");
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            return RandomNumber.GenerateNewRandom(Min, Max);
        }

        public static string Rainbow()
        {
            int numColors = 1000;
            var colors = new List<string>();
            var random = new Random();
            for (int i = 0; i < numColors; i++)
            {
                colors.Add(String.Format("#{0:X2}{1:X2}00", i, random.Next(0x1000000) - i));
            }

            int index = 0;
            string rainbow = colors[index];

            if (index > numColors)
                index = 0;
            else
                index++;

            return rainbow;
        }

        public static string RainbowT()
        {
            int numColorst = 1000;
            var colorst = new List<string>();
            var randomt = new Random();
            for (int i = 0; i < numColorst; i++)
            {
                colorst.Add(String.Format("#{0:X6}", randomt.Next(0x1000000)));
            }

            int indext = 0;
            string rainbowt = colorst[indext];

            if (indext > numColorst)
                indext = 0;
            else
                indext++;

            return rainbowt;
        }

        public static double GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return ts.TotalSeconds;
        }

        internal static int GetIUnixTimestamp()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            var unixTime = ts.TotalSeconds;
            return Convert.ToInt32(unixTime);
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        public static long Now()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalMilliseconds;
            return (long)unixTime;
        }

        public static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!IsValid(character))
                    return "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";
            }

            return figure;
        }

        private static bool IsValid(char character)
        {
            return Allowedchars.Contains(character);
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            inputStr = inputStr.ToLower();
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!IsValid(inputStr[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ShutdownStarted { get; set; }

        public static void PerformShutDown()
        {
            PerformShutDown(false);
        }

        public static void PerformRestart()
        {
            PerformShutDown(true);
        }

        public static void PerformShutDown(bool restart)
        {
            Console.Clear();
            Logger.Info("Server shutting down...");
            Console.Title = "HABBIE EMULATOR: SHUTTING DOWN!";

            ///GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(GetLanguageManager().TryGetValue("server.shutdown.message")));
            GetGame().StopGameLoop();
            Thread.Sleep(2500);
            GetConnectionManager().Destroy();//Stop listening.
            GetGame().GetPacketManager().UnregisterAll();//Unregister the packets.
            GetGame().GetPacketManager().WaitForAllToComplete();
            GetGame().GetClientManager().CloseAll();//Close all connections

            Logger.Info(PrettyVersion + "has successfully shutdown.");
            IsLive = false;

            if (restart)
            {
                Process.Start(Assembly.GetEntryAssembly().Location);
                Logger.Info("Rebooting " + PrettyVersion);

            }

            System.Threading.Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static ConfigurationData GetConfig()
        {
            return _configuration;
        }

        public static Encoding GetDefaultEncoding()
        {
            return _defaultEncoding;
        }

        public static ConnectionHandling GetConnectionManager()
        {
            return _connectionManager;
        }

        public static Game GetGame()
        {
            return _game;
        }

        public static RCONSocket GetRCONSocket()
        {
            return _rcon;
        }
    }
}