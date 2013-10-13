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
using log4net;
using log4net.Config;

namespace rehabGame
{
    class Log
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(Log));

        public Log()
        {
            BasicConfigurator.Configure();
        }
    }
}
