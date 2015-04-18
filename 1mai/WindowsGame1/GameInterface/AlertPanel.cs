using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display
{
    public class AlertPanel : InputVisualComponent
    {
        Sprite frame;
        ControlPanelButton close;
        public SpriteText text;
        Timer tmr;
        public event EventHandler On_Blink = delegate { };

        #region Properties
        public override int Width
        {
            get
            {
                return frame.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        public override int Height
        {
            get
            {
                return frame.Height;
            }
            set
            {
                base.Height = value;
            }
        }
        #endregion

        public AlertPanel(Game game)
            : base(game)
        {
            frame = new Sprite(game, GraphicsCollection.GetPack("alert-frame"));
            AddChild(frame);

            tmr = new Timer(this.Game);

            close = new ControlPanelButton(game, ControlPanelButtonType.Close);
            close.StackOrder = 3;
            close.XRelative = 428;
            close.YRelative = 7;
            close.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(close_OnMousePress);
            AddChild(close);

            text = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            text.StackOrder = 3;
            text.MaxLength = 350;
            text.XRelative = 30;
            text.YRelative = 50;
            AddChild(text);
        }

        public string Text
        {
            get { return text.Text; }
            set { text.Text = value; }
        }

        public void SplitTextToRows(int width)
        {
            text.SplitTextToRows(width);
        }

        void close_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            this.IsVisible = false;
            this.Enabled = false;
        }

        public void SetText(string newText)
        {
            if (text.Text != newText)
            {
                text.Text = newText;
                tmr.IntervalType = TimerIntervalType.Seconds;
                tmr.Interval = 1;
                tmr.OnTick += new EventHandler(tmr_OnTick);
                tmr.Start();
            }
        }

        void tmr_OnTick(object sender, EventArgs e)
        {
            On_Blink(this, null);
        }

        public void StopBlinking()
        {
            tmr.Stop();
        }

        public void Close()
        {
            close_OnMousePress(null, null);
        }

        public void UpdateYear()
        {
            Close();
        }
    }
}
