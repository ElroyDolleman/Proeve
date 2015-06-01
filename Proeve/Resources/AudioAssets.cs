using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Proeve.Resources
{
    class AudioAssets
    {
        #region PATHS
        private const string AUDIO_PATH = "Audio\\";
        #endregion
        #region FILES
        private const string EXPLOSION = "Explosion_1";
        private const string DRAG = "Drag";
        private const string WEAPON_SWOOSH = "Weapon_Swoosh";
        private const string WEAPON_IMPACT = "Weapon_Impact";
        private const string WEAPON_HIT = "Weapon_Hit";
        #endregion
        #region SOUNDEFFECTS
        public static SoundEffect explosion;
        public static SoundEffect drag;
        public static SoundEffect weaponSwoosh;
        public static SoundEffect weaponImpact;
        public static SoundEffect weaponHit;
        #endregion

        public static void Load(ContentManager contentManager)
        {
            explosion = contentManager.Load<SoundEffect>(AUDIO_PATH + EXPLOSION);
            drag = contentManager.Load<SoundEffect>(AUDIO_PATH + DRAG);

            weaponSwoosh = contentManager.Load<SoundEffect>(AUDIO_PATH + WEAPON_SWOOSH);
            weaponImpact = contentManager.Load<SoundEffect>(AUDIO_PATH + WEAPON_IMPACT);
            weaponHit = contentManager.Load<SoundEffect>(AUDIO_PATH + WEAPON_HIT);
        }
    }
}
