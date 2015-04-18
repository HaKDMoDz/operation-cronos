using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Operation_Cronos.Sound;

namespace Operation_Cronos
{
    public class CommandCenterGeneralButton: Button
    {
        #region Fields
        Sprite ButtonSprite;

        bool isPressed = false;

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
        /// Shows whether the button is curently pressed or not
        /// </summary>
        public bool IsPressed
        {
            get { return isPressed;}
        }

        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager));}
        }
        #endregion

        #region Constructor
        public CommandCenterGeneralButton(Game game, Sprite sprite, int drawOrder)
            : base(game)
        {
            ButtonSprite = sprite;

            ButtonSprite.XRelative = 0;
            ButtonSprite.YRelative = 0;

            this.StackOrder = drawOrder;

            AddChild(ButtonSprite);
        }
        #endregion

        #region Methods
        public void Show()
        {
            this.Enabled = true;
            ButtonSprite.Visible = true;
            ButtonSprite.Enabled = true;
        }

        public void Hide()
        {
            this.Enabled = false;
            ButtonSprite.Visible = false;
            ButtonSprite.Enabled = false;
        }

        public override void PressAnimation()
        {
            ButtonSprite.FrameNumber = 1;
            isPressed = true;
        }
        public override void ReleaseAnimation()
        {
        }
        public override void MouseLeaveAnimation()
        {
            if (!isPressed)//Button curently not pressed
            {                
                ButtonSprite.FrameNumber = 0;
            }
        }
        public override void MouseOverAnimation()
        {
            if (!isPressed)//Button curently not pressed
            {
                SoundManager.PlaySound(Sounds.CommandCenterButton);
                ButtonSprite.FrameNumber = 1;
            }
        }

        public void ReleaseButton()
        {
            ButtonSprite.FrameNumber = 0;
            isPressed = false;
        }
        #endregion

        #region Overrides
        public override void Initialize() {
            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
        #endregion
    }
}
