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

        private const float END_TIME_HIT_ANIMATION = .367f;

        // Readonly positions
        private Vector2 MyAnimationPosition { get { return new Vector2(310, 320); } }
        private Vector2 EnemyAnimationPosition { get { return new Vector2(Main.WindowWidth - 310, 320); } }

        private Vector2 MyAttackPosition { get { return new Vector2(Main.WindowCenter.X + 40, Main.WindowCenter.Y); } }
        private Vector2 EnemyAttackPosition { get { return new Vector2(Main.WindowCenter.X - 40, Main.WindowCenter.Y); } }

        private Vector2 MySpySpecialPosition { get { return new Vector2(Main.WindowCenter.X + 160, Main.WindowCenter.Y - 70); } }
        private Vector2 EnemySpySpecialPosition { get { return new Vector2(Main.WindowCenter.X - 160, Main.WindowCenter.Y - 70); } }

        private Vector2 MyMinerSpecialPosition { get { return new Vector2(Main.WindowCenter.X + 140, Main.WindowCenter.Y + 70); } }
        private Vector2 EnemyMinerSpecialPosition { get { return new Vector2(Main.WindowCenter.X - 140, Main.WindowCenter.Y + 70); } }

        private Vector2 MyHealthbarPosition { get { return new Vector2(300, 520); } }
        private Vector2 EnemyHealthbarPosition { get { return new Vector2(620, 520); } }

        private Vector2 MyDamagePosition { get { return new Vector2(Main.WindowWidth - 280, 380); } }
        private Vector2 EnemyDamagePosition { get { return new Vector2(280, 380); } }

        private Vector2 MyHitEffectPosition { get { return new Vector2(Main.WindowWidth - 280, 340); } }
        private Vector2 EnemyHitEffectPosition { get { return new Vector2(280, 340); } }

        private Vector2 MyWeaponPosition { get { return new Vector2(220, 510); } }
        private Vector2 EnemyWeaponPosition { get { return new Vector2(Main.WindowWidth - 220, 510); } }

        private Vector2 MyRankNamePosition { get { return new Vector2(300, 490); } }
        private Vector2 EnemyRankNamePosition { get { return new Vector2(620, 490); } }

        // Characters
        private Character character;
        private Character enemyCharacter;

        // Fighting
        private bool myAttackTurn;
        private int damage;
        private float damageSpriteAlpha;
        private bool isHealing;

        private Sprite damageSprite;
        private Sprite healSprite;

        private const int DAMAGE_SPEED = 3;
        private const float FADE_OUT_SPEED = .2f;

        private SpineAnimation hitEffect;

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

        private Dictionary<Character.Special, int> specialRankNameFrames;
        private Dictionary<Character.Rank, int> rankNameFrames;
        private Dictionary<Character.Rank, int> rewards;

        private Sprite myRankName, enemyRankName;
        private Sprite axeIcon, swordIcon, shieldIcon;

        public FightState()
        {
            
        }

        public override void Initialize()
        {
            #region SET DICTIONARIES
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

            specialAnimations.Add(Character.Special.Healer, AnimationAssets.HealSpecial);
            specialAnimations[Character.Special.Healer].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Healer].loop = false;

            specialAnimations.Add(Character.Special.Miner, AnimationAssets.MinerSpecial);
            specialAnimations[Character.Special.Miner].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Miner].loop = false;

            specialAnimations.Add(Character.Special.Spy, AnimationAssets.SpySpecial);
            specialAnimations[Character.Special.Spy].Position = Main.WindowCenter;
            specialAnimations[Character.Special.Spy].loop = false;

            // RANK NAME FRAMES
            rankNameFrames = new Dictionary<Character.Rank, int>();
            rankNameFrames.Add(Character.Rank.Leader, 4);
            rankNameFrames.Add(Character.Rank.General, 5);
            rankNameFrames.Add(Character.Rank.Captain, 6);
            rankNameFrames.Add(Character.Rank.Soldier, 7);
            rankNameFrames.Add(Character.Rank.Miner, 1);

            specialRankNameFrames = new Dictionary<Character.Special, int>();
            specialRankNameFrames.Add(Character.Special.Healer, 1);
            specialRankNameFrames.Add(Character.Special.Miner, 2);
            specialRankNameFrames.Add(Character.Special.Spy, 3);
            specialRankNameFrames.Add(Character.Special.Bomb, 8);

            // HIT MOMENTS
            hitMoments = new Dictionary<Character.Weapon, float>();
            hitMoments.Add(Character.Weapon.Axe, AXE_HIT_TIME);
            hitMoments.Add(Character.Weapon.Sword, SWORD_HIT_TIME);
            hitMoments.Add(Character.Weapon.Shield, SHIELD_HIT_TIME);
            hitMoments.Add(Character.Weapon.None, AXE_HIT_TIME);

            // REWARDS
            rewards = new Dictionary<Character.Rank, int>();
            rewards.Add(Character.Rank.Soldier, 10);
            rewards.Add(Character.Rank.Captain, 20);
            rewards.Add(Character.Rank.General, 30);
            rewards.Add(Character.Rank.Bomb, 30);
            rewards.Add(Character.Rank.Miner, 30);
            rewards.Add(Character.Rank.Spy, 30);
            rewards.Add(Character.Rank.Healer, 30);
            rewards.Add(Character.Rank.Leader, 50);
            #endregion

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

            myRankName = ArtAssets.RankNamesBold;
            myRankName.position = MyRankNamePosition;

            enemyRankName = ArtAssets.RankNamesBold;
            enemyRankName.position = EnemyRankNamePosition;

            // HIT EFFECT
            hitEffect = AnimationAssets.HitEffect;
            hitEffect.loop = false;
            hitEffect.Position = -Vector2.One * 100;
        }

        public void SetUnits(Character attacker, Character defender)
        {
            if (Globals.multiplayerConnection.myTurn) {
                this.character = attacker;
                this.enemyCharacter = defender;

                myAttackTurn = !(enemyCharacter.special == Character.Special.Bomb && character.special != Character.Special.Miner);
            }
            else {
                this.character = defender;
                this.enemyCharacter = attacker;

                myAttackTurn = (character.special == Character.Special.Bomb && enemyCharacter.special != Character.Special.Miner);
            }

            isHealing = attacker.special == Character.Special.Healer;

            Attack();

            character.animation.Position = MyAnimationPosition;
            enemyCharacter.animation.Position = EnemyAnimationPosition;

            character.animation.Scale = ANIMATION_SCALE;
            enemyCharacter.animation.Scale = ANIMATION_SCALE;

            myHealthBar = new Healthbar(ArtAssets.Healthbar, character.maxHP, character.hp, MyHealthbarPosition);
            enemyHealthbar = new Healthbar(ArtAssets.Healthbar, enemyCharacter.maxHP, enemyCharacter.hp, EnemyHealthbarPosition);

            myRankName.CurrentFrame = character.special == Character.Special.None ? rankNameFrames[character.rank] : specialRankNameFrames[character.special];
            enemyRankName.CurrentFrame = enemyCharacter.special == Character.Special.None ? rankNameFrames[enemyCharacter.rank] : specialRankNameFrames[enemyCharacter.special];
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

                if (!character.IsDead && !enemyCharacter.IsDead && !isHealing)
                {
                    myAttackTurn = !myAttackTurn;
                    Attack();
                }
                else
                    EndFight();
            }

            // UPDATE WEAPON ANIMATION
            currentAnimation.Update(gameTime);
            hitEffect.Update(gameTime);

            // HIT MOMENT
            if (currentAnimation.Time >= hitInterval && currentAnimation.IsPlayingAnimation && !isFlickering)
            {
                // Lose HP
                if (myAttackTurn) enemyCharacter.hp -= MathHelper.Clamp(damage, enemyCharacter.hp - enemyCharacter.maxHP, enemyCharacter.hp);
                else character.hp -= MathHelper.Clamp(damage, character.hp - character.maxHP, character.hp);

                if (currentAnimation != specialAnimations[Character.Special.Healer])
                {
                    // Set flickering values
                    isFlickering = true;
                    flickerAmount = 0;
                    show = false;

                    // Set visual damage
                    damageSprite.position = myAttackTurn ? MyDamagePosition : EnemyDamagePosition;
                    damageSpriteAlpha = 1f;
                    damageSprite.CurrentFrame = Math.Min(damage, 3);

                    //hitEffect = currentAnimation != specialAnimations[Character.Special.Spy] ? AnimationAssets.HitEffect : AnimationAssets.SpySpecialHitEffect;
                    hitEffect.Position = myAttackTurn ? MyHitEffectPosition : EnemyHitEffectPosition;
                    hitEffect.Play();
                    hitEffect.FlipX = !myAttackTurn;
                }
                else
                {
                    // Set visual heal
                    damageSprite.position = myAttackTurn ? MyDamagePosition : EnemyDamagePosition;
                    damageSpriteAlpha = 1f;
                    damageSprite.CurrentFrame = Math.Min(-damage+3, 5);

                    isFlickering = true;
                }
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
            if (enemyCharacter.IsDead)
                Globals.earnedDiamonds += rewards[enemyCharacter.rank];

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

            if (currentAnimation == specialAnimations[Character.Special.Spy])
            {
                currentAnimation.FlipX = !currentAnimation.FlipX;
                currentAnimation.Position = myAttackTurn ? MySpySpecialPosition : EnemySpySpecialPosition;
            }
            else if (currentAnimation == specialAnimations[Character.Special.Miner])
            {
                currentAnimation.FlipX = !currentAnimation.FlipX;
                currentAnimation.Position = myAttackTurn ? MyMinerSpecialPosition : EnemyMinerSpecialPosition;
            }

            isFlickering = false;

            if ((myAttackTurn ? character.special : enemyCharacter.special) == Character.Special.None)
                hitInterval = myAttackTurn ? hitMoments[character.weapon] : hitMoments[enemyCharacter.weapon];
            else if ((myAttackTurn ? character.special : enemyCharacter.special) == Character.Special.Healer)
                hitInterval = 1.4f;
            else
                hitInterval = .5f;
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
                case Character.Special.Miner:
                    if (defender.special == Character.Special.Bomb)
                        damage = Math.Max(3, defender.hp);
                    else 
                        damage = 1;

                    currentAnimation = specialAnimations[attacker.special];
                    break;
                case Character.Special.Spy:
                    if (defender.rank == Character.Rank.Leader)
                        damage = Math.Max(3, defender.hp);
                    else
                        damage = 1;

                    currentAnimation = specialAnimations[attacker.special];
                    break;
                case Character.Special.Bomb:
                    damage = Math.Max(3, defender.hp);
                    currentAnimation = specialAnimations[attacker.special];
                    break;
                case Character.Special.Healer:
                    if (isHealing) {
                        damage = Math.Max(-2, -(defender.maxHP - defender.hp));
                        currentAnimation = specialAnimations[attacker.special];
                    }
                    else {
                        damage = 0;
                        currentAnimation = specialAnimations[attacker.special];
                    }
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
            if (!isHealing) vsText.Draw(spriteBatch);

            myHealthBar.Draw(spriteBatch);
            enemyHealthbar.Draw(spriteBatch);

            myRankName.Draw(spriteBatch);
            enemyRankName.Draw(spriteBatch);

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
            bool spyAnimationIsPlaying = currentAnimation == specialAnimations[Character.Special.Spy];

            if (currentAnimation.IsPlayingAnimation && spyAnimationIsPlaying)
                currentAnimation.Draw(skeletonRenderer);

            if (show || myAttackTurn)
                character.DrawAnimation(skeletonRenderer);

            if (show || !myAttackTurn)
                enemyCharacter.DrawAnimation(skeletonRenderer);

            if (hitEffect.Time < END_TIME_HIT_ANIMATION)
                hitEffect.Draw(skeletonRenderer);

            if (currentAnimation.IsPlayingAnimation && !spyAnimationIsPlaying)
                currentAnimation.Draw(skeletonRenderer);
        }

        public override void DrawForeground(SpriteBatch spriteBatch)
        {
            damageSprite.colorEffect = Color.White * damageSpriteAlpha;
            damageSprite.Draw(spriteBatch);
        }
    }
}
