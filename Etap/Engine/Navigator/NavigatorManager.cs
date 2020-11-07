using Etap;
using Etap.Communication.Packets.Outgoing.Navigator;
using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Engine.Navigator
{
    class NavigatorManager
    {
        NavigatorContent Navigator;

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
                RetroEnvironment.GetGame().GetClientManager().GetClientByUserID(GameScreenManager.Instance.ClientID).SendPacket(new InitializeNewNavigatorEvent());
            }
            
        }

        internal void OpenNavigator()
        {
            Navigator.Open();
        }

        internal void SetSize(int width, int height)
        {
            Navigator.SetSize(width, height);
        }

        bool OnlyFirstTime = false;
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
