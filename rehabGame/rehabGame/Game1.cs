using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace rehabGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Boolean blank = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera camera { get; protected set; }
        ModelManager modelManager;
        public enum GameState { START, PLAY, LEVEL_CHANGE, END }
        public static GameState currentGameState = GameState.START;
        SplashScreen splashScreen;
        Backgrounds background;
        SpriteFont scoreFont;
        public static int score = 0;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            //Enable Full Screen Mode
            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //camera = new Camera(this, new Vector3(0, 150, 120), Vector3.Zero, Vector3.Down);
            //Components.Add(camera);
            modelManager = new ModelManager(this);
            Components.Add(modelManager);
            modelManager.Enabled = false;
            modelManager.Visible = false;

            //Background images component
            background = new Backgrounds(this);
            Components.Add(background);
            background.Enabled = true;
            background.Visible = true;

            //Splash screen component
            splashScreen = new SplashScreen(this);
            Components.Add(splashScreen);
            splashScreen.SetData(IConstants.TITLE, currentGameState);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (currentGameState == GameState.END)
                ChangeGameState(GameState.END, 0);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.Clear(Color.Gray);
            if (!blank)
                base.Draw(gameTime);
            spriteBatch.Begin();
            if (currentGameState == GameState.PLAY)
            {
                //Draw the current score
                string scoreText = IConstants.SCORE + score;
                spriteBatch.DrawString(scoreFont, scoreText, new Vector2(10, 10), Color.Red);
            }

            spriteBatch.End();
        }

        public void ChangeGameState(GameState state, int level)
        {
            currentGameState = state;

            switch (currentGameState)
            {
                case GameState.LEVEL_CHANGE:
                    //ToDo
                    break;
                case GameState.PLAY:
                    modelManager.Enabled = true;
                    modelManager.Visible = true;
                    splashScreen.Enabled = false;
                    splashScreen.Visible = false;
                    background.Enabled = false;
                    background.Visible = false;
                    break;
                case GameState.END:
                    splashScreen.SetData(IConstants.GAME_OVER, GameState.END);
                    modelManager.Enabled = false;
                    modelManager.Visible = false;
                    background.Enabled = true;
                    background.Visible = true;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    break;
            }
        }

        public static void addScore(int points)
        {
            score += points;
        }
    }
}