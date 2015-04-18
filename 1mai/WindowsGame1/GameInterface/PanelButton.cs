using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {

    public enum PanelButtonType{
        UpArrow,
        DownArrow,
        Minimize,
        TimeUpArrow,
        TimeDownArrow,
        TimeMinimize,
        TimeLeft,
        TimeRight,
        TimeTravelButton,
        ResourcesMinimize,
        ResourcesExit,
        ResourcesAlert,
        ResourcesSave,
        ResourcesControl,
        ResourcesMission,
        YearReset
    }

    public class PanelButton : Button{

        Sprite visual;
        PanelButtonType buttonType;

        #region Properties
        public PanelButtonType ButtonType
        {
            get { return buttonType; }
        }

        public override int Width
        {
            get
            {
                return visual.Width;
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
                return visual.Height;
            }
            set
            {
                base.Height = value;
            }
        }
        #endregion

        public PanelButton(Game game, PanelButtonType type):base(game) {
            buttonType = type;

            switch (type) {
                case PanelButtonType.UpArrow:
                    visual = new Sprite(game, GraphicsCollection.GetPack("building-up"));
                    break;
                case PanelButtonType.DownArrow:
                    visual = new Sprite(game, GraphicsCollection.GetPack("building-down"));
                    break;
                case PanelButtonType.Minimize:
                    visual = new Sprite(game, GraphicsCollection.GetPack("building-minimize"));
                    break;
                case PanelButtonType.TimeDownArrow:
                    visual = new Sprite(game, GraphicsCollection.GetPack("time-panel-arrowdown"));
                    break;
                case PanelButtonType.TimeUpArrow:
                    visual = new Sprite(game, GraphicsCollection.GetPack("time-panel-arrowup"));
                    break;
                case PanelButtonType.TimeMinimize:
                    visual = new Sprite(game, GraphicsCollection.GetPack("time-panel-minimize"));
                    break;
                case PanelButtonType.TimeLeft:
                    visual = new Sprite(game, GraphicsCollection.GetPack("time-bar-leftarrow"));
                    break;
                case PanelButtonType.TimeRight:
                    visual = new Sprite(game, GraphicsCollection.GetPack("time-bar-rightarrow"));
                    break;
                case PanelButtonType.TimeTravelButton:
                    visual = new Sprite(game, GraphicsCollection.GetPack("time-panel-button-travel"));
                    break;
                case PanelButtonType.ResourcesMinimize:
                    visual = new Sprite(game, GraphicsCollection.GetPack("left-menu-minimize"));
                    break;
                case PanelButtonType.ResourcesAlert:
                    visual = new Sprite(game, GraphicsCollection.GetPack("left-menu-button-alert"));
                    break;
                case PanelButtonType.ResourcesControl:
                    visual = new Sprite(game, GraphicsCollection.GetPack("left-menu-button-control"));
                    break;
                case PanelButtonType.ResourcesExit:
                    visual = new Sprite(game, GraphicsCollection.GetPack("left-menu-button-exit"));
                    break;
                case PanelButtonType.ResourcesMission:
                    visual = new Sprite(game, GraphicsCollection.GetPack("left-menu-button-mission"));
                    break;
                case PanelButtonType.ResourcesSave:
                    visual = new Sprite(game, GraphicsCollection.GetPack("left-menu-button-save"));
                    break;
                case PanelButtonType.YearReset:
                    visual = new Sprite(game, GraphicsCollection.GetPack("year-panel-reset"));
                    break;
            }            

            AddChild(visual);
        }

        public override void MouseOverAnimation()
        {
            visual.FrameNumber = 1;
        }

        public override void MouseLeaveAnimation()
        {
            visual.FrameNumber = 0;
        }
    }
}
