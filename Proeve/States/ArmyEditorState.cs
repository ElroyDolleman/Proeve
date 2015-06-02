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

using Spine;

namespace Proeve.States
{
    class ArmyEditorState : State
    {
        // Read Only Positions
        private Vector2 ArmyShowCasePosition { get { return new Vector2(334, 10); } }
        private Vector2 StartButtonPosition { get { return new Vector2(498, 695); } }
        private Vector2 RankNamePosition { get { return new Vector2(507, 20); } }
        private Vector2 HomeButtonPosition { get { return new Vector2(8, 10); } }

        // Character
        private Dictionary<Character.Rank, Sprite> armyDict;
        private Character.Army currentArmy;

        private Color SelectedColor { get { return Color.Black * .65f; } }
        private Character selectedCharacter;
        
        // Background
        private Color backgroundOverColor { get { return Color.Black * .75f; } }

        private E2DTexture background;

        // UI
        private StatsUI statsUI;
        private Sprite armyShowCase;
        private Sprite armyName;

        // Grid
        private Point gridLocation;
        private int gridWidth, gridHeight;
        private Rectangle GridArea { get { return new Rectangle(gridLocation.X, gridLocation.Y, gridWidth * Globals.TILE_WIDTH, gridHeight * Globals.TILE_HEIGHT); } }

        // Drag
        private const byte DRAG_HOLD_TIME = 200;

        private Point startDragGridPosition;
        private float dragHoldTimer;
        private bool drag;

        // Next Army Buttons
        private Rectangle LeftNextButtonHitbox { get { return new Rectangle(360, 196, 32, 50); } }
        private Rectangle RightNextButtonHitbox { get { return new Rectangle(930, 196, 32, 50); } }

        public ArmyEditorState()
        {
            
        }

        public override void Initialize()
        {
            drag = false;

            // Set grid values
            gridWidth = Globals.GRID_WIDTH;
            gridHeight = 3;
            gridLocation = new Point(Globals.GridLocation.X, Globals.GridLocation.Y + Globals.TILE_HEIGHT * 5);

            // Set sprites/textures
            background = ArtAssets.backgroundGrassLevel;

            // UI
            statsUI = new StatsUI();
            Globals.statsUI = statsUI;
            Globals.statsUI.Diamonds = Globals.diamonds;

            armyShowCase = ArtAssets.ArmyShowCase;
            armyShowCase.position = ArmyShowCasePosition;

            // Set buttons
            buttons.Add(new Button(ArtAssets.StartButton, StartButtonPosition));
            buttons[0].ClickEvent += Ready;

            buttons.Add(new Button(ArtAssets.HomeButton, HomeButtonPosition));
            buttons[1].ClickEvent += Quit;

            Armies.army = new List<Character>();
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Leader).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.General).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.General).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Captain).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Captain).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Soldier).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Soldier).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Miner).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Bomb).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Bomb).Clone());

            Armies.army[0].position = Grid.ToPixelLocation(new Point(0, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[1].position = Grid.ToPixelLocation(new Point(1, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[2].position = Grid.ToPixelLocation(new Point(2, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[3].position = Grid.ToPixelLocation(new Point(3, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[4].position = Grid.ToPixelLocation(new Point(4, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[5].position = Grid.ToPixelLocation(new Point(5, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[6].position = Grid.ToPixelLocation(new Point(6, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[7].position = Grid.ToPixelLocation(new Point(7, 0), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[8].position = Grid.ToPixelLocation(new Point(0, 1), gridLocation, Globals.TileDimensions).ToVector2();
            Armies.army[9].position = Grid.ToPixelLocation(new Point(1, 1), gridLocation, Globals.TileDimensions).ToVector2();

            SelectCharacter(Armies.army[0]);

            armyName = ArtAssets.ArmyNames;
            armyName.position = RankNamePosition;
            SetArmyDictionary(Character.Army.Normal);
        }

        private void Ready()
        {
            statsUI.RemoveCharacter();
            Globals.background = background;

            selectedCharacter.ResetColorEffect();
            ((GameState)StateManager.GetState(1)).SetArmy(Armies.army);

            StateManager.ChangeState(Settings.STATES.MatchFinder);
        }

        private void Quit()
        {
            StateManager.RemoveState();
            StateManager.ChangeState(Settings.STATES.MainMenu);
        }

        public override void Update(GameTime gameTime)
        {
            if (!drag)
                if (Globals.mouseState.LeftButtonHold && startDragGridPosition == Grid.ToGridLocation(Globals.mouseState.Position.ToPoint(), gridLocation, Globals.TileDimensions) && Main.WindowRectangle.Contains(Globals.mouseState.Position))
                {
                    dragHoldTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (!(dragHoldTimer < DRAG_HOLD_TIME))
                    {
                        drag = true; 
                        dragHoldTimer = 0;
                    }
                }

            if (Globals.mouseState.LeftButtonPressed && Main.WindowRectangle.Contains(Globals.mouseState.Position))
            {
                if (selectedCharacter.Hitbox.Contains(Globals.mouseState.Position))
                    drag = true;
                else
                    foreach (Character c in Armies.army)
                        if (c.Hitbox.Contains(Globals.mouseState.Position))
                        {
                            SelectCharacter(c);
                        }

                // Check switchable unit
                foreach (KeyValuePair<Character.Rank, Sprite> entry in armyDict)
                {
                    Rectangle hitbox = new Rectangle((int)entry.Value.position.X, (int)entry.Value.position.Y, entry.Value.sourceRectangle.Width, entry.Value.sourceRectangle.Height);

                    if (hitbox.Contains(Globals.mouseState.Position))
                    {
                        if (selectedCharacter.special == Character.Special.None)
                        {
                            if (selectedCharacter.rank == entry.Key)
                                ChangeCharacter(entry.Key);
                        }
                        else
                        {
                            switch (entry.Key)
                            {
                                case Character.Rank.Spy:
                                case Character.Rank.Miner:
                                case Character.Rank.Healer:
                                    ChangeCharacter(entry.Key);
                                    break;
                                case Character.Rank.Bomb:
                                    if (selectedCharacter.rank == Character.Rank.Bomb)
                                        ChangeCharacter(entry.Key);
                                    break;
                            }
                        }
                    }
                }

                if (LeftNextButtonHitbox.Contains(Globals.mouseState.Position))
                {
                    switch(currentArmy)
                    {
                        case Character.Army.Normal: SetArmyDictionary(Character.Army.Sea); break;
                        case Character.Army.Tiki: SetArmyDictionary(Character.Army.Normal); break;
                        case Character.Army.Sea: SetArmyDictionary(Character.Army.Tiki); break;
                    }
                }

                if (RightNextButtonHitbox.Contains(Globals.mouseState.Position))
                {
                    switch (currentArmy)
                    {
                        case Character.Army.Normal: SetArmyDictionary(Character.Army.Tiki); break;
                        case Character.Army.Tiki: SetArmyDictionary(Character.Army.Sea); break;
                        case Character.Army.Sea: SetArmyDictionary(Character.Army.Normal); break;
                    }
                }
            }

            if (drag && Globals.mouseState.LeftButtonReleased)
            {
                Vector2 mouseGridPosition = (Grid.ToGridLocation(Globals.mouseState.Position.ToPoint(), gridLocation, Globals.TileDimensions) * Globals.TileDimensions + gridLocation).ToVector2();

                if (GridArea.Contains(mouseGridPosition))
                {
                    bool overlap = false;

                    // Check for overlapping with another character
                    foreach (Character c in Armies.army)
                        if (c.position == mouseGridPosition)
                        {
                            overlap = true;

                            // Swap drag Character with overlap character
                            c.position = selectedCharacter.position;
                            selectedCharacter.position = mouseGridPosition;

                            break;
                        }

                    if (!overlap)
                        selectedCharacter.position = mouseGridPosition;
                }

                drag = false;
            }

            statsUI.UpdateAnimation(gameTime);
            statsUI.UpdateWeaponChanging();
        }

        private void SelectCharacter(Character c)
        {
            if (selectedCharacter != null)
                selectedCharacter.ResetColorEffect();

            selectedCharacter = c;
            statsUI.ChangeCharacter(selectedCharacter);

            selectedCharacter.ColorEffect = SelectedColor;

            dragHoldTimer = 0;
            startDragGridPosition = Grid.ToGridLocation(selectedCharacter.position.ToPoint(), gridLocation, Globals.TileDimensions);
        }

        private void ChangeCharacter(Character.Rank rank)
        {
            Vector2 position = selectedCharacter.position;
            int index = Armies.army.IndexOf(selectedCharacter);

            Armies.army[index] = Armies.GetCharacter(rank, currentArmy);
            Armies.army[index].position = position;

            SelectCharacter(Armies.army[index]);
        }

        private void SetArmyDictionary(Character.Army army)
        {
            Sprite armySheet;

            switch (army)
            {
                default:
                case Character.Army.Normal: armySheet = ArtAssets.MedievalArmySheet; background = ArtAssets.backgroundGrassLevel; break;
                case Character.Army.Tiki: armySheet = ArtAssets.TikiArmySheet; background = ArtAssets.backgroundTikiLevel; break;
                case Character.Army.Sea: armySheet = ArtAssets.SeaArmySheet; background = ArtAssets.backgroundSeaLevel; break;
            }

            armyDict = new Dictionary<Character.Rank, Sprite>();
            armyDict.Add(Character.Rank.Leader, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Leader].CurrentFrame = 8;
            armyDict.Add(Character.Rank.General, (Sprite)armySheet.Clone()); armyDict[Character.Rank.General].CurrentFrame = 7;
            armyDict.Add(Character.Rank.Captain, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Captain].CurrentFrame = 6;
            armyDict.Add(Character.Rank.Soldier, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Soldier].CurrentFrame = 5;
            armyDict.Add(Character.Rank.Bomb, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Bomb].CurrentFrame = 4;
            armyDict.Add(Character.Rank.Spy, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Spy].CurrentFrame = 1;
            armyDict.Add(Character.Rank.Miner, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Miner].CurrentFrame = 3;
            armyDict.Add(Character.Rank.Healer, (Sprite)armySheet.Clone()); armyDict[Character.Rank.Healer].CurrentFrame = 2;

            armyDict[Character.Rank.Leader].position = new Vector2(422, 107);
            armyDict[Character.Rank.General].position = new Vector2(542, 107);
            armyDict[Character.Rank.Captain].position = new Vector2(662, 107);
            armyDict[Character.Rank.Soldier].position = new Vector2(782, 107);
            armyDict[Character.Rank.Bomb].position = new Vector2(422, 227);
            armyDict[Character.Rank.Spy].position = new Vector2(542, 227);
            armyDict[Character.Rank.Miner].position = new Vector2(662, 227);
            armyDict[Character.Rank.Healer].position = new Vector2(782, 227);

            currentArmy = army;
            armyName.CurrentFrame = (int)army + 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.DrawE2DTexture(background, Vector2.Zero);

            spriteBatch.DrawRectangle(Vector2.Zero, Main.WindowWidth, GridArea.Top, backgroundOverColor);
            spriteBatch.DrawRectangle(new Vector2(0, GridArea.Top), GridArea.Left, GridArea.Height, backgroundOverColor);
            spriteBatch.DrawRectangle(new Vector2(GridArea.Right, GridArea.Top), Main.WindowWidth - GridArea.Right, GridArea.Height, backgroundOverColor);
            spriteBatch.DrawRectangle(new Vector2(0, GridArea.Bottom), Main.WindowWidth, Main.WindowHeight - GridArea.Bottom, backgroundOverColor);

            // UI
            statsUI.Draw(spriteBatch);
            armyShowCase.Draw(spriteBatch);
            armyName.Draw(spriteBatch);

            // Draw all character chips
            foreach(Character c in Armies.army)
                if (!drag || selectedCharacter != c)
                    c.Draw(spriteBatch);

            // Draw selected character
            if (drag)
            {
                int W = selectedCharacter.sprite.sourceRectangle.Width / 2;
                int H = selectedCharacter.sprite.sourceRectangle.Height / 2;

                Vector2 dragPos = Globals.mouseState.Position - new Vector2(W, H);
                dragPos.X = MathHelper.Clamp(dragPos.X, GridArea.Left - W, GridArea.Right - W);
                dragPos.Y = MathHelper.Clamp(dragPos.Y, GridArea.Top - H, GridArea.Bottom - H);

                spriteBatch.DrawSprite(selectedCharacter.sprite, dragPos);
            }

            foreach (KeyValuePair<Character.Rank, Sprite> entry in armyDict)
            {
                entry.Value.Draw(spriteBatch);

                if (selectedCharacter.rank != entry.Key)
                {
                    switch(entry.Key)
                    {
                        case Character.Rank.Miner:
                        case Character.Rank.Spy:
                        case Character.Rank.Healer:
                            if (selectedCharacter.special == Character.Special.None || selectedCharacter.rank == Character.Rank.Bomb)
                                goto default;
                            break;
                        default:
                            Sprite clone = (Sprite)entry.Value.Clone();
                            clone.colorEffect = Color.Black * .78f;
                            clone.Draw(spriteBatch);
                            break;
                    }
                }
            }
        }

        public override void DrawAnimation(SkeletonMeshRenderer skeletonRenderer)
        {
            statsUI.DrawAnimation(skeletonRenderer);
        }
    }
}
