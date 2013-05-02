using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rehabGame
{
    class BoardRotation : BasicModel
    {
        Matrix rotation = Matrix.Identity;

        public BoardRotation(Model m)
            : base(m)
        {
        }

        public override void Update()
        {
            rotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);
        }

        public override Matrix GetWorld()
        {
            return world * rotation;
        }
    }
}