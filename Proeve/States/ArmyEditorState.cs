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
        // Character
        private Color SelectedColor { get { return Color.Black * .65f; } }

        private Character selectedCharacter;
        
        // Background
        private Color backgroundOverColor { get { return Color.Black * .75f; } }

        private E2DTexture background;

        // UI
        private StatsUI statsUI;

        // Grid
        private Point gridLocation;
        private int gridWidth, gridHeight;
        private Rectangle GridArea { get { return new Rectangle(gridLocation.X, gridLocation.Y, gridWidth * Globals.TILE_WIDTH, gridHeight * Globals.TILE_HEIGHT); } }

        // Drag
        private const byte DRAG_HOLD_TIME = 200;

        private Point startDragGridPosition;
        private float dragHoldTimer;
        private bool drag;

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

            // Set buttons
            buttons.Add(new Button(ArtAssets.TestButton, 24, 24));
            buttons[0].ClickEvent += Ready;

            Armies.army = new List<Character>();
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Marshal).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.General).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.General).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Majoor).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Majoor).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Captain).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Captain).Clone());
            Armies.army.Add(Armies.GetCharacter(Character.Rank.Special).Clone());
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
        }

        private void Ready()
        {
            statsUI.RemoveCharacter();

            selectedCharacter.ResetColorEffect();
            ((GameState)StateManager.GetState(1)).SetArmy(Armies.army);

            StateManager.ChangeState(Settings.STATES.MatchFinder);
        }

        public override void Update(GameTime gameTime)
        {
            if (!drag)
                if (Globals.mouseState.LeftButtonHold && startDragGridPosition == Grid.ToGridLocation(Globals.mouseState.Position.ToPoint(), gridLocation, Globals.TileDimensions))
                {
                    dragHoldTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (!(dragHoldTimer < DRAG_HOLD_TIME))
                    {
                        drag = true; 
                        dragHoldTimer = 0;
                    }
                }

            if (Globals.mouseState.LeftButtonPressed)
            {
                if (selectedCharacter.Hitbox.Contains(Globals.mouseState.Position))
                    drag = true;
                else
                    foreach (Character c in Armies.army)
                        if (c.Hitbox.Contains(Globals.mouseState.Position))
                        {
                            SelectCharacter(c);
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
        }

        public override void DrawAnimation(SkeletonMeshRenderer skeletonRenderer)
        {
            statsUI.DrawAnimation(skeletonRenderer);
        }
    }
}
