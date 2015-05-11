using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;
using Proeve.Resources.Calculations;
using Proeve.Resources.Calculations.Pathfinding;

namespace Proeve.States
{
    class GameUIState : State
    {
        private bool isTurn = true;
        private int selected = -1;
        private List<bool> canMove;
        private List<Vector2> canMoveTo;

        public GameUIState()
        {
            
        }

        public override void Initialize()
        {
            canMove = new List<bool>();
            for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
            {
                canMove.Add(true);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isTurn)
            {
                if (selected == -1)
                {
                    bool contains = false;
                    for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
                    {
                        if (((GameState)StateManager.GetState(1)).GetArmy()[i].Hitbox.Contains(Globals.mouseState.Position) && Globals.mouseState.LeftButtonPressed)
                        {
                            selected = i;
                            contains = true;
                        }
                    }

                    if (!contains)
                    {
                        selected = -1;
                    }
                    else if (selected != -1)
                    {
                        List<List<Node>> field = new List<List<Node>>();
                        for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetLevel().GetLength(0); i++)
                        {
                            field.Add(new List<Node>());
                            for (int j = 0; j < ((GameState)StateManager.GetState(1)).GetLevel().GetLength(1); j++)
                            {
                               field[i].Add(new Node(i, j, ((GameState)StateManager.GetState(1)).GetLevel()[i, j]));
                            }
                        }
                        AStar.SetField(field);
                        Point gridPos = Grid.ToGridLocation(new Point((int)((GameState)StateManager.GetState(1)).GetArmy()[selected].Position.X, (int)((GameState)StateManager.GetState(1)).GetArmy()[selected].Position.Y), Globals.GridLocation, Globals.TileDimensions);
                        AStar.Area(gridPos.X, gridPos.Y, ((GameState)StateManager.GetState(1)).GetArmy()[selected].move);
                        List<Node> nodes = AStar.GetClosed();
                        canMoveTo = new List<Vector2>();
                        Console.WriteLine(nodes.Count);
                        for (int i = 1; i < nodes.Count; i++)
                        {
                            canMoveTo.Add(new Vector2(nodes[i].x, nodes[i].y));
                        }
                    }
                }
                else
                {

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);
            if (selected >= 0)
            {
                if (canMove[selected] == true)
                {
                    for (int i = 0; i < canMoveTo.Count; i++)
                    {
                        spriteBatch.DrawRectangle(new Rectangle(Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).X, Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).Y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT), Color.White);
                    }
                }
            }
        }

        private List<Character> GetArmy()
        {
            return ((GameState)StateManager.GetState(1)).GetArmy();
        }

        public List<Character> GetEnemyArmy()
        {
            return ((GameState)StateManager.GetState(1)).GetEnemyArmy(); ;
        }
    }
}
