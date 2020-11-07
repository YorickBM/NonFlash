using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.ButtonTypes;

namespace Engine.Navigator
{
    class NavigatorContent : CloseAndInfoUI
    {
        private Image hiddenLine, backdrop;
        private List<InMenuButton> menuButtons;

        public NavigatorContent(ContentManager content, Vector2i position, Vector2i size, int offsetX = 0, int offsetY = 0) : base(content, position, size, "Navigator", offsetX, offsetY)
        {
            hiddenLine = new Image(content, "Menu/Navigator/hiddenLine", Vector2.Zero);
            backdrop = new Image(content, "Menu/Navigator/backdrop", Vector2.Zero);

            int width = size.X - 2 * 8;
            width = width / 4;

            menuButtons = new List<InMenuButton>();
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect();  }, "navigator.toplevelview.official_view"));
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); }, "navigator.toplevelview.hotel_view"));
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); }, "navigator.toplevelview.roomads_view"));
            menuButtons.Add(new InMenuButton(content, width, () => { foreach (InMenuButton menuButton in menuButtons) menuButton.Deselect(); }, "navigator.toplevelview.myworld_view"));
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            hiddenLine.UnloadContent();
            backdrop.UnloadContent();
            foreach(InMenuButton menuButton in menuButtons)
                menuButton.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            backdrop.resize(size.X - 6, size.Y - headerHeight - 33 - edgeBottom.dimensions.Y);
            backdrop.Update(gameTime);

            hiddenLine.resize(size.X - 6, hiddenLine.dimensions.Y);
            hiddenLine.Update(gameTime);

            foreach (InMenuButton menuButton in menuButtons)
                menuButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isOpen())
            {
                backdrop.Draw(spriteBatch, (position + offset + new Vector2i(3, headerHeight + 33 + 1)), 0.9f);
                hiddenLine.Draw(spriteBatch, (position + offset + new Vector2i(3, headerHeight + 33)), 0.9f);
                base.Draw(spriteBatch);

                Vector2i specialOffset = new Vector2i(0, 0);
                foreach (InMenuButton menuButton in menuButtons)
                {
                    menuButton.Draw(spriteBatch, (specialOffset + position + offset + new Vector2i(1 + 8, headerHeight + 4)), 0.95f);
                    specialOffset = new Vector2i(specialOffset.X + menuButton.GetSize().X, specialOffset.Y);
                }
            }
        }
    }
}
