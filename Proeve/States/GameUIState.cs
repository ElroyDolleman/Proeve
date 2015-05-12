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
        private bool IsTurn { get { return Globals.multiplayerConnection.myTurn; } }
        private int selected = -1;
        private List<bool> canMove;
        private List<Point> canMoveTo;

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

            Globals.multiplayerConnection.RecieveMove += RecievedMove;
            Globals.multiplayerConnection.RecieveFight += RecievedFight;
            Globals.multiplayerConnection.RecieveEndTurn += OtherPlayerEndedHisTurn;
        }

        public override void Update(GameTime gameTime)
        {
            


            if (IsTurn)
            {
                if (Globals.mouseState.RightButtonPressed)
                {
                    
                    Armies.army[0].GridPosition = new Point(-1, 2);

                    Console.WriteLine("old position = {0}", Armies.army[0].Position.ToPoint());
                    Console.WriteLine("new gridPos = {0}", Armies.army[0].GridPosition);
                    Console.WriteLine("new position = {0}", Armies.army[0].Position);
                }

                if (selected == -1)
                {
                    if (Globals.mouseState.LeftButtonPressed)
                    {
                        bool contains = false;
                        for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
                        {
                            if (((GameState)StateManager.GetState(1)).GetArmy()[i].Hitbox.Contains(Globals.mouseState.Position))
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
                            int[,] level = (int[,])((GameState)StateManager.GetState(1)).GetLevel().Clone();

                            for (int i = 0; i < Armies.army.Count(); i++)
                            {
                                Point gridpos = Armies.army[i].GridPosition;

                                Console.WriteLine("gridpos: {0}", gridpos);

                                level[gridpos.X, gridpos.Y] = 1;
                            }
                            for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetEnemyArmy().Count(); i++)
                            {
                                Point gridpos = Grid.ToGridLocation(new Point((int)((GameState)StateManager.GetState(1)).GetEnemyArmy()[i].Position.X, (int)((GameState)StateManager.GetState(1)).GetEnemyArmy()[i].Position.Y), Globals.GridLocation, Globals.TileDimensions);
                                level[gridpos.X, gridpos.Y] = 1;
                            }

                            Point GPos = Grid.ToGridLocation(new Point((int)((GameState)StateManager.GetState(1)).GetEnemyArmy()[selected].Position.X, (int)((GameState)StateManager.GetState(1)).GetEnemyArmy()[selected].Position.Y), Globals.GridLocation, Globals.TileDimensions);
                            level[GPos.X, GPos.Y] = 0;

                            level = Grid.RotateGrid(level, 2);

                            List<List<Node>> field = new List<List<Node>>();
                            for (int i = 0; i < level.GetLength(0); i++)
                            {
                                field.Add(new List<Node>());
                                for (int j = 0; j < level.GetLength(1); j++)
                                {
                                   field[i].Add(new Node(i, j, level[i, j]));
                                }
                            }
                            AStar.SetField(field);
                            Point gridPos = Grid.ToGridLocation(new Point((int)((GameState)StateManager.GetState(1)).GetArmy()[selected].Position.X, (int)((GameState)StateManager.GetState(1)).GetArmy()[selected].Position.Y), Globals.GridLocation, Globals.TileDimensions);
                            AStar.Area(gridPos.X, gridPos.Y, ((GameState)StateManager.GetState(1)).GetArmy()[selected].move);
                            List<Node> nodes = AStar.GetClosed();
                            canMoveTo = new List<Point>();
                            //Console.WriteLine(nodes.Count);
                            for (int i = 1; i < nodes.Count; i++)
                            {
                                canMoveTo.Add(new Point(nodes[i].x, nodes[i].y));
                            }
                        }
                    }
                }
                else
                {

                    if (Globals.mouseState.LeftButtonPressed)
                    {
                        for (int i = 0; i < canMoveTo.Count; i++)
                        {
                            Rectangle hitbox = new Rectangle(Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).X, Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).Y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
                            if (hitbox.Contains(Globals.mouseState.Position))
                            {
                                Globals.multiplayerConnection.SendMove(selected, canMoveTo[i]);
                                ((GameState)StateManager.GetState(1)).MoveUnit(((GameState)StateManager.GetState(1)).GetArmy()[selected], canMoveTo[i]);
                                selected = -1;
                                break;
                            }
                        }
                    }
                }
            }

            ((GameState)StateManager.GetState(1)).Update(gameTime);
            Globals.multiplayerConnection.Update(gameTime);
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

            spriteBatch.DrawDebugText("IsTurn: " + IsTurn, new Point(4, 4), Color.White);
        }

        private List<Character> GetArmy()
        {
            return ((GameState)StateManager.GetState(1)).GetArmy();
        }

        public List<Character> GetEnemyArmy()
        {
            return ((GameState)StateManager.GetState(1)).GetEnemyArmy(); ;
        }

        private void RecievedMove(int charIndex, Point gridLocation)
        {
            //enemyArmy[charIndex].Position = Grid.ToPixelLocation(gridLocation, Globals.GridLocation, Globals.TileDimensions).ToVector2();
        }

        public void RecievedFight(int charIndexAttacker, int charIndexDefender)
        {
            // Attacker is the opponent. Defender is you.

            //army[charIndexDefender].Position = new Vector2(0, 0);
            //enemyArmy[charIndexAttacker].Position = new Vector2(82, 0);
        }

        private void OtherPlayerEndedHisTurn()
        {

        }
    }
}
