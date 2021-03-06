﻿using System;
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
                    //be.TextureEnabled = true;
                    be.Projection = projection;
                    be.View = view;
                    be.World = Helper.GetBoardWorld(bRotation, bMovement) * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
    }
}