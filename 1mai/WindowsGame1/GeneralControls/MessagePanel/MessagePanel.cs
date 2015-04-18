using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.IO;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos
{
    class MessagePanel : InputVisualComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;

        Sprite BgSprite; //a transparent stretched pixel
        Sprite PanelSprite;
        SpriteText MessageSpriteText;
        MessagePanelOKButton btnOK;

        Timer tmrOpenPanel;

        int VPadding = 15;
        int HPadding = 15;

        /// <summary>
        /// will contain the initial text, before formating it to rows
        /// </summary>
        string initialText = "";

        #endregion

        #region Events
        public event EventHandler OnOK = delegate { };
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the text shown in the MessagePanel
        /// </summary>
        public string Text
        {
            get
            {
                return MessageSpriteText.Text; ;
            }
            set
            {
                MessageSpriteText.Text = value;
                initialText = value;
                MessageSpriteText.SplitTextToRows(PanelSprite.Width - HPadding*2);
            }
        }

        /// <summary>
        /// Gets or sets the Vertical Padding of the Text inside the MessagePanel
        /// </summary>
        public int VerticalPadding
        {
            get
            {
                return VPadding;
            }
            set
            {
                //minus the old VPadding
                MessageSpriteText.YRelative -= VPadding;
                VPadding = value;
                //plus the new VPadding
                MessageSpriteText.YRelative += VPadding;
            }
        }

        /// <summary>
        /// Gets or sets the HorizontalPadding of the Text inside the MessagePanel
        /// </summary>
        public int HorizontalPadding
        {
            get
            {
                return HPadding;
            }
            set
            {
                //minus the old HPadding
                MessageSpriteText.XRelative -= HPadding;
                HPadding = value;
                //plus the new HPadding
                MessageSpriteText.XRelative += HPadding;

                //because the Text might have already been split to rows,
                //its value will be set to the initialText value (before splitting to rows)
                //and it will be split to rows again, after this
                MessageSpriteText.Text = initialText;
                MessageSpriteText.SplitTextToRows(PanelSprite.Width - HPadding*2);
            }
        }
        
        #endregion

        #region Constructor
        public MessagePanel(Game game, Rectangle screen)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));

            Color AlphaZero = new Color(99, 99, 99, 0);//Alpha = 0

            BgSprite = new Sprite(game,graphicsCollection[graphicsCollection.GetPackIndex("pixel")].Frames);
            BgSprite.XRelative = 0;
            BgSprite.YRelative = 0;
            BgSprite.Tint = AlphaZero;
            BgSprite.Width = screen.Width;
            BgSprite.Height = screen.Height;
            BgSprite.StackOrder = 1;

            PanelSprite = new Sprite(game, graphicsCollection[graphicsCollection.GetPackIndex("message_panel")].Frames);
            PanelSprite.XRelative = Convert.ToInt32(BgSprite.Width / 2 - PanelSprite.Width / 2);
            PanelSprite.YRelative = Convert.ToInt32(BgSprite.Height / 2 - PanelSprite.Height / 2);
            PanelSprite.FrameNumber = 0;
            PanelSprite.StackOrder = 3;

            MessageSpriteText = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            MessageSpriteText.MaxLength = 500;
            MessageSpriteText.Tint = Color.WhiteSmoke;
            MessageSpriteText.XRelative = PanelSprite.XRelative + HPadding;
            MessageSpriteText.YRelative = PanelSprite.YRelative + VPadding;
            MessageSpriteText.Text = "";
            MessageSpriteText.StackOrder = 5;
            MessageSpriteText.Visible = false;
            
            btnOK = new MessagePanelOKButton(game);
            btnOK.XRelative = Convert.ToInt32(PanelSprite.XRelative + PanelSprite.Width / 2 - btnOK.Width/2);
            btnOK.YRelative = Convert.ToInt32(PanelSprite.YRelative + PanelSprite.Height - btnOK.Height / 2 - 40);
            btnOK.OnRelease += new EventHandler<ButtonEventArgs>(btnOK_OnRelease);
            btnOK.StackOrder = 7;
            btnOK.Visible = false;
            btnOK.Enabled = false;

            tmrOpenPanel = new Timer(game);
            tmrOpenPanel.IntervalType = TimerIntervalType.Miliseconds;
            tmrOpenPanel.Interval = 50;
            tmrOpenPanel.OnTick += new EventHandler(tmrOpenPanel_OnTick);
            tmrOpenPanel.Start();
            
            AddChild(BgSprite);
            AddChild(PanelSprite);
            AddChild(MessageSpriteText);
            AddChild(btnOK);

            ((DisplayManager)Game.Services.GetService(typeof(DisplayManager))).CameraFreeze();
        }

        void tmrOpenPanel_OnTick(object sender, EventArgs e)
        {
            if (PanelSprite.FrameNumber < 3) //0 or 1 or 2
                PanelSprite.FrameNumber += 1;
            if (PanelSprite.FrameNumber == 3)//last frame
            {
                PanelSprite.AnimationSpeed = 0;
                btnOK.Visible = true;
                btnOK.Enabled = true;
                MessageSpriteText.Visible = true;
                tmrOpenPanel.Stop();
            }
        }

        void btnOK_OnRelease(object sender, ButtonEventArgs e)
        {
            //((DisplayManager)Game.Services.GetService(typeof(DisplayManager))).CameraUnfreeze();
            OnOK(this, new EventArgs());
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
