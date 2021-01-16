using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.ButtonTypes;
using static Util.Button;

namespace Util
{
    class TopButtonsContainer
    {
        ContentManager content;
        private List<InMenuButton> menuButtons;
        int width;

        Image hiddenLine, backdrop;
        InMenuButton template;

        public TopButtonsContainer(ContentManager content)
        {
            this.content = content;
            this.width = 0;

            menuButtons = new List<InMenuButton>();
            hiddenLine = new Image(content, "Menu/Navigator/hiddenLine", Vector2.Zero);
            backdrop = new Image(content, "Menu/Navigator/backdrop", Vector2.Zero);
            template = new InMenuButton(content, (int)((width - 18) / 1), () => { }, "No Message");
        }

        public TopButtonsContainer SetWidth(int x)
        {
            width = x;
            return this;
        }
        public int GetHeight() { return 34; }

        public List<InMenuButton> GetButtons() { return menuButtons; }

        public TopButtonsContainer AddButton(int slots, MyAction action, string name, bool useLanguage = true)
        {
            InMenuButton btn = new InMenuButton(content, (int)((width - 18) / slots), action, name);
            //InMenuButton btn = template.Clone() as InMenuButton;
            //btn.SetWidth((int)((width - 18) / slots));
            //btn.SetAction(action);
            //btn.SetName(name);

            if (!useLanguage) btn.ToggleLanguageManager();
            menuButtons.Add(btn);

            return this;
        }

        public TopButtonsContainer Reset()
        {
            menuButtons.Clear();
            return this;
        }

        public void UnloadContent()
        {
            hiddenLine.UnloadContent();
            backdrop.UnloadContent();

            foreach (InMenuButton menuButton in menuButtons)
                menuButton.UnloadContent();
        }

        public void Update(GameTime gameTime, Vector2i size, int headerHeight, int offsetY)
        {
            backdrop.resize(size.X - 6, size.Y - headerHeight - 33 - offsetY);
            backdrop.Update(gameTime);

            hiddenLine.resize(size.X - 6, hiddenLine.dimensions.Y);
            hiddenLine.Update(gameTime);

            try
            {
                foreach (InMenuButton menuButton in menuButtons)
                    menuButton.Update(gameTime);
            } catch(Exception ex)
            {
                Logger.Error("Could not update menuButtons");
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2i position, Vector2i offset, int headerHeight, float depth)
        {
            backdrop.Draw(spriteBatch, (position + offset + new Vector2i(3, headerHeight + 33 + 1)), depth - 0.01f);
            hiddenLine.Draw(spriteBatch, (position + offset + new Vector2i(3, headerHeight + 33)), depth - 0.01f);

            Vector2i specialOffset = new Vector2i(0, 0);
            foreach (InMenuButton menuButton in menuButtons)
            {
                menuButton.Draw(spriteBatch, (specialOffset + position + offset + new Vector2i(1 + 8, headerHeight + 4)), depth);
                specialOffset = new Vector2i(specialOffset.X + menuButton.GetSize().X, specialOffset.Y);
            }
        }
    }
}
