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
    public class Level1 : DrawableGameComponent
    {
        public Matrix board1Rotation = Matrix.Identity;
        public Matrix board2Rotation = Matrix.Identity;
        public Matrix board3Rotation = Matrix.Identity;
        public Matrix board4Rotation = Matrix.Identity;
        public Matrix board5Rotation = Matrix.Identity;

        public Matrix ballRotation = Matrix.Identity;

        public Matrix wallRotation = Matrix.Identity;

        public Matrix board1Movement = Matrix.Identity;
        public Matrix board2Movement = Matrix.Identity;
        public Matrix board3Movement = Matrix.Identity;
        public Matrix board4Movement = Matrix.Identity;
        public Matrix board5Movement = Matrix.Identity;

        Vector3 pos = new Vector3(0, 150, 100);
        Vector3 target = Vector3.Zero;
        Vector3 cameraPosition;
        Vector3 cameraDirection;
        
        float yawAngle = 0; //Left Right
        float pitchAngle = 0; // Up Down
        float rollAngle = 0; //Do nothing

        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        Vector3 ballPosition = new Vector3(0, -8F, 0);
        
        Vector3 boardPosition = new Vector3(0, 0, 0);
        Vector3 board1Position = new Vector3(0, 0, -100);
        Vector3 board2Position = new Vector3(0, 0, -200);
        Vector3 board3Position = new Vector3(0, 0, -300);
        Vector3 board4Position = new Vector3(0, 0, -400);
        Vector3 board5Position = new Vector3(0, 0, -500);

        Vector3 wallPosition = new Vector3(-25, 0, 0);

        public Matrix boardWorld = Matrix.Identity;
        public Matrix ballWorld = Matrix.Identity;
        public Matrix wallWorld = Matrix.Identity;

        Model[] balls = new Model[1];
        Model[] boards = new Model[5];
        Model[] walls = new Model[5];

        public Wiimote bb = new Wiimote();
        public enum BallOnBoard { FIRST, SECOND, THIRD, FOURTH, FIFTH }
        public BallOnBoard ballCurrentlyOn = BallOnBoard.FIRST;

        public int left = 0;
        public int right = 0;
        public int up = 0;
        public int down = 0;

        public Level1(Game game)
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            balls[0] = Game.Content.Load<Model>(@"Models\ball");
            walls[0] = Game.Content.Load<Model>(@"Models\wall");
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
                generalBoardUpdate();
                board1Update();
                board2Update();
                board3Update();
                board4Update();
                board5Update();
                ballUpdate();
                ballUpdate1();
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.currentGameState == Game1.GameState.PLAY)
            {
                BoardHelper.boardDraw(board1Movement, boards[0], board1Rotation, projection, view);
                BoardHelper.boardDraw(board2Movement, boards[1], board2Rotation, projection, view);
                BoardHelper.boardDraw(board3Movement, boards[2], board3Rotation, projection, view);
                BoardHelper.boardDraw(board4Movement, boards[3], board4Rotation, projection, view);
                BoardHelper.boardDraw(board5Movement, boards[4], board5Rotation, projection, view);
                BallHelper.ballDraw(balls[0], ballWorld, ballRotation, projection, view);
                WallHelper.wallDraw(walls[0], wallWorld, wallRotation, projection, view);
            }

            base.Draw(gameTime);
        }

        public void generalBoardUpdate()
        {
            board1Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board1Movement = Matrix.CreateTranslation(boardPosition);
            board2Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board2Movement = Matrix.CreateTranslation(board1Position);
            board3Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board3Movement = Matrix.CreateTranslation(board2Position);
            board4Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board4Movement = Matrix.CreateTranslation(board3Position);
            board5Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board5Movement = Matrix.CreateTranslation(board4Position);

            WiimoteState s = bb.WiimoteState;
            BalanceBoardState bbs = s.BalanceBoardState;
            yawAngle -= bbs.CenterOfGravity.X * 0.0009F;
            pitchAngle -= bbs.CenterOfGravity.Y * 0.0009F;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                yawAngle += 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                yawAngle -= 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                pitchAngle += 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
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
            if (ballCurrentlyOn == BallOnBoard.FIRST)
            {
                //Rotate model
                board1Rotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            }
        }

        public void board2Update()
        {
            if (ballCurrentlyOn == BallOnBoard.SECOND)
            {
                //Rotate model
                board2Rotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            }
        }

        public void board3Update()
        {
            if (ballCurrentlyOn == BallOnBoard.THIRD)
            {
                //Rotate model
                board3Rotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            }
        }

        public void board4Update()
        {
            if (ballCurrentlyOn == BallOnBoard.FOURTH)
            {
                //Rotate model
                board4Rotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            }
        }

        public void board5Update()
        {
            if (ballCurrentlyOn == BallOnBoard.FIFTH)
            {
                //Rotate model
                board5Rotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            }
        }

        public void ballUpdate()
        {
            WiimoteState s = bb.WiimoteState;
            BalanceBoardState bbs = s.BalanceBoardState;
            ballPosition.X -= bbs.CenterOfGravity.X * 0.05F;
            ballPosition.Z += bbs.CenterOfGravity.Y * 0.05F;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                ballPosition.X -= (0.005F * right++);
                //ballPosition.Y = Helper.adjustBallHeight(ballPosition.X, pitchAngle) * ((int) (ballCurrentlyOn) * 100);
                left = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                ballPosition.X += (0.005F * left++);
                //ballPosition.Y = Helper.adjustBallHeight(ballPosition.X, pitchAngle) * ((int)(ballCurrentlyOn) * 100);
                right = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ballPosition.Z -= (0.005F * up++);
                //ballPosition.Y = Helper.adjustBallHeight(ballPosition.Z, yawAngle) * ((int)(ballCurrentlyOn) * 100);
                down = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                ballPosition.Z += (0.005F * down++);
                //ballPosition.Y = Helper.adjustBallHeight(ballPosition.Z, yawAngle) * ((int)(ballCurrentlyOn) * 100);
                up = 0;
            }
            
            if (ballPosition.X < -64)
                ballPosition.X = -64;
            if (ballPosition.X > 64)
                ballPosition.X = 64;
            if (ballPosition.Z < -46)
                ballPosition.Z = -46;
            if (ballPosition.Z > 46)
                ballPosition.Z = 46;

            //Move model
            ballWorld = Matrix.CreateTranslation(ballPosition);
            wallWorld = Matrix.CreateTranslation(wallPosition);
        }

        public void ballUpdate1()
        {
            if (ballPosition.X > 12 && ballPosition.Y >= -8F && ballPosition.Y < 90 && ballPosition.Z < -12)
                dropTheBall1();

            if (ballPosition.X < 12 && ballPosition.Y >= 90 && ballPosition.Y < 190 && ballPosition.Z < 12)
                dropTheBall2();

            if (ballPosition.X > 12 && ballPosition.Y >= 190 && ballPosition.Y < 290 && ballPosition.Z < -12)
                dropTheBall3();

            if (ballPosition.X < 12 && ballPosition.Y >= 290 && ballPosition.Y < 390 && ballPosition.Z < 12)
                dropTheBall4();

            if (ballPosition.X > 12 && ballPosition.Y >= 390 && ballPosition.Z < -12)
                dropTheBall5();
        }

        private void dropTheBall1()
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= 90)
                ballPosition.Y = 90;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= 0)
                cameraPosition.Z = 0;
            CreateLookAt();
            ballCurrentlyOn = BallOnBoard.SECOND;
        }

        private void dropTheBall2()
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= 190)
                ballPosition.Y = 190;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -100)
                cameraPosition.Z = -100;
            CreateLookAt();
            ballCurrentlyOn = BallOnBoard.THIRD;
        }

        private void dropTheBall3()
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= 290)
                ballPosition.Y = 290;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -200)
                cameraPosition.Z = -200;
            CreateLookAt();
            ballCurrentlyOn = BallOnBoard.FOURTH;
        }

        private void dropTheBall4()
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= 390)
                ballPosition.Y = 390;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -300)
                cameraPosition.Z = -300;
            CreateLookAt();
            ballCurrentlyOn = BallOnBoard.FIFTH;
        }

        private void dropTheBall5()
        {
            Game1.currentLevel = Game1.Level.TWO;
            ((Game1)Game).ChangeGameState(Game1.GameState.LEVEL_CHANGE, 1);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, Vector3.Down);
        }
    }
}