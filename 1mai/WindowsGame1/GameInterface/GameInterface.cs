using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display {
    public class GameInterface : InputVisualComponent {

        Minimap minimap;
        BuildingPanel panel;
        TimeTravelPanel timePanel;
        TimeBar timeBar;
        LeftMenu leftMenu;
        YearPanel yearPanel;
        int width = 0;
        int height = 0;
        int numberOfItemsCreated = 0;
        int numberOfItemsToCreate = 6;//the number of panels

        #region Properties
        public override int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        private GameMap Map {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        private GameManager GameManager {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }

        private DisplayManager DisplayManager
        {
            get { return (DisplayManager)Game.Services.GetService(typeof(DisplayManager)); }
        }

        public int PercentCreated
        {
            get { return Convert.ToInt32((numberOfItemsCreated * 100) / numberOfItemsToCreate); }
        }
        #endregion

        #region Events
        public event EventHandler OnSaveGame = delegate { };
        #endregion

        public GameInterface(Game game, Rectangle screen)
            : base(game) {
                width = screen.Width;
                height = screen.Height;

            #region Minimap and Building Panel
            minimap = new Minimap(game);
            minimap.XRelative = screen.Width - minimap.Width+5;
            minimap.YRelative = 0;
            minimap.StackOrder = 1;
            minimap.OnCategorySelected += new EventHandler<BuildingTypeEventArgs>(minimap_OnCategorySelected);
            minimap.OnCategoryUnselected += new EventHandler<BuildingTypeEventArgs>(minimap_OnCategoryUnselected);
            minimap.OnMinimize += new EventHandler(minimap_OnMinimize);

            AddChild(minimap);
            numberOfItemsCreated++;
            DisplayManager.ChangePreloaderPercent(PercentCreated);

            panel = new BuildingPanel(game);
            panel.XRelative = screen.Width - panel.Width - 42;
            panel.OpenPositionY = minimap.Height - 47;
            panel.ClosedPositionY = -100;
            panel.YRelative = panel.ClosedPositionY;
            minimap_OnMinimize(null, null);

            panel.StackOrder = 0;
            panel.OnPanelClosed += new EventHandler(panel_OnPanelClosed);
            panel.OnPanelOpened += new EventHandler(panel_OnPanelOpened);

            AddChild(panel);
            numberOfItemsCreated++;
            DisplayManager.ChangePreloaderPercent(PercentCreated);
            #endregion

            #region Time
            timePanel = new TimeTravelPanel(game);
            timePanel.XRelative = screen.Width - timePanel.Width+5;
            timePanel.YRelative = screen.Height - timePanel.Height;
            timePanel.StackOrder = 1;

            timePanel.OnMinimize += new EventHandler(timePanel_OnMinimize);
            timePanel.OnYearChanged += new EventHandler(timePanel_OnYearChanged);
            timePanel.OnYearIncreased += new EventHandler(timePanel_OnYearIncreased);
            timePanel.OnYearDecreased += new EventHandler(timePanel_OnYearDecreased);

            AddChild(timePanel);
            numberOfItemsCreated++;
            DisplayManager.ChangePreloaderPercent(PercentCreated);

            timeBar = new TimeBar(game, GameManager.StartingTravelYear, GameManager.EndTravelYear);
            timeBar.StackOrder = 0;
            timeBar.UpdateLimitPositions(screen.Width);
            timeBar.YRelative = screen.Height - timeBar.Height - 33;
            timeBar.XRelative = timeBar.ClosedPosition;
            AddChild(timeBar);
            timeBar.SlideOut();
            numberOfItemsCreated++;
            DisplayManager.ChangePreloaderPercent(PercentCreated);
            #endregion

            #region LeftMenu
            leftMenu = new LeftMenu(game);
            leftMenu.XRelative = -5;
            leftMenu.YRelative = -5;
            leftMenu.On_SaveGame += new EventHandler(Do_OnSaveGame);
            AddChild(leftMenu);

            leftMenu.UpdateLeftPanelsPosition(screen);
            numberOfItemsCreated++;
            DisplayManager.ChangePreloaderPercent(PercentCreated);
            #endregion

            #region Year Panel
            yearPanel = new YearPanel(game);
            yearPanel.XRelative = screen.Width / 2 - yearPanel.Width / 2;
            yearPanel.YRelative = -5;
            yearPanel.OnYearReset += new EventHandler(yearPanel_OnYearReset);
            AddChild(yearPanel);
            numberOfItemsCreated++;
            DisplayManager.ChangePreloaderPercent(PercentCreated);
            #endregion

            Game.Services.AddService(typeof(GameInterface), this);
        } 
        

        public void ReleaseSlider() {
            timeBar.DraggableSlider = null;
        }

        public void UpdateSize(Rectangle screen) {
            minimap.XRelative = screen.Width - minimap.Width+5;
            panel.XRelative = screen.Width - panel.Width - 50;
            timePanel.XRelative = screen.Width - timePanel.Width + 5;
            timePanel.YRelative = screen.Height - timePanel.Height;
            timeBar.UpdateLimitPositions(screen.Width);
            timeBar.UpdatePosition(screen);
            timeBar.YRelative = screen.Height - timeBar.Height - 33;
            yearPanel.XRelative = screen.Width / 2 - yearPanel.Width / 2;
        }

        public void UpdateYear(int year)
        {
            yearPanel.Year = year;
            timeBar.Year = year;
            timePanel.Year = year;

            panel.DisplayIcons();

        }

        public void UpdateMinimapSize(int width, int height)
        {
            minimap.UpdateSize(width, height);
        }

        public void UpdateMinimapCamera(Point position, Point size)
        {
            minimap.UpdateCamera(position, size);
        }

        public void DisplaySlots(List<Point> pos)
        {
            minimap.DisplaySlots(pos);
        }

        public void EraseMinimapSlots()
        {
            minimap.EraseSlots();
        }

        public void ClearSelectedBuildingIcon() {
            panel.ClearSelectedIcon();
        }

        #region Time event handlers
        void timePanel_OnMinimize(object sender, EventArgs e) {
            if (timeBar.IsOpen) {
                timeBar.SlideIn();
            } else {
                timeBar.SlideOut();
            }
        }
        void timePanel_OnYearChanged(object sender, EventArgs e) 
        {
            GameManager.UpdateYear(timeBar.Year);
        }

        void yearPanel_OnYearReset(object sender, EventArgs e) {
            timeBar.Year = yearPanel.Year;
        }
        void timePanel_OnYearDecreased(object sender, EventArgs e)
        {
            if (timePanel.Year > GameManager.StartingTravelYear)
            {
                timePanel.Year--;
                GameManager.UpdateYear(timePanel.Year);
            }
        }

        void timePanel_OnYearIncreased(object sender, EventArgs e)
        {
            if (timePanel.Year < GameManager.EndTravelYear)
            {
                timePanel.Year++;
                GameManager.UpdateYear(timePanel.Year);
            }
        }
        #endregion

        #region Minimap and Building Panel event handlers
        void minimap_OnCategorySelected(object sender, BuildingTypeEventArgs e) {
            if (panel.IsOpen) {
                panel.SlideIn();
            } else {
                panel.RefreshBuildings(e.Category);
                panel.SlideOut();
            }
        }

        void minimap_OnCategoryUnselected(object sender, BuildingTypeEventArgs e) {
            panel.SlideIn();
        }

        void minimap_OnMinimize(object sender, EventArgs e)
        {
            if (panel.IsOpen)
                panel.SlideIn();
            else
            {
                if (panel.BuildingCategory != ConstructionType.None)
                {
                    panel.SlideOut();
                }
                else
                {
                    minimap.SelectBuildingCategory(ConstructionType.Population);
                    panel.RefreshBuildings(ConstructionType.Population);
                }
            }
        }

        void panel_OnPanelClosed(object sender, EventArgs e) {
            if (minimap.BuildingCategory != panel.BuildingCategory )
            {
                panel.RefreshBuildings(minimap.BuildingCategory);
                
                if (minimap.BuildingCategory != ConstructionType.None) {
                    panel.SlideOut();
                }
            }
        }
        void panel_OnPanelOpened(object sender, EventArgs e) 
        {
        }
        #endregion

        public void ConfirmBuild(Slot slot)
        {
            int price = GameManager.BuildingProperties.BuildingBaseProperties(panel.SelectedBuilding, GameManager.CurrentYear, 0, GameManager.CurrentYear).Price;
            if (GameManager.GetMoney(GameManager.CurrentYear) >= price)
            {
                GameManager.SpendMoney(GameManager.CurrentYear, price);
                SetMoney(GameManager.GetMoney(GameManager.CurrentYear));
                Map.CreateBuilding(slot, panel.SelectedBuilding);
            }
            else
            {
                //Debug.AddToLog("Not enough cashcaval");
            }
        }

        public void ShowCurrentMission()
        {           
        }

        public void SetParameters(MilleniumGoalsSet data)
        {
            leftMenu.SetParameters(data);
        }

        public void SetPopulation(int population)
        {
            yearPanel.Population = population;
        }

        public void SetMoney(int money)
        {
            yearPanel.Money = money;
            panel.DisplayIcons();
        }

        public void ChangeTravelYears(int startYear, int endYear)
        {
            timeBar.StartYear = startYear;
            timeBar.EndYear = endYear;
        }

        private void Do_OnSaveGame(object sender, EventArgs e)
        {
            OnSaveGame(this, new EventArgs());
        }

        public void SetOverpopulationStatus(int value)
        {
            yearPanel.ShowIfOverpopulated(value);
        }

        public void SetAlertPanel(string text)
        {
            leftMenu.SetAlertPanelText(text);
        }
    }
}
