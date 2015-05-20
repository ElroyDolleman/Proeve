using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private const string BACKGROUND_PATH = "Backgrounds\\";
        private const string WEAPON_ANIMATION_PATH = "Weapons\\Animations\\";

        #endregion
        #region FILES

        /* MEDIEVAL ARMY */
        private const string MEDIEVAL_MARSHAL = "kingAnimation";
        private const string MEDIEVAL_GENERAL = "dragonAnimation";
        private const string MEDIEVAL_MAJOOR = "knightAnimation";
        private const string MEDIEVAL_CAPTAIN = "soldierAnimation";
        private const string MEDIEVAL_MINOR = "narAnimation";
        private const string MEDIEVAL_HEALER = "wizardAnimation";
        private const string MEDIEVAL_SPY = "spyAnimation";
        private const string MEDIEVAL_BOMB = "bombAnimation";

        /* OTHER */
        private const string SHINE_EFFECT = "shine";

        /* WEAPONS */
        private const string WEAPON_AXE = "axeAnimation";
        private const string WEAPON_SWORD = "swordAnimation";
        private const string WEAPON_SHIELD = "shieldAnimation";

        #endregion
        #region ANIMATIONS

        /* MEDIEVAL ARMY */
        private static SpineAnimation medievalMarshal;
        private static SpineAnimation medievalGeneral;
        private static SpineAnimation medievalMajoor;
        private static SpineAnimation medievalCaptain;
        private static SpineAnimation medievalMinor;
        private static SpineAnimation medievalHealer;
        private static SpineAnimation medievalSpy;
        private static SpineAnimation medievalBomb;

        /* WEAPONS */
        private static SpineAnimation axeNormalAttack;
        private static SpineAnimation swordNormalAttack;
        private static SpineAnimation shieldNormalAttack;

        private static SpineAnimation axeCritAttack;
        private static SpineAnimation swordCritAttack;
        private static SpineAnimation shieldCritAttack;

        /* OTHER */
        private static SpineAnimation shineEffect;

        #endregion
        #region READ ONLY

        /* MEDIEVAL ARMY */
        public static SpineAnimation MedievalMarshal { get { return (SpineAnimation)medievalMarshal.Clone(); } }
        public static SpineAnimation MedievalGeneral { get { return (SpineAnimation)medievalGeneral.Clone(); } }
        public static SpineAnimation MedievalMajoor { get { return (SpineAnimation)medievalMajoor.Clone(); } }
        public static SpineAnimation MedievalCaptain { get { return (SpineAnimation)medievalCaptain.Clone(); } }
        public static SpineAnimation MedievalMinor { get { return (SpineAnimation)medievalMinor.Clone(); } }
        public static SpineAnimation MedievalHealer { get { return (SpineAnimation)medievalHealer.Clone(); } }
        public static SpineAnimation MedievalSpy { get { return (SpineAnimation)medievalSpy.Clone(); } }
        public static SpineAnimation MedievalBomb { get { return (SpineAnimation)medievalBomb.Clone(); } }

        /* WEAPONS */
        private static SpineAnimation AxeNormalAttack { get { return (SpineAnimation)axeNormalAttack.Clone(); } }
        private static SpineAnimation SwordNormalAttack { get { return (SpineAnimation)swordNormalAttack.Clone(); } }
        private static SpineAnimation ShieldNormalAttack { get { return (SpineAnimation)shieldNormalAttack.Clone(); } }

        private static SpineAnimation AxeCritAttack { get { return (SpineAnimation)axeCritAttack.Clone(); } }
        private static SpineAnimation SwordCritAttack { get { return (SpineAnimation)swordCritAttack.Clone(); } }
        private static SpineAnimation ShieldCritAttack { get { return (SpineAnimation)shieldCritAttack.Clone(); } }

        /* OTHER */
        public static SpineAnimation ShineEffect { get { return (SpineAnimation)shineEffect.Clone(); } }

        #endregion

        public static void LoadAnimations(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            // MEDIEVAL MARSHAL
            medievalMarshal = new SpineAnimation();
            medievalMarshal.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_MARSHAL);

            // MEDIEVAL GENERAL
            medievalGeneral = new SpineAnimation();
            medievalGeneral.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_GENERAL, "Idle");

            // MEDIEVAL MAJOOR
            medievalMajoor = new SpineAnimation();
            medievalMajoor.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_MAJOOR);

            // MEDIEVAL CAPTAIN
            medievalCaptain = new SpineAnimation();
            medievalCaptain.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_CAPTAIN);

            // MEDIEVAL MINOR
            medievalMinor = new SpineAnimation();
            medievalMinor.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_MINOR);

            // MEDIEVAL HEALER
            medievalHealer = new SpineAnimation();
            medievalHealer.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_HEALER);

            // MEDIEVAL SPY
            medievalSpy = new SpineAnimation();
            medievalSpy.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_SPY);

            // MEDIEVAL BOMB
            medievalBomb = new SpineAnimation();
            medievalBomb.LoadAnimation(graphicsDevice, contentManager, MEDIEVAL_ARMY_PATH, MEDIEVAL_BOMB);

            // SHINE EFFECT
            shineEffect = new SpineAnimation();
            shineEffect.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, BACKGROUND_PATH, SHINE_EFFECT, "animation");

            // WEAPON AXE
            axeNormalAttack = new SpineAnimation();
            axeNormalAttack.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, WEAPON_ANIMATION_PATH, WEAPON_AXE, "AxeNormal");
            axeCritAttack = new SpineAnimation();
            axeCritAttack.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, WEAPON_ANIMATION_PATH, WEAPON_AXE, "AxeCrit");

            // WEAPON SWORD
            swordNormalAttack = new SpineAnimation();
            swordNormalAttack.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, WEAPON_ANIMATION_PATH, WEAPON_SWORD, "SwordNormal");
            swordCritAttack = new SpineAnimation();
            swordCritAttack.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, WEAPON_ANIMATION_PATH, WEAPON_SWORD, "SwordCrit");

            // WEAPON SHIELD
            shieldNormalAttack = new SpineAnimation();
            shieldNormalAttack.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, WEAPON_ANIMATION_PATH, WEAPON_SHIELD, "ShieldNormal");
            shieldCritAttack = new SpineAnimation();
            shieldCritAttack.LoadAnimation(Globals.graphicsDevice, Globals.contentManager, WEAPON_ANIMATION_PATH, WEAPON_SHIELD, "ShieldCrit");
        }
    }
}
