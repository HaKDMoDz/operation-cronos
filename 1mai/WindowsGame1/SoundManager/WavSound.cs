using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Operation_Cronos;

namespace Operation_Cronos.Sound
{
    public class WavSound : GameComponent
    {
        #region Fields
        private ContentManager content;
        private SoundEffect sound;
        private SoundEffectInstance playedSound;

        private bool isStopped = true;
        bool isPaused = false;
        bool isTurnedOn = true;
        private Sounds soundType = Sounds.None;
        bool loopSound = false;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the volume for the sound
        /// </summary>
        public float Volume
        {
            get
            {
                return playedSound.Volume;
            }
            set
            {
                playedSound.Volume = value;
            }
        }

        /// <summary>
        /// Gets whether the sound is stopped or not
        /// </summary>
        public bool IsStopped
        {
            get
            {
                return isStopped;
            }
        }

        /// <summary>
        /// Gets whether the sound is paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
        }

        /// <summary>
        /// Gets or sets the Turned on state of the sound
        /// </summary>
        public bool IsTurnedOn
        {
            get
            {
                return isTurnedOn;
            }
            set
            {
                isTurnedOn = value;
                if (isTurnedOn == false)
                    playedSound.Stop();
            }
        }

        /// <summary>
        /// Gets or sets the type of the sound, based on the Sounds enumeration
        /// </summary>
        public Sounds SoundType
        {
            get
            {
                return soundType;
            }
            set
            {
                soundType = value;
            }
        }

        /// <summary>
        /// Gets whether the sound should loop or not
        /// </summary>
        public bool LoopSound
        {
            get
            {
                return loopSound;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new SoundEffectInstance object
        /// </summary>
        public WavSound(Game game, string soundName, bool loopSound)
            : base(game)
        {
            content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory + "\\Sounds");
            sound = content.Load<SoundEffect>(soundName);
            playedSound = sound.CreateInstance();
            playedSound.Volume = 0.0f;
            playedSound.IsLooped = loopSound;

            game.Components.Add(this);
        }

        /// <summary>
        /// Starts playing the sound 
        /// 
        public void Play()
        {
            if (isTurnedOn)
            {
                isPaused = false;
                isStopped = false;
                playedSound.Play();
            }
        }

        /// <summary>
        /// Pauses the song
        /// </summary>
        public void Pause()
        {
            if (isTurnedOn)
            {
                if (!isPaused && !isStopped)
                {
                    playedSound.Pause();
                    isStopped = false;
                    isPaused = true;
                }
            }
        }

        /// <summary>
        /// Resumes the paused song
        /// </summary>
        public void Resume()
        {
            if (isTurnedOn)
            {
                if (isPaused)
                {
                    playedSound.Resume();
                    isPaused = false;
                    isStopped = false;
                }
            }
        }

        /// <summary>
        /// Stops the currently playing sound
        /// </summary>
        public void StopSound()
        {
            if (isTurnedOn)
            {
                playedSound.Stop();
                isPaused = false;
                isStopped = true;
            }
        }
    }
}
