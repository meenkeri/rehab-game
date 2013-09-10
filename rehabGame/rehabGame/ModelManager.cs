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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ModelManager : DrawableGameComponent
    {
        public Matrix boardRotation = Matrix.Identity;
        public Matrix ballRotation = Matrix.Identity;

        public Matrix boardMovement = Matrix.Identity;
        public Matrix board1Movement = Matrix.Identity;
        public Matrix board2Movement = Matrix.Identity;
        public Matrix board3Movement = Matrix.Identity;
        public Matrix board4Movement = Matrix.Identity;

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
        Vector3 board1Position = new Vector3(0, 0, -100);
        Vector3 board2Position = new Vector3(0, 0, -200);
        Vector3 board3Position = new Vector3(0, 0, -300);
        Vector3 board4Position = new Vector3(0, 0, -400);
        Vector3 board5Position = new Vector3(0, 0, -500);
        
        public Matrix boardWorld = Matrix.Identity;
        public Matrix ballWorld = Matrix.Identity;
        Model[] balls = new Model[1];
        Model[] boards = new Model[5];

        public Wiimote bb = new Wiimote();

        public ModelManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            CreateLookAt();

            // Initialize projection matrix
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 1000);

            try
            {
                bb.Connect();
                bb.SetLEDs(1);
            }
            catch { Console.WriteLine("Can't find a BalanceBoard");}

            base.Initialize();
        }

        protected override void LoadContent()
        {
            balls[0] = Game.Content.Load<Model>(@"Models\ball");
            boards[0] = Game.Content.Load<Model>(@"Models\board");
            boards[1] = Game.Content.Load<Model>(@"Models\board");
            boards[2] = Game.Content.Load<Model>(@"Models\board");
            boards[3] = Game.Content.Load<Model>(@"Models\board");
            boards[4] = Game.Content.Load<Model>(@"Models\board");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Game1.currentGameState == Game1.GameState.PLAY)
            {
                boardUpdate();
                board1Update();
                board2Update();
                board3Update();
                board4Update();
                ballUpdate();
                ballUpdate1();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.currentGameState == Game1.GameState.PLAY)
            {
                BoardHelper.boardDraw(boardMovement, boards[0], boardRotation, projection, view);
                BoardHelper.boardDraw(board1Movement, boards[1], boardRotation, projection, view);
                BoardHelper.boardDraw(board2Movement, boards[2], boardRotation, projection, view);
                BoardHelper.boardDraw(board3Movement, boards[3], boardRotation, projection, view);
                BoardHelper.boardDraw(board4Movement, boards[4], boardRotation, projection, view);
                BallHelper.ballDraw(balls[0], ballWorld, ballRotation, projection, view);
            }
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

        public void board4Update()
        {
            boardRotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);

            //Move model
            board4Movement = Matrix.CreateTranslation(board4Position);

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
            if (ballPosition.Y >= 35)
                ballPosition.Y = 35;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= 0)
                cameraPosition.Z = 0;
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

        public bool IsCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);
                
                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);
                    
                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }
    }
}