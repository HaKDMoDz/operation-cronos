using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.Input;
using Operation_Cronos.IO;

namespace Operation_Cronos
{
    class MainMenuUser:DrawableGameComponent
    {
        #region Fields

        SpriteFont Font;

        /// <summary>
        /// The string to draw
        /// </summary>
        string UserString = string.Empty;
        /// <summary>
        /// The screen positions of the string to draw
        /// </summary>
        Vector2 UserStringPosition;
        /// <summary>
        /// Weather the string will be active or not (active = drawn and will generate events)
        /// </summary>
        bool isActive = true;
        /// <summary>
        /// The height of a character, in pixels
        /// </summary>
        int FontHeight = 13;              //from MainMenu.spritefont
        /// <summary>
        /// The width of a character, in pixels
        /// </summary>
        int CharWidth = 10;
        /// <summary>
        /// The number of pixels between characters
        /// </summary>
        int CharSpacing = 0;              //from MainMenu.spritefont
        /// <summary>
        /// The color of the text, after mouse click
        /// </summary>
        Color ClickColor = Color.Chartreuse;
        /// <summary>
        /// The default color of the text
        /// </summary>
        Color DefaultColor = Color.MediumSeaGreen;
        /// <summary>
        /// The color of string drawn
        /// </summary>
        Color DrawColor = Color.MediumSeaGreen;
        /// <summary>
        /// Indicates weather the user string was clicked on and therefore selected
        /// </summary>
        bool UserIsSelected = false;

        MouseManager input;

        ContentManager Content;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the user string
        /// </summary>
        public string UserName
        {
            get { return UserString;}
            set { UserString = value; }
        }

        /// <summary>
        /// Gets or sets the screen positions of the user string
        /// </summary>
        public Vector2 Position
        {
            get { return UserStringPosition; }
            set { UserStringPosition = value; }
        }

        /// <summary>
        /// Gets the status of the user string (selected/notSelected)
        /// </summary>
        public bool isSelected
        {
            get { return UserIsSelected; }
        }
        #endregion

        #region Events
        public event EventHandler OnUserClick = delegate { };
        #endregion

        #region Constructors
        public MainMenuUser(Game game, string user, int drawOrder, int X, int Y)
            : base(game)
        {
            UserString = user;
            UserStringPosition = new Vector2(X, Y);
            this.DrawOrder = drawOrder;

            //adaptation for classes of the main menu, for they do not derive from VisualComponent

            //FontsCollection fontCollection = ((FontsCollection)Game.Services.GetService(typeof(FontsCollection))).GetPack(;
            Font = ((FontsCollection)Game.Services.GetService(typeof(FontsCollection))).GetPack("MainMenu").Font;
            //Font = Game.Content.LoadSettings<SpriteFont>("MainMenu"); old loading version  
            //-----------------------------------        

            input = new MouseManager(game);
            input.OnPress += new EventHandler<MouseEventArgs>(Do_ClickEventLaunch);

            Game.Components.Add(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Stops drawing the string (and will stop generating events)
        /// </summary>
        public void Hide()
        {
            isActive = false;
            UserIsSelected = false;
        }

        /// <summary>
        /// Restarts drawing the string (and will start generating events)
        /// </summary>
        public void Show()
        {
            isActive = true;
        }
        #endregion

        #region Event Handlers
        void Do_ClickEventLaunch(object sender, MouseEventArgs e)
        {
            if (isActive)
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (e.ScreenPosition.X >= UserStringPosition.X && e.ScreenPosition.X <= (UserStringPosition.X + UserString.Length * CharWidth+(UserString.Length-1)*CharSpacing) &&
                        e.ScreenPosition.Y >= UserStringPosition.Y && e.ScreenPosition.Y <= (UserStringPosition.Y + FontHeight))
                    {
                        DrawColor = ClickColor;
                        UserIsSelected = true;
                        OnUserClick(this, new EventArgs());
                    }
                    else
                    {
                        //any outside the user string surface click visualy deactivates the click effect
                        DrawColor = DefaultColor;
                        UserIsSelected = false;
                    }
                }
        }
        #endregion

        #region Overrides
        public override void Initialize()
        {
            Content = new ContentManager(Game.Services);
            Content.RootDirectory = "Content";
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (isActive)
            {
                SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
                spriteBatch.DrawString(Font, UserString, UserStringPosition, DrawColor);
            }
            base.Draw(gameTime);
        }
        #endregion
    }
}