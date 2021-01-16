using Engine.Catalogus;
using Engine.Inventory;
using Engine.Navigator;
using Etap.Communication.Packets.Incoming.Handshake;
using Etap.Communication.Packets.Outgoing.Misc;
using Etap.Engine.Room;
using Etap.Utilities;
using Hoteloverzicht;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Textbox;
using Overlay;
using Retro.Communication.Packets.Outgoing.Misc;
using Splashscreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Util;

namespace Etap.ImagesCode
{
    class GameScreenManager
    {
        private static GameScreenManager instance;

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
        private RoomManager roomManager;
        private FurniManager furniManager;
        private CatalogusManager catalogusManager;
        private InventoryManager inventoryManager;

        internal Image catalogusnotFoundIcon;

        private ContentManager _contentManager;
        private Dictionary<int, Furnitype> _furnitypes;

        internal NavigatorManager GetNavigatorManager()
        {
            return navigatorManager;
        }
        internal RoomManager GetRoomManager()
        {
            return roomManager;
        }
        internal FurniManager GetFurniManager()
        {
            return furniManager;
        }
        internal CatalogusManager GetCatalogusManager()
        {
            return catalogusManager;
        }
        internal InventoryManager GetInventoryManager()
        {
            return inventoryManager;
        }

        internal Color BackgroundColor = new Color(13, 20, 27);

        internal ContentManager GetContentManager()
        {
            return _contentManager;
        }

        internal Vector2i Dimensions = new Vector2i(1600, 900); //Min Width = 1000;
        internal bool Quit;

        public void LoadContent(ContentManager content)
        {
            _contentManager = content;
            _furnitypes = new Dictionary<int, Furnitype>();
            loadFurniXml();

            splashScreenManager = new SplashScreenManager(content, new Vector2i(0, 5));
            splashScreenManager.Start();

            overlayRenderer = new OverlayRenderer(content);
            overlayRenderer.Hide();
            hotelOverview = new HotelOverviewContent(content, Vector2.Zero);

            navigatorManager = new NavigatorManager(content);
            roomManager = new RoomManager(content);
            furniManager = new FurniManager();
            catalogusManager = new CatalogusManager(content);
            inventoryManager = new InventoryManager(content);

            splashScreenManager.setPercentage(38);

            catalogusnotFoundIcon = new Image(content, "catalogue/icons/icon_1", Vector2.Zero);

            Thread thr = new Thread(() => SplashProgressThread());
            thr.Start();
        }

        public Furnitype GetFurniTypeBySpriteId(int id)
        {
            Furnitype outType;
            _furnitypes.TryGetValue(id, out outType);
            return outType;
        }

        public void loadFurniXml()
        {
            string text = File.ReadAllText(@"Content/furnidata.xml");
            var sr = new System.IO.StringReader(text);
            var xs = new XmlSerializer(typeof(Furnidata));

            var result = xs.Deserialize(sr);
            Furnidata furnidata = (Furnidata)result;

            foreach (Furnitype type in furnidata.roomitemtypes)
                if(!_furnitypes.ContainsKey(type.id)) _furnitypes.Add(type.id, type);
        }

        private void SplashProgressThread()
        {
            int UserID = 1;
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

            splashScreenManager.setPercentage(58);
            Thread.Sleep(500);

            FloorGenerator.Initialize();

            splashScreenManager.setPercentage(76);
            Logger.DebugWarn("Waiting on Connection Confirmation");
            while (!RetroEnvironment.ConnectionIsSucces && !RetroEnvironment.ShutdownStarted)
            {
            }

            Logger.Debug("Completed SplashScreen");

            //if(RetroEnvironment.ConnectionIsSucces)
            SplashscreenFinish();
        }

        public void Draw()
        {
            try
            {
                splashScreenManager.Draw(this.SpriteBatch);
                if (GetRoomManager().isInRoom())
                {
                    GetRoomManager().Draw(this.SpriteBatch);
                }
                else
                {
                    hotelOverview.Draw(this.SpriteBatch);
                }
                overlayRenderer.Draw(this.SpriteBatch);
                navigatorManager.Draw(this.SpriteBatch);
                catalogusManager.Draw(this.SpriteBatch);
                inventoryManager.Draw(this.SpriteBatch);

            }
            catch(Exception ex)
            {
                Logger.Error("GameScreenManager Draw Error!!\n", ex);
            }
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
            GetRoomManager().ExitRoom();
            hotelOverview.EnterOverview();
        }

        public void UnloadContent()
        {
            splashScreenManager.UnloadContent();
            overlayRenderer.UnloadContent();
            hotelOverview.UnloadContent();
            navigatorManager.UnloadContent();
            GetRoomManager().UnloadContent();
            catalogusManager.UnloadContent();
            inventoryManager.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            splashScreenManager.Update(gameTime);
            hotelOverview.Update(gameTime);
            overlayRenderer.Update(gameTime);
            navigatorManager.Update(gameTime);
            catalogusManager.Update(gameTime);
            inventoryManager.Update(gameTime);
        }
    }
}
