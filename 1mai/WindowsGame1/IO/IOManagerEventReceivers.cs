using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using System.Collections.Generic;
using Operation_Cronos.GameProcessor;


namespace Operation_Cronos.IO
{
    public partial class IOManager : GameComponent
    {
        #region IEventReceiver Members

        #region MainMenu
        public void NewGame(object sender, MainMenuEventArgs args)
        {
            //creates the Profile Folder for the new profile
            DisplayManager.userList.AddUser(args.UserName);

            //the CommandCenter Graphics is loaded only once
            if (DisplayManager.CommandCenterLoaded == false)
                LoadCommandCenterGraphics();
            else
            {
                DisplayManager.InstanciateInterface(IOOperation.CommandCenter);
            }
        }

        public void LoadGame(object sender, MainMenuEventArgs args)
        {
            //the CommandCenter Graphics is loaded only once
            if (DisplayManager.CommandCenterLoaded == false)
                LoadCommandCenterGraphics();
            else
                DisplayManager.InstanciateInterface(IOOperation.CommandCenter);
        }

        public void DeleteUser(object sender, MainMenuEventArgs args)
        {
            DisplayManager.userList.DeleteUser(args.UserName);
        }

        public void QuitGame(object sender, MainMenuEventArgs args) { }
        #endregion

        #region CommandCenter
        public void EnterZone(object sender, CommandCenterEventArgs args)
        {
            //the GameInterface_Graphics Graphics is loaded only once
            if (DisplayManager.GameInterfaceLoaded == false)
                LoadGameInterfaceGraphics(args.ZoneName);
            else
                DisplayManager.DisplayGame();
        }

        public void SaveGame(object sender, CommandCenterEventArgs args)
        {
            DisplayManager.CameraFreeze();
            DisplayManager.ShowPreloaderTimerMode(IOOperation.SavingGame);
            ProfileManager.SaveGame(args);
        }

        public void SaveGameAndZone(object sender, CommandCenterEventArgs args, List<Slot> _slots, MilleniumGoalsSet mg, List<Research> research)
        {
            DisplayManager.CameraFreeze();
            DisplayManager.ShowPreloaderTimerMode(IOOperation.SavingGame);
            ProfileManager.SaveGame(args);
            ProfileManager.SaveHistory(_slots);
            ProfileManager.SaveZoneParameters(mg);
            ProfileManager.SaveZoneResearch(research);
        }

        #endregion

        #endregion
    }
}
