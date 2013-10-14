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
        
        Level1 level1;
        Level2 level2;

        public enum GameState { START, PLAY, LEVEL_CHANGE, END }
        public static GameState currentGameState = GameState.START;
        public enum Level { ONE, TWO, THREE }
        public static Level currentLevel = Level.ONE;

        SplashScreen splashScreen;
        Backgrounds background;
        SpriteFont scoreFont;
        public static int score = 0;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue backgroundCue;
        Cue levelUpCue;

        public Game1()
        {
            Log.logger.Info("Loading the game1 constructor");
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            //Enable Full Screen Mode
            graphics.IsFullScreen = true;
            new BalanceBoard().setupBalanceBoard();
            new Log();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Log.logger.Info("Initializing the game1");
            //camera = new Camera(this, new Vector3(0, 150, 120), Vector3.Zero, Vector3.Down);
            //Components.Add(camera);
            
            level1 = new Level1(this);
            Components.Add(level1);
            level1.Enabled = false;
            level1.Visible = false;

            level2 = new Level2(this);
            Components.Add(level2);
            level2.Enabled = false;
            level2.Visible = false;

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
            Log.logger.Info("Loading the game1 content");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");

            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            backgroundCue = soundBank.GetCue("background");
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
            GraphicsDevice.Clear(Color.Black);
            if (!blank)
                base.Draw(gameTime);
            spriteBatch.Begin();
            if (currentGameState == GameState.PLAY)
            {
                //Draw the current score
                string scoreText = IConstants.SCORE + score;
                spriteBatch.DrawString(scoreFont, scoreText, new Vector2(10, 10), Color.White);
            }

            spriteBatch.End();
        }

        public void ChangeGameState(GameState state, int level)
        {
            currentGameState = state;

            switch (currentGameState)
            {
                case GameState.LEVEL_CHANGE:
                    splashScreen.SetData("Level " + (level + 1), GameState.LEVEL_CHANGE);
                    Log.logger.Info("Level changed to " + level);
                    level1.Enabled = false;
                    level1.Visible = false;
                    level2.Enabled = false;
                    level2.Visible = false;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    backgroundCue.Stop(AudioStopOptions.Immediate);
                    levelUpCue = soundBank.GetCue("levelUp");
                    levelUpCue.Play();
                    addScore(50);
                    break;
                case GameState.PLAY:
                    switch (currentLevel)
                    {
                        case Level.ONE:
                        level1.Enabled = true;
                        level1.Visible = true;
                        break;

                        case Level.TWO:
                        level2.Enabled = true;
                        level2.Visible = true;
                        break;
                    }
                    splashScreen.Enabled = false;
                    splashScreen.Visible = false;
                    background.Enabled = false;
                    background.Visible = false;
                    if (backgroundCue.IsPlaying)
                        backgroundCue.Stop(AudioStopOptions.Immediate);
                    backgroundCue = soundBank.GetCue("background");
                    backgroundCue.Play();
                    break;
                case GameState.END:
                    splashScreen.SetData(IConstants.GAME_OVER, GameState.END);
                    Log.logger.Info("Game ended");
                    level1.Enabled = false;
                    level1.Visible = false;
                    level2.Enabled = false;
                    level2.Visible = false;
                    background.Enabled = true;
                    background.Visible = true;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    backgroundCue.Stop(AudioStopOptions.Immediate);
                    break;
            }
        }

        public static void addScore(int points)
        {
            score += points;
        }
    }
}