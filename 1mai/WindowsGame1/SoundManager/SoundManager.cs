using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Operation_Cronos;

namespace Operation_Cronos.Sound
{
    public enum Sounds
    {
        MainMenuVents,
        MainMenuStairs,
        MainMenuTimeGateWater,
        MainMenuRoboticArms,
        MainMenuPiston,
        MainMenuCreditsPanelOpening,
        MainMenuCreditsPanelClosing,
        MainMenuSlot,
        CommandCenterButton,
        CommandCenterZone,
        BackgroundSound,
        SlidingSoundLong,
        SlidingSoundShort,
        LeftMenuButtons,
        EconomyFactory,
        EconomyMine,
        EconomyOilWell,
        EconomySawMill,
        Education,
        Energy,
        EnvironmentNursery,
        EnvironmentRecycling,
        EnvironmentWaterPurification,
        FoodAnimalFarm,
        FoodCropFarm,
        FoodFarmedFisherie,
        FoodOrchard,
        Health,
        Population,
        None
    }

    public class SoundManager : GameComponent
    {
        #region Fields
        private List<WavSound> soundsList;

        private float DefaultVolume = 0.6f;
        private float generalSoundVolume;
        private bool generalSoundState = true; //On or Off

        private Timer tmrBackgroundStartPlayingDelay;

        #region sounds
        WavSound MainMenuVents;
        WavSound MainMenuStairs;
        WavSound MainMenuTimeGateWater;
        WavSound MainMenuRoboticArms;
        WavSound MainMenuPiston;
        WavSound MainMenuCreditsPanelOpening;
        WavSound MainMenuCreditsPanelClosing;
        WavSound MainMenuSlot;
        WavSound CommandCenterButton;
        WavSound CommandCenterZone;
        WavSound SlidingPanelLong;
        WavSound SlidingPanelShort;
        WavSound LeftMenuButtons;
        //sunete pt cladiri
        WavSound EconomyFactory;
        WavSound EconomyMine;
        WavSound EconomyOilWell;
        WavSound EconomySawMill;
        WavSound Education;
        WavSound Energy;
        WavSound EnvironmentNursery;
        WavSound EnvironmentRecycling;
        WavSound EnvironmentWaterPurification;
        WavSound FoodAnimalFarm;
        WavSound FoodCropFarm;
        WavSound FoodFarmedFisherie;
        WavSound FoodOrchard;
        WavSound Health;
        WavSound Population;
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the general sound volume (float value between 0.0f and 1.0f)
        /// </summary>
        public float GeneralSoundVolume
        {
            get
            {
                return generalSoundVolume;
            }
            set
            {
                if (value >= 0.0f && value <= 1.0f)
                {
                    generalSoundVolume = value;
                    SetGeneralSoundVolume();
                }
            }
        }

        /// <summary>
        /// Indicates whether the general volume is at the default volume level or
        /// sets the general volume to the default value
        /// </summary>
        public bool UseDefaultSoundVolume
        {
            get
            {
                return DefaultVolume == generalSoundVolume;
            }
            set
            {
                generalSoundVolume = DefaultVolume;
                SetGeneralSoundVolume();
            }
        }

        /// <summary>
        /// Gets or sets the on or off state of all the sounds
        /// </summary>
        public bool SoundsOn
        {
            get
            {
                return generalSoundState;
            }
            set
            {
                generalSoundState = value;
                SetSoundOnOrOff();              
            }
        }
        #endregion

        #region Constructor
        public SoundManager(Game game)
            : base(game)
        {
            soundsList = new List<WavSound>();

            MainMenuVents = new WavSound(game, "mainMenuVents", true);
            MainMenuVents.SoundType = Sounds.MainMenuVents;
            soundsList.Add(MainMenuVents);
       
            MainMenuStairs = new WavSound(game, "mainMenuStairs", false);
            MainMenuStairs.SoundType = Sounds.MainMenuStairs;
            soundsList.Add(MainMenuStairs);

            MainMenuTimeGateWater = new WavSound(game, "mainMenuTimeGateWater", true);
            MainMenuTimeGateWater.SoundType = Sounds.MainMenuTimeGateWater;
            soundsList.Add(MainMenuTimeGateWater);

            MainMenuRoboticArms = new WavSound(game, "mainMenuRoboticArms", false);
            MainMenuRoboticArms.SoundType = Sounds.MainMenuRoboticArms;
            soundsList.Add(MainMenuRoboticArms);

            MainMenuPiston = new WavSound(game, "mainMenuPiston", false);
            MainMenuPiston.SoundType = Sounds.MainMenuPiston;
            soundsList.Add(MainMenuPiston);

            MainMenuCreditsPanelOpening = new WavSound(game, "mainMenuCreditsPanelOpening", false);
            MainMenuCreditsPanelOpening.SoundType = Sounds.MainMenuCreditsPanelOpening;
            soundsList.Add(MainMenuCreditsPanelOpening);

            MainMenuCreditsPanelClosing = new WavSound(game, "mainMenuCreditsPanelClosing", false);
            MainMenuCreditsPanelClosing.SoundType = Sounds.MainMenuCreditsPanelClosing;
            soundsList.Add(MainMenuCreditsPanelClosing);

            MainMenuSlot = new WavSound(game, "mainMenuSlot", false);
            MainMenuSlot.SoundType = Sounds.MainMenuSlot;
            soundsList.Add(MainMenuSlot);

            CommandCenterButton = new WavSound(game, "commandCenterButton", false);
            CommandCenterButton.SoundType = Sounds.CommandCenterButton;
            soundsList.Add(CommandCenterButton);

            CommandCenterZone = new WavSound(game, "commandCenterZone", false);
            CommandCenterZone.SoundType = Sounds.CommandCenterZone;
            soundsList.Add(CommandCenterZone);

            SlidingPanelLong = new WavSound(game, "slidingPanelLong", false);
            SlidingPanelLong.SoundType = Sounds.SlidingSoundLong;
            soundsList.Add(SlidingPanelLong);

            SlidingPanelShort= new WavSound(game, "slidingPanelShort", false);
            SlidingPanelShort.SoundType = Sounds.SlidingSoundShort;
            soundsList.Add(SlidingPanelShort);

            LeftMenuButtons = new WavSound(game, "LeftMenuButtons", false);
            LeftMenuButtons.SoundType = Sounds.LeftMenuButtons;
            soundsList.Add(LeftMenuButtons);

            EconomyFactory = new WavSound(game, "BuildingSounds\\EconomyFactory", false);
            EconomyFactory.SoundType = Sounds.EconomyFactory;
            soundsList.Add(EconomyFactory);

            EconomyMine = new WavSound(game, "BuildingSounds\\EconomyMine", false);
            EconomyMine.SoundType = Sounds.EconomyMine;
            soundsList.Add(EconomyMine);

            EconomyOilWell = new WavSound(game, "BuildingSounds\\EconomyOilWell", false);
            EconomyOilWell.SoundType = Sounds.EconomyOilWell;
            soundsList.Add(EconomyOilWell);

            EconomySawMill = new WavSound(game, "BuildingSounds\\EconomySawMill", false);
            EconomySawMill.SoundType = Sounds.EconomySawMill;
            soundsList.Add(EconomySawMill);

            Education = new WavSound(game, "BuildingSounds\\Education", false);
            Education.SoundType = Sounds.Education;
            soundsList.Add(Education);

            Energy = new WavSound(game, "BuildingSounds\\Energy", false);
            Energy.SoundType = Sounds.Energy;
            soundsList.Add(Energy);

            EnvironmentNursery = new WavSound(game, "BuildingSounds\\EnvironmentNursery", false);
            EnvironmentNursery.SoundType = Sounds.EnvironmentNursery;
            soundsList.Add(EnvironmentNursery);

            EnvironmentRecycling = new WavSound(game, "BuildingSounds\\EnvironmentRecycling", false);
            EnvironmentRecycling.SoundType = Sounds.EnvironmentRecycling;
            soundsList.Add(EnvironmentRecycling);

            EnvironmentWaterPurification = new WavSound(game, "BuildingSounds\\EnvironmentWaterPurification", false);
            EnvironmentWaterPurification.SoundType = Sounds.EnvironmentWaterPurification;
            soundsList.Add(EnvironmentWaterPurification);

            FoodAnimalFarm = new WavSound(game, "BuildingSounds\\FoodAnimalFarm", false);
            FoodAnimalFarm.SoundType = Sounds.FoodAnimalFarm;
            soundsList.Add(FoodAnimalFarm);

            FoodCropFarm = new WavSound(game, "BuildingSounds\\FoodCropFarm", false);
            FoodCropFarm.SoundType = Sounds.FoodCropFarm;
            soundsList.Add(FoodCropFarm);

            FoodFarmedFisherie = new WavSound(game, "BuildingSounds\\FoodFarmedFisherie", false);
            FoodFarmedFisherie.SoundType = Sounds.FoodFarmedFisherie;
            soundsList.Add(FoodFarmedFisherie);

            FoodOrchard = new WavSound(game, "BuildingSounds\\FoodOrchard", false);
            FoodOrchard.SoundType = Sounds.FoodOrchard;
            soundsList.Add(FoodOrchard);

            Health = new WavSound(game, "BuildingSounds\\Health", false);
            Health.SoundType = Sounds.Health;
            soundsList.Add(Health);

            Population = new WavSound(game, "BuildingSounds\\Population", false);
            Population.SoundType = Sounds.Population;
            soundsList.Add(Population);
                       
            UseDefaultSoundVolume = true;
            SetGeneralSoundVolume();

            tmrBackgroundStartPlayingDelay = new Timer(game);
            tmrBackgroundStartPlayingDelay.IntervalType = TimerIntervalType.Seconds;
            tmrBackgroundStartPlayingDelay.Interval = 3;//3 seconds
            tmrBackgroundStartPlayingDelay.OnTick +=new EventHandler(tmrBackgroundStartPlayingDelay_OnTick);
                     
            game.Components.Add(this);
            Game.Services.AddService(typeof(SoundManager), this);
        }

        #endregion

        #region Event Handlers
        void tmrBackgroundStartPlayingDelay_OnTick(object sender, EventArgs e)
        {
            tmrBackgroundStartPlayingDelay.Stop();
            //the interval between background sounds will be between 6 and 11 seconds
        }
        #endregion

        #region Methods
        public void PlaySound(Sounds sound)
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                if (soundsList[i].SoundType == sound)
                {
                    soundsList[i].Play();
                }
            }
        }

        public void StopSound(Sounds sound)
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                if (soundsList[i].SoundType == sound)
                {
                    soundsList[i].StopSound();
                }
            }
        }

        public void PauseSound(Sounds sound)
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                if (soundsList[i].SoundType == sound)
                {
                    soundsList[i].Pause();
                }
            }
        }

        public void ResumeSound(Sounds sound)
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                if (soundsList[i].SoundType == sound)
                {
                    soundsList[i].Resume();
                }
            }
        }

        /// <summary>
        /// Sets all the sounds On or Off
        /// </summary>
        void SetSoundOnOrOff()
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                soundsList[i].IsTurnedOn = generalSoundState;
            }
        }

        void SetGeneralSoundVolume()
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                soundsList[i].Volume = generalSoundVolume;
            }
        }

        /// <summary>
        /// Stops all the MainMenu Sounds
        /// </summary>
        public void StopMainMenuSounds()
        {
            for (int i = 0; i < soundsList.Count; i++)
            {
                if (soundsList[i].SoundType.ToString().Contains("MainMenu"))
                    soundsList[i].StopSound();
            }
        }

        public void StartBackgroundSong()
        {

        }

        public void StopBackgroundSong()
        {

        }
        #endregion

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

    }
}