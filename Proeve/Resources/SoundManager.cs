using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Proeve.Resources
{
    class SoundManager
    {
        private float volume;
        public float Volume
        {
            get { return volume; }
            set { volume = MathHelper.Clamp(value, 0f, 1f); }
        }

        public SoundManager()
        {
            volume = 1f;
        }

        public void PlaySound(SoundEffect sound, float soundVolume = 1f)
        {
            soundVolume = MathHelper.Clamp(soundVolume, 0f, 1f) * volume;
            sound.Play(soundVolume, 0, 0);
        }

        public void PlayMusic(Song song)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(song);
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
