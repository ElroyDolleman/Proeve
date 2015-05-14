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
            canMove[canMove.Count - 1] = false;
            canMove[canMove.Count - 2] = false;

            Globals.multiplayerConnection.RecieveMove += RecievedMove;
            Globals.multiplayerConnection.RecieveFight += RecievedFight;
            Globals.multiplayerConnection.RecieveEndTurn += OtherPlayerEndedHisTurn;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsTurn)
            {
                if (selected == -1)
                {
                    if (Globals.mouseState.LeftButtonPressed)
                    {
                        bool contains = false;
                        for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
                        {
                            if (((GameState)StateManager.GetState(1)).GetArmy()[i].Hitbox.Contains(Globals.mouseState.Position) && canMove[i])
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
                            int[,] level = new int[((GameState)StateManager.GetState(1)).GetLevel().GetLength(0), ((GameState)StateManager.GetState(1)).GetLevel().GetLength(1)];

                            for (int i = 0; i < level.GetLength(0); i++)
                            for (int j = 0; j < level.GetLength(1); j++)
                            {
                                level[i, j] = ((GameState)StateManager.GetState(1)).GetLevel()[i, j];
                            }

                            for (int i = 0; i < Armies.army.Count(); i++)
                            {
                                Point gridpos = ((GameState)StateManager.GetState(1)).GetArmy()[i].GridPosition;

                                level[gridpos.X, gridpos.Y] = 1;
                            }

                            for (int i = 0; i < Armies.opponentArmy.Count(); i++)
                            {
                                Point gridpos = Armies.opponentArmy[i].GridPosition; // Grid.ToGridLocation(new Point((int)((GameState)StateManager.GetState(1)).GetEnemyArmy()[i].Position.X + Globals.TILE_WIDTH / 2, (int)((GameState)StateManager.GetState(1)).GetEnemyArmy()[i].Position.Y + Globals.TILE_WIDTH / 2), Globals.GridLocation, Globals.TileDimensions);
                                level[gridpos.X, gridpos.Y] = 1;
                            }

                            Point GPos = Armies.army[selected].GridPosition;
                            level[GPos.X, GPos.Y] = 0;

                            // Debug
                            for (int i = 0; i < level.GetLength(0); i++)
                            {
                                Console.WriteLine();
                                for (int j = 0; j < level.GetLength(1); j++)
                                    Console.Write(level[i, j]);
                            }
                            // Debug

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
                            AStar.Area(GPos.X, GPos.Y, Armies.army[selected].move);
                            List<Node> nodes = AStar.GetClosed();
                            canMoveTo = new List<Point>();

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
                                canMove[selected] = false;
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

            //foreach(Character c in Armies.army)
            //{
            //    spriteBatch.DrawRectangle(Globals.GridLocation.ToVector2() + c.GridPosition.ToVector2() * 82, 82, 82, Color.Blue * .5f);
            //}
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

            
        }

        private void OtherPlayerEndedHisTurn()
        {

        }
    }
}
