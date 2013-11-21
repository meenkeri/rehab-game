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
    public class Backgrounds : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D startBackground;

        public Backgrounds(Game game)
            : base(game)
        {
            Log.logger.Info("Loading Backgrounds constructor");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Log.logger.Info("Initializing the Background");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Log.logger.Info("Loading the Background content");
            //Create sprite batch
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            startBackground = Game.Content.Load<Texture2D>(@"Textures\background");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (Game1.currentGameState == Game1.GameState.START)
            {
                //Draw the start screen background image
                spriteBatch.Draw(startBackground, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.Gray);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
