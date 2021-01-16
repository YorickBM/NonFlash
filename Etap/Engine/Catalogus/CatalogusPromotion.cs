using Etap.ImagesCode;
using Etap.Utilities;
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
using Util;
using Util.ButtonTypes;

namespace Engine.Catalogus
{
    class CatalogusPromotion
    {
        private string _pageLink;
        private string _title;
        private int attempts = 0;

        private ContentManager _content;
        private Image _image;
        private Font _text;
        private Image _backdrop0;
        private Image _backdrop1;

        private Rectangle _area;
        internal Timer t;

        public CatalogusPromotion(ContentManager content)
        {
            _content = content;
            _backdrop0 = new Image(content, "catalogue/promotion-0", Vector2.Zero);
            _backdrop1 = new Image(content, "catalogue/promotion-1", Vector2.Zero);
            _text = new Font(content, "Fonts/CataPromoTitle", "catalogue.promotion.empty");
            _image = null;

            t = new Timer(100);
            t.Elapsed += new ElapsedEventHandler(t_reset);
        }

        internal string GetPageLink() { return _pageLink; }
        internal string GetTitle() { return _title; }

        public void SetSize(Vector2i s)
        {
            _area.Width = s.X;
            _area.Height = s.Y;
            if(_image != null) _image.resizeCenter(s.X, s.Y);
        }
        public void SetPosition(Vector2i s)
        {
            _area.X = s.X;
            _area.Y = s.Y;
        }
        public Vector2i GetSize() { return new Vector2i(_area.Width, _area.Height); }
        public Vector2i GetPosition() { return new Vector2i(_area.X, _area.Y); }

        public void Unloadcontent()
        {
            _backdrop0.UnloadContent();
            _backdrop1.UnloadContent();
            _text.UnloadContent();
            if (_image != null) _image.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch, float depth)
        {
            if (_image != null) _image.Draw(spriteBatch, GetPosition(), depth);
            if (GetSize().X < 200) _backdrop0.Draw(spriteBatch, GetPosition() + new Vector2(5, GetSize().Y - 3 - _backdrop0.GetTexture().Height), depth + 0.01f);
            else _backdrop1.Draw(spriteBatch, GetPosition() + new Vector2(5, GetSize().Y - 3 - _backdrop0.GetTexture().Height), depth + 0.01f);
            _text.Draw(spriteBatch, GetPosition() + new Vector2(10, GetSize().Y - _text.measureString().Y - 10), depth + 0.02f);
        }

        private bool isClicked = false;
        public void Update(GameTime gameTime) 
        {
            _text.SetText(_title);
            _text.Update(gameTime);

            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (_area.Contains(mousePoint))
            {
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                isClicked = false;
            }

            if(isClicked)
            {
                if (!t.Enabled)
                {
                    Logger.Debug("Opening promotion page:", _pageLink);
                    //GameScreenManager.Instance.GetCatalogusManager().OpenPage(_pageLink);
                    t.Enabled = true;
                }
            }
        }

        internal void SetTitle(string title)
        {
            _title = title;
        }

        internal void SetPageLink(string pageLink)
        {
            _pageLink = pageLink;
        }

        internal void SetImage(string image)
        {
            _image = new Image(_content, image, Vector2.Zero);
        }

        private void t_reset(object sender, ElapsedEventArgs e)
        {
            t.Enabled = false;
        }
    }
}
