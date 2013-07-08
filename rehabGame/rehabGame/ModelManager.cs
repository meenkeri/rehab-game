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
        public Matrix ballRotation = Matrix.Identity;
        float yawAngle = 0; //Left Right
        float pitchAngle = 0; // Up Down
        float rollAngle = 0; //Do nothing

        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        Vector3 position = new Vector3(0, -4.6F, 0);

        protected Matrix boardWorld = Matrix.Identity;
        public Matrix ballWorld = Matrix.Identity;
        Model[] models = new Model[3];
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
            view = Matrix.CreateLookAt(new Vector3(0, 150, 120), Vector3.Zero, Vector3.Down);

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
            models[1] = Game.Content.Load<Model>(@"Models\board");
            models[2] = Game.Content.Load<Model>(@"Models\ball");
          
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            boardUpdate();
            ballUpdate();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            boardDraw();
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

        public void boardDraw()
        {
            Model model = models[1];
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = projection;
                    be.View = view;
                    be.World = GetWorldBoard() * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
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

            //Move model
            ballWorld = Matrix.CreateTranslation(position);
        }

        public void ballDraw()
        {
            Model model = models[2];
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = projection;
                    be.View = view;
                    be.World = GetWorldBall() * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
        
        public Matrix GetWorldBoard()
        {
            return boardRotation;
        }

        public Matrix GetWorldBall()
        {
            return ballWorld * ballRotation;
        }
    }
}
