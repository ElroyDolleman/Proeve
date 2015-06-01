using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Proeve.Entities;

namespace Proeve.Resources
{
    class AnimationAssets
    {
        #region FILEPATHS
        private const string ANIMATION_PATH = "Characters\\Animations\\";
        private const string MEDIEVAL_ARMY_PATH = ANIMATION_PATH + "MedievalArmy\\";
        private const string TIKI_ARMY_PATH = ANIMATION_PATH + "TikiArmy\\";
        private const string SEA_ARMY_PATH = ANIMATION_PATH + "SeaArmy\\";

        private const string BACKGROUND_PATH = "Backgrounds\\";
        private const string WEAPON_ANIMATION_PATH = "Weapons\\Animations\\";
        private const string UI_ANIMATION_PATH = "UI\\Animations\\";

        #endregion
        #region FILES

        /* MEDIEVAL ARMY */
        private const string MEDIEVAL_LEADER = "kingAnimation";
        private const string MEDIEVAL_GENERAL = "dragonAnimation";
        private const string MEDIEVAL_CAPTAIN = "knightAnimation";
        private const string MEDIEVAL_SOLDIER = "soldierAnimation";
        private const string MEDIEVAL_MINER = "narAnimation";
        private const string MEDIEVAL_HEALER = "wizardAnimation";
        private const string MEDIEVAL_SPY = "spyAnimation";
        private const string MEDIEVAL_BOMB = "bombAnimation";

        /* TIKI ARMY */
        private const string TIKI_LEADER = "tikiLeader";
        private const string TIKI_GENERAL = "tikiGeneral";
        private const string TIKI_CAPTAIN = "tikiCaptain";
        private const string TIKI_SOLDIER = "tikiSoldier";
        private const string TIKI_MINER = "tikiMiner";
        private const string TIKI_HEALER = "tikiHealer";
        private const string TIKI_SPY = "tikiSpy";
        private const string TIKI_BOMB = "tikiBomb";

        /* SEA ARMY */
        private const string SEA_LEADER = "seaLeader";
        private const string SEA_GENERAL = "seaGeneral";
        private const string SEA_CAPTAIN = "seaCaptain";
        private const string SEA_SOLDIER = "seaSoldier";
        private const string SEA_MINER = "seaMiner";
        private const string SEA_HEALER = "seaHealer";
        private const string SEA_SPY = "seaSpy";
        private const string SEA_BOMB = "seaBomb";

        /* OTHER */
        private const string SHINE_EFFECT = "shine";
        private const string MOVEMENT_ATTACK_ICON = "Movement";
        private const string HEAL_ICON = "healIcon";

        /* WEAPONS */
        private const string WEAPON_AXE = "axeAnimation";
        private const string WEAPON_SWORD = "swordAnimation";
        private const string WEAPON_SHIELD = "shieldAnimation";

        private const string HIT_EFFECT = "hitEffect";
        private const string SPY_SPECIAL = "spySpecial";
        private const string HEAL_SPECIAL = "healAnimation";
        private const string MINER_SPECIAL = "minerSpecial";

        #endregion
        #region ANIMATION NAMES
        /* WEAPONS */
        private const string AXE_NORMAL_ANIMATION = "AxeNormal";
        private const string AXE_CRIT_ANIMATION = "AxeCrit";
        private const string SWORD_NORMAL_ANIMATION = "SwordNormal";
        private const string SWORD_CRIT_ANIMATION = "SwordCrit";
        private const string SHIELD_NORMAL_ANIMATION = "ShieldNormal";
        private const string SHIELD_CRIT_ANIMATION = "ShieldCrit";

        private const string MOVEMENT_ICON_ANIMATION = "Arrow";
        private const string ATTACK_ICON_ANIMATION = "Attack";

        #endregion
        #region ANIMATIONS

        /* MEDIEVAL ARMY */
        private static SpineAnimation medievalLeader;
        private static SpineAnimation medievalGeneral;
        private static SpineAnimation medievalCaptain;
        private static SpineAnimation medievalSoldier;
        private static SpineAnimation medievalMiner;
        private static SpineAnimation medievalHealer;
        private static SpineAnimation medievalSpy;
        private static SpineAnimation medievalBomb;

        /* TIKI ARMY */
        private static SpineAnimation tikiLeader;
        private static SpineAnimation tikiGeneral;
        private static SpineAnimation tikiCaptain;
        private static SpineAnimation tikiSoldier;
        private static SpineAnimation tikiMiner;
        private static SpineAnimation tikiHealer;
        private static SpineAnimation tikiSpy;
        private static SpineAnimation tikiBomb;

        /* SEA ARMY */
        private static SpineAnimation seaLeader;
        private static SpineAnimation seaGeneral;
        private static SpineAnimation seaCaptain;
        private static SpineAnimation seaSoldier;
        private static SpineAnimation seaMiner;
        private static SpineAnimation seaHealer;
        private static SpineAnimation seaSpy;
        private static SpineAnimation seaBomb;

        /* WEAPONS */
        private static SpineAnimation axeNormalAttack;
        private static SpineAnimation swordNormalAttack;
        private static SpineAnimation shieldNormalAttack;

        private static SpineAnimation axeCritAttack;
        private static SpineAnimation swordCritAttack;
        private static SpineAnimation shieldCritAttack;

        private static SpineAnimation spySpecial;
        private static SpineAnimation spySpecialHitEffect;
        private static SpineAnimation healSpecial;
        private static SpineAnimation minerSpecial;

        private static SpineAnimation hitEffect;

        /* OTHER */
        private static SpineAnimation shineEffect;

        private static SpineAnimation arrowIcon;
        private static SpineAnimation attackIcon;
        private static SpineAnimation healIcon;

        #endregion
        #region READ ONLY

        /* MEDIEVAL ARMY */
        public static SpineAnimation MedievalLeader { get { return (SpineAnimation)medievalLeader.Clone(); } }
        public static SpineAnimation MedievalGeneral { get { return (SpineAnimation)medievalGeneral.Clone(); } }
        public static SpineAnimation MedievalCaptain { get { return (SpineAnimation)medievalCaptain.Clone(); } }
        public static SpineAnimation MedievalSoldier { get { return (SpineAnimation)medievalSoldier.Clone(); } }
        public static SpineAnimation MedievalMiner { get { return (SpineAnimation)medievalMiner.Clone(); } }
        public static SpineAnimation MedievalHealer { get { return (SpineAnimation)medievalHealer.Clone(); } }
        public static SpineAnimation MedievalSpy { get { return (SpineAnimation)medievalSpy.Clone(); } }
        public static SpineAnimation MedievalBomb { get { return (SpineAnimation)medievalBomb.Clone(); } }

        /* TIKI ARMY */
        public static SpineAnimation TikiLeader { get { return (SpineAnimation)tikiLeader.Clone(); } }
        public static SpineAnimation TikiGeneral { get { return (SpineAnimation)tikiGeneral.Clone(); } }
        public static SpineAnimation TikiCaptain { get { return (SpineAnimation)tikiCaptain.Clone(); } }
        public static SpineAnimation TikiSoldier { get { return (SpineAnimation)tikiSoldier.Clone(); } }
        public static SpineAnimation TikiMiner { get { return (SpineAnimation)tikiMiner.Clone(); } }
        public static SpineAnimation TikiHealer { get { return (SpineAnimation)tikiHealer.Clone(); } }
        public static SpineAnimation TikiSpy { get { return (SpineAnimation)tikiSpy.Clone(); } }
        public static SpineAnimation TikiBomb { get { return (SpineAnimation)tikiBomb.Clone(); } }

        /* SEA ARMY */
        public static SpineAnimation SeaLeader { get { return (SpineAnimation)seaLeader.Clone(); } }
        public static SpineAnimation SeaGeneral { get { return (SpineAnimation)seaGeneral.Clone(); } }
        public static SpineAnimation SeaCaptain { get { return (SpineAnimation)seaCaptain.Clone(); } }
        public static SpineAnimation SeaSoldier { get { return (SpineAnimation)seaSoldier.Clone(); } }
        public static SpineAnimation SeaMiner { get { return (SpineAnimation)seaMiner.Clone(); } }
        public static SpineAnimation SeaHealer { get { return (SpineAnimation)seaHealer.Clone(); } }
        public static SpineAnimation SeaSpy { get { return (SpineAnimation)seaSpy.Clone(); } }
        public static SpineAnimation SeaBomb { get { return (SpineAnimation)seaBomb.Clone(); } }

        /* WEAPONS */
        public static SpineAnimation AxeNormalAttack { get { return (SpineAnimation)axeNormalAttack.Clone(); } }
        public static SpineAnimation SwordNormalAttack { get { return (SpineAnimation)swordNormalAttack.Clone(); } }
        public static SpineAnimation ShieldNormalAttack { get { return (SpineAnimation)shieldNormalAttack.Clone(); } }

        public static SpineAnimation AxeCritAttack { get { return (SpineAnimation)axeCritAttack.Clone(); } }
        public static SpineAnimation SwordCritAttack { get { return (SpineAnimation)swordCritAttack.Clone(); } }
        public static SpineAnimation ShieldCritAttack { get { return (SpineAnimation)shieldCritAttack.Clone(); } }

        public static SpineAnimation SpySpecial { get { return (SpineAnimation)spySpecial.Clone(); } }
        public static SpineAnimation HealSpecial { get { return (SpineAnimation)healSpecial.Clone(); } }
        public static SpineAnimation SpySpecialHitEffect { get { return (SpineAnimation)spySpecialHitEffect.Clone(); } }
        public static SpineAnimation MinerSpecial { get { return (SpineAnimation)minerSpecial.Clone(); } }

        public static SpineAnimation HitEffect { get { return (SpineAnimation)hitEffect.Clone(); } }

        /* OTHER */
        public static SpineAnimation ShineEffect { get { return (SpineAnimation)shineEffect.Clone(); } }

        public static SpineAnimation ArrowIcon { get { return (SpineAnimation)arrowIcon.Clone(); } }
        public static SpineAnimation AttackIcon { get { return (SpineAnimation)attackIcon.Clone(); } }
        public static SpineAnimation HealIcon { get { return (SpineAnimation)healIcon.Clone(); } }

        #endregion

        public static void LoadAnimations(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            #region MEDIEVAL ARMY
            // MEDIEVAL Leader
            medievalLeader = new SpineAnimation();
            medievalLeader.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_LEADER);

            // MEDIEVAL GENERAL
            medievalGeneral = new SpineAnimation();
            medievalGeneral.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_GENERAL, "Idle");

            // MEDIEVAL Captain
            medievalCaptain = new SpineAnimation();
            medievalCaptain.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_CAPTAIN);

            // MEDIEVAL Soldier
            medievalSoldier = new SpineAnimation();
            medievalSoldier.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_SOLDIER);

            // MEDIEVAL MINOR
            medievalMiner = new SpineAnimation();
            medievalMiner.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_MINER);

            // MEDIEVAL HEALER
            medievalHealer = new SpineAnimation();
            medievalHealer.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_HEALER);
            medievalHealer.Offset = new Vector2(0, 2);

            // MEDIEVAL SPY
            medievalSpy = new SpineAnimation();
            medievalSpy.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_SPY);

            // MEDIEVAL BOMB
            medievalBomb = new SpineAnimation();
            medievalBomb.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_BOMB);
            #endregion
            #region TIKI ARMY
            // TIKI Leader
            tikiLeader = new SpineAnimation();
            tikiLeader.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_LEADER, "Idle");

            // TIKI GENERAL
            tikiGeneral = new SpineAnimation();
            tikiGeneral.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_GENERAL);
            tikiGeneral.Offset = new Vector2(0, 4);

            // TIKI Captain
            tikiCaptain = new SpineAnimation();
            tikiCaptain.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_CAPTAIN, "Idle");
            tikiCaptain.Offset = new Vector2(0, 10);

            // TIKI Soldier
            tikiSoldier = new SpineAnimation();
            tikiSoldier.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_SOLDIER);
            tikiSoldier.Offset = new Vector2(0, 8);

            // TIKI MINOR
            tikiMiner = new SpineAnimation();
            tikiMiner.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_MINER);

            // TIKI HEALER
            tikiHealer = new SpineAnimation();
            tikiHealer.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_HEALER);

            // TIKI SPY
            tikiSpy = new SpineAnimation();
            tikiSpy.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_SPY);

            // TIKI BOMB
            tikiBomb = new SpineAnimation();
            tikiBomb.LoadAnimation(graphicsDevice, contentManager, TIKI_ARMY_PATH, TIKI_BOMB);
            #endregion
            #region SEA ARMY
            // SEA Leader
            seaLeader = new SpineAnimation();
            seaLeader.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_LEADER);

            // SEA GENERAL
            seaGeneral = new SpineAnimation();
            seaGeneral.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_GENERAL);
            seaGeneral.Offset = new Vector2(0, 4);

            // SEA Captain
            seaCaptain = new SpineAnimation();
            seaCaptain.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_CAPTAIN);
            seaCaptain.Offset = new Vector2(0, 10);

            // SEA Soldier
            seaSoldier = new SpineAnimation();
            seaSoldier.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_SOLDIER);
            seaSoldier.Offset = new Vector2(0, 8);

            // SEA MINOR
            seaMiner = new SpineAnimation();
            seaMiner.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_MINER);

            // SEA HEALER
            seaHealer = new SpineAnimation();
            seaHealer.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_HEALER);

            // SEA SPY
            seaSpy = new SpineAnimation();
            seaSpy.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_SPY);

            // SEA BOMB
            seaBomb = new SpineAnimation();
            seaBomb.LoadAnimation(graphicsDevice, contentManager, SEA_ARMY_PATH, SEA_BOMB);
            #endregion

            // SHINE EFFECT
            shineEffect = new SpineAnimation();
            shineEffect.LoadAnimation(graphicsDevice, contentManager, BACKGROUND_PATH, SHINE_EFFECT);

            // WEAPON AXE
            axeNormalAttack = new SpineAnimation();
            axeNormalAttack.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, WEAPON_AXE, AXE_NORMAL_ANIMATION);
            axeCritAttack = new SpineAnimation();
            axeCritAttack.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, WEAPON_AXE, AXE_CRIT_ANIMATION);

            // WEAPON SWORD
            swordNormalAttack = new SpineAnimation();
            swordNormalAttack.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, WEAPON_SWORD, SWORD_NORMAL_ANIMATION);
            swordCritAttack = new SpineAnimation();
            swordCritAttack.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, WEAPON_SWORD, SWORD_CRIT_ANIMATION);

            // WEAPON SHIELD
            shieldNormalAttack = new SpineAnimation();
            shieldNormalAttack.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, WEAPON_SHIELD, SHIELD_NORMAL_ANIMATION);
            shieldCritAttack = new SpineAnimation();
            shieldCritAttack.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, WEAPON_SHIELD, SHIELD_CRIT_ANIMATION);

            // SPY SPECIAL
            spySpecial = new SpineAnimation();
            spySpecial.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, SPY_SPECIAL, "BackstabBack");
            spySpecialHitEffect = new SpineAnimation();
            spySpecialHitEffect.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, SPY_SPECIAL, "BackstabFront");

            // HEAL SPECIAL
            healSpecial = new SpineAnimation();
            healSpecial.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, HEAL_SPECIAL);

            // MINER SPECIAL
            minerSpecial = new SpineAnimation();
            minerSpecial.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, MINER_SPECIAL);

            // HIT 
            hitEffect = new SpineAnimation();
            hitEffect.LoadAnimation(graphicsDevice, contentManager, WEAPON_ANIMATION_PATH, HIT_EFFECT, "animation2");

            // UI
            arrowIcon = new SpineAnimation();
            arrowIcon.LoadAnimation(graphicsDevice, contentManager, UI_ANIMATION_PATH, MOVEMENT_ATTACK_ICON, MOVEMENT_ICON_ANIMATION);

            attackIcon = new SpineAnimation();
            attackIcon.LoadAnimation(graphicsDevice, contentManager, UI_ANIMATION_PATH, MOVEMENT_ATTACK_ICON, ATTACK_ICON_ANIMATION);

            healIcon = new SpineAnimation();
            healIcon.LoadAnimation(graphicsDevice, contentManager, UI_ANIMATION_PATH, HEAL_ICON, "Heal");
        }
    }
}
