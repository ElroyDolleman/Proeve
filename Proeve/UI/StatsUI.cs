using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Helpers;
using E2DFramework.Graphics;

using Spine;

using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.UI
{
    class StatsUI
    {
        #region READ ONLY POSITIONS
        private Vector2 StatsUIPosition { get { return new Vector2(0, 102); } }
        private Vector2 HealthBarPosition { get { return new Vector2(118, 378); } }
        private Vector2 StepCountPosition { get { return new Vector2(110, 434); } }

        private Vector2 AxeIconPosition { get { return new Vector2(162, 620); } }
        private Vector2 SwordIconPosition { get { return new Vector2(119, 542); } }
        private Vector2 ShieldIconPosition { get { return new Vector2(75, 620); } }

        private Vector2 AnimationPosition { get { return new Vector2(118, 240); } }

        private Vector2 RankNamePosition { get { return new Vector2(32, 125); } }
        private Vector2 DiamondsUIPosition { get { return new Vector2(95, 10); } }
        private Vector2 DiamondsNumbersPosition { get { return new Vector2(172, 47); } }
        #endregion

        public Character SelectedCharacter { get; private set; }
        private Healthbar healthbar;

        private Sprite background;
        private Sprite stepCount;
        private Sprite rankName;
        private Sprite diamondsUI;

        private Dictionary<Character.Weapon, Sprite> WeaponIcons;
        private Dictionary<Character.Rank, int> rankNameFrames;

        private List<Sprite> diamondsDisplay;
        private const int DIAMONDS_LIMIT = 99999;

        public int Diamonds
        {
            get 
            {
                int score = 0;
                for (int i = 0; i < diamondsDisplay.Count; i++)
                    score += (diamondsDisplay[i].CurrentFrame - 1) * (1 * (int)Math.Pow(10, i));

                return Math.Min(score, DIAMONDS_LIMIT);
            }
            set
            {
                int[] digits = MathHelp.GetDigits(value);

                diamondsDisplay = new List<Sprite>();

                for (int i = 0; i < digits.Length; i++) 
                { 
                    diamondsDisplay.Add(ArtAssets.Numbers);
                    Sprite number = diamondsDisplay[i];
                    number.CurrentFrame = digits[i] + 1;
                    number.position = DiamondsNumbersPosition + new Vector2(i * number.sourceRectangle.Width, 0);
                }
            }
        }

        public StatsUI()
        {
            // Set sprites
            background = ArtAssets.CharacterInformationUI;
            background.position = StatsUIPosition;

            stepCount = ArtAssets.StepCount;
            stepCount.position = StepCountPosition;

            diamondsUI = ArtAssets.DiamondsUI;
            diamondsUI.position = DiamondsUIPosition;

            // Set weapon icons
            WeaponIcons = new Dictionary<Character.Weapon, Sprite>();
            WeaponIcons.Add(Character.Weapon.Axe, ArtAssets.AxeIcon);
            WeaponIcons.Add(Character.Weapon.Sword, ArtAssets.SwordIcon);
            WeaponIcons.Add(Character.Weapon.Shield, ArtAssets.ShieldIcon);

            WeaponIcons[Character.Weapon.Axe].position = AxeIconPosition;
            WeaponIcons[Character.Weapon.Sword].position = SwordIconPosition;
            WeaponIcons[Character.Weapon.Shield].position = ShieldIconPosition;

            // Set rankNameFrames
            rankNameFrames = new Dictionary<Character.Rank, int>();
            rankNameFrames.Add(Character.Rank.Leader, 4);
            rankNameFrames.Add(Character.Rank.General, 6);
            rankNameFrames.Add(Character.Rank.Captain, 7);
            rankNameFrames.Add(Character.Rank.Soldier, 2);
            rankNameFrames.Add(Character.Rank.Spy, 1);
            rankNameFrames.Add(Character.Rank.Healer, 5);
            rankNameFrames.Add(Character.Rank.Miner, 3);
            rankNameFrames.Add(Character.Rank.Bomb, 8);

            rankName = ArtAssets.RankNamesNormal;
            rankName.position = RankNamePosition;

            // Set default score
            Diamonds = 0;
        }

        private void SetHealthBar(int hp)
        {
            healthbar = new Healthbar(ArtAssets.Healthbar, hp);
            healthbar.Center = HealthBarPosition;
        }

        public void ChangeCharacter(Character c)
        {
            if (SelectedCharacter != null)
            {
                if (SelectedCharacter.weapon != Character.Weapon.None)
                    WeaponIcons[SelectedCharacter.weapon].CurrentFrame = 1;
            }

            SelectedCharacter = c;

            healthbar = new Healthbar(ArtAssets.Healthbar, SelectedCharacter.maxHP, SelectedCharacter.hp, Vector2.Zero);
            healthbar.Center = HealthBarPosition;
            stepCount.CurrentFrame = SelectedCharacter.move;

            if (SelectedCharacter.animation.Position != AnimationPosition)
                SelectedCharacter.animation.Position = AnimationPosition;

            SelectedCharacter.animation.Scale = Globals.ANIMATION_SCALE;

            if (SelectedCharacter.weapon != Character.Weapon.None)
                WeaponIcons[c.weapon].CurrentFrame = 2;

            rankName.CurrentFrame = rankNameFrames[SelectedCharacter.rank];
        }

        public void RemoveCharacter()
        {
            if (SelectedCharacter != null)
            {
                if (SelectedCharacter.weapon != Character.Weapon.None)
                    WeaponIcons[SelectedCharacter.weapon].CurrentFrame = 1;

                SelectedCharacter = null;
            }
        }

        private void ChangeWeapon(Character.Weapon oldWeapon, Character.Weapon newWeapon)
        {
            WeaponIcons[oldWeapon].CurrentFrame = 1;
            WeaponIcons[newWeapon].CurrentFrame = 2;

            SelectedCharacter.weapon = newWeapon;
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            if (SelectedCharacter != null)
                SelectedCharacter.UpdateSpineAnimation(gameTime);
        }

        public void UpdateWeaponChanging()
        {
            if (Globals.mouseState.LeftButtonPressed && SelectedCharacter != null && Main.WindowRectangle.Contains(Globals.mouseState.Position))
                if (SelectedCharacter.special == Character.Special.None)
                    foreach (Character.Weapon w in WeaponIcons.Keys)
                        if (Vector2.Distance(Globals.mouseState.Position, WeaponIcons[w].position) < WeaponIcons[w].sourceRectangle.Width / 2)
                            ChangeWeapon(SelectedCharacter.weapon, w);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            diamondsUI.Draw(spriteBatch);
            foreach (Sprite number in diamondsDisplay)
                number.Draw(spriteBatch);

            if (SelectedCharacter != null)
            {
                healthbar.Draw(spriteBatch);

                if (SelectedCharacter.special != Character.Special.Bomb)
                    stepCount.Draw(spriteBatch);

                rankName.Draw(spriteBatch);
            }

            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Axe]);
            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Sword]);
            spriteBatch.DrawSprite(WeaponIcons[Character.Weapon.Shield]);
        }

        public void DrawAnimation(SkeletonMeshRenderer skeletonRenderer)
        {
            if (SelectedCharacter != null)
                SelectedCharacter.DrawAnimation(skeletonRenderer);
        }
    }
}
