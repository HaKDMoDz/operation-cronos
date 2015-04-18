using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class BuildingsBaseProperties
    {
        //public BuildingProperties this[Construction building]
        //{
        //    //!! Destruction Period always = 1
        //    //lifetime, constructing period, destruction period, cost
        //    get { return new BuildingProperties(50, 4, 1, 1000); }
        //    set { }
        //}

        public BuildingProperties BuildingBaseProperties(Construction building, int ConstructionYear, int UpgradeYear, int YearOrInterest)
        {
            if ((ConstructionYear < (int)HistoricalPeriod.HP2_FirstYear) &&//is constructed in HP1
                (YearOrInterest < (int)HistoricalPeriod.HP2_FirstYear)) //year of interest in HP1
            {
                return BuildingBaseProperties(building, 1);
            }
            if ((ConstructionYear < (int)HistoricalPeriod.HP2_FirstYear) &&//is constructed in HP1
                (YearOrInterest >= (int)HistoricalPeriod.HP2_FirstYear)) //year of interest in HP2
            {
                if (UpgradeYear >= (int)HistoricalPeriod.HP2_FirstYear) //is upgraded
                    return BuildingBaseProperties(building, 2);
                else  //not upgraded
                    return BuildingBaseProperties(building, 1);
            }
            if ((ConstructionYear >= (int)HistoricalPeriod.HP2_FirstYear) &&//is constructed in HP2
                (YearOrInterest >= (int)HistoricalPeriod.HP2_FirstYear)) //year of interest in HP2
            {
                return BuildingBaseProperties(building, 2);
            }
            return null;
        }

        public BuildingsBaseProperties()
        {
        }

        private BuildingProperties BuildingBaseProperties(Construction building, int HP)
        {
            BuildingProperties bp = new BuildingProperties();
            switch (HP)
            {
                case 1: //HP1
                   
                    switch (building)
                    {
                        case Construction.EnergySolar:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnergySolid:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);//al 3-lea param = 1 (distruction period)
                            break;
                        case Construction.EnergyWind:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnergyGeo:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnergyNuclear:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.FoodCropFarm:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(40, 4, 1, 850);
                            break;
                        case Construction.FoodAnimalFarm:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(40, 4, 1, 900);
                            break;
                        case Construction.FoodOrchard:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(30, 4, 1, 600);
                            break;
                        case Construction.FoodFarmedFisherie:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(30, 4, 1, 800);
                            break;
                        case Construction.FoodWildFisherie:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.HealthClinic:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(30, 4, 1, 700);
                            break;
                        case Construction.HealthHospital:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(45, 6, 1, 1200);
                            break;
                        case Construction.HealthLaboratory:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnvironmentNursery:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnvironmentWaterPurification:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnvironmentRecycling:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EducationSchool:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(30, 4, 1, 650);
                            break;
                        case Construction.EducationHighSchool:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(45, 4, 1, 1200);
                            break;
                        case Construction.EducationUniversity:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(65, 6, 1, 2500);
                            break;
                        case Construction.EducationResearch:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EconomyMine:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(45, 6, 1, 1650);
                            break;
                        case Construction.EconomyFactory:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(70, 6, 1, 1400);
                            break;
                        case Construction.EconomyOilWell:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EconomySawMill:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(30, 4, 1, 900);
                            break;
                        case Construction.PopulationVillage:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(500, 6, 1, 3000);
                            break;
                        case Construction.PopulationTown:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(500, 8, 1, 15000);
                            break;
                        case Construction.PopulationCity:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(500, 10, 1, 150000);
                            break;
                        case Construction.PopulationMetropolis:
                            //LifeTime, ConstructionPeriod, DestructionPeriod, Price
                            bp = new BuildingProperties(500, 14, 1, 1000000);
                            break;
                        default:
                            bp = new BuildingProperties(0,0,0,0);
                            break;
                    }
                    break;
                case 2: //HP2
                    switch (building)
                    {
                        case Construction.EnergySolar:
                            bp = new BuildingProperties(50, 2, 1, 1800);
                            break;
                        case Construction.EnergySolid:
                            bp = new BuildingProperties(60, 4, 1, 2000);//al 3-lea param = 1 (distruction period)
                            break;
                        case Construction.EnergyWind:
                            bp = new BuildingProperties(50, 2, 1, 1600);
                            break;
                        case Construction.EnergyGeo:
                            bp = new BuildingProperties(60, 4, 1, 3000);
                            break;
                        case Construction.EnergyNuclear:
                            bp = new BuildingProperties(70, 6, 1, 5000);
                            break;
                        case Construction.FoodCropFarm:
                            bp = new BuildingProperties(50, 2, 1, 900);
                            break;
                        case Construction.FoodAnimalFarm:
                            bp = new BuildingProperties(50, 2, 1, 1000);
                            break;
                        case Construction.FoodOrchard:
                            bp = new BuildingProperties(50, 2, 1, 700);
                            break;
                        case Construction.FoodFarmedFisherie:
                            bp = new BuildingProperties(50, 2, 1, 950);
                            break;
                        case Construction.FoodWildFisherie:
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.HealthClinic:
                            bp = new BuildingProperties(50, 2, 1, 700);
                            break;
                        case Construction.HealthHospital:
                            bp = new BuildingProperties(70, 4, 1, 1200);
                            break;
                        case Construction.HealthLaboratory:
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EnvironmentNursery:
                            bp = new BuildingProperties(50, 2, 1, 600);
                            break;
                        case Construction.EnvironmentWaterPurification:
                            bp = new BuildingProperties(50, 4, 1, 1200);
                            break;
                        case Construction.EnvironmentRecycling:
                            bp = new BuildingProperties(50, 4, 1, 1200);
                            break;
                        case Construction.EducationSchool:
                            bp = new BuildingProperties(50, 2, 1, 900);
                            break;
                        case Construction.EducationHighSchool:
                            bp = new BuildingProperties(60, 2, 1, 1400);
                            break;
                        case Construction.EducationUniversity:
                            bp = new BuildingProperties(75, 4, 1, 3000);
                            break;
                        case Construction.EducationResearch:
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                        case Construction.EconomyMine:
                            bp = new BuildingProperties(50, 4, 1, 2000);
                            break;
                        case Construction.EconomyFactory:
                            bp = new BuildingProperties(80, 4, 1, 1700);
                            break;
                        case Construction.EconomyOilWell:
                            bp = new BuildingProperties(70, 2, 1, 2500);
                            break;
                        case Construction.EconomySawMill:
                            bp = new BuildingProperties(40, 2, 1, 1000);
                            break;
                        case Construction.PopulationVillage:
                            bp = new BuildingProperties(500, 4, 1, 3500);
                            break;
                        case Construction.PopulationTown:
                            bp = new BuildingProperties(500, 6, 1, 20000);
                            break;
                        case Construction.PopulationCity:
                            bp = new BuildingProperties(500, 8, 1, 200000);
                            break;
                        case Construction.PopulationMetropolis:
                            bp = new BuildingProperties(500, 12, 1, 1500000);
                            break;
                        default:
                            bp = new BuildingProperties(0, 0, 0, 0);
                            break;
                    }
                    break;
            }
            return bp;
        }
    }
}
