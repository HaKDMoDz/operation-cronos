using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos;
using Operation_Cronos.Input;
using Operation_Cronos.IO;
using Operation_Cronos.Display;
using Operation_Cronos.Sound;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Profiles
{
    public class ProfileManager:GameComponent
    {
        #region Fields

        String profilesFolder = "Profiles";
        String defaultProfileFolder = "Default";
        String currentProfileFolder = "";

        XMLData xmlData;
        
        //indicates weather the user has created a new profile or has selected an existing one
        bool isNewProfile = true;

        #endregion

        #region Properties
        private CommandCenter CommandCenter
        {
            get { return (CommandCenter)Game.Services.GetService(typeof(CommandCenter)); }
        }

        private IOManager IOManager
        {
            get { return (IOManager)Game.Services.GetService(typeof(IOManager)); }
        }

        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }

        /// <summary>
        /// Indicates weather the user has created a new profile or has selected an existing one
        /// </summary>
        public bool ProfileIsNew
        {
            get 
            {
                return isNewProfile;
            }
        }
        #endregion

        #region Constructor
        public ProfileManager(Game game)
            : base(game)
        {
            xmlData = new XMLData(game);
            
            game.Components.Add(this);
            Game.Services.AddService(typeof(ProfileManager), this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the profile name for the user
        /// </summary>
        /// <param name="userName">The user/profile name</param>
        /// <param name="newProfile">If true, a new profile will be created, else the profile will be loaded</param>
        public void SetProfile(String userName, bool newProfile)
        {
            currentProfileFolder = userName;
            
            isNewProfile = newProfile;
        }

        public void LoadProfile()
        {
            if (isNewProfile) //the profile will be created with the 'default' settings
            {
                isNewProfile = false;

                DirectoryInfo defaultDir = new DirectoryInfo(profilesFolder+"\\"+defaultProfileFolder);

                foreach (FileInfo defaultFile in defaultDir.GetFiles())
                {
                    if (Directory.Exists(profilesFolder + "\\" + currentProfileFolder) == false)
                    {
                        Directory.CreateDirectory(profilesFolder + "\\" + currentProfileFolder);
                    }
                    File.Copy(profilesFolder + "\\" + defaultProfileFolder + "\\" + defaultFile.Name, profilesFolder + "\\" + currentProfileFolder + "\\" + defaultFile.Name);
                }
            }
            //at this point the profile already exists (weather it has just been created or it already existed)
            //and it will be loaded

            CommandCenterEventArgs optionsPanelSettings = xmlData.LoadSettings(profilesFolder + "\\" + currentProfileFolder);
            CommandCenter.SetGameOptions(optionsPanelSettings);
            try
            {
                CommandCenter.OnSaveOptions -= new EventHandler<CommandCenterEventArgs>(CommandCenter_OnSaveOptions);
            }
            catch (Exception) { }

            CommandCenter.OnSaveOptions += new EventHandler<CommandCenterEventArgs>(CommandCenter_OnSaveOptions);
            ResetAutosaveTimer(CommandCenter.GetCurrentSettings.AutosaveTimeValue);
            //sound            
            float volume = (float)optionsPanelSettings.VolumeValue;
            if (volume > 0)
                volume /= 10; //maximum will be 0.9f
            SoundManager.GeneralSoundVolume = volume;
            if (optionsPanelSettings.SoundState == "On")
                 SoundManager.SoundsOn = true;
            else
               SoundManager.SoundsOn = false;
            
            CommandCenterEventArgs timeGatePanelLevels = xmlData.LoadTimeGateUpgradeLevels(profilesFolder + "\\" + currentProfileFolder);
            CommandCenter.SetTimeGateLevels(timeGatePanelLevels);

            List<CommandCenterEventArgs> zonesParameters = xmlData.LoadZonesParameters(profilesFolder + "\\" + currentProfileFolder);
            CommandCenter.SetZoneParameters(zonesParameters);

            CommandCenter.SetCurrentUser(currentProfileFolder);
         }

        /// <summary>
        /// Saves the current profile's time gate upgrades
        /// </summary>
        public void SaveGame(CommandCenterEventArgs timeGateUpgrades)
        {
            xmlData.SaveTimeGateUpgradeLevels(profilesFolder + "\\" + currentProfileFolder, timeGateUpgrades);
        }

        /// <summary>
        /// Save Zone history
        /// </summary>
        public void SaveHistory(List<Slot> _slots)
        {
            xmlData.SaveZoneHistory(profilesFolder + "\\" + currentProfileFolder, _slots);
        }

        public List<Slot> LoadZoneHistory()
        {
            return xmlData.LoadZoneHistory(profilesFolder + "\\" + currentProfileFolder);
        }

        public void SaveZoneParameters(MilleniumGoalsSet mg)
        {
            xmlData.SaveZoneParameters(profilesFolder + "\\" + currentProfileFolder, mg);

            //List<CommandCenterEventArgs> zonesParameters = xmlData.LoadZonesParameters(profilesFolder + "\\" + currentProfileFolder);
            //CommandCenter.SetZoneParameters(zonesParameters);
        }

        public void SaveZoneResearch(List<Research> research)
        {
            xmlData.SaveZoneResearch(profilesFolder + "\\" + currentProfileFolder,research);
        }

        public List<Research> LoadResearchList()
        {
            return xmlData.LoadResearchList(profilesFolder + "\\" + currentProfileFolder);
        }
        /// <summary>
        /// Called after the profile is loader or when the options panel save settings button is pressed
        /// </summary>
        private void ResetAutosaveTimer(int minutes)
        {
            IOManager.AutosaveTimer.Stop();
            IOManager.AutosaveTimer.Interval = 60 * minutes;
            IOManager.AutosaveTimer.Start();
        }

        public void LogOut()
        {
            currentProfileFolder = "";
            SoundManager.SoundsOn = true;
            SoundManager.UseDefaultSoundVolume = true;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Saves the options to the Settings xml
        /// </summary>
        void CommandCenter_OnSaveOptions(object sender, CommandCenterEventArgs e)
        {
            //CommandCenter.OnSaveOptions -= new EventHandler<CommandCenterEventArgs>(CommandCenter_OnSaveOptions);
            ResetAutosaveTimer(e.AutosaveTimeValue);

            //sound
            float volume = (float)e.VolumeValue;
            if (volume > 0)
                volume /= 10; //maximum will be 0.9f
            SoundManager.GeneralSoundVolume = volume;
            if (e.SoundState == "On")
                SoundManager.SoundsOn = true;
            else
                SoundManager.SoundsOn = false;
            
            xmlData.SaveSettings(profilesFolder + "\\" + currentProfileFolder, e);
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
