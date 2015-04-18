using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class BuildingsBaseBonuses
    {
        public BuildingsBaseBonuses()
        {
        }

        public MilleniumGoalsSet BuildingMilleniumGoalSet(Construction building, int year)
        {
            if (year < (int)HistoricalPeriod.HP2_FirstYear)
                return BuildingMilleniumGoalSet_HP1(building);
            else
                return BuildingMilleniumGoalSet_HP2(building);
        }

        private MilleniumGoalsSet BuildingMilleniumGoalSet_HP1(Construction building)
        {
            MilleniumGoalsSet GoalSet;
            switch (building)
            {
                case Construction.EnergySolar:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnergySolid:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnergyWind:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnergyGeo:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnergyNuclear:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.FoodCropFarm:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-250, 0, 0, 0, 800, 0);
                    break;
                case Construction.FoodAnimalFarm:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 0, 0, 700, 0);
                    break;
                case Construction.FoodOrchard:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-50, 0, 0, 0, 250, 0);
                    break;
                case Construction.FoodFarmedFisherie:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-150, 0, 0, 0, 650, 0);
                    break;
                case Construction.FoodWildFisherie:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.HealthClinic:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-150, 400, 0, 0, -50, 0);
                    break;
                case Construction.HealthHospital:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-400, 1400, 0, 0, -100, 0);
                    break;
                case Construction.HealthLaboratory:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnvironmentNursery:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnvironmentWaterPurification:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnvironmentRecycling:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EducationSchool:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 500, 0, -50, 0);
                    break;
                case Construction.EducationHighSchool:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-200, 0, 1200, 0, -100, 0);
                    break;
                case Construction.EducationUniversity:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-400, 0, 2800, 0, -200, 0);
                    break;
                case Construction.EducationResearch:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EconomyMine:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(1800, -200, 0, 0, -250, 0);
                    break;
                case Construction.EconomyFactory:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(1500, -150, 0, 0, -250, 0);
                    break;
                case Construction.EconomyOilWell:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EconomySawMill:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(900, -150, 0, 0, -150, 0);
                    break;
                case Construction.PopulationVillage:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.PopulationTown:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.PopulationCity:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.PopulationMetropolis:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                default:
                    GoalSet = new MilleniumGoalsSet();
                    break;
            }

            return GoalSet;
        }

        private MilleniumGoalsSet BuildingMilleniumGoalSet_HP2(Construction building)
        {
            MilleniumGoalsSet GoalSet;

            switch (building)
            {
                case Construction.EnergySolar:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-250, 0, 0, 500, -50, 0);
                    break;
                case Construction.EnergySolid:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-250, -200, 0, 1500, -150, -500);
                    break;
                case Construction.EnergyWind:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-250, 0, 0, 300, -50, 0);
                    break;
                case Construction.EnergyGeo:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 0, 1600, -100, 0);
                    break;
                case Construction.EnergyNuclear:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-350, 0, 0, 4000, -100, -150);
                    break;
                case Construction.FoodCropFarm:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-200, 0, 0, -150, 1200, -250);
                    break;
                case Construction.FoodAnimalFarm:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 0, -200, 1100, -100);
                    break;
                case Construction.FoodOrchard:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 0, -50, 500, 50);
                    break;
                case Construction.FoodFarmedFisherie:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-200, 0, 0, -150, 1000, 0);
                    break;
                case Construction.FoodWildFisherie:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.HealthClinic:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 600, 0, -50, -50, 0);
                    break;
                case Construction.HealthHospital:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-300, 1600, 0, -200, -200, 0);
                    break;
                case Construction.HealthLaboratory:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EnvironmentNursery:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 0, -50, -50, 500);
                    break;
                case Construction.EnvironmentWaterPurification:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-200, 0, 0, -350, -50, 1500);
                    break;
                case Construction.EnvironmentRecycling:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-350, 0, 0, -150, -100, 1500);
                    break;
                case Construction.EducationSchool:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-100, 0, 800, -50, -50, 0);
                    break;
                case Construction.EducationHighSchool:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-200, 0, 1500, -100, -100, 0);
                    break;
                case Construction.EducationUniversity:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(-400, 0, 3500, -200, -200, 0);
                    break;
                case Construction.EducationResearch:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.EconomyMine:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(2500, -100, 0, -250, -100, -350);
                    break;
                case Construction.EconomyFactory:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(2000, -50, 0, -150, -200, -200);
                    break;
                case Construction.EconomyOilWell:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(3000, -100, 0, -200, -50, -450);
                    break;
                case Construction.EconomySawMill:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(1400, -50, 0, -200, -100, -350);
                    break;
                case Construction.PopulationVillage:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.PopulationTown:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.PopulationCity:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                case Construction.PopulationMetropolis:
                    //Economy, Health, Education, Energy, Food, Environment
                    GoalSet = new MilleniumGoalsSet(0, 0, 0, 0, 0, 0);
                    break;
                default:
                    GoalSet = new MilleniumGoalsSet();
                    break;
            }

            return GoalSet;
        }

    }
}
