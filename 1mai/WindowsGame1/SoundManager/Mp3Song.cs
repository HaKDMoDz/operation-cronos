using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Operation_Cronos;

namespace Operation_Cronos.Sound
{
    public class Mp3Song : GameComponent
    {
        #region Fields
        private ContentManager content;
        private Song song;

        private bool isStopped = false;
        bool isPaused = false;

        #endregion

        /// <summary>
        /// Gets or sets the volume to play songs. 1.0f is max volume.
        /// </summary>
        public float Volume
        {
            get
            {
                return MediaPlayer.Volume;
            }
            set
            {
                MediaPlayer.Volume = value;
            }
        }

        /// <summary>
        /// Gets whether the song is stopped or not
        /// </summary>
        public bool IsStopped
        {
            get
            {
                // return (MediaPlayer.State == MediaState.Stopped);
                return isStopped;
            }
        }

        /// <summary>
        /// Gets whether the song is paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
        }

        /// <summary>
        /// Creates a new Song object
        /// </summary>
        public Mp3Song(Game game, string songName)
            : base(game)
        {
            content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory + "\\Sounds");
            song = content.Load<Song>(songName);
            MediaPlayer.Volume = 0.0f;

            game.Components.Add(this);
        }

        /// <summary>
        /// Starts playing the song. 
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void Play(bool loop)
        {
            isPaused = false;
            isStopped = false;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(song);
        }

        /// <summary>
        /// Pauses the song
        /// </summary>
        public void Pause()
        {
            MediaPlayer.Pause();
            isStopped = false;
            isPaused = true;
        }

        /// <summary>
        /// Resumes the paused song
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
            isPaused = false;
            isStopped = false;
        }

        /// <summary>
        /// Stops the currently playing song
        /// </summary>
        public void StopSong()
        {
            MediaPlayer.Stop();
            isPaused = false;
            isStopped = true;
        }

    }
}
