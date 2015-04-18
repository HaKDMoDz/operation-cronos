using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;
using System.Xml.Linq;
using System.Xml;
using Operation_Cronos.Input;
using Operation_Cronos.Profiles;
using Operation_Cronos.Sound;

namespace Operation_Cronos.Display
{
    public class GameMap : InputVisualComponent
    {
        #region Fields
        private SelectableComponent selectedObject;
        private List<Slot> slotList;
        private int width, height;
        private Boolean changesMade;

        private string zonesFolder = "Zones";
        private string mapFile = "Map.xml";
        XElement xmlLoaderMap;

        int stackOrder_map = 0;
        int stackOrder_slots = 1;

        //used when reading data from xml
        int numberOfItemsToRead = 0;
        int numberOfItemsRead = 0;

        ConstructionPanel constructionPanel = null;
        #endregion

        #region Properties
        private GameInterface GUI
        {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }

        public List<Slot> SlotList
        {
            get { return slotList; }
        }

        public override int Width
        {
            get
            {
                return width;
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
                return height;
            }
            set
            {
                base.Height = value;
            }
        }

        /// <summary>
        /// Indicates that there were changes made and the history needs to be rewritten.
        /// </summary>
        public Boolean ChangesMade
        {
            get { return changesMade; }
            set { changesMade = value; }
        }

        public MilleniumGoalsSet MapProduction
        {
            get { return new MilleniumGoalsSet(20, 20, 20, 20, 20, 20); }
        }
        public Rectangle MapBounds
        {
            get { return new Rectangle(0, 0, width, height); }
        }

        public int PercentCreated
        {
            get { return Convert.ToInt32((numberOfItemsRead * 100) / numberOfItemsToRead); }
        }
        public ProfileManager ProfilesManager
        {
            get { return (ProfileManager)Game.Services.GetService(typeof(ProfileManager)); }
        }
        public DisplayManager DisplayManager
        {
            get { return (DisplayManager)Game.Services.GetService(typeof(DisplayManager)); }
        }
        #endregion
        
        #region Constructor
        public GameMap(Game game, string Zone)
            : base(game)
        {
            changesMade = false;

            mapFile = zonesFolder + "\\" + Zone + "\\" + mapFile;

            //Loading the Map
            xmlLoaderMap = XElement.Load(mapFile);
            IEnumerable<XElement> mapData = from element in xmlLoaderMap.Elements() select element;
            int elementCount = 0;

            int w = 0;
            int h = 0;

            slotList = new List<Slot>();

            foreach (XElement data in mapData)
            {
                numberOfItemsToRead += data.Elements().Count();
            }

            List<Slot> historySlotList = ProfilesManager.LoadZoneHistory();
            numberOfItemsToRead += historySlotList.Count;

            foreach (XElement data in mapData) //3 categories: Map Dimensions, Map Elements, Slots
            {
                switch (elementCount)
                {
                    case 0: //reads the map dimensions and loades the ground

                        w = Convert.ToInt32(data.Element("Width").Value);
                        h = Convert.ToInt32(data.Element("Height").Value);

                        for (int i = 0; i < w; i++)
                        {
                            for (int j = 0; j < h; j++)
                            {
                                Sprite ground = new Sprite(game, GraphicsCollection.GetPack("ground"));
                                ground.StackOrder = stackOrder_map;
                                ground.XRelative = ground.Width * i;
                                ground.YRelative = ground.Height * j;
                                ground.FrameNumber = 0;
                                AddChild(ground);

                                width = w * ground.Width;
                                height = h * ground.Height;
                            }
                        }
                        numberOfItemsRead++;
                        DisplayManager.ChangePreloaderPercent(PercentCreated);
                        break;
                    case 1: //reads the map elements
                        IEnumerable<XElement> mapElements = from element in data.Elements() select element;

                        foreach (XElement mapElement in mapElements)
                        {
                            string TexturePack = Convert.ToString(mapElement.Element("TexturePack").Value);
                            int frameNumber = Convert.ToInt32(mapElement.Element("FrameNumber").Value);
                            int X = Convert.ToInt32(mapElement.Element("X").Value);
                            int Y = Convert.ToInt32(mapElement.Element("Y").Value);

                            Sprite mapSpriteElement = new Sprite(game, GraphicsCollection.GetPack(TexturePack).Frames[frameNumber]);
                            mapSpriteElement.StackOrder = stackOrder_map;
                            mapSpriteElement.XRelative = X;
                            mapSpriteElement.YRelative = Y;
                            mapSpriteElement.FrameNumber = 0;
                            AddChild(mapSpriteElement);

                            numberOfItemsRead++;
                            DisplayManager.ChangePreloaderPercent(PercentCreated);
                        }
                        break;
                    case 2: //reads the slots
                        IEnumerable<XElement> slotsData = from element in data.Elements() select element;

                        foreach (XElement slotElement in slotsData)
                        {
                            ConstructionType slot_constructionType = (ConstructionType)Enum.Parse(typeof(ConstructionType), Convert.ToString(slotElement.Element("ConstructionType").Value));

                            int X = Convert.ToInt32(slotElement.Element("X").Value);
                            int Y = Convert.ToInt32(slotElement.Element("Y").Value);

                            Slot slot;
                            slot = new Slot(game, slot_constructionType);
                            slot.StackOrder = stackOrder_slots;
                            slot.XSlotCenter = X;
                            slot.YSlotCenter = Y;
                            slot.Visible = false;
                            slot.OnSelected += new EventHandler(slot_OnSelected);
                            AddChild(slot);
                            slotList.Add(slot);

                            numberOfItemsRead++;
                            DisplayManager.ChangePreloaderPercent(PercentCreated);
                        }
                        break;
                        //case 3, se det slot-ul dupa x si y si se face instanta slot
                        // se face instanta reservation
                    //se trimit SetConstructionReservation(slot+reservation)                    
                }
                elementCount++;
            }

            //reading zone history
            for (int i = 0; i < slotList.Count; i++)
            {
                for (int j = 0; j < historySlotList.Count; j++)
                {
                    if (slotList[i].ConstructionType == historySlotList[j].ConstructionType &&
                        slotList[i].XSlotCenter == historySlotList[j].XSlotCenter &&
                        slotList[i].YSlotCenter == historySlotList[j].YSlotCenter)
                    {
                        slotList[i].ReservationList = historySlotList[j].ReservationList;
                        numberOfItemsRead++;
                        DisplayManager.ChangePreloaderPercent(PercentCreated);
                        break;
                    }
                }
            }

            Game.Services.AddService(typeof(GameMap), this);

            //test only, the population will be loaded from the saved game
            GameManager.LoadHistory();
            changesMade = true;
            GameManager.UpdateYear(2010);

            HideSlots();

            DisplayManager.GameMap_HidePreloader();
        }
        
        #endregion

        #region Building

        void slot_OnSelected(object sender, EventArgs e)
        {
            GUI.ConfirmBuild((Slot)sender);
        }
        /// <summary>
        /// Called to display all the building slots of a certain category
        /// </summary>
        /// <param name="category"></param>
        public void DisplaySlots(ConstructionType category)
        {
            HideSlots();
            List<Point> positions = new List<Point>();
            foreach (Slot slot in slotList)
            {
                if (slot.ConstructionType == category)
                    if (slot.IsFree(GameManager.GetCurrentYear()))
                    {
                        slot.IsVisible = true;
                        slot.Enabled = true;
                        positions.Add(new Point(slot.XRelative, slot.YRelative));
                    }
            }
            GUI.DisplaySlots(positions);
        }

        /// <summary>
        /// Hides all the slots
        /// </summary>
        public void HideSlots()
        {
            foreach (Slot slot in slotList)
            {
                slot.IsVisible = false;
                slot.Enabled = false;
            }
            GUI.EraseMinimapSlots();
        }

        /// <summary>
        /// Create a new building.
        /// </summary>
        /// <param name="slot">The slot on which to create the building.</param>
        /// <param name="building">The building type.</param>

        public void CreateBuilding(Slot slot, Construction building)
        {
            CreateBuilding(slot, building, GameManager.CurrentYear);
            RemoveConstructionPanel();
        }

        public void SetConstructionReservation(Slot slot, Reservation reservation)
        {
        }

        public void CreateBuilding(Slot slot, Construction building, int year)
        {
            BuildingsBaseProperties properties = new BuildingsBaseProperties();

            BaseConstruction newBuilding;
            if (slot.ConstructionType != ConstructionType.Population)
                newBuilding = new BaseConstruction(this.Game, slot.XSlotCenter, slot.YSlotCenter, false, false);
            else
                newBuilding = new BaseConstruction(this.Game, slot.XSlotCenter, slot.YSlotCenter, true, false);

            newBuilding.ConstructionName = building;
            newBuilding.ConstructionYear = year;
            newBuilding.ConstructionType = slot.ConstructionType;
            newBuilding.ConstructionLifetime = properties.BuildingBaseProperties(building, year, 0, year).Lifetime;
            newBuilding.ConstructingPeriod = properties.BuildingBaseProperties(building, year, 0, year).ConstructingPeriod;
            newBuilding.DegradationPeriod = properties.BuildingBaseProperties(building, year, 0, year).DestructionPeriod;
            newBuilding.StackOrder = stackOrder_slots + 1;
            newBuilding.ChangeGraphics();
            newBuilding.Slot = slot;

            AddChild(newBuilding);

            slot.CurrentConstruction = newBuilding;
            slot.MakeReservation(year, newBuilding.ConstructionLifetime, building);

            changesMade = true;

            GUI.ClearSelectedBuildingIcon();

            newBuilding.OnSelected += new EventHandler(Building_OnSelected);
            RemoveConstructionPanel();
        }

        /// <summary>
        /// Attach a new building to a slot.
        /// </summary>
        /// <param name="slot"></param>
        private void SetNewBuilding(Slot slot)
        {
            BuildingsBaseProperties properties = new BuildingsBaseProperties();
            BaseConstruction building;
            if (slot.ConstructionType != ConstructionType.Population)
            {
                building = new BaseConstruction(this.Game, slot.XSlotCenter, slot.YSlotCenter, false, false);
            }
            else
            {
                building = new BaseConstruction(this.Game, slot.XSlotCenter, slot.YSlotCenter, true, slot.GetReservation(GameManager.CurrentYear).IsPopulationUpgraded);
            }

            building.ConstructionName = slot.GetReservation(GameManager.CurrentYear).ConstructionName;
            building.ConstructionYear = slot.GetReservation(GameManager.CurrentYear).StartingYear;
            building.ConstructionLifetime = slot.GetReservation(GameManager.CurrentYear).Duration;
            building.ConstructionType = slot.ConstructionType;
            if (slot.GetReservation(GameManager.CurrentYear).IsDestroyed)
            {
                building.DegradationPeriod = 0;
            }
            else
            {
                building.DegradationPeriod = 1; //properties.BuildingBaseProperties(building.ConstructionName, building.ConstructionYear, building.UpgradeYear, GameManager.CurrentYear).DestructionPeriod;
            }
            building.ConstructingPeriod = properties.BuildingBaseProperties(building.ConstructionName, building.ConstructionYear, building.UpgradeYear, GameManager.CurrentYear).ConstructingPeriod;
            building.StackOrder = stackOrder_slots + 1;
            AddChild(building);
            building.ChangeGraphics();
            slot.CurrentConstruction = building;
            building.Slot = slot;

            building.OnSelected += new EventHandler(Building_OnSelected);
            RemoveConstructionPanel();
        }

        public void Upgrade(Slot slot, int year)
        {
            BuildingsBaseProperties properties = new BuildingsBaseProperties();
            int price = properties.BuildingBaseProperties(slot.GetReservation(year).ConstructionName,
                slot.GetReservation(year).StartingYear, year, year).Price;
            GameManager.SpendMoney(year, price / 10);
            GUI.SetMoney(GameManager.GetMoney(year));
            changesMade = true;
        }

        public void UndoBuild(int year, Slot slot)
        {
            BuildingsBaseProperties properties = new BuildingsBaseProperties();
            int price = properties.BuildingBaseProperties(slot.GetReservation(year).ConstructionName,
                slot.GetReservation(year).StartingYear, year, year).Price;
            GUI.SetMoney(GameManager.GetMoney(year) + price);
            changesMade = true;
        }
        #endregion

        #region Selection
        void GameMap_OnMouseRelease(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            if (selectedObject != null)
            {
                Debug.AddToLog("Selected object: " + selectedObject.ToString());
            }
            ClearSelection();
        }

        private void ClearSelection()
        {
            if (selectedObject != null)
            {
                selectedObject.ClearSelection();
                selectedObject = null;
            }
        }
        #endregion

        #region GameProcessing

        /// <summary>
        /// Refresh the building associated with a slot.
        /// </summary>
        /// <param name="slot"></param>
        public void RefreshBuilding(Slot slot)
        {
            //if the slot has a building already attached to it
            if (slot.CurrentConstruction != null)
            {
                //if the slot has a reservation in the current year
                if (slot.GetReservation(GameManager.CurrentYear) != null)
                {
                    //if the current building doesn't match the current reservation
                    if ((slot.GetReservation(GameManager.CurrentYear).StartingYear != slot.CurrentConstruction.ConstructionYear) ||
                        (slot.GetReservation(GameManager.CurrentYear).EndingYear != slot.CurrentConstruction.DestructionYear))
                    {
                        RemoveChild(slot.CurrentConstruction);
                        slot.CurrentConstruction = null;

                        SetNewBuilding(slot);
                    }
                    //if the building matches the current reservation
                    else
                    {
                        slot.CurrentConstruction.ChangeGraphics();
                    }

                }
                //if the slot has no reservation
                else
                {
                    RemoveChild(slot.CurrentConstruction);
                    slot.CurrentConstruction = null;
                }
            }
            //if the slot has no construction attached
            else
            {
                //if the slot has a reservation
                if (slot.GetReservation(GameManager.CurrentYear) != null)
                {
                    SetNewBuilding(slot);
                }
            }
        }
        
        public void UpdateYear(int year)
        {
            RemoveConstructionPanel();
            foreach (Slot slot in slotList)
            {
                RefreshBuilding(slot);
            }
            SetAlerts(GameManager.GetConsumptionCoverage(year));
        }

        /// <summary>
        /// returns a list with all the reservations for a certain year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Reservation> GetReservationsForYear(int year)
        {
            List<Reservation> reservationList = new List<Reservation>();
            foreach (Slot slot in slotList)
            {
                if (slot.GetReservation(year) != null)
                {
                    reservationList.Add(slot.GetReservation(year));
                }
            }
            return reservationList;
        }

        public void ResetYearlyParameters()
        {
            changesMade = false;
        }
        #endregion

        public void RemoveReservation(Reservation r)
        {
            foreach (Slot s in slotList)
            {
                if (s.HasReservation(r))
                {
                    s.CancelReservation(r);
                    return;
                }
            }
        }

        public void UpgradePopulationConstruction(Slot slot, Construction populationConstruction)
        {
            Reservation reservation = new Reservation(GameManager.CurrentYear, 500, populationConstruction);
            reservation.IsPopulationUpgraded = true;
            slot.MakeReservation(reservation);
        }

        #region Construction Panel
        void Building_OnSelected(object sender, EventArgs e)
        {
            RemoveConstructionPanel();

            BaseConstruction construction = ((BaseConstruction)sender);
            constructionPanel = new ConstructionPanel(this.Game, construction);
            constructionPanel.StackOrder = stackOrder_slots + 15;
            UpdateConstructionPanelPosition();
            AddChild(constructionPanel);
            constructionPanel.IsVisible = true;
            constructionPanel.Enabled = true;

        }

        public void RemoveConstructionPanel()
        {
            if (constructionPanel != null)
            {
                constructionPanel.Close();
                RemoveChild(constructionPanel);
                constructionPanel = null;
            }
        }

        public void SetAlerts(MilleniumGoalsSet mg)
        {
            string text = "ALERT\n\n";
            if (mg.Economy * 100 < 30)
                text += "Economy is at a low level!\n";
            if (mg.Education * 100 < 30)
                text += "Education is at a low level!\n";
            if (mg.Energy * 100 < 30)
                text += "Energy is at a low level!\n";
            if (mg.Environment * 100 < 30)
                text += "Environment is at a low level!\n";
            if (mg.Health * 100 < 30)
                text += "Health is at a low level!\n";
            if (mg.Food * 100 < 30)
                text += "Food is at a low level!\n";
            GUI.SetAlertPanel(text);
        }

        public void UpdateConstructionPanelPosition()
        {
            if (constructionPanel != null)
            {
                constructionPanel.XRelative = (int)(GUI.XRelative + (GUI.Width / 2) - (constructionPanel.Width / 2));
                constructionPanel.YRelative = (int)(GUI.YRelative + (GUI.Height / 2) - (constructionPanel.Height / 2));
            }

        }
        #endregion       
    }
}
