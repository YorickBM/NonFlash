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
    class AnimationSwitchHoverButtonData
    {
        public AnimationSwitchHoverButtonData(String pathOne, MyAction actionOne, Vector2 offsetOne)
        {
            path = pathOne;
            action = actionOne;
            offset = offsetOne;
            frames = new Vector2i(1, 3);
            color = Color.White;
        }
        public AnimationSwitchHoverButtonData(String pathOne, MyAction actionOne, Vector2 offsetOne, Vector2i framesOne)
        {
            path = pathOne;
            action = actionOne;
            offset = offsetOne;
            frames = framesOne;
            color = Color.White;
        }
        public AnimationSwitchHoverButtonData(String pathOne, MyAction actionOne, Vector2 offsetOne, Vector2i framesOne, Color colorOne)
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

    class AnimationSwitchHoverButton
    { 
        List<AnimationSwitchHoverButtonData> data;
        List<HoverButton> buttons;

        int activeButton = 0;
        Timer timer;
        Vector2 size;
        Vector2 originalSize;

        int activeAnimationFrame;
        Vector2i animatedFrames;
        Timer animationTimer;

        public AnimationSwitchHoverButton(ContentManager content, Vector2 size, Vector2i animatedFrames, int animationDelay, params AnimationSwitchHoverButtonData[] dataList)
        {
            buttons = new List<HoverButton>();
            data = new List<AnimationSwitchHoverButtonData>();
            this.size = size;
            originalSize = size;

            foreach (AnimationSwitchHoverButtonData dataItem in dataList)
                data.Add(dataItem);

            foreach (AnimationSwitchHoverButtonData dataItem in data)
                buttons.Add(new HoverButton(content, dataItem.path, dataItem.frames, dataItem.action, dataItem.color));

            timer = new Timer(200);
            timer.Elapsed += new ElapsedEventHandler(timer_reset);

            this.animatedFrames = animatedFrames;
            activeAnimationFrame = animatedFrames.X;
            animationTimer = new Timer(animationDelay);
            animationTimer.AutoReset = true;
            animationTimer.Elapsed += new ElapsedEventHandler(animationNext);
        }

        void timer_reset(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
        }

        public HoverButton getActiveButton()
        {
            return buttons[activeButton];
        }

        internal void SetHover(bool isHoverring)
        {
            throw new NotImplementedException();
        }

        public void toggle()
        {
            activeButton += 1;
            if (activeButton == buttons.Count()) activeButton = 0;
        }
        public void setButton(int num)
        {
            activeButton = num;
        }

        public void setSize(Vector2 newSize)
        {
            size = newSize;
        }
        public void resetSize()
        {
            size = originalSize;
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

        private bool animationActive = false;
        public void Update(GameTime gameTime)
        {

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);
            var rectangle = new Rectangle((int)getActiveButton().Position.X + (int)(getActiveButton().framesDimensions.X - size.X), 
                (int)getActiveButton().Position.Y + (int)(getActiveButton().framesDimensions.Y - size.Y), (int)size.X, (int)size.Y);

            if (getActiveButton().enabled)
            {
                if (rectangle.Contains(mousePoint))
                {
                    getActiveButton().isHovered = true;
                    getActiveButton().isClicked = mouseState.LeftButton == ButtonState.Pressed;
                }
                else
                {
                    getActiveButton().isHovered = false;
                    getActiveButton().isClicked = false;
                }

                if (getActiveButton().isHovered && !getActiveButton().isClicked)
                {
                    if (getActiveButton().color != getActiveButton().originColor) getActiveButton().color = getActiveButton().originColor;

                    if (!getActiveButton().t.Enabled)
                        getActiveButton().loadFrame(2);
                }
                else if (getActiveButton().isClicked)
                {
                    getActiveButton().loadFrame(1);
                    if (!getActiveButton().t.Enabled && !timer.Enabled)
                    {
                        getActiveButton().clickAction.Invoke();
                        getActiveButton().t.Enabled = true;
                        timer.Enabled = true;
                    }

                    if (getActiveButton().clickColor != null)
                        getActiveButton().color = getActiveButton().clickColor;
                }
                else
                {
                    if (!getActiveButton().t.Enabled)
                    {
                        getActiveButton().loadFrame(0);
                        if (getActiveButton().color != getActiveButton().originColor) getActiveButton().color = getActiveButton().originColor;
                    }
                }
            }

            if (animationActive)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float dept = 0.8f, SpriteEffects effect = SpriteEffects.None)
        {
            if(!animationActive)
                getActiveButton().Draw(spriteBatch, position + new Vector2(0, 0), dept, effect);
            else
            {
                getActiveButton().Draw(spriteBatch, position + new Vector2(0, 0), dept, effect);
            }
        }

        internal void startAnimation()
        {
            if (activeAnimationFrame > animatedFrames.Y) activeAnimationFrame = animatedFrames.X;
            setButton(activeAnimationFrame++);
            animationTimer.Start();
        }
        internal void stopAnimation()
        {
            animationTimer.Stop();
            activeAnimationFrame = animatedFrames.X;
            setButton(animatedFrames.X - 1);
        }

        void animationNext(object sender, ElapsedEventArgs e)
        {
            if (activeAnimationFrame > animatedFrames.Y) activeAnimationFrame = animatedFrames.X;
            setButton(activeAnimationFrame++);
        }
    }
}
