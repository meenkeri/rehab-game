using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                yawAngle += 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                yawAngle -= 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                pitchAngle += 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                pitchAngle -= 0.01F;
            if (yawAngle >= 0.4F )
                yawAngle = 0.4F;
            if (yawAngle <= -0.4F)
                yawAngle = -0.4F;
            if (pitchAngle >= 0.4F)
                pitchAngle = 0.4F;
            if (pitchAngle <= -0.4F)
                pitchAngle = -0.4F;

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