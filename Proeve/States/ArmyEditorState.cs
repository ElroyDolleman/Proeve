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
        // Read only positions
        private Vector2 CharacterInformationUIPosition { get { return new Vector2(0, 102); } }
        private Vector2 HealthBarPosition { get { return new Vector2(118, 378); } }

        private Vector2 AxeIconPosition { get { return new Vector2(162, 620); } }
        private Vector2 SwordIconPosition { get { return new Vector2(75, 620); } }
        private Vector2 ShieldIconPosition { get { return new Vector2(119, 542); } }

        private Vector2 AnimationPosition { get { return new Vector2(118, 240); } }

        // Character
        private Color SelectedColor { get { return Color.Black * .65f; } }

        private Character selectedCharacter;
        
        // Background
        private Color backgroundOverColor { get { return Color.Black * .75f; } }

        private E2DTexture background;

        // UI
        private Sprite characterInformationUI;
        private Healthbar healthbar;

        // Grid
        private Point gridLocation;
        private int gridWidth, gridHeight;
        private Rectangle GridArea { get { return new Rectangle(gridLocation.X, gridLocation.Y, gridWidth * Globals.TILE_WIDTH, gridHeight * Globals.TILE_HEIGHT); } }

        // Weapons
        private Dictionary<Character.Weapon, Sprite> WeaponIcons;

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

            characterInformationUI = ArtAssets.CharacterInformationUI;
            characterInformationUI.position = CharacterInformationUIPosition;

            // Set weapon icons
            WeaponIcons = new Dictionary<Character.Weapon, Sprite>();
            WeaponIcons.Add(Character.Weapon.Axe, ArtAssets.AxeIcon);
            WeaponIcons.Add(Character.Weapon.Sword, ArtAssets.SwordIcon);
            WeaponIcons.Add(Character.Weapon.Shield, ArtAssets.ShieldIcon);

            WeaponIcons[Character.Weapon.Axe].position = AxeIconPosition;
            WeaponIcons[Character.Weapon.Sword].position = SwordIconPosition;
            WeaponIcons[Character.Weapon.Shield].position = ShieldIconPosition;

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

            selectedCharacter = Armies.army[0];
            selectedCharacter.animation.Position = AnimationPosition;
            selectedCharacter.ColorEffect = SelectedColor;

            WeaponIcons[selectedCharacter.weapon].CurrentFrame = 2;

            SetHealthBar(selectedCharacter.maxHP);
        }

        private void Ready()
        {
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
                            selectedCharacter.ResetColorEffect();

                            if (selectedCharacter.weapon != Character.Weapon.None)
                                WeaponIcons[selectedCharacter.weapon].CurrentFrame = 1;

                            selectedCharacter = c;

                            SetHealthBar(selectedCharacter.maxHP);

                            selectedCharacter.ColorEffect = SelectedColor;
                            selectedCharacter.animation.Position = AnimationPosition;

                            if (selectedCharacter.weapon != Character.Weapon.None)
                                WeaponIcons[c.weapon].CurrentFrame = 2;

                            dragHoldTimer = 0;
                            startDragGridPosition = Grid.ToGridLocation(selectedCharacter.position.ToPoint(), gridLocation, Globals.TileDimensions);
                        }

                if (selectedCharacter.special == Character.Special.None)
                    foreach (Character.Weapon w in WeaponIcons.Keys)
                        if (Vector2.Distance(Globals.mouseState.Position, WeaponIcons[w].position) < WeaponIcons[w].sourceRectangle.Width / 2)
                            ChangeWeapon(selectedCharacter, w);
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

            selectedCharacter.UpdateSpineAnimation(gameTime);
        }

        private void SetHealthBar(int hp)
        {
            healthbar = new Healthbar(ArtAssets.Healthbar, hp);
            healthbar.Center = HealthBarPosition;
        }

        private void ChangeWeapon(Character character, Character.Weapon weapon)
        {
            WeaponIcons[character.weapon].CurrentFrame = 1;
            character.weapon = weapon;
            WeaponIcons[character.weapon].CurrentFrame = 2;
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
            characterInformationUI.Draw(spriteBatch);
            healthbar.Draw(spriteBatch);

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

            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Axe]);
            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Sword]);
            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Shield]);
        }

        public override void DrawAnimation(SkeletonMeshRenderer skeletonRenderer)
        {
            selectedCharacter.DrawAnimation(skeletonRenderer);
        }
    }
}
