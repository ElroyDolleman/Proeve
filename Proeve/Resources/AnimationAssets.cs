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

        private const string SHINE_EFFECT = "shine";

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
        }
    }
}
