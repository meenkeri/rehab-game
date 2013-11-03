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
        //Models declaration
        Model ball;
        Model[] boards = new Model[4];

        //Camera declaration
        Vector3 pos = new Vector3(0, 150, 100);
        Vector3 target = Vector3.Zero;
        Vector3 cameraPosition;
        Vector3 cameraDirection;
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        //Board Matrix
        public Matrix board1Movement = Matrix.Identity;
        public Matrix board2Movement = Matrix.Identity;
        public Matrix board3Movement = Matrix.Identity;
        public Matrix board4Movement = Matrix.Identity;

        public Matrix board1Rotation = Matrix.Identity;
        public Matrix board2Rotation = Matrix.Identity;
        public Matrix board3Rotation = Matrix.Identity;
        public Matrix board4Rotation = Matrix.Identity;

        //Board Positions
        Vector3 board1Position = new Vector3(0, 0, 0);
        Vector3 board2Position = new Vector3(0, 0, -100);
        Vector3 board3Position = new Vector3(0, 0, -200);
        Vector3 board4Position = new Vector3(0, 0, -300);

        //Board Angle
        float yawAngle = 0; //Left Right
        float pitchAngle = 0; // Up Down
        float rollAngle = 0; //Do nothing

        //Board Enum
        public enum BallOnBoard
        {
            FIRST = 0,
            SECOND = 112,
            THIRD = 224,
            FOURTH = 336
        }
        public BallOnBoard ballCurrentlyOn = BallOnBoard.FIRST;

        //Ball Matrix
        public Matrix ballRotation = Matrix.Identity;
        public Matrix ballWorld = Matrix.Identity;

        //Ball Position
        Vector3 ballPosition = new Vector3(0, -8F, 0);
        Vector3 previousBallPosition;

        //Ball On The Board Height Adjustment Variables
        public float LRHeight;
        public float UDHeight;

        //No Ball/Board Movement When The Ball Is Falling
        public Boolean drop = false;

        //Ball Rolling Effect Variables
        public int left = 0;
        public int right = 0;
        public int up = 0;
        public int down = 0;

        public Level1(Game game)
            : base(game)
        {
            Log.logger.Info("Loading Level1 constructor");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Log.logger.Info("Initializing level1");
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
            Log.logger.Info("Loading level1 content");
            ball = Game.Content.Load<Model>(@"Models\ball");
            boards[0] = Game.Content.Load<Model>(@"Models\Level11");
            boards[1] = Game.Content.Load<Model>(@"Models\Level12");
            boards[2] = Game.Content.Load<Model>(@"Models\Level13");
            boards[3] = Game.Content.Load<Model>(@"Models\Level14");

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
                generalBallUpdate();
                isHole();
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
                BallHelper.ballDraw(ball, ballWorld, ballRotation, projection, view);
            }

            base.Draw(gameTime);
        }

        public void generalBoardUpdate()
        {
            board1Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board1Movement = Matrix.CreateTranslation(board1Position);
            board2Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board2Movement = Matrix.CreateTranslation(board2Position);
            board3Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board3Movement = Matrix.CreateTranslation(board3Position);
            board4Rotation = Matrix.CreateRotationX(MathHelper.Pi / 2);
            board4Movement = Matrix.CreateTranslation(board4Position);

            WiimoteState s = BalanceBoard.getBalanceBoard().WiimoteState;
            BalanceBoardState bbs = s.BalanceBoardState;
            pitchAngle -= bbs.CenterOfGravity.X * 0.0009F;
            yawAngle += bbs.CenterOfGravity.Y * 0.0009F;

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

        public void generalBallUpdate()
        {
            previousBallPosition = ballPosition;

            LRHeight = Helper.adjustBallHeight(ballPosition.X, pitchAngle);
            UDHeight = Helper.adjustBallHeight(ballPosition.Z, yawAngle);

            if (!drop)
            {
                WiimoteState s = BalanceBoard.getBalanceBoard().WiimoteState;
                BalanceBoardState bbs = s.BalanceBoardState;
                ballPosition.X -= bbs.CenterOfGravity.X * 0.05F;
                ballPosition.Z += bbs.CenterOfGravity.Y * 0.05F;
                ballPosition.Y = LRHeight + UDHeight + (float)ballCurrentlyOn;

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    ballPosition.X -= (0.005F * right++);
                    left = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    ballPosition.X += (0.005F * left++);
                    right = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    ballPosition.Z -= (0.005F * up++);
                    down = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    ballPosition.Z += (0.005F * down++);
                    up = 0;
                }
            }

            if (ballPosition.X < -75)
                ballPosition.X = -75;
            if (ballPosition.X > 75)
                ballPosition.X = 75;
            if (ballPosition.Z < -54)
                ballPosition.Z = -54;
            if (ballPosition.Z > 56)
                ballPosition.Z = 56;

            //Move model
            ballWorld = Matrix.CreateTranslation(ballPosition);
        }

        public void isHole()
        {
            switch (ballCurrentlyOn)
            {
                case BallOnBoard.FIRST:
                    if (ballPosition.X > 33 && ballPosition.X < 41 && ballPosition.Z < -28 && ballPosition.Z > -36)
                    {
                        drop = true;
                        dropTheBall(104, 12, BallOnBoard.SECOND);
                    }
                    break;

                case BallOnBoard.SECOND:
                    if (ballPosition.X < -47 && ballPosition.X > -55 && ballPosition.Z < -26 && ballPosition.Z > -33)
                    {
                        drop = true;
                        dropTheBall(216, 124, BallOnBoard.THIRD);
                    }
                    break;

                case BallOnBoard.THIRD:
                    if (ballPosition.X < -39 && ballPosition.X > -49 && ballPosition.Z > 17 && ballPosition.Z < 26)
                    {
                        drop = true;
                        dropTheBall(328, 236, BallOnBoard.FOURTH);
                    }
                    break;

                case BallOnBoard.FOURTH:
                    if (ballPosition.X > 32 && ballPosition.X < 41 && ballPosition.Z > 21 && ballPosition.Z < 30)
                    {
                        Game1.currentLevel = Game1.Level.TWO;
                        ((Game1)Game).ChangeGameState(Game1.GameState.LEVEL_CHANGE, 1);
                    }
                    break;

                default:
                    Log.logger.Info("Does not match the value " + ballCurrentlyOn + " any cases inside switch");
                    break;
            }
        }

        private void dropTheBall(int ballHeight, int cameraHeight, BallOnBoard next)
        {
            ballPosition += Vector3.Up * 0.6F;
            if (ballPosition.Y >= ballHeight)
            {
                ballPosition.Y = ballHeight;
                ballCurrentlyOn = next;
                yawAngle = pitchAngle = 0;
                drop = false;
            }
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -cameraHeight)
                cameraPosition.Z = -cameraHeight;
            CreateLookAt();
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, Vector3.Down);
        }
    }
}
