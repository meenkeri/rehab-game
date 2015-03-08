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
    public class IConstants
    {
        //WBB
        public static string WBB_ERROR = "Can't find a BalanceBoard";

        //Splash screen
        public static string TITLE = "Welcome to Rehab Game";
        public static string SUB_TITLE = "Press Enter To Begin" + "\n" + "\n" + "\n" +
                                         "Game Developed By: " + "\n" +
                                         "Shankar Meenkeri" + "\n" +
                                         "San Diego State University";
        public static string QUIT = "Press Enter To Quit";
        public static string GAME_OVER = "Game Over";

        //Score
        public static string SCORE = "Score: ";

        //Time Left
        public static string TIME = "Time left: ";
    }
}