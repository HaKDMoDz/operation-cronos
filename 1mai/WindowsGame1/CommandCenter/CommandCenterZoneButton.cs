using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Operation_Cronos.Sound;

namespace Operation_Cronos
{
    public class CommandCenterZoneButton: Button
    {
        #region Fields
        Sprite ButtonSprite;
        Sprite EffectSprite;

        int speed_EffectSprite = 17;

        //the name of the Zone
        string name = "";

        //the status of the Current Zone (initially locked==true)
        bool locked;

        /// <summary>
        /// The " ZONE X Locked" text
        /// </summary>
        string lockedString = "";
        /// <summary>
        /// The text to display when the Description button is pressed for the Current Zone
        /// </summary>
        string descriptionString = "";

        /// <summary>
        /// The text to display when the Mission Briefing button is pressed fot the Current Zone 
        /// </summary>
        string missionBriefingString = "";

        /// <summary>
        /// The text to display when the Rewards button is pressed fot the Current Zone
        /// </summary>
        string rewardsString = "";

        /// <summary>
        /// The text to display when the Parameters button is pressed fot the Current Zone
        /// </summary>
        string parametersString = "";

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
                ButtonSprite.Position= value;
                ButtonSprite.XRelative = value.X;
                ButtonSprite.YRelative = value.Y;
                EffectSprite.Position = value;
                EffectSprite.XRelative = value.X;
                EffectSprite.YRelative = value.Y;
                base.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Zone; Also sets the lockedString
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
                lockedString = "\n\n\n\n                    " + name + "\n\n                    " + "Locked";
            }
        }

        /// <summary>
        /// Gets or sets the status of the zone (locked or unlocked)
        /// </summary>
        public bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                locked = value;
            }

        }

        /// <summary>
        /// Gets or sets the text to display when the Description button is pressed 
        /// for the Current Zone. 
        /// If the Zone is locked the lockedString will be returned (for Get) or will be Set
        /// </summary>
        public string DescriptionString
        {
            get
            {
                if (locked==false)
                    return descriptionString;
                else
                    return lockedString;
            }
            set //works only if the Zone is unlocked
            {
                if (locked == false)
                    descriptionString = value;
                else
                    descriptionString = lockedString;
            }
        }

        /// <summary>
        /// Gets or sets the text to display when the Mission Briefing button is pressed 
        /// for the Current Zone. 
        /// If the Zone is locked the lockedString will be returned (for Get) or will be Set
        /// </summary>
        public string MissionBriefingString
        {
            get
            {
                if (locked == false)
                    return missionBriefingString;
                else
                    return lockedString;
            }
            set //works only if the Zone is unlocked
            {
                if (locked == false)
                    missionBriefingString = value;
                else
                    missionBriefingString = lockedString;
            }
        }

        /// <summary>
        /// Gets or sets the text to display when the Rewards button is pressed 
        /// for the Current Zone. 
        /// If the Zone is locked the lockedString will be returned (for Get) or will be Set
        /// </summary>
        public string RewardsString
        {
            get
            {
                if (locked == false)
                    return rewardsString;
                else
                    return lockedString;
            }
            set //works only if the Zone is unlocked
            {
                if (locked == false)
                    rewardsString = value;
                else
                    rewardsString = lockedString;
            }
        }

        /// <summary>
        /// Gets or sets the text to display when the Parameters button is pressed 
        /// for the Current Zone. 
        /// If the Zone is locked the lockedString will be returned (for Get) or will be Set
        /// </summary>
        public string ParametersString
        {
            get
            {
                if (locked == false)
                    return parametersString;
                else
                    return lockedString;
            }
            set //works only if the Zone is unlocked
            {
                if (locked == false)
                    parametersString = value;
                else
                    parametersString = lockedString;
            }
        }

        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }
        #endregion

        #region Constructor
        public CommandCenterZoneButton(Game game, Sprite sprite, Sprite effect, int drawOrder)
            : base(game)
        {
            ButtonSprite = sprite;

            ButtonSprite.XRelative = 0;
            ButtonSprite.YRelative = 0;

            EffectSprite = effect;
            EffectSprite.AnimationSpeed = 0;
            EffectSprite.Visible = false;
            EffectSprite.XRelative = 0;
            EffectSprite.YRelative = 0;

            this.StackOrder = drawOrder;

            locked = true; //the zone is initially locked

            AddChild(ButtonSprite);
            AddChild(EffectSprite);           
        }
        #endregion

        #region Methods
        public void Show()
        {
            Enabled = true;
            ButtonSprite.Visible = true;

            this.Enabled = true;
        }

        public void Hide()
        {
            this.Enabled = false;

            ButtonSprite.Visible = false;
            EffectSprite.Visible = false;
        }

        public override void PressAnimation()
        {
            if (EffectSprite.AnimationSpeed > 0)
                EffectSprite.PauseAnimation();
            else
            {
                EffectSprite.AnimationSpeed = speed_EffectSprite;
                EffectSprite.Visible = true;
            }
        }
        public override void ReleaseAnimation()
        {
        }
        public override void MouseLeaveAnimation()
        {
            if (EffectSprite.AnimationSpeed > 0)
            {
                EffectSprite.PauseAnimation();
                EffectSprite.Visible = false;
            }
        }
        public override void MouseOverAnimation()
        {
            if (EffectSprite.Visible == false)
            {
                SoundManager.PlaySound(Sounds.CommandCenterZone);
                EffectSprite.FrameNumber = 0;
                EffectSprite.Visible = true;
                EffectSprite.AnimationSpeed = speed_EffectSprite;
                
            }
        }

        /// <summary>
        /// Visually, the button will be seen as 'MouseOver' and not as 'MouseClick'
        /// </summary>
        public void ReleaseZoneButton()
        {
            EffectSprite.AnimationSpeed = speed_EffectSprite;
            EffectSprite.Visible = true;
        }

        /// <summary>
        /// Visually, the button will be seen as 'MouseLeave'
        /// </summary>
        public void DeactivateZoneButton()
        {
            EffectSprite.PauseAnimation();
            EffectSprite.Visible = false;
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


