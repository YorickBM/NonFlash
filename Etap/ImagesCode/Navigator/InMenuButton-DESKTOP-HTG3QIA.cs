using Etap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.ButtonTypes;
using static Util.Button;

namespace Engine.Navigator
{
    class InMenuButton
    {
        private ToggleHoverButton cornerLeft, cornerRight, edgeTop, edgeLeft, edgeRight, backdrop, edgeBottom;
        private Vector2i size, Position;
        private Font buttonName;

        public InMenuButton(ContentManager content, int width, MyAction action, string text = "button.name.notfound")
        {
            this.size = new Vector2i(width, 30);
            this.Position = new Vector2i(0, 0);
            buttonName = new Font(content, "Fonts/InMenuButton", text, Color.Black);

            cornerLeft = new ToggleHoverButton(content, new Vector2i(6, 6), new ToggleHoverButtonData("Menu/Navigator/Button/Corner", action, new Vector2(0, 0), new Vector2i(3, 1), Color.White));
            cornerRight = new ToggleHoverButton(content, new Vector2i(6, 6), new ToggleHoverButtonData("Menu/Navigator/Button/Corner", () => { }, new Vector2(0, 0), new Vector2i(3, 1), Color.White));
            edgeTop = new ToggleHoverButton(content, new Vector2i(size.X - cornerLeft.getActiveButton().framesDimensions.X * 2, 5), new ToggleHoverButtonData("Menu/Navigator/Button/edgeTop", () => { }, new Vector2(0, 0), new Vector2i(1, 3), Color.White));
            edgeLeft = new ToggleHoverButton(content, new Vector2i(4, size.Y - cornerLeft.getActiveButton().framesDimensions.Y), new ToggleHoverButtonData("Menu/Navigator/Button/edgeSide", () => { }, new Vector2(0, 0), new Vector2i(3, 1), Color.White));
            edgeRight = new ToggleHoverButton(content, new Vector2i(4, size.Y - cornerRight.getActiveButton().framesDimensions.Y), new ToggleHoverButtonData("Menu/Navigator/Button/edgeSide", () => { }, new Vector2(0, 0), new Vector2i(3, 1), Color.White));
            backdrop = new ToggleHoverButton(content, new Vector2i(size.X - edgeLeft.getActiveButton().framesDimensions.X * 2, 27), new ToggleHoverButtonData("Menu/Navigator/Button/Backdrop", () => { }, new Vector2(0, 0), new Vector2i(1, 3), Color.White));
        }

        public void UnloadContent()
        {
            cornerLeft.UnloadContent();
            cornerRight.UnloadContent();
            edgeTop.UnloadContent();
            edgeLeft.UnloadContent();
            edgeRight.UnloadContent();
            backdrop.UnloadContent();

            buttonName.UnloadContent();
        }

        public Vector2i GetSize()
        {
            return size;
        }

        //private bool processed = false;
        public void Update(GameTime gameTime)
        {
            if(RetroEnvironment.GetLanguageManager() != null)
                buttonName.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(buttonName.GetOriginalText()));

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle((int)Position.X, (int)Position.Y, size.X, size.Y);

            if (rectangle.Contains(mousePoint))
            {
                backdrop.SetHover(true);
                cornerLeft.SetHover(true);
                cornerRight.SetHover(true);

                edgeTop.SetHover(true);
                edgeLeft.SetHover(true);
                edgeRight.SetHover(true);

                bool isClicked = mouseState.LeftButton == ButtonState.Pressed;
                if (!backdrop.GetActive())
                {
                    backdrop.SetClick(isClicked);
                    cornerLeft.SetClick(isClicked);
                    cornerRight.SetClick(isClicked);

                    edgeTop.SetClick(isClicked);
                    edgeLeft.SetClick(isClicked);
                    edgeRight.SetClick(isClicked);
                } else
                {
                    backdrop.SetClick(false);
                    cornerLeft.SetClick(false);
                    cornerRight.SetClick(false);

                    edgeTop.SetClick(false);
                    edgeLeft.SetClick(false);
                    edgeRight.SetClick(false);
                }
            }
            else
            {
                backdrop.SetHover(false);
                cornerLeft.SetHover(false);
                cornerRight.SetHover(false);

                edgeTop.SetHover(false);
                edgeLeft.SetHover(false);
                edgeRight.SetHover(false);
            }

            cornerLeft.Update(gameTime, false);
            cornerRight.Update(gameTime, false);

            edgeTop.Update(gameTime, false);
            edgeLeft.Update(gameTime, false);
            edgeRight.Update(gameTime, false);

            backdrop.Update(gameTime, false);

            buttonName.SetPosition(new Vector2(size.X / 2 - buttonName.measureString().X / 2, size.Y / 2 - buttonName.measureString().Y / 2));
            buttonName.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2i pos, float deptLayer = 0.96f)
        {
            Position = pos;
            backdrop.Draw(spriteBatch, pos + new Vector2i(edgeLeft.getActiveButton().framesDimensions.X, edgeTop.getActiveButton().framesDimensions.Y - 2), deptLayer);
            edgeTop.Draw(spriteBatch, pos + new Vector2i(cornerLeft.getActiveButton().framesDimensions.X, 0), deptLayer);
            edgeLeft.Draw(spriteBatch, pos + new Vector2i(0, cornerLeft.getActiveButton().framesDimensions.Y), deptLayer);
            edgeRight.Draw(spriteBatch, pos + new Vector2i(size.X - edgeRight.getActiveButton().framesDimensions.X, cornerLeft.getActiveButton().framesDimensions.Y), deptLayer, SpriteEffects.FlipHorizontally);
            
            cornerLeft.Draw(spriteBatch, pos + new Vector2i(0, 0), deptLayer + 0.01f);
            cornerRight.Draw(spriteBatch, pos + new Vector2i(size.X - cornerRight.getActiveButton().framesDimensions.X, 0), deptLayer + 0.01f, SpriteEffects.FlipHorizontally);

            buttonName.Draw(spriteBatch, buttonName.GetPosition() + pos + new Vector2i(0, 0), deptLayer + 0.02f);
        }

        public void SetActive(bool value = true)
        {
            backdrop.SetActive(value);
            cornerLeft.SetActive(value);
            cornerRight.SetActive(value);

            edgeTop.SetActive(value);
            edgeLeft.SetActive(value);
            edgeRight.SetActive(value);
        }

        public void Select()
        {
            bool isClicked = true;
            backdrop.SetClick(isClicked);
            cornerLeft.SetClick(isClicked);
            cornerRight.SetClick(isClicked);

            edgeTop.SetClick(isClicked);
            edgeLeft.SetClick(isClicked);
            edgeRight.SetClick(isClicked);
        }
        public void Deselect()
        {
            backdrop.reset();
            cornerLeft.reset();
            cornerRight.reset();

            edgeTop.reset();
            edgeLeft.reset();
            edgeRight.reset();
        }

        public void Hover()
        {

        }
    }
}
