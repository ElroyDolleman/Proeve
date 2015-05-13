using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proeve.Resources.Calculations.Pathfinding
{
    public class AStar {
	
	    private static List<List<Node>> field;
	
	    private static List<Node> open;
	    private static List<Node> closed;
	    private static List<Node> path;
	
	    private static Node start;
	    public static Node end;
	
	    private static int range;
	
	
	    public static List<Node> GetOpen(){
		    return open;
	    }
	
	    public static List<Node> GetClosed(){
		    return closed;
	    }
	
	    public static List<Node> GetPath(){
		    return path;
	    }
	
	
	    public static void SetField(int[][] field){
		    AStar.field = new List<List<Node>>();
		    for (int i = 0; i < field.Length; i++){
			    AStar.field.Add(new List<Node>());
			    for (int j = 0; j < field[i].Length; j++){
				    AStar.field[i].Add(new Node(i, j, field[i][j]));
			    }
		    }
	    }

        public static void SetField(List<List<Node>> field)
        {
            AStar.field = field;
        }

	    public static void Path(int x, int y, int x2, int y2){
		    start = field[x][y];
		    end = field[x2][y2];
            range = -1;
		    Calculate();
	    }
	
	    public static void Area(int x, int y, int r){
		    start = field[x][y];
		    range = r;
		    Calculate();
	    }
	
	    private static void Calculate(){
		    open = new List<Node>();
		    closed = new List<Node>();
		    path = null;
		    AddToOpen(start);
		
		    Node current;

            while (open.Count() > 0)
            {
			    current = null;
			    for (int i = 0; i < open.Count(); i++){
				    if ((current == null || current.g >= open[i].g)){
					    current = open[i];
				    }
			    }
			    SwitchToClosed(current);
			
			    if (closed[closed.Count()-1] != end){
                    List<Node> surround = GetSurrounding(current);

                    for (int i = 0; i < surround.Count; i++)
                    {
                        if (surround[i].status == null)
                        {
						    surround[i].parent = current;
						    surround[i].setValues();
                            //Console.WriteLine(range + ":" + surround[i].g);
						    if (range > -1 ? surround[i].g <= range*Node.size : true){
							    AddToOpen(surround[i]);
						    }
					    }
					    else{
						    if (TestG(surround[i], current)){
							    surround[i].parent = current;
							    surround[i].setValues();
						    }
					    }
				    }
				    surround = null;
			    }
		    }
            current = null;
		
		    if (end != null && end.status == "Closed"){
		    //if (closed.size() == AStar.field.size()){
			    SetPath();
		    }
	    }
	
	

	    private static void SetPath(){
		    path = new List<Node>();
		    Node n = end;
		    path.Add(n);
		    while(n.parent != null){
			    n = n.parent;
			    if (n != null){
				    path.Add(n);
			    }
		    }
	    }
	
	    private static bool TestG(Node n, Node p){
            int Gx = (p.x - n.x) * Node.size;
            int Gy = (p.y - n.y) * Node.size;
		
		    if (Gx < 0) { Gx = -Gx; }
		    if (Gy < 0) { Gy = -Gy; }
		
		    int g = (int) (p.g + Math.Sqrt(Gx * Gx + Gy * Gy));
		    Gx = 0;
		    Gy = 0;
		    return (n.g > g);
	    }
	
	    private static List<Node> GetSurrounding(Node c){
		    List<Node> temp = new List<Node>();
	
		    bool up = !(c.y-1 >= 0) || c.up || AStar.field[c.x][c.y-1].down;
		    bool right = !(c.x + 1 < AStar.field.Count()) || c.right || AStar.field[c.x+1][c.y].left;
		    bool down = !(c.y+1 < AStar.field[0].Count()) || c.down || AStar.field[c.x][c.y+1].up;
		    bool left = !(c.x-1 >= 0) || c.left || AStar.field[c.x-1][c.y].right;
	
		    if (!up){
			    temp.Add(AStar.field[c.x][c.y-1]);
		    }
		    if (!down){
			    temp.Add(AStar.field[c.x][c.y+1]);
		    }
		    if (!left){
			    temp.Add(AStar.field[c.x-1][c.y]);
		    }
		    if (!right){
			    temp.Add(AStar.field[c.x+1][c.y]);
		    }
		    return temp;
	    }
	
	    private static void AddToOpen(Node n){
		    open.Add(n);
		    n.status = "Open";
	    }
	
	    private static void SwitchToClosed(Node n){
		    open.Remove(n);
		    closed.Add(n);
		    n.status = "Closed";
	    }
    }

}
