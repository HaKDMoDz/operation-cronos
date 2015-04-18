using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos
{
    public class CommandCenterRightButton : Button
    {
        #region Fields
        Sprite ButtonSprite;

        SpriteText spriteText;
        Color MouseOverColor = Color.White;
        Color MouseLeaveColor = Color.DarkGray;

        public bool ButtonIsPressed = false;

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
                ButtonSprite.Position = value;
                ButtonSprite.XRelative = value.X;
                ButtonSprite.YRelative = value.Y;
                base.Position = value;
            }
        }

        /// <summary>
        /// Sets the position of the text, inside the Button
        /// </summary>
        public Point TextPosition
        {
            set
            {
                spriteText.XRelative = value.X;
                spriteText.YRelative = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the text shown over the button
        /// </summary>
        public string Text
        {
            get
            {
                return spriteText.Text;
            }
            set
            {
                spriteText.Text = value;
            }
        }
        #endregion

        #region Constructor
        public CommandCenterRightButton(Game game, Sprite sprite, int drawOrder)
            : base(game)
        {
            ButtonSprite = sprite;

            ButtonSprite.XRelative = 0;
            ButtonSprite.YRelative = 0;

            this.StackOrder = drawOrder;

            spriteText = new SpriteText(game, FontsCollection.GetPack("Calibri 8").Font);
            spriteText.Text = "";
            spriteText.XRelative = 0;
            spriteText.YRelative = 0;
            spriteText.Tint = MouseLeaveColor;
            spriteText.StackOrder = drawOrder + 1;

            AddChild(ButtonSprite);
            AddChild(spriteText);
        }
        #endregion

        #region Methods
        /// <summary>
        ///Shows the button (but doesn't enable it)
        /// </summary>
        public void Show()
        {
            ButtonSprite.Visible = true;
            spriteText.Visible = true;
            spriteText.Tint = MouseLeaveColor;
        }

        /// <summary>
        /// Hides the button and also disables it
        /// </summary>
        public void Hide()
        {
            this.Enabled = false;
            ButtonSprite.Visible = false;
            spriteText.Visible = false;
        }

        /// <summary>
        /// Unpresses the button when needed (when another button in this category is pressed)
        /// </summary>
        public void UnpressButton()
        {
            ButtonIsPressed = false;
            spriteText.Tint = MouseLeaveColor;
        }

        /// <summary>
        /// Hides the text of the button and disables the button(when there is no zone selected)
        /// </summary>
        public void Disable()
        {
            ButtonIsPressed = false;
            spriteText.Visible = false;

            this.Enabled = false;
        }

        /// <summary>
        /// Shows the text of the button and enables the button (when a zone was selected)
        /// </summary>
        public void Enable()
        {
            spriteText.Visible = true;
            spriteText.Tint = MouseLeaveColor;

            this.Enabled = true;
        }

        public override void PressAnimation()
        {
                ButtonIsPressed = true;
                spriteText.Tint = MouseOverColor;
        }
        public override void ReleaseAnimation()
        {
        }
        public override void MouseLeaveAnimation()
        {
            if (!ButtonIsPressed)
                spriteText.Tint = MouseLeaveColor;
        }
        public override void MouseOverAnimation()
        {
            spriteText.Tint = MouseOverColor;
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
