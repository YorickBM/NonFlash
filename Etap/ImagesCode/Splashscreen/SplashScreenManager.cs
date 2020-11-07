using Etap.ImagesCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Splashscreen
{
    class SplashScreenManager
    {

        private bool started;
        private Image backdrop { get; set; }
        private Image logo { get; set; }
        private Image dynamicImage { get; set; }
        private Image loadingBar { get; set; }
        private Image progressBar { get; set; }

        public SplashScreenManager(ContentManager content, Vector2i options)
        {
            started = false;
            backdrop = new Image(content, "SplashScreen/Background", Vector2.Zero);
            logo = new Image(content, "SplashScreen/Logo", Vector2.Zero);

            Random r = new Random();
            int rInt = r.Next(options.X, options.Y + 1);
            dynamicImage = new Image(content, "SplashScreen/Dynamic/" + rInt, Vector2.Zero);

            loadingBar = new Image(content, "SplashScreen/LoadingBar", Vector2.Zero);
            progressBar = new Image(content, "SplashScreen/LoadingBarProgress", Vector2.Zero);
            setPercentage(5);
        }
       
        public void UnloadContent()
        {
            backdrop.UnloadContent();
            logo.UnloadContent();
            dynamicImage.UnloadContent();
            loadingBar.UnloadContent();
            progressBar.UnloadContent();
        }

        public void setPercentage(int percentage)
        {
            float value = percentage * 394;
            progressBar.resize((int)(value / 100), (int)progressBar.dimensions.Y + 2);
        }

        public void Update(GameTime gameTime)
        {
            backdrop.Update(gameTime);
            logo.Update(gameTime);
            dynamicImage.Update(gameTime);
            loadingBar.Update(gameTime);
            progressBar.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!started)
                return;

            Vector2 offset = new Vector2(0, -80);
            backdrop.Draw(spriteBatch, offset + calculateCenter(backdrop));
            logo.Draw(spriteBatch, offset + calculateCenter(logo));
            dynamicImage.Draw(spriteBatch, offset + calculateCenter(dynamicImage) + new Vector2(5, -6));

            //120 tekst
            loadingBar.Draw(spriteBatch, calculateCenter(loadingBar) +  new Vector2(0, -offset.Y + 160));
            progressBar.Draw(spriteBatch, calculateCenter(loadingBar) + new Vector2(4, -offset.Y + 163));
            //200 percentage
        }
        private Vector2 calculateCenter(Image img)
        {
            return new Vector2(GameScreenManager.Instance.Dimensions.X / 2 - img.dimensions.X / 2, GameScreenManager.Instance.Dimensions.Y / 2 - img.dimensions.Y / 2);
        }

        internal void Finish()
        {
            started = false; //Stop drawing again :)
        }
        internal void Start()
        {
            started = true;
        }
    }
}
