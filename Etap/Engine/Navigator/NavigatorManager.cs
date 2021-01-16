using Etap;
using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.Communication.Packets.Outgoing.Rooms.Connection;
using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Util;

namespace Engine.Navigator
{
    class NavigatorManager
    {
        NavigatorContent Navigator;
        Timer delay;
        bool OnlyFirstTime = false;

        public NavigatorManager(ContentManager content)
        {
            OnlyFirstTime = true;
            Navigator = new NavigatorContent(content, new Vector2i(0, 0), new Vector2i(425, 300));
        }
        public void ToggleNavigator()
        {
            
            //RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(GameScreenManager.Instance.ClientID).SendPacket(new GetUserFlatCatsEvent());
            //RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(GameScreenManager.Instance.ClientID).SendPacket(new GetNavigatorFlatsEvent());

            if(Navigator.isOpen())
            {
                Navigator.Close();
            } else
            {
                RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(GameScreenManager.Instance.ClientID).ResetRooms();
                RetroEnvironment.GetGame().GetClientManager().SendPacket(new InitializeNewNavigatorEvent());
            }
            
        }

        internal void Initialize(string section)
        {
            Navigator.Initialize(GameScreenManager.Instance.GetContentManager(), section);
        }

        internal void GoToRoom(int roomId, int acces)
        {
            GameScreenManager.Instance.GetInventoryManager().Close();
            Navigator.Close();
            GameScreenManager.Instance.GetRoomManager().EnterRoom(roomId, acces, "");
        }

        internal void OpenNavigator()
        {
            Navigator.Open();
            RetroEnvironment.GetGame().GetClientManager().SendPacket(new NavigatorSearchEvent("official_view", ""));
        }

        internal void SetSize(int width, int height)
        {
            Navigator.SetSize(width, height);
        }

        internal void SetPosition(int posX, int posY)
        {
            if(OnlyFirstTime)
            {
                OnlyFirstTime = false;
                Navigator.SetPosition(new Vector2i(posX, posY));
            }
        }

        internal void Update(GameTime gameTime)
        {
            if(Navigator.isOpen())
                Navigator.Update(gameTime);
        }

        internal void UnloadContent()
        {
            Navigator.UnloadContent();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Navigator.Draw(spriteBatch);
        }
    }
}
