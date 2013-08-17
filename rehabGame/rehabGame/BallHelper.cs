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
    class BallHelper
    {
        public static void ballDraw(Model ball, Matrix bWorld, Matrix bRotation, Matrix projection, Matrix view)
        {
            Matrix[] transforms = new Matrix[ball.Bones.Count];
            ball.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ball.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = projection;
                    be.View = view;
                    be.World = Helper.GetBallWorld(bWorld, bRotation) * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
    }
}
