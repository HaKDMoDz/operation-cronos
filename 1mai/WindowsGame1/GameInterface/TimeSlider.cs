using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display {
    public class TimeSlider : InputVisualComponent, IDraggable {

        public event EventHandler OnYearIncreased = delegate { };
        public event EventHandler OnYearDecreased = delegate { };

        Sprite frame;
        SpriteText year;
        PanelButton left;
        PanelButton right;

        public int Year {
            set { year.Text = value.ToString(); }
        }

        public TimeSlider(Game game)
            : base(game) {

            frame = new Sprite(game, GraphicsCollection.GetPack("time-bar-bar2"));
            AddChild(frame);

            left = new PanelButton(game, PanelButtonType.TimeLeft);
            left.StackOrder = 1;
            left.YRelative = 4;
            left.XRelative = 4;
            AddChild(left);

            right = new PanelButton(game, PanelButtonType.TimeRight);
            right.XRelative = frame.Width - 12;
            right.YRelative = 4;
            AddChild(right);

            year = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            year.XRelative = 35;
            year.YRelative = 2;
            year.StackOrder = 2;
            AddChild(year);

            OnMousePress += new EventHandler<MouseEventArgs>(TimeSlider_OnMousePress);

            left.OnRelease += new EventHandler<ButtonEventArgs>(left_OnRelease);
            right.OnRelease += new EventHandler<ButtonEventArgs>(right_OnRelease);
        }

        void right_OnRelease(object sender, ButtonEventArgs e) {
            if (e.Button == MouseButton.LeftButton) {
                OnYearIncreased(this, new EventArgs());
            }
        }

        void left_OnRelease(object sender, ButtonEventArgs e) {
            if (e.Button == MouseButton.LeftButton) {
                OnYearDecreased(this, new EventArgs());
            }
        }

        void TimeSlider_OnMousePress(object sender, MouseEventArgs e) {
            Press(e.ScreenPosition);
        }

        #region IDraggable Members
        Point mousePosInside;

        public Point MousePositionInside {
            get { return mousePosInside; }
        }

        public event EventHandler OnPress = delegate { };

        public event EventHandler OnRelease = delegate { };

        public void Press(Point mouseWorldPos) {
            mousePosInside.X = mouseWorldPos.X - this.X;
            mousePosInside.Y = mouseWorldPos.Y - this.Y;
            OnPress(this, new EventArgs());
        }

        public void Release(Point mouseWorldPos) {
        }

        #endregion
    }
}