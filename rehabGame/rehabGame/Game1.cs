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
        //GameDeviceManager
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //Proprioception
        public static Boolean blank = false;
        
        //Game levels
        Level1 level1;
        Level2 level2;
        Level3 level3;

        //Game audio
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue backgroundCue;
        Cue levelUpCue;

        //GameState
        public enum GameState { START, PLAY, LEVEL_CHANGE, END }
        public static GameState currentGameState = GameState.START;
        
        //Current level
        public enum Level { ONE, TWO, THREE }
        public static Level currentLevel = Level.ONE;

        //SplashScreen and background
        SplashScreen splashScreen;
        Backgrounds background;
        
        //Font text display
        SpriteFont scoreFont;
        SpriteFont timeFont;
        public static float time = 30;
        public static int score = 0;

        public Game1()
        {
            Log.logger.Info("Loading the game1 constructor");
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            //Enable Full Screen Mode
            graphics.IsFullScreen = true;
            
            new BalanceBoard();
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
            
            //Level Components
            level1 = new Level1(this);
            Components.Add(level1);
            level1.Enabled = false;
            level1.Visible = false;

            level2 = new Level2(this);
            Components.Add(level2);
            level2.Enabled = false;
            level2.Visible = false;

            level3 = new Level3(this);
            Components.Add(level3);
            level3.Enabled = false;
            level3.Visible = false;

            //Background image component
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
            //Text on the screen
            scoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");
            timeFont = Content.Load<SpriteFont>(@"Fonts\TimeFont");

            //Audio
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            backgroundCue = soundBank.GetCue("background");

            base.LoadContent();
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
            
            //Time countdown - End the game after 30 seconds
            if (currentGameState == GameState.PLAY)
            {
                time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time < 0)
                    ChangeGameState(Game1.GameState.END, 0);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //2D on top of 3D
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            //Background color
            GraphicsDevice.Clear(Color.Black);
            
            //Game visual control
            if (!blank)
                base.Draw(gameTime);
            
            spriteBatch.Begin();
            if (currentGameState == GameState.PLAY)
            {
                //Draw the current score
                string scoreText = IConstants.SCORE + score;
                spriteBatch.DrawString(scoreFont, scoreText, new Vector2(10, 10), Color.White);

                //Draw the time left
                string timeText = IConstants.TIME + (int)time;
                spriteBatch.DrawString(timeFont, timeText, new Vector2((Window.ClientBounds.Width) - 180, 10), Color.White);
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
                    level3.Enabled = false;
                    level3.Visible = false;
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

                        case Level.THREE:
                        level3.Enabled = true;
                        level3.Visible = true;
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
                    level3.Enabled = false;
                    level3.Visible = false;
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