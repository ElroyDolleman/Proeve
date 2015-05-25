using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        #endregion

        public Character SelectedCharacter { get; private set; }
        private Healthbar healthbar;

        private Sprite background;
        private Sprite stepCount;
        private Sprite rankName;

        private Dictionary<Character.Weapon, Sprite> WeaponIcons;
        private Dictionary<Character.Special, int> specialRankNameFrames;
        private Dictionary<Character.Rank, int> rankNameFrames;

        public StatsUI()
        {
            // Set sprites
            background = ArtAssets.CharacterInformationUI;
            background.position = StatsUIPosition;

            stepCount = ArtAssets.StepCount;
            stepCount.position = StepCountPosition;

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
            rankNameFrames.Add(Character.Rank.Special, 1);
            rankNameFrames.Add(Character.Rank.Bomb, 8);

            specialRankNameFrames = new Dictionary<Character.Special, int>();
            specialRankNameFrames.Add(Character.Special.Miner, 3);
            specialRankNameFrames.Add(Character.Special.Healer, 5);
            specialRankNameFrames.Add(Character.Special.Spy, 1);

            rankName = ArtAssets.RankNamesNormal;
            rankName.position = RankNamePosition;
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

            rankName.CurrentFrame = SelectedCharacter.rank != Character.Rank.Special ? rankNameFrames[SelectedCharacter.rank] : specialRankNameFrames[SelectedCharacter.special];
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
            if (Globals.mouseState.LeftButtonPressed && SelectedCharacter != null)
                if (SelectedCharacter.special == Character.Special.None)
                    foreach (Character.Weapon w in WeaponIcons.Keys)
                        if (Vector2.Distance(Globals.mouseState.Position, WeaponIcons[w].position) < WeaponIcons[w].sourceRectangle.Width / 2)
                            ChangeWeapon(SelectedCharacter.weapon, w);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

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
