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
        public Wiimote bb = new Wiimote();

        public void BalanceBoard()
        {
            try
            {
                bb.Connect();
                bb.SetLEDs(1);
            }
            catch { Console.WriteLine(IConstants.WBB_ERROR); }
        }
    }
}
