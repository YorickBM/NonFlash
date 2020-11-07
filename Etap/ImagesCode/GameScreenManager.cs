using Engine.Navigator;
using Etap.Communication.Packets.Incoming.Handshake;
using Etap.Communication.Packets.Outgoing.Misc;
using Etap.Utilities;
using Hoteloverzicht;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overlay;
using Retro.Communication.Packets.Outgoing.Misc;
using Splashscreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Util;

namespace Etap.ImagesCode
{
    class GameScreenManager
    {
        private static GameScreenManager instance;

        internal NavigatorManager GetNavigatorManager()
        {
            return navigatorManager;
        }

        internal int ClientID = -1;

        public static GameScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameScreenManager();
                }
                return instance;
            }
        }

        internal GraphicsDevice GraphicsDevice;
        internal SpriteBatch SpriteBatch;
        internal OverlayRenderer overlayRenderer;
        internal HotelOverviewContent hotelOverview;
        internal SplashScreenManager splashScreenManager;
        private NavigatorManager navigatorManager;
        internal NavigatorManager getNavigatorManager() { return navigatorManager; }

        internal Color BackgroundColor = new Color(13, 20, 27);
        internal Vector2i Dimensions = new Vector2i(1600, 900); //Min Width = 1000;
        internal bool Quit;

        public void LoadContent(ContentManager content)
        {
            splashScreenManager = new SplashScreenManager(content, new Vector2i(0, 5));
            splashScreenManager.Start();

            overlayRenderer = new OverlayRenderer(content);
            overlayRenderer.Hide();
            hotelOverview = new HotelOverviewContent(content, Vector2.Zero);

            navigatorManager = new NavigatorManager(content);

            splashScreenManager.setPercentage(38);

            Thread thr = new Thread(() => SplashProgressThread());
            thr.Start();
        }

        private void SplashProgressThread()
        {
            int UserID = 1;
            Console.WriteLine(UserID);

            Logger.Debug("Initializing Clientsided Server");
            Logger.Info("Please enter SSOTicket:  ");
            string ssoTicket = "";

            RetroEnvironment.Initialize();
            RetroEnvironment.GetConnectionManager().CreateClient();

            Thread.Sleep(500);
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new GetClientVersionEvent());

            RetroEnvironment.GetGame().GetClientManager().SendPacket(new InitCryptoEvent());
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new GenerateSecretKeyEvent("59f6aa4d465979f7c9226744d3117cd2d7a9ffa3d6b275efe5c3fcf8d18d343c9c543d63bd5c9370bad738abc672a9f43cad956a24e0d30fb0dbaabf44f3f71b1c2160e4eb9fb594d844f10867a90269c952d0111cd124894926fa3bcada300340ef5a385020014f44dde37377c235953d4a9e936b3696cd91da5991ef3bafbc"));

            RetroEnvironment.GetGame().GetClientManager().SendPacket(new ClientVariablesEvent());
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new UniqueIDEvent());

            RetroEnvironment.GetGame().GetClientManager().SendPacket(new SSOTicketEvent(ssoTicket));
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new EventTrackerEvent());
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new InfoRetrieveEvent());
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new EventTrackerEvent());

            splashScreenManager.setPercentage(76);
            Logger.DebugWarn("Waiting on Connection Confirmation");
            while (!RetroEnvironment.ConnectionIsSucces && !RetroEnvironment.ShutdownStarted)
            {
            }

            Logger.Debug("Completed SplashScreen");

            //if(RetroEnvironment.ConnectionIsSucces)
            SplashscreenFinish();
        }

        private bool inRoom = true;
        public bool isInRoom() { return inRoom; }
        public void ExitRoom() { inRoom = false; }
        public void EnterRoom() { inRoom = true; }

        public void Draw()
        {
            splashScreenManager.Draw(this.SpriteBatch);
            hotelOverview.Draw(this.SpriteBatch);
            overlayRenderer.Draw(this.SpriteBatch);
            navigatorManager.Draw(this.SpriteBatch);
        }

        public void SplashscreenFinish()
        {
            BackgroundColor = new Color(1, 1, 1);
            overlayRenderer.Show();
            LoadOverview();
            splashScreenManager.Finish();
        }

        public void LoadOverview()
        {
            ExitRoom();
            hotelOverview.EnterOverview();
        }

        public void UnloadContent()
        {
            splashScreenManager.UnloadContent();
            overlayRenderer.UnloadContent();
            hotelOverview.UnloadContent();
            navigatorManager.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            splashScreenManager.Update(gameTime);
            hotelOverview.Update(gameTime);
            overlayRenderer.Update(gameTime);
            navigatorManager.Update(gameTime);
        }
    }
}
