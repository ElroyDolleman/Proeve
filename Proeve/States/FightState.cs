using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.States
{
    class FightState : State
    {
        // Animation hit constants
        private const float AXE_HIT_TIME = .528f;
        private const float SWORD_HIT_TIME = .528f;
        private const float SHIELD_HIT_TIME = .350f;

        private const float AXE_CRIT_HIT_TIME = .528f;
        private const float SWORD_CRIT_HIT_TIME = .528f;
        private const float SHIELD_CRIT_HIT_TIME = .528f;

        // Readonly positions
        private Vector2 MyAnimationPosition { get { return new Vector2(200, 340); } }
        private Vector2 EnemyAnimationPosition { get { return new Vector2(Main.WindowWidth - 200, 340); } }

        // Characters
        private Character character;
        private Character enemyCharacter;

        // Fighting
        private bool myAttackTurn;
        private int damage;

        // Animations
        private Dictionary<Character.Weapon, SpineAnimation> weaponAnimations;
        private Dictionary<Character.Weapon, SpineAnimation> criticalAnimations;
        private Dictionary<Character.Special, SpineAnimation> specialAnimations;
        private SpineAnimation currentAnimation;

        // Flickering Fields
        private const int FLICKER_INTERVAL = 160;
        private const int FLICKER_LIMIT = 3;
        private bool isFlickering;
        private bool show;
        private float flickerTimer;
        private int flickerAmount;

        public FightState()
        {
            
        }

        public override void Initialize()
        {
            // WEAPON DICTIONARY
            weaponAnimations = new Dictionary<Character.Weapon, SpineAnimation>();

            weaponAnimations.Add(Character.Weapon.Axe, AnimationAssets.AxeNormalAttack);
            weaponAnimations[Character.Weapon.Axe].Position = Main.WindowCenter;
            weaponAnimations[Character.Weapon.Axe].loop = false;

            weaponAnimations.Add(Character.Weapon.Sword, AnimationAssets.SwordNormalAttack);
            weaponAnimations[Character.Weapon.Sword].Position = Main.WindowCenter;
            weaponAnimations[Character.Weapon.Sword].loop = false;

            weaponAnimations.Add(Character.Weapon.Shield, AnimationAssets.ShieldNormalAttack);
            weaponAnimations[Character.Weapon.Shield].Position = Main.WindowCenter;
            weaponAnimations[Character.Weapon.Shield].loop = false;

            weaponAnimations.Add(Character.Weapon.None, AnimationAssets.AxeNormalAttack);
            weaponAnimations[Character.Weapon.None].Position = Main.WindowCenter;

            // CRITICAL DICTIONARY
            criticalAnimations = new Dictionary<Character.Weapon, SpineAnimation>();

            criticalAnimations.Add(Character.Weapon.Axe, AnimationAssets.AxeCritAttack);
            criticalAnimations[Character.Weapon.Axe].Position = Main.WindowCenter;
            criticalAnimations[Character.Weapon.Axe].loop = false;

            criticalAnimations.Add(Character.Weapon.Sword, AnimationAssets.SwordCritAttack);
            criticalAnimations[Character.Weapon.Sword].Position = Main.WindowCenter;
            criticalAnimations[Character.Weapon.Sword].loop = false;

            criticalAnimations.Add(Character.Weapon.Shield, AnimationAssets.ShieldCritAttack);
            criticalAnimations[Character.Weapon.Shield].Position = Main.WindowCenter;
            criticalAnimations[Character.Weapon.Shield].loop = false;

            // SPECIAL DICTIONARY
            specialAnimations = new Dictionary<Character.Special, SpineAnimation>();

            specialAnimations.Add(Character.Special.Bomb, AnimationAssets.AxeNormalAttack);
            specialAnimations.Add(Character.Special.Healer, AnimationAssets.AxeNormalAttack);
            specialAnimations.Add(Character.Special.Minor, AnimationAssets.AxeNormalAttack);
            specialAnimations.Add(Character.Special.Spy, AnimationAssets.AxeNormalAttack);

            show = true;
            flickerTimer = 0;
            flickerAmount = FLICKER_LIMIT;
        }

        public void SetUnits(Character attacker, Character defender)
        {
            if (Globals.multiplayerConnection.myTurn) {
                this.character = attacker;
                this.enemyCharacter = defender;

                myAttackTurn = !(enemyCharacter.special == Character.Special.Bomb && character.special != Character.Special.Minor);
            }
            else {
                this.character = defender;
                this.enemyCharacter = attacker;

                myAttackTurn = (character.special == Character.Special.Bomb && enemyCharacter.special != Character.Special.Minor);
            }

            if (myAttackTurn)
                Attacking(character, enemyCharacter);
            else
                Attacking(enemyCharacter, character);

            currentAnimation.FlipX = !myAttackTurn;

            character.animation.Position = MyAnimationPosition;
            enemyCharacter.animation.Position = EnemyAnimationPosition;
        }

        public override void Update(GameTime gameTime)
        {
            if (!character.IsDead && !enemyCharacter.IsDead)
            {
                if (!currentAnimation.IsPlayingAnimation && false)
                {
                    myAttackTurn = !myAttackTurn;

                    if (myAttackTurn)
                        Attacking(character, enemyCharacter);
                    else
                        Attacking(enemyCharacter, character);

                    currentAnimation.FlipX = !myAttackTurn;
                }

                currentAnimation.Update(gameTime);


                if (currentAnimation.Time >= .5f && currentAnimation.IsPlayingAnimation && !isFlickering)
                {
                    isFlickering = true;
                    flickerAmount = 0;
                    show = false;
                }

                // FLICKER EFFECT
                if (flickerAmount < FLICKER_LIMIT)
                {
                    flickerTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (flickerTimer >= FLICKER_INTERVAL)
                    {
                        show = !show;

                        if (show)
                            flickerAmount++;

                        flickerTimer = 0;
                    }
                }
            }

            character.UpdateSpineAnimation(gameTime);
            enemyCharacter.UpdateSpineAnimation(gameTime);
        }

        private void Attacking(Character attacker, Character defender)
        {
            switch (attacker.special)
            {
                case Character.Special.None:
                    damage = WeaponDamage(attacker.weapon, defender.weapon);
                    if (damage == 1)
                        currentAnimation = weaponAnimations[attacker.weapon];
                    else
                        currentAnimation = criticalAnimations[attacker.weapon];
                    break;
                case Character.Special.Minor:
                    if (defender.special == Character.Special.Bomb) {
                        damage = defender.hp;
                        currentAnimation = specialAnimations[attacker.special];
                    }
                    else { 
                        damage = 1;
                        currentAnimation = weaponAnimations[attacker.weapon];
                    }
                    break;
                case Character.Special.Spy:
                    if (defender.rank == Character.Rank.Marshal) {
                        damage = defender.hp;
                        currentAnimation = specialAnimations[attacker.special];
                    }
                    else {
                        damage = 1;
                        currentAnimation = weaponAnimations[attacker.weapon];
                    }
                    break;
                case Character.Special.Bomb:
                    damage = defender.hp;
                    currentAnimation = specialAnimations[attacker.special];
                    break;
                default:
                    damage = 1;
                    currentAnimation = weaponAnimations[attacker.weapon];
                    break;
            }
        }

        private int WeaponDamage(Character.Weapon attackWeapon, Character.Weapon defendWeapon)
        {
            switch (attackWeapon)
            {
                default:
                case Character.Weapon.Axe:
                    if (defendWeapon == Character.Weapon.Shield)
                        return 2;
                    else
                        return 1;
                case Character.Weapon.Shield:
                    if (defendWeapon == Character.Weapon.Sword)
                        return 2;
                    else
                        return 1;
                case Character.Weapon.Sword:
                    if (defendWeapon == Character.Weapon.Axe)
                        return 2;
                    else
                        return 1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(2).Draw(spriteBatch);

            spriteBatch.DrawRectangle(Main.WindowRectangle, Color.Black * .5f);

            spriteBatch.DrawDebugText("Rank: " + character.rank, 100, 16, Color.White);
            spriteBatch.DrawDebugText("Weapon: " + character.weapon, 100, 32, Color.White);
            spriteBatch.DrawDebugText("Level: " + character.Level, 100, 48, Color.White);
            spriteBatch.DrawDebugText("Steps: " + character.move, 100, 64, Color.White);
            spriteBatch.DrawDebugText("HP: " + character.hp, 100, 80, Color.White);

            spriteBatch.DrawDebugText("Rank: " + enemyCharacter.rank, 800, 16, Color.White);
            spriteBatch.DrawDebugText("Weapon: " + enemyCharacter.weapon, 800, 32, Color.White);
            spriteBatch.DrawDebugText("Level: " + enemyCharacter.Level, 800, 48, Color.White);
            spriteBatch.DrawDebugText("Steps: " + enemyCharacter.move, 800, 64, Color.White);
            spriteBatch.DrawDebugText("HP: " + enemyCharacter.hp, 800, 80, Color.White);

            if (character.IsDead)
                spriteBatch.DrawDebugText("Lost", 100, 120, Color.White);
            else if (enemyCharacter.IsDead)
                spriteBatch.DrawDebugText("Win", 100, 120, Color.White);
            else if (myAttackTurn)
                spriteBatch.DrawDebugText("Damage: " + damage, 100, 120, Color.White);
            else
                spriteBatch.DrawDebugText("Damage: " + damage, 500, 120, Color.White);

            spriteBatch.DrawDebugText("Time: " + currentAnimation.Time, 120, 700, Color.White);
            spriteBatch.DrawDebugText(currentAnimation.AnimationName, 120, 718, Color.White);
        }

        public override void DrawAnimation(Spine.SkeletonMeshRenderer skeletonRenderer)
        {
            if (show || !myAttackTurn)
                character.DrawAnimation(skeletonRenderer);

            if (show || myAttackTurn)
                enemyCharacter.DrawAnimation(skeletonRenderer);

            if (currentAnimation.IsPlayingAnimation)
                currentAnimation.Draw(skeletonRenderer);
        }
    }
}
