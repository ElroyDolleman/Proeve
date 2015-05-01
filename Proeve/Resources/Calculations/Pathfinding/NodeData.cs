using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proeve.Resources.Calculations.Pathfinding
{
    public class NodeData
    {
        public static int size = 16;

        public static bool[][] data = new bool[][]{
	        //up, right, down, left
	        {false, false, false, false},//1
		
	        {true, false, false, false},//2
	        {false, true, false, false},//3
	        {false, false, true, false},//4
	        {false, false, false, true},//5
		
	        {true, true, false, false},//6
	        {true, false, true, false},//7
	        {true, false, false, true},//8
		
	        {false, true, true, false},//9
	        {false, true, false, true},//10
		
	        {false, false, true, true},//11
		
	        {false, true, true, true},//12
	        {true, false, true, true},//13
	        {true, true, false, true},//14
	        {true, true, true, false},//15
		
	        {true, true, true, true}//16
	        };
        }
    }
}
