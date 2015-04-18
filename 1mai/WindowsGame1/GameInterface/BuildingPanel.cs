using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    public partial class BuildingPanel : InputVisualComponent, ISliding {

        public event EventHandler OnPanelOpened = delegate { };
        public event EventHandler OnPanelClosed = delegate { };
        public event EventHandler OnSlotSelected = delegate { };
        public event EventHandler OnSlotUnSelected = delegate { };
        
        Sprite frame;
        PanelButton upArrow;
        PanelButton downArrow;
        int openPositionY;
        int closedPositionY;
        int direction;
        int speed;
        ConstructionType buildingCategory;
        BuildingIcon selectedIcon;

        Tooltip tooltip;

        #region Properties
        public ConstructionType BuildingCategory {
            get { return buildingCategory; }
        }

        public int OpenPositionY {
            get { return openPositionY; }
            set { openPositionY = value; }
        }

        public int ClosedPositionY {
            get { return closedPositionY; }
            set { closedPositionY = value; }
        }

        public Boolean IsOpen {
            get { return YRelative > ClosedPositionY; }
        }

        private GameMap Map {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        public Construction SelectedBuilding {
            get { return selectedIcon.BuildingName; }
        }
        #endregion

        public BuildingPanel(Game game)
            : base(game) {

            frame = new Sprite(game, GraphicsCollection.GetPack("building-frame"));
            frame.StackOrder = 0;

            upArrow = new PanelButton(game, PanelButtonType.UpArrow);
            upArrow.XRelative = 55;
            upArrow.YRelative = 254;
            upArrow.StackOrder = 1;

            downArrow = new PanelButton(game, PanelButtonType.DownArrow);
            downArrow.XRelative = 92;
            downArrow.YRelative = 255;
            downArrow.StackOrder = 1;

            tooltip = new Tooltip(game, 3);
            tooltip.XRelative = 23;
            tooltip.StackOrder = 2;
            tooltip.Visible = false;

            AddChild(frame);
            AddChild(upArrow);
            AddChild(downArrow);
            AddChild(tooltip);
          
            direction = 0;
            speed = 15;

            CreateIcons();
        }

        

        public override int Width {
            get {
                return frame.Width;
            }
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            try
            {
                YRelative += direction * speed;
            }
            catch (Exception e)
            {
            }
            if (YRelative + direction * speed > OpenPositionY) {
                if (direction == 1) {
                    YRelative = OpenPositionY;
                    direction = 0;
                    OnPanelOpened(this, new EventArgs());
                }
            }
            if (YRelative + direction * speed < ClosedPositionY) {
                if (direction == -1) {
                    YRelative = ClosedPositionY;
                    direction = 0;
                    OnPanelClosed(this, new EventArgs());
                }
            }
        }

        public void SlideIn() {
            direction = -1;
        }

        public void SlideOut() {
            direction = 1;
        }

        public void RefreshBuildings(ConstructionType category)
        {
            buildingCategory = category;
            HideIcons();
            if (Map != null)
            {
                Map.DisplaySlots(ConstructionType.None);
            }

            switch (category)
            {
                case ConstructionType.Economy:
                    selectedIcons = economyIcons;
                    break;
                case ConstructionType.Education:
                    selectedIcons = educationIcons;
                    break;
                case ConstructionType.Environment:
                    selectedIcons = environmentIcons;
                    break;
                case ConstructionType.Food:
                    selectedIcons = foodIcons;
                    break;
                case ConstructionType.Health:
                    selectedIcons = healthIcons;
                    break;
                case ConstructionType.Energy:
                    selectedIcons = energyIcons;
                    break;
                case ConstructionType.Population:
                    selectedIcons = populationIcons;
                    break;
            }

            if (selectedIcons != null)
                DisplayIcons();
        }

        void icon_OnRelease(object sender, ButtonEventArgs e) 
        {
            BuildingIcon icon = (BuildingIcon)sender;
            if (!icon.IsSelected) 
            {
                if (selectedIcon != null)
                    selectedIcon.Unselect();
                icon.Select();
                selectedIcon = icon;
                RefreshBuildings(icon.BuildingType);
                Map.DisplaySlots(((BuildingIcon)sender).BuildingType);
            } else {
                ClearSelectedIcon();
            }
        }
        public void ClearSelectedIcon() {
            if (selectedIcon != null) {
                selectedIcon.Unselect();
                selectedIcon = null;
                Map.HideSlots();
            }
        }
    }
}
