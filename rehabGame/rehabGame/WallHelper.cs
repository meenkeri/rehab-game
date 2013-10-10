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
    class WallHelper
    {
        public static void wallDraw(Model wall, Matrix wWorld, Matrix wRotation, Matrix projection, Matrix view)
        {
            Matrix[] transforms = new Matrix[wall.Bones.Count];
            wall.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wall.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = projection;
                    be.View = view;
                    be.World = Helper.GetBallWorld(wWorld, wRotation) * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
    }
}
