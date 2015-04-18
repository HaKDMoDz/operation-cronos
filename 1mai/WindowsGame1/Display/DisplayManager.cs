using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.Profiles;
using Operation_Cronos.Input;
using Operation_Cronos.IO;
using Operation_Cronos.GameProcessor;
using Operation_Cronos.Sound;
using System.Threading;

namespace Operation_Cronos.Display
{

    public partial class DisplayManager : GameComponent
    {

        Camera camera;
        Preloader preloader;
        //--MainMenu----------------
        MainMenu mainMenu;
        //needs to remain public
        public UsersList userList;
        //-------------------------
        InputVisualComponent root;
        //--CommandCenter----------
        CommandCenter commandCenter;
        //-------------------------
        GameInterface gameInterface;
        Sprite pointer;

        public bool CommandCenterLoaded = false;
        public bool GameInterfaceLoaded = false;

        GameMap gameMap;


        #region Properties
        public GraphicsCollection GraphicsCollection
        {
            get { return (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection)); }
        }

        private FontsCollection FontsCollection
        {
            get { return (FontsCollection)Game.Services.GetService(typeof(FontsCollection)); }
        }
        private IOManager IOManager
        {
            get { return (IOManager)Game.Services.GetService(typeof(IOManager)); }
        }
        public ProfileManager ProfilesManager
        {
            get { return (ProfileManager)Game.Services.GetService(typeof(ProfileManager)); }
        }
        public Matrix CameraPosition
        {
            get { return camera.WorldProjection; }
        }
        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        private GameInterface GUI
        {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }
        public SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }
        #endregion

        #region Events
        public event EventHandler OnGameInterfaceLoaded = delegate { };
        #endregion

        public DisplayManager(Game game)
            : base(game)
        {
            game.Components.Add(this);
            camera = new Camera(game);
            camera.Position = new Vector2(0, 0);
            camera.Freeze();
            camera.OnMove += new EventHandler(camera_OnMove);
            this.OnGameInterfaceLoaded += new EventHandler(DisplayManager_OnGameInterfaceLoaded);

            Game.Services.AddService(typeof(DisplayManager), this);
        }
             

        #region MainMenu
        void CreateAndShowMainMenu()
        {
            CameraFreeze();
            XMLLoader xmlLoader = new XMLLoader();
            xmlLoader.Load("Profiles\\profiles.xml");
            userList = new UsersList(xmlLoader.Document);

            mainMenu = new MainMenu(this.Game, ref userList);
            mainMenu.OnNewGameStartClick += new EventHandler<MainMenuEventArgs>(mainMenu_OnNewGameStartClick);
            mainMenu.OnExitClick += new EventHandler(mainMenu_OnExitClick);
            mainMenu.OnDeleteUserClick += new EventHandler<MainMenuEventArgs>(mainMenu_OnDeleteUSerClick);
            mainMenu.OnLoadGameClick += new EventHandler<MainMenuEventArgs>(mainMenu_OnLoadGameClick);

            pointer = new Sprite(this.Game, GraphicsCollection.GetPack("pointer"));
            pointer.XRelative = 0;
            pointer.YRelative = 0;
            pointer.DrawOrder = 10000;
        }

        void mainMenu_OnLoadGameClick(object sender, MainMenuEventArgs e)
        {
            mainMenu.Hide();

            //IOManagerEventReceiver will test if the CommandCenter Graphics is already loaded
            IOManager.LoadGame(this, e);

            //sets the selected profile
            ProfilesManager.SetProfile(e.UserName, false);
        }

        void mainMenu_OnExitClick(object sender, EventArgs e)
        {
            this.Game.Exit();
        }

        void mainMenu_OnNewGameStartClick(object sender, MainMenuEventArgs e)
        {
            mainMenu.Hide();

            //sets the input profile
            ProfilesManager.SetProfile(e.UserName, true);

            //IOManagerEventReceiver will test if the CommandCenter Graphics is already loaded
            IOManager.NewGame(e, new MainMenuEventArgs(e.UserName));
        }

        void mainMenu_OnDeleteUSerClick(object sender, MainMenuEventArgs e)
        {
            IOManager.DeleteUser(e, new MainMenuEventArgs(e.UserName));
        }
        #endregion

        #region Command Center
        void CreateAndShowCommandCenter()
        {
            CameraFreeze();
            commandCenter = new CommandCenter(this.Game);
            commandCenter.StackOrder = 5;
            root.AddChild(commandCenter);

            commandCenter.OnEnterZone += new EventHandler<CommandCenterEventArgs>(commandCenter_OnEnterZone);
            commandCenter.OnSaveGame += new EventHandler<CommandCenterEventArgs>(commandCenter_OnSaveGame);
            commandCenter.OnLogOut += new EventHandler(commandCenter_OnLogOut);
            commandCenter.OnExitGame += new EventHandler(commandCenter_OnExitGame);
            commandCenter.Show(); //the MAP button will initially be pressed, this way

            //loads the selected profile (if the profile already exists)
            //or creates and then loades the input profile (if it is a new profile)
            ProfilesManager.LoadProfile();

            CommandCenterLoaded = true;
        }

        void commandCenter_OnEnterZone(object sender, CommandCenterEventArgs e)
        {
            commandCenter.Hide();
            //--------
            camera.ChangeResolution(e.ResolutionValue.X, e.ResolutionValue.Y);
            if (e.FullScreen == "On")
                camera.Fullscreen = true;
            else
                camera.Fullscreen = false;
            //--------

            //preloader---
            preloader.Width = this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            preloader.Height = this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            preloader.X = 0;
            preloader.Y = 0;
            //-----------

            camera_OnMove(null, null); //refreshes the gameInterface panels' positions according to the 
            //new camera position

            IOManager.EnterZone(this, e);
        }

        void commandCenter_OnSaveGame(object sender, CommandCenterEventArgs e)
        {
            IOManager.SaveGame(sender, e);
        }

        void commandCenter_OnLogOut(object sender, EventArgs e)
        {
            commandCenter.Hide();
            ProfilesManager.LogOut();
            mainMenu.Show();
        }

        void commandCenter_OnExitGame(object sender, EventArgs e)
        {
            this.Game.Exit();
        }
        #endregion

        #region Camera
        void StartMovingCamera(Keys key)
        {
            switch (key)
            {
                default:
                    camera.Direction = CameraDirection.None;
                    break;
                case Keys.Down:
                    switch (camera.Direction)
                    {
                        case CameraDirection.None:
                            camera.Direction = CameraDirection.Down;
                            break;
                        case CameraDirection.Left:
                            camera.Direction = CameraDirection.DownLeft;
                            break;
                        case CameraDirection.Right:
                            camera.Direction = CameraDirection.DownRight;
                            break;
                        case CameraDirection.UpLeft:
                            camera.Direction = CameraDirection.DownLeft;
                            break;
                        case CameraDirection.UpRight:
                            camera.Direction = CameraDirection.DownRight;
                            break;
                    }
                    camera.CameraStatus = CameraStatus.KeyboardScrolled;
                    break;
                case Keys.Up:
                    switch (camera.Direction)
                    {
                        case CameraDirection.None:
                            camera.Direction = CameraDirection.Up;
                            break;
                        case CameraDirection.Left:
                            camera.Direction = CameraDirection.UpLeft;
                            break;
                        case CameraDirection.Right:
                            camera.Direction = CameraDirection.UpRight;
                            break;
                        case CameraDirection.DownLeft:
                            camera.Direction = CameraDirection.UpLeft;
                            break;
                        case CameraDirection.DownRight:
                            camera.Direction = CameraDirection.UpRight;
                            break;
                    }
                    camera.CameraStatus = CameraStatus.KeyboardScrolled;
                    break;
                case Keys.Left:
                    switch (camera.Direction)
                    {
                        case CameraDirection.None:
                            camera.Direction = CameraDirection.Left;
                            break;
                        case CameraDirection.Up:
                            camera.Direction = CameraDirection.UpLeft;
                            break;
                        case CameraDirection.Down:
                            camera.Direction = CameraDirection.DownLeft;
                            break;
                        case CameraDirection.DownRight:
                            camera.Direction = CameraDirection.DownLeft;
                            break;
                        case CameraDirection.UpRight:
                            camera.Direction = CameraDirection.UpLeft;
                            break;
                    }
                    camera.CameraStatus = CameraStatus.KeyboardScrolled;
                    break;
                case Keys.Right:
                    switch (camera.Direction)
                    {
                        case CameraDirection.None:
                            camera.Direction = CameraDirection.Right;
                            break;
                        case CameraDirection.Left:
                            camera.Direction = CameraDirection.Right;
                            break;
                        case CameraDirection.Up:
                            camera.Direction = CameraDirection.UpRight;
                            break;
                        case CameraDirection.Down:
                            camera.Direction = CameraDirection.DownRight;
                            break;
                        case CameraDirection.DownLeft:
                            camera.Direction = CameraDirection.DownRight;
                            break;
                        case CameraDirection.UpLeft:
                            camera.Direction = CameraDirection.UpRight;
                            break;
                    }
                    camera.CameraStatus = CameraStatus.KeyboardScrolled;
                    break;
            }
        }


        void StopMovingCamera()
        {
            camera.Direction = CameraDirection.None;
        }
        void camera_OnMove(object sender, EventArgs e)
        {
            if (gameInterface != null)
            {
                gameInterface.XRelative = (int)camera.Position.X;
                gameInterface.YRelative = (int)camera.Position.Y;
            }
            //
            if (pointer != null && camera != null)
            {
                pointer.X = Mouse.GetState().X - 13 + (int)camera.Position.X;
                pointer.Y = Mouse.GetState().Y - 3 + (int)camera.Position.Y;
            }
            //
            if (GUI != null)
            {
                GUI.UpdateMinimapCamera(new Point((int)camera.Position.X, (int)camera.Position.Y),
                    new Point(camera.Screen.Width, camera.Screen.Height));
            }
            //
            if (gameMap != null)
            {
                gameMap.UpdateConstructionPanelPosition();
            }
        }
        #endregion

        #region GUI
        void CreateAndShowGUI()
        {
            CameraFreeze();
            gameInterface = new GameInterface(this.Game, camera.Screen);
            gameInterface.OnSaveGame += new EventHandler(gameInterface_OnSaveGame);
            gameInterface.StackOrder = 3;
            gameInterface.XRelative = 0;
            gameInterface.YRelative = 0;
            
            root.AddChild(gameInterface);

            OnGameInterfaceLoaded(this, new EventArgs());
        }

        void gameInterface_OnSaveGame(object sender, EventArgs e)
        {
            CameraFreeze();
            SoundManager.StopBackgroundSong();
            IOManager.SaveGameAndZone(sender, commandCenter.GetCurrentSettings, gameMap.SlotList, GameManager.GetConsumptionCoverage(GameManager.CurrentYear), GameManager.ResearchList);
            SoundManager.StartBackgroundSong();
        }

        void DisplayManager_OnGameInterfaceLoaded(object sender, EventArgs e)
        {
            Debug.AddToLog("Interfata loaded");
            InstanciateInterface(IOOperation.GameMap_Instance);
            //InstanciateInterfaceWrapper(IOOperation.GameMap_Instance);
        }
        #endregion

        #region GameMap
        void CreateGameMap()
        {
            CameraFreeze();
            gameMap = new GameMap(this.Game,IOManager.CurrentZone);          
            gameMap.StackOrder = 0;
            gameMap.OnMousePress += new EventHandler<MouseEventArgs>(gameMap_OnMousePress);
            gameMap.OnMouseMove += new EventHandler<MouseEventArgs>(gameMap_OnMouseMove);
            gameMap.OnMouseRelease += new EventHandler<MouseEventArgs>(gameMap_OnMouseRelease);
            root.AddChild(gameMap);
            GUI.UpdateMinimapSize(gameMap.Width, gameMap.Height);
            GUI.UpdateMinimapCamera(new Point((int)camera.Position.X, (int)camera.Position.Y),
                                new Point(camera.Screen.Width, camera.Screen.Height));
        }

        void gameMap_OnMouseRelease(object sender, MouseEventArgs e)
        {
            camera.CameraStatus = CameraStatus.Stopped;
            camera.Position = new Vector2((float)Math.Round(camera.Position.X), (float)Math.Round(camera.Position.Y));
        }

        void gameMap_OnMouseMove(object sender, MouseEventArgs e)
        {
            #region Camera Drag
            if (camera.CameraStatus == CameraStatus.Dragged)
            {
                Vector2 add = new Vector2((float)(e.ScreenPosition.X - camera.MouseDraggingPosition.X),
                    (float)(e.ScreenPosition.Y - camera.MouseDraggingPosition.Y));
                if (gameMap.MapBounds.Contains(new Rectangle(
                                (int)(camera.Position.X - add.X / 2),
                                (int)(camera.Position.Y - add.Y / 2),
                                camera.Screen.Width,
                                camera.Screen.Height
                            )
                        )
                    )
                {
                    //Vector2 newPosition = camera.Position - add / 2;
                    camera.Position -= add / 2;
                    camera.MouseDraggingPosition = e.ScreenPosition;
                }
                else
                {
                    camera.CameraStatus = CameraStatus.Stopped;
                    camera.Position = new Vector2((float)Math.Round(camera.Position.X), (float)Math.Round(camera.Position.Y));
                }
            }
            else
            {
            #endregion

            #region Mouse Scroll
                Point mouseScreenPos = new Point(e.ScreenPosition.X - (int)camera.Position.X, e.ScreenPosition.Y - (int)camera.Position.Y);
                Rectangle noMoveArea = new Rectangle(10, 10, camera.Screen.Width - 20, camera.Screen.Height - 20);
                if (noMoveArea.Contains(mouseScreenPos))
                {
                    if (camera.CameraStatus == CameraStatus.MouseScrolled || camera.CameraStatus == CameraStatus.Stopped)
                        camera.Direction = CameraDirection.None;
                }
                else
                {
                    if (mouseScreenPos.X < noMoveArea.X)
                        if (mouseScreenPos.Y < noMoveArea.Y)
                            camera.Direction = CameraDirection.UpLeft;
                        else
                            if (mouseScreenPos.Y > noMoveArea.Height)
                                camera.Direction = CameraDirection.DownLeft;
                            else
                                camera.Direction = CameraDirection.Left;
                    else
                        if (mouseScreenPos.Y < noMoveArea.Y)
                            if (mouseScreenPos.X > noMoveArea.Width)
                                camera.Direction = CameraDirection.UpRight;
                            else camera.Direction = CameraDirection.Up;
                        else
                            if (mouseScreenPos.Y > noMoveArea.Height)
                                if (mouseScreenPos.X > noMoveArea.Width)
                                    camera.Direction = CameraDirection.DownRight;
                                else camera.Direction = CameraDirection.Down;
                            else
                                if (mouseScreenPos.X > noMoveArea.Width)
                                    camera.Direction = CameraDirection.Right;
                    camera.CameraStatus = CameraStatus.MouseScrolled;
                }
                
            }
            #endregion
        }

        void gameMap_OnMousePress(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.RightButton)
            {
                camera.CameraStatus = CameraStatus.Dragged;
                camera.MouseDraggingPosition = e.ScreenPosition;
            }
        }
        #endregion

        #region Root
        void CreateRoot()
        {
            root = new InputVisualComponent(this.Game);
            root.StackOrder = 0;
            root.StartingDrawOrder = 50;
            root.X = 0;
            root.Y = 0;
            root.XRelative = 0;
            root.YRelative = 0;

            root.OnStackChanged += new EventHandler(root_OnStackChanged);
            root.OnStackOrderChanged += new EventHandler(root_OnStackChanged);
            root.OnMouseRelease += new EventHandler<MouseEventArgs>(root_OnMouseRelease);
        }

        void root_OnMouseRelease(object sender, MouseEventArgs e)
        {
            if (gameInterface != null)
            {
                gameInterface.ReleaseSlider();
            }
        }

        void root_OnStackChanged(object sender, EventArgs e)
        {
            root.StartingDrawOrder = 0;
        }
        #endregion

        #region Preloader
        void CreatePreloader()
        {
            Rectangle dimensions = new Rectangle(0, 0, this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth, this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            preloader = new Preloader(this.Game, dimensions);
            preloader.StackOrder = 10;
            preloader.StartingDrawOrder = 0;
            preloader.Visible = false;
            preloader.Enabled = false;
            preloader.X = 0;
            preloader.Y = 0;
            preloader.On_PreloaderTimerStopped += new EventHandler(preloader_On_PreloaderTimerStopped);
            preloader.SetPercentage();
        }

        void preloader_On_PreloaderTimerStopped(object sender, EventArgs e)
        {
            if (gameMap != null)
            CameraUnfreeze();
        }

        public void ShowPreloaderTimerMode(IOOperation operation)
        {
            CameraFreeze();
            preloader.Visible = true;
            preloader.Enabled = true;
            preloader.Width = camera.Screen.Width;
            preloader.Height = camera.Screen.Height;
            preloader.X = (int)camera.Position.X;
            preloader.Y = (int)camera.Position.Y;
            preloader.Status = operation;
            preloader.TimerMode();
        }
        #endregion

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
            CreatePreloader();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        /// <summary>
        /// Creates a new GameInterface_Graphics or GameMap instance in a separate thread
        /// or (loades a new profile and shows the CommandCenter) using a separate thread
        /// </summary>
        /// <param name="operation"></param>
        public void InstanciateInterface(IOOperation operation)
        {
            preloader.Visible = true;
            preloader.Enabled = true;
            preloader.Status = operation;
            ThreadPool.QueueUserWorkItem(new WaitCallback(InstanciateInterfaceWrapper), operation);
            //InstanciateInterfaceWrapper(operation);
        }

        /// <summary>
        /// A wrapper of the CreateAndShowCommandCenter/CreateAndShowGUI/CreateGameMap method, 
        /// called by the Thread.QueueUserWorkItem method.
        /// </summary>
        private void InstanciateInterfaceWrapper(Object data)
        {
            if ((IOOperation)data == IOOperation.GameInterface_Instance)
            {
                preloader.Status = ((IOOperation)data);
                CreateAndShowGUI();
            }
            if ((IOOperation)data == IOOperation.GameMap_Instance)
            {
                CreateGameMap();
            }
            if ((IOOperation)data == IOOperation.CommandCenter)
            {
                ShowPreloaderTimerMode((IOOperation)data);
                commandCenter.Show();
                ProfilesManager.LoadProfile();
            }
        }

        /// <summary>
        /// The GameMap and GameInterface_Graphics instances are removed
        /// and the CommandCenter is shown
        /// </summary>
        public void ReturnToCommandCenter()
        {
            CameraFreeze();
            SoundManager.StopBackgroundSong();

            camera.ChangeResolution(1024, 768);
            camera.Fullscreen = false;
            camera.Position = new Vector2(0, 0);
            commandCenter.Show();
            
            IOManager.SaveGameAndZone(this, commandCenter.GetCurrentSettings, gameMap.SlotList, GameManager.GetConsumptionCoverage(GameManager.CurrentYear), GameManager.ResearchList);

            root.RemoveChild(gameMap);
            Game.Services.RemoveService(typeof(GameMap));
            gameMap = null;
            root.RemoveChild(gameInterface);
            Game.Services.RemoveService(typeof(GameInterface));

            ProfilesManager.LoadProfile();
            CameraFreeze();
        }

        /// <summary>
        /// A new GameInterface_Graphics instance is created in a new thread and after it is created
        /// a new in GameMap instance is created in a new thread
        /// </summary>
        public void DisplayGame()
        {
            InstanciateInterface(IOOperation.GameInterface_Instance);
        }

        public void ChangePreloaderPercent(double percent)
        {
            preloader.Percent = percent;
        }

        public void GameMap_HidePreloader()
        {
            preloader.Visible = false;
            preloader.Enabled = false;
            CameraUnfreeze();
        }
        
        public void CameraFreeze()
        {
            camera.Freeze();
        }

        public void CameraUnfreeze()
        {
            camera.Unfreeze();
        }

    }
}