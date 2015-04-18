using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos {

    public class ScrollbarArrow : Button
    {
        #region Fields
        Sprite Arrow;
        #endregion

        #region Properties
        public override Point Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                Arrow.Position = value;
                Arrow.XRelative = value.X;
                Arrow.YRelative = value.Y;
                base.Position = value;
            }
        }
        #endregion

        #region Constructors
        public ScrollbarArrow(Game game, Sprite arrow, int drawOrder)
            : base(game)
        {
            Arrow = arrow;
            Arrow.XRelative = 0;
            Arrow.YRelative = 0;
                        
            this.StackOrder = drawOrder;
            AddChild(Arrow);
        }
        #endregion

        #region Methods
        public void Show()
        {
            this.Enabled = true;
            Arrow.FrameNumber = 0;
            Arrow.Visible = true;
        }

        public void Hide()
        {
            this.Enabled = false;
            Arrow.Visible = false;
        }

        public override void PressAnimation()
        {
            Arrow.FrameNumber = 1;
        }
        public override void ReleaseAnimation()
        {
            Arrow.FrameNumber = 0;
        }
        public override void MouseLeaveAnimation()
        {
            Arrow.FrameNumber = 0;
        }
        public override void MouseOverAnimation()
        {
            Arrow.FrameNumber = 1;
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

    public class Scrollbar : Button
    {
        #region Fields

        Sprite BarSprite;
        public ScrollbarArrow UpArrow;
        public ScrollbarArrow DownArrow;

        /// <summary>
        /// Shows weather the scrollbar has a bar between the up and down arrow or not
        /// </summary>
        bool UseBar;

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
        public Scrollbar(Game game, Sprite Bar, Sprite upArrow, Sprite downArrow,int drawOrder ,int[] positions)
            : base(game)
        {
            BarSprite = Bar;
            BarSprite.XRelative = positions[0];
            BarSprite.YRelative = positions[1];
            this.StackOrder = drawOrder;

            UpArrow  = new ScrollbarArrow(game, upArrow, drawOrder);
            UpArrow.Position = new Point(positions[2],positions[3]);
            UpArrow.OnPress+= new EventHandler<ButtonEventArgs>(Do_ScrollUp);

            DownArrow  = new ScrollbarArrow(game, downArrow, drawOrder);
            DownArrow.Position = new Point(positions[4],positions[5]);
            DownArrow.OnPress+= new EventHandler<ButtonEventArgs>(Do_ScrollDown);

            UseBar = true;

             AddChild(BarSprite);
			 AddChild(UpArrow);
			 AddChild(DownArrow);
        }

        /// <summary>
        /// Scrollbar 2 Sprites constructor (without Scrollbar bar sprite)
        /// </summary>
        /// <param name="game"></param>
        /// <param name="UpArrow">The scrollbar up arrow 2frames-sprite</param>
        /// <param name="DownArrow">The scrollbar down arrow 2frames-sprite</param>
        /// <param name="drawOrder">The Scrollbar draw order</param>
        /// <param name="positions">A size 4 int vector for UpArrow(X,Y), DownArrow(X,Y)</param>
        public Scrollbar(Game game, Sprite upArrow, Sprite downArrow,int drawOrder ,int[] positions)
            : base(game)
        {
            UpArrow  = new ScrollbarArrow(game, upArrow, drawOrder);
            UpArrow.Position = new Point(positions[0],positions[1]);
            UpArrow.OnPress+= new EventHandler<ButtonEventArgs>(Do_ScrollUp);

            DownArrow  = new ScrollbarArrow(game, downArrow, drawOrder);
            DownArrow.Position = new Point(positions[2],positions[3]);
            DownArrow.OnPress+= new EventHandler<ButtonEventArgs>(Do_ScrollDown);

            UseBar = false;

            this.StackOrder = drawOrder;
			
			AddChild(UpArrow);
			AddChild(DownArrow);
        }
        #endregion

        #region Event Handlers

        void Do_ScrollUp(object sender, ButtonEventArgs e)
        {
            OnScrollUp(this, new EventArgs());
        }

        void Do_ScrollDown(object sender, ButtonEventArgs e)
        {
            OnScrollDown(this, new EventArgs());
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
                this.Enabled = false;
                UpArrow.Hide();
                DownArrow.Hide();

                if (UseBar)
                    BarSprite.Visible = false;
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
                this.Enabled = true;
                UpArrow.Show();
                DownArrow.Show();
                                
                if (UseBar)
                    BarSprite.Visible = true;
            }
            catch { }
        }

        public override void PressAnimation()
        {
        }
        public override void ReleaseAnimation()
        {
        }
        public override void MouseLeaveAnimation()
        {
        }
        public override void MouseOverAnimation()
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