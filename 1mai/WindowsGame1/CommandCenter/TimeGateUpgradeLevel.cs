using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Operation_Cronos.IO;

namespace Operation_Cronos
{
    public enum TimeGateUpgradeStatus
    {
        Locked,
        Unlocked,
        Bought,
    }

    public class TimeGateUpgradeLevel : Button
    {
        #region Fields
        Sprite StatusSprite;
        /// <summary>
        /// A sprite consisting in an image of one of the numbers I,II,III,IV
        /// </summary>
        Sprite BoughtSprite;

        TimeGateUpgradeStatus status = TimeGateUpgradeStatus.Locked;

        GraphicsCollection graphicsCollection;

        string name = "";
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
                StatusSprite.Position = value;
                StatusSprite.XRelative = value.X;
                StatusSprite.YRelative = value.Y;

                BoughtSprite.Position = value;
                BoughtSprite.XRelative = value.X;
                BoughtSprite.YRelative = value.Y;

                base.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the current Level button
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the Status of the current Level button
        /// </summary>
        public TimeGateUpgradeStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (this.Enabled) 
                    ChangeGraphics();
            }
        }
        #endregion

        #region Constructor
        public TimeGateUpgradeLevel(Game game, Sprite boughtSprite, int drawOrder)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));

            StatusSprite = new Sprite(game, graphicsCollection.GetPack("LevelStatus"));
            StatusSprite.XRelative = 0;
            StatusSprite.YRelative = 0;

            BoughtSprite = boughtSprite;
            BoughtSprite.XRelative = 0;
            BoughtSprite.YRelative = 0;

            this.StackOrder = drawOrder;

            AddChild(StatusSprite);
            AddChild(BoughtSprite);
        }
        #endregion

        #region Methods
        public void Show()
        {
            this.Enabled = true;
            switch (status)
            {
                case TimeGateUpgradeStatus.Locked:
                    StatusSprite.FrameNumber = 0;
                    StatusSprite.Visible = true;
                    break;
                case TimeGateUpgradeStatus.Unlocked:
                    StatusSprite.FrameNumber = 1;
                    StatusSprite.Visible = true;
                    break;
                case TimeGateUpgradeStatus.Bought:
                    BoughtSprite.Visible = true;
                    break;
            }
        }

        public void Hide()
        {
            this.Enabled = false;
            StatusSprite.Visible = false;
            BoughtSprite.Visible = false;
        }

        /// <summary>
        /// Changes the Graphics of the current level, depending on its status
        /// Method is called each time the status is changes using the Status property
        /// </summary>
        private void ChangeGraphics()
        {
            switch (status)
            {
                case TimeGateUpgradeStatus.Locked:
                    StatusSprite.FrameNumber = 0;
                    StatusSprite.Visible = true;
                    BoughtSprite.Visible = false;
                    break;
                case TimeGateUpgradeStatus.Unlocked:
                    StatusSprite.FrameNumber = 1;
                    StatusSprite.Visible = true;
                    BoughtSprite.Visible = false;
                    break;
                case TimeGateUpgradeStatus.Bought:
                    StatusSprite.Visible = false;
                    BoughtSprite.Visible = true;
                    break;
            }
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
