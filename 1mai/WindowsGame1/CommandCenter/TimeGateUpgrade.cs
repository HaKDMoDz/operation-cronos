using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos
{
    public class TimeGateUpgrade : Button
    {
        #region Fields
        Sprite ButtonSprite;
        int animationSpeed = 17;
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
        #endregion

        #region Constructor
        public TimeGateUpgrade(Game game, Sprite sprite, int drawOrder)
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
            ButtonSprite.FrameNumber = 0;
            ButtonSprite.Visible = true;
        }

        public void Hide()
        {
            this.Enabled = false;
            ButtonSprite.Visible = false;
        }

        public override void PressAnimation()
        {
            ButtonSprite.PauseAnimation();
        }
        public override void ReleaseAnimation()
        {
        }
        public override void MouseLeaveAnimation()
        {
            ButtonSprite.PauseAnimation();
        }
        public override void MouseOverAnimation()
        {
            ButtonSprite.AnimationSpeed = animationSpeed;
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
