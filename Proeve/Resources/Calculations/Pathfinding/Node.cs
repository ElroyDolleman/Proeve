using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proeve.Resources.Calculations.Pathfinding
{
    public class Node
    {
        public static int size;

        public int x;
        public int y;

        public float f;
        public float g;
        public float h;

        public bool up;
        public bool down;
        public bool right;
        public bool left;

        public String status = null;
        public Node parent = null;

        public Node(int x, int y)
        {
            up = true;
            right = true;
            down = true;
            left = true;
            this.x = x;
            this.y = y;
        }

        public void setValues()
        {
            int Hx = (AStar.end == null ? 0 : this.x - AStar.end.x) * size;
            int Hy = (AStar.end == null ? 0 : this.y - AStar.end.y) * size;

            if (Hx < 0) { Hx = -Hx; }
            if (Hy < 0) { Hy = -Hy; }

            this.h = Hx + Hy;

            int Gx = ((parent == null ? 0 : this.parent.x) - this.x) * size;
            int Gy = ((parent == null ? 0 : this.parent.y) - this.y) * size;

            if (Gx < 0) { Gx = -Gx; }
            if (Gy < 0) { Gy = -Gy; }

            this.g = (int)((parent == null ? 0 : this.parent.g) + Math.Sqrt(Gx * Gx + Gy * Gy));

            this.f = this.g + this.h;
        }


    }
}

