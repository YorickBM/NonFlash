using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ButtonTypes;
using Util;

namespace Overlay
{
    class ToolbarContent
    {

        //Async or Sync task scheduler for Friendbar
        //Splashcreen when Hoteloverview done
        //Splashscreen done make it fill in all the values on overview..
        //When clicking on button make it transparent (That makes it that kind of strange looking)

        internal int maxFriendsStatic = 8;
        internal int maxFriendsDynamic = 8;
        internal int showNum = 0;
        internal int notAvailablePixels = 612;

        private ToggleButton toggleLeft { get; set; }
        private ToggleButton toggleRight { get; set; }
        
        private Image edgeTop { get; set; }
        private Image edgeBottom { get; set; }
        private Image background { get; set; }

        private SwitchHoverButton HOHK { get; set; }
        private HoverButton rooms { get; set; }
        private HoverButton shop { get; set; }
        private HoverButton buildersClub { get; set; }
        private HoverButton myItems { get; set; }
        private HoverButton myProfile { get; set; }
        private HoverButton camera { get; set; }
        private Image dividerOne { get; set; }

        private Image dividerTwo { get; set; }
        private HoverButton myFriends { get; set; }
        private HoverButton searchFriends { get; set; }
        private AnimationSwitchHoverButton messager { get; set; }
        private HoverButton friendsLeft { get; set; }
        private List<ToggleHoverButton> friends { get; set; }
        private HoverButton friendsRight { get; set; }

        private Vector2 offset;

        public ToolbarContent(ContentManager content, int offsetX = 0, int offsetY = 0)
        {
            offset = new Vector2(offsetX, offsetY);
            friends = new List<ToggleHoverButton>();
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends.Add(new ToggleHoverButton(content, new Vector2i(127, 36), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/Friends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18)), new ToggleHoverButtonData("Client/ToolBarBottom/FriendBar/NoFriends", () => { foreach (ToggleHoverButton btn in friends) { btn.resetState(); } }, new Vector2(0, 18))));
            //friends[0].toggle();

            float availablePixels = GameScreenManager.Instance.Dimensions.X - notAvailablePixels - 80;
            float availableFriendSlots = availablePixels / 127;
            maxFriendsDynamic = (int)Math.Floor(availableFriendSlots);

            toggleLeft = new ToggleButton(content, "Client/ToolBarBottom/ToolBar/ToggleBtn", new Vector2i(2,1), () => { toggleLeft.Toggle(); } );
            toggleRight = new ToggleButton(content, "Client/ToolBarBottom/ToolBar/ToggleBtn", new Vector2i(2, 1), () => { toggleRight.Toggle(); });

            edgeTop = new Image(content, "Client/ToolBarBottom/ToolBar/Background/Edge", new Vector2(GameScreenManager.Instance.Dimensions.X, 2));
            edgeBottom = new Image(content, "Client/ToolBarBottom/ToolBar/Background/Edge", new Vector2(GameScreenManager.Instance.Dimensions.X, 1));
            background = new Image(content, "Client/ToolBarBottom/ToolBar/Background/Background", new Vector2(GameScreenManager.Instance.Dimensions.X, 47));

            HOHK = new SwitchHoverButton(content, new Vector2(33, 32), 
                new SwitchHoverButtonData("Client/ToolBarBottom/ToolBar/HomeRoom", () => { HOHK.toggle(); }, new Vector2(0, 18), new Vector2i(3, 1), Color.PaleVioletRed),
                new SwitchHoverButtonData("Client/ToolBarBottom/ToolBar/HotelOverview", () => { HOHK.toggle(); }, new Vector2(0, 18), new Vector2i(3, 1), Color.PaleVioletRed));
            rooms = new HoverButton(content, "Client/ToolBarBottom/ToolBar/Rooms", new Vector2i(3, 1), () => { GameScreenManager.Instance.getNavigatorManager().ToggleNavigator(); }, Color.PaleVioletRed);
            shop = new HoverButton(content, "Client/ToolBarBottom/ToolBar/Catalogue", new Vector2i(3, 1), () => { Console.WriteLine("shop"); }, Color.PaleVioletRed); //Onscherp
            buildersClub = new HoverButton(content, "Client/ToolBarBottom/ToolBar/BuildersClub", new Vector2i(3, 1), () => { Console.WriteLine("buildersClub"); }, Color.PaleVioletRed);
            myItems = new HoverButton(content, "Client/ToolBarBottom/ToolBar/Inventory", new Vector2i(3, 1), () => { Console.WriteLine("myItems"); }, Color.PaleVioletRed);
            myProfile = new HoverButton(content, "Client/ToolBarBottom/ToolBar/GameCenter", new Vector2i(1, 1), () => { Console.WriteLine("myProfile"); }, Color.PaleVioletRed);
            camera = new HoverButton(content, "Client/ToolBarBottom/ToolBar/Camera", new Vector2i(3, 1), () => { Console.WriteLine("camera"); }, Color.PaleVioletRed);
            dividerOne = new Image(content, "Client/ToolBarBottom/ToolBar/Divider", Vector2.Zero);

            dividerTwo = new Image(content, "Client/ToolBarBottom/ToolBar/Divider", Vector2.Zero);
            myFriends = new HoverButton(content, "Client/ToolBarBottom/FriendBar/FriendList", new Vector2i(3, 1), () => { Console.WriteLine("myFriends"); }, Color.PaleVioletRed);
            searchFriends = new HoverButton(content, "Client/ToolBarBottom/FriendBar/FindFriends", new Vector2i(3, 1), () => { Console.WriteLine("searchFriends"); }, Color.PaleVioletRed);
            messager = new AnimationSwitchHoverButton(content, new Vector2(28, 33), new Vector2i(2,3), 432,
                new AnimationSwitchHoverButtonData("Client/ToolBarBottom/FriendBar/PrivateChatNoChat", () => { /* Nothing todo no messages */ }, new Vector2(0, 18), new Vector2i(3, 1), Color.PaleVioletRed), 
                new AnimationSwitchHoverButtonData("Client/ToolBarBottom/FriendBar/PrivateChatHasChat", () => { Console.WriteLine("messager"); }, new Vector2(0, 18), new Vector2i(3, 1), Color.PaleVioletRed),
                new AnimationSwitchHoverButtonData("Client/ToolBarBottom/FriendBar/PrivateChatHasNotification_0", () => { messager.stopAnimation(); }, new Vector2(0, 18), new Vector2i(3, 1), Color.PaleVioletRed),
                new AnimationSwitchHoverButtonData("Client/ToolBarBottom/FriendBar/PrivateChatHasNotification_1", () => { messager.stopAnimation(); }, new Vector2(0, 18), new Vector2i(3, 1), Color.PaleVioletRed));
            
            friendsLeft = new HoverButton(content, "Client/ToolBarBottom/FriendBar/toggleBtn", new Vector2i(3, 1), () => { showNum += 1; }, Color.White);
            friendsRight = new HoverButton(content, "Client/ToolBarBottom/FriendBar/toggleBtn", new Vector2i(3, 1), () => { showNum -= 1; }, Color.White);
        }

        public void UnloadContent()
        {
            toggleLeft.UnloadContent();
            toggleRight.UnloadContent();

            edgeTop.UnloadContent();
            edgeBottom.UnloadContent();
            background.UnloadContent();

            HOHK.UnloadContent();
            rooms.UnloadContent();
            shop.UnloadContent();
            buildersClub.UnloadContent();
            myItems.UnloadContent();
            myProfile.UnloadContent();
            camera.UnloadContent();
            dividerOne.UnloadContent();

            dividerTwo.UnloadContent();
            myFriends.UnloadContent();
            searchFriends.UnloadContent();
            messager.UnloadContent();
            friendsRight.UnloadContent();
            friendsLeft.UnloadContent();

            foreach (ToggleHoverButton btn in friends)
                btn.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            edgeTop.Draw(spriteBatch, position + offset, 0.8f);
            background.Draw(spriteBatch, position + offset + new Vector2(0, 2), 0.8f);
            edgeBottom.Draw(spriteBatch, position + offset + new Vector2(0, 49), 0.8f);

            toggleLeft.Draw(spriteBatch, position + offset + new Vector2(0, 4), 0.82f, SpriteEffects.None);
            toggleRight.Draw(spriteBatch, position + offset + new Vector2(GameScreenManager.Instance.Dimensions.X - toggleRight.framesDimensions.X, 4), 0.82f, SpriteEffects.FlipHorizontally);

            DrawTools(spriteBatch, position, !Convert.ToBoolean(toggleLeft.activeFrame));
            DrawFriends(spriteBatch, position, !Convert.ToBoolean(toggleRight.activeFrame));
        }

        public void Update(GameTime gameTime)
        {
            toggleLeft.Update(gameTime);
            toggleRight.Update(gameTime);

            if (GameScreenManager.Instance.Dimensions.X != edgeTop.dimensions.X)
                edgeTop.resize((int)GameScreenManager.Instance.Dimensions.X, 2);
            if (GameScreenManager.Instance.Dimensions.X != edgeBottom.dimensions.X)
                edgeBottom.resize((int)GameScreenManager.Instance.Dimensions.X, 1);
            if (GameScreenManager.Instance.Dimensions.X != background.dimensions.X)
                background.resize((int)GameScreenManager.Instance.Dimensions.X, 47);

            edgeTop.Update(gameTime);
            edgeBottom.Update(gameTime);
            background.Update(gameTime);

            HOHK.Update(gameTime);
            rooms.Update(gameTime);
            shop.Update(gameTime);
            buildersClub.Update(gameTime);
            myItems.Update(gameTime);
            myProfile.Update(gameTime);
            camera.Update(gameTime);
            dividerOne.Update(gameTime);

            dividerTwo.Update(gameTime);
            myFriends.Update(gameTime);
            searchFriends.Update(gameTime);
            messager.Update(gameTime);
            friendsRight.Update(gameTime);
            friendsLeft.Update(gameTime);

            float availablePixels = GameScreenManager.Instance.Dimensions.X - notAvailablePixels - 80;
            float availableFriendSlots = availablePixels / 127;
            maxFriendsDynamic = (int)Math.Floor(availableFriendSlots);
            if (maxFriendsDynamic > maxFriendsStatic) maxFriendsDynamic = maxFriendsStatic;

            if (showNum == 0) friendsRight.Disable(true);
            else friendsRight.Enable();
            if (showNum + maxFriendsDynamic >= friends.Count()) friendsLeft.Disable(true);
            else friendsLeft.Enable();

            foreach (ToggleHoverButton btn in friends)
                btn.Update(gameTime);
        }

        public void DrawTools(SpriteBatch spriteBatch, Vector2 position, bool expanded)
        {
            Vector2 iconOffset = position + new Vector2(27, 1);
            if(expanded)
            {
                HOHK.Enable();
                HOHK.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (HOHK.getActiveButton().framesDimensions.Y / 2))), 0.81f);
                iconOffset += new Vector2(HOHK.getActiveButton().framesDimensions.X + 16, 0);

                rooms.Enable();
                rooms.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (rooms.framesDimensions.Y / 2))), 0.81f);
                iconOffset += new Vector2(rooms.framesDimensions.X + 16, 0);
            } else
            {
                HOHK.Disable(true);
                rooms.Disable(true);
            }

            shop.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (shop.framesDimensions.Y / 2))), 0.81f);
            iconOffset += new Vector2(shop.framesDimensions.X + 16, 0);

            buildersClub.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (buildersClub.framesDimensions.Y / 2))), 0.81f);
            iconOffset += new Vector2(buildersClub.framesDimensions.X + 16, 0);

            if (GameScreenManager.Instance.isInRoom())
            {
                myItems.Enable();
                myItems.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (myItems.framesDimensions.Y / 2))), 0.81f);
                iconOffset += new Vector2(myItems.framesDimensions.X + 16, 0);
            } else
            {
                myItems.Disable(true);
            }

            myProfile.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (myProfile.framesDimensions.Y / 2))), 0.81f);
            iconOffset += new Vector2(myProfile.framesDimensions.X + 16, 0);

            if (GameScreenManager.Instance.isInRoom())
            {
                camera.Enable();
                camera.Draw(spriteBatch, offset + iconOffset + new Vector2(0, 2 + (int)((50 / 2) - (camera.framesDimensions.Y / 2))), 0.81f);
                iconOffset += new Vector2(camera.framesDimensions.X + 16, 0);
            } else
            {
                camera.Disable(true);
            }

            dividerOne.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (dividerOne.dimensions.Y / 2))), 0.81f);

        }

        public void DrawFriends(SpriteBatch spriteBatch, Vector2 position, bool expanded)
        {
            Vector2 iconOffset = position + new Vector2(GameScreenManager.Instance.Dimensions.X - 20, 1);
            if (expanded && friends.Count > 0)
            {
                iconOffset += new Vector2(-friendsRight.framesDimensions.X, 0);
                friendsRight.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (friendsRight.framesDimensions.Y / 2)) + 1), 0.82f);
                for (int i = showNum; i < maxFriendsDynamic + showNum; i++)
                {
                    ToggleHoverButton btn = friends[i];
                    btn.Draw(spriteBatch, offset + iconOffset + new Vector2((int)(-btn.getActiveButton().framesDimensions.X), (int)((50 / 2) - (btn.getActiveButton().framesDimensions.Y)) + (int)(btn.getActiveOffset().Y)), 0.82f);
                    iconOffset += new Vector2(-btn.getActiveButton().framesDimensions.X - 3, 0);
                }
                iconOffset += new Vector2(-friendsLeft.framesDimensions.X + 3, 0);
                friendsLeft.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (friendsLeft.framesDimensions.Y / 2)) + 1), 0.82f, SpriteEffects.FlipHorizontally);
                iconOffset += new Vector2(-8, 0);
            }
            iconOffset += new Vector2(-32, 0);
            messager.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (messager.getActiveButton().framesDimensions.Y / 2)) - 2), 0.82f);
            iconOffset += new Vector2(-messager.getActiveButton().framesDimensions.X - 16, 0);

            searchFriends.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (searchFriends.framesDimensions.Y / 2)) - 2), 0.82f);
            iconOffset += new Vector2(-searchFriends.framesDimensions.X - 16, 0);

            myFriends.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (myFriends.framesDimensions.Y / 2)) - 2), 0.82f);
            iconOffset += new Vector2(-16, 0);
            if (expanded)
            {
                dividerTwo.Draw(spriteBatch, offset + iconOffset + new Vector2(0, (int)((50 / 2) - (dividerTwo.dimensions.Y / 2))), 0.82f);
            }
        }
    }
}
