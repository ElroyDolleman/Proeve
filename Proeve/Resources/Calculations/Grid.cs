using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Proeve.Resources.Calculations
{
    class Grid
    {
        public static Point ToPixelLocation(Point location, Point baseLocation, Point tileDimensions)
        {
            Point answer = new Point();
          /*  if (location.X < baseLocation.X) location.X -= tileDimensions.X;
            if (location.Y < baseLocation.Y) location.Y -= tileDimensions.Y;/**/

            answer.X = (int)Math.Floor((double)(baseLocation.X + location.X * tileDimensions.X));
            answer.Y = (int)Math.Floor((double)(baseLocation.Y + location.Y * tileDimensions.Y));

            return answer;
        }

        public static Point ToGridLocation(Point location, Point baseLocation, Point tileDimensions)
        {
            Point answer = new Point();
            if (location.X < baseLocation.X) location.X -= tileDimensions.X;
            if (location.Y < baseLocation.Y) location.Y -= tileDimensions.Y;

            answer.X = (int)Math.Floor((double)((location.X - baseLocation.X) / tileDimensions.X));
            answer.Y = (int)Math.Floor((double)((location.Y - baseLocation.Y) / tileDimensions.Y));

            return answer;
        }
    }
}
