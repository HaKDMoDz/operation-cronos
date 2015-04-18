using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    public class TimeBar : InputVisualComponent, ISliding{

        public event EventHandler OnPanelOpened = delegate { };
        public event EventHandler OnPanelClosed = delegate { };

        Sprite frame;
        TimeSlider slider;
        int openPosition;
        int closedPosition;
        int direction;
        int speed;
        int startYear;
        int endYear;
        int year;
        float fraction;
        IDraggable draggable;
        const int LeftSlideLimit = 21;
        const int RightSlideLimit = 511;
        const int Length = 490;

        #region Properties
        public int OpenPosition {
            get { return openPosition; }
        }

        public int ClosedPosition {
            get { return closedPosition; }
        }

        public Boolean IsOpen {
            get { return XRelative < ClosedPosition; }
        }

        public override int Height {
            get {
                return frame.Height;
            }
        }

        public override int Width {
            get {
                return frame.Width;
            }
        }

        public IDraggable DraggableSlider {
            get { return draggable; }
            set { draggable = value; }
        }

        public int Year {
            get { return year; }
            set {
                if (value <= endYear && value >= startYear) {
                    year = value;
                    slider.Year = value;

                    float f = (float)(year - startYear) / (float)(endYear - startYear);
                    slider.XRelative = LeftSlideLimit + (int)((float)Length * f);
                }
            }
        }

        public int EndYear {
            set { endYear = value; }
        }

        public int StartYear {
            set { startYear = value; }
        }
        #endregion


        public TimeBar(Game game, int sYear, int eYear)
            : base(game) {
            frame = new Sprite(game, GraphicsCollection.GetPack("time-bar-bar1"));
            AddChild(frame);

            slider = new TimeSlider(game);
            slider.YRelative = 25;
            slider.StackOrder = 1;
            slider.OnPress += new EventHandler(slider_OnPress);
            slider.OnYearIncreased += new EventHandler(slider_OnYearIncreased);
            slider.OnYearDecreased += new EventHandler(slider_OnYearDecreased);
            
            AddChild(slider);

            speed = 20;
            direction = 0;

            OnMouseMove += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(TimeBar_OnMouseMove);

            fraction = 1;
            startYear = sYear;
            endYear = eYear;
            Year = 2010;
        }

        void slider_OnYearDecreased(object sender, EventArgs e) {
            if (Year > startYear) {
                Year--;
            }
        }

        void slider_OnYearIncreased(object sender, EventArgs e) {
            if (Year < endYear) {
                Year++;
            }
        }

        void TimeBar_OnMouseMove(object sender, Operation_Cronos.Input.MouseEventArgs e) {
            int relativeMouseX = e.ScreenPosition.X - this.X;
            if (draggable != null) {
                if (relativeMouseX - draggable.MousePositionInside.X > TimeBar.LeftSlideLimit
                    && relativeMouseX - draggable.MousePositionInside.X < TimeBar.RightSlideLimit) {
                    ((InputVisualComponent)draggable).XRelative = relativeMouseX - draggable.MousePositionInside.X;
                } else {
                    if (relativeMouseX - draggable.MousePositionInside.X < TimeBar.LeftSlideLimit) {
                        ((InputVisualComponent)draggable).XRelative = TimeBar.LeftSlideLimit;
                    } else {
                        if (relativeMouseX - draggable.MousePositionInside.X > TimeBar.RightSlideLimit) {
                            ((InputVisualComponent)draggable).XRelative = TimeBar.RightSlideLimit;
                        }
                    }
                }
                fraction = ((float)slider.XRelative - LeftSlideLimit) / (float)Length;
                Year = startYear + (int)((float)(endYear - startYear) * fraction);
            }
        }

        void slider_OnPress(object sender, EventArgs e) {
            DraggableSlider = slider;
        }

        public void UpdateLimitPositions(int screenWidth) {
            openPosition = screenWidth - 750;
            closedPosition = screenWidth - 155;
        }

        public void UpdatePosition(Rectangle screen) {
            UpdateLimitPositions(screen.Width);
            YRelative = screen.Height - 93;
            if (IsOpen) {
                XRelative = openPosition;
            } else {
                XRelative = closedPosition;
            }
        }

        public void SlideIn() {
            direction = 1;
        }

        public void SlideOut() {
            direction = -1;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            try
            {
                XRelative += direction * speed;
            }
            catch (Exception e) { }
            if (XRelative + direction * speed < OpenPosition) {
                if (direction == -1) {
                    XRelative = OpenPosition;
                    direction = 0;
                    OnPanelOpened(this, new EventArgs());
                }

            }
            if (XRelative + direction * speed > ClosedPosition) {
                if (direction == 1) {
                    XRelative = ClosedPosition;
                    direction = 0;
                    OnPanelClosed(this, new EventArgs());
                }
            }
        }
    }
}
