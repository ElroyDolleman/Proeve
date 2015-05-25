using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.UI;
using Proeve.Entities;
using Proeve.Resources;
using Proeve.Resources.Calculations;
using Proeve.Resources.Calculations.Pathfinding;

namespace Proeve.States
{
    class GameUIState : State
    {
        private Vector2 EndTurnButtonPosition { get { return new Vector2(794, 695); } }

        private bool IsTurn { get { return Globals.multiplayerConnection.myTurn; } }
        private int selected = -1;
        private List<bool> canMove;
        private List<Point> canMoveTo;
        private List<bool> canAttack;
        private List<int> canAttackThis;
        private List<SpineAnimation> moveArrows;
        private List<SpineAnimation> attackIcons;
        private StatsUI statsUI;

        public GameUIState()
        {
            
        }

        public override void Initialize()
        {
            statsUI = Globals.statsUI;

            buttons.Add(new Button(ArtAssets.EndTurnButton, EndTurnButtonPosition));
            buttons[0].ClickEvent = EndTurn;

            SetMovable();

            Globals.multiplayerConnection.RecieveMove += RecievedMove;
            Globals.multiplayerConnection.RecieveFight += RecievedFight;
            Globals.multiplayerConnection.RecieveEndTurn += OtherPlayerEndedHisTurn;
        }

        public override void Update(GameTime gameTime)
        {
            // UPDATE ANIMTION
            if (moveArrows != null)
            {
                for (int i = 0; i < moveArrows.Count; i++)
                {
                    moveArrows[i].Update(gameTime);
                }
            }
            if (attackIcons != null)
            {
                for (int i = 0; i < attackIcons.Count; i++)
                {
                    attackIcons[i].Update(gameTime);
                }
            }
            if (canAttackThis != null)
            {
                for (int i = 0; i < canAttackThis.Count; i++)
                {
                    Character c = Armies.opponentArmy[canAttackThis[i]];

                    
                    c.UpdateSpriteSheetAnimation(gameTime);

                    if (c.sprite.CurrentFrame == 1)
                        c.sprite.CurrentFrame = 2;

                }
            }
            // END UPDATE ANIMATION

            // UNFINISHED WIN/LOSE DEFINITION/EFFECT
            if (Armies.army[0].IsDead || Armies.opponentArmy[0].IsDead)
            {
                StateManager.ChangeState(Settings.STATES.Result);
            }
            // END UNFINISHED WIN/LOSE DEFINITION/EFFECT

            if (IsTurn)
            {
                // CHECK IF UNIT CAN ATTACK AFTER MOVING
                for (int i = 0; i < Armies.army.Count; i++)
                {
                    if (!canMove[i] && !canAttack[i] && Armies.army[i].waypoints.Count == 0)
                        Armies.army[i].ColorEffect = Color.Black * .5f;
                    else if (!canMove[i] && canAttack[i])
                    {
                        canAttack[i] = false;
                        if (Armies.army[i].special != Character.Special.Healer)
                        {
                            for (int j = 0; j < Armies.opponentArmy.Count; j++)
                            {
                                if(Armies.army[i].IsNextTo(Armies.opponentArmy[j]))
                                {
                                    canAttack[i] = true;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Armies.army.Count; j++)
                            {
                                if (Armies.army[i].IsNextTo(Armies.army[j]))
                                {
                                    canAttack[i] = true;
                                }
                            }
                        }
                    }
                    else if (canAttack[i])
                        Armies.army[i].ResetColorEffect();
                }
                // END CHECK IF UNIT CAN ATTACK AFTER MOVING


                if (selected == -1)
                {
                    if (canMove[canMove.Count-3] && Armies.army[Armies.army.Count-3].special == Character.Special.Miner)
                    {
                        if(Armies.army[Armies.army.Count - 1].IsNextTo(Armies.army[Armies.army.Count - 3]))
                        {
                            canMove[canMove.Count - 1] = true;
                        }
                        else
                        {
                            canMove[canMove.Count - 1] = false;
                        }
                        if (Armies.army[Armies.army.Count - 2].IsNextTo(Armies.army[Armies.army.Count - 3]))
                        {
                            canMove[canMove.Count - 2] = true;
                        }
                        else
                        {
                            canMove[canMove.Count - 2] = false;
                        }
                    }
                    else
                    {
                        canMove[canMove.Count - 1] = false;
                        canMove[canMove.Count - 2] = false;
                    }
                    if (Globals.mouseState.LeftButtonPressed)
                    {
                        bool contains = false;
                        for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
                        {
                            if (((GameState)StateManager.GetState(1)).GetArmy()[i].Hitbox.Contains(Globals.mouseState.Position) && (canMove[i] || canAttack[i]))
                            {
                                selected = i;
                                contains = true;
                                statsUI.ChangeCharacter((StateManager.GetState(1) as GameState).GetArmy()[i]);
                            }
                        }

                        if (!contains)
                        {
                            selected = -1;
                        }
                        else if (selected != -1)
                        {
                            if (canMove[selected])
                            {
                                
                                int[,] level = ((GameState)StateManager.GetState(1)).DuplicateLevel();
                                
                                ((GameState)StateManager.GetState(1)).SetUnwalkable(ref level, Armies.army[selected]);

                                // Uncomment to Print the grid
                                /*for (int i = 0; i < level.GetLength(0); i++)
                                {
                                    Console.WriteLine();
                                    for (int j = 0; j < level.GetLength(1); j++)
                                        Console.Write(level[i, j]);
                                }*/
                                // Uncomment to Print the grid

                                // PATHFINDING FOR REACHABLE AREA
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
                                Point GPos = Armies.army[selected].GridPosition;
                                AStar.Area(GPos.X, GPos.Y, Armies.army[selected].move);
                                List<Node> nodes = AStar.GetClosed();
                                canMoveTo = new List<Point>();

                                moveArrows = new List<SpineAnimation>();
                                for (int i = 1; i < nodes.Count; i++)
                                {
                                    canMoveTo.Add(new Point(nodes[i].x, nodes[i].y));
                                    moveArrows.Add(AnimationAssets.ArrowIcon);
                                    GPos = Grid.ToPixelLocation(new Point(nodes[i].x, nodes[i].y), Globals.GridLocation, Globals.TileDimensions);
                                    moveArrows[i - 1].Position = new Vector2(GPos.X + Globals.TILE_WIDTH / 2, GPos.Y + Globals.TILE_HEIGHT / 2 - 20);
                                }
                                // END PATHFINDING FOR REACHABLE AREA
                            }
                            if (canAttack[selected])
                            {
                                canAttackThis = new List<int>();
                                attackIcons = new List<SpineAnimation>();

                                for (int i = 0; i < Armies.opponentArmy.Count; i++)
                                {
                                    if (Armies.army[selected].IsNextTo(Armies.opponentArmy[i]))
                                    {
                                        canAttackThis.Add(i);
                                        attackIcons.Add(AnimationAssets.AttackIcon);
                                        attackIcons[attackIcons.Count-1].Position = new Vector2(Armies.opponentArmy[i].position.X + Globals.TILE_WIDTH/2,Armies.opponentArmy[i].position.Y);
                                    }
                                }
                                if (!canMove[selected] && canAttackThis.Count == 0)
                                {
                                    canAttack[selected] = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Globals.mouseState.LeftButtonPressed)
                    {
                        bool contains = false;
                        if (canMoveTo != null)
                        {
                            for (int i = 0; i < canMoveTo.Count; i++)
                            {
                                Rectangle hitbox = new Rectangle(Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).X, Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).Y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
                                if (hitbox.Contains(Globals.mouseState.Position))
                                {
                                    if (Armies.army[selected].special == Character.Special.Bomb)
                                    {
                                        canMove[Armies.army.Count - 3] = false;
                                    }

                                    Globals.multiplayerConnection.SendMove(selected, canMoveTo[i]);
                                    ((GameState)StateManager.GetState(1)).MoveUnit(((GameState)StateManager.GetState(1)).GetArmy()[selected], canMoveTo[i]);

                                    canMove[selected] = false;
                                    selected = -1;
                                    contains = true;
                                    canMoveTo = null;
                                    break;
                                }
                            }
                        }
                        if (canAttackThis != null)
                        {
                            if (selected >= 0 && Armies.army[selected].special != Character.Special.Healer)
                            {
                                for (int i = 0; i < canAttackThis.Count; i++)
                                {
                                    Armies.opponentArmy[canAttackThis[i]].sprite.CurrentFrame = 1;
                                }
                                for (int i = 0; i < canAttackThis.Count; i++)
                                {
                                    Rectangle hitbox = Armies.opponentArmy[canAttackThis[i]].Hitbox;
                                    if (hitbox.Contains(Globals.mouseState.Position))
                                    {
                                        statsUI.RemoveCharacter();
                                        Globals.multiplayerConnection.SendFight(selected, canAttackThis[i]);
                                        ((GameState)StateManager.GetState(1)).AttackUnit(Armies.army[selected], Armies.opponentArmy[canAttackThis[i]]);

                                        canAttack[selected] = false;
                                        selected = -1;
                                        contains = true;
                                        canAttackThis = null;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < canAttackThis.Count; i++)
                                {
                                    Armies.army[canAttackThis[i]].sprite.CurrentFrame = 1;
                                }
                                for (int i = 0; i < canAttackThis.Count; i++)
                                {
                                    Rectangle hitbox = Armies.army[canAttackThis[i]].Hitbox;
                                    if (hitbox.Contains(Globals.mouseState.Position))
                                    {
                                        statsUI.RemoveCharacter();
                                        Globals.multiplayerConnection.SendFight(selected, canAttackThis[i]);
                                        ((GameState)StateManager.GetState(1)).AttackUnit(Armies.army[selected], Armies.army[canAttackThis[i]]);

                                        canAttack[selected] = false;
                                        selected = -1;
                                        contains = true;
                                        canAttackThis = null;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!contains)
                        {
                            selected = -1;
                            canMoveTo = null;
                            canAttackThis = null;
                        }
                        moveArrows = null;
                        attackIcons = null;
                    }
                }
            }

            if (StateManager.GetState(1) is GameState)
                ((GameState)StateManager.GetState(1)).Update(gameTime);
            Globals.multiplayerConnection.Update(gameTime);

            statsUI.UpdateAnimation(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);
            
            /*if (selected >= 0)
            {
                if (canMove[selected])
                {
                    for (int i = 0; i < canMoveTo.Count; i++)
                    {
                        spriteBatch.DrawRectangle(new Rectangle(Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).X, Grid.ToPixelLocation(new Point((int)canMoveTo[i].X, (int)canMoveTo[i].Y), Globals.GridLocation, Globals.TileDimensions).Y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT), Color.BlueViolet * .50f);
                    }
                }
            }/**/

            statsUI.Draw(spriteBatch);
        }

        public override void DrawAnimation(Spine.SkeletonMeshRenderer skeletonRenderer)
        {
            StateManager.GetState(1).DrawAnimation(skeletonRenderer);

            if (moveArrows != null)
            {
                for (int i = 0; i < moveArrows.Count; i++)
                {
                    moveArrows[i].Draw(skeletonRenderer);
                }
            }
            if (attackIcons != null)
            {
                for (int i = 0; i < attackIcons.Count; i++)
                {
                    attackIcons[i].Draw(skeletonRenderer);
                }
            }

            statsUI.DrawAnimation(skeletonRenderer);
        }

        private void SetMovable()
        {
            canMove = new List<bool>();
            canAttack = new List<bool>();

            for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
            {
                canMove.Add(true);
                canAttack.Add(true);
            }
            canMove[canMove.Count - 1] = false;
            canMove[canMove.Count - 2] = false;
            canAttack[canAttack.Count - 1] = false;
            canAttack[canAttack.Count - 2] = false;
        }

        private void EndTurn()
        {
            if (IsTurn)
                Globals.multiplayerConnection.SendEndTurn();
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
            ((GameState)StateManager.GetState(1)).MoveUnit(Armies.opponentArmy[charIndex], gridLocation);
        }

        public void RecievedFight(int charIndexAttacker, int charIndexDefender)
        {
            statsUI.RemoveCharacter();
            ((GameState)StateManager.GetState(1)).AttackUnit(Armies.opponentArmy[charIndexAttacker], Armies.army[charIndexDefender]);
        }

        private void OtherPlayerEndedHisTurn()
        {
            foreach (Character c in Armies.army)
                c.ResetColorEffect();

            SetMovable();
        }
    }
}
