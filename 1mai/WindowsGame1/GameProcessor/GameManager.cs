using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Operation_Cronos.Sound;
using Operation_Cronos.Profiles;

namespace Operation_Cronos.GameProcessor {
    public partial class GameManager : GameComponent
    {
        public const int MaximumTimeRange = 510;
        public const int MinimumYear = 1500;
        private TimeManager timeManager;
        private PopulationManager popManager;
        private ResearchHistory researchHistory;
        private TimeMachine timeMachine;
        BuildingsBaseProperties properties;
        BuildingsBaseBonuses bonuses;
        Modifier modifier;

        #region Properties
        private GameInterface GUI
        {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }

        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        private DisplayManager DisplayManager
        {
            get { return (DisplayManager)Game.Services.GetService(typeof(DisplayManager)); }
        }

        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }

        private ProfileManager ProfileManager
        {
            get { return (ProfileManager)Game.Services.GetService(typeof(ProfileManager)); }
        }

        private GameInterface GameInterface
        {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }

        public int CurrentYear
        {
            get { return GetCurrentYear(); }
        }

        public int StartingTravelYear
        {
            get { return timeMachine.StartYear; }
        }

        public int EndTravelYear
        {
            get { return timeMachine.EndYear; }
        }

        public BuildingsBaseProperties BuildingProperties
        {
            get { return properties; }
        }

        public BuildingsBaseBonuses BuildingBonuses
        {
            get { return bonuses; }
        }

        public Modifier Modifier
        {
            get { return modifier; }
        }

        public List<Research> ResearchList
        {
            get
            {
                return researchHistory.ResearchList;
            }
        }
        #endregion

        public GameManager(Game game)
            : base(game)
        {
            timeManager = new TimeManager(game);
            Game.Services.AddService(typeof(GameManager), this);
            Game.Components.Add(this);
            timeMachine = new TimeMachine(game);
            //money
            AllocateMemoryForMoney();

            //properties, bonuses etc.
            properties = new BuildingsBaseProperties();
            bonuses = new BuildingsBaseBonuses();
            modifier = new Modifier(game);
        }

        public void UpdateYear(int year)
        {
            timeManager.Year = year;
            GUI.ClearSelectedBuildingIcon();
            if (GameMap.ChangesMade)
            {
                UpdatePopulation();
                UpdateMonetaryHistory();
            }
            
            GUI.SetPopulation((int)popManager.GetPopulation(CurrentYear));
            GUI.SetMoney(GetMoney(CurrentYear));
            GUI.SetParameters(popManager.GetConsumptionCoverage(CurrentYear));

            GameMap.ResetYearlyParameters();
            GameInterface.SetOverpopulationStatus(popManager.Overpopulation(CurrentYear));
            Debug.AddToLog(popManager);

            SoundManager.StopBackgroundSong();
            SoundManager.StartBackgroundSong();
        }

        public int GetCurrentYear()
        {
            return timeManager.Year;
        }

        public MilleniumGoalsSet GetConsumptionCoverage(int year)
        {
            return popManager.GetConsumptionCoverage(year);
        }       

        public List<Research> GetResearchList(ConstructionType type)
        {
            return researchHistory.GetResearchList(type);
        }

        public void CompleteResearch(Research r)
        {
            researchHistory.CompleteResearch(r,CurrentYear);
            SpendMoney(CurrentYear, r.Cost);
            GUI.SetMoney(GetMoney(CurrentYear));
            GameMap.ChangesMade = true;
        }

        private void UpdatePopulation()
        {
            for (int year = StartingTravelYear; year < EndTravelYear; year++)
            {
                popManager.UpdatePopulation(year);
            }
        }

        public void LoadHistory()
        {
            List<Research> researchList = ProfileManager.LoadResearchList();
            researchHistory = new ResearchHistory(researchList);

            //research
            //researchHistory = new ResearchHistory();
            //MilleniumGoalsSet set = new MilleniumGoalsSet(1, 1, 1, 1, 1, 1);
            //set.Food = 1.1f;
            //Research r = new Research(set, 5000, ConstructionType.Food);
            //r.Name = "Improved Farming";
            //r.Description = "Increase food production by 10%";
            //r.YearAvailable = 1850;
            //researchHistory.AddResearchToList(r);

            //set.Food = 1.05f;
            //set.Health = 0.8f;
            //r = new Research(set, 10000, ConstructionType.Food);
            //r.Name = "Genetically altered crops";
            //r.Description = "Increase food production by 5%\nDecrease health by 10%";
            //r.YearAvailable = 1980;
            //researchHistory.AddResearchToList(r);

            //set.Food = 1.5f;
            //set.Education = 0.5f;
            //r = new Research(set, 10000, ConstructionType.Food);
            //r.Name = "Rednecks immigration";
            //r.Description = "Increase food production by 50%\nDecrease education by 50%";
            //r.YearAvailable = 1950;
            //r.CompleteResearch(1980);
            //researchHistory.AddResearchToList(r);
            //
            //
            //population
            popManager = new PopulationManager(this.Game);
            popManager.CreateDefaultPopulation();
            for (int year = StartingTravelYear; year < EndTravelYear; year++)
            {
                popManager.UpdatePopulation(year);
            }
            //
            timeMachine.UpgradeTimeMachine();
            timeMachine.UpgradeTimeMachine();
            timeMachine.UpgradeTimeMachine();
        }
    }
}