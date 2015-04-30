using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Input;

using Proeve.Entities;

namespace Proeve.Resources
{
    class Globals
    {
        public static E2DMouseState mouseState;
        public static ContentManager contentManager;

        public static int coins;

        public static Point GridLocation { get { return new Point(228, 40); } }
        public const int GRID_WIDTH = 82;
        public const int GRID_HEIGHT = 82;
    }
}
