using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.UI;
using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.States
{
    class FightState : State
    {
        // Animation hitting
        private Dictionary<Character.Weapon, float> hitMoments;
        private const float AXE_HIT_TIME = .4f;
        private const float SWORD_HIT_TIME = .4f;
        private const float SHIELD_HIT_TIME = .1f;
        private float hitInterval;

        private const float ANIMATION_SCALE = .70f;
        private const float WEAPON_SCALE = .75f;

        // Readonly positions
        private Vector2 MyAnimationPosition { get { return new Vector2(310, 320); } }
        private Vector2 EnemyAnimationPosition { get { return new Vector2(Main.WindowWidth - 310, 320); } }

        private Vector2 MyAttackPosition { get { return new Vector2(Main.WindowCenter.X + 40, Main.WindowCenter.Y); } }
        private Vector2 EnemyAttackPosition { get { return new Vector2(Main.WindowCenter.X - 40, Main.WindowCenter.Y); } }

        private Vector2 MyHealthbarPosition { get { return new Vector2(300, 520); } }
        private Vector2 EnemyHealthbarPosition { get { return new Vector2(620, 520); } }

        private Vector2 MyDamagePosition { get { return new Vector2(Main.WindowWidth - 280, 380); } }
        private Vector2 EnemyDamagePosition { get { return new Vector2(280, 380); } }

        private Vector2 MyWeaponPosition { get { return new Vector2(220, 510); } }
        private Vector2 EnemyWeaponPosition { get { return new Vector2(Main.WindowWidth - 220, 510); } }

        // Characters
        private Character character;
        private Character enemyCharacter;

        // Fighting
        private bool myAttackTurn;
        private int damage;
        private Sprite damageSprite;
        private float damageSpriteAlpha;

        private const int DAMAGE_SPEED = 3;
        private const float FADE_OUT_SPEED = .2f;

        // Animations
        private Dictionary<Character.Weapon, SpineAnimation> weaponAnimations;
        private Dictionary<Character.Weapon, SpineAnimation> criticalAnimations;
        private Dictionary<Character.Special, SpineAnimation> specialAnimations;
        private SpineAnimation currentAnimation;

        // Flickering Fields
        private const int FLICKER_INTERVAL = 120;
        private const int FLICKER_LIMIT = 2;
        private bool isFlickering;
        private bool show;
        private float flickerTimer;
        private int flickerAmount;

        // Timer
        private float timer;
        private const int ATTACK_INTERVAL = 500;

        // UI
        private Sprite background;
        private Sprite vsText;
        private Healthbar myHealthBar, enemyHealthbar;

        private Sprite axeIcon, swordIcon, shieldIcon;

        public FightState()
        {
            
        }

        public override void Initialize()
        {
            // WEAPON DICTIONARY
            weaponAnimations = new Dictionary<Character.Weapon, SpineAnimation>();

            weaponAnimations.Add(Character.Weapon.Axe, AnimationAssets.AxeNormalAttack);
            weaponAnimations[Character.Weapon.Axe].loop = false;
            weaponAnimations[Character.Weapon.Axe].Scale = WEAPON_SCALE;

            weaponAnimations.Add(Character.Weapon.Sword, AnimationAssets.SwordNormalAttack);
            weaponAnimations[Character.Weapon.Sword].loop = false;
            weaponAnimations[Character.Weapon.Sword].Scale = WEAPON_SCALE;

            weaponAnimations.Add(Character.Weapon.Shield, AnimationAssets.ShieldNormalAttack);
            weaponAnimations[Character.Weapon.Shield].loop = false;
            weaponAnimations[Character.Weapon.Shield].Scale = WEAPON_SCALE;

            weaponAnimations.Add(Character.Weapon.None, AnimationAssets.AxeNormalAttack);
            weaponAnimations[Character.Weapon.None].loop = false;
            weaponAnimations[Character.Weapon.None].Scale = .13f;

            // CRITICAL DICTIONARY
            criticalAnimations = new Dictionary<Character.Weapon, SpineAnimation>();

            criticalAnimations.Add(Character.Weapon.Axe, AnimationAssets.AxeCritAttack);
            criticalAnimations[Character.Weapon.Axe].loop = false;
            criticalAnimations[Character.Weapon.Axe].Scale = WEAPON_SCALE;

            criticalAnimations.Add(Character.Weapon.Sword, AnimationAssets.SwordCritAttack);
            criticalAnimations[Character.Weapon.Sword].loop = false;
            criticalAnimations[Character.Weapon.Sword].Scale = WEAPON_SCALE;

            criticalAnimations.Add(Character.Weapon.Shield, AnimationAssets.ShieldCritAttack);
            criticalAnimations[Character.Weapon.Shield].loop = false;
            criticalAnimations[Character.Weapon.Shield].Scale = WEAPON_SCALE;

            // SPECIAL DICTIONARY
            specialAnimations = new Dictionary<Character.Special, SpineAnimation>();

            specialAnimations.Add(Character.Special.Bomb, AnimationAssets.AxeNormalAttack);
            specialAnimations[Character.Special.Bomb].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Bomb].loop = false;

            specialAnimations.Add(Character.Special.Healer, AnimationAssets.AxeNormalAttack);
            specialAnimations[Character.Special.Healer].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Healer].loop = false;

            specialAnimations.Add(Character.Special.Minor, AnimationAssets.AxeNormalAttack);
            specialAnimations[Character.Special.Minor].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Minor].loop = false;

            specialAnimations.Add(Character.Special.Spy, AnimationAssets.AxeNormalAttack);
            specialAnimations[Character.Special.Spy].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Spy].loop = false;

            // HIT MOMENTS
            hitMoments = new Dictionary<Character.Weapon, float>();
            hitMoments.Add(Character.Weapon.Axe, AXE_HIT_TIME);
            hitMoments.Add(Character.Weapon.Sword, SWORD_HIT_TIME);
            hitMoments.Add(Character.Weapon.Shield, SHIELD_HIT_TIME);
            hitMoments.Add(Character.Weapon.None, AXE_HIT_TIME);

            // DAMAGE SPRITE
            damageSprite = ArtAssets.DamageTextSprite;
            damageSpriteAlpha = 0f;
            damageSprite.colorEffect = Color.White * damageSpriteAlpha;

            // SETTING VALUES
            show = true;
            flickerTimer = 0;
            flickerAmount = FLICKER_LIMIT;
            timer = 0;

            // UI
            background = ArtAssets.FightPopUp; background.position = Main.WindowCenter;
            vsText = ArtAssets.VSText; vsText.position = Main.WindowCenter;

            axeIcon = ArtAssets.AxeIcon; axeIcon.CurrentFrame = 2;
            swordIcon = ArtAssets.SwordIcon; swordIcon.CurrentFrame = 2;
            shieldIcon = ArtAssets.ShieldIcon; shieldIcon.CurrentFrame = 2;
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

            Attack();

            character.animation.Position = MyAnimationPosition;
            enemyCharacter.animation.Position = EnemyAnimationPosition;

            character.animation.Scale = ANIMATION_SCALE;
            enemyCharacter.animation.Scale = ANIMATION_SCALE;

            myHealthBar = new Healthbar(ArtAssets.Healthbar, character.maxHP, character.hp, MyHealthbarPosition);
            enemyHealthbar = new Healthbar(ArtAssets.Healthbar, enemyCharacter.maxHP, enemyCharacter.hp, EnemyHealthbarPosition);
        }

        public override void Update(GameTime gameTime)
        {
            // TIMER FOR NEXT ATTACK
            if (!currentAnimation.IsPlayingAnimation)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // Damage
                damageSpriteAlpha = Math.Max(0, damageSpriteAlpha - FADE_OUT_SPEED);
            }

            // ATTACKING
            if (timer >= ATTACK_INTERVAL)
            {
                timer = 0;

                if (!character.IsDead && !enemyCharacter.IsDead)
                {
                    myAttackTurn = !myAttackTurn;
                    Attack();
                }
                else
                    EndFight();
            }

            // UPDATE WEAPON ANIMATION
            currentAnimation.Update(gameTime);

            // HIT MOMENT
            if (currentAnimation.Time >= hitInterval && currentAnimation.IsPlayingAnimation && !isFlickering)
            {
                // Set flickering values
                isFlickering = true;
                flickerAmount = 0;
                show = false;

                // Lose HP
                if (myAttackTurn) enemyCharacter.hp -= Math.Min(damage, enemyCharacter.hp);
                else character.hp -= Math.Min(damage, character.hp);

                // Set visual damage
                damageSprite.position = myAttackTurn ? MyDamagePosition : EnemyDamagePosition;
                damageSpriteAlpha = 1f;
                damageSprite.CurrentFrame = Math.Min(damage, 3);
            }

            damageSprite.position.Y -= DAMAGE_SPEED;

            // FLICKER EFFECT
            if (flickerAmount < FLICKER_LIMIT && isFlickering)
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

            // UPDATE CHARACTER ANIMATION 
            character.UpdateSpineAnimation(gameTime);
            enemyCharacter.UpdateSpineAnimation(gameTime);

            // UPDATE HEALTH
            myHealthBar.hp = character.hp;
            enemyHealthbar.hp = enemyCharacter.hp;
        }

        private void EndFight()
        {

            StateManager.RemoveState();
        }

        private void Attack()
        {
            if (myAttackTurn)
                DealDamage(character, enemyCharacter);
            else
                DealDamage(enemyCharacter, character);

            currentAnimation.Play();
            currentAnimation.FlipX = myAttackTurn;
            currentAnimation.Position = myAttackTurn ? MyAttackPosition : EnemyAttackPosition;

            isFlickering = false;

            if ((myAttackTurn ? character.special : enemyCharacter.special) == Character.Special.None)
                hitInterval = myAttackTurn ? hitMoments[character.weapon] : hitMoments[enemyCharacter.weapon];
            else
            {
                hitInterval = .5f;
            }
        }

        private void DealDamage(Character attacker, Character defender)
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
                        damage = Math.Max(3, defender.hp);
                        currentAnimation = specialAnimations[attacker.special];
                    }
                    else { 
                        damage = 1;
                        currentAnimation = weaponAnimations[attacker.weapon];
                    }
                    break;
                case Character.Special.Spy:
                    if (defender.rank == Character.Rank.Marshal) {
                        damage = Math.Max(3, defender.hp);
                        currentAnimation = specialAnimations[attacker.special];
                    }
                    else {
                        damage = 1;
                        currentAnimation = weaponAnimations[attacker.weapon];
                    }
                    break;
                case Character.Special.Bomb:
                    damage = Math.Max(3, defender.hp);
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
            // Background
            StateManager.GetState(2).Draw(spriteBatch);
            spriteBatch.DrawRectangle(Main.WindowRectangle, Color.Black * .5f);

            // UI
            background.Draw(spriteBatch); 
            vsText.Draw(spriteBatch);

            myHealthBar.Draw(spriteBatch);
            enemyHealthbar.Draw(spriteBatch);

            switch (character.weapon)
            {
                case Character.Weapon.Axe: spriteBatch.DrawSprite(axeIcon, MyWeaponPosition); break;
                case Character.Weapon.Sword: spriteBatch.DrawSprite(swordIcon, MyWeaponPosition); break;
                case Character.Weapon.Shield: spriteBatch.DrawSprite(shieldIcon, MyWeaponPosition); break;
            }

            switch (enemyCharacter.weapon)
            {
                case Character.Weapon.Axe: spriteBatch.DrawSprite(axeIcon, EnemyWeaponPosition); break;
                case Character.Weapon.Sword: spriteBatch.DrawSprite(swordIcon, EnemyWeaponPosition); break;
                case Character.Weapon.Shield: spriteBatch.DrawSprite(shieldIcon, EnemyWeaponPosition); break;
            }
        }

        public override void DrawAnimation(Spine.SkeletonMeshRenderer skeletonRenderer)
        {
            if (show || myAttackTurn)
                character.DrawAnimation(skeletonRenderer);

            if (show || !myAttackTurn)
                enemyCharacter.DrawAnimation(skeletonRenderer);

            if (currentAnimation.IsPlayingAnimation)
                currentAnimation.Draw(skeletonRenderer);
        }

        public override void DrawForeground(SpriteBatch spriteBatch)
        {
            damageSprite.colorEffect = Color.White * damageSpriteAlpha;
            damageSprite.Draw(spriteBatch);
        }
    }
}
