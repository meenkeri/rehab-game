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
    class BoardTilt : BasicModel
    {
        Matrix rotation = Matrix.Identity;
        float yawAngle = 0; //Left Right
        float pitchAngle = 0; // Up Down
        float rollAngle = 0; //Do nothing

        public BoardTilt(Model m)
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
            if (yawAngle >= 0.2F )
                yawAngle = 0.2F;
            if (yawAngle <= -0.2F)
                yawAngle = -0.2F;
            if (pitchAngle >= 0.2F)
                pitchAngle = 0.2F;
            if (pitchAngle <= -0.2F)
                pitchAngle = -0.2F;

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