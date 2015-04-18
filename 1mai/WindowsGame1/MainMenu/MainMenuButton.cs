using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display
{
    class MainMenuButton : Microsoft.Xna.Framework.GameComponent
    {
        #region Fields

        /// <summary>
        /// The (main)texture of the button
        /// </summary>
        Sprite ButtonSprite;

        /// <summary>
        /// Used for the mouse Over/Leave/Press/Release effects
        /// </summary>
        Sprite ButtonLeftState;

        /// <summary>
        /// Used for the mouse Over/Leave/Press/Release effects or LoadScreen LoadSettings/Delete/Back
        /// effects, if button type is LoadScreenButton
        /// </summary>
        Sprite ButtonRightState;

        /// <summary>
        /// Shows if the button is active or not
        /// </summary>
        bool isActive = true;

        /// <summary>
        /// The mouse needs to stay on/off the button for at least one second for the MouseOn/MouseOff to be considered
        /// </summary>
        Timer timer;

        /// <summary>
        /// Indicates for how long the Mouse rested on the button surface
        /// </summary>
        int TimeOver;
        /// <summary>
        /// Indicates for how long the Mouse has been outside the button surface
        /// </summary>
        int TimeOff;

        /// <summary>
        /// Indicates the button type 
        /// (weather the button is a MainMenu Button or a MainMenu-LoadScreen Button)
        /// </summary>
        bool LoadScreenButton = false;

        MouseManager input;
        #endregion

        #region Events
        public event EventHandler OnMouseOver = delegate { };
        public event EventHandler OnMouseLeave = delegate { };
        public event EventHandler OnMouseClick = delegate { };
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for MainMenu Buttons
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sprite">The 1frame - Primary Sprite</param>
        /// <param name="btnLeftState">The 2frames - Left Effect Sprite</param>
        /// <param name="btnRightState">The 2frames - Right Effect Sprite</param>
        /// <param name="drawOrder"></param>
        /// <param name="positions">A size 6 int vector for ButtonSprite(X,Y), ButtonLeftState(X,Y), ButtonRightState(X,Y)</param>
        public MainMenuButton(Game game, Sprite sprite, Sprite btnLeftState, Sprite btnRightState, int drawOrder, int[] positions)
            : base(game)
        {
            ButtonSprite = sprite;
            ButtonSprite.DrawOrder = drawOrder;
            ButtonSprite.X = positions[0];
            ButtonSprite.Y = positions[1];

            ButtonLeftState = btnLeftState;
            ButtonLeftState.DrawOrder = drawOrder;
            ButtonLeftState.X = positions[2];
            ButtonLeftState.Y = positions[3];

            ButtonRightState = btnRightState;
            ButtonRightState.DrawOrder = drawOrder;
            ButtonRightState.X = positions[4];
            ButtonRightState.Y = positions[5];

            
            input = new MouseManager(game);
            input.OnPress += new EventHandler<MouseEventArgs>(input_OnPress);
            input.OnMouseMove += new EventHandler<MouseEventArgs>(input_OnMouseMove);
             
            TimeOver = 0;
            TimeOff = 0;
            timer = new Timer(game);
            timer.IntervalType = TimerIntervalType.Miliseconds;
            timer.Interval = 500;
            timer.Start();
            timer.OnTick += new EventHandler(Do_UpdateTimeOverOff);

            Game.Components.Add(this);
        }

        /// <summary>
        /// Constructor for LoadScreen Buttons
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sprite">The 1frame - Primary Sprite</param>
        /// <param name="btnLeftState">The 2frames - Effect Sprite</param>
        /// <param name="drawOrder"></param>
        /// <param name="positions">A size 4 int vector for ButtonSprite(X,Y), ButtonRightState(X,Y)</param>
        public MainMenuButton(Game game, Sprite sprite, Sprite btnRightState, int drawOrder, int[] positions)
            : base(game)
        {
            ButtonSprite = sprite;
            ButtonSprite.DrawOrder = drawOrder;
            ButtonSprite.X = positions[0];
            ButtonSprite.Y = positions[1];

            ButtonRightState = btnRightState;
            ButtonRightState.DrawOrder = drawOrder;
            ButtonRightState.X = positions[2];
            ButtonRightState.Y = positions[3];


            input = new MouseManager(game);
            input.OnPress += new EventHandler<MouseEventArgs>(input_OnPress);
            input.OnMouseMove += new EventHandler<MouseEventArgs>(input_OnMouseMove);

            LoadScreenButton = true;
            Game.Components.Add(this);
        }
        #endregion

        #region Event Handlers

            #region Timer Event Handler
            void Do_UpdateTimeOverOff(object sender, EventArgs e)
            {
                if (isActive) //if button is visible (active)
                {
                    if (MouseIsOver())
                    {
                        TimeOver += 1;
                        if (TimeOver == 1) //500 miliseconds (1/2 second)
                        {
                            OnMouseOver(this, new EventArgs());
                        }
                        TimeOff = 0;
                    }
                    else
                    {
                        if (TimeOff != TimeOver)//excludes the first state of the mouse, when the app is launched
                        {                   //and consideres the 'mouse leave' after at least one 'mouse over'
                            TimeOff += 1;//timer.Interval;
                            if (TimeOff == 1) //500 miliseconds (1/2 second)
                            {
                                OnMouseLeave(this, new EventArgs());
                            }
                        }
                        TimeOver = 0;
                    }
                }
            }
            #endregion
        
            #region InputManager Event Handlers
            void input_OnPress(object sender, MouseEventArgs e)
            {
                if (MouseIsOver() && isActive)
                    //isActive shows that the button is visible (active)
                    if (e.Button == MouseButton.LeftButton)
                        OnMouseClick(this, new EventArgs());
            }

            void input_OnMouseMove(object sender, MouseEventArgs e)
            {
                if (MouseIsOver() && isActive)
                {
                    //isActive shows that the button is visible (active)
                    ButtonRightState.FrameNumber = 1;
                    if (LoadScreenButton)//if MainMenu-LoadScreen Button,event launched right away 
                        OnMouseOver(this, new EventArgs());
                    else//if MainMenu Button, event lauched depending on timer
                        ButtonLeftState.FrameNumber = 1;
                }
                if (MouseIsOver() == false && isActive)
                {
                    ButtonRightState.FrameNumber = 0;
                    if (LoadScreenButton)//if MainMenu-LoadScreen Button,event launched right away 
                        OnMouseLeave(this, new EventArgs());
                    else//if MainMenu Button, event lauched depending on timer
                        ButtonLeftState.FrameNumber = 0;
                }
            }
            #endregion
         

        #endregion

        #region Methods
        /// <summary>
        /// Stops drawing and also deactivates the button
        /// </summary>
        public void Hide()
        {
            try
            {
                ButtonSprite.Visible = false;
                if (!LoadScreenButton)
                {
                    ButtonLeftState.Visible = false;
                    ButtonLeftState.FrameNumber = 0;
                }
                ButtonRightState.Visible = false;
                ButtonRightState.FrameNumber = 0;
                if (!LoadScreenButton)
                    timer.Stop();
                isActive = false;
            }
            catch { }
        }

        /// <summary>
        /// Starts drawing and also reactivates the button
        /// </summary>
        public void Show()
        {
            try
            {
                ButtonSprite.Visible = true;
                if (!LoadScreenButton)
                    ButtonLeftState.Visible = true;
                ButtonRightState.Visible = true;
                if (!LoadScreenButton)
                    timer.Start();
                ResetMouseOverOff();
                isActive = true;
            }
            catch { }
        }
        
        bool MouseIsOver()
        {
            if (Mouse.GetState().X >= ButtonSprite.X && Mouse.GetState().X <= (ButtonSprite.X + ButtonSprite.Width) &&
                Mouse.GetState().Y >= ButtonSprite.Y && Mouse.GetState().Y <= (ButtonSprite.Y + ButtonSprite.Height))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Resets the TimeOver counter
        /// </summary>
        public void ResetMouseOverOff()
        {
            if (!LoadScreenButton)
            {
                TimeOver = 0;
                TimeOff = 0;
            }
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
