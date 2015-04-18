using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Operation_Cronos.Display {
    public enum HistoricalPeriod
    {
        HP1_FirstYear = 1800,
        HP1_LastYear = 1900,
        HP2_FirstYear = 1901,
        HP2_LastYear = 2010
    }
    public enum ConstructionType {
        None,
        Economy,
        Education,
        Environment,
        Food,
        Health,
        Energy,
        Population
    }
    public enum Construction {
        //---Energy
        [DescriptionAttribute("Solar power plant")]
        EnergySolar,
        [DescriptionAttribute("Solid fuel power plant")]
        EnergySolid,
        [DescriptionAttribute("Wind power plant")]
        EnergyWind,
        [DescriptionAttribute("Geothermal power plant")]
        EnergyGeo,
        [DescriptionAttribute("Nuclear power plant")]
        EnergyNuclear,
        //---Food
        [DescriptionAttribute("Crop farm")]
        FoodCropFarm,
        [DescriptionAttribute("Animal farm")]
        FoodAnimalFarm,
        [DescriptionAttribute("Orchard")]
        FoodOrchard,
        [DescriptionAttribute("Farmed fisherie")]
        FoodFarmedFisherie,
        [DescriptionAttribute("Wild fisherie")]
        FoodWildFisherie,
        //---Health
        [DescriptionAttribute("Clinic")]
        HealthClinic,
        [DescriptionAttribute("Hospital")]
        HealthHospital,
        [DescriptionAttribute("Laboratory")]
        HealthLaboratory,
        //---Environment
        [DescriptionAttribute("Nursery")]
        EnvironmentNursery,
        [DescriptionAttribute("Water purification")]
        EnvironmentWaterPurification,
        [DescriptionAttribute("Recycling center")]
        EnvironmentRecycling,
        //---Education
        [DescriptionAttribute("School")]
        EducationSchool,
        [DescriptionAttribute("Highschool")]
        EducationHighSchool,
        [DescriptionAttribute("University")]
        EducationUniversity,
        [DescriptionAttribute("Research center")]
        EducationResearch,
        //---Economy
        [DescriptionAttribute("Mine")]
        EconomyMine,
        [DescriptionAttribute("Factory")]
        EconomyFactory,
        [DescriptionAttribute("Oil well")]
        EconomyOilWell,
        [DescriptionAttribute("Saw mill")]
		EconomySawMill,
        //---Population
        [DescriptionAttribute("Village")]
        PopulationVillage,
        [DescriptionAttribute("Town")]
        PopulationTown,
        [DescriptionAttribute("City")]
        PopulationCity,
        [DescriptionAttribute("Metropolis")]
        PopulationMetropolis,
    }
}
