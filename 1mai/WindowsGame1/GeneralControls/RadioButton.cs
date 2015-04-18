using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos
{
    class RadioButton: Button
    {
        #region Fields

        Sprite ButtonSprite;
        Sprite Box;

        bool isChecked = false;
        bool allowDirectUncheck = true;

        /// <summary>
        /// The Radio's name
        /// </summary>
        String name = "";
        #endregion

        #region Properties
        /// <summary>
        /// Gets the status of the Radio Button
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
        }

        /// <summary>
        /// If set to 'false', once checked the Radio will remain checked, unless you uncheck it using the Uncheck method
        /// (useful when creating an interdependent group of Radios)
        /// </summary>
        public bool AllowDirectUncheck
        {
            set { allowDirectUncheck = value; }
        }

        /// <summary>
        /// Gets or Sets the Radio Button's name
        /// </summary>
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region Events
        public event EventHandler OnCheck = delegate { };
        public event EventHandler OnUncheck = delegate { };
        #endregion

        #region Constructors 
        /// <summary>
        /// RadioButton Constructor
        /// </summary>
        /// <param name="ButtonSprite">A two frames Sprite which will act as a button for checking/unchecking the Radio Box</param>
        /// <param name="Box">A two frames Sprite which will be the Radio Box</param>
        /// <param name="positions">A 4 size int vector containing the coordinates btnSprite.X, btnSprite.Y, box.X, box.Y</param>
        public RadioButton(Game game, Sprite btnSprite, Sprite box, int DrawOrder, int[] positions)
            : base(game)
        {
            ButtonSprite = btnSprite;
            ButtonSprite.StackOrder = DrawOrder;
            ButtonSprite.XRelative = positions[0];
            ButtonSprite.YRelative = positions[1];
            AddChild(ButtonSprite);

            Box = box;
            Box.StackOrder = DrawOrder;
            Box.XRelative = positions[2];
            Box.YRelative = positions[3];
            AddChild(Box);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Hides the Radio Button
        /// </summary>
        public void Hide()
        {
            ButtonSprite.Visible = false;
            ButtonSprite.Enabled = false;
            Box.Visible = false;
            Box.Enabled = false;

            this.Enabled = false;
        }

        /// <summary>
        /// Shows the Radio Button
        /// </summary>
        public void Show()
        {
            ButtonSprite.Visible = true;
            ButtonSprite.Enabled = true;
            Box.Visible = true;
            Box.Enabled = true;

            this.Enabled = true;
        }

        /// <summary>
        /// Checks the Radio Button
        /// </summary>
        public void Check()
        {
            PressAnimation();
        }

        /// <summary>
        /// Unchecks the RadionButton
        /// </summary>
        public void Uncheck()
        {
            Box.FrameNumber = 0;
            isChecked = false;
        }

        public override void PressAnimation()
        {
            if (allowDirectUncheck)
            {
                isChecked = !isChecked;
                if (isChecked)
                {
                    Box.FrameNumber = 1;
                    OnCheck(this, new EventArgs());
                }
                else
                {
                    Box.FrameNumber = 0;
                    OnUncheck(this, new EventArgs());
                }
            }
            else
            {
                if (!isChecked)
                {
                    isChecked = true;
                    Box.FrameNumber = 1;
                    OnCheck(this, new EventArgs());
                }
            }
        }
        public override void ReleaseAnimation()
        {
        }
        public override void MouseLeaveAnimation()
        {
            ButtonSprite.FrameNumber = 0;
        }
        public override void MouseOverAnimation()
        {
            ButtonSprite.FrameNumber = 1;
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
