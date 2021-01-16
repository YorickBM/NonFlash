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
using System.Timers;
using static Util.Button;

namespace Util.ButtonTypes
{
    public enum HoverConstructedButtonType { BASIC1, GREEN };
    class HoverConstructedButton
    {

        private Button crnRB, crnRT, crnLB, crnLT;
        private Button edgT, edgB, edgLT, edgLB, edgRT, edgRB;
        private Image backdropT, backdropB;

        private Font buttonName;
        private Rectangle area;
        private Timer t;
        private MyAction clickAction;

        private List<Button> buttons;
        private Color[] colorsT, colorsB;

        public HoverConstructedButton(ContentManager content, MyAction action, Vector2i size, HoverConstructedButtonType type, Color[] top, Color[] bottom, string text = "button.name.notfound", string font = "Fonts/InMenuButton")
        {
            area = new Rectangle(0, 0, size.X, size.Y);
            buttonName = new Font(content, font, text, Color.Black);
            clickAction = action;

            buttons = new List<Button>();
            colorsT = top;
            colorsB = bottom;

            t = new Timer(100);
            t.Elapsed += new ElapsedEventHandler(t_reset);

            crnRB = new Button(content, "Buttons/" + type.ToString() + "/cornerBottom", new Vector2i(3,1), () => { }); buttons.Add(crnRB);
            crnRT = new Button(content, "Buttons/" + type.ToString() + "/cornerTop", new Vector2i(3,1), () => { }); buttons.Add(crnRT);
            crnLB = new Button(content, "Buttons/" + type.ToString() + "/cornerBottom", new Vector2i(3,1), () => { }); buttons.Add(crnLB);
            crnLT = new Button(content, "Buttons/" + type.ToString() + "/cornerTop", new Vector2i(3,1), () => { }); buttons.Add(crnLT);

            edgT = new Button(content, "Buttons/" + type.ToString() + "/edgeTop", new Vector2i(1, 3), () => { }); buttons.Add(edgT);
            edgT.ResizeFrame(new Vector2i(size.X - (crnLT.framesDimensions.X * 2), edgT.framesDimensions.Y));
            edgB = new Button(content, "Buttons/" + type.ToString() + "/edgeBottom", new Vector2i(1, 3), () => { }); buttons.Add(edgB);
            edgB.ResizeFrame(new Vector2i(size.X - (crnLT.framesDimensions.X * 2), edgT.framesDimensions.Y));

            edgLT = new Button(content, "Buttons/" + type.ToString() + "/edgeLeftTop", new Vector2i(3, 1), () => { }); buttons.Add(edgLT);
            edgLT.ResizeFrame(new Vector2i(crnLB.framesDimensions.X, (size.Y / 2) - crnLT.framesDimensions.Y));
            edgLB = new Button(content, "Buttons/" + type.ToString() + "/edgeLeftBottom", new Vector2i(3, 1), () => { }); buttons.Add(edgLB);
            edgLB.ResizeFrame(new Vector2i(crnLB.framesDimensions.X, (size.Y / 2) - crnLT.framesDimensions.Y));

            edgRT = new Button(content, "Buttons/" + type.ToString() + "/edgeLeftTop", new Vector2i(3, 1), () => { }); buttons.Add(edgRT);
            edgRT.ResizeFrame(new Vector2i(crnRB.framesDimensions.X, (size.Y / 2) - crnRT.framesDimensions.Y));
            edgRB = new Button(content, "Buttons/" + type.ToString() + "/edgeLeftBottom", new Vector2i(3, 1), () => { }); buttons.Add(edgRB);
            edgRB.ResizeFrame(new Vector2i(crnRB.framesDimensions.X, (size.Y / 2) - crnRT.framesDimensions.Y));

            backdropT = new Image(content, "Buttons/" + type.ToString() + "/filler", new Vector2i(size.X - (crnLT.framesDimensions.X * 2), (size.Y / 2) - crnRT.framesDimensions.Y));
            backdropB = new Image(content, "Buttons/" + type.ToString() + "/filler", new Vector2i(size.X - (crnLT.framesDimensions.X * 2), (size.Y / 2) - crnRT.framesDimensions.Y));
        }

        public void SetPosition(Vector2i pos) { area.X = pos.X; area.Y = pos.Y; }
        public Vector2i GetSize() { return new Vector2i(area.Width, area.Height); }

        private bool isHovered = false;
        private bool isClicked = false;
        public void Update(GameTime gameTime)
        {
            if (RetroEnvironment.GetLanguageManager() != null) buttonName.SetText(RetroEnvironment.GetLanguageManager().TryGetValue(buttonName.GetOriginalText()));
            buttonName.Update(gameTime);

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (area.Contains(mousePoint))
            {
                isHovered = true;
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                isHovered = false;
                isClicked = false;
            }

            if (isHovered && !isClicked)
            {
                if (!t.Enabled)
                {
                    buttons.ForEach(s => s.loadFrame(2));
                    if (backdropB.GetColor() != colorsB[0]) backdropB.SetColor(colorsB[2]);
                    if (backdropT.GetColor() != colorsT[0]) backdropT.SetColor(colorsT[2]);
                }
            }
            else if (isClicked)
            {
                buttons.ForEach(s => s.loadFrame(1));
                if (backdropB.GetColor() != colorsB[1]) backdropB.SetColor(colorsB[1]);
                if (backdropT.GetColor() != colorsT[1]) backdropT.SetColor(colorsT[1]);
                if (!t.Enabled)
                {
                    clickAction.Invoke();
                    t.Enabled = true;
                }
            }
            else
            {
                if (!t.Enabled)
                {
                    buttons.ForEach(s => s.loadFrame(0));
                    if(backdropB.GetColor() != colorsB[0]) backdropB.SetColor(colorsB[0]);
                    if (backdropT.GetColor() != colorsT[0]) backdropT.SetColor(colorsT[0]);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2i pos, float depth)
        {
            SetPosition(pos);

            crnRB.Draw(spriteBatch, pos + new Vector2i(GetSize().X - crnRB.framesDimensions.X, GetSize().Y - crnRB.framesDimensions.Y), depth, SpriteEffects.FlipHorizontally);
            crnRT.Draw(spriteBatch, pos + new Vector2i(GetSize().X - crnRB.framesDimensions.X, 0), depth, SpriteEffects.FlipHorizontally);
            crnLB.Draw(spriteBatch, pos + new Vector2i(0, GetSize().Y - crnRB.framesDimensions.Y), depth, SpriteEffects.None);
            crnLT.Draw(spriteBatch, pos + new Vector2i(0, 0), depth, SpriteEffects.None);

            edgT.Draw(spriteBatch, pos + new Vector2i(crnLT.framesDimensions.X, 0), depth, SpriteEffects.None);
            edgB.Draw(spriteBatch, pos + new Vector2i(crnLB.framesDimensions.X, GetSize().Y - edgB.framesDimensions.Y), depth, SpriteEffects.None);
            edgLT.Draw(spriteBatch, pos + new Vector2i(0, crnLT.framesDimensions.Y), depth, SpriteEffects.None);
            edgLB.Draw(spriteBatch, pos + new Vector2i(0, GetSize().Y - crnLB.framesDimensions.Y - edgLB.framesDimensions.Y), depth, SpriteEffects.None);
            edgRT.Draw(spriteBatch, pos + new Vector2i(GetSize().X - 3, crnLT.framesDimensions.Y), depth, SpriteEffects.FlipHorizontally);
            edgRB.Draw(spriteBatch, pos + new Vector2i(GetSize().X - 3, GetSize().Y - crnLB.framesDimensions.Y - edgLB.framesDimensions.Y), depth, SpriteEffects.FlipHorizontally);

            backdropT.Draw(spriteBatch, pos + new Vector2i(edgLT.framesDimensions.X, edgT.framesDimensions.Y), depth, SpriteEffects.None);
            backdropB.Draw(spriteBatch, pos + new Vector2i(edgLT.framesDimensions.X, edgT.framesDimensions.Y + backdropT.Size.Y), depth, SpriteEffects.None);

            buttonName.Draw(spriteBatch, pos + new Vector2i((GetSize().X / 2) - (buttonName.measureString().X / 2), (GetSize().Y / 2) - (buttonName.measureString().Y/2)), depth + 0.01f);
        }

        public void UnloadContent()
        {
            buttons.ForEach(s => s.UnloadContent());
            buttons.Clear();
        }

        private void t_reset(object sender, ElapsedEventArgs e)
        {
            t.Enabled = false;
        }
    }
}
