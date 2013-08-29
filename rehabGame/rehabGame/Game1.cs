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
using WiimoteLib;

namespace rehabGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Matrix boardRotation = Matrix.Identity;
        public Matrix board1Movement = Matrix.Identity;
        public Matrix board2Movement = Matrix.Identity;
        public Matrix board3Movement = Matrix.Identity;
        public Matrix ballRotation = Matrix.Identity;
        public Matrix boardMovement = Matrix.Identity;

        Vector3 pos = new Vector3(0, 150, 100);
        Vector3 target = Vector3.Zero;
        Vector3 cameraPosition;
        Vector3 cameraDirection;

        float yawAngle = 0; //Left Right
        float pitchAngle = 0; // Up Down
        float rollAngle = 0; //Do nothing

        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        Vector3 ballPosition = new Vector3(0, -4.1F, 0);
        Vector3 board1Position = new Vector3(0, 0, -200);
        Vector3 board2Position = new Vector3(0, 0, -400);
        Vector3 board3Position = new Vector3(0, 0, -800);

        public Matrix boardWorld = Matrix.Identity;
        public Matrix ballWorld = Matrix.Identity;
        Model[] balls = new Model[1];
        Model[] boards = new Model[5];

        public Wiimote bb = new Wiimote();

        public static Boolean blank = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera camera { get; protected set; }
        
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
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            CreateLookAt();

            // Initialize projection matrix
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Window.ClientBounds.Width /
                (float)Window.ClientBounds.Height,
                1, 1000);

            try
            {
                bb.Connect();
                bb.SetLEDs(1);
            }
            catch { Console.WriteLine("Can't find a BalanceBoard"); }
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
            balls[0] = Content.Load<Model>(@"Models\ball");
            boards[0] = Content.Load<Model>(@"Models\board");
            boards[1] = Content.Load<Model>(@"Models\board");
            boards[2] = Content.Load<Model>(@"Models\board");
            boards[3] = Content.Load<Model>(@"Models\board");

            // TODO: use this.Content to load your game content here
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

            boardUpdate();
            board1Update();
            board2Update();
            board3Update();
            ballUpdate();
            ballUpdate1();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            BoardHelper.boardDraw(boardMovement, boards[0], boardRotation, projection, view);
            BoardHelper.boardDraw(board1Movement, boards[1], boardRotation, projection, view);
            BoardHelper.boardDraw(board2Movement, boards[2], boardRotation, projection, view);
            BoardHelper.boardDraw(board3Movement, boards[3], boardRotation, projection, view);
            BallHelper.ballDraw(balls[0], ballWorld, ballRotation, projection, view);
            if (!blank)
                base.Draw(gameTime); 
        }

        public void boardUpdate()
        {
            WiimoteState s = bb.WiimoteState;
            BalanceBoardState bbs = s.BalanceBoardState;
            yawAngle -= bbs.CenterOfGravity.X * 0.0009F;
            pitchAngle -= bbs.CenterOfGravity.Y * 0.0009F;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                yawAngle += 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                yawAngle -= 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                pitchAngle += 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                pitchAngle -= 0.01F;
            if (yawAngle >= 0.05F)
                yawAngle = 0.05F;
            if (yawAngle <= -0.05F)
                yawAngle = -0.05F;
            if (pitchAngle >= 0.05F)
                pitchAngle = 0.05F;
            if (pitchAngle <= -0.05F)
                pitchAngle = -0.05F;
        }

        public void board1Update()
        {
            boardRotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);

            //Move model
            board1Movement = Matrix.CreateTranslation(board1Position);

            //Rotate model
            boardRotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        }

        public void board2Update()
        {
            boardRotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);

            //Move model
            board2Movement = Matrix.CreateTranslation(board2Position);

            //Rotate model
            boardRotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        }

        public void board3Update()
        {
            boardRotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);

            //Move model
            board3Movement = Matrix.CreateTranslation(board3Position);

            //Rotate model
            boardRotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        }
        public void ballUpdate()
        {
            WiimoteState s = bb.WiimoteState;
            BalanceBoardState bbs = s.BalanceBoardState;
            ballPosition.X -= bbs.CenterOfGravity.X * 0.05F;
            ballPosition.Z += bbs.CenterOfGravity.Y * 0.05F;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                ballPosition += Vector3.Left * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                ballPosition += Vector3.Right * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                ballPosition += Vector3.Forward * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                ballPosition += Vector3.Backward * 0.2F;

            if (ballPosition.X < -24)
                ballPosition.X = -24;
            if (ballPosition.X > 24)
                ballPosition.X = 24;
            if (ballPosition.Z < -18)
                ballPosition.Z = -18;
            if (ballPosition.Z > 17)
                ballPosition.Z = 17;
        }

        public void ballUpdate1()
        {
            if (ballPosition.X > 12 && ballPosition.Y >= -4.1F && ballPosition.Y < 76 && ballPosition.Z < -12)
                dropTheBall();

            if (ballPosition.X < 12 && ballPosition.Y >= 76 && ballPosition.Y < 160 && ballPosition.Z < 12)
                dropTheBall1();

            //Move model
            ballWorld = Matrix.CreateTranslation(ballPosition);
        }

        public void dropTheBall()
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= 76)
                ballPosition.Y = 76;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -90)
                cameraPosition.Z = -90;
            CreateLookAt();

        }

        public void dropTheBall1()
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= 153)
                ballPosition.Y = 153;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -280)
                cameraPosition.Z = -280;
            CreateLookAt();

        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, Vector3.Down);
        }
    }
}