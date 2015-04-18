using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.Sound;

namespace Operation_Cronos.Display
{
    public class TimeTravelPanel : InputVisualComponent
    {

        public event EventHandler OnMinimize = delegate { };
        public event EventHandler OnYearChanged = delegate { };
        public event EventHandler OnYearIncreased = delegate { };
        public event EventHandler OnYearDecreased = delegate { };

        Sprite frame;
        SpriteText spriteTextCurrentYear;
        PanelButton minimize;
        PanelButton upArrow;
        PanelButton downArrow;
        PanelButton travel;

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        private SoundManager SoundManager
        {
            get
            {
                return (SoundManager)Game.Services.GetService(typeof(SoundManager));
            }
        }
        

        /// <summary>
        /// Gets or Sets the current year in the spriteText
        /// </summary>
        public int Year
        {
            get
            {
                return Convert.ToInt32(spriteTextCurrentYear.Text);
            }
            set
            {
                spriteTextCurrentYear.Text = value.ToString();
            }
        }

        public TimeTravelPanel(Game game)
            : base(game)
        {

            #region Elements
            frame = new Sprite(game, GraphicsCollection.GetPack("time-panel-frame"));

            AddChild(frame);

            spriteTextCurrentYear = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            spriteTextCurrentYear.Text = GameManager.CurrentYear.ToString();
            spriteTextCurrentYear.Tint = Color.WhiteSmoke;
            spriteTextCurrentYear.XRelative = 55;
            spriteTextCurrentYear.YRelative = 42;

            AddChild(spriteTextCurrentYear);

            minimize = new PanelButton(game, PanelButtonType.TimeMinimize);
            minimize.XRelative = -10;
            minimize.YRelative = 27;
            minimize.StackOrder = 1;
            minimize.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(minimize_OnMousePress);
            AddChild(minimize);

            upArrow = new PanelButton(game, PanelButtonType.TimeUpArrow);
            upArrow.StackOrder = 1;
            upArrow.XRelative = 95;
            upArrow.YRelative = 32;
            upArrow.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(upArrow_OnMousePress);
            AddChild(upArrow);

            downArrow = new PanelButton(game, PanelButtonType.TimeDownArrow);
            downArrow.XRelative = 97;
            downArrow.YRelative = 55;
            downArrow.StackOrder = 1;
            downArrow.OnPress += new EventHandler<ButtonEventArgs>(downArrow_OnPress);
            AddChild(downArrow);

            travel = new PanelButton(game, PanelButtonType.TimeTravelButton);
            travel.XRelative = 33;
            travel.YRelative = 80;
            travel.StackOrder = 1;
            travel.OnPress += new EventHandler<ButtonEventArgs>(travel_OnPress);
            AddChild(travel);

            #endregion

        }


        void downArrow_OnPress(object sender, ButtonEventArgs e)
        {
            if (e.Button == Operation_Cronos.Input.MouseButton.LeftButton)
            {
                OnYearDecreased(this, e);
            }
        }

        void upArrow_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            if (e.Button == Operation_Cronos.Input.MouseButton.LeftButton)
            {
                OnYearIncreased(this, e);
            }
        }

        void travel_OnPress(object sender, ButtonEventArgs e)
        {
            if (e.Button == Operation_Cronos.Input.MouseButton.LeftButton)
            {
                OnYearChanged(sender, e);
            }
        }

        void minimize_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            OnMinimize(this, new EventArgs());
            SoundManager.PlaySound(Sounds.SlidingSoundLong);
        }


        public override int Width
        {
            get
            {
                return frame.Width;
            }
        }

        public override int Height
        {
            get
            {
                return frame.Height;
            }
        }
    }
}
