using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.Display;
using Operation_Cronos.IO;
using Operation_Cronos.Profiles;
using Operation_Cronos.Sound;

namespace Operation_Cronos
{
    public enum MainMenuEvents
    {
        NewGame,
        LoadGame,
        Credits,
        Exit,
        None,
    }

    public class MainMenu : Microsoft.Xna.Framework.GameComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;
        Sprite ImagineStatica;
        Sprite ActivareEfectPortal;
        Sprite AnimatieBrateRobotice;
        Sprite AnimatieEfectPortal;
        Sprite AnimatieMonitoareSecundare;
        Sprite monitor_1;
        Sprite monitor_2;
        Sprite monitor_3;
        Sprite monitor_4;
        Sprite AnimatieScari;
        Sprite AnimatieStalpDreapta;
        Sprite AnimatieStalpStanga;
        Sprite AnimatieVentilatoare;
        Sprite MonitorPrincipal;
        Sprite Slot_1;
        Sprite Slot_2;
        Sprite Slot_3;
        Sprite Portal;
        Sprite LoadScreenFrame;
        Sprite NewGameScreenUserName;

        MainMenuButton btnNewGame;
        MainMenuButton btnLoadGame;
        MainMenuButton btnCredits;
        MainMenuButton btnExit;
        MainMenuButton btnLoadScreenLoad;
        MainMenuButton btnLoadScreenDelete;
        MainMenuButton btnBack;
        MainMenuButton btnNewGameScreenStart;

        MainMenuScrollbar LoadScreenScrollbar;
        MainMenuUser User1;
        MainMenuUser User2;
        MainMenuUser User3;

        string SelectedUser = "";
        List<string> usersList;
        int UserIntervalStart = 0;

        public MainMenuInputBox inputNewGameUserName;

        int drawOrder_ImagineStatica = 16;
        int drawOrder_AnimatieVentilatoare = 17;
        int drawOrder_AnimatieStalpDreapta = 18;
        int drawOrder_ActivareEfectPortal = 19;
        int drawOrder_AnimatieEfectPortal = 20;
        int drawOrder_Portal = 21;
        int drawOrder_AnimatieStalpStanga = 22;
        int drawOrder_AnimatieBrateRobotice = 23;
        int drawOrder_AnimatieScari = 24;
        int drawOrder_AnimatieMonitoareSecundare = 25;
        int drawOrder_monitors = 26;
        int drawOrder_Slots = 30;
        int drawOrder_MonitorPrincipal = 31;
        int drawOrder_Buttons = 33;

        int speed_AnimatieVentilatoare = 25;
        int speed_AnimatieScari = 20;
        int speed_ActivareEfectPortal = 50;//aprox 3 x stairsSpeed
        int speed_AnimatieEfectPortal = 20;
        int speed_AnimatieMonitoareSecundare = 30;
        int speed_AnimatieBrateRobotice = 25;
        int speed_AnimatieStalpi = 25;//must be the same with speed_AnimatieBrateRobotice

        Random randomSlot;

        Timer ExitTimer;

        //the animations will Rollback on MouseLeave
        //but if buttons are clicked and then the Mouse Leaves, the animations will not Rollback
        bool AnimatieNewGame_AllowRollback = true;
        bool AnimatieExit_AllowRollback = true;
        bool AnimatieCredits_AllowRollback = true;
        bool AnimatieLoad_AllowRollback = true;

        MainMenuEvents ClickEventOccured = MainMenuEvents.None;

        SpriteText txtCreditsCronosTeam;
        SpriteText txtCreditsMembers;

        //will be a reference to the UserList received as a parameter of the MainMenu Constructor
        UsersList uList;
        #endregion

        #region Events
        public event EventHandler OnNewGameClick = delegate { };
        public event EventHandler<MainMenuEventArgs> OnNewGameStartClick = delegate { };
        public event EventHandler<MainMenuEventArgs> OnDeleteUserClick = delegate { };
        public event EventHandler<MainMenuEventArgs> OnLoadGameClick = delegate { };
        public event EventHandler OnExitClick = delegate { };
        #endregion

        #region Properties
        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }
        #endregion

        #region Constructors
        public MainMenu(Game game, ref UsersList list)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));
            randomSlot = new Random();
                        
            uList = list;
            usersList = new List<string> { };
            for (int i = 0; i < list.Count; i++)
                usersList.Add(list[i].Name);

            InitialState();
            Game.Components.Add(this);
        }
        #endregion

        #region Methods
        void InitialState()
        {
            //if unspecified, for Sprites, X and Y coordinates are 0 by default
            ImagineStatica = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("ImagineStatica")].Frames);
            ImagineStatica.DrawOrder = drawOrder_ImagineStatica;

            AnimatieVentilatoare = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieVentilatoare")].Frames);
            AnimatieVentilatoare.AnimationSpeed = speed_AnimatieVentilatoare;
            AnimatieVentilatoare.GenerateEvents = false; //preloaderSprite will run continuously
            AnimatieVentilatoare.X = 233;
            AnimatieVentilatoare.Y = 163;
            AnimatieVentilatoare.DrawOrder = drawOrder_AnimatieVentilatoare;
            SoundManager.PlaySound(Sounds.MainMenuVents);

            AnimatieStalpDreapta = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieStalpDreapta")].Frames);
            AnimatieStalpDreapta.FrameNumber = 5;
            AnimatieStalpDreapta.X = 593;
            AnimatieStalpDreapta.Y = 179;
            AnimatieStalpDreapta.DrawOrder = drawOrder_AnimatieStalpDreapta;

            Portal = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Portal")].Frames);
            Portal.FrameNumber = 0;
            Portal.X = 240;
            Portal.Y = 93;
            Portal.DrawOrder = drawOrder_Portal;

            AnimatieStalpStanga = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieStalpStanga")].Frames);
            AnimatieStalpStanga.FrameNumber = 5;
            AnimatieStalpStanga.X = 43;
            AnimatieStalpStanga.Y = 187;
            AnimatieStalpStanga.DrawOrder = drawOrder_AnimatieStalpStanga;

            AnimatieBrateRobotice = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieBrateRobotice")].Frames);
            AnimatieBrateRobotice.FrameNumber = 25;
            AnimatieBrateRobotice.FrameOfInterest = 5;
            AnimatieBrateRobotice.GenerateFrameOfInterestEvent = true;//will generate Event on frame 5
            AnimatieBrateRobotice.X = 175;
            AnimatieBrateRobotice.Y = 0;
            AnimatieBrateRobotice.DrawOrder = drawOrder_AnimatieBrateRobotice;

            AnimatieScari = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieScari")].Frames);
            AnimatieScari.FrameNumber = 0;
            AnimatieScari.X = 410;
            AnimatieScari.Y = 420;
            AnimatieScari.DrawOrder = drawOrder_AnimatieScari;

            AnimatieMonitoareSecundare = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieMonitoareSecundare")].Frames);
            AnimatieMonitoareSecundare.FrameNumber = 0;
            AnimatieMonitoareSecundare.X = 185;
            AnimatieMonitoareSecundare.Y = 511;
            AnimatieMonitoareSecundare.DrawOrder = drawOrder_AnimatieMonitoareSecundare;

            monitor_4 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("monitor_4")].Frames);
            monitor_4.FrameNumber = 0;
            monitor_4.X = 285;
            monitor_4.Y = 571;
            monitor_4.DrawOrder = drawOrder_monitors;

            MonitorPrincipal = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MonitorPrincipal")].Frames);
            MonitorPrincipal.X = 583;
            MonitorPrincipal.Y = 498;
            MonitorPrincipal.DrawOrder = drawOrder_MonitorPrincipal;

            Slot_1 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Slot_1")].Frames);
            Slot_1.DrawOrder = drawOrder_Slots;
            Slot_1.X = 478;
            Slot_1.Y = 560;
            Slot_2 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Slot_2")].Frames);
            Slot_2.DrawOrder = drawOrder_Slots;
            Slot_2.X = 481;
            Slot_2.Y = 615;
            Slot_3 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Slot_3")].Frames);
            Slot_3.DrawOrder = drawOrder_Slots;
            Slot_3.X = 483;
            Slot_3.Y = 668;

            int[] positions = new int[6] { 681, 560, 645, 565, 873, 560 };
            btnNewGame = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("NewGame")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffLeft")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffRight")].Frames), drawOrder_Buttons, positions);
            btnNewGame.OnMouseOver += new EventHandler(Do_NewGameMouseOver);
            btnNewGame.OnMouseLeave += new EventHandler(Do_NewGameMouseLeave);
            btnNewGame.OnMouseClick += new EventHandler(Do_NewGameMouseClick);

            positions = new int[6] { 672, 598, 646, 603, 872, 598 };
            btnLoadGame = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("LoadGame")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffLeft")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffRight")].Frames), drawOrder_Buttons, positions);
            btnLoadGame.OnMouseOver += new EventHandler(Do_LoadGameMouseOver);
            btnLoadGame.OnMouseLeave += new EventHandler(Do_LoadGameMouseLeave);
            btnLoadGame.OnMouseClick += new EventHandler(Do_LoadGameMouseClick);

            positions = new int[6] { 695, 636, 647, 641, 871, 636 };
            btnCredits = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Credits")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffLeft")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffRight")].Frames), drawOrder_Buttons, positions);
            btnCredits.OnMouseOver += new EventHandler(Do_CreditsMouseOver);
            btnCredits.OnMouseLeave += new EventHandler(Do_CreditsMouseLeave);
            btnCredits.OnMouseClick += new EventHandler(Launch_CreditsEvent);

            positions = new int[6] { 730, 674, 648, 679, 870, 674 };
            btnExit = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Exit")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffLeft")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOffRight")].Frames), drawOrder_Buttons, positions);
            btnExit.OnMouseOver += new EventHandler(Do_ExitMouseOver);
            btnExit.OnMouseLeave += new EventHandler(Do_ExitMouseLeave);
            btnExit.OnMouseClick += new EventHandler(Do_ExitMouseClick);

            //ActivareEfectPortal frame 0 is completely transparent
            ActivareEfectPortal = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("ActivareEfectPortal")].Frames);
            ActivareEfectPortal.X = 350;
            ActivareEfectPortal.Y = 200;
            ActivareEfectPortal.DrawOrder = drawOrder_ActivareEfectPortal;

            //hidden
            AnimatieEfectPortal = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("AnimatieEfectPortal")].Frames);
            AnimatieEfectPortal.X = 350;
            AnimatieEfectPortal.Y = 200;
            AnimatieEfectPortal.DrawOrder = 0;

            //hidden
            monitor_1 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("monitor_1")].Frames);
            monitor_1.DrawOrder = 0;
            monitor_1.X = 201;
            monitor_1.Y = 608;
            monitor_1.GenerateEvents = false;//preloaderSprite will run continuously when Visible

            //hidden
            monitor_2 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("monitor_2")].Frames);
            monitor_2.DrawOrder = 0;
            monitor_2.X = 395;
            monitor_2.Y = 556;
            monitor_2.GenerateEvents = false;//preloaderSprite will run continuously when Visible

            //hidden
            monitor_3 = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("monitor_3")].Frames);
            monitor_3.DrawOrder = 0;
            monitor_3.X = 302;
            monitor_3.Y = 518;
            monitor_3.GenerateEvents = false;//preloaderSprite will run continuously when Visible

            ExitTimer = new Timer(Game);
            ExitTimer.Interval = 1;//x seconds


            //LoadSettings Screen Section
            positions = new int[4] { 650, 689, 703, 691 };
            btnLoadScreenLoad = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("LoadScreenLoad")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOff")].Frames), drawOrder_Buttons, positions);
            btnLoadScreenLoad.OnMouseClick += new EventHandler(Do_LoadScreenLoadClick);

            positions = new int[4] { 727, 687, 802, 689 };
            btnLoadScreenDelete = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("LoadScreenDelete")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOff")].Frames), drawOrder_Buttons, positions);
            btnLoadScreenDelete.OnMouseClick += new EventHandler(Do_LoadScreenDeleteClick);

            LoadScreenFrame = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("LoadScreenFrame")].Frames);
            LoadScreenFrame.DrawOrder = drawOrder_Buttons;
            LoadScreenFrame.X = 647;
            LoadScreenFrame.Y = 560;

            positions = new int[6] { 868, 586, 857, 565, 858, 647 };
            LoadScreenScrollbar = new MainMenuScrollbar(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("ScrollbarBar")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("ScrollbarUp")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("ScrollbarDown")].Frames), drawOrder_Buttons, positions);
            LoadScreenScrollbar.OnScrollUp += new EventHandler(Do_ScrollUp);
            LoadScreenScrollbar.OnScrollDown += new EventHandler(Do_ScrollDown);

            User1 = new MainMenuUser(Game, "", drawOrder_Buttons, 660, 580);
            User1.OnUserClick += new EventHandler(Do_ChangeSelectedUser);

            User2 = new MainMenuUser(Game, "", drawOrder_Buttons, 660, 610);
            User2.OnUserClick += new EventHandler(Do_ChangeSelectedUser);

            User3 = new MainMenuUser(Game, "", drawOrder_Buttons, 660, 640);
            User3.OnUserClick += new EventHandler(Do_ChangeSelectedUser);

            //NewGame Screen Section
            NewGameScreenUserName = new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("NewGameScreenUserName")].Frames[0]);
            NewGameScreenUserName.X = 650;
            NewGameScreenUserName.Y = 570;
            NewGameScreenUserName.DrawOrder = drawOrder_Buttons;

            positions = new int[4] { 648, 689, 718, 690 };
            btnNewGameScreenStart = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("StartNewGame")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOff")].Frames), drawOrder_Buttons, positions);
            btnNewGameScreenStart.OnMouseClick += new EventHandler(Launch_NewGameStartEvent);

            positions = new int[3] { 660, 620, 170 }; //X,Y,Width
            inputNewGameUserName = new MainMenuInputBox(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("pixel")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("pixel")].Frames), drawOrder_Buttons, positions);

            //NewGame/LoadGame/Credits Back Button
            positions = new int[4] { 827, 685, 882, 687 };
            btnBack = new MainMenuButton(Game, new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("Back")].Frames), new Sprite(Game, graphicsCollection[graphicsCollection.GetPackIndex("MouseOnOff")].Frames), drawOrder_Buttons, positions);


            //Credits Screen Section
            txtCreditsCronosTeam = new SpriteText(Game,((FontsCollection)Game.Services.GetService(typeof(FontsCollection))).GetPack("MainMenu").Font); 
            txtCreditsCronosTeam.X = 660;
            txtCreditsCronosTeam.Y = 560;
            txtCreditsCronosTeam.DrawOrder = drawOrder_Buttons;
            txtCreditsCronosTeam.Tint = Color.Chartreuse;
            txtCreditsCronosTeam.Text = "Cronos Team:";
            txtCreditsCronosTeam.MaxLength = 15;

            txtCreditsMembers = new SpriteText(Game,((FontsCollection)Game.Services.GetService(typeof(FontsCollection))).GetPack("MainMenu").Font); 
            txtCreditsMembers.X = 660;
            txtCreditsMembers.Y = 590;
            txtCreditsMembers.DrawOrder = drawOrder_Buttons;
            txtCreditsMembers.Tint = Color.MediumSeaGreen;
            txtCreditsMembers.Text = "Corneliu  Dascalu\nAlexandru Girigan\nAdriana  Dumitras\nGabriel  Tironeac";
            txtCreditsMembers.MaxLength = 100;


            LoadScreenHide();
            CreditsScreenHide();
            NewGameScreenHide();
            txtCreditsCronosTeam.Visible = false;
            txtCreditsMembers.Visible = false;
        }

        /// <summary>
        /// Hides the MainMenu (stops drawing and dactivates it)
        /// </summary>
        public void Hide()
        {
            try
            {
                ImagineStatica.Visible = false;
                ActivareEfectPortal.Visible = false;
                AnimatieBrateRobotice.Visible = false;
                AnimatieEfectPortal.Visible = false;
                AnimatieMonitoareSecundare.Visible = false;
                monitor_1.Visible = false;
                monitor_2.Visible = false;
                monitor_3.Visible = false;
                monitor_4.Visible = false;
                AnimatieScari.Visible = false;
                AnimatieStalpDreapta.Visible = false;
                AnimatieStalpStanga.Visible = false;
                AnimatieVentilatoare.Visible = false;
                MonitorPrincipal.Visible = false;
                Slot_1.Visible = false;
                Slot_2.Visible = false;
                Slot_3.Visible = false;
                Portal.Visible = false;

                SoundManager.StopMainMenuSounds();

                LoadScreenHide();//1.
                NewGameScreenHide();//2.
                CreditsScreenHide();//3.
                MainScreenHide();//4. in this order!?

                ClickEventOccured = MainMenuEvents.None;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Shows the MainMenu (resets it and starts drawing it)
        /// </summary>
        public void Show()
        {
            try
            {
                ImagineStatica.Visible = true;
                ActivareEfectPortal.Visible = true;
                ActivareEfectPortal.FrameNumber = 0;
                ActivareEfectPortal.AnimationSpeed = 0;
                AnimatieBrateRobotice.Visible = true;
                AnimatieBrateRobotice.FrameNumber = 25;
                AnimatieBrateRobotice.AnimationSpeed = 0;
                AnimatieEfectPortal.Visible = true;
                AnimatieEfectPortal.DrawOrder = 0;
                AnimatieEfectPortal.AnimationSpeed = 0;
                AnimatieMonitoareSecundare.Visible = true;
                AnimatieMonitoareSecundare.FrameNumber = 0;
                AnimatieMonitoareSecundare.AnimationSpeed = 0;
                monitor_1.Visible = true;
                monitor_1.DrawOrder = 0;
                monitor_2.Visible = true;
                monitor_2.DrawOrder = 0;
                monitor_3.Visible = true;
                monitor_3.DrawOrder = 0;
                monitor_4.Visible = true;
                monitor_4.FrameNumber = 0;
                AnimatieScari.Visible = true;
                AnimatieScari.FrameNumber = 0;
                AnimatieStalpDreapta.Visible = true;
                AnimatieStalpDreapta.FrameNumber = 5;
                AnimatieStalpDreapta.AnimationSpeed = 0;
                AnimatieStalpStanga.Visible = true;
                AnimatieStalpStanga.FrameNumber = 5;
                AnimatieStalpStanga.AnimationSpeed = 0;
                AnimatieVentilatoare.Visible = true;
                MonitorPrincipal.Visible = true;
                Slot_1.Visible = true;
                Slot_1.FrameNumber = 0;
                Slot_2.Visible = true;
                Slot_2.FrameNumber = 0;
                Slot_3.Visible = true;
                Slot_3.FrameNumber = 0;
                Portal.Visible = true;
                Portal.FrameNumber = 0;

                MainScreenShow();

                SoundManager.PlaySound(Sounds.MainMenuVents);

                AnimatieNewGame_AllowRollback = true;
                AnimatieExit_AllowRollback = true;
                AnimatieCredits_AllowRollback = true;

                ClickEventOccured = MainMenuEvents.None;
            }
            catch
            {
            }
        }

        void LoadScreenShow()
        {
            MainScreenHide();

            btnLoadScreenLoad.Show();
            btnLoadScreenDelete.Show();
            btnBack.Show();
            btnBack.OnMouseClick += new EventHandler(Do_LoadScreenBackClick);

            LoadScreenFrame.Visible = true;

            LoadScreenScrollbar.Show();
            UserIntervalStart = 0;

            //refreshes the user list
            usersList = new List<string> { };
            for (int i = 0; i < uList.Count; i++)
                usersList.Add(uList[i].Name);

            UpdateUsers(UserIntervalStart);

            ClickEventOccured = MainMenuEvents.None;

            AnimatieNewGame_AllowRollback = true;
            AnimatieExit_AllowRollback = true;
            AnimatieCredits_AllowRollback = true;
            AnimatieLoad_AllowRollback = true;
        }

        void LoadScreenHide()
        {
            MainScreenShow();

            btnLoadScreenLoad.Hide();
            btnLoadScreenDelete.Hide();
            btnBack.Hide();
            btnBack.OnMouseClick -= new EventHandler(Do_LoadScreenBackClick);

            LoadScreenScrollbar.Hide();
            User1.Hide();
            User2.Hide();
            User3.Hide();

            LoadScreenFrame.Visible = false;

            Do_LoadGameMouseLeave(this, new EventArgs());

            ClickEventOccured = MainMenuEvents.None;
        }

        void MainScreenShow()
        {
            btnNewGame.Show();
            btnLoadGame.Show();
            btnCredits.Show();
            btnExit.Show();

            AnimatieNewGame_AllowRollback = true;
            AnimatieExit_AllowRollback = true;
            AnimatieCredits_AllowRollback = true;
            AnimatieLoad_AllowRollback = true;
        }

        void MainScreenHide()
        {
            btnNewGame.Hide();
            btnLoadGame.Hide();
            btnCredits.Hide();
            btnExit.Hide();
        }

        void CreditsScreenShow()
        {
            MainScreenHide();
            btnBack.Show();
            btnBack.OnMouseClick += new EventHandler(Do_CreditsScreenBackClick);
            btnBack.OnMouseClick += new EventHandler(Do_CreditsMouseLeave);

            ClickEventOccured = MainMenuEvents.None;

            txtCreditsCronosTeam.Visible = true;
            txtCreditsMembers.Visible = true;
        }

        void CreditsScreenHide()
        {
            MainScreenShow();
            btnBack.Hide();
            btnBack.OnMouseClick -= new EventHandler(Do_CreditsScreenBackClick);
            btnBack.OnMouseClick -= new EventHandler(Do_CreditsMouseLeave);
            ClickEventOccured = MainMenuEvents.None;

            txtCreditsCronosTeam.Visible = false;
            txtCreditsMembers.Visible = false;
        }

        void NewGameScreenShow()
        {
            MainScreenHide();
            NewGameScreenUserName.Visible = true;
            btnBack.Show();
            btnNewGameScreenStart.Show();
            inputNewGameUserName.Show();
            btnBack.OnMouseClick += new EventHandler(Do_NewGameScreenBackClick);
            btnBack.OnMouseClick += new EventHandler(Do_NewGameMouseLeave);

            ClickEventOccured = MainMenuEvents.None;
        }

        void NewGameScreenHide()
        {
            MainScreenShow();
            NewGameScreenUserName.Visible = false;
            btnBack.Hide();
            btnNewGameScreenStart.Hide();
            inputNewGameUserName.Hide();
            btnBack.OnMouseClick -= new EventHandler(Do_NewGameScreenBackClick);
            btnBack.OnMouseClick -= new EventHandler(Do_NewGameMouseLeave);
            ClickEventOccured = MainMenuEvents.None;
        }

        /// <summary>
        /// The 3 Users shown on the LoadSettings Screen
        /// </summary>
        void UpdateUsers(int i)
        {
            User1.Hide();//Hide also makes all Users unselected!
            User2.Hide();
            User3.Hide();

            if (usersList.Count > i)
            {
                User1.UserName = usersList[i];
                User1.Show();
            }
            if (usersList.Count > (i + 1))
            {
                User2.UserName = usersList[i + 1];
                User2.Show();
            }
            if (usersList.Count > (i + 2))
            {
                User3.UserName = usersList[i + 2];
                User3.Show();
            }
        }

        /// <summary>
        /// Determines the selected user, if there is one
        /// </summary>
        void SetSelectedUser()
        {
            if (User1.isSelected)
                SelectedUser = User1.UserName;
            else
                if (User2.isSelected)
                    SelectedUser = User2.UserName;
                else
                    if (User3.isSelected)
                        SelectedUser = User3.UserName;
                    else
                        SelectedUser = "";
        }
        #endregion

        #region Event Handlers

        #region NewGame Button Event Handlers
        void Do_NewGameMouseOver(object sender, EventArgs e)
        {

            if (AnimatieBrateRobotice.FrameNumber == 25)
            {
                if (AnimatieNewGame_AllowRollback)
                {
                    //NewGame Animation can only start after/if Exit Animation was paused on Frame 25
                    SoundManager.PlaySound(Sounds.MainMenuStairs);
                    AnimatieScari.AnimationSpeed = speed_AnimatieScari;
                    AnimatieScari.AnimDirection = AnimationDirection.Forward;
                    AnimatieScari.OnLastFrame += new EventHandler(Do_PauseAnimation);
                    
                    ActivareEfectPortal.AnimationSpeed = speed_ActivareEfectPortal;
                    ActivareEfectPortal.AnimDirection = AnimationDirection.Forward;
                    ActivareEfectPortal.OnLastFrame += new EventHandler(Do_StartAnimatieEfectPortal);
                }
            }
            else//buys more time for the Exit Animation to end
            {
                btnNewGame.ResetMouseOverOff();
            }
        }

        void Do_NewGameMouseLeave(object sender, EventArgs e)
        {
            if (AnimatieNewGame_AllowRollback)//button was not clicked previosly to MouseLeave
            {
                SoundManager.StopSound(Sounds.MainMenuStairs);
                AnimatieScari.AnimationSpeed = speed_AnimatieScari;
                AnimatieScari.AnimDirection = AnimationDirection.Backward;
                AnimatieScari.OnFirstFrame += new EventHandler(Do_PauseAnimation);

                SoundManager.StopSound(Sounds.MainMenuTimeGateWater);
                AnimatieEfectPortal.AnimationSpeed = 0;
                AnimatieEfectPortal.DrawOrder = 0;

                ActivareEfectPortal.AnimationSpeed = speed_ActivareEfectPortal;
                ActivareEfectPortal.AnimDirection = AnimationDirection.Backward;
                ActivareEfectPortal.OnFirstFrame += new EventHandler(Do_PauseAnimation);
            }
        }

        void Do_NewGameMouseClick(object sender, EventArgs e)
        {
            if (ClickEventOccured == MainMenuEvents.None)
            {   //no click event occured yet
                ClickEventOccured = MainMenuEvents.NewGame;

                if (AnimatieMonitoareSecundare.AnimationSpeed > 0 || AnimatieMonitoareSecundare.FrameNumber == 10)//OFFLINE
                    Do_CreditsMouseLeave(this, new EventArgs());
                AnimatieCredits_AllowRollback = false;

                Do_LoadGameMouseLeave(this, new EventArgs());
                AnimatieLoad_AllowRollback = false;

                AnimatieNewGame_AllowRollback = false;
                AnimatieExit_AllowRollback = false;

                if (AnimatieBrateRobotice.AnimationSpeed > 0 || AnimatieBrateRobotice.FrameNumber == 0)
                {
                    AnimatieExit_AllowRollback = true;
                    AnimatieNewGame_AllowRollback = true;
                    AnimatieBrateRobotice.OnLastFrame += new EventHandler(Do_NewGameMouseOver);
                    AnimatieScari.OnLastFrame += new EventHandler(Do_Load_NewGameScreen);
                    AnimatieScari.GenerateFrameOfInterestEvent = true;
                    AnimatieScari.FrameOfInterest = 1;
                    AnimatieScari.OnFrameOfInterest += new EventHandler(BlockRollback);

                    Do_ExitMouseLeave(this, new EventArgs());
                    AnimatieExit_AllowRollback = false;
                }
                else
                    if (AnimatieScari.FrameNumber == 0 || AnimatieScari.AnimationSpeed > 0)
                    {
                        AnimatieScari.OnLastFrame += new EventHandler(Do_Load_NewGameScreen);
                        AnimatieNewGame_AllowRollback = true;
                        Do_NewGameMouseOver(this, new EventArgs());
                        AnimatieNewGame_AllowRollback = false;
                    }
                    else
                        Do_Load_NewGameScreen(this, new EventArgs());
            }
        }

        void BlockRollback(object sender, EventArgs e)
        {
            AnimatieBrateRobotice.OnLastFrame -= new EventHandler(Do_NewGameMouseOver);
            AnimatieScari.OnFrameOfInterest -= new EventHandler(BlockRollback);
            AnimatieScari.GenerateFrameOfInterestEvent = false;
            AnimatieNewGame_AllowRollback = false;
        }

        //The NewGameScreen will be shown
        void Do_Load_NewGameScreen(object sender, EventArgs e)
        {
            OnNewGameClick(this, new EventArgs());
            AnimatieScari.OnLastFrame -= new EventHandler(Do_Load_NewGameScreen);
            NewGameScreenShow();
        }

        void Launch_NewGameStartEvent(object sender, EventArgs e)
        {
            string text = inputNewGameUserName.Text;
            text = text.Trim();
            if (text.Length == 0)
            {
                inputNewGameUserName.PostMessage("Invalid user name!");
            }
            else
            {
                if (usersList.Contains(text))
                    inputNewGameUserName.PostMessage("User name already exists!");
                else
                    OnNewGameStartClick(this, new MainMenuEventArgs(text));
            }
        }

        void Do_StartAnimatieEfectPortal(object sender, EventArgs e)
        {
            Do_PauseAnimation(sender, e);

            SoundManager.PlaySound(Sounds.MainMenuTimeGateWater);

            ActivareEfectPortal.OnLastFrame -= new EventHandler(Do_StartAnimatieEfectPortal);

            AnimatieEfectPortal.DrawOrder = drawOrder_AnimatieEfectPortal;
            AnimatieEfectPortal.DrawOrder = drawOrder_AnimatieEfectPortal;
            AnimatieEfectPortal.AnimationSpeed = speed_AnimatieEfectPortal;
        }
        #endregion

        #region LoadGame Button Event Handlers
        void Do_LoadGameMouseOver(object sender, EventArgs e)
        {
            if (AnimatieLoad_AllowRollback)
            {                   //stops AnimationRollback after LoadGame Click

                SoundManager.PlaySound(Sounds.MainMenuSlot);

                switch (randomSlot.Next(3))
                {
                    case 0:
                        Slot_1.FrameNumber = 1;
                        break;
                    case 1:
                        Slot_2.FrameNumber = 1;
                        break;
                    case 2:
                        Slot_3.FrameNumber = 1;
                        break;
                }
            }
        }

        void Do_LoadGameMouseLeave(object sender, EventArgs e)
        {
            if (AnimatieLoad_AllowRollback)
            {               //stops AnimationRollback after LoadGame Click
                SoundManager.StopSound(Sounds.MainMenuSlot);
                Slot_1.FrameNumber = 0;
                Slot_2.FrameNumber = 0;
                Slot_3.FrameNumber = 0;
            }
        }

        void Do_LoadGameMouseClick(object sender, EventArgs e)
        {
            if (ClickEventOccured == MainMenuEvents.None)
            {
                ClickEventOccured = MainMenuEvents.LoadGame;

                if (AnimatieBrateRobotice.AnimationSpeed > 0 || AnimatieBrateRobotice.FrameNumber == 0)
                    Do_ExitMouseLeave(this, new EventArgs());
                AnimatieExit_AllowRollback = false;

                if (AnimatieScari.AnimationSpeed > 0 || AnimatieScari.FrameNumber == 10)
                    Do_NewGameMouseLeave(this, new EventArgs());
                AnimatieNewGame_AllowRollback = false;

                if (AnimatieMonitoareSecundare.AnimationSpeed > 0 || AnimatieMonitoareSecundare.FrameNumber == 10)//OFFLINE
                {
                    Do_CreditsMouseLeave(this, new EventArgs());
                    AnimatieCredits_AllowRollback = false;
                }
                if (Slot_1.FrameNumber == 0 && Slot_2.FrameNumber == 0 && Slot_3.FrameNumber == 0)
                    Do_LoadGameMouseOver(this, new EventArgs());
                AnimatieLoad_AllowRollback = false;
                LoadScreenShow();
            }

        }
        #endregion

        #region Credits Button Event Handlers
        void Do_CreditsMouseOver(object sender, EventArgs e)
        {
            if (AnimatieCredits_AllowRollback)
            {
                SoundManager.PlaySound(Sounds.MainMenuCreditsPanelOpening);

                monitor_4.FrameNumber = 1; //'ONLINE'
                AnimatieMonitoareSecundare.AnimDirection = AnimationDirection.Forward;
                AnimatieMonitoareSecundare.AnimationSpeed = speed_AnimatieMonitoareSecundare;
                AnimatieMonitoareSecundare.OnLastFrame += new EventHandler(Do_StartMonitoareSecundare);
            }
        }

        void Do_CreditsMouseLeave(object sender, EventArgs e)
        {
            if (AnimatieCredits_AllowRollback)
            {
                SoundManager.StopSound(Sounds.MainMenuCreditsPanelOpening);
                SoundManager.PlaySound(Sounds.MainMenuCreditsPanelClosing);

                monitor_1.DrawOrder = 0;//hidden
                monitor_1.ResetAnimation();

                monitor_2.DrawOrder = 0;//hidden
                monitor_2.ResetAnimation();

                monitor_3.DrawOrder = 0;//hidden
                monitor_3.ResetAnimation();

                monitor_4.FrameNumber = 0;//'OFFLINE'

                AnimatieMonitoareSecundare.AnimDirection = AnimationDirection.Backward;
                AnimatieMonitoareSecundare.AnimationSpeed = speed_AnimatieMonitoareSecundare;
                AnimatieMonitoareSecundare.OnFirstFrame += new EventHandler(Do_PauseAnimation);
            }
        }

        void Do_StartMonitoareSecundare(object sender, EventArgs e)
        {
            Do_PauseAnimation(sender, e);

            AnimatieMonitoareSecundare.OnLastFrame -= new EventHandler(Do_StartMonitoareSecundare);

            monitor_1.DrawOrder = drawOrder_monitors;
            monitor_1.AnimationSpeed = speed_AnimatieMonitoareSecundare;

            monitor_2.DrawOrder = drawOrder_monitors;
            monitor_2.AnimationSpeed = speed_AnimatieMonitoareSecundare;

            monitor_3.DrawOrder = drawOrder_monitors;
            monitor_3.AnimationSpeed = speed_AnimatieMonitoareSecundare;
        }

        void Launch_CreditsEvent(object sender, EventArgs e)
        {

            if (ClickEventOccured == MainMenuEvents.None)
            {
                ClickEventOccured = MainMenuEvents.Credits;

                if (AnimatieBrateRobotice.AnimationSpeed > 0 || AnimatieBrateRobotice.FrameNumber == 0)
                    Do_ExitMouseLeave(this, new EventArgs());
                AnimatieExit_AllowRollback = false;

                if (AnimatieScari.AnimationSpeed > 0 || AnimatieScari.FrameNumber == 10)
                    Do_NewGameMouseLeave(this, new EventArgs());
                AnimatieNewGame_AllowRollback = false;

                Do_LoadGameMouseLeave(this, new EventArgs());
                AnimatieLoad_AllowRollback = false;

                AnimatieCredits_AllowRollback = false;
                if (AnimatieMonitoareSecundare.FrameNumber == 0 || AnimatieMonitoareSecundare.AnimationSpeed > 0)//OFFLINE
                {
                    AnimatieMonitoareSecundare.OnLastFrame += new EventHandler(Do_CreditsScreen);
                    AnimatieCredits_AllowRollback = true;
                    Do_CreditsMouseOver(this, new EventArgs());
                    AnimatieCredits_AllowRollback = false;
                }
                else
                    CreditsScreenShow();
            }
        }

        void Do_CreditsScreen(object sender, EventArgs e)
        {
            AnimatieMonitoareSecundare.OnLastFrame -= new EventHandler(Do_CreditsScreen);
            CreditsScreenShow();
        }
        #endregion

        #region Exit Button Event Handler
        void Do_ExitMouseOver(object sender, EventArgs e)
        {
            if (AnimatieScari.FrameNumber == 0 || (AnimatieScari.FrameNumber == 10 && AnimatieNewGame_AllowRollback == false))
            {
                if (AnimatieExit_AllowRollback)
                {
                    SoundManager.PlaySound(Sounds.MainMenuRoboticArms);
                    //Exit Animation can only start after/if NewGame Animation was paused on Frame 0
                    AnimatieBrateRobotice.AnimDirection = AnimationDirection.Backward;
                    AnimatieBrateRobotice.FrameOfInterest = 5;
                    AnimatieBrateRobotice.GenerateFrameOfInterestEvent = true;//Event on FrameNumber==5 of interest                     
                    AnimatieBrateRobotice.AnimationSpeed = speed_AnimatieBrateRobotice;

                    AnimatieBrateRobotice.OnFrameOfInterest += new EventHandler(Do_BackwardAnimatiiStalpi);
                    AnimatieBrateRobotice.OnFirstFrame += new EventHandler(Do_DeactivatePortal);
                }
            }
            else//buys more time for the NewGame Animation to end (half second)
            {
                btnExit.ResetMouseOverOff();
            }
        }

        void Do_ExitMouseLeave(object sender, EventArgs e)
        {
            if (AnimatieExit_AllowRollback)//button was not clicked previosly to MouseLeave
            {
                SoundManager.StopSound(Sounds.MainMenuPiston);
                SoundManager.PlaySound(Sounds.MainMenuRoboticArms);

                AnimatieBrateRobotice.AnimDirection = AnimationDirection.Forward;
                AnimatieBrateRobotice.GenerateFrameOfInterestEvent = false;//Event on FrameNumber==5 NOT of interest
                AnimatieBrateRobotice.AnimationSpeed = speed_AnimatieBrateRobotice;

                if (AnimatieBrateRobotice.FrameNumber <= 5)
                {
                    ForwardAnimatiiStalpi();
                }

                AnimatieBrateRobotice.OnLastFrame += new EventHandler(Do_PauseAnimation);
            }
        }

        void Do_BackwardAnimatiiStalpi(object sender, EventArgs e)
        {
            SoundManager.PlaySound(Sounds.MainMenuPiston);
            SoundManager.StopSound(Sounds.MainMenuRoboticArms);

            AnimatieBrateRobotice.OnFrameOfInterest -= new EventHandler(Do_BackwardAnimatiiStalpi);

            AnimatieStalpDreapta.AnimDirection = AnimationDirection.Backward;
            AnimatieStalpDreapta.AnimationSpeed = speed_AnimatieStalpi;
            AnimatieStalpStanga.AnimDirection = AnimationDirection.Backward;
            AnimatieStalpStanga.AnimationSpeed = speed_AnimatieStalpi;

            AnimatieStalpDreapta.OnFirstFrame += new EventHandler(Do_TryPauseAnimation);
            AnimatieStalpStanga.OnFirstFrame += new EventHandler(Do_TryPauseAnimation);
            AnimatieBrateRobotice.GenerateFrameOfInterestEvent = false;
        }

        void ForwardAnimatiiStalpi()
        {
            AnimatieStalpDreapta.AnimDirection = AnimationDirection.Forward;
            AnimatieStalpDreapta.AnimationSpeed = speed_AnimatieStalpi;
            AnimatieStalpStanga.AnimDirection = AnimationDirection.Forward;
            AnimatieStalpStanga.AnimationSpeed = speed_AnimatieStalpi;

            AnimatieStalpDreapta.OnLastFrame += new EventHandler(Do_ActivatePortal);
            AnimatieStalpStanga.OnLastFrame += new EventHandler(Do_PauseAnimation);
        }

        void Do_ActivatePortal(object sender, EventArgs e)
        {
            ((Sprite)sender).PauseAnimation();
            Portal.FrameNumber = 0;
            AnimatieStalpDreapta.OnLastFrame -= new EventHandler(Do_ActivatePortal);
        }

        void Do_DeactivatePortal(object sender, EventArgs e)
        {
            SoundManager.StopSound(Sounds.MainMenuPiston);
            SoundManager.StopSound(Sounds.MainMenuRoboticArms);
            ((Sprite)sender).PauseAnimation();
            Portal.FrameNumber = 1;
            AnimatieBrateRobotice.OnFirstFrame -= new EventHandler(Do_DeactivatePortal);
        }

        void Do_TryPauseAnimation(object sender, EventArgs e)
        {           
            SoundManager.StopSound(Sounds.MainMenuPiston);
            SoundManager.PlaySound(Sounds.MainMenuRoboticArms);
            if (AnimatieBrateRobotice.FrameNumber < 5)
            {
                ((Sprite)sender).PauseAnimation();
            }
            else//exactly on AnimatieBrateRobotice.FrameNumber==5 occured a MouseLeave
            {
                ForwardAnimatiiStalpi();
            }
        }

        void Do_ExitMouseClick(object sender, EventArgs e)
        {
            if (ClickEventOccured == MainMenuEvents.None)
            {
                ClickEventOccured = MainMenuEvents.Exit;
                AnimatieExit_AllowRollback = false;

                if (AnimatieMonitoareSecundare.AnimationSpeed > 0 || AnimatieMonitoareSecundare.FrameNumber == 10)
                    Do_CreditsMouseLeave(this, new EventArgs());
                AnimatieCredits_AllowRollback = false;

                Do_LoadGameMouseLeave(this, new EventArgs());
                AnimatieLoad_AllowRollback = false;

                AnimatieNewGame_AllowRollback = false;
                AnimatieExit_AllowRollback = false;

                if (AnimatieScari.FrameNumber == 10 || AnimatieScari.AnimationSpeed > 0)
                {
                    AnimatieNewGame_AllowRollback = true;
                    AnimatieExit_AllowRollback = true;

                    AnimatieScari.OnFirstFrame += new EventHandler(Do_ExitMouseOver);
                    AnimatieBrateRobotice.OnFirstFrame += new EventHandler(Start_ExitGameTimer);
                    AnimatieBrateRobotice.GenerateFrameOfInterestEvent = true;
                    AnimatieBrateRobotice.FrameOfInterest = 24;
                    AnimatieBrateRobotice.OnFrameOfInterest += new EventHandler(BlockRollback2);

                    Do_NewGameMouseLeave(this, new EventArgs());
                    AnimatieNewGame_AllowRollback = false;
                }
                else
                {
                    if (AnimatieBrateRobotice.FrameNumber == 25 || AnimatieBrateRobotice.AnimationSpeed > 0)
                    {
                        AnimatieBrateRobotice.OnFirstFrame += new EventHandler(Start_ExitGameTimer);
                        AnimatieExit_AllowRollback = true;
                        Do_ExitMouseOver(this, new EventArgs());
                        AnimatieExit_AllowRollback = false;
                    }
                    else
                        Start_ExitGameTimer(this, new EventArgs());
                }
            }
        }

        void Start_ExitGameTimer(object sender, EventArgs e)
        {
            AnimatieBrateRobotice.OnFirstFrame -= new EventHandler(Start_ExitGameTimer);
            ExitTimer.Start();
            ExitTimer.OnTick += new EventHandler(Launch_ExitGameEvent);
        }

        void Launch_ExitGameEvent(object sender, EventArgs e)
        {
            ExitTimer.OnTick -= new EventHandler(Launch_ExitGameEvent);
            ExitTimer.Stop();
            //the event is launched after x seconds from the time the preloaderSprite ended
            OnExitClick(this, new EventArgs());
        }

        void BlockRollback2(object sender, EventArgs e)
        {
            AnimatieScari.OnFirstFrame -= new EventHandler(Do_ExitMouseOver);
            AnimatieBrateRobotice.OnFrameOfInterest -= new EventHandler(BlockRollback2);
            AnimatieExit_AllowRollback = false;
        }

        #endregion

        #region LoadScreen Buttons Event Handlers
        void Do_LoadScreenLoadClick(object sender, EventArgs e)
        {
            SetSelectedUser();

            if (SelectedUser != "")
            {
                OnLoadGameClick(this, new MainMenuEventArgs(SelectedUser));
            }
        }

        void Do_LoadScreenDeleteClick(object sender, EventArgs e)
        {
            SetSelectedUser();

            if (SelectedUser != "")
            {
                //launch event                        
                OnDeleteUserClick(this, new MainMenuEventArgs(SelectedUser));

                usersList.Remove(SelectedUser);
                UserIntervalStart = 0;
            }
            UpdateUsers(UserIntervalStart);
        }

        void Do_LoadScreenBackClick(object sender, EventArgs e)
        {
            LoadScreenHide();
        }

        void Do_ChangeSelectedUser(object sender, EventArgs e)
        {
            SelectedUser = ((MainMenuUser)sender).UserName;
        }

        void Do_ScrollUp(object sender, EventArgs e)
        {
            if (UserIntervalStart > 0)
            {
                UserIntervalStart--;
                UpdateUsers(UserIntervalStart);
            }
        }

        void Do_ScrollDown(object sender, EventArgs e)
        {
            if ((UserIntervalStart + 3) < usersList.Count)
            {
                UserIntervalStart++;
                UpdateUsers(UserIntervalStart);
            }
        }
        #endregion

        #region CreditsScreen Buttons Event Handlers
        void Do_CreditsScreenBackClick(object sender, EventArgs e)
        {
            CreditsScreenHide();
        }

        #endregion

        #region NewGameScreen Buttons Event Handlers
        void Do_NewGameScreenBackClick(object sender, EventArgs e)
        {
            NewGameScreenHide();
        }
        #endregion

        #region Other Sprites Event Handlers

        void Do_PauseAnimation(object sender, EventArgs e)
        {
            ((Sprite)sender).PauseAnimation();
            if ((Sprite)sender == AnimatieScari)
            {
                if (AnimatieScari.FrameNumber == 10)
                {
                    AnimatieScari.OnLastFrame -= new EventHandler(Do_PauseAnimation);
                }
                if (AnimatieScari.FrameNumber == 0)
                {
                    AnimatieScari.OnFirstFrame -= new EventHandler(Do_PauseAnimation);
                }

                SoundManager.StopSound(Sounds.MainMenuStairs);
            }
            if ((Sprite)sender == AnimatieStalpStanga)
            {
                AnimatieStalpStanga.OnLastFrame -= new EventHandler(Do_PauseAnimation);
            }
            if ((Sprite)sender == AnimatieBrateRobotice)
            {
                SoundManager.StopSound(Sounds.MainMenuRoboticArms);
                AnimatieBrateRobotice.OnLastFrame -= new EventHandler(Do_PauseAnimation);
            }
            if ((Sprite)sender == AnimatieMonitoareSecundare)
            {
                AnimatieMonitoareSecundare.OnFirstFrame -= new EventHandler(Do_PauseAnimation);
                SoundManager.StopSound(Sounds.MainMenuCreditsPanelOpening);
                SoundManager.StopSound(Sounds.MainMenuCreditsPanelClosing);
            }
            if ((Sprite)sender == ActivareEfectPortal)
            {
                ActivareEfectPortal.OnFirstFrame -= new EventHandler(Do_PauseAnimation);
            }
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