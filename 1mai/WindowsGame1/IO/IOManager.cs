using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.Profiles;
using Operation_Cronos.Display;


namespace Operation_Cronos.IO
{
    public partial class IOManager : GameComponent
    {

        public const string MainMenuXML = "MainMenuAnimations.xml";
        public const string FontsXML = "fonts.xml";
        public const string CommandCenterXML = "CommandCenterAnimations.xml";
        public const string GameInterfaceXML = "Interface.xml";
        public const string GameMapXML = "ConstructionTypes.xml";

        /// <summary>
        /// A collection with all the TexturePacks in the game.
        /// </summary>
        private GraphicsCollection graphicsCollection;
        private FontsCollection fontsCollection;

        private string currentZone = "";

        public Timer AutosaveTimer;

        #region Properties
        /// <summary>
        /// A collection with all the TexturePacks in the game.
        /// </summary>
        public GraphicsCollection Graphics
        {
            get
            {
                return graphicsCollection;
            }
            set
            {
                graphicsCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently loading zone
        /// </summary>
        public string CurrentZone
        {
            get
            {
                return currentZone;
            }
            set
            {
                currentZone = value;
            }

        }

        private ProfileManager ProfileManager
        {
            get { return (ProfileManager)Game.Services.GetService(typeof(ProfileManager)); }
        }

        private DisplayManager DisplayManager
        {
            get { return (DisplayManager)Game.Services.GetService(typeof(DisplayManager)); }
        }

        private CommandCenter CommandCenter
        {
            get { return (CommandCenter)Game.Services.GetService(typeof(CommandCenter)); }
        }

        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }
        #endregion

        public IOManager(Game game)
            : base(game)
        {
            DisplayManager.CameraFreeze();
            graphicsCollection = new GraphicsCollection(game);
            Game.Services.AddService(typeof(GraphicsCollection), graphicsCollection);
            graphicsCollection.OnProgress += new EventHandler<IOEventArgs>(graphicsCollection_OnProgress);
            graphicsCollection.OnComplete += new EventHandler<IOEventArgs>(graphicsCollection_OnComplete);
            graphicsCollection.OnStart += new EventHandler<IOEventArgs>(graphicsCollection_OnStart);

            fontsCollection = new FontsCollection(game);
            Game.Services.AddService(typeof(FontsCollection), fontsCollection);
            fontsCollection.OnStart += new EventHandler<IOEventArgs>(fontsCollection_OnStart);
            fontsCollection.OnProgress += new EventHandler<IOEventArgs>(fontsCollection_OnProgress);
            fontsCollection.OnComplete += new EventHandler<IOEventArgs>(fontsCollection_OnComplete);
            
            AutosaveTimer = new Timer(game);
            AutosaveTimer.IntervalType = TimerIntervalType.Seconds;
            AutosaveTimer.Interval = 0;
            AutosaveTimer.OnTick += new EventHandler(AutosaveTimer_OnTick);

            Game.Services.AddService(typeof(IOManager), this);
        }
        
        public void LoadFonts()
        {
            XMLLoader fontsXml = new XMLLoader(IOManager.FontsXML);
            fontsCollection.AddFontsFromXml(fontsXml.Document, IOOperation.MainMenu, IOContent.Fonts);
        }

        public void LoadMainMenuGraphics()
        {
            XMLLoader main_menuXml = new XMLLoader(IOManager.MainMenuXML);
            graphicsCollection.AddCollectionFromXml(main_menuXml.Document, IOOperation.MainMenu, IOContent.Graphics);
        }

        public void LoadCommandCenterGraphics()
        {
            XMLLoader command_centerXml = new XMLLoader(IOManager.CommandCenterXML);
            graphicsCollection.AddCollectionFromXml(command_centerXml.Document, IOOperation.CommandCenter, IOContent.Graphics);
        }

        public void LoadGameInterfaceGraphics(string Zone)
        {
            XMLLoader xml = new XMLLoader(IOManager.GameInterfaceXML);
            graphicsCollection.AddCollectionFromXml(xml.Document, IOOperation.GameInterface_Graphics, IOContent.Graphics);

            CurrentZone = Zone;
            DisplayManager.GameInterfaceLoaded = true;
        }

        public void LoadGameMapGraphics()
        {
            XMLLoader xml = new XMLLoader(IOManager.GameMapXML);
            graphicsCollection.AddCollectionFromXml(xml.Document, IOOperation.GameMap_Graphics, IOContent.Graphics);
        }

        #region Graphics Event Handlers
        void graphicsCollection_OnProgress(object sender, IOEventArgs e)
        {
            DisplayManager.GraphicsProgress(sender, e);
        }
        void graphicsCollection_OnComplete(object sender, IOEventArgs e)
        {
            DisplayManager.GraphicsComplete(sender, e);
        }
        void graphicsCollection_OnStart(object sender, IOEventArgs e)
        {
            DisplayManager.GraphicsStart(sender, e);
        }
        #endregion

        #region Fonts Event Handlers
        void fontsCollection_OnComplete(object sender, IOEventArgs e)
        {
            DisplayManager.FontsComplete(sender, e);
        }

        void fontsCollection_OnProgress(object sender, IOEventArgs e)
        {
            DisplayManager.FontsProgress(sender, e);
        }

        void fontsCollection_OnStart(object sender, IOEventArgs e)
        {
            DisplayManager.FontsStart(sender, e);
        }
        #endregion

        #region Autosave timer Event Handler
        void AutosaveTimer_OnTick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            SaveGame(sender, CommandCenter.GetCurrentSettings);
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