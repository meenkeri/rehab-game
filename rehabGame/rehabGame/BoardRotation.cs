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
        float yawAngle = 0; //Left Right
        float pitchAngle = 0; // Up Down
        float rollAngle = 0; //Do nothing

        public BoardRotation(Model m)
            : base(m)
        {
            
        }

        public override void Update()
        {
            rotation = Matrix.CreateRotationZ(MathHelper.Pi / 2);
            
            //Rotate model
            rotation *= Matrix.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        }

        public override Matrix GetWorld()
        {
            return world * rotation;
        }
    }
}