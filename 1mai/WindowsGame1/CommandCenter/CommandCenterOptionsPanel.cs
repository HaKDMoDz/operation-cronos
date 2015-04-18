using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.Display;
using Operation_Cronos.Input;
using Operation_Cronos.IO;

namespace Operation_Cronos
{
    class CommandCenterOptionsPanel : InputVisualComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;

        CommandCenterGeneralButton btnSaveOptions;

        //--General Buttons-----------------
        CommandCenterGeneralButton btnGame;
        CommandCenterGeneralButton btnVideo;
        CommandCenterGeneralButton btnAudio;
        CommandCenterGeneralButton btnControls;
        //----------------------------------

        //--Submenu Buttons-----------------
        CommandCenterGeneralButton btnGame_SaveGame;
        CommandCenterGeneralButton btnGame_Difficulty;

        CommandCenterGeneralButton btnVideo_Resolution;
        CommandCenterGeneralButton btnVideo_FullScreen;

        CommandCenterGeneralButton btnAudio_Volume;
        CommandCenterGeneralButton btnAudio_Sound;

        CommandCenterGeneralButton btnControls_Mouse;
        CommandCenterGeneralButton btnControls_Keyboard;
        //-----------------------------------

        List<List<CommandCenterGeneralButton>> generalButtons;
        List<CommandCenterGeneralButton> sublistGame;
        List<CommandCenterGeneralButton> sublistVideo;
        List<CommandCenterGeneralButton> sublistAudio;
        List<CommandCenterGeneralButton> sublistControls;


        //--SUBMENU Buttons OPTIONS----------            
       
        //GAME
        CommandCenterGeneralButton btnSave;
        Sprite AutosaveSprite;
        Scrollbar AutosaveScrollbar;
        SpriteText AutosaveTime;
        int[] TimeValues = new int[3] {10,20,30};//save game time interval
        int TimeValueIndexSelected;

        RadioButton rbnEasy;
        RadioButton rbnMedium;
        RadioButton rbnHard;
        List<RadioButton> DifficultyRadioGroup = new List<RadioButton>(3);
        String DifficultyLevelCurrentlySelected;
            
        //VIDEO
        Scrollbar ResolutionIndicator;
        SpriteText Resolution;
        Point[] ResolutionValues = new Point[7] { new Point(1024, 768), new Point(1280, 720), new Point(1280, 800), new Point(1280,1024),new Point(1440,900), new Point(1600, 900), new Point(1600, 1200) };
        int ResolutionValueIndexSelected;//the default resolution will be the first one, set in CommandCenterEventArgs

        RadioButton rbnFullScreenOn;
        RadioButton rbnFullScreenOff;
        String FullScreenCurrentState;
           
        //AUDIO
        Scrollbar VolumeIndicator;
        Sprite VolumeValueSprite;

        RadioButton rbnSoundOn;
        RadioButton rbnSoundOff;
        String SoundCurrentState;

        //CONTROLS
        Sprite CameraSpeedSprite;
        Scrollbar CameraSpeedIndicator;
        Sprite CameraSpeedValueSprite;
        CommandCenterGeneralButton btnKeyShortcuts;
        Sprite KeyShortcutsPanel;
        CommandCenterGeneralButton btnKeyShortcutsPanel_Back;
        //-----------------------------------

        /// <summary>
        /// In case the options are not changed by the player, the DefaultOptions
        /// will be accessed from the CommandCenter and sent as CommandCenterEventArgs on EnterZone
        /// </summary>
        public CommandCenterEventArgs DefaultOptions;


        //this section is destined to save the default settings (or the last saved settings)
        //for when the player changes the settings but forgets to click SaveSettings Options, 
        //and returns to the Options Panel later on (he must find the default options or the last saved options)
        int TimeValueIndexSelected_LastSave; //Autosave Time
        string DifficultyLevel_LastSave; 
        int ResolutionValueIndexSelected_LastSave;
        string FullScreenState_LastSave; //"On" / "Off"
        int VolumeValue_LastSave; //the volume Sprite frame number
        string SoundState_LastSave; //"On" / "Off"
        int CameraSpeed_LastState; //the camera Speed Sprite frame number
        //---------------------------------------------------------------------------------

        bool isHidden = false;
        int[] positions;
        #endregion

        #region Events
        public event EventHandler<CommandCenterEventArgs> OnSaveOptions = delegate { };
        public event EventHandler OnSaveGame = delegate { };
        #endregion

        #region Properties
        public bool IsHidden
        {
            get
            {
                return isHidden;
            }
        }
        #endregion

        #region Constructors
        public CommandCenterOptionsPanel(Game game, int DrawOrder): base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));
            generalButtons = new List<List<CommandCenterGeneralButton>>();
            sublistGame = new List<CommandCenterGeneralButton>();
            sublistVideo = new List<CommandCenterGeneralButton>();
            sublistAudio = new List<CommandCenterGeneralButton>();
            sublistControls = new List<CommandCenterGeneralButton>();


            //------------------------------
            //--Setting Default Options (will be used when initializing objects, in the next instructions)
            DefaultOptions = new CommandCenterEventArgs(1);
            //everything is set to a GENERAL default
            //------------------------------


            //---SaveSettings Options Button-----
            btnSaveOptions = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("SaveOptions")), DrawOrder);
            btnSaveOptions.Position = new Point(390, 675);
            btnSaveOptions.OnPress +=new EventHandler<ButtonEventArgs>(Do_SaveOptionsPress);
            btnSaveOptions.OnRelease += new EventHandler<ButtonEventArgs>(Do_SaveOptionsRelease);
            AddChild(btnSaveOptions);
            //---------------------------

            //-----Primary Buttons-------
            btnGame = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Options_Game")), DrawOrder);
            btnGame.Position = new Point(70, 150);
            btnGame.OnPress += new EventHandler<ButtonEventArgs>(Do_GeneralBtnPressed);
            btnGame.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseOver);
            btnGame.OnMouseLeave+= new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseLeave);
            AddChild(btnGame);            
            sublistGame.Add(btnGame);

            btnVideo = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Options_Video")), DrawOrder);
            btnVideo.Position = new Point(70, 300);
            btnVideo.OnPress += new EventHandler<ButtonEventArgs>(Do_GeneralBtnPressed);
            btnVideo.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseOver);
            btnVideo.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseLeave);
            AddChild(btnVideo);
            sublistVideo.Add(btnVideo);

            btnAudio = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Options_Audio")), DrawOrder);
            btnAudio.Position = new Point(70, 440);
            btnAudio.OnPress += new EventHandler<ButtonEventArgs>(Do_GeneralBtnPressed);
            btnAudio.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseOver);
            btnAudio.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseLeave);
            AddChild(btnAudio);
            sublistAudio.Add(btnAudio);

            btnControls = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Options_Controls")), DrawOrder);
            btnControls.Position = new Point(70, 580);
            btnControls.OnPress += new EventHandler<ButtonEventArgs>(Do_GeneralBtnPressed);
            btnControls.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseOver);
            btnControls.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_GeneralBtnMouseLeave);
            AddChild(btnControls);
            sublistControls.Add(btnControls);

            //---GAME Submenu Buttons-------
            btnGame_SaveGame = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("SaveGame")), DrawOrder);
            btnGame_SaveGame.Position = new Point(260, 128);
            btnGame_SaveGame.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnGame_SaveGame.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnGame_SaveGame.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnGame_SaveGame);
            sublistGame.Add(btnGame_SaveGame);

            btnGame_Difficulty = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Difficulty")), DrawOrder);
            btnGame_Difficulty.Position = new Point(260, 181);
            btnGame_Difficulty.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnGame_Difficulty.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnGame_Difficulty.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnGame_Difficulty);
            sublistGame.Add(btnGame_Difficulty);

                //---Submenu button SaveGame Options-----
            btnSave = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("SaveBtn")), DrawOrder);
            btnSave.Position = new Point(454,126);
            btnSave.OnRelease += new EventHandler<ButtonEventArgs>(Do_BtnSaveMouseRelease);
            AddChild(btnSave);

            AutosaveSprite = new Sprite(game, graphicsCollection.GetPack("Autosave"));
            AutosaveSprite.StackOrder = DrawOrder;
            AutosaveSprite.XRelative = 567;
            AutosaveSprite.YRelative = 133;
            AddChild(AutosaveSprite);

            positions = new int [6]{746,127,705,127,820,127};
            AutosaveScrollbar = new Scrollbar(game, new Sprite(game, graphicsCollection.GetPack("AutosaveTimeDisplay")), new Sprite(Game, graphicsCollection.GetPack("ScrollBarLeftArrow")), new Sprite(Game, graphicsCollection.GetPack("ScrollBarRightArrow")), DrawOrder, positions);
            AutosaveScrollbar.OnScrollUp+=new EventHandler(Do_AutosaveScrollbar_LeftScroll);
            AutosaveScrollbar.OnScrollDown += new EventHandler(Do_AutosaveScrollbar_RightScroll);
            AddChild(AutosaveScrollbar);

            AutosaveTime = new SpriteText(game, FontsCollection.GetPack("Courier New 10").Font);
            AutosaveTime.XRelative = 754;
            AutosaveTime.YRelative = 133;
            TimeValueIndexSelected = DefaultOptions.TimeValueIndex;//by default, autosave every 30 min
            AutosaveTime.Text = TimeValues[TimeValueIndexSelected].ToString() + " min";
            AutosaveTime.Tint = Color.LightGray;
            AddChild(AutosaveTime);

            DefaultOptions.AutosaveTimeValue = TimeValues[TimeValueIndexSelected];
                //---------------------------------------


                //---Submenu button Difficulty Options---
            positions = new int[4] { 443, 185, 508, 180 };
            rbnEasy = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("Diff_Easy")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions);
            rbnEasy.AllowDirectUncheck = false;
            rbnEasy.OnCheck += new EventHandler(Do_DifficultyRadioChecked);
            rbnEasy.Name = "Easy";
            AddChild(rbnEasy);
            DifficultyRadioGroup.Add(rbnEasy);

            positions = new int[4] { 558, 185, 651, 180 };
            rbnMedium = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("Diff_Medium")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions);
            rbnMedium.AllowDirectUncheck = false;
            rbnMedium.OnCheck += new EventHandler(Do_DifficultyRadioChecked);
            rbnMedium.Name = "Medium";
            AddChild(rbnMedium);
            DifficultyRadioGroup.Add(rbnMedium);

            positions = new int[4] { 703, 185, 773, 180 };
            rbnHard = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("Diff_Hard")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions); ;
            rbnHard.AllowDirectUncheck = false;
            rbnHard.OnCheck += new EventHandler(Do_DifficultyRadioChecked);
            rbnHard.Name = "Hard";
            AddChild(rbnHard);
            DifficultyRadioGroup.Add(rbnHard);
                        
            DifficultyLevelCurrentlySelected = DefaultOptions.DifficultyLevel;
            switch (DifficultyLevelCurrentlySelected)
            {
                case "Easy":
                    rbnEasy.Check();
                    break;
                case "Medium":
                    rbnMedium.Check();
                    break;
                case "Hard":
                    rbnHard.Check();
                    break;
                default:
                    rbnEasy.Check();
                    break;
            }
                //---------------------------------------

            //------------------------------


            //---VIDEO Submenu Buttons------
            btnVideo_Resolution = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Resolution")), DrawOrder);
            btnVideo_Resolution.Position = new Point(260, 276);
            btnVideo_Resolution.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnVideo_Resolution.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnVideo_Resolution.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnVideo_Resolution);
            sublistVideo.Add(btnVideo_Resolution);

            btnVideo_FullScreen = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("FullScreen")), DrawOrder);
            btnVideo_FullScreen.Position = new Point(260, 326);
            btnVideo_FullScreen.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnVideo_FullScreen.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnVideo_FullScreen.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnVideo_FullScreen);
            sublistVideo.Add(btnVideo_FullScreen);

                //---Submenu button ResolutionIndex Options-------
            Resolution = new SpriteText(Game, FontsCollection.GetPack("Courier New 10").Font);
            Resolution.XRelative = 617;
            Resolution.YRelative = 280;
            Resolution.Tint = Color.LightGray;
            
            ResolutionValueIndexSelected = DefaultOptions.ResolutionIndex;
            DefaultOptions.ResolutionValue = ResolutionValues[ResolutionValueIndexSelected];

            Resolution.Text = ResolutionValues[ResolutionValueIndexSelected].X.ToString() + " X " + ResolutionValues[ResolutionValueIndexSelected].Y.ToString();
            AddChild(Resolution);

            positions = new int[6] { 592, 275, 550, 275, 738, 275 };
            ResolutionIndicator = new Scrollbar(game, new Sprite(game, graphicsCollection.GetPack("ResolutionDisplay")), new Sprite(Game, graphicsCollection.GetPack("ScrollBarLeftArrow")), new Sprite(Game, graphicsCollection.GetPack("ScrollBarRightArrow")), DrawOrder, positions);        
            ResolutionIndicator.OnScrollUp += new EventHandler(Do_ResolutionIndicator_LeftScroll);
            ResolutionIndicator.OnScrollDown += new EventHandler(Do_ResolutionIndicator_RightScroll);
            AddChild(ResolutionIndicator);
                //------------------------------

                //---Submenu button FullScreen Options-------
            positions = new int[4] {465, 330, 505, 322};
            rbnFullScreenOn = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("On")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions);
            rbnFullScreenOn.AllowDirectUncheck = false;
            rbnFullScreenOn.OnCheck +=new EventHandler(Do_FullScreenRadioOnChecked);
            AddChild(rbnFullScreenOn);

            positions = new int[4] {575, 330, 625, 322 };
            rbnFullScreenOff = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("Off")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions);
            rbnFullScreenOff.AllowDirectUncheck = false;
            rbnFullScreenOff.OnCheck += new EventHandler(Do_FullScreenRadioOffChecked);
            AddChild(rbnFullScreenOff);

            FullScreenCurrentState = DefaultOptions.FullScreen;//"Off" by default;
            switch (FullScreenCurrentState)
            {
                case "On":
                    rbnFullScreenOn.Check();
                    break;
                case "Off":
                    rbnFullScreenOff.Check();
                    break;
                default:
                    rbnFullScreenOff.Check();
                    break;
            }
                //------------------------------

            //------------------------------


            //---AUDIO Submenu Buttons------
            btnAudio_Volume = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Volume")), DrawOrder);
            btnAudio_Volume.Position = new Point(260, 411);
            btnAudio_Volume.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnAudio_Volume.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnAudio_Volume.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnAudio_Volume);
            sublistAudio.Add(btnAudio_Volume);

            btnAudio_Sound = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Sound")), DrawOrder);
            btnAudio_Sound.Position = new Point(260, 461);
            btnAudio_Sound.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnAudio_Sound.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnAudio_Sound.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnAudio_Sound);
            sublistAudio.Add(btnAudio_Sound);

                //---Submenu button Volume Options-------
            VolumeValueSprite = new Sprite(game, graphicsCollection.GetPack("Increment"));
            VolumeValueSprite.StackOrder = DrawOrder;
            VolumeValueSprite.XRelative = 457;
            VolumeValueSprite.YRelative = 417;
            VolumeValueSprite.FrameNumber = DefaultOptions.VolumeValue;//0 by default
            AddChild(VolumeValueSprite);

            positions = new int[6] {450, 410, 410, 410, 545, 410 };
            VolumeIndicator = new Scrollbar(game, new Sprite(game, graphicsCollection.GetPack("PlusMinusDisplay")), new Sprite(Game, graphicsCollection.GetPack("Minus")), new Sprite(Game, graphicsCollection.GetPack("Plus")), DrawOrder, positions);
            VolumeIndicator.OnScrollUp += new EventHandler(Do_VolumeIndicatorLeftScroll);
            VolumeIndicator.OnScrollDown += new EventHandler(Do_VolumeIndicatorRightScroll);
            AddChild(VolumeIndicator);
                //---------------------------------------

                //---Submenu button Sound Options-------
            positions = new int[4] { 395, 465, 435, 459 };
            rbnSoundOn = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("On")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions);
            rbnSoundOn.AllowDirectUncheck = false;
            rbnSoundOn.OnCheck +=new EventHandler(Do_SoundRadioOnChecked);
            AddChild(rbnSoundOn);

            positions = new int[4] { 507, 465, 557, 459 };
            rbnSoundOff = new RadioButton(game, new Sprite(game, graphicsCollection.GetPack("Off")), new Sprite(Game, graphicsCollection.GetPack("Checkbox")), DrawOrder, positions);
            rbnSoundOff.AllowDirectUncheck = false;
            rbnSoundOff.OnCheck += new EventHandler(Do_SoundRadioOffChecked);
            AddChild(rbnSoundOff);

            rbnSoundOn.Check();
            SoundCurrentState = DefaultOptions.SoundState;//"On" by default
            switch (SoundCurrentState)
            {
                case "On":
                    rbnSoundOn.Check();
                    break;
                case "Off":
                    rbnSoundOff.Check();
                    break;
                default:
                    rbnSoundOn.Check();
                    break;
            }
                //------------------------------

            //------------------------------


            //---CONTROLS Submenu Buttons---
            btnControls_Mouse = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("MouseOptions")), DrawOrder);
            btnControls_Mouse.Position = new Point(260, 546);
            btnControls_Mouse.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnControls_Mouse.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnControls_Mouse.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnControls_Mouse);
            sublistControls.Add(btnControls_Mouse);

            btnControls_Keyboard = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("KeyboardOptions")), DrawOrder);
            btnControls_Keyboard.Position = new Point(260, 601);
            btnControls_Keyboard.OnPress += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnPressed);
            btnControls_Keyboard.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseOver);
            btnControls_Keyboard.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_SubmenuBtnMouseLeave);
            AddChild(btnControls_Keyboard);
            sublistControls.Add(btnControls_Keyboard);

                //---Submenu button CameraSpeed Options-----
            CameraSpeedSprite = new Sprite(game, graphicsCollection.GetPack("CameraSpeed"));
            CameraSpeedSprite.XRelative = 400;
            CameraSpeedSprite.YRelative = 550;
            AddChild(CameraSpeedSprite);

            CameraSpeedValueSprite = new Sprite(game, graphicsCollection.GetPack("Increment"));
            CameraSpeedValueSprite.XRelative = 637;
            CameraSpeedValueSprite.YRelative = 550;
            CameraSpeedValueSprite.FrameNumber = DefaultOptions.CameraSpeed;//0 by default
            AddChild(CameraSpeedValueSprite);

            positions = new int[6] { 630, 543, 590, 543, 727, 543 };
            CameraSpeedIndicator = new Scrollbar(game, new Sprite(game, graphicsCollection.GetPack("PlusMinusDisplay")), new Sprite(Game, graphicsCollection.GetPack("Minus")), new Sprite(Game, graphicsCollection.GetPack("Plus")), DrawOrder, positions);
            CameraSpeedIndicator.OnScrollUp += new EventHandler(Do_CameraSpeedIndicator_LeftScroll);
            CameraSpeedIndicator.OnScrollDown += new EventHandler(Do_CameraSpeedIndicator_RightScroll);
            AddChild(CameraSpeedIndicator);
                //---------------------------------------

                //---Submenu button KeyShortcuts Options-----
            btnKeyShortcuts = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("KeyShortcuts")), DrawOrder);
            btnKeyShortcuts.Position = new Point(440, 600);
            btnKeyShortcuts.OnMousePress += new EventHandler<MouseEventArgs>(Do_BtnKeyShortcutsPressed);
            btnKeyShortcuts.OnMouseRelease += new EventHandler<MouseEventArgs>(Do_BtnKeyShortcutsReleased);
            AddChild(btnKeyShortcuts);

            KeyShortcutsPanel = new Sprite(game, graphicsCollection.GetPack("KeyShortcutsPanel"));
            KeyShortcutsPanel.XRelative = 52;
            KeyShortcutsPanel.YRelative = 125;
            KeyShortcutsPanel.StackOrder = DrawOrder + 2;
            KeyShortcutsPanel.Enabled = false;
            AddChild(KeyShortcutsPanel);

            btnKeyShortcutsPanel_Back = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("btnKeyShortcutsPanel_Back")), DrawOrder+3);
            btnKeyShortcutsPanel_Back.Position = new Point(855, 150);
            btnKeyShortcutsPanel_Back.OnMouseRelease += new EventHandler<MouseEventArgs>(Do_BtnKeyShortcutsPanel_BackReleased);
            btnKeyShortcutsPanel_Back.Enabled = false;
            AddChild(btnKeyShortcutsPanel_Back);
                //---------------------------------------

            //------------------------------

            //------------------------------
            //--Saving/Setting LastSave Options (which are the Default values, at this point)
            TimeValueIndexSelected_LastSave = TimeValueIndexSelected;
           
            if (rbnEasy.IsChecked)
                DifficultyLevel_LastSave = rbnEasy.Name; 
            else
                if (rbnMedium.IsChecked)
                    DifficultyLevel_LastSave = rbnMedium.Name;
                else
                    DifficultyLevel_LastSave = rbnHard.Name;

            ResolutionValueIndexSelected_LastSave = ResolutionValueIndexSelected;

            if (rbnFullScreenOn.IsChecked)
                FullScreenState_LastSave = "On";
            else
                FullScreenState_LastSave = "Off";

            VolumeValue_LastSave = VolumeValueSprite.FrameNumber;

            if (rbnSoundOn.IsChecked)
                SoundState_LastSave = "On";
            else
                SoundState_LastSave = "Off";

            CameraSpeed_LastState = CameraSpeedValueSprite.FrameNumber;
            //------------------------------

            generalButtons.Add(sublistGame);
            generalButtons.Add(sublistVideo);
            generalButtons.Add(sublistAudio);
            generalButtons.Add(sublistControls);

            this.StackOrder = DrawOrder;
        }
        #endregion

        #region Event Handlers

        #region SaveSettings Options Button
        void Do_SaveOptionsPress(object sender, EventArgs e)
        {            
            //refreshes the LastSave values
            TimeValueIndexSelected_LastSave = TimeValueIndexSelected;

            if (rbnEasy.IsChecked)
                DifficultyLevel_LastSave = rbnEasy.Name;
            else
                if (rbnMedium.IsChecked)
                    DifficultyLevel_LastSave = rbnMedium.Name;
                else
                    DifficultyLevel_LastSave = rbnHard.Name;

            ResolutionValueIndexSelected_LastSave = ResolutionValueIndexSelected;

            if (rbnFullScreenOn.IsChecked)
                FullScreenState_LastSave = "On";
            else
                FullScreenState_LastSave = "Off";

            VolumeValue_LastSave = VolumeValueSprite.FrameNumber;

            if (rbnSoundOn.IsChecked)
                SoundState_LastSave = "On";
            else
                SoundState_LastSave = "Off";

            CameraSpeed_LastState = CameraSpeedValueSprite.FrameNumber;

            Hide();
            Show();
            //Launches the OnSaveOptions Event
            CommandCenterEventArgs args = new CommandCenterEventArgs(ResolutionValueIndexSelected_LastSave, FullScreenState_LastSave, TimeValueIndexSelected_LastSave, DifficultyLevel_LastSave, VolumeValue_LastSave, SoundState_LastSave, CameraSpeed_LastState);
            args.ResolutionValue = ResolutionValues[ResolutionValueIndexSelected_LastSave];
            args.AutosaveTimeValue = TimeValues[TimeValueIndexSelected_LastSave];
            //OnSaveOptions(this, new CommandCenterEventArgs(ResolutionValues[ResolutionValueIndexSelected],FullScreenCurrentState));
            OnSaveOptions(this, args);
        }

        void Do_SaveOptionsRelease(object sender, EventArgs e)
        {
            btnSaveOptions.ReleaseButton();
            btnSaveOptions.MouseOverAnimation();
        }
        #endregion

        #region Game,Video,Audio,Credits Buttons
        void Do_GeneralBtnPressed(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < generalButtons.Count; i++)
            {
                if (generalButtons[i][0] != ((CommandCenterGeneralButton)sender))//not the button that launched the Event
                {
                    if (generalButtons[i][0].IsPressed)//and currently pressed (previosly selected)
                    {
                        generalButtons[i][0].ReleaseButton();//it is released
                        //hides the previously selected button's submenu
                        for (int j = 1; j < generalButtons[i].Count; j++)
                        {
                            generalButtons[i][j].Hide();
                            generalButtons[i][j].ReleaseButton();//if it was pressed it will be released
                            HideSubmenuButtonOptions(i, j);
                        }
                    }
                }
                else //(generalButtons[i][0] == ((CommandCenterGeneralButton)sender))// the button that Launched the Event
                {
                    for (int j = 1; j < generalButtons[i].Count; j++)
                        generalButtons[i][j].Show();
                }
            }
            
        }

        void Do_GeneralBtnMouseOver(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < generalButtons.Count; i++)
            {
                if (generalButtons[i][0] == ((CommandCenterGeneralButton)sender))//the button that launched the Event
                {
                    if (!generalButtons[i][0].IsPressed)//is not currently pressed
                    {
                        //shows the button's submenu
                        for (int j = 1; j < generalButtons[i].Count; j++)
                        {
                            generalButtons[i][j].Show();
                        }
                    }
                    break; //the other buttons will not be tested since the Event Sender one was found
                }
            }
        }

        void Do_GeneralBtnMouseLeave(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < generalButtons.Count; i++)
            {
                if (generalButtons[i][0] == ((CommandCenterGeneralButton)sender))//the button that launched the Event
                {
                    if (!generalButtons[i][0].IsPressed)//is not currently pressed
                    {
                        for (int j = 1; j < generalButtons[i].Count; j++)
                        {
                            generalButtons[i][j].Hide();
                        }
                    }
                    break; //the other buttons will not be tested since the Event Sender one was found
                }
            }
        }
        #endregion

        #region Submenus Buttons
        void Do_SubmenuBtnPressed(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < generalButtons.Count; i++)
            {
                if (generalButtons[i][0].IsPressed) //to determine the submenu curently selected
                {
                    for (int j = 1; j < generalButtons[i].Count; j++)
                    {
                        if (generalButtons[i][j] != ((CommandCenterGeneralButton)sender))//not the submenu button that launched the Event
                        {
                            if (generalButtons[i][j].IsPressed)//and is currently pressed (previosly selected)
                            {
                                generalButtons[i][j].ReleaseButton();//it is released
                                HideSubmenuButtonOptions(i, j);
                            }
                        }
                        else //it's the submenu button that Launched the Event
                            ShowSubmenuButtonOptions(i,j);
                    }
                    break;
                }
            }
        }

        void Do_SubmenuBtnMouseOver(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < generalButtons.Count; i++)
            {
                if (generalButtons[i][0].IsPressed) //the Submenu Button that Launched the Event is in this submenu
                {
                    for (int j = 1; j < generalButtons[i].Count; j++)
                    {
                        if (generalButtons[i][j] == ((CommandCenterGeneralButton)sender))//Submenu button that Launched the Event
                        {
                            if (!generalButtons[i][j].IsPressed)
                                ShowSubmenuButtonOptions(i,j);
                            break;
                        }
                    }
                    break;
                }
            }
        }
                
        void Do_SubmenuBtnMouseLeave(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < generalButtons.Count; i++)
            {
                if (generalButtons[i][0].IsPressed) //the Submenu Button that Launched the Event is in this submenu
                {
                    for (int j = 1; j < generalButtons[i].Count; j++)
                    {
                        if (generalButtons[i][j] == ((CommandCenterGeneralButton)sender))//Submenu button that Launched the Event
                        {
                            if (!generalButtons[i][j].IsPressed)
                                HideSubmenuButtonOptions(i, j);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        #endregion

        #region Game Submenu Buttons Options
        void Do_BtnSaveMouseRelease(object sender, ButtonEventArgs e)
        {
            btnSave.ReleaseButton();

            //SaveSettings Game Event Launched
            OnSaveGame(this, new EventArgs());
        }

        void Do_AutosaveScrollbar_LeftScroll(object sender, EventArgs e)
        {
            if (TimeValueIndexSelected > 0)
            {
                TimeValueIndexSelected -= 1;
                AutosaveTime.Text = TimeValues[TimeValueIndexSelected].ToString() + " min";
            }
        }

        void Do_AutosaveScrollbar_RightScroll(object sender, EventArgs e)
        {
            if (TimeValueIndexSelected < TimeValues.Length - 1)
            {
                TimeValueIndexSelected += 1;
                AutosaveTime.Text = TimeValues[TimeValueIndexSelected].ToString() + " min";
            }
        }

        void Do_DifficultyRadioChecked(object sender, EventArgs e)
        {
            for (int i = 0; i < DifficultyRadioGroup.Count; i++)
            {
                //if there is another Radio (than the one that launched this Event) that is checked it will be unchecked
                if (DifficultyRadioGroup[i] != ((RadioButton)sender))
                    if (DifficultyRadioGroup[i].IsChecked)
                        DifficultyRadioGroup[i].Uncheck();
                if (DifficultyRadioGroup[i] == ((RadioButton)sender))
                    DifficultyLevelCurrentlySelected = DifficultyRadioGroup[i].Name;
            }
        }
        #endregion

        #region Video Submenu Buttons Options
        void Do_ResolutionIndicator_LeftScroll(object sender, EventArgs e)
        {
            if (ResolutionValueIndexSelected > 0)
            {
                ResolutionValueIndexSelected -= 1;
                Resolution.Text = ResolutionValues[ResolutionValueIndexSelected].X.ToString() +" X " + ResolutionValues[ResolutionValueIndexSelected].Y.ToString();
            }
        }

        void Do_ResolutionIndicator_RightScroll(object sender, EventArgs e)
        {
            if (ResolutionValueIndexSelected < ResolutionValues.Length - 1)
            {
                ResolutionValueIndexSelected += 1;
                Resolution.Text = ResolutionValues[ResolutionValueIndexSelected].X.ToString() + " X " + ResolutionValues[ResolutionValueIndexSelected].Y.ToString();
            }
        }

        void Do_FullScreenRadioOnChecked(object sender, EventArgs e)
        {
            rbnFullScreenOff.Uncheck();
            FullScreenCurrentState = "On";
        }

        void Do_FullScreenRadioOffChecked(object sender, EventArgs e)
        {
            rbnFullScreenOn.Uncheck();
            FullScreenCurrentState = "Off";
        }
        #endregion

        #region Audio Submenu Buttons Options
        void Do_VolumeIndicatorLeftScroll(object sender, EventArgs e)
        {
            if (VolumeValueSprite.FrameNumber > 0)
            {
                VolumeValueSprite.FrameNumber -= 1;
            }
        }

        void Do_VolumeIndicatorRightScroll(object sender, EventArgs e)
        {
            if (VolumeValueSprite.FrameNumber < 9) //VolumeValueSprite has 10 frames
            {
                VolumeValueSprite.FrameNumber += 1;
            }
        }

        void Do_SoundRadioOnChecked(object sender, EventArgs e)
        {
            rbnSoundOff.Uncheck();
            SoundCurrentState = "On";
        }

        void Do_SoundRadioOffChecked(object sender, EventArgs e)
        {
            rbnSoundOn.Uncheck();
            SoundCurrentState = "Off";
        }
        #endregion

        #region Controls Buttons Options
        void Do_CameraSpeedIndicator_LeftScroll(object sender, EventArgs e)
        {
            if (CameraSpeedValueSprite.FrameNumber > 0)
                CameraSpeedValueSprite.FrameNumber -= 1;
        }

        void Do_CameraSpeedIndicator_RightScroll(object sender, EventArgs e)
        {
            if (CameraSpeedValueSprite.FrameNumber < 9)
                CameraSpeedValueSprite.FrameNumber += 1;
        }

        void Do_BtnKeyShortcutsPressed(object sender, MouseEventArgs e)
        {
        }

        void Do_BtnKeyShortcutsReleased(object sender, MouseEventArgs e)
        {
            btnKeyShortcuts.ReleaseButton();
            btnKeyShortcuts.MouseOverAnimation();

            //this.HideSprites();
            KeyShortcutsPanel.Visible = true;
            KeyShortcutsPanel.Enabled = true;
            btnKeyShortcutsPanel_Back.Show();
            btnKeyShortcutsPanel_Back.Enabled = true;
        }

        void Do_BtnKeyShortcutsPanel_BackReleased(object sender, MouseEventArgs e)
        {
            btnKeyShortcutsPanel_Back.ReleaseButton();

            this.Hide();
            this.Show();
        }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Hides the entire Options Panel
        /// </summary>
        public void Hide()
        {
            ResetOptions(); //for when options are changed but the SaveSettings Options button was not pressed
            //the options return to the LastSave values

            for (int i = 0; i < generalButtons.Count; i++)
            {
                for (int j = 0; j < generalButtons[i].Count; j++)
                {
                    generalButtons[i][j].Hide();
                    generalButtons[i][j].ReleaseButton();//will release the submenu pressed button
                    HideSubmenuButtonOptions(i, j);
                }
                generalButtons[i][0].ReleaseButton();//will release the pressed button
            }
            btnSaveOptions.Hide();
            btnSaveOptions.Enabled = false;

            this.Enabled = false;
        }

        /// <summary>
        /// Shows the Options Panel (the 4 general buttons)
        /// </summary>
        public void Show()
        {
            ResetOptions(); //for when options are changed but the SaveSettings Options button was not pressed
            //the options return to the LastSave values
            for (int i = 0; i < generalButtons.Count; i++)
                    generalButtons[i][0].Show();
            btnSaveOptions.Show();

            this.Enabled = true;
        }

        /// <summary>
        /// Shows the indicated submenu's options
        /// </summary>
        void ShowSubmenuButtonOptions(int generalButtonIndex, int submenuButtonIndex)
        {
            switch (generalButtonIndex)
            {
                case 0: //Game
                    switch (submenuButtonIndex)
                    {
                        case 1: //SaveGame
                            btnSave.Show();
                            AutosaveSprite.Visible = true;
                            AutosaveScrollbar.Show();
                            AutosaveTime.Visible = true;
                            break;
                        case 2: //Difficulty
                            rbnEasy.Show();
                            rbnMedium.Show();
                            rbnHard.Show();
                            break;
                    }
                    break;
                case 1: //Video
                    switch (submenuButtonIndex)
                    {
                        case 1: //ResolutionIndex
                            Resolution.Visible = true;
                            ResolutionIndicator.Show();
                            break;
                        case 2: //FullScreen
                            rbnFullScreenOn.Show();
                            rbnFullScreenOff.Show();
                            break;
                    }
                    break;
                case 2: //Audio
                    switch (submenuButtonIndex)
                    {
                        case 1: //Volume
                            VolumeIndicator.Show();
                            VolumeValueSprite.Visible = true;                            
                            break;
                        case 2: //Sound
                            rbnSoundOn.Show();
                            rbnSoundOff.Show();
                            break;
                    }
                    break;
                case 3: //Controls
                    switch (submenuButtonIndex)
                    {
                        case 1: //Mouse
                            CameraSpeedSprite.Visible = true;
                            CameraSpeedValueSprite.Visible = true;
                            CameraSpeedIndicator.Show();
                            break;
                        case 2: //Keyboard
                            btnKeyShortcuts.Show();
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Hides the indicated submenu's options
        /// </summary>
        void HideSubmenuButtonOptions(int generalButtonIndex, int submenuButtonIndex)
        {
            switch (generalButtonIndex)
            {
                case 0: //Game
                    switch (submenuButtonIndex)
                    {
                        case 1: //SaveGame
                            btnSave.Hide();
                            AutosaveSprite.Visible = false;
                            AutosaveScrollbar.Hide();
                            AutosaveTime.Visible = false;
                            break;
                        case 2: //Difficulty
                            rbnEasy.Hide();
                            rbnMedium.Hide();
                            rbnHard.Hide();
                            break;
                    }
                    break;
                case 1: //Video
                    switch (submenuButtonIndex)
                    {
                        case 1: //ResolutionIndex
                            Resolution.Visible = false;
                            ResolutionIndicator.Hide();
                            break;
                        case 2: //FullScreen
                            rbnFullScreenOn.Hide();
                            rbnFullScreenOff.Hide();
                            break;
                    }
                    break;
                case 2: //Audio
                    switch (submenuButtonIndex)
                    {
                        case 1: //Volume
                            VolumeIndicator.Hide();
                            VolumeValueSprite.Visible = false;
                            break;
                        case 2: //Sound
                            rbnSoundOn.Hide();
                            rbnSoundOff.Hide();
                            break;
                    }
                    break;
                case 3: //Controls
                    switch (submenuButtonIndex)
                    {
                        case 1: //Mouse
                            CameraSpeedSprite.Visible = false;
                            CameraSpeedValueSprite.Visible = false;
                            CameraSpeedIndicator.Hide();
                            break;
                        case 2: //Keyboard
                            btnKeyShortcuts.Hide();
                            KeyShortcutsPanel.Visible = false;
                            KeyShortcutsPanel.Enabled = false;
                            btnKeyShortcutsPanel_Back.Hide();
                            btnKeyShortcutsPanel_Back.Enabled = false;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Resets all the options to the LastSave values, in case they were changed but the SaveSettings Options
        /// button was not pressed
        /// </summary>
        void ResetOptions()
        {
            //resets the Settings to the LastSave values
            TimeValueIndexSelected = TimeValueIndexSelected_LastSave;
            AutosaveTime.Text = TimeValues[TimeValueIndexSelected].ToString() + " min";

            DifficultyLevelCurrentlySelected = DifficultyLevel_LastSave;
            if (DifficultyLevel_LastSave == rbnEasy.Name)
                rbnEasy.Check();
            else
                if (DifficultyLevel_LastSave == rbnMedium.Name)
                    rbnMedium.Check();
                else
                    rbnHard.Check();

            ResolutionValueIndexSelected = ResolutionValueIndexSelected_LastSave;
            Resolution.Text = ResolutionValues[ResolutionValueIndexSelected].X.ToString() + " X " + ResolutionValues[ResolutionValueIndexSelected].Y.ToString();

            FullScreenCurrentState = FullScreenState_LastSave;
            if (FullScreenState_LastSave == "On")
                rbnFullScreenOn.Check();
            else
                rbnFullScreenOff.Check();

            VolumeValueSprite.FrameNumber = VolumeValue_LastSave;

            SoundCurrentState = SoundState_LastSave;
            if (SoundState_LastSave == "On")
                rbnSoundOn.Check();
            else
                rbnSoundOff.Check();

            CameraSpeedValueSprite.FrameNumber = CameraSpeed_LastState;
        }

        /// <summary>
        /// Sets the LastSave values, considering they are loaded from an xml, for a certain
        /// profile, and they will be the default ones
        /// </summary>
        public void SetLastSaveValues(CommandCenterEventArgs settings)
        {
            //refreshes the LastSave values

            TimeValueIndexSelected_LastSave = settings.TimeValueIndex;
            DifficultyLevel_LastSave = settings.DifficultyLevel;

            ResolutionValueIndexSelected_LastSave = settings.ResolutionIndex;

            FullScreenState_LastSave = settings.FullScreen;

            VolumeValue_LastSave = settings.VolumeValue;
            SoundState_LastSave = settings.SoundState;

            CameraSpeed_LastState = CameraSpeedValueSprite.FrameNumber;

            //When this method is called, the commandCenter has just loaded
            //and the Options Panel is hidden. When the Options button is clicked
            //the Show method is called and , from it, the ResetOptions method,
            //so that the graphics reflect the exact settings
        }

        public Point GetCurrentlySavedResolution()
        {
            return ResolutionValues[ResolutionValueIndexSelected_LastSave];
        }

        public int  GetCurrentlySavedAutosaveTime()
        {
            return TimeValues[TimeValueIndexSelected_LastSave];
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
