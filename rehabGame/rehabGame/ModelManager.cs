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
    public class ModelManager : DrawableGameComponent
    {
        public Matrix boardRotation = Matrix.Identity;
        public Matrix board1Movement = Matrix.Identity;
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

        Vector3 position = new Vector3(0, -4.1F, 0);
        Vector3 board1Position = new Vector3(0, 0, -200);
        Vector3 board2Position = new Vector3(0, 0, -400);
        Vector3 board3Position = new Vector3(0, 0, -600);
        
        public Matrix boardWorld = Matrix.Identity;
        public Matrix ballWorld = Matrix.Identity;
        Model[] balls = new Model[1];
        Model[] boards = new Model[5];

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            balls[0] = Game.Content.Load<Model>(@"Models\ball");
            boards[0] = Game.Content.Load<Model>(@"Models\board");
            boards[1] = Game.Content.Load<Model>(@"Models\board");
            boards[2] = Game.Content.Load<Model>(@"Models\board");
            boards[3] = Game.Content.Load<Model>(@"Models\board");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            boardUpdate();
            board1Update();
            ballUpdate();
           
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            boardDraw(0, boardMovement);
            boardDraw(1, board1Movement);
            
            ballDraw();

            base.Draw(gameTime);
        }

        public void draw(Camera camera, Model model)
        {
            
        }

        public void boardUpdate()
        {
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

            boardRotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);

            //Rotate model
            boardRotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        }

        public void boardDraw(int boardNum, Matrix boardMovement)
        {
            Matrix[] transforms = new Matrix[boards[boardNum].Bones.Count];
            boards[boardNum].CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in boards[boardNum].Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.Projection = projection;
                        be.View = view;
                        be.World = Helper.GetBoardWorld(boardRotation, boardMovement) * mesh.ParentBone.Transform;
                    }
                    mesh.Draw();
                }
        }

        public void board1Update()
        {
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

            boardRotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);

            //Move model
            board1Movement = Matrix.CreateTranslation(board1Position);

            //Rotate model
            boardRotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        }

        public void ballUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                position += Vector3.Left * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                position += Vector3.Right * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                position += Vector3.Forward * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                position += Vector3.Backward * 0.2F;
            
            if (position.X < -24)
                position.X = -24;
            if (position.X > 24)
                position.X = 24;
            if (position.Z < -18)
                position.Z = -18;
            if (position.Z > 17)
                position.Z = 17;

            if (position.X > 12 && position.Z < -12)
                dropTheBall();

            //Move model
            ballWorld = Matrix.CreateTranslation(position);
        }

        public void ballDraw()
        {
              Matrix[] transforms = new Matrix[balls[0].Bones.Count];
                balls[0].CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in balls[0].Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        be.EnableDefaultLighting();
                        be.Projection = projection;
                        be.View = view;
                        be.World = Helper.GetBallWorld(ballWorld, ballRotation) * mesh.ParentBone.Transform;
                    }
                    mesh.Draw();
                }
        }
        
        public void dropTheBall()
        {
            position += Vector3.Up * 0.6F;
            if (position.Y >= 76)
                position.Y = 76;
            cameraPosition.Z -= 1.5F;
            if (cameraPosition.Z <= -90)
                cameraPosition.Z = -90;
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