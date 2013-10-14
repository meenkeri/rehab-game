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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SplashScreen : DrawableGameComponent
    {
        string textToDraw;
        string secondaryTextToDraw;
        SpriteFont spriteFont;
        SpriteFont secondarySpriteFont;
        SpriteBatch spriteBatch;
        Game1.GameState currentGameState;
        
        public SplashScreen(Game game)
            : base(game)
        {
            Log.logger.Info("Loading SplashScreen constructor");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Log.logger.Info("Initializing the SplashScreen");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Log.logger.Info("Loading the SplashScreen content");
            //Load fonts
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\SplashScreenFontLarge");
            secondarySpriteFont = Game.Content.Load<SpriteFont>(@"Fonts\SplashScreenFont");
            
            //Create sprite batch
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Did player hit enter?
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                //If we're not in end game, move to play state
                if (currentGameState == Game1.GameState.LEVEL_CHANGE || currentGameState == Game1.GameState.START)
                    ((Game1)Game).ChangeGameState(Game1.GameState.PLAY, 0);

                //If we are in end game, exit
                else if (currentGameState == Game1.GameState.END)
                    Game.Exit();
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Get size of string
            Vector2 TitleSize = spriteFont.MeasureString(textToDraw);

            //Draw main text
            spriteBatch.DrawString(spriteFont, textToDraw, new Vector2(Game.Window.ClientBounds.Width / 2 - TitleSize.X / 2, Game.Window.ClientBounds.Height / 2), Color.White);

            //Draw subtext
            spriteBatch.DrawString(secondarySpriteFont, secondaryTextToDraw, new Vector2(Game.Window.ClientBounds.Width / 2 - secondarySpriteFont.MeasureString(secondaryTextToDraw).X / 2, Game.Window.ClientBounds.Height / 2 + TitleSize.Y + 10), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void SetData(string main, Game1.GameState currGameState)
        {
            textToDraw = main;
            this.currentGameState = currGameState;

            switch (currentGameState)
            {
                case Game1.GameState.START:
                case Game1.GameState.LEVEL_CHANGE:
                    secondaryTextToDraw = IConstants.SUB_TITLE;
                    break;
                case Game1.GameState.END:
                    secondaryTextToDraw = IConstants.QUIT;
                    break;
            }
        }
    }
}
