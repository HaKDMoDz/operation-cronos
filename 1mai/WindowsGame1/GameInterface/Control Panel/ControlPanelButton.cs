using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display
{
    public enum ControlPanelButtonType
    {
        Research,
        Graph,
        Close,
        Economy,
        Environment,
        Energy,
        Education,
        Food,
        Health,
        ResearchIcon,
        ResearchOK
    }
    public class ControlPanelButton:Button
    {
        Sprite visual;
        ControlPanelButtonType type;
        object data;
        String tooltipText = "";
        bool isSelected;        

        #region Properties
        public String TooltipText
        {
            get { return tooltipText; }
        }

        public ControlPanelButtonType ButtonType
        {
            get { return type; }
        }

        public override int  Width
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

        public override int  Height
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

        public bool IsSelected
        {
            set
            {
                isSelected = value;
                if (isSelected == false)
                    visual.FrameNumber = 0;
                else
                    visual.FrameNumber = 1;
            }
        }
        #endregion

        /// <summary>
        /// Container for any data
        /// </summary>
        public object Data
        {
            get { return data; }
            set { data = value; }
        }

        public ControlPanelButton(Game game, ControlPanelButtonType type)
            : base(game)
        {
            this.type = type;
            isSelected = false;
            switch (type)
            {
                case ControlPanelButtonType.Research:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-button-research"));
                    tooltipText = "Research";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Graph:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-button-graph"));
                    tooltipText = "Evolution graph";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Close:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-button-close"));                    
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Economy:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-mg-economy"));
                    tooltipText = "Economy";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Education:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-mg-education"));
                    tooltipText = "Education";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Energy:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-mg-energy"));
                    tooltipText = "Energy";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Environment:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-mg-environment"));
                    tooltipText = "Environment";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Food:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-mg-food"));
                    tooltipText = "Food";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.Health:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-mg-health"));
                    tooltipText = "Health";
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.ResearchIcon:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-research-icon"));
                    AddChild(visual);
                    break;
                case ControlPanelButtonType.ResearchOK:
                    visual = new Sprite(game, GraphicsCollection.GetPack("control-research-ok"));
                    AddChild(visual);
                    break;
            }
        }

        public override void MouseOverAnimation()
        {
            visual.FrameNumber = 1;
        }
        public override void MouseLeaveAnimation()
        {
            if (!isSelected)
                visual.FrameNumber = 0;
        }
    }
}
