﻿using System;
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
        public static GraphicsDevice graphicsDevices;
        public static MultiplayerConnection multiplayerConnection;

        public static int coins;

        public static Point GridLocation { get { return new Point(228, 40); } }
        public static Point TileDimensions { get { return new Point(TILE_WIDTH, TILE_HEIGHT); } }

        public const int TILE_WIDTH = 82;
        public const int TILE_HEIGHT = 82;

        public const int GRID_WIDTH = 8;
        public const int GRID_HEIGHT = 8;
    }
}
