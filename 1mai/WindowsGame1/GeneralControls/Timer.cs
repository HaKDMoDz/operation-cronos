using System;
using Microsoft.Xna.Framework;

namespace Operation_Cronos
{
    public enum TimerIntervalType
    {
        Seconds,
        Miliseconds,
    }
    public class Timer : Microsoft.Xna.Framework.GameComponent
    {
        #region Fields
        /// <summary>
        /// Adds the elapsed game time to its value until it reaches the maximum interval value, then resets
        /// </summary>
        private float timer = 0f;
        /// <summary>
        /// The minimum, default value for the clock interval, when counting in seconds
        /// </summary>
        private float defaultSecondsInterval = 1000.0f; // 1000 milliseconds/1 second
        /// <summary>
        /// The minimum, default value for the clock interval, when counting in miliseconds
        /// </summary>
        private float defaultMilisecondsInterval = 1.0f; // 1 millisecond
        /// <summary>
        /// Raises the default interval at the desired number of miliseconds
        /// </summary>
        private int multiplyIntervalBy = 1;
        /// <summary>
        /// Shows if the timer is started or stopped
        /// </summary>
        private bool started = false;
        /// <summary>
        /// Sets the timer to work in seconds or miliseconds (default: Seconds)
        /// </summary>
        private TimerIntervalType intervalType = TimerIntervalType.Seconds;
        #endregion

        #region Events
        public event EventHandler OnTick = delegate { };
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the timer interval in seconds or miliseconds (according to the 
        /// IntervalType)
        /// </summary>
        public int Interval
        {
            get { return multiplyIntervalBy; }
            set { multiplyIntervalBy = value; }
        }

        /// <summary>
        /// Gets or sets the timer interval type
        /// </summary>
        public TimerIntervalType IntervalType
        {
            get { return intervalType;}
            set { intervalType = value;}
        }

        /// <summary>
        /// Shows if timer is started
        /// </summary>
        public bool Started
        {
            get { return started;}
        }
        #endregion

        #region Constructors
        public Timer(Game game)
            : base(game)
        {
            Game.Components.Add(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the Timer
        /// </summary>
        public void Start()
        {
            started = true;
        }

        /// <summary>
        /// Stops the Timer (also resets it)
        /// </summary>
        public void Stop()
        {
            started = false;
            timer = 0f;
        }
        #endregion

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (started)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (intervalType == TimerIntervalType.Seconds)
                {
                    if (timer >= (defaultSecondsInterval * multiplyIntervalBy))
                    {
                        OnTick(this, new EventArgs());
                        timer = 0f; //resets the timer
                    }
                }
                else //Miliseconds
                {
                    if (timer >= (defaultMilisecondsInterval * multiplyIntervalBy))
                    {
                        OnTick(this, new EventArgs());
                        timer = 0f; //resets the timer
                    }
                }
               base.Update(gameTime);
            }
        }
        #endregion
    }
}
