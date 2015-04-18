using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.IO;
using Operation_Cronos.Display;
using Operation_Cronos.GameProcessor;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Reflection;

namespace Operation_Cronos
{
    public enum ConstructionStatus
    {
        [DescriptionAttribute("In construction")]
        InConstruction,
        [DescriptionAttribute("In production")]
        InProduction,
        [DescriptionAttribute("In degradation")]
        InDegradation,
        None
    }

    public class BaseConstruction : SelectableComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;

        protected ConstructionType constructionType;
        protected Construction constructionName;

        int constructionYear;
        int constructionLifetime;
        int constructingPeriod;
        int degradationPeriod;
        int upgradeYear;
        int repairCost;
        int upgradeCost;
        /// <summary>
        /// indicates whether the construction (built in historical period 1) was upgraded to historical period 2
        /// </summary>
        bool upgraded;

        /// <summary>
        /// indicates whether the construction should be highlighted
        /// </summary>
        bool isSelected;
        Color selectionColor = new Color(Color.LightGreen,210);

        Slot slot;

        public ConstructionStatus constructionStatus;
        /// <summary>
        /// indicates if the construction is a general type on or a population on
        /// </summary>
        bool isPopulationConstruction;
        /// <summary>
        /// indicates weather the population construction starts its contructing period from stage 1
        /// or (being created as an update of a preexisting lower level population constructin) from stage 2
        /// </summary>
        bool isPopulationConstruction_UpgradedLevel;

        /// <summary>
        /// (x,y) is the center point of the construction's position (the slot's coordinates)
        /// </summary>
        int x, y;

        /// <summary>
        /// initially historicalPeriod_1/2_Availability==0 but is incremented when 
        /// HP1/2_ConstructionSprite and HP1/2_AspectSprite are instanciated;
        /// when historicalPeriod_1/2_Availability>=2, both HP1/2/_ConstructionSprite and 
        /// HP1/2_AspectSprite were instanciated, 
        /// therefor the construction is interpreted as being available for that historical period (1/2)
        /// </summary>
        int historicalPeriod_1_Availability;
        int historicalPeriod_2_Availability;

        //HP stands for Historical Period
        protected Sprite HP1_ConstructionSprite;
        protected Sprite HP1_AspectSprite;
        protected Sprite HP1_AspectExtraAnimationSprite;
        protected Sprite HP2_ConstructionSprite;
        protected Sprite HP2_AspectSprite;
        protected Sprite HP2_AspectExtraAnimationSprite;
        protected Sprite HP1_DegradationSprite;
        protected Sprite HP2_DegradationSprite;
        List<Sprite> spriteList;

        //!!the preloaderSprite speed must be set before setting the sprite
        protected int HP1_Aspect_AnimationSpeed;
        protected int HP1_Aspect_ExtraAnimationSpeed;
        protected int HP2_Aspect_AnimationSpeed;
        protected int HP2_Aspect_ExtraAnimationSpeed;

        #endregion

        #region Properties

        #region General Properties
        public Slot Slot
        {
            get { return slot; }
            set { slot = value; }
        }
        /// <summary>
        /// Gets or sets the current status of the construction
        /// </summary>
        public ConstructionStatus Status
        {
            get
            {
                return constructionStatus;
            }
            set
            {
                constructionStatus = value;
            }
        }
        /// <summary>
        /// Gets or sets the BaseConstruction Year for the current construction
        /// </summary>
        public int ConstructionYear
        {
            get
            {
                return constructionYear;
            }
            set
            {
                if (value >= 1500 & value <= 2010)
                {
                    constructionYear = value;
                    if (isPopulationConstruction)
                    {
                        if (constructionYear < (int)HistoricalPeriod.HP2_FirstYear)
                        {
                            upgraded = true;
                            if ((constructionYear + constructingPeriod) > (int)HistoricalPeriod.HP2_FirstYear)
                                upgradeYear = constructionYear + constructingPeriod;
                        }
                        else
                            upgraded = false;
                    }
                }
            }
        }

        public int DestructionYear
        {
            get
            {
                return (ConstructionYear + ConstructionLifetime);
            }
        }

        /// <summary>
        /// Gets or sets the BaseConstruction Lifetime for the current construction
        /// MUST set ConstructionYear first
        /// </summary>
        public int ConstructionLifetime
        {
            get
            {
                return constructionLifetime;
            }
            set
            {
                if (value >= 0 && constructionYear > 0)
                    constructionLifetime = value;
            }
        }

        /// <summary>
        /// Gets or sets the Repair Cost for the current construction
        /// </summary>
        public int RepairCost
        {
            get
            {
                return repairCost;
            }
            set
            {
                if (value >= 0)
                    repairCost = value;
            }
        }

        /// <summary>
        /// Gets or sets the Upgrade Cost for the current construction
        /// </summary>
        public int UpgradeCost
        {
            get
            {
                return upgradeCost;
            }
            set
            {
                //if (upgradable)
                if (value >= 0)
                    upgradeCost = value;
            }
        }

        /// <summary>
        /// Gets or sets the BaseConstruction Constructiong Period for the current construction
        /// Atention: construction period must be multiple of 2 to be considered
        /// </summary>
        public int ConstructingPeriod
        {
            get
            {
                return constructingPeriod;
            }
            set
            {
                //if value can be devided by 2, for 2 periods of construction
                if (value > 0)
                    if (Math.IEEERemainder(value, 2) == 0)
                    {
                        if (isPopulationConstruction_UpgradedLevel) //starts direcly from the second constructing stage
                            constructingPeriod = Convert.ToInt16(value / 2);
                        else
                            constructingPeriod = value;
                        if (isPopulationConstruction)
                        {
                            if ((constructionYear < (int)HistoricalPeriod.HP2_FirstYear) && (constructionYear + constructingPeriod) > (int)HistoricalPeriod.HP2_FirstYear)
                                upgradeYear = constructionYear + constructingPeriod;
                        }
                    }
            }
        }

        /// <summary>
        ///Gets or sets the BaseConstruction Degradation Period for the current construction
        /// </summary>
        public int DegradationPeriod
        {
            get
            {
                return degradationPeriod;
            }
            set
            {
                degradationPeriod = value;
            }
        }


        /// <summary>
        ///Gets or sets the BaseConstruction Upgrade Year for the current construction
        ///(the value is accepted (for Set) only if it's a nonPopulation construction
        ///and the upgradeYear > constructionYear
        ///and if upgradeYear is in the second historical period)
        /// </summary>
        public int UpgradeYear
        {
            get
            {
                return upgradeYear;
            }
            set
            {
                if ((value > constructionYear) && (value >= (int)HistoricalPeriod.HP2_FirstYear))
                {
                    if (isPopulationConstruction == false)
                    {
                        upgradeYear = value;
                        upgraded = true;
                    }
                }
            }
        }

        /// <summary>
        /// Shows if the construction has been upgraded (an upgradeYear has been set)
        /// </summary>
        public bool Upgraded
        {
            get
            {
                return upgraded;
            }
        }

        /// <summary>
        /// Gets the type of the BaseConstruction object
        /// </summary>
        public ConstructionType ConstructionType
        {
            get
            {
                return constructionType;
            }
            set
            {
                constructionType = value;
            }
        }

        public Construction ConstructionName
        {
            get
            {
                return constructionName;
            }
            set
            {
                constructionName = value;
                SetGraphics(value);
            }
        }

        public bool SelectConstruction
        {
            set
            {
                isSelected = value;
                ChangeGraphics();
            }
        }
        #endregion

        #region Historical Periods
        /// <summary>
        /// Shows if the construction is available for the First Historical Period
        /// (if both ConstructionSprite and AspectSprite were set for this period)
        /// </summary>
        public bool HistoricalPeriod_1_Availability
        {
            get
            {
                if (historicalPeriod_1_Availability >= 2)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Shows if the construction is available for the Second Historical Period
        /// (if both ConstructionSprite and AspectSprite were set for this period)
        /// </summary>
        public bool HistoricalPeriod_2_Availability
        {
            get
            {
                if (historicalPeriod_2_Availability >= 2)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #endregion

        #region Constructors
        public BaseConstruction(Game game, int xRelative, int yRelative, bool isPopulationConstruction, bool isPopulationConstruction_UpgradedLevel)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));
            x = xRelative;
            y = yRelative;
            this.isPopulationConstruction = isPopulationConstruction;
            this.isPopulationConstruction_UpgradedLevel = isPopulationConstruction_UpgradedLevel;
            spriteList = new List<Sprite>();
            InitialState();
        }
        #endregion

        #region Method InitialState
        void InitialState()
        {
            constructionYear = 0;
            constructionLifetime = 0;
            if (isPopulationConstruction)
            {
                degradationPeriod = 0; //population constructions do not degradate
                upgraded = true;//set to upgrade automatically to historical period 2, when entering it

                if (isPopulationConstruction_UpgradedLevel)
                    constructingPeriod = 1;//takes 1 year to complete the construction stage
                //(because the constructing starts directly from stage 2)
                else
                    constructingPeriod = 2;//takes two years to complete the construction stage
                upgradeYear = (int)HistoricalPeriod.HP2_FirstYear; //initially

            }
            else
            {
                degradationPeriod = 1;//(initially) takes one year for the construction to disappear
                upgradeYear = 0;
                upgraded = false;
                constructingPeriod = 2;//takes two years to complete the construction stage
            }
            repairCost = 0;
            upgradeCost = 0;

            isSelected = false;

            constructionType = ConstructionType.None;
            constructionStatus = ConstructionStatus.None;

            historicalPeriod_1_Availability = 0;
            historicalPeriod_2_Availability = 0;

            HP1_Aspect_AnimationSpeed = 0;
            HP1_Aspect_ExtraAnimationSpeed = 0;
            HP2_Aspect_AnimationSpeed = 0;
            HP2_Aspect_ExtraAnimationSpeed = 0;

            HP1_AspectExtraAnimationSprite = null;
            HP2_AspectExtraAnimationSprite = null;
        }
        #endregion

        #region Methods

        #region HistoricalPeriods Sprites
        /// <summary>
        /// Creates the BaseConstruction Sprite for the First Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP1_ConstructionSprite(string spritePath)
        {
            historicalPeriod_1_Availability++;
            HP1_ConstructionSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP1_ConstructionSprite.XRelative = x - HP1_ConstructionSprite.Width / 2;
            HP1_ConstructionSprite.YRelative = y - HP1_ConstructionSprite.Height / 2;

            HP1_ConstructionSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP1_ConstructionSprite.Visible = false;
            spriteList.Add(HP1_ConstructionSprite);
            AddChild(HP1_ConstructionSprite);
        }

        /// <summary>
        /// Creates the Aspect Sprite for the First Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP1_AspectSprite(string spritePath)
        {
            historicalPeriod_1_Availability++;
            HP1_AspectSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP1_AspectSprite.AnimationSpeed = HP1_Aspect_AnimationSpeed;

            HP1_AspectSprite.XRelative = x - HP1_AspectSprite.Width / 2;
            HP1_AspectSprite.YRelative = y - HP1_AspectSprite.Height / 2;

            HP1_AspectSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP1_AspectSprite.Visible = false;
            spriteList.Add(HP1_AspectSprite);
            AddChild(HP1_AspectSprite);
        }

        /// <summary>
        /// Creates the Aspect ExtraAnimation Sprite for the First Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP1_AspectExtraAnimationSprite(string spritePath)
        {
            HP1_AspectExtraAnimationSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP1_AspectExtraAnimationSprite.AnimationSpeed = HP1_Aspect_ExtraAnimationSpeed;

            HP1_AspectExtraAnimationSprite.XRelative = x - HP1_AspectExtraAnimationSprite.Width / 2;
            HP1_AspectExtraAnimationSprite.YRelative = y - HP1_AspectExtraAnimationSprite.Height / 2;

            HP1_AspectExtraAnimationSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP1_AspectExtraAnimationSprite.Visible = false;
            spriteList.Add(HP1_AspectExtraAnimationSprite);
            AddChild(HP1_AspectExtraAnimationSprite);
        }

        /// <summary>
        /// Creates the Degradation Sprite for the First Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP1_DegradationSprite(string spritePath)
        {
            HP1_DegradationSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP1_DegradationSprite.XRelative = x - HP1_DegradationSprite.Width / 2;
            HP1_DegradationSprite.YRelative = y - HP1_DegradationSprite.Height / 2;

            HP1_DegradationSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP1_DegradationSprite.Visible = false;
            spriteList.Add(HP1_DegradationSprite);
            AddChild(HP1_DegradationSprite);
        }



        /// <summary>
        /// Creates the BaseConstruction Sprite for the Second Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP2_ConstructionSprite(string spritePath)
        {
            historicalPeriod_2_Availability++;
            HP2_ConstructionSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP2_ConstructionSprite.XRelative = x - HP2_ConstructionSprite.Width / 2;
            HP2_ConstructionSprite.YRelative = y - HP2_ConstructionSprite.Height / 2;

            HP2_ConstructionSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP2_ConstructionSprite.Visible = false;
            spriteList.Add(HP2_ConstructionSprite);
            AddChild(HP2_ConstructionSprite);
        }

        /// <summary>
        /// Creates the Aspect Sprite for the Second Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP2_AspectSprite(string spritePath)
        {
            historicalPeriod_2_Availability++;
            HP2_AspectSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP2_AspectSprite.AnimationSpeed = HP2_Aspect_AnimationSpeed;

            HP2_AspectSprite.XRelative = x - HP2_AspectSprite.Width / 2;
            HP2_AspectSprite.YRelative = y - HP2_AspectSprite.Height / 2;

            HP2_AspectSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP2_AspectSprite.Visible = false;
            spriteList.Add(HP2_AspectSprite);
            AddChild(HP2_AspectSprite);
        }

        /// <summary>
        /// Creates the Aspect ExtraAnimation Sprite for the Second Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP2_AspectExtraAnimationSprite(string spritePath)
        {
            HP2_AspectExtraAnimationSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP2_AspectExtraAnimationSprite.AnimationSpeed = HP2_Aspect_ExtraAnimationSpeed;

            HP2_AspectExtraAnimationSprite.XRelative = x - HP2_AspectExtraAnimationSprite.Width / 2;
            HP2_AspectExtraAnimationSprite.YRelative = y - HP2_AspectExtraAnimationSprite.Height / 2;

            HP2_AspectExtraAnimationSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP2_AspectExtraAnimationSprite.Visible = false;
            spriteList.Add(HP2_AspectExtraAnimationSprite);
            AddChild(HP2_AspectExtraAnimationSprite);
        }

        /// <summary>
        /// Creates the Degradation Sprite for the Second Historical Period 
        /// </summary>
        /// <param name="spriteName">The name of the Sprite in the GraphicsCollection Pack</param>    
        public void Set_HP2_DegradationSprite(string spritePath)
        {
            HP2_DegradationSprite = new Sprite(Game, graphicsCollection.GetPack(spritePath));

            HP2_DegradationSprite.XRelative = x - HP2_DegradationSprite.Width / 2;
            HP2_DegradationSprite.YRelative = y - HP2_DegradationSprite.Height / 2;

            HP2_DegradationSprite.highlightColor = selectionColor;

            //initially hidden and disabled
            HP2_DegradationSprite.Visible = false;
            spriteList.Add(HP2_DegradationSprite);
            AddChild(HP2_DegradationSprite);
        }
        #endregion

        #region BaseConstruction Stages

        /// <summary>
        /// Adapts the graphics for the current construction to the construction stage and historical period
        /// Both stage and historical period are previosly determined based on the current year
        /// </summary>
        /// <param name="stage">Stage 0..4; 0 means nothing is displayed, 4 means degradation state</param>
        /// <param name="period">Historical period 1,2</param>
        protected void ConstructionStage(int stage, int period)
        {
            Hide();
            switch (stage)
            {
                case 0:
                    constructionStatus = ConstructionStatus.None;
                    break;
                case 1://first construction stage
                    if (period == 1 && HistoricalPeriod_1_Availability)
                    {
                        HP1_ConstructionSprite.Visible = true;
                        HP1_ConstructionSprite.FrameNumber = 0;
                        HP1_ConstructionSprite.Highlight = isSelected;
                        this.Enabled = true;
                    }
                    if (period == 2 && HistoricalPeriod_2_Availability)
                    {
                        HP2_ConstructionSprite.Visible = true;
                        HP2_ConstructionSprite.FrameNumber = 0;
                        HP2_ConstructionSprite.Highlight = isSelected;
                        this.Enabled = true;  
                    }
                    constructionStatus = ConstructionStatus.InConstruction;
                  break;
                case 2://second construction stage
                    if (period == 1 && HistoricalPeriod_1_Availability)
                    {
                        HP1_ConstructionSprite.Visible = true;
                        HP1_ConstructionSprite.FrameNumber = 1;
                        HP1_ConstructionSprite.Highlight = isSelected;
                        this.Enabled = true;
                       
                    }
                    if (period == 2 && HistoricalPeriod_2_Availability)
                    {
                        HP2_ConstructionSprite.Visible = true;
                        HP2_ConstructionSprite.FrameNumber = 1;
                        HP2_ConstructionSprite.Highlight = isSelected;
                        this.Enabled = true;
                       
                    }
                    constructionStatus = ConstructionStatus.InConstruction;
                    break;
                case 3://third stage construction == aspect sprite; construction finished
                    if (period == 1 && HistoricalPeriod_1_Availability)
                    {
                        HP1_AspectSprite.Visible = true;
                        HP1_AspectSprite.FrameNumber = 0;
                        HP1_AspectSprite.Highlight = isSelected;
                        HP1_AspectSprite.AnimationSpeed = HP1_Aspect_AnimationSpeed;

                        if (HP1_AspectExtraAnimationSprite != null)
                        {
                            HP1_AspectExtraAnimationSprite.Visible = true;
                            HP1_AspectExtraAnimationSprite.FrameNumber = 0;
                            HP1_AspectExtraAnimationSprite.Highlight = isSelected;
                            HP1_AspectExtraAnimationSprite.AnimationSpeed = HP1_Aspect_ExtraAnimationSpeed;
                        }
                        this.Enabled = true;
                    }
                    if (period == 2 && HistoricalPeriod_2_Availability)
                    {
                        HP2_AspectSprite.Visible = true;
                        HP2_AspectSprite.FrameNumber = 0;
                        HP2_AspectSprite.Highlight = isSelected;
                        HP2_AspectSprite.AnimationSpeed = HP2_Aspect_AnimationSpeed;

                        if (HP2_AspectExtraAnimationSprite != null)
                        {
                            HP2_AspectExtraAnimationSprite.Visible = true;
                            HP2_AspectExtraAnimationSprite.FrameNumber = 0;
                            HP2_AspectExtraAnimationSprite.Highlight = isSelected;
                            HP2_AspectExtraAnimationSprite.AnimationSpeed = HP2_Aspect_ExtraAnimationSpeed;
                        }
                        this.Enabled = true;
                    }
                    constructionStatus = ConstructionStatus.InProduction;
                    break;
                case 4://forth stage construction == degradation period
                    if (period == 1 && HistoricalPeriod_1_Availability)
                    {
                        if (HP1_DegradationSprite != null)
                        {
                            HP1_DegradationSprite.Visible = true;
                            HP1_DegradationSprite.Highlight = isSelected;
                        }
                        this.Enabled = true;
                    }
                    if (period == 2 && HistoricalPeriod_2_Availability)
                    {
                        if (HP2_DegradationSprite != null)
                        {
                            HP2_DegradationSprite.Visible = true;
                            HP2_DegradationSprite.Highlight = isSelected;
                        }
                        this.Enabled = true;
                    }
                    constructionStatus = ConstructionStatus.InDegradation;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region SetGraphics
        private void SetGraphics(Construction value)
        {
            switch (value)
            {
                case Construction.EnergySolar:
                    Set_HP2_ConstructionSprite("Energy_SolarPowerPlant_Construction_P2");
                    Set_HP2_AspectSprite("Energy_SolarPowerPlant_Aspect_P2");
                    Set_HP2_DegradationSprite("Energy_SolarPowerPlant_Degradation_P2");
                    break;
                case Construction.EnergySolid:
                    Set_HP2_ConstructionSprite("Energy_SolidFuelPowerPlant_Construction_P2");
                    Set_HP2_AspectSprite("Energy_SolidFuelPowerPlant_Aspect_P2");
                    HP2_Aspect_ExtraAnimationSpeed = 8;
                    Set_HP2_AspectExtraAnimationSprite("Energy_SolidFuelPowerPlant_AspectExtraAnimation_P2");
                    Set_HP2_DegradationSprite("Energy_SolidFuelPowerPlant_Degradation_P2");
                    break;
                case Construction.EnergyWind:
                    Set_HP2_ConstructionSprite("Energy_WindPowerPlant_Construction_P2");
                    HP2_Aspect_AnimationSpeed = 25; //in this order! (before setting the sprite)
                    Set_HP2_AspectSprite("Energy_WindPowerPlant_Aspect_P2");
                    Set_HP2_DegradationSprite("Energy_WindPowerPlant_Degradation_P2");
                    break;
                case Construction.EnergyGeo:
                    Set_HP2_ConstructionSprite("Energy_GeothermalPowerPlant_Construction_P2");
                    Set_HP2_AspectSprite("Energy_GeothermalPowerPlant_Aspect_P2");
                    Set_HP2_DegradationSprite("Energy_GeothermalPowerPlant_Degradation_P2");
                    break;
                case Construction.EnergyNuclear:
                    Set_HP2_ConstructionSprite("Energy_NuclearPowerPlant_Construction_P2");
                    Set_HP2_AspectSprite("Energy_NuclearPowerPlant_Aspect_P2");
                    HP2_Aspect_ExtraAnimationSpeed = 8;
                    Set_HP2_AspectExtraAnimationSprite("Energy_NuclearPowerPlant_AspectExtraAnimation_P2");
                    Set_HP2_DegradationSprite("Energy_NuclearPowerPlant_Degradation_P2");
                    break;
                case Construction.FoodCropFarm:
                    Set_HP1_ConstructionSprite("Food_CropFarm_Construction_P1");
                    Set_HP1_AspectSprite("Food_CropFarm_Aspect_P1");
                    Set_HP1_DegradationSprite("Food_CropFarm_Degradation_P1");

                    Set_HP2_ConstructionSprite("Food_CropFarm_Construction_P2");
                    HP2_Aspect_AnimationSpeed = 5;
                    Set_HP2_AspectSprite("Food_CropFarm_Aspect_P2");
                    Set_HP2_DegradationSprite("Food_CropFarm_Degradation_P2");
                    break;
                case Construction.FoodAnimalFarm:
                    Set_HP1_ConstructionSprite("Food_AnimalFarm_Construction_P1");
                    Set_HP1_AspectSprite("Food_AnimalFarm_Aspect_P1");
                    Set_HP1_DegradationSprite("Food_AnimalFarm_Degradation_P1");

                    Set_HP2_ConstructionSprite("Food_AnimalFarm_Construction_P2");
                    Set_HP2_AspectSprite("Food_AnimalFarm_Aspect_P2");
                    Set_HP2_DegradationSprite("Food_AnimalFarm_Degradation_P2");
                    break;
                case Construction.FoodOrchard:
                    Set_HP1_ConstructionSprite("Food_Orchard_Construction_P1");
                    Set_HP1_AspectSprite("Food_Orchard_Aspect_P1");
                    Set_HP1_DegradationSprite("Food_Orchard_Degradation_P1");

                    Set_HP2_ConstructionSprite("Food_Orchard_Construction_P2");
                    Set_HP2_AspectSprite("Food_Orchard_Aspect_P2");
                    Set_HP2_DegradationSprite("Food_Orchard_Degradation_P2");
                    break;
                case Construction.FoodFarmedFisherie:
                    Set_HP1_ConstructionSprite("Food_FarmedFisherie_Construction_P1");
                    Set_HP1_AspectSprite("Food_FarmedFisherie_Aspect_P1");
                    Set_HP1_DegradationSprite("Food_FarmedFisherie_Degradation_P1");

                    Set_HP2_ConstructionSprite("Food_FarmedFisherie_Construction_P2");
                    Set_HP2_AspectSprite("Food_FarmedFisherie_Aspect_P2");
                    Set_HP2_DegradationSprite("Food_FarmedFisherie_Degradation_P2");
                    break;
                case Construction.FoodWildFisherie:
                    Set_HP1_ConstructionSprite("Food_WildFisherie_Construction_P1");
                    Set_HP1_AspectSprite("Food_WildFisherie_Aspect_P1");
                    Set_HP1_DegradationSprite("Food_WildFisherie_Degradation_P1");

                    Set_HP2_ConstructionSprite("Food_WildFisherie_Construction_P2");
                    Set_HP2_AspectSprite("Food_WildFisherie_Aspect_P2");
                    Set_HP2_DegradationSprite("Food_WildFisherie_Degradation_P2");
                    break;
                case Construction.HealthClinic:
                    Set_HP1_ConstructionSprite("Health_Clinic_Construction_P1");
                    Set_HP1_AspectSprite("Health_Clinic_Aspect_P1");
                    Set_HP1_DegradationSprite("Health_Clinic_Degradation_P1");

                    Set_HP2_ConstructionSprite("Health_Clinic_Construction_P2");
                    Set_HP2_AspectSprite("Health_Clinic_Aspect_P2");
                    Set_HP2_DegradationSprite("Health_Clinic_Degradation_P2");
                    break;
                case Construction.HealthHospital:
                    Set_HP1_ConstructionSprite("Health_Hospital_Construction_P1");
                    Set_HP1_AspectSprite("Health_Hospital_Aspect_P1");
                    Set_HP1_DegradationSprite("Health_Hospital_Degradation_P1");

                    Set_HP2_ConstructionSprite("Health_Hospital_Construction_P2");
                    Set_HP2_AspectSprite("Health_Hospital_Aspect_P2");
                    HP2_Aspect_ExtraAnimationSpeed = 36;//in this order! (before setting the sprite)
                    Set_HP2_AspectExtraAnimationSprite("Health_Hospital_AspectExtraAnimation_P2");
                    Set_HP2_DegradationSprite("Health_Hospital_Degradation_P2");
                    break;
                case Construction.HealthLaboratory:
                    break;
                case Construction.EnvironmentNursery:
                    Set_HP2_ConstructionSprite("Environment_Nursery_Construction_P2");
                    Set_HP2_AspectSprite("Environment_Nursery_Aspect_P2");
                    Set_HP2_DegradationSprite("Environment_Nursery_Degradation_P2");
                    break;
                case Construction.EnvironmentWaterPurification:
                    Set_HP2_ConstructionSprite("Environment_WaterPurificationCenter_Construction_P2");
                    Set_HP2_AspectSprite("Environment_WaterPurificationCenter_Aspect_P2");
                    Set_HP2_DegradationSprite("Environment_WaterPurificationCenter_Degradation_P2");
                    break;
                case Construction.EnvironmentRecycling:
                    Set_HP2_ConstructionSprite("Environment_RecyclingCenter_Construction_P2");
                    HP2_Aspect_AnimationSpeed = 9; //in this order! (before setting the sprite)
                    Set_HP2_AspectSprite("Environment_RecyclingCenter_Aspect_P2");
                    Set_HP2_DegradationSprite("Environment_RecyclingCenter_Degradation_P2");
                    break;
                case Construction.EducationSchool:
                    Set_HP1_ConstructionSprite("Education_School_Construction_P1");
                    Set_HP1_AspectSprite("Education_School_Aspect_P1");
                    Set_HP1_DegradationSprite("Education_School_Degradation_P1");

                    Set_HP2_ConstructionSprite("Education_School_Construction_P2");
                    Set_HP2_AspectSprite("Education_School_Aspect_P2");
                    Set_HP2_DegradationSprite("Education_School_Degradation_P2");
                    break;
                case Construction.EducationHighSchool:
                    Set_HP1_ConstructionSprite("Education_HighSchool_Construction_P1");
                    Set_HP1_AspectSprite("Education_HighSchool_Aspect_P1");
                    Set_HP1_DegradationSprite("Education_HighSchool_Degradation_P1");

                    Set_HP2_ConstructionSprite("Education_HighSchool_Construction_P2");
                    Set_HP2_AspectSprite("Education_HighSchool_Aspect_P2");
                    Set_HP2_DegradationSprite("Education_HighSchool_Degradation_P2");
                    break;
                case Construction.EducationUniversity:
                    Set_HP1_ConstructionSprite("Education_University_Construction_P1");
                    Set_HP1_AspectSprite("Education_University_Aspect_P1");
                    Set_HP1_DegradationSprite("Education_University_Degradation_P1");

                    Set_HP2_ConstructionSprite("Education_University_Construction_P2");
                    Set_HP2_AspectSprite("Education_University_Aspect_P2");
                    Set_HP2_DegradationSprite("Education_University_Degradation_P2");
                    break;
                case Construction.EducationResearch:
                    break;
                case Construction.EconomyFactory:
                    Set_HP1_ConstructionSprite("Economy_Factory_Construction_P1");
                    Set_HP1_AspectSprite("Economy_Factory_Aspect_P1");
                    Set_HP1_DegradationSprite("Economy_Factory_Degradation_P1");

                    Set_HP2_ConstructionSprite("Economy_Factory_Construction_P2");
                    Set_HP2_AspectSprite("Economy_Factory_Aspect_P2");
                    Set_HP2_DegradationSprite("Economy_Factory_Degradation_P2");
                    break;
                case Construction.EconomyMine:
                    Set_HP1_ConstructionSprite("Economy_Mine_Construction_P1");
                    HP1_Aspect_AnimationSpeed = 10; //in this order! (before setting the sprite)
                    Set_HP1_AspectSprite("Economy_Mine_Aspect_P1");
                    Set_HP1_DegradationSprite("Economy_Mine_Degradation_P1");

                    Set_HP2_ConstructionSprite("Economy_Mine_Construction_P2");
                    Set_HP2_AspectSprite("Economy_Mine_Aspect_P2");
                    HP2_Aspect_ExtraAnimationSpeed = 25; //in this order! (before setting the sprite)
                    Set_HP2_AspectExtraAnimationSprite("Economy_Mine_AspectExtraAnimation_P2");
                    Set_HP2_DegradationSprite("Economy_Mine_Degradation_P2");
                    break;
                case Construction.EconomyOilWell:
                    Set_HP2_ConstructionSprite("Economy_OilProbe_Construction_P2");
                    HP2_Aspect_AnimationSpeed = 15; //in this order! (before setting the sprite)
                    Set_HP2_AspectSprite("Economy_OilProbe_Aspect_P2");
                    Set_HP2_DegradationSprite("Economy_OilProbe_Degradation_P2");
                    break;
                case Construction.EconomySawMill:
                    Set_HP1_ConstructionSprite("Economy_SawMill_Construction_P1");
                    HP1_Aspect_AnimationSpeed = 25; //in this order! (before setting the sprite)
                    Set_HP1_AspectSprite("Economy_SawMill_Aspect_P1");
                    Set_HP1_DegradationSprite("Economy_SawMill_Degradation_P1");

                    Set_HP2_ConstructionSprite("Economy_SawMill_Construction_P2");
                    HP2_Aspect_AnimationSpeed = 25; //in this order! (before setting the sprite)
                    Set_HP2_AspectSprite("Economy_SawMill_Aspect_P2");
                    Set_HP2_DegradationSprite("Economy_SawMill_Degradation_P2");
                    break;
                case Construction.PopulationVillage:
                    Set_HP1_ConstructionSprite("Population_Village_Construction_P1");
                    Set_HP1_AspectSprite("Population_Village_Aspect_P1");

                    Set_HP2_ConstructionSprite("Population_Village_Construction_P2");
                    Set_HP2_AspectSprite("Population_Village_Aspect_P2");
                    break;
                case Construction.PopulationTown:
                    Set_HP1_ConstructionSprite("Population_Town_Construction_P1");
                    Set_HP1_AspectSprite("Population_Town_Aspect_P1");

                    Set_HP2_ConstructionSprite("Population_Town_Construction_P2");
                    Set_HP2_AspectSprite("Population_Town_Aspect_P2");
                    break;
                case Construction.PopulationCity:
                    Set_HP1_ConstructionSprite("Population_City_Construction_P1");
                    Set_HP1_AspectSprite("Population_City_Aspect_P1");

                    Set_HP2_ConstructionSprite("Population_City_Construction_P2");
                    Set_HP2_AspectSprite("Population_City_Aspect_P2");
                    break;
                case Construction.PopulationMetropolis:
                    Set_HP1_ConstructionSprite("Population_Metropolis_Construction_P1");
                    Set_HP1_AspectSprite("Population_Metropolis_Aspect_P1");

                    Set_HP2_ConstructionSprite("Population_Metropolis_Construction_P2");
                    Set_HP2_AspectSprite("Population_Metropolis_Aspect_P2");
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region ChangeGraphics

        public void ChangeGraphics()
        {
            int CurrentYear = ((GameManager)Game.Services.GetService(typeof(GameManager))).GetCurrentYear();
            int CurrentHP;

            if (CurrentYear <= (int)HistoricalPeriod.HP1_LastYear)
                CurrentHP = 1;
            else
                CurrentHP = 2;

            Hide();
            //Cases for nonPopulation constructions:
            //1 - not upgraded and
            //      1.1 constructed in HP1 and currently in HP1
            //      1.2 constructed in HP1 and currently in HP2
            //      1.3 constructed in HP2 and currently in HP1 (not shown)
            //      1.4 constructed in HP2 and currently in HP2
            //2 - upgraded and
            //      2.1 currently in HP1
            //      2.2 currently in HP2, but before the upgradeYear
            //      2.3 currently in HP2, but in or after the upgradeYear
            
            if (((Upgraded == false) && (constructionYear <= (int)HistoricalPeriod.HP1_LastYear) && (CurrentHP == 1)) ||
                ((Upgraded == false) && (constructionYear >= (int)HistoricalPeriod.HP2_FirstYear)) ||
                (Upgraded && (CurrentHP == 1)))
            //not upgraded and 1.1 - constructed in HP1 and currently in HP1  or
            //not upgraded and 1.3+1.4 - constructed in HP2 (if currently in HP1 -> not shown, if HP2->shown)
            //upgraded and 2.1 - currently in HP1
            {
                if (CurrentYear < ConstructionYear) //not shown
                {
                    ConstructionStage(0, 0);
                }

                if (isPopulationConstruction_UpgradedLevel)
                {
                    if ((CurrentYear >= ConstructionYear) && //construction stage 2
                        (CurrentYear < ConstructionYear + ConstructingPeriod))
                        ConstructionStage(2, CurrentHP);
                }
                else
                {
                    if ((CurrentYear >= ConstructionYear) && //construction stage 1
                    (CurrentYear < (ConstructionYear + ConstructingPeriod / 2)))
                    {
                        ConstructionStage(1, CurrentHP);
                    }
                    if ((CurrentYear >= (ConstructionYear + ConstructingPeriod / 2)) && //construction stage 2
                        (CurrentYear < ConstructionYear + ConstructingPeriod))
                    {
                        ConstructionStage(2, CurrentHP);
                    }
                }

                if ((CurrentYear >= (ConstructionYear + ConstructingPeriod)) &&
                    (CurrentYear <= (ConstructionYear + ConstructionLifetime - DegradationPeriod)))//aspect
                    ConstructionStage(3, CurrentHP);
                if ((CurrentYear > (ConstructionYear + ConstructionLifetime - DegradationPeriod)) &&
                     (CurrentYear <= (ConstructionYear + ConstructionLifetime)))//degradation
                {
                    if (degradationPeriod == 0)
                    {
                        ConstructionStage(3, CurrentHP);
                    }
                    else
                        ConstructionStage(4, CurrentHP);
                }
            }

            if ((Upgraded == false && (constructionYear <= (int)HistoricalPeriod.HP1_LastYear) && (CurrentHP == 2)) ||
                (Upgraded && (CurrentHP == 2) && (CurrentYear < upgradeYear)))
            //not upgraded and 1.2 - constructed in HP1 and currently in HP2   or
            //upgraded and 2.2 - currently in HP2, but before the upgradeYear
            {
                if (CurrentYear < ConstructionYear) //not shown
                    ConstructionStage(0, 0);

                if (isPopulationConstruction_UpgradedLevel)
                {
                    if ((CurrentYear >= ConstructionYear) && //construction stage 2
                       (CurrentYear < ConstructionYear + ConstructingPeriod))
                        ConstructionStage(2, 1);
                }
                else
                {
                    if ((CurrentYear >= ConstructionYear) && //construction stage 1
                   (CurrentYear < (ConstructionYear + ConstructingPeriod / 2)))
                        ConstructionStage(1, 1);
                    if ((CurrentYear >= (ConstructionYear + ConstructingPeriod / 2)) && //construction stage 2
                        (CurrentYear < ConstructionYear + ConstructingPeriod))
                        ConstructionStage(2, 1);
                }

                if ((CurrentYear >= (ConstructionYear + ConstructingPeriod)) &&
                    (CurrentYear <= (ConstructionYear + ConstructionLifetime - DegradationPeriod)))//aspect
                    ConstructionStage(3, 1);
                if ((CurrentYear > (ConstructionYear + ConstructionLifetime - DegradationPeriod)) &&
                     (CurrentYear <= (ConstructionYear + ConstructionLifetime)))//degradation
                {
                    if (degradationPeriod == 0)
                    {
                        ConstructionStage(3, 1);
                    }
                    else
                        ConstructionStage(4, 1);
                }
                    
            }

            if (Upgraded && (CurrentHP == 2) && (CurrentYear >= upgradeYear))
            // upgraded and 2.3 - currently in HP2, but in or after the upgradeYear
            {
                if (isPopulationConstruction_UpgradedLevel)
                {
                    if ((CurrentYear >= ConstructionYear) && //construction stage 2
                        (CurrentYear < ConstructionYear + ConstructingPeriod))
                        ConstructionStage(2, 2);
                }
                //can only be in the aspect or degradation stage
                if ((CurrentYear >= (ConstructionYear + ConstructingPeriod)) &&
                    (CurrentYear <= (ConstructionYear + ConstructionLifetime - DegradationPeriod)))//aspect
                    ConstructionStage(3, 2);
                if ((CurrentYear > (ConstructionYear + ConstructionLifetime - DegradationPeriod)) &&
                     (CurrentYear <= (ConstructionYear + ConstructionLifetime)))//degradation
                {
                    if (degradationPeriod == 0)
                    {
                        ConstructionStage(3, 2);
                    }
                    else
                        ConstructionStage(4, 2);                    
                }
            }
        }
        #endregion

        #region Show/Hide
        public void Show()
        {
            this.Enabled = true;
        }

        public void Hide()
        {
            for (int i = 0; i < spriteList.Count; i++)
                spriteList[i].Visible = false;
            this.Enabled = false;
        }
        #endregion

        #endregion

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion
    }
}
