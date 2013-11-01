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
using WiimoteLib;

namespace rehabGame
{
    class BalanceBoard
    {
        public static Wiimote bb = new Wiimote();

        public BalanceBoard()
        {
            try
            {
                bb.Connect();
                bb.SetLEDs(1);
            }
            catch { Console.WriteLine(IConstants.WBB_ERROR); }
        }

        public static Wiimote getBalanceBoard()
        {
            return bb;
        }
    }
}
