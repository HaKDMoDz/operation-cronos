using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.Input;
using Operation_Cronos.IO;
using Operation_Cronos.Display;
using System.Collections.Generic;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos
{
    public enum TopButtonPressed
    {
        Map,
        TimeGate,
        Help,
        DidYouKnow,
        Options,
        None,
    }

    public enum SubmenuDirection
    {
        Forward,
        Backward,
    }

    public class CommandCenter : InputVisualComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;
        Sprite Background;
        Sprite Map;
        Sprite RightMenu;
        Sprite Submenu1_Right;
        Sprite Submenu2_Right;

        Timer tmrSubmenu1;
        Timer tmrSubmenu2;
        int submenuStep = 71; // 3 x (SubmenuRightXClosed - SubmenuRightXOpen)
        SubmenuDirection submenu1Direction = SubmenuDirection.Forward;
        SubmenuDirection submenu2Direction = SubmenuDirection.Forward;

        int SubmenuRightXClosed = 908;
        int SubmenuRightXOpen = 695;

        CommandCenterGeneralButton btnMap;
        CommandCenterGeneralButton btnTimeGate;
        CommandCenterGeneralButton btnHelp;
        CommandCenterGeneralButton btnDidYouKnow;
        CommandCenterGeneralButton btnOptions;

        CommandCenterGeneralButton btnLogOut;
        CommandCenterGeneralButton btnExitGame;

        CommandCenterZoneList ZoneList;

        CommandCenterRightButton btnRight1;
        CommandCenterRightButton btnRight2;
        CommandCenterRightButton btnRight3;
        CommandCenterRightButton btnRight4;
        CommandCenterRightButton btnRight5;
        List<CommandCenterRightButton> rightButtons;

        TimeGateUpgrade timeGateUpgrade1;
        TimeGateUpgrade timeGateUpgrade2;
        TimeGateUpgrade timeGateUpgrade3;
        TimeGateUpgrade timeGateUpgrade4;

        TimeGateUpgradeCategory timeGateUpgradeCategory1;
        TimeGateUpgradeCategory timeGateUpgradeCategory2;
        TimeGateUpgradeCategory timeGateUpgradeCategory3;
        TimeGateUpgradeCategory timeGateUpgradeCategory4;
        List<TimeGateUpgradeCategory> timeGateUpgradeCategories;

        CommandCenterOptionsPanel OptionsPanel;
        /// <summary>
        /// will save the CommandCenterEventArgs received from the OptionsPanel on 'SaveSettings Options'
        /// </summary>
        CommandCenterEventArgs OptionsEventArgs;

        CommandCenterHelpPanel HelpPanel;
        CommandCenterDidYouKnowPanel DidYouKnowPanel;

        TopButtonPressed topButtonPressed = TopButtonPressed.Map;

        /// <summary>
        /// Message shown on MouseOver, in one of the submenus, when the TimeGate button is selected
        /// from the Top Menu
        /// </summary>
        SpriteText TimeGate_SubmenuMessage; 
        /// <summary>
        /// Message Y value for Submenu1Right
        /// </summary>
        int TimeGate_SubmenuMessage_Y1 = 120; 
        /// <summary>
        /// Message Y value for Submenu2Right
        /// </summary>
        int TimeGate_SubmenuMessage_Y2 = 420; 

        //the Message shown in submenu2, when a zone is selected and a RightButton is clicked
        SpriteText Zone_SubmenuMessage;

        int drawOrder_Fundal = 16;
        int drawOrder_Harta = 17;
        int drawOrder_TopButtons = 18;
        int drawOrder_Zona = 19;
        int drawOrder_TimeGates = 21;
        int drawOrder_TimeGateLevels = 22;
        int drawOrder_OptionsPanel = 22;
        int drawOrder_SubmeniuDreapta = 23;
        int drawOrder_SubmeniuDreapta_Messages = 24;
        int drawOrder_MeniuDreapta = 25;

        bool isHidden = false;

        SpriteText LoggedInUser = null;
        Sprite LoggedInUserBackground;
        #endregion

        #region Properties
        public bool IsHidden
        {
            get
            {
                return isHidden;
            }
        }

        /// <summary>
        /// Returns the current settings for the options panel, time gate upgrades panel and zone parameters
        /// </summary>
        public CommandCenterEventArgs GetCurrentSettings
        {
            get
            {
                return OptionsEventArgs;
            }
        }
        #endregion

        #region Events
        public event EventHandler<CommandCenterEventArgs> OnEnterZone = delegate { };
        public event EventHandler<CommandCenterEventArgs> OnSaveGame = delegate { };
        public event EventHandler OnLogOut = delegate { };
        public event EventHandler OnExitGame = delegate { };
        public event EventHandler<CommandCenterEventArgs> OnSaveOptions = delegate { };
        public event EventHandler<CommandCenterEventArgs> OnZoneSelected = delegate { };

        private event EventHandler OnSubmenuOpened = delegate { };
        #endregion

        #region Constructors
        public CommandCenter(Game game)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));
            InitialState(game);
        }
        #endregion

        #region Method InitialState

        void InitialState(Game game)
        {
            //---TimeGate Submenu Message--
            TimeGate_SubmenuMessage = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            TimeGate_SubmenuMessage.StackOrder = drawOrder_SubmeniuDreapta_Messages;
            TimeGate_SubmenuMessage.XRelative = 750;
            TimeGate_SubmenuMessage.YRelative = TimeGate_SubmenuMessage_Y1;
            TimeGate_SubmenuMessage.Tint = Color.GhostWhite;
            TimeGate_SubmenuMessage.MaxLength = 500;
            TimeGate_SubmenuMessage.Text = "None";//text used only for initialization, needed to measure text height
            AddChild(TimeGate_SubmenuMessage);

            TimeGate_SubmenuMessage.Visible = false;
            //-----------------------------

            //---Zone Submenu Message--
            Zone_SubmenuMessage = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            Zone_SubmenuMessage.StackOrder = drawOrder_SubmeniuDreapta_Messages;
            Zone_SubmenuMessage.XRelative = 750;
            Zone_SubmenuMessage.YRelative = TimeGate_SubmenuMessage_Y2;
            Zone_SubmenuMessage.Tint = Color.White;
            Zone_SubmenuMessage.MaxLength = 500;
            Zone_SubmenuMessage.Text = "";
            AddChild(Zone_SubmenuMessage);

            Zone_SubmenuMessage.Visible = false;
            //-----------------------------

            //---Submenu Timers-------
            tmrSubmenu1 = new Timer(game);
            tmrSubmenu1.IntervalType = TimerIntervalType.Miliseconds;
            tmrSubmenu1.Interval = 80;
            tmrSubmenu1.OnTick += new EventHandler(tmrSubmenu1_OnTick);

            tmrSubmenu2 = new Timer(game);
            tmrSubmenu2.IntervalType = TimerIntervalType.Miliseconds;
            tmrSubmenu2.Interval = 80;
            tmrSubmenu2.OnTick += new EventHandler(tmrSubmenu2_OnTick);
            //-----------------------

            //TimeGate_SubmenuMessage and the Submenu Timers needed to be instantiated here for they are used in 
            //event and methods of some of the objects that follow


            //---Bg and Map----------
            Background = new Sprite(game, graphicsCollection.GetPack("Background"));
            Background.StackOrder = drawOrder_Fundal;
            AddChild(Background);

            Map = new Sprite(game, graphicsCollection.GetPack("WorldMap"));
            Map.StackOrder = drawOrder_Harta;
            Map.Visible = false;//hidden
            AddChild(Map);
            //-----------------------

            RightMenu = new Sprite(game, graphicsCollection.GetPack("RightMenu"));
            RightMenu.XRelative = 935;
            RightMenu.YRelative = 0;
            RightMenu.StackOrder = drawOrder_MeniuDreapta;
            AddChild(RightMenu);

            //-----Submenus---------
            Submenu1_Right = new Sprite(game, graphicsCollection.GetPack("RightSubmenu"));
            Submenu1_Right.XRelative = SubmenuRightXClosed;
            Submenu1_Right.YRelative = 90;
            Submenu1_Right.StackOrder = drawOrder_SubmeniuDreapta;
            AddChild(Submenu1_Right);

            Submenu2_Right = new Sprite(game, graphicsCollection.GetPack("RightSubmenu"));
            Submenu2_Right.XRelative = SubmenuRightXClosed;
            Submenu2_Right.YRelative = 390;
            Submenu2_Right.StackOrder = drawOrder_SubmeniuDreapta;
            AddChild(Submenu2_Right);

            this.OnSubmenuOpened += new EventHandler(Do_OnSubmenuOpened);
            //-------------------------


            //-----Top Buttons + LogOut and ExitGame---------
            btnMap = new CommandCenterGeneralButton(game,new Sprite(game,graphicsCollection.GetPack("Map")),drawOrder_TopButtons);
            btnMap.Position = new Point(50,38);
            btnMap.OnPress += new EventHandler<ButtonEventArgs>(Do_MapOnPress);
            AddChild(btnMap);

            btnTimeGate = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("TimeGate")), drawOrder_TopButtons);
            btnTimeGate.Position = new Point(182,38);
            btnTimeGate.OnPress += new EventHandler<ButtonEventArgs>(Do_TimeGateOnPress);
            AddChild(btnTimeGate);

            btnHelp = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("Help")), drawOrder_TopButtons);
            btnHelp.Position = new Point(383, 38);
            btnHelp.OnPress += new EventHandler<ButtonEventArgs>(Do_HelpOnPress);
            AddChild(btnHelp);

            btnDidYouKnow = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnow")), drawOrder_TopButtons);
            btnDidYouKnow.Position = new Point(525, 38);
            btnDidYouKnow.OnPress += new EventHandler<ButtonEventArgs>(Do_DidYouKNowOnPress);
            AddChild(btnDidYouKnow);

            btnOptions = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("OptionsCC")), drawOrder_TopButtons);
            btnOptions.Position = new Point(770, 38);
            btnOptions.OnPress += new EventHandler<ButtonEventArgs>(Do_OptionsOnPress);
            AddChild(btnOptions);

            btnLogOut = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("LogOut")), drawOrder_MeniuDreapta);
            btnLogOut.Position = new Point(955, 170);
            btnLogOut.OnRelease += new EventHandler<ButtonEventArgs>(Do_LogOutOnRelease);
            AddChild(btnLogOut);

            btnExitGame = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("ExitGame")), drawOrder_MeniuDreapta);
            btnExitGame.Position = new Point(955, 240);
            btnExitGame.OnRelease += new EventHandler<ButtonEventArgs>(Do_ExitGameOnRelease);
            AddChild(btnExitGame);
            //------------------------


            //-----Zone List-------
            ZoneList = new CommandCenterZoneList(game, drawOrder_Zona);
            ZoneList.OnNewZoneActivated += new EventHandler<CommandCenterEventArgs>(Do_NewZoneActivated);
            ZoneList.OnZoneDeactivated += new EventHandler<CommandCenterEventArgs>(Do_ZoneDeactivated);
            AddChild(ZoneList);
            //-----------------------

            //-----Right Buttons-----
            rightButtons = new List<CommandCenterRightButton>();

            btnRight1 = new CommandCenterRightButton(game, new Sprite(game, graphicsCollection.GetPack("RightMenuButton")), drawOrder_MeniuDreapta);
            btnRight1.Position = new Point(943, 390);
            btnRight1.Text = "Description";
            btnRight1.TextPosition = new Point(950, 402);
            btnRight1.OnPress += new EventHandler<ButtonEventArgs>(Do_RightButtonPressed);
            rightButtons.Add(btnRight1);
            AddChild(btnRight1);

            btnRight2 = new CommandCenterRightButton(game, new Sprite(game, graphicsCollection.GetPack("RightMenuButton")), drawOrder_MeniuDreapta);
            btnRight2.Position = new Point(943, 447);
            btnRight2.Text = "Mission\nbriefing";
            btnRight2.TextPosition = new Point(960, 452);
            btnRight2.OnPress += new EventHandler<ButtonEventArgs>(Do_RightButtonPressed);
            rightButtons.Add(btnRight2);
            AddChild(btnRight2);

            btnRight3 = new CommandCenterRightButton(game, new Sprite(game, graphicsCollection.GetPack("RightMenuButton")), drawOrder_MeniuDreapta);
            btnRight3.Position = new Point(943, 504);
            btnRight3.Text = "Rewards";
            btnRight3.TextPosition = new Point(957, 516);
            btnRight3.OnPress += new EventHandler<ButtonEventArgs>(Do_RightButtonPressed);
            rightButtons.Add(btnRight3);
            AddChild(btnRight3);

            btnRight4 = new CommandCenterRightButton(game, new Sprite(game, graphicsCollection.GetPack("RightMenuButton")), drawOrder_MeniuDreapta);
            btnRight4.Position = new Point(943, 561);
            btnRight4.Text = "Parameters";
            btnRight4.TextPosition = new Point(951, 573);
            btnRight4.OnPress += new EventHandler<ButtonEventArgs>(Do_RightButtonPressed);
            rightButtons.Add(btnRight4);
            AddChild(btnRight4);

            btnRight5 = new CommandCenterRightButton(game, new Sprite(game, graphicsCollection.GetPack("RightMenuButton")), drawOrder_MeniuDreapta);
            btnRight5.Position = new Point(943, 618);
            btnRight5.Text = "Enter  zone";
            btnRight5.TextPosition = new Point(954, 630);
            btnRight5.OnPress += new EventHandler<ButtonEventArgs>(Do_RightButtonPressed);
            rightButtons.Add(btnRight5);
            AddChild(btnRight5);

            DisableRightButtons();
            //-----------------------

            //---Options Panel-------
            OptionsPanel = new CommandCenterOptionsPanel(game, drawOrder_OptionsPanel);
            OptionsPanel.Hide();
                                   
            //**In case the OptionsPanel.OnSaveOptions Event is not launched 
            //(the player does not change the default settings)
            //although these settings are changed from the GENERAL settings (OptionsPanel.DefaultOptions)
            //to the loaded profile's PARTICULAR default settings by calling the 
            //SetGameOptions method when a profile is loaded
            OptionsEventArgs = OptionsPanel.DefaultOptions;
            int[] initialLevels = new int[] {0,0,0,0};
            OptionsEventArgs.TimeGateUpgradeLevels = initialLevels;
            //**
            OptionsPanel.OnSaveGame += new EventHandler(OptionsPanel_OnSaveGame);
            OptionsPanel.OnSaveOptions += new EventHandler<CommandCenterEventArgs>(OptionsPanel_OnSaveOptions);
            //OptionsPanel.OnLogOut += new EventHandler(OptionsPanel_OnExitToMainMenu);
            //OptionsPanel.OnExitGame += new EventHandler(OptionsPanel_OnExitGame);
            AddChild(OptionsPanel);
            //-----------------------

            //-----TimeGateUpgrade---
            timeGateUpgrade1 = new TimeGateUpgrade(game, new Sprite(game, graphicsCollection.GetPack("TimeGate1")), drawOrder_TimeGates);
            timeGateUpgrade1.Position = new Point(100,100);
            timeGateUpgrade1.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade1_MouseOver);
            timeGateUpgrade1.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade1_MouseLeave);
            AddChild(timeGateUpgrade1);

            timeGateUpgrade2 = new TimeGateUpgrade(game, new Sprite(game, graphicsCollection.GetPack("TimeGate2")), drawOrder_TimeGates);
            timeGateUpgrade2.Position = new Point(500,100);
            timeGateUpgrade2.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade2_MouseOver);
            timeGateUpgrade2.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade2_MouseLeave);
            AddChild(timeGateUpgrade2);

            timeGateUpgrade3 = new TimeGateUpgrade(game, new Sprite(game, graphicsCollection.GetPack("TimeGate3")), drawOrder_TimeGates);
            timeGateUpgrade3.Position = new Point(100,420);
            timeGateUpgrade3.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade3_MouseOver);
            timeGateUpgrade3.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade3_MouseLeave);
            AddChild(timeGateUpgrade3);

            timeGateUpgrade4 = new TimeGateUpgrade(game, new Sprite(game, graphicsCollection.GetPack("TimeGate4")), drawOrder_TimeGates);
            timeGateUpgrade4.Position = new Point(500,420);
            timeGateUpgrade4.OnMouseOver += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade4_MouseOver);
            timeGateUpgrade4.OnMouseLeave += new EventHandler<ButtonEventArgs>(Do_TimeGateUpgrade4_MouseLeave);
            AddChild(timeGateUpgrade4);

            timeGateUpgradeCategories = new List<TimeGateUpgradeCategory>();

            timeGateUpgradeCategory1 = new TimeGateUpgradeCategory(game, drawOrder_TimeGateLevels, new Point[4] { new Point(100, 100), new Point(295, 100), new Point(100, 285), new Point(295, 285) });
            //initially first level Unlocked
            timeGateUpgradeCategory1.SetLevelsStatus(TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
            timeGateUpgradeCategory1.OnLevelBought +=new EventHandler<ButtonEventArgs>(timeGateUpgradeCategory1_OnLevelBought);
            timeGateUpgradeCategory1.OnLastLevelBought +=new EventHandler<ButtonEventArgs>(timeGateUpgradeCategory1_OnLastLevelBought);
            timeGateUpgradeCategories.Add(timeGateUpgradeCategory1);
            AddChild(timeGateUpgradeCategory1);

            timeGateUpgradeCategory2 = new TimeGateUpgradeCategory(game, drawOrder_TimeGateLevels, new Point[4] { new Point(500, 100), new Point(695, 100), new Point(500, 285), new Point(695, 285) });
            //initially all levels locked
            timeGateUpgradeCategory2.OnLevelBought += new EventHandler<ButtonEventArgs>(timeGateUpgradeCategory2_OnLevelBought);
            timeGateUpgradeCategory2.SetLevelsStatus(TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
            timeGateUpgradeCategories.Add(timeGateUpgradeCategory2);
            AddChild(timeGateUpgradeCategory2);

            timeGateUpgradeCategory3 = new TimeGateUpgradeCategory(game, drawOrder_TimeGateLevels, new Point[4] { new Point(100, 419), new Point(295, 419), new Point(100, 605), new Point(295, 605) });
            //initially first level unlocked
            timeGateUpgradeCategory3.SetLevelsStatus(TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
            timeGateUpgradeCategory3.OnLevelBought += new EventHandler<ButtonEventArgs>(timeGateUpgradeCategory3_OnLevelBought);
            timeGateUpgradeCategory3.OnLastLevelBought += new EventHandler<ButtonEventArgs>(timeGateUpgradeCategory3_OnLastLevelBought);
            timeGateUpgradeCategories.Add(timeGateUpgradeCategory3);
            AddChild(timeGateUpgradeCategory3);

            timeGateUpgradeCategory4 = new TimeGateUpgradeCategory(game, drawOrder_TimeGateLevels, new Point[4] { new Point(500, 419), new Point(695, 419), new Point(500, 605), new Point(695, 605) });
            //initially all levels locked
            timeGateUpgradeCategory4.SetLevelsStatus(TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
            timeGateUpgradeCategory4.OnLevelBought += new EventHandler<ButtonEventArgs>(timeGateUpgradeCategory4_OnLevelBought);         
            timeGateUpgradeCategories.Add(timeGateUpgradeCategory4);
            AddChild(timeGateUpgradeCategory4);
           
            HideTimeGateUpgrades();
            //-----------------------

            //---Help Panel-------
            HelpPanel = new CommandCenterHelpPanel(game, drawOrder_OptionsPanel);
            HelpPanel.Hide();

            AddChild(HelpPanel);
            //-----------------------


            //---DidYouKnow Panel-----
            DidYouKnowPanel = new CommandCenterDidYouKnowPanel(game, drawOrder_OptionsPanel);
            DidYouKnowPanel.Hide();

            AddChild(DidYouKnowPanel);
            //------------------------

            //---Logged in as---------
            LoggedInUserBackground = new Sprite(game, graphicsCollection.GetPack("login"));
            LoggedInUserBackground.Visible = false;
            LoggedInUserBackground.StackOrder = drawOrder_MeniuDreapta;
            LoggedInUserBackground.XRelative = 25;
            LoggedInUserBackground.YRelative = 665;
            AddChild(LoggedInUserBackground);

            LoggedInUser = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            LoggedInUser.Visible = false;
            LoggedInUser.StackOrder = drawOrder_MeniuDreapta+1;
            LoggedInUser.XRelative = 30;
            LoggedInUser.YRelative = 672;
            AddChild(LoggedInUser);
            //------------------------

            Game.Services.AddService(typeof(CommandCenter), this);
        }
        #endregion

        #region Event Handlers

            #region Top Buttons
            void Do_MapOnPress(object sender, ButtonEventArgs e)
            {
                //show Map area
                Map.Visible = true;
                ShowZones();

                //hide TimeGate area
                HideTimeGateUpgrades();
                CloseSubmenus();

                //hide Options Panel
                OptionsPanel.Hide();

                //hide Help Panel
                HelpPanel.Hide();

                //hide DidYouKnowPanel
                DidYouKnowPanel.Hide();

                LoggedInUser.Visible = true;
                LoggedInUserBackground.Visible = true;

                topButtonPressed = TopButtonPressed.Map;
                ReleasseAllTopButtons();
            }

            void Do_TimeGateOnPress(object sender, ButtonEventArgs e)
            {
                //hide Map area
                Map.Visible = false;
                HideZones();
                DisableRightButtons();
                CloseSubmenus();

                //show TimeGate area
                ShowTimeGateUpgrades();

                //hide Options Panel
                OptionsPanel.Hide();

                //hide Help Panel
                HelpPanel.Hide();

                //hide DidYouKnowPanel
                DidYouKnowPanel.Hide();

                topButtonPressed = TopButtonPressed.TimeGate;
                ReleasseAllTopButtons();

                LoggedInUser.Visible = false;
                LoggedInUserBackground.Visible = false;
            }

            void Do_HelpOnPress(object sender, ButtonEventArgs e)
            {
                //show Help Panel
                HelpPanel.Show();

                //hide TimeGate area
                HideTimeGateUpgrades();

                CloseSubmenus();

                //hide Map area
                Map.Visible = false;
                HideZones();
                DisableRightButtons();

                //hide Options Panel
                OptionsPanel.Hide();

                //hide DidYouKnowPanel
                DidYouKnowPanel.Hide();

                topButtonPressed = TopButtonPressed.Help;
                ReleasseAllTopButtons();

                LoggedInUser.Visible = false;
                LoggedInUserBackground.Visible = false;
            }

            void Do_DidYouKNowOnPress(object sender, ButtonEventArgs e)
            {
                DidYouKnowPanel.Show();

                //hide TimeGate area
                HideTimeGateUpgrades();

                CloseSubmenus();

                //hide Map area
                Map.Visible = false;
                HideZones();
                DisableRightButtons();

                //hide Options Panel
                OptionsPanel.Hide();

                //hide Help Panel
                HelpPanel.Hide();

                topButtonPressed = TopButtonPressed.DidYouKnow;
                ReleasseAllTopButtons();

                LoggedInUser.Visible = false;
                LoggedInUserBackground.Visible = false;
            }

            void Do_OptionsOnPress(object sender, ButtonEventArgs e)
            {
                //hide TimeGate area
                HideTimeGateUpgrades();

                CloseSubmenus();

                //hide Map area
                Map.Visible = false;
                HideZones();
                DisableRightButtons();

                //show Options Panel
                OptionsPanel.Show();

                //hide Help Panel
                HelpPanel.Hide();

                //hide DidYouKnowPanel
                DidYouKnowPanel.Hide();

                topButtonPressed = TopButtonPressed.Options;
                ReleasseAllTopButtons();

                LoggedInUser.Visible = false;
                LoggedInUserBackground.Visible = false;
            }
            #endregion

            #region LogOut - ExitGame Buttons
            void Do_LogOutOnRelease(object sender, EventArgs e)
            {
                btnLogOut.ReleaseButton();
                OnLogOut(this, e);
            }

            void Do_ExitGameOnRelease(object sender, EventArgs e)
            {
                OnExitGame(this, e);
            }
            #endregion

            #region Options Panel
            void OptionsPanel_OnSaveGame(object sender, EventArgs e)
            {
                OnSaveGame(this, OptionsEventArgs);
            }

            void OptionsPanel_OnSaveOptions(object sender, CommandCenterEventArgs e)
            {
                //saves here the CommandCenterEventArgs received when the 'SaveSettings Options' button
                //was pressed
                //OptionsEventArgs were initially set to be OptionsPanel.DefaultOptions;

                SetOptionsEventArgs(e, false);

                //launches the OnSaveOptions event, for the Profile Manager to save the options
                OnSaveOptions(this, e);
            }
            #endregion

            #region Right Buttons

            void Do_RightButtonPressed(object sender, ButtonEventArgs e)
            {
                if (ZoneList.ZoneIsSelected)
                {                    
                    for (int i = 0; i < rightButtons.Count; i++)
                    {
                        if (((CommandCenterRightButton)sender) != rightButtons[i])
                            //all the buttons (except for the one that launched the event) are unpressed
                            rightButtons[i].UnpressButton();
                        else //found the button that lanched the event
                        {
                            if (i == 0) //first button
                            {
                                Zone_SubmenuMessage.Text = ZoneList.SelectedZone.DescriptionString;
                                if (!ZoneList.SelectedZone.Locked)
                                    Zone_SubmenuMessage.SplitTextToRows(Submenu1_Right.Width - 80);
                                Zone_SubmenuMessage.Visible = true;
                            }
                            if (i == 1) //second button
                            {
                                Zone_SubmenuMessage.Text = ZoneList.SelectedZone.MissionBriefingString;
                                if (!ZoneList.SelectedZone.Locked)
                                    Zone_SubmenuMessage.SplitTextToRows(Submenu1_Right.Width - 80);
                                Zone_SubmenuMessage.Visible = true;
                            }
                            if (i == 2) //third button
                            {
                                Zone_SubmenuMessage.Text = ZoneList.SelectedZone.RewardsString;
                                if (!ZoneList.SelectedZone.Locked)
                                    Zone_SubmenuMessage.SplitTextToRows(Submenu1_Right.Width - 80);
                                Zone_SubmenuMessage.Visible = true;
                            }
                            if (i == 3) //forth button
                            {
                                Zone_SubmenuMessage.Text = ZoneList.SelectedZone.ParametersString;
                                //if (!ZoneList.SelectedZone.Locked)
                                //    Zone_SubmenuMessage.SplitTextToRows(Submenu1_Right.Width - 80);
                                Zone_SubmenuMessage.Visible = true;
                            }
                            if (i == 4) //fifth button == Enter Zone
                            {
                                if (!ZoneList.SelectedZone.Locked)
                                //the selected zone is not locked
                                {
                                    //Launches the OnEnterZone Event
                                    //sending the OptionsPanel setting as CommandCenterEventArgs
                                    SetOptionsEventArgs(null, false);

                                    OnEnterZone(this, OptionsEventArgs);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Zones List
            void Do_NewZoneActivated(object sender, CommandCenterEventArgs e)
            {
                OpenSubmenu2();

                //launches the event, sending the index of the selected zone
                OnZoneSelected(this, e);
            }

            void Do_ZoneDeactivated(object sender, CommandCenterEventArgs e)
            {
                CloseSubmenu2();
                DisableRightButtons();
                Zone_SubmenuMessage.Text = "";
                Zone_SubmenuMessage.Visible = false;
            }
            #endregion

            #region TimeGate Upgrades
            void Do_TimeGateUpgrade1_MouseOver(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory1.Show();
                                
                Update_TimeGateSubmenuMessage(1);
                OpenSubmenu2();
            }

            void Do_TimeGateUpgrade2_MouseOver(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory2.Show();

                Update_TimeGateSubmenuMessage(2);
                OpenSubmenu2();
            }

            void Do_TimeGateUpgrade3_MouseOver(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory3.Show();
                                
                Update_TimeGateSubmenuMessage(3);
                OpenSubmenu1();
            }

            void Do_TimeGateUpgrade4_MouseOver(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory4.Show();

                Update_TimeGateSubmenuMessage(4);
                OpenSubmenu1();
            }

            void Do_TimeGateUpgrade1_MouseLeave(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory1.Hide();

                TimeGate_SubmenuMessage.Visible = false;
                CloseSubmenu2();
            }

            void Do_TimeGateUpgrade2_MouseLeave(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory2.Hide();

                TimeGate_SubmenuMessage.Visible = false;
                CloseSubmenu2();
            }

            void Do_TimeGateUpgrade3_MouseLeave(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory3.Hide();

                TimeGate_SubmenuMessage.Visible = false;
                CloseSubmenu1();
            }

            void Do_TimeGateUpgrade4_MouseLeave(object sender, ButtonEventArgs e)
            {
                timeGateUpgradeCategory4.Hide();

                TimeGate_SubmenuMessage.Visible = false;
                CloseSubmenu1();
            }

            void timeGateUpgradeCategory1_OnLevelBought(object sender, ButtonEventArgs e)
            {
                int[] levels = OptionsEventArgs.TimeGateUpgradeLevels;
                levels[0] += 1; ;
                OptionsEventArgs.TimeGateUpgradeLevels = levels;
            }

            void timeGateUpgradeCategory2_OnLevelBought(object sender, ButtonEventArgs e)
            {
                int[] levels = OptionsEventArgs.TimeGateUpgradeLevels;
                levels[1]+=1;
                OptionsEventArgs.TimeGateUpgradeLevels = levels;
            }

            void timeGateUpgradeCategory3_OnLevelBought(object sender, ButtonEventArgs e)
            {
                int[] levels = OptionsEventArgs.TimeGateUpgradeLevels;
                levels[2]+=1;
                OptionsEventArgs.TimeGateUpgradeLevels = levels;
            }

            void timeGateUpgradeCategory4_OnLevelBought(object sender, ButtonEventArgs e)
            {
                int[] levels = OptionsEventArgs.TimeGateUpgradeLevels;
                levels[3]+=1;
                OptionsEventArgs.TimeGateUpgradeLevels = levels;
            }

            void timeGateUpgradeCategory1_OnLastLevelBought(object sender, ButtonEventArgs e)
            {
                //the first level of the next category (2) is unlocked
                timeGateUpgradeCategory2.SetLevelsStatus(TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
            }

            void timeGateUpgradeCategory3_OnLastLevelBought(object sender, ButtonEventArgs e)
            {
                //the first level of the next category (4) is unlocked
                timeGateUpgradeCategory4.SetLevelsStatus(TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
            }
            #endregion

            #region Submenus OnSubmenuOpen and Timers
            /// <summary>
            /// EventHandler for the OnSubmenuOpened Event Launched after
            /// either submenu1 or submenu2 is completly opened
            /// </summary>
            void Do_OnSubmenuOpened(object sender, EventArgs e)
            {
                switch (topButtonPressed)
                {
                    case TopButtonPressed.Map: //submenu1 was opened by clicking a zone
                        EnableRightButtons();
                        btnRight1.Press(MouseButton.LeftButton);//is initially clicked
                        break;
                    case TopButtonPressed.TimeGate://submenu1 or submenu2 was opened by MouseOver a TimeGateUpgrade Sprite
                        TimeGate_SubmenuMessage.Visible = true;
                        break;
                }
            }

            void tmrSubmenu1_OnTick(object sender, EventArgs e)
            {
                if (submenu1Direction == SubmenuDirection.Forward)
                {
                    if (Submenu1_Right.XRelative - SubmenuRightXOpen >= submenuStep)
                    //has at least one more step until opened
                    {
                        Submenu1_Right.XRelative -= submenuStep;
                        Submenu1_Right.X -= submenuStep;
                    }
                    else //submenu is in Opened positions
                    {
                        tmrSubmenu1.Stop();
                        OnSubmenuOpened(this, new EventArgs());
                    }
                }
                else //Backward
                {
                    if (SubmenuRightXClosed - Submenu1_Right.XRelative >= submenuStep)
                    //has at least one more step until opened
                    {
                        Submenu1_Right.XRelative += submenuStep;
                        Submenu1_Right.X += submenuStep;
                    }
                    else//submenu is in Closed positions
                        tmrSubmenu1.Stop();
                }
            }

            void tmrSubmenu2_OnTick(object sender, EventArgs e)
            {
                if (submenu2Direction == SubmenuDirection.Forward)
                {
                    if (Submenu2_Right.XRelative - SubmenuRightXOpen >= submenuStep)
                    //has at least one more step until opened
                    {
                        Submenu2_Right.XRelative -= submenuStep;
                        Submenu2_Right.X -= submenuStep;
                    }
                    else //submenu is in Opened positions
                    {
                        tmrSubmenu2.Stop();
                        OnSubmenuOpened(this, new EventArgs());
                    }
                }
                else //Backward
                {                    
                    if (SubmenuRightXClosed - Submenu2_Right.XRelative >= submenuStep)
                    //has at least one more step until opened
                    {
                        Submenu2_Right.XRelative += submenuStep;
                        Submenu2_Right.X += submenuStep;
                    }
                    else //submenu is in Closed positions
                        tmrSubmenu2.Stop();
                }
            }
            #endregion

        #endregion

        #region Methods

            #region General Methods
            /// <summary>
            /// Hides the MainInterface (stops drawing and dactivates it)
            /// </summary>
            public void Hide()
            {
                try
                {
                    this.Enabled = false;

                    Background.Visible = false;
                    Map.Visible = false;
                    RightMenu.Visible = false;
                    Submenu1_Right.Visible = false;
                    Submenu2_Right.Visible = false;

                    //closed the submenus and hides the messages made to be shown on them
                    CloseSubmenus();

                    btnMap.Hide();
                    btnHelp.Hide();
                    btnOptions.Hide();
                    btnTimeGate.Hide();
                    btnDidYouKnow.Hide();

                    btnLogOut.Hide();
                    btnExitGame.Hide();
                    
                    OptionsPanel.Hide();
                    HelpPanel.Hide();
                    DidYouKnowPanel.Hide();
                    
                    DisableRightButtons();
                    for (int i = 0; i < rightButtons.Count; i++)
                    {
                        rightButtons[i].Hide();
                    }
                    //this way the buttons are set as enabled=false and visible=false

                    HideZones();
                    HideTimeGateUpgrades();

                    isHidden = true;

                    LoggedInUser.Visible = false;
                    LoggedInUserBackground.Visible = false;
                }
                catch
                {
                }
            }

            /// <summary>
            /// Shows the MainInterface (resets it and starts drawing it)
            /// </summary>
            public void Show()
            {
                try
                {
                    this.Enabled = true;

                    Background.Visible = true;
                    RightMenu.Visible = true;
                    Submenu1_Right.Visible = true;
                    Submenu2_Right.Visible = true;

                    btnMap.Show();
                    btnHelp.Show();
                    btnOptions.Show();
                    btnTimeGate.Show();
                    btnDidYouKnow.Show();

                    btnLogOut.Show();
                    btnExitGame.Show();

                    //the Right buttons are shown but are disabled (as no zone is yet pressed)
                    for (int i = 0; i < rightButtons.Count; i++)
                    {
                        rightButtons[i].Show();
                    }
                    DisableRightButtons();

                    isHidden = false;

                    btnMap.Press(MouseButton.LeftButton);
                }
                catch
                {
                }
            }

            public void SetCurrentUser(string user)
            {
                LoggedInUser.Text = user;
                while (LoggedInUser.Width + LoggedInUser.XRelative < LoggedInUserBackground.Width)
                {
                    LoggedInUser.Text = " " + LoggedInUser.Text;
                }
            }
            #endregion

            #region Zones
            void HideZones()
            {
                ZoneList.Hide();
            }

            void ShowZones()
            {
                ZoneList.Show();
            }

            /// <summary>
            /// Sets the text displayed after clicking the Parameters Button for a zone
            /// </summary>
            public void SetZoneParameters(List<CommandCenterEventArgs> zonesParameters)
            {
                ZoneList.SetZoneParameters(zonesParameters);
            }
            #endregion

            #region TimeGate
            void HideTimeGateUpgrades()
            {
                timeGateUpgrade1.Hide();
                timeGateUpgrade2.Hide();
                timeGateUpgrade3.Hide();
                timeGateUpgrade4.Hide();

                Do_TimeGateUpgrade1_MouseLeave(this, new ButtonEventArgs());
                Do_TimeGateUpgrade2_MouseLeave(this, new ButtonEventArgs());
                Do_TimeGateUpgrade3_MouseLeave(this, new ButtonEventArgs());
                Do_TimeGateUpgrade4_MouseLeave(this, new ButtonEventArgs());
            }

            void ShowTimeGateUpgrades()
            {
                timeGateUpgrade1.Show();
                timeGateUpgrade2.Show();
                timeGateUpgrade3.Show();
                timeGateUpgrade4.Show();
            }

            //called from ProfileManager
            public void SetTimeGateLevels(CommandCenterEventArgs e)
            {
                int[] levels = new int[4];
                levels = e.TimeGateUpgradeLevels;
                SetOptionsEventArgs(e, true);//true == only time gate levels will be modified

                for (int i = 0; i < 4; i++) //all upgrade categories
                {
                    switch (levels[i])//Time Gate Upgrades Category i
                    {
                        case 0://no level unlocked
                            timeGateUpgradeCategories[i].SetLevelsStatus(TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
                            break;
                        case 1://level 1 unlocked
                            timeGateUpgradeCategories[i].SetLevelsStatus(TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
                            break;
                        case 2://level 2 unlocked, level 1 bought
                            timeGateUpgradeCategories[i].SetLevelsStatus(TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked, TimeGateUpgradeStatus.Locked);
                            break;
                        case 3://level 3 unlocked, levels 1 and 2 bought
                            timeGateUpgradeCategories[i].SetLevelsStatus(TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Unlocked, TimeGateUpgradeStatus.Locked);
                            break;
                        case 4://level 4 unlocked, levels 1, 2 and 3 bought
                            timeGateUpgradeCategories[i].SetLevelsStatus(TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Unlocked);
                            break;
                        case 5://all levels bought
                            timeGateUpgradeCategories[i].SetLevelsStatus(TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Bought, TimeGateUpgradeStatus.Bought);
                            break;
                    }
                }                
            }
            #endregion

            #region TimeGate Message
            void Update_TimeGateSubmenuMessage(int TimeGateConsidered)
            {
                switch (TimeGateConsidered)
                {
                    case 1:
                        TimeGate_SubmenuMessage.Text = "\n\n  Allows you to travel back\n  in time for a maximum of\n  "+((int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level4).ToString()+" years, by gradually\n  unlocking the next 4 levels:\n";
                        TimeGate_SubmenuMessage.Text += "\n        I.   " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level1).ToString()+" years";
                        TimeGate_SubmenuMessage.Text += "\n        II.  " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level2).ToString() + " years";
                        TimeGate_SubmenuMessage.Text += "\n        III. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level3).ToString() + " years";
                        TimeGate_SubmenuMessage.Text += "\n        IV. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level4).ToString() + " years";
                        TimeGate_SubmenuMessage.YRelative = TimeGate_SubmenuMessage_Y2;
                        TimeGate_SubmenuMessage.Y = TimeGate_SubmenuMessage_Y2;
                        break;
                    case 2:
                        TimeGate_SubmenuMessage.Text = "\n\n  Allows you to travel back\n  in time for a maximum of\n  "+((int)TimeGateUpgradeLevelsValues.UpgradeCategory2_Level4).ToString()+" years, by gradually\n  unlocking the next 4 levels:\n";
                        TimeGate_SubmenuMessage.Text += "\n        I.   " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory2_Level1).ToString() + " years";
                        TimeGate_SubmenuMessage.Text += "\n        II.  " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory2_Level2).ToString() + " years";
                        TimeGate_SubmenuMessage.Text += "\n        III. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory2_Level3).ToString() + " years";
                        TimeGate_SubmenuMessage.Text += "\n        IV. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory2_Level4).ToString() + " years";
                        TimeGate_SubmenuMessage.YRelative = TimeGate_SubmenuMessage_Y2;
                        TimeGate_SubmenuMessage.Y = TimeGate_SubmenuMessage_Y2;
                        break;
                    case 3:
                        TimeGate_SubmenuMessage.Text = "\n\n Allows you to receive a bonus\n representing a percent of each\n zone's economical income,\n by gradually unlocking\n the next 4 levels:\n";
                        TimeGate_SubmenuMessage.Text += "\n        I.   " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory3_Level1).ToString() + " %";
                        TimeGate_SubmenuMessage.Text += "\n        II.  " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory3_Level2).ToString() + " %";
                        TimeGate_SubmenuMessage.Text += "\n        III. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory3_Level3).ToString() + " %";
                        TimeGate_SubmenuMessage.Text += "\n        IV. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory3_Level4).ToString() + " %";               
                        TimeGate_SubmenuMessage.YRelative = TimeGate_SubmenuMessage_Y1;
                        TimeGate_SubmenuMessage.Y = TimeGate_SubmenuMessage_Y1;
                        break;
                    case 4: 
                        TimeGate_SubmenuMessage.Text = "\n\n Allows you to receive a bonus\n representing a percent of each\n zone's economical income,\n by gradually unlocking\n the next 4 levels:\n";
                        TimeGate_SubmenuMessage.Text += "\n        I.   " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory4_Level1).ToString() + " %";
                        TimeGate_SubmenuMessage.Text += "\n        II.  " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory4_Level2).ToString() + " %";
                        TimeGate_SubmenuMessage.Text += "\n        III. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory4_Level3).ToString() + " %";
                        TimeGate_SubmenuMessage.Text += "\n        IV. " + ((int)TimeGateUpgradeLevelsValues.UpgradeCategory4_Level4).ToString() + " %";               
                        TimeGate_SubmenuMessage.YRelative = TimeGate_SubmenuMessage_Y1;
                        TimeGate_SubmenuMessage.Y = TimeGate_SubmenuMessage_Y1;
                        break;
                }
            }
            #endregion

            #region Top Buttons
            /// <summary>
            /// When another TopButton is pressed, the others are releassed
            /// </summary>
            public void ReleasseAllTopButtons()
            {
                switch (topButtonPressed)
                {
                    case TopButtonPressed.Map:
                        btnOptions.ReleaseButton();
                        btnTimeGate.ReleaseButton();
                        btnDidYouKnow.ReleaseButton();
                        btnHelp.ReleaseButton();
                        break;
                    case TopButtonPressed.Help:
                        btnMap.ReleaseButton();
                        btnOptions.ReleaseButton();
                        btnTimeGate.ReleaseButton();
                        btnDidYouKnow.ReleaseButton();
                        break;
                    case TopButtonPressed.Options:
                        btnMap.ReleaseButton();
                        btnTimeGate.ReleaseButton();
                        btnDidYouKnow.ReleaseButton();
                        btnHelp.ReleaseButton();
                        break;
                    case TopButtonPressed.TimeGate:
                        btnMap.ReleaseButton();
                        btnOptions.ReleaseButton();
                        btnDidYouKnow.ReleaseButton();
                        btnHelp.ReleaseButton();
                        break;
                    case TopButtonPressed.DidYouKnow:
                        btnMap.ReleaseButton();
                        btnTimeGate.ReleaseButton();
                        btnOptions.ReleaseButton();
                        btnHelp.ReleaseButton();
                        break;
                }
            }
            #endregion

            #region Submenus
            /// <summary>
            /// Closes the Submenus directly, without any fade-out effect
            /// Also hides all the messages that are made to be displayed on the Submenus
            /// </summary>
            void CloseSubmenus()
            {
                Submenu1_Right.XRelative = SubmenuRightXClosed;
                Submenu1_Right.X = SubmenuRightXClosed;

                Submenu2_Right.XRelative = SubmenuRightXClosed;
                Submenu2_Right.X = SubmenuRightXClosed;

                TimeGate_SubmenuMessage.Visible = false;
                Zone_SubmenuMessage.Visible = false;
            }
            
            /// <summary>
            /// Opens Submenu1 with fade-in effect
            /// </summary>
            void OpenSubmenu1()
            {
                if (tmrSubmenu1.Started == false)
                    tmrSubmenu1.Start();
                submenu1Direction = SubmenuDirection.Forward;
            }

            /// <summary>
            /// Closes Submenu1 with fade-out effect
            /// </summary>
            void CloseSubmenu1()
            {
                if (tmrSubmenu1.Started == false)
                    tmrSubmenu1.Start();
                submenu1Direction = SubmenuDirection.Backward;
            }

            /// <summary>
            /// Opens Submenu2 with fade-in effect
            /// </summary>
            void OpenSubmenu2()
            {
                if (tmrSubmenu2.Started == false)
                    tmrSubmenu2.Start();
                submenu2Direction = SubmenuDirection.Forward;
            }

            /// <summary>
            /// Closes Submenu2 with fade-out effect
            /// </summary>
            void CloseSubmenu2()
            {
                if (tmrSubmenu2.Started == false)
                    tmrSubmenu2.Start();
                submenu2Direction = SubmenuDirection.Backward;
            }
            #endregion

            #region Right Buttons
            /// <summary>
            /// Enables the buttons on the right menu; the buttons will start launching click events
            /// </summary>
            void EnableRightButtons()
            {
                for (int i=0;i<rightButtons.Count;i++)
                {
                    rightButtons[i].Enable();
                }
            }

            /// <summary>
            /// Disables the buttons on the right menu; the buttons will stop launching click events
            /// </summary>
            void DisableRightButtons()
            {
                for (int i=0;i<rightButtons.Count;i++)
                {
                    rightButtons[i].Disable();
                }
            }
            #endregion           

            #region Options Panel
            //called from ProfileManager
            public void SetGameOptions(CommandCenterEventArgs e)
            {
                //sets the 'default'/LastSave options, when the profile is loaded
                //(the default settings for the profile)
                OptionsEventArgs = e;                
                OptionsPanel.SetLastSaveValues(e);

                OptionsEventArgs.ResolutionValue = OptionsPanel.GetCurrentlySavedResolution();
                OptionsEventArgs.AutosaveTimeValue = OptionsPanel.GetCurrentlySavedAutosaveTime();
                                
            }
            #endregion

            #region Options Event Args
            private void SetOptionsEventArgs(CommandCenterEventArgs e, bool ChangeOnlyTimeGateLevels)
            {
                CommandCenterEventArgs OptionsTemp = OptionsEventArgs;
                if (e != null) 
                {
                    if (ChangeOnlyTimeGateLevels == true)
                    {
                        OptionsEventArgs.TimeGateUpgradeLevels = e.TimeGateUpgradeLevels;
                    }
                    else//change only options settings
                    {
                        OptionsEventArgs = e;
                        OptionsEventArgs.TimeGateUpgradeLevels = OptionsTemp.TimeGateUpgradeLevels;
                    }
                }
                else//e == null -> only update ZoneName
                {
                    OptionsEventArgs.ZoneName = ZoneList.SelectedZone.Name;
                }

                OptionsEventArgs.ResolutionValue = OptionsPanel.GetCurrentlySavedResolution();
                OptionsEventArgs.AutosaveTimeValue = OptionsPanel.GetCurrentlySavedAutosaveTime();
            }
            #endregion

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
