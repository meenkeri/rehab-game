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
    class Helper
    {
        public static Matrix GetBoardWorld(Matrix rotation, Matrix movement)
        {
            return rotation * movement;
        }

        public static Matrix GetBallWorld(Matrix ballWorld, Matrix ballRotation)
        {
            return ballWorld * ballRotation;
        }

        public static float adjustBallHeight(float adjacent, double angle)
        {
            return (float)((Math.Tan(angle) * adjacent) - 4);
        }
    }
}