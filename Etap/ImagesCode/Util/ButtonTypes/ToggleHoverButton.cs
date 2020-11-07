using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Util.Button;

namespace Util.ButtonTypes
{
    class ToggleHoverButtonData
    {
        public ToggleHoverButtonData(String pathOne, MyAction actionOne, Vector2 offsetOne)
        {
            path = pathOne;
            action = actionOne;
            offset = offsetOne;
            frames = new Vector2i(1, 3);
            color = Color.White;
        }
        public ToggleHoverButtonData(String pathOne, MyAction actionOne, Vector2 offsetOne, Vector2i framesOne)
        {
            path = pathOne;
            action = actionOne;
            offset = offsetOne;
            frames = framesOne;
            color = Color.White;
        }
        public ToggleHoverButtonData(String pathOne, MyAction actionOne, Vector2 offsetOne, Vector2i framesOne, Color colorOne)
        {
            path = pathOne;
            action = actionOne;
            offset = offsetOne;
            frames = framesOne;
            color = colorOne;
        }

        public String path { get; set; }
        public MyAction action { get; set; }
        public Vector2 offset { get; set; }
        public Vector2i frames { get; set; }
        public Color color { get; set; }
    }

    class ToggleHoverButton
    { 
        List<ToggleHoverButtonData> data;
        List<HoverButton> buttons;

        int activeButton = 0;
        Timer timer;
        Vector2i size;
        Vector2i originalSize;

        public ToggleHoverButton(ContentManager content, Vector2i size, params ToggleHoverButtonData[] dataList)
        {
            buttons = new List<HoverButton>();
            data = new List<ToggleHoverButtonData>();
            this.size = size;
            originalSize = size;

            foreach (ToggleHoverButtonData dataItem in dataList)
                data.Add(dataItem);

            foreach (ToggleHoverButtonData dataItem in data)
                buttons.Add(new HoverButton(content, dataItem.path, dataItem.frames, dataItem.action, dataItem.color));

            timer = new Timer(200);
            timer.Elapsed += new ElapsedEventHandler(timer_reset);
        }

        void timer_reset(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
        }

        public HoverButton getActiveButton()
        {
            return buttons[activeButton];
        }

        public void toggle()
        {
            activeButton += 1;
            if (activeButton == buttons.Count()) activeButton = 0;
        }

        public Vector2 getActiveOffset()
        {
            return data[activeButton].offset;
        }

        public void UnloadContent()
        {
            foreach (HoverButton btn in buttons)
                btn.UnloadContent();
        }

        private bool Clicked = false;

        public void SetHover(bool value)
        {
            getActiveButton().isHovered = value;
           
        }
        public void SetClick(bool value)
        {
            getActiveButton().isClicked = value;
        }

        public void Update(GameTime gameTime, bool rectangleCalc = true)
        {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            var btn = getActiveButton();
            btn.ResizeFrame(size);

            var rectangle = new Rectangle((int)btn.Position.X, (int)btn.Position.Y, size.X, size.Y);

            if (rectangleCalc)
            {
                if (rectangle.Contains(mousePoint))
                {
                    btn.isHovered = true;
                    btn.isClicked = mouseState.LeftButton == ButtonState.Pressed;
                }
                else
                {
                    btn.isHovered = false;
                }
            }

            if (btn.isHovered && !btn.isClicked)
            {
                if (!btn.t.Enabled)
                    btn.loadFrame(2);

                if (btn.color != btn.originColor) btn.color = btn.originColor;
            } else if (btn.isClicked) {
                btn.loadFrame(1);

                if (!btn.t.Enabled)
                    btn.clickAction.Invoke();

                if (!btn.t.Enabled)
                    if (Clicked) Clicked = false;
                    else Clicked = true;

                btn.t.Start();
            } else
            {
                btn.loadFrame(0);
            }

            if (Clicked)
                btn.loadFrame(1);
        }

        public void reset()
        {
            Clicked = false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float dept = 0.8f, SpriteEffects effect = SpriteEffects.None)
        {
            getActiveButton().Draw(spriteBatch, position + new Vector2(0, 0), dept, effect);
        }
    }
}
