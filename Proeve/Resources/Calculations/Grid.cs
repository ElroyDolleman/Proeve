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

            answer.X = (int)Math.Floor((double)(baseLocation.X + location.X * tileDimensions.X));
            answer.Y = (int)Math.Floor((double)(baseLocation.Y + location.Y * tileDimensions.Y));

            return answer;
        }

        public static Point ToGridLocation(Point location, Point baseLocation, Point tileDimensions)
        {
            Point answer = new Point();

            answer.X = (int)Math.Ceiling((double)((location.X - baseLocation.X) / tileDimensions.X));
            answer.Y = (int)Math.Floor((double)((location.Y - baseLocation.Y) / tileDimensions.Y));

            return answer;
        }

        public static int[,] RotateGrid(int[,] grid, int rotateCount = 1)
        {
            int[,] answer = new int[grid.GetLength(0),grid.GetLength(1)];

            for (int i = 0; i < answer.GetLength(0); i++)
            for (int j = 0; j < answer.GetLength(1); j++)
            {
                answer[i, j] = grid[(grid.GetLength(0) - 1) - j, i];
            }

            if (rotateCount % 4 == 1)
            {
                return answer;
            }
            else
            {
                return RotateGrid(answer, (rotateCount - 1) % 4);
            }
        }
    }
}
