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

//using Spine;

namespace Proeve.States
{
    class ArmyEditorState : State
    {
        private bool drag;
        private int dragIndex;
        private E2DTexture background;

        private Point gridLocation;
        private int gridWidth, gridHeight;
        private Rectangle GridArea { get { return new Rectangle(gridLocation.X, gridLocation.Y, gridWidth * Globals.TILE_WIDTH, gridHeight * Globals.TILE_HEIGHT); } }
        private Character selectedCharacter;

        private Dictionary<Character.Weapon, Sprite> WeaponIcons;
        private Vector2 WeaponIconPosition { get { return new Vector2(228, 24); } }

        public ArmyEditorState()
        {
            
        }

        public override void Initialize()
        {
            dragIndex = -1;
            drag = false;

            gridWidth = Globals.GRID_WIDTH;
            gridHeight = 3;

            gridLocation = new Point(Globals.GridLocation.X, Globals.GridLocation.Y + Globals.TILE_HEIGHT * 5);

            background = ArtAssets.editorBackground;

            buttons.Add(new Button(ArtAssets.TestButton, 24, 24));
            buttons[0].ClickEvent += Ready;

            WeaponIcons = new Dictionary<Character.Weapon, Sprite>();
            WeaponIcons.Add(Character.Weapon.Axe, ArtAssets.AxeIcon);
            WeaponIcons.Add(Character.Weapon.Sword, ArtAssets.SwordIcon);
            WeaponIcons.Add(Character.Weapon.Shield, ArtAssets.ShieldIcon);

            WeaponIcons[Character.Weapon.Axe].position =  WeaponIconPosition;
            WeaponIcons[Character.Weapon.Sword].position =  WeaponIconPosition + Vector2.UnitX * WeaponIcons[Character.Weapon.Sword].sourceRectangle.Width;
            WeaponIcons[Character.Weapon.Shield].position =  WeaponIconPosition + Vector2.UnitX * WeaponIcons[Character.Weapon.Shield].sourceRectangle.Width * 2;

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
            WeaponIcons[selectedCharacter.weapon].sourceRectangle.X += WeaponIcons[selectedCharacter.weapon].sourceRectangle.Width;
        }

        private void Ready()
        {
            ((GameState)StateManager.GetState(1)).SetArmy(Armies.army);

            StateManager.ChangeState(Settings.STATES.MatchFinder);
        }

        public override void Update(GameTime gameTime)
        {
            if (Globals.mouseState.LeftButtonPressed)
            {
                foreach (Character c in Armies.army)
                    if (c.Hitbox.Contains(Globals.mouseState.Position))
                    {
                        WeaponIcons[selectedCharacter.weapon].sourceRectangle.X = 0;

                        selectedCharacter = c;
                        dragIndex = Armies.army.IndexOf(c);
                        drag = true;

                        WeaponIcons[c.weapon].sourceRectangle.X += WeaponIcons[c.weapon].sourceRectangle.Width;
                    }

                if (selectedCharacter.special != Character.Special.Bomb)
                    foreach (Character.Weapon w in WeaponIcons.Keys)
                    {
                        Sprite s = WeaponIcons[w];
                        Rectangle hitbox = new Rectangle((int)s.position.X, (int)s.position.Y, s.sourceRectangle.Width, s.sourceRectangle.Height);

                        if (hitbox.Contains(Globals.mouseState.Position))
                            ChangeWeapon(selectedCharacter, w);
                    }
            }

            if (Globals.mouseState.LeftButtonReleased && drag)
            {
                Vector2 mouseGridPosition = (Grid.ToGridLocation(Globals.mouseState.Position.ToPoint(), gridLocation, Globals.TileDimensions) * Globals.TileDimensions + gridLocation).ToVector2();

                if (GridArea.Contains(mouseGridPosition))
                {
                    bool overlap = false;

                    foreach (Character c in Armies.army)
                        if (c.position == mouseGridPosition)
                        {
                            overlap = true;

                            // Swap drag Character with overlap character
                            c.position = Armies.army[dragIndex].position;
                            Armies.army[dragIndex].position = mouseGridPosition;

                            break;
                        }

                    if (!overlap)
                        Armies.army[dragIndex].position = mouseGridPosition;
                }

                drag = false;
                dragIndex = -1;
            }
        }

        private void ChangeWeapon(Character character, Character.Weapon weapon)
        {
            WeaponIcons[character.weapon].sourceRectangle.X = 0;
            character.weapon = weapon;
            WeaponIcons[character.weapon].sourceRectangle.X = WeaponIcons[character.weapon].sourceRectangle.Width;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawE2DTexture(background, Vector2.Zero);

            foreach(Character c in Armies.army)
            {
                if (dragIndex != Armies.army.IndexOf(c))
                    c.Draw(spriteBatch);
                else
                    spriteBatch.DrawSprite(c.sprite, Globals.mouseState.Position - new Vector2(41, 41));
            }

            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Axe]);
            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Sword]);
            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Shield]);

            spriteBatch.DrawRectangle(selectedCharacter.position, Globals.TILE_WIDTH, Globals.TILE_HEIGHT, Color.Red * .45f);
            spriteBatch.DrawDebugText("Rank: " + selectedCharacter.rank, 100, 16, Color.White);
            spriteBatch.DrawDebugText("Weapon: " + selectedCharacter.weapon, 100, 32, Color.White);
            spriteBatch.DrawDebugText("Level: " + selectedCharacter.Level, 100, 48, Color.White);
            spriteBatch.DrawDebugText("Steps: " + selectedCharacter.move, 100, 64, Color.White);
            spriteBatch.DrawDebugText("HP: " + selectedCharacter.hp, 100, 80, Color.White);
        }
    }
}
