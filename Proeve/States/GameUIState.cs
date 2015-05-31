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
        private Vector2 HomeButtonPosition { get { return new Vector2(8, 10); } }

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

            buttons.Add(new Button(ArtAssets.HomeButton, HomeButtonPosition));
            buttons[1].ClickEvent = QuitMatch;

            SetMovable();

            Globals.multiplayerConnection.ReceiveMove += ReceivedMove;
            Globals.multiplayerConnection.ReceiveFight += ReceivedFight;
            Globals.multiplayerConnection.ReceiveEndTurn += OtherPlayerEndedHisTurn;

            Globals.earnedDiamonds = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsTurn)
            {
                buttons[0].enabled = false;
            }
            else
            {
                buttons[0].enabled = true;
            }
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
            if (selected >= 0 && canAttackThis != null && canAttack[selected])
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

            // CONNECTION LOST
            if (Globals.multiplayerConnection.ConnectionLost)
                StateManager.ChangeState(Settings.STATES.Result);
            // END CONNECTION LOST

            if (IsTurn && !Globals.multiplayerConnection.IsWaitingForResponse)
            {
                CheckIfCanAttack();
                
                if (selected != -1)
                {
                    if (Globals.mouseState.LeftButtonPressed)
                    {
                        bool contains = false;
                        contains = TestForMove(gameTime);
                        if (!contains)
                            contains = TestForAttack(gameTime);

                        if (!contains)
                        {
                            canMoveTo = null;
                        }
                        moveArrows = null;
                        attackIcons = null;
                    }
                }

                if (Globals.mouseState.LeftButtonPressed)
                {
                    SelectUnit(gameTime);
                }
            }

            if (StateManager.GetState(1) is GameState)
                ((GameState)StateManager.GetState(1)).Update(gameTime);
            Globals.multiplayerConnection.Update(gameTime);

            statsUI.UpdateAnimation(gameTime);

            if (selected > -1 && !canAttack[selected])
                canAttackThis = null;
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
            if (attackIcons != null && Armies.army[selected].waypoints.Count == 0)
            {
                for (int i = 0; i < attackIcons.Count; i++)
                {
                    attackIcons[i].Draw(skeletonRenderer);
                }
            }

            statsUI.DrawAnimation(skeletonRenderer);
        }

        public bool TestForMove(GameTime gameTime)
        {
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
                        if (!canAttack[selected])
                            selected = -1;
                        canMoveTo = null;
                        return true; ;
                    }
                }
            }
            return false;
        }

        public bool TestForAttack(GameTime gameTime)
        {
            if (selected >= 0 && canAttack[selected] && canAttackThis != null)
            {
                if (Armies.army[selected].special != Character.Special.Healer)
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
                            if (!canMove[selected])
                                selected = -1;
                            return true;
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
                            if (!canMove[selected])
                                selected = -1;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void SelectUnit(GameTime gameTime)
        {
            for (int i = 0; i < Armies.army.Count; i++)
            {
                if (Armies.army[i].Hitbox.Contains(Globals.mouseState.Position) && (canMove[i] || canAttack[i]) && Armies.army[i].waypoints.Count == 0 && !Armies.army[i].IsDead)
                {
                    bool valid = true;
                    if (canAttackThis != null && selected >= 0 && Armies.army[selected].special == Character.Special.Healer)
                    {
                        for (int j = 0; j < canAttackThis.Count; j++)
                        {
                            if (i == canAttackThis[j])
                            {
                                valid = false;
                            }
                        }
                    }
                    if (valid && StateManager.GetState(1) is GameState)
                    {
                        selected = i;
                        statsUI.ChangeCharacter(((GameState)StateManager.GetState(1)).GetArmy()[i]);
                    }
                }
            }

            if (selected != -1)
            {
                if (canMove[selected])
                {
                    GetWalkable(gameTime);
                }
                if (canAttack[selected])
                {
                    GetAttackable(gameTime);
                }
            }
        }

        public void GetWalkable(GameTime gameTime)
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
                moveArrows[i - 1].Update(gameTime);
                moveArrows[i - 1].Update(gameTime);
            }
            // END PATHFINDING FOR REACHABLE AREA
        }

        public void GetAttackable(GameTime gameTime)
        {
            canAttackThis = new List<int>();
            attackIcons = new List<SpineAnimation>();

            if (Armies.army[selected].special != Character.Special.Healer)
            {
                for (int i = 0; i < Armies.opponentArmy.Count; i++)
                {
                    if (Armies.army[selected].IsNextTo(Armies.opponentArmy[i]) && !Armies.opponentArmy[i].IsDead)
                    {
                        canAttackThis.Add(i);
                        attackIcons.Add(AnimationAssets.AttackIcon);
                        attackIcons[attackIcons.Count - 1].Position = new Vector2(Armies.opponentArmy[i].position.X + Globals.TILE_WIDTH / 2, Armies.opponentArmy[i].position.Y);
                    }
                }
                if (!canMove[selected] && canAttackThis.Count == 0)
                {
                    canAttack[selected] = false;
                }
            }
            else
            {
                for (int i = 0; i < Armies.army.Count; i++)
                {
                    if (Armies.army[selected].IsNextTo(Armies.army[i]) && !Armies.army[i].IsDead && Armies.army[i].hp != Armies.army[i].maxHP)
                    {
                        canAttackThis.Add(i);
                        attackIcons.Add(AnimationAssets.HealIcon);
                        attackIcons[attackIcons.Count - 1].Position = new Vector2(Armies.army[i].position.X + Globals.TILE_WIDTH / 2, Armies.army[i].position.Y);
                    }
                }
                if (!canMove[selected] && canAttackThis.Count == 0)
                {
                    canAttack[selected] = false;
                }
            }
        }

        public void CheckIfCanAttack()
        {
            for (int i = 0; i < Armies.army.Count; i++)
            {
                if (!canMove[i] && canAttack[i])
                {
                    canAttack[i] = false;
                    if (Armies.army[i].special != Character.Special.Healer)
                    {
                        for (int j = 0; j < Armies.opponentArmy.Count; j++)
                        {
                            if (Armies.army[i].IsNextTo(Armies.opponentArmy[j]) && !Armies.opponentArmy[j].IsDead)
                            {
                                canAttack[i] = true;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Armies.army.Count; j++)
                        {
                            if (Armies.army[i].IsNextTo(Armies.army[j]) && !Armies.army[j].IsDead)
                            {
                                canAttack[i] = true;
                            }
                        }
                    }
                }
                if (!canAttack[i] && !canMove[i] && Armies.army[i].waypoints.Count == 0)
                    Armies.army[i].ColorEffect = Color.Black * .5f;
            }

            if (canMove[canMove.Count - 3] && Armies.army[Armies.army.Count - 3].special == Character.Special.Miner)
            {
                if (Armies.army[Armies.army.Count - 1].IsNextTo(Armies.army[Armies.army.Count - 3]))
                {
                    Armies.army[canMove.Count - 1].ResetColorEffect();
                    canMove[canMove.Count - 1] = true;
                }
                else
                {
                    canMove[canMove.Count - 1] = false;
                }
                if (Armies.army[Armies.army.Count - 2].IsNextTo(Armies.army[Armies.army.Count - 3]))
                {
                    Armies.army[canMove.Count - 2].ResetColorEffect();
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
            return ((GameState)StateManager.GetState(1)).GetEnemyArmy();
        }

        private void ReceivedMove(int charIndex, Point gridLocation)
        {
            ((GameState)StateManager.GetState(1)).MoveUnit(Armies.opponentArmy[charIndex], gridLocation);
        }

        public void ReceivedFight(int charIndexAttacker, int charIndexDefender)
        {
            statsUI.RemoveCharacter();
            ((GameState)StateManager.GetState(1)).AttackUnit(Armies.opponentArmy[charIndexAttacker], Armies.opponentArmy[charIndexAttacker].special != Character.Special.Healer ? Armies.army[charIndexDefender] : Armies.opponentArmy[charIndexDefender]);
        }

        private void OtherPlayerEndedHisTurn()
        {
            foreach (Character c in Armies.army)
                c.ResetColorEffect();

            SetMovable();
        }

        private void QuitMatch()
        {
            Armies.army[0].hp = 0;
        }
    }
}
