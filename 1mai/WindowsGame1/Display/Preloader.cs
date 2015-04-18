using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using Operation_Cronos.IO;

namespace Operation_Cronos.Display {

    public class Preloader : VisualComponent 
    {
        private double percent;
        private Sprite preloaderSprite;
        private SpriteFont font;
        private SpriteText status;
        private Sprite BgSprite; //a black stretched pixel

        private SpriteText val1;
        private SpriteText val2;
        private SpriteText val3;
        private SpriteText val4;

        int val1InitX = 627;
        int val1InitY = 517;
        int valX = 5;
        int valY = 4;

        int milisecondsPassedWhenInTimerMode = 0;
        Timer tmr = null;

        #region Properties
        public double Percent 
        {
            set 
            {
                percent = value;

                string percentStr = (Convert.ToInt32(percent)).ToString();
                val1.Text = " ";
                val2.Text = " ";
                val3.Text = " ";

                if (percentStr.Length == 3)//(percent==100)
                {
                    val1.Text = "1";
                    val2.Text = "0";
                    val3.Text = "0";                    
                }
                else//percent<100
                {
                    if (percentStr.Length == 1)//0 - 9
                        val3.Text = percentStr[0].ToString();
                    else //0 - 99
                    {
                        val2.Text = percentStr[0].ToString();
                        val3.Text = percentStr[1].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the Width of the preloaderSprite
        /// </summary>
        public override int Width
        {
            get
            {
                return BgSprite.Width;
            }
            set
            {
                BgSprite.Width = value;
                base.Width = value;
                RefreshDimensions();
            }
        }

        /// <summary>
        /// Gets the Height of the preloadeSprite
        /// </summary>
        public override int Height
        {
            get
            {
                return BgSprite.Height;
            }
            set
            {
                BgSprite.Height = value;
                base.Height = value;
                RefreshDimensions();
            }
        }

        /// <summary>
        /// Sets the Status, a text indicating what is currently loading
        /// </summary>
        public IOOperation Status
        {
            set
            {
                string oldStatus = status.Text;

                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes( typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                    status.Text = attributes[0].Description;
                else
                    status.Text = "";

                SetStatusPosition();

                UpdateComponentsX();
                UpdateComponentsY();

                if (status.Text != oldStatus)
                {
                    tmr.Stop();
                    DisplayManager.CameraUnfreeze();
                }
            }
        }

        private DisplayManager DisplayManager
        {
            get { return (DisplayManager)Game.Services.GetService(typeof(DisplayManager)); }
        }
        #endregion

        #region Properties
        public  event EventHandler On_PreloaderTimerStopped = delegate{};
        #endregion

        public Preloader(Game game, Rectangle screen)
            : base(game) 
        {
            BgSprite = new Sprite(game, Game.Content.Load<Texture2D>("main_menu\\Pixel\\10000"));
            BgSprite.Width = screen.Width;
            BgSprite.Height = screen.Height;
            BgSprite.Tint = Color.Black;

            preloaderSprite = new Sprite(game, Game.Content.Load<Texture2D>("preloader\\10000"));
            preloaderSprite.XRelative = BgSprite.Width / 2 - preloaderSprite.Width / 2;
            preloaderSprite.YRelative = BgSprite.Height / 2 - preloaderSprite.Height / 2;           
            
            font = Game.Content.Load<SpriteFont>("trebuchet");
            
            //---
            val1 = new SpriteText(game, font);
            val1.Text = " ";
            val1.Tint = Color.WhiteSmoke;

            val2 = new SpriteText(game, font);
            val2.Text = " ";
            val2.Tint = Color.WhiteSmoke;

            val3 = new SpriteText(game, font);
            val3.Text = " ";
            val3.Tint = Color.WhiteSmoke;

            val4 = new SpriteText(game, font);
            val4.Text = "%";
            val4.Tint = Color.WhiteSmoke;
            //---

            font = Game.Content.Load<SpriteFont>("calibri13"); //or santassleigh20
            status = new SpriteText(game, font);
            status.XRelative = 0;
            status.YRelative = 100;
            status.Tint = Color.WhiteSmoke;

            tmr = new Timer(game);

            Components.Add(BgSprite);
            Components.Add(preloaderSprite);
            Components.Add(val1);
            Components.Add(val2);
            Components.Add(val3);
            Components.Add(val4);
            Components.Add(status);
        }

        public void SetPercentage()
        {
            val1.XRelative = val1InitX + preloaderSprite.XRelative;
            val2.XRelative = val1.XRelative + val1.Width + valX;
            val3.XRelative = val2.XRelative + val2.Width + valX;
            val4.XRelative = val3.XRelative + val3.Width + valX;
            UpdateComponentsX();

            val1.YRelative = val1InitY + preloaderSprite.YRelative;
            val2.YRelative = val1.YRelative - valY;
            val3.YRelative = val2.YRelative - valY;
            val4.YRelative = val3.YRelative - valY;
            UpdateComponentsY();
        }

        /// <summary>
        /// Sets the Preloader to be used in Saving Game Mode
        /// </summary>
        public void TimerMode()
        {
            SetStatusPosition();
            DisplayManager.CameraFreeze();
            UpdateComponentsX();
            UpdateComponentsY();

            milisecondsPassedWhenInTimerMode = 0;

            tmr.Stop();
            tmr.IntervalType = TimerIntervalType.Miliseconds;
            tmr.Interval = 20; //miliseconds
            tmr.OnTick += new EventHandler(tmr_OnTick);
            tmr.Start();

            DisplayManager.CameraFreeze();
        }

        void tmr_OnTick(object sender, EventArgs e)
        {
            milisecondsPassedWhenInTimerMode+= ((Timer)sender).Interval;
            if (milisecondsPassedWhenInTimerMode <= ((Timer)sender).Interval*100)//x seconds
            {
                Percent = (milisecondsPassedWhenInTimerMode / ((Timer)sender).Interval);
            }
            else
            {
                status.Text = "";
                ((Timer)sender).Stop();
                this.Visible = false;
                this.Enabled = false;
                BgSprite.Width = 0;
                BgSprite.Height = 0;

               // DisplayManager.CameraUnfreeze();
                On_PreloaderTimerStopped(this, new EventArgs());
            }
        }

        public void RefreshDimensions()
        {
            preloaderSprite.XRelative = BgSprite.Width / 2 - preloaderSprite.Width / 2;
            preloaderSprite.YRelative = BgSprite.Height / 2 - preloaderSprite.Height / 2;

            SetStatusPosition();

            Percent = 0;
            SetPercentage();
        }

        private void SetStatusPosition()
        {
            status.XRelative = preloaderSprite.XRelative + preloaderSprite.Width / 2 - status.Width / 2;
            status.YRelative = preloaderSprite.YRelative + preloaderSprite.Height / 2 + 280;
        }
    }
}
