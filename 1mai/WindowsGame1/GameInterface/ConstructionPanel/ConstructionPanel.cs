using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Reflection;
using Operation_Cronos;
using Operation_Cronos.Sound;

namespace Operation_Cronos.Display
{
    public class ConstructionPanel : InputVisualComponent
    {
        #region Fields
        Sprite frame;
        ConstructionPanelButton destroy;
        ConstructionPanelButton upgrade;
        ConstructionPanelButton repair;
        ConstructionPanelButton close;

        BaseConstruction selectedConstruction;

        SpriteText textBuildingNameAndStage;
        SpriteText textGeneralInfo;
        SpriteText textUpgradeCost;
        SpriteText textRepairCost;

        int buttonsXposition = 20;

        Tooltip tooltip = null;
        #endregion

        #region Properties
        public GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }

        public GameInterface GameInterface
        {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }

        public GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }

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

        #region Constructor
        public ConstructionPanel(Game game, BaseConstruction construction)
            : base(game)
        {
            frame = new Sprite(game, GraphicsCollection.GetPack("construction-panel-frame"));
            frame.StackOrder = 1;
            AddChild(frame);

            selectedConstruction = construction;

            if (Destroyable())
            {
                destroy = new ConstructionPanelButton(game, ConstructionPanelButtonType.Destroy);
                destroy.StackOrder = 2;
                destroy.XRelative = buttonsXposition;
                destroy.YRelative = 45;
                destroy.OnMouseRelease += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(destroy_OnMouseRelease);
                destroy.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
                destroy.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
                AddChild(destroy);
            }

            if (GeneralConstructionUpgradeable())
            {
                upgrade = new ConstructionPanelButton(game, ConstructionPanelButtonType.Upgrade);
                upgrade.StackOrder = 2;
                upgrade.XRelative = buttonsXposition;
                upgrade.YRelative = 173;
                upgrade.OnMouseRelease += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(upgrade_OnMouseRelease);
                upgrade.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
                upgrade.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
                AddChild(upgrade);

                textUpgradeCost = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
                textUpgradeCost.StackOrder = 2;
                textUpgradeCost.XRelative = 95;
                textUpgradeCost.YRelative = 225;
                textUpgradeCost.Tint = Color.Green;
                textUpgradeCost.Text = GetUpgradeText() + GetUpgradeCost().ToString();
                AddChild(textUpgradeCost);
            }

            if (PopulationConstructionUpgradeable())
            {
                upgrade = new ConstructionPanelButton(game, ConstructionPanelButtonType.Upgrade);
                upgrade.StackOrder = 2;
                upgrade.XRelative = buttonsXposition;
                upgrade.YRelative = 173;
                upgrade.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(upgrade_OnMouseRelease);
                upgrade.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
                upgrade.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
                AddChild(upgrade);

                textUpgradeCost = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
                textUpgradeCost.XRelative = 95;
                textUpgradeCost.YRelative = 225;
                textUpgradeCost.StackOrder = 2;
                textUpgradeCost.Tint = Color.Green;
                textUpgradeCost.Text = GetUpgradeText() + GetUpgradeCost().ToString();
                AddChild(textUpgradeCost);
            }

            if (Repairable())
            {
                repair = new ConstructionPanelButton(game, ConstructionPanelButtonType.Repair);
                repair.StackOrder = 2;
                repair.XRelative = buttonsXposition;
                repair.YRelative = 108;
                repair.OnMouseRelease += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(repair_OnMouseRelease);
                repair.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
                repair.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
                AddChild(repair);

                textRepairCost = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
                textRepairCost.XRelative = 323;
                textRepairCost.YRelative = 225;
                textRepairCost.StackOrder = 2;
                textRepairCost.Tint = Color.Green;
                textRepairCost.Text = "Repair price: " + GetRepairCost().ToString();
                AddChild(textRepairCost);
            }
            else //not repairable
            {
                if (AlreadyRepairedInCurrentYear())
                {
                    textRepairCost = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
                    textRepairCost.XRelative = 323;
                    textRepairCost.YRelative = 225;
                    textRepairCost.Tint = Color.Green;
                    textRepairCost.StackOrder = 2;
                    textRepairCost.Text = "Repaired this year";
                    AddChild(textRepairCost);
                }
            }

            close = new ConstructionPanelButton(game, ConstructionPanelButtonType.Close);
            close.StackOrder = 2;
            close.XRelative = 430;
            close.YRelative = 7;
            close.OnMouseRelease += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(close_OnMouseRelease);
            AddChild(close);

            textBuildingNameAndStage = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            textBuildingNameAndStage.XRelative = 95;
            textBuildingNameAndStage.YRelative = 20;
            textBuildingNameAndStage.Tint = Color.Gray;
            textBuildingNameAndStage.StackOrder = 2;
            textBuildingNameAndStage.Text = GetDescriptionFromEnum(selectedConstruction.ConstructionName, 0);
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
                textBuildingNameAndStage.Text += "  -  " + GetDescriptionFromEnum((selectedConstruction.Slot.GetReservation(GameManager.CurrentYear).Status(GameManager.CurrentYear)), 0);
            AddChild(textBuildingNameAndStage);

            textGeneralInfo = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            textGeneralInfo.XRelative = 95;
            textGeneralInfo.YRelative = 60;
            textGeneralInfo.StackOrder = 2;
            textGeneralInfo.Tint = Color.White;
            textGeneralInfo.Text = GetConstructionYear() + "\n" + GetLifetimeLeft() + "\n" + GetConstructionParameters();
            AddChild(textGeneralInfo);

            selectedConstruction.SelectConstruction = true;

            if (selectedConstruction.ConstructionType == ConstructionType.Education ||
                 selectedConstruction.ConstructionType == ConstructionType.Health ||
                 selectedConstruction.ConstructionType == ConstructionType.Population ||
                 selectedConstruction.ConstructionType == ConstructionType.Energy)
                SoundManager.PlaySound((Sounds)Enum.Parse(typeof(Sounds), selectedConstruction.ConstructionType.ToString()));
            else
                SoundManager.PlaySound((Sounds)Enum.Parse(typeof(Sounds), selectedConstruction.ConstructionName.ToString()));
        }
        #endregion

        #region Event Handlers

        #region Destroy
        void destroy_OnMouseRelease(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            selectedConstruction.SelectConstruction = false;

            if (selectedConstruction.Slot.GetReservation(GameManager.CurrentYear).Status(GameManager.CurrentYear) == ConstructionStatus.InConstruction)
            {
                GameMap.UndoBuild(GameManager.CurrentYear, selectedConstruction.Slot);
                selectedConstruction.Slot.UndoReservation(GameManager.CurrentYear);
            }
            else
                selectedConstruction.Slot.ShortenReservation(GameManager.CurrentYear);

            GameMap.RefreshBuilding(selectedConstruction.Slot);
            int year = GameManager.CurrentYear;
            GameManager.UpdateYear(year);

            Close();
        }
        #endregion

        #region Repair
        void repair_OnMouseRelease(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            int price = GetRepairCost();
                //GameManager.BuildingProperties.BuildingBaseProperties(selectedConstruction.ConstructionName, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).Price;
            
            if (GameManager.GetMoney(GameManager.CurrentYear) >= price)
            {
                GameManager.SpendMoney(GameManager.CurrentYear, price);
                GameInterface.SetMoney(GameManager.GetMoney(GameManager.CurrentYear));

                //do repair 
                BuildingsBaseProperties properties = new BuildingsBaseProperties();
                selectedConstruction.SelectConstruction = false;

                selectedConstruction.Slot.ProlongReservation(GameManager.CurrentYear);
                selectedConstruction.ConstructionLifetime = (GameManager.CurrentYear - selectedConstruction.ConstructionYear) + properties.BuildingBaseProperties(selectedConstruction.ConstructionName, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).Lifetime;
                GameMap.RefreshBuilding(selectedConstruction.Slot);

                Close();
                int year = GameManager.CurrentYear;
                GameManager.UpdateYear(year);
            }
            else
            {
                //Debug.AddToLog("Not enough cashcaval");
                textRepairCost.Text = "Can not afford to repair";
                textRepairCost.Tint = Color.Red;
            }
        }
        #endregion

        #region Upgrade
        void upgrade_OnMouseRelease(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            int price = GetUpgradeCost();            
            if (GameManager.GetMoney(GameManager.CurrentYear) >= price)
            {
                GameManager.SpendMoney(GameManager.CurrentYear, price);
                GameInterface.SetMoney(GameManager.GetMoney(GameManager.CurrentYear));

                //do upgrade
                if (selectedConstruction.ConstructionType != ConstructionType.Population)
                {
                    selectedConstruction.SelectConstruction = false;
                    selectedConstruction.UpgradeYear = GameManager.CurrentYear;
                    selectedConstruction.Slot.Upgrade(GameManager.CurrentYear);
                    GameMap.RefreshBuilding(selectedConstruction.Slot);
                    GameMap.Upgrade(selectedConstruction.Slot, GameManager.CurrentYear);
                }
                else
                {
                    selectedConstruction.Slot.ShortenReservation(GameManager.CurrentYear);
                    GameMap.RefreshBuilding(selectedConstruction.Slot);

                    switch (selectedConstruction.ConstructionName)
                    {
                        case Construction.PopulationVillage:
                            GameMap.UpgradePopulationConstruction(selectedConstruction.Slot, Construction.PopulationTown);
                            break;
                        case Construction.PopulationTown:
                            GameMap.UpgradePopulationConstruction(selectedConstruction.Slot, Construction.PopulationCity);
                            break;
                        case Construction.PopulationCity:
                            GameMap.UpgradePopulationConstruction(selectedConstruction.Slot, Construction.PopulationMetropolis); ;
                            break;
                    }

                    GameMap.RefreshBuilding(selectedConstruction.Slot);
                    int year = GameManager.CurrentYear;
                    GameManager.UpdateYear(year);

                    Close();
                }
            }
            else
            {
                //Debug.AddToLog("Not enough cashcaval");
                textUpgradeCost.Text = "Can not afford to upgrade";
                textUpgradeCost.Tint = Color.Red;
            }
        }
        #endregion

        #region Close
        void close_OnMouseRelease(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            if (selectedConstruction.ConstructionType == ConstructionType.Education ||
                selectedConstruction.ConstructionType == ConstructionType.Health ||
                selectedConstruction.ConstructionType == ConstructionType.Population ||
                selectedConstruction.ConstructionType == ConstructionType.Energy)
                SoundManager.StopSound((Sounds)Enum.Parse(typeof(Sounds), selectedConstruction.ConstructionType.ToString()));
            else
                SoundManager.StopSound((Sounds)Enum.Parse(typeof(Sounds), selectedConstruction.ConstructionName.ToString()));
    

            selectedConstruction.SelectConstruction = false;
            this.IsVisible = false;
            this.Enabled = false;
        }

        public void Close()
        {
            close_OnMouseRelease(null, null);
        }
        #endregion

        #region MouseOver/MouseLeave
        public void Button_OnMouseOver(object sender, ButtonEventArgs args)
        {
            if (tooltip == null)
            {
                tooltip = new Tooltip(this.Game, 1);
                tooltip.XRelative = buttonsXposition + 8;
                tooltip.YRelative = 5;
                tooltip.StackOrder = 3;
                tooltip.Visible = true;
                AddChild(tooltip);
            }
            tooltip.Text = " " + ((ConstructionPanelButton)sender).ButtonType.ToString();
            tooltip.YRelative = ((ConstructionPanelButton)sender).YRelative + 40;
            tooltip.IsVisible = true;
        }

        public void Button_OnMouseLeave(object sender, ButtonEventArgs args)
        {
            tooltip.IsVisible = false;
            tooltip.Text = "";
        }
        #endregion

        #endregion

        #region Methods

        #region Destroy Related
        /// <summary>
        /// Determines if the construction is destroyable
        /// </summary>
        private bool Destroyable()
        {
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
                return true;
            else
                return false;
        }
        #endregion

        #region Upgrade Related
        /// <summary>
        /// Determines if the construction is a upgradeable general construction
        /// </summary>
        private bool GeneralConstructionUpgradeable()
        {
            if (selectedConstruction.ConstructionType == ConstructionType.Population)
                return false;

            bool canUpgrade = true;
            //upgrades can not be made if 
            //- currently in HP1 (1)
            //- currently in HP2 and 
            //                      - constructionYear in HP2 (2)
            //                      - constructionYear in HP1 BUT (upgraded and after upgradeYear)(3)
            //                      - constructionYear in HP1 and constructionStage - in Construction (4)
            //                      - constructinStage - in Degradation (5)

            if (GameManager.CurrentYear < (int)HistoricalPeriod.HP2_FirstYear) //(1)
                canUpgrade = false;
            if ((GameManager.CurrentYear >= (int)HistoricalPeriod.HP2_FirstYear) &&
                (selectedConstruction.ConstructionYear >= (int)HistoricalPeriod.HP2_FirstYear)) //(2)
                canUpgrade = false;
            if ((GameManager.CurrentYear >= (int)HistoricalPeriod.HP2_FirstYear) && //(3)
                (selectedConstruction.ConstructionYear < (int)HistoricalPeriod.HP2_FirstYear) &&
                (selectedConstruction.UpgradeYear >= (int)HistoricalPeriod.HP2_FirstYear) &&
                (GameManager.CurrentYear >= selectedConstruction.UpgradeYear))
                canUpgrade = false;
            if ((GameManager.CurrentYear >= (int)HistoricalPeriod.HP2_FirstYear) &&
                (selectedConstruction.ConstructionYear < (int)HistoricalPeriod.HP2_FirstYear) && //(4)
                (selectedConstruction.Slot.GetReservation(GameManager.CurrentYear).Status(GameManager.CurrentYear) == ConstructionStatus.InConstruction))
                canUpgrade = false;
            if ((GameManager.CurrentYear >= (int)HistoricalPeriod.HP2_FirstYear) && //(4)
                (selectedConstruction.Slot.GetReservation(GameManager.CurrentYear).Status(GameManager.CurrentYear) == ConstructionStatus.InDegradation))
                canUpgrade = false;
            return canUpgrade;
        }

        /// <summary>
        /// Determines if the construction is an upgradeable population construction
        /// </summary>
        private bool PopulationConstructionUpgradeable()
        {
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
                return false;
            if (selectedConstruction.ConstructionName == Construction.PopulationMetropolis)
                return false;
            else
            {
                return true;
            }

        }

        private string GetUpgradeText()
        {
            string text = "";
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
            {
                text = "Upgrade cost: ";
            }
            else
            {
                switch (selectedConstruction.ConstructionName)
                {
                    case Construction.PopulationVillage:
                        text = "Upgrade to Town cost: ";
                        break;
                    case Construction.PopulationTown:
                        text = "Upgrade to City cost: ";
                        break;
                    case Construction.PopulationCity:
                        text = "Upgrade to Metropolis cost: ";
                        break;
                }
            }
            return text;
        }

        private int GetUpgradeCost()
        {
            BuildingsBaseProperties bp = new BuildingsBaseProperties();
            int upgradePrice = 0;
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
                upgradePrice = bp.BuildingBaseProperties(selectedConstruction.ConstructionName, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).UpgradePrice;
            else
            {
                switch (selectedConstruction.ConstructionName)
                {//gets the upgrade cost of the next stage population construction
                    case Construction.PopulationVillage:
                        upgradePrice = bp.BuildingBaseProperties(Construction.PopulationTown, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).UpgradePricePopulation;
                        break;
                    case Construction.PopulationTown:
                        upgradePrice = bp.BuildingBaseProperties(Construction.PopulationCity, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).UpgradePricePopulation;
                        break;
                    case Construction.PopulationCity:
                        upgradePrice = bp.BuildingBaseProperties(Construction.PopulationMetropolis, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).UpgradePricePopulation;
                        break;
                }
            }

            int cost = upgradePrice;
            return cost;
        }
        #endregion

        #region Repair Related
        /// <summary>
        /// Determines if the construction is repairable
        /// </summary>
        private bool Repairable()
        {
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
            {
                BuildingsBaseProperties properties = new BuildingsBaseProperties();
                if (selectedConstruction.ConstructionLifetime >= ((GameManager.CurrentYear - selectedConstruction.ConstructionYear) + properties.BuildingBaseProperties(selectedConstruction.ConstructionName, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).Lifetime))
                    return false;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        ///Determines if the construction was already repaired in the current year
        /// </summary>
        private bool AlreadyRepairedInCurrentYear()
        {
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
            {
                BuildingsBaseProperties properties = new BuildingsBaseProperties();
                if (selectedConstruction.ConstructionLifetime == ((GameManager.CurrentYear - selectedConstruction.ConstructionYear) + properties.BuildingBaseProperties(selectedConstruction.ConstructionName, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).Lifetime))
                    return true;
                else return false;
            }
            return false;
        }

        private int GetRepairCost()
        {
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
            {
                BuildingsBaseProperties bp = new BuildingsBaseProperties();
                int price = bp.BuildingBaseProperties(selectedConstruction.ConstructionName, selectedConstruction.ConstructionYear, selectedConstruction.UpgradeYear, GameManager.CurrentYear).Price;
                int lifetimeLeftPercentage = (int)(((double)(selectedConstruction.DestructionYear - GameManager.CurrentYear) * 100) / (double)(selectedConstruction.ConstructionLifetime));
                int repairCost = (int)((100 - lifetimeLeftPercentage) * ((double)price / 100));
                return repairCost;
            }
            else
                return 0;
        }
        #endregion

        #region General

        private string GetConstructionYear()
        {
            return "Built in " + selectedConstruction.ConstructionYear.ToString();
        }

        private string GetLifetimeLeft()
        {
            string text = "";
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
            {
                text += "Lifetime left: ";

                int lifetimeLeftPercentage = (int)(((double)(selectedConstruction.DestructionYear - GameManager.CurrentYear) * 100) / (double)(selectedConstruction.ConstructionLifetime));

                text += lifetimeLeftPercentage.ToString() + "%";
            }
            return text;
        }

        private string GetConstructionParameters()
        {
            if (selectedConstruction.ConstructionType != ConstructionType.Population)
            {
                BuildingsBaseBonuses parameters = new BuildingsBaseBonuses();
                string text = "Production and consumption:";
                int year = 0;
                if (selectedConstruction.ConstructionYear < (int)HistoricalPeriod.HP2_FirstYear)
                {
                    if (selectedConstruction.UpgradeYear >= (int)HistoricalPeriod.HP2_FirstYear)
                    {
                        if (GameManager.CurrentYear < selectedConstruction.UpgradeYear)
                            year = (int)HistoricalPeriod.HP1_FirstYear;
                        else
                            year = (int)HistoricalPeriod.HP2_FirstYear;
                    }
                    else
                        year = (int)HistoricalPeriod.HP1_FirstYear;
                }
                else
                    year = (int)HistoricalPeriod.HP2_FirstYear;

                Debug.AddToLog(selectedConstruction.UpgradeYear.ToString() + " " + year.ToString());

                float Economy = parameters.BuildingMilleniumGoalSet(selectedConstruction.ConstructionName, year).Economy;
                float Health = parameters.BuildingMilleniumGoalSet(selectedConstruction.ConstructionName, year).Health;
                float Education = parameters.BuildingMilleniumGoalSet(selectedConstruction.ConstructionName, year).Education;
                float Energy = parameters.BuildingMilleniumGoalSet(selectedConstruction.ConstructionName, year).Energy;
                float Food = parameters.BuildingMilleniumGoalSet(selectedConstruction.ConstructionName, year).Food;
                float Environment = parameters.BuildingMilleniumGoalSet(selectedConstruction.ConstructionName, year).Environment;

                string pozitiveParams = "";
                string negativeParams = "";

                if (Economy > 0)
                    pozitiveParams += "\n Economy              +" + Economy.ToString();
                else if (Economy < 0)
                    negativeParams += "\n Economy               " + Economy.ToString();
                if (Health > 0)
                    pozitiveParams += "\n Health                  +" + Health.ToString();
                else if (Health < 0)
                    negativeParams += "\n Health                   " + Health.ToString();
                if (Education > 0)
                    pozitiveParams += "\n Education            +" + Education.ToString();
                else if (Education < 0)
                    negativeParams += "\n Education             " + Education.ToString();
                if (Energy > 0)
                    pozitiveParams += "\n Energy                  +" + Energy.ToString();
                else if (Energy < 0)
                    negativeParams += "\n Energy                   " + Energy.ToString();
                if (Food > 0)
                    pozitiveParams += "\n Food                     +" + Food.ToString();
                else if (Food < 0)
                    negativeParams += "\n Food                      " + Food.ToString();
                if (Environment > 0)
                    pozitiveParams += "\n Environment       +" + Environment.ToString();
                else if (Environment < 0)
                    negativeParams += "\n Environment        " + Environment.ToString();

                text += pozitiveParams + negativeParams;
                return text;
            }
            else //population
            {
                return "";
            }
        }
        #endregion

        #endregion

        private string GetDescriptionFromEnum(Enum enumerationType, int descriptionIndex)
        {
            FieldInfo fi = enumerationType.GetType().GetField(enumerationType.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0 && descriptionIndex < attributes.Length)
                return attributes[descriptionIndex].Description;
            else
                return "";
        }
    }
}
