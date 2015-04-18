using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.Input;

namespace Operation_Cronos {

    public class MainMenuScrollbar : Microsoft.Xna.Framework.GameComponent
    {
        #region Fields

        Sprite BarSprite;
        Sprite UpArrowSprite;
        Sprite DownArrowSprite;

        /// <summary>
        /// Shows weather the scrollbar has a bar between the up and down arrow or not
        /// </summary>
        bool UseBar;

        /// <summary>
        /// Sets the scrollbar as active/inactive (inactive = invisible and deactivated)
        /// </summary>
        bool isActive;

        MouseManager input;
        #endregion

        #region Events
        public event EventHandler OnScrollUp = delegate { };
        public event EventHandler OnScrollDown = delegate { };
        #endregion

        #region Constructors
        /// <summary>
        /// Scrollbar 3 Sprites constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Bar">The scrollbar bar 1frame-sprite</param>
        /// <param name="UpArrow">The scrollbar up arrow 2frames-sprite</param>
        /// <param name="DownArrow">The scrollbar down arrow 2frames-sprite</param>
        /// <param name="drawOrder">The Scrollbar draw order</param>
        /// <param name="positions">A size 6 int vector for Bar(X,Y), UpArrow(X,Y), DownArrow(X,Y)</param>
        public MainMenuScrollbar(Game game, Sprite Bar, Sprite UpArrow, Sprite DownArrow,int drawOrder ,int[] positions)
            : base(game)
        {
            BarSprite = Bar;
            BarSprite.DrawOrder = drawOrder;
            BarSprite.X = positions[0];
            BarSprite.Y = positions[1];

            UpArrowSprite = UpArrow;
            UpArrowSprite.DrawOrder = drawOrder;
            UpArrowSprite.X = positions[2];
            UpArrowSprite.Y = positions[3];
            //UpArrowSprite.SourceRectangle = new Rectangle(UpArrowSprite.X, UpArrowSprite.Y, UpArrowSprite.Width, UpArrowSprite.Height);

            DownArrowSprite = DownArrow;
            DownArrowSprite.DrawOrder = drawOrder;
            DownArrowSprite.X = positions[4];
            DownArrowSprite.Y = positions[5];
            //DownArrowSprite.SourceRectangle = new Rectangle(positions[4], positions[4], DownArrowSprite.Width, DownArrowSprite.Height);

            input = new MouseManager(game);
            input.OnMouseMove += new EventHandler<MouseEventArgs>(input_OnMouseMove);
            input.OnPress += new EventHandler<MouseEventArgs>(input_OnPress); 

            UseBar = true;

            Game.Components.Add(this);
        }

        /// <summary>
        /// Scrollbar 2 Sprites constructor (without Scrollbar bar sprite)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="UpArrow">The scrollbar up arrow 2frames-sprite</param>
        /// <param name="DownArrow">The scrollbar down arrow 2frames-sprite</param>
        /// <param name="drawOrder">The Scrollbar draw order</param>
        /// <param name="positions">A size 4 int vector for UpArrow(X,Y), DownArrow(X,Y)</param>
        public MainMenuScrollbar(Game game, Sprite UpArrow, Sprite DownArrow,int drawOrder ,int[] positions)
            : base(game)
        {
            UpArrowSprite = UpArrow;
            UpArrowSprite.DrawOrder = drawOrder;
            UpArrowSprite.X = positions[0];
            UpArrowSprite.Y = positions[1];

            DownArrowSprite = DownArrow;
            DownArrowSprite.DrawOrder = drawOrder;
            DownArrowSprite.X = positions[2];
            DownArrowSprite.Y = positions[3];

            input = new MouseManager(game);
            input.OnMouseMove += new EventHandler<MouseEventArgs>(input_OnMouseMove);
            input.OnPress += new EventHandler<MouseEventArgs>(input_OnPress); 

            UseBar = false;

            Game.Components.Add(this);
        }
        #endregion

        #region Event Handlers

        void input_OnPress(object sender, MouseEventArgs e)
        {
            if (MouseIsOverUpArrow() && isActive)
                if (e.Button == MouseButton.LeftButton)
                    OnScrollUp(this, new EventArgs());

            if (MouseIsOverDownArrow() && isActive)
                if (e.Button == MouseButton.LeftButton)
                    OnScrollDown(this, new EventArgs());
        }

        void input_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsOverUpArrow() && isActive )//&& UpArrowSprite.Collides(e.ScreenPosition))
                UpArrowSprite.FrameNumber = 1;
            else
                UpArrowSprite.FrameNumber = 0;

            if (MouseIsOverDownArrow() && isActive)//&& DownArrowSprite.Collides(e.ScreenPosition))
                DownArrowSprite.FrameNumber = 1;
            else
                DownArrowSprite.FrameNumber = 0;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Deactivates the scrollbar
        /// </summary>
        public void Hide()
        {
            try
            {
                UpArrowSprite.Visible = false;
                DownArrowSprite.Visible = false;

                if (UseBar)
                    BarSprite.Visible = false;
                isActive = false;
            }
            catch { }
        }

        /// <summary>
        /// Activates the scrollbar
        /// </summary>
        public void Show()
        {
            try
            {
                UpArrowSprite.Visible = true;
                DownArrowSprite.Visible = true;
                                
                if (UseBar)
                    BarSprite.Visible = true;
                isActive = true;
            }
            catch { }
        }

        bool MouseIsOverUpArrow()
        {
            if (Mouse.GetState().X >= UpArrowSprite.X && Mouse.GetState().X <= (UpArrowSprite.X + UpArrowSprite.Width) &&
                Mouse.GetState().Y >= UpArrowSprite.Y && Mouse.GetState().Y <= (UpArrowSprite.Y + UpArrowSprite.Height))
                return true;
            else
                return false;
        }

        bool MouseIsOverDownArrow()
        {
            if (Mouse.GetState().X >= DownArrowSprite.X && Mouse.GetState().X <= (DownArrowSprite.X + DownArrowSprite.Width) &&
              Mouse.GetState().Y >= DownArrowSprite.Y && Mouse.GetState().Y <= (DownArrowSprite.Y + DownArrowSprite.Height))
                return true;
            else
                return false;
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