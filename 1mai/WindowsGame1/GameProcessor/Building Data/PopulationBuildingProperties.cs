using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class PopulationBuildingProperties
    {
        public int MaxCapacity(Construction construction, int upgradeYear, int currentYear)
        {
            if (upgradeYear <= currentYear)
            {
                switch (construction)
                {
                    case Construction.PopulationVillage:
                        return 2500;
                    case Construction.PopulationTown:
                        return 10000;
                    case Construction.PopulationCity:
                        return 100000;
                    case Construction.PopulationMetropolis:
                        return 1000000;
                }
            }
            else
            {
                switch (construction)
                {
                    case Construction.PopulationVillage:
                        return 3000;
                    case Construction.PopulationTown:
                        return 15000;
                    case Construction.PopulationCity:
                        return 150000;
                    case Construction.PopulationMetropolis:
                        return 1500000;
                }
            }
            return 0;
        }
    }
}
