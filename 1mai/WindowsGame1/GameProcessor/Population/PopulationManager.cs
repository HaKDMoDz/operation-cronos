using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.GameProcessor
{
    public class PopulationManager:GameComponent
    {
        public const int StartingPopulation = 200;
        MilleniumGoalsSet baseConsumptionPerPerson;
        List<float> populationLevels;

        #region Properties
        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        
        public MilleniumGoalsSet BaseConsumptionPerPerson
        {
            get { return baseConsumptionPerPerson; }
            set { baseConsumptionPerPerson = value; }
        }
        #endregion

        public PopulationManager(Game game):base(game)
        {
            baseConsumptionPerPerson = new MilleniumGoalsSet(1f,1f,1f,1f,1f,1f);
            populationLevels = new List<float>(GameManager.MaximumTimeRange);
        }

        public void CreateDefaultPopulation()
        {
            for (int i = 0; i < GameManager.MaximumTimeRange; i++)
            {
                populationLevels.Add(StartingPopulation);
            }
        }

        public void UpdatePopulation(int year)
        {
            populationLevels[year - GameManager.MinimumYear] = populationLevels[year - GameManager.MinimumYear - 1] +
                populationLevels[year - GameManager.MinimumYear - 1] * 0.05f * (GetGrowth(year) - 0.5f);
        }

        public float GetPopulation(int year)
        {
            return populationLevels[year - GameManager.MinimumYear - 1];
        }

        public MilleniumGoalsSet GetConsumptionCoverage(int year)
        {
            MilleniumGoalsSet production = GetBuildingProduction(year);
            MilleniumGoalsSet consumption = baseConsumptionPerPerson * GameManager.Modifier.Consumption(year) * (GetPopulation(year) + Overpopulation(year));
            MilleniumGoalsSet coverage = new MilleniumGoalsSet();
            coverage.Economy = production.Economy / consumption.Economy;
            coverage.Education = production.Education / consumption.Education;
            coverage.Energy = production.Energy / consumption.Energy;
            if (consumption.Environment == 0)
            {
                coverage.Environment = 1;
            }
            else
            {
                coverage.Environment = production.Environment / consumption.Environment;
            }
            coverage.Food = production.Food / consumption.Food;
            coverage.Health = production.Health / consumption.Health;
            return coverage;
        }

        public int Overpopulation(int year)
        {
            PopulationBuildingProperties properties = new PopulationBuildingProperties();
            int normalPop = 0;
            int capacity =0 ;
            foreach (Slot slot in GameMap.SlotList)
            {
                if (slot.ConstructionType == ConstructionType.Population)
                {
                    if (slot.GetReservation(year) != null)
                    {
                        capacity = properties.MaxCapacity(slot.GetReservation(year).ConstructionName,
                            slot.GetReservation(year).UpgradeYear, year);
                        normalPop += capacity;
                    }
                }
            }
            if (GetPopulation(year) - capacity > 0)
            {
                return (int)GetPopulation(year) - capacity;
            }
            else
            {
                return 0;
            }
        }

        public MilleniumGoalsSet GetBuildingProduction(int year)
        {
            MilleniumGoalsSet total = new MilleniumGoalsSet();
            foreach (Reservation res in GameMap.GetReservationsForYear(year))
            {
                int yearNew = 0;

                if (res.StartingYear < (int)HistoricalPeriod.HP2_FirstYear)
                {

                    if (res.UpgradeYear >= (int)HistoricalPeriod.HP2_FirstYear)
                    {
                        if (GameManager.CurrentYear < res.UpgradeYear)
                            yearNew = (int)HistoricalPeriod.HP1_FirstYear;
                        else
                            yearNew = (int)HistoricalPeriod.HP2_FirstYear;
                    }
                    else
                        yearNew = (int)HistoricalPeriod.HP1_FirstYear;
                }
                else
                    yearNew = (int)HistoricalPeriod.HP2_FirstYear;

                if (res.Status(year) == ConstructionStatus.InProduction)
                {
                    total += GameManager.BuildingBonuses.BuildingMilleniumGoalSet(res.ConstructionName, yearNew) * GameManager.Modifier.Production(year);
                }
            }
            total += GameMap.MapProduction;
            return total;
        }

        public float GetGrowth(int year)
        {
            if (year < (int)HistoricalPeriod.HP2_FirstYear)
                return GetConsumptionCoverage(year).MeanValueHP1;
            else
                return GetConsumptionCoverage(year).MeanValueHP2;
        }

        public override string ToString()
        {
            return "Pop: " + populationLevels[GameManager.CurrentYear - GameManager.MinimumYear - 1] +
                " overpop: "+Overpopulation(GameManager.CurrentYear)+
                "\nProd: " + GetBuildingProduction(GameManager.CurrentYear) +
                "\nCoverage: " + GetConsumptionCoverage(GameManager.CurrentYear) +
                "\nGrowth: " + GetGrowth(GameManager.CurrentYear);
        }
    }
}
