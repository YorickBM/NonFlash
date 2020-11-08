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
        private DropdownButton dropdown;
        private ScrollView view;

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

            dropdown = new DropdownButton(content, 116, position, 
                new DropdownItem("navigator.filter.anything", ()=> { }),
                new DropdownItem("navigator.filter.room.name", () => { }),
                new DropdownItem("navigator.filter.owner", () => { }),
                new DropdownItem("navigator.filter.tag", () => { }),
                new DropdownItem("navigator.filter.group", () => { })
                );

            Image imgOne = new Image(content, "Menu/Navigator/NewRoom", new Vector2(189,61));
            imgOne.SetPosition(new Vector2i(0, 0));
            Image imgTwo = new Image(content, "Menu/Navigator/RandomRoom", new Vector2(189, 61));
            imgTwo.SetPosition(new Vector2i(0, 60));
            Image imgThree = new Image(content, "Menu/Navigator/NewRoom", new Vector2(189, 62));
            imgThree.SetPosition(new Vector2i(0, 120));

            view = new ScrollView(content, new Vector2i(0, 0), new Vector2i(imgThree.dimensions.X, 120), imgOne, imgTwo, imgThree);
            //view.SetScrolled(new Vector2i(0, 64));
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            hiddenLine.UnloadContent();
            backdrop.UnloadContent();
            foreach(InMenuButton menuButton in menuButtons)
                menuButton.UnloadContent();
            dropdown.UnloadContent();
            view.UnloadContent();
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

            dropdown.SetPosition(position + offset + new Vector2i(14, 43 + headerHeight));
            dropdown.Update(gameTime);

            view.SetPosition(position + new Vector2i(14, headerHeight + 75)) ;
            view.Update(gameTime);
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

                dropdown.Draw(spriteBatch, 0.97f);
                view.Draw(spriteBatch, 0.96f);
            }
        }
    }
}
