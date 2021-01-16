using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Textbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Textfield
{
    class TextField
    {
        private TextBox textBox;
        private SpriteFont font;
        private Rectangle area;

        private Image crnBtmLft, crnBtmRght, crnTopLft, crnTopRght;
        private Image edgeLft, edgeTop, edgeBtm, edgeRght;

        private Image crnBtmLftB, crnBtmRghtB, crnTopLftB, crnTopRghtB;
        private Image lftB, rgthB, mdlB;

        public TextField(ContentManager content, string fontName, Rectangle area, GraphicsDevice graphicsDevice, string type = "")
        {
            font = content.Load<SpriteFont>(fontName);
            this.area = area;
            //textBox = new TextBox(area, 200, "This is a test. Move the cursor, select, delete, write...", graphicsDevice, font, Color.LightGray, Color.DarkGreen, 30);

            //float margin = 3;
            //textBox.Area = new Rectangle((int)(area.X + margin), area.Y, (int)(area.Width - margin),
            //    area.Height);
            //textBox.Renderer.Color = Color.White;
            //textBox.Cursor.Selection = new Color(Color.Purple, .4f);

            if (type.Equals(""))
            {
                crnBtmLft = new Image(content, "textfield/cornerb", Vector2.Zero);
                crnBtmRght = new Image(content, "textfield/cornerb", Vector2.Zero);
                crnTopLft = new Image(content, "textfield/corner", Vector2.Zero);
                crnTopRght = new Image(content, "textfield/corner", Vector2.Zero);

                edgeLft = new Image(content, "textfield/edge", Vector2.Zero);
                edgeTop = new Image(content, "textfield/edge", Vector2.Zero);
                edgeBtm = new Image(content, "textfield/edge", Vector2.Zero);
                edgeRght = new Image(content, "textfield/edge", Vector2.Zero);

                lftB = new Image(content, "textfield/backdrop", Vector2.Zero);
                rgthB = new Image(content, "textfield/backdrop", Vector2.Zero);
                mdlB = new Image(content, "textfield/backdrop", Vector2.Zero);

                crnBtmLftB = new Image(content, "textfield/cornerBackdropB", Vector2.Zero);
                crnBtmRghtB = new Image(content, "textfield/cornerBackdropB", Vector2.Zero);
                crnTopLftB = new Image(content, "textfield/cornerBackdrop", Vector2.Zero);
                crnTopRghtB = new Image(content, "textfield/cornerBackdrop", Vector2.Zero);
            } else
            {
                crnBtmLft = new Image(content, "textfield/" + type + "/cornerb", Vector2.Zero);
                crnBtmRght = new Image(content, "textfield/" + type + "/cornerb", Vector2.Zero);
                crnTopLft = new Image(content, "textfield/" + type + "/corner", Vector2.Zero);
                crnTopRght = new Image(content, "textfield/" + type + "/corner", Vector2.Zero);

                edgeLft = new Image(content, "textfield/" + type + "/edge", Vector2.Zero);
                edgeTop = new Image(content, "textfield/" + type + "/edge", Vector2.Zero);
                edgeBtm = new Image(content, "textfield/" + type + "/edge", Vector2.Zero);
                edgeRght = new Image(content, "textfield/" + type + "/edge", Vector2.Zero);

                lftB = new Image(content, "textfield/" + type + "/backdrop", Vector2.Zero);
                rgthB = new Image(content, "textfield/" + type + "/backdrop", Vector2.Zero);
                mdlB = new Image(content, "textfield/" + type + "/backdrop", Vector2.Zero);

                crnBtmLftB = new Image(content, "textfield/" + type + "/cornerBackdropB", Vector2.Zero);
                crnBtmRghtB = new Image(content, "textfield/" + type + "/cornerBackdropB", Vector2.Zero);
                crnTopLftB = new Image(content, "textfield/" + type + "/cornerBackdrop", Vector2.Zero);
                crnTopRghtB = new Image(content, "textfield/" + type + "/cornerBackdrop", Vector2.Zero);
            }

            SetEdgeColor(Color.Black);
        }

        public void Update(GameTime gameTime)
        {
            //float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);
            //textBox.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            edgeLft.resize(new Vector2i(1, area.Height - crnTopLft.GetTexture().Height - crnBtmLft.GetTexture().Height));
            edgeTop.resize(new Vector2i(area.Width - crnTopLft.GetTexture().Width - crnTopRght.GetTexture().Width, edgeTop.GetTexture().Height));
            edgeBtm.resize(new Vector2i(area.Width - crnBtmLft.GetTexture().Width - crnBtmRght.GetTexture().Width, edgeBtm.GetTexture().Height));
            edgeRght.resize(new Vector2i(1, area.Height - crnTopRght.GetTexture().Height - crnBtmRght.GetTexture().Height));

            mdlB.resize(new Vector2i(area.Width - crnTopLft.GetTexture().Width - crnTopRght.GetTexture().Width, area.Height - edgeTop.GetTexture().Height - edgeBtm.GetTexture().Height));
            lftB.resize(new Vector2i(crnTopLft.GetTexture().Width, area.Height - crnTopLft.GetTexture().Height - crnBtmLft.GetTexture().Height));
            rgthB.resize(new Vector2i(crnTopRght.GetTexture().Width, area.Height - crnTopRght.GetTexture().Height - crnBtmRght.GetTexture().Height));

            //textBox.Active = true;
            //textBox.Update();
        }

        public void SetEdgeColor(Color color)
        {
            crnBtmLft.SetColor(color);
            crnBtmRght.SetColor(color);
            crnTopLft.SetColor(color);
            crnTopRght.SetColor(color);

            edgeLft.SetColor(color);
            edgeTop.SetColor(color);
            edgeBtm.SetColor(color);
            edgeRght.SetColor(color);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float depth)
        {
            crnBtmLft.Draw(spriteBatch, position + new Vector2i(0, area.Height - crnBtmLft.GetTexture().Height), depth, SpriteEffects.None);
            crnBtmRght.Draw(spriteBatch, position + new Vector2i(area.Width - crnBtmRght.GetTexture().Width, area.Height - crnBtmLft.GetTexture().Height), depth, SpriteEffects.FlipHorizontally);
            crnTopLft.Draw(spriteBatch, position + new Vector2i(0,0), depth, SpriteEffects.None);
            crnTopRght.Draw(spriteBatch, position + new Vector2i(area.Width - crnTopRght.GetTexture().Width,0), depth, SpriteEffects.FlipHorizontally);

            crnBtmLftB.Draw(spriteBatch, position + new Vector2i(0, area.Height - crnBtmLft.GetTexture().Height), depth, SpriteEffects.None);
            crnBtmRghtB.Draw(spriteBatch, position + new Vector2i(area.Width - crnBtmRght.GetTexture().Width, area.Height - crnBtmLft.GetTexture().Height), depth, SpriteEffects.FlipHorizontally);
            crnTopLftB.Draw(spriteBatch, position + new Vector2i(0, 0), depth, SpriteEffects.None);
            crnTopRghtB.Draw(spriteBatch, position + new Vector2i(area.Width - crnTopRght.GetTexture().Width, 0), depth, SpriteEffects.FlipHorizontally);

            edgeLft.Draw(spriteBatch, position + new Vector2i(0, crnTopLft.GetTexture().Height), depth, SpriteEffects.None);
            edgeTop.Draw(spriteBatch, position + new Vector2i(crnTopLft.GetTexture().Width, 0), depth, SpriteEffects.None);
            edgeBtm.Draw(spriteBatch, position + new Vector2i(crnBtmLft.GetTexture().Width, area.Height - edgeBtm.GetTexture().Height), depth, SpriteEffects.None);
            edgeRght.Draw(spriteBatch, position + new Vector2i(area.Width - edgeRght.GetTexture().Width, crnTopRght.GetTexture().Height), depth, SpriteEffects.None);

            lftB.Draw(spriteBatch, position + new Vector2i(edgeLft.GetTexture().Width, crnTopLft.GetTexture().Height), depth, SpriteEffects.None);
            mdlB.Draw(spriteBatch, position + new Vector2i(crnTopLft.GetTexture().Width, edgeTop.GetTexture().Height), depth, SpriteEffects.None);
            rgthB.Draw(spriteBatch, position + new Vector2i(lftB.SourceRect.Width + mdlB.SourceRect.Width - edgeRght.GetTexture().Width, crnTopRght.GetTexture().Height), depth, SpriteEffects.None);

            //textBox.Draw(spriteBatch, depth + 0.1f);
        }

        public void UnloadContent()
        {
            crnBtmLft.UnloadContent();
            crnBtmRght.UnloadContent();
            crnTopLft.UnloadContent();
            crnTopRght.UnloadContent();

            crnBtmLftB.UnloadContent();
            crnBtmRghtB.UnloadContent();
            crnTopLftB.UnloadContent();
            crnTopRghtB.UnloadContent();

            edgeLft.UnloadContent();
            edgeTop.UnloadContent();
            edgeBtm.UnloadContent();
            edgeRght.UnloadContent();

            lftB.UnloadContent();
            mdlB.UnloadContent();
            rgthB.UnloadContent();
        }
    }
}
