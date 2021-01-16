using Etap.ImagesCode;
using Etap.Source.EtapEngine;
using Etap.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Etap
{

    /*
     * Depth layer exlpained:
     * 0.0 = Hoteloverzicht
     * 0.1 = floor tiles
     * *CALCULATE WHERE FURNI RENDER LAYER IS*
     * 0.8 = Overlay
     * 0.9 = Menu's
     * 1.0 = hotel alert
     */
    public enum ImageFloat
    {
        NONE,
        CENTER,
        RIGHTBOTTOM,
        LEFTBOTTOM,
        RIGHTTOP,
        LEFTTOP
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager content;
        ResizeStatus resizing;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            resizing = new ResizeStatus();
        }

        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (int)GameScreenManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)GameScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();

            ///Window.Position = new Point(1000, 300);
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(OnResize);

            base.Initialize();
        }

        private void OnResize(object sender, EventArgs e)
        {
            resizing.Pending = true; // Resize is pending
            resizing.Width = Window.ClientBounds.Width;
            resizing.Height = Window.ClientBounds.Height;
        }

        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        protected override void LoadContent()
        {
            GameScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            content = this.Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameScreenManager.Instance.SpriteBatch = spriteBatch;
            GameScreenManager.Instance.LoadContent(Content);
        }

        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        protected override void UnloadContent()
        {
            GameScreenManager.Instance.UnloadContent();
        }

        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameScreenManager.Instance.Quit || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputManager.Instance.KeyPressed(Keys.Escape))
            {
                RetroEnvironment.CompleteShutdown();
                Exit();
            }

            if (resizing.Pending)
            {
                Console.WriteLine("Resizing");
                graphics.PreferredBackBufferWidth = resizing.Width;
                graphics.PreferredBackBufferHeight = resizing.Height;
                graphics.ApplyChanges();
                //camera.UpdateProjection(resizing.Width, resizing.Height);
                GameScreenManager.Instance.Dimensions.X = resizing.Width;
                GameScreenManager.Instance.Dimensions.Y = resizing.Height;
                resizing.Pending = false; // Resize is complete (no longer pending)
            }

            GameScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        /// This is called when the game should draw itself.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(GameScreenManager.Instance.BackgroundColor);
                //GraphicsDevice.Clear(Color.ForestGreen);

                spriteBatch.Begin(SpriteSortMode.FrontToBack);
                GameScreenManager.Instance.Draw();
                spriteBatch.End();

                base.Draw(gameTime);
            }catch(Exception)
            {
                Logger.Error("Missing a draw frame!!!");
            }
        }
    }
}
