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
    class BoardHelper
    {
        public static void boardDraw(Matrix bMovement, Model board, Matrix bRotation, Matrix projection, Matrix view)
        {
            Matrix[] transforms = new Matrix[board.Bones.Count];
            board.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in board.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = projection;
                    be.View = view;
                    be.World = Helper.GetBoardWorld(bRotation, bMovement) * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }

        public static void boardUpdate(float yawAngle, float pitchAngle, float rollAngle, Matrix boardRotation)
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
    }
}
