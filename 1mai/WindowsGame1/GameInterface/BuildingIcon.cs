using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;
using System.Reflection;
using System.ComponentModel;

namespace Operation_Cronos.Display {
   
    public class BuildingIcon : Button, ISelectable {
        Sprite visual;
        Construction building;
        ConstructionType category;
        Boolean selected;
        string tooltipText;

        #region Properties
        public Construction BuildingName {
            get { return building; }
        }
        public ConstructionType BuildingType {
            get { return category; }
        }
        /// <summary>
        /// Gets the text to be displayed in the tooltip
        /// at MouseOver icon
        /// </summary>
        public string TooltipText
        {
            get
            {
                return tooltipText;
            }
        }

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }

        #endregion

        public BuildingIcon(Game game, ConstructionType category, Construction building)
            : base(game)
        {
            this.building = building;
            this.category = category;

            #region Choose Graphics
            switch (building)
            {
                case Construction.EnergySolar:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-energy-solar"));
                    AddChild(visual);
                    break;
                case Construction.EnergySolid:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-energy-oil"));
                    AddChild(visual);
                    break;
                case Construction.EnergyWind:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-energy-wind"));
                    AddChild(visual);
                    break;
                case Construction.EnergyGeo:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-energy-geo"));
                    AddChild(visual);
                    break;
                case Construction.EnergyNuclear:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-energy-nuclear"));
                    AddChild(visual);
                    break;
                case Construction.EducationSchool:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-education-school"));
                    AddChild(visual);
                    break;
                case Construction.EducationHighSchool:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-education-highschool"));
                    AddChild(visual);
                    break;
                case Construction.EducationUniversity:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-education-university"));
                    AddChild(visual);
                    break;
                case Construction.FoodAnimalFarm:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-food-animalFarm"));
                    AddChild(visual);
                    break;
                case Construction.FoodCropFarm:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-food-cropFarm"));
                    AddChild(visual);
                    break;
                case Construction.FoodOrchard:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-food-orchard"));
                    AddChild(visual);
                    break;
                case Construction.FoodFarmedFisherie:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-food-farmedFisherie"));
                    AddChild(visual);
                    break;
                case Construction.PopulationVillage:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-population-village"));
                    AddChild(visual);
                    break;
                case Construction.PopulationTown:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-population-town"));
                    AddChild(visual);
                    break;
                case Construction.PopulationCity:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-population-city"));
                    AddChild(visual);
                    break;
                case Construction.PopulationMetropolis:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-population-metropolis"));
                    AddChild(visual);
                    break;
                case Construction.HealthHospital:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-health-hospital"));
                    AddChild(visual);
                    break;
                case Construction.HealthClinic:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-health-clinic"));
                    AddChild(visual);
                    break;
                case Construction.EconomyFactory:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-economy-factory"));
                    AddChild(visual);
                    break;
                case Construction.EconomyOilWell:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-economy-oilWell"));
                    AddChild(visual);
                    break;
                case Construction.EconomyMine:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-economy-mine"));
                    AddChild(visual);
                    break;
                case Construction.EconomySawMill:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-economy-sawMill"));
                    AddChild(visual);
                    break;
                case Construction.EnvironmentNursery:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-environment-nursery"));
                    AddChild(visual);
                    break;
                case Construction.EnvironmentRecycling:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-environment-recycling"));
                    AddChild(visual);
                    break;
                case Construction.EnvironmentWaterPurification:
                    visual = new Sprite(game, GraphicsCollection.GetPack("icon-environment-waterPurification"));
                    AddChild(visual);
                    break;
                default:
                    break;
            }
            #endregion
            
            RefreshTooltip();
        }

        public void RefreshTooltip()
        {
            tooltipText = GetDescription(building);


            BuildingsBaseBonuses parameters = new BuildingsBaseBonuses();
            BuildingsBaseProperties properties = new BuildingsBaseProperties();
            PopulationBuildingProperties popProp = new PopulationBuildingProperties();

            float Economy = parameters.BuildingMilleniumGoalSet(building, GameManager.CurrentYear).Economy;
            float Health = parameters.BuildingMilleniumGoalSet(building, GameManager.CurrentYear).Health;
            float Education = parameters.BuildingMilleniumGoalSet(building, GameManager.CurrentYear).Education;
            float Energy = parameters.BuildingMilleniumGoalSet(building, GameManager.CurrentYear).Energy;
            float Food = parameters.BuildingMilleniumGoalSet(building, GameManager.CurrentYear).Food;
            float Environment = parameters.BuildingMilleniumGoalSet(building, GameManager.CurrentYear).Environment;

            string pozitiveParams = "";
            string negativeParams = "";

            if (Economy > 0)
                pozitiveParams += "\n Economy             +" + Economy.ToString();
            else if (Economy < 0)
                negativeParams += "\n Economy             -" + Economy.ToString();
            if (Health > 0)
                pozitiveParams += "\n Health                  +" + Health.ToString();
            else if (Health < 0)
                negativeParams += "\n Health                  -" + Health.ToString();
            if (Education > 0)
                pozitiveParams += "\n Education          +" + Education.ToString();
            else if (Education < 0)
                negativeParams += "\n Education          -" + Education.ToString();
            if (Energy > 0)
                pozitiveParams += "\n Energy                  +" + Energy.ToString();
            else if (Energy < 0)
                negativeParams += "\n Energy                  -" + Energy.ToString();
            if (Food > 0)
                pozitiveParams += "\n Food                      +" + Food.ToString();
            else if (Food < 0)
                negativeParams += "\n Food                      -" + Food.ToString();
            if (Environment > 0)
                pozitiveParams += "\n Environment    +" + Environment.ToString();
            else if (Environment < 0)
                negativeParams += "\n Environment    -" + Environment.ToString();
            

            int price = properties.BuildingBaseProperties(building, GameManager.CurrentYear, 0, GameManager.CurrentYear).Price;
            
            bool canAfford = true;
            try
            {
            canAfford = (GameManager.GetMoney(GameManager.CurrentYear) >= price);
            }
            catch(Exception ex){}
            if (!canAfford)
            {
                //can afford
                tooltipText += "\n  - CAN NOT AFFORD - ";
                tooltipText += pozitiveParams;
                tooltipText += negativeParams;
            }
            else //can not afford
            {
                tooltipText += "\n" + pozitiveParams;
                tooltipText += negativeParams;
            }

            tooltipText += "\nPrice: $" + price.ToString();
            if (building == Construction.PopulationCity || building == Construction.PopulationMetropolis ||
                 building == Construction.PopulationTown || building == Construction.PopulationVillage)
            {
                tooltipText += "\nNormal capacity:\n   " + popProp.MaxCapacity(building, GameManager.CurrentYear, GameManager.CurrentYear).ToString();
            }
        }

        public override void MouseOverAnimation() {
            if (!IsSelected)
                visual.FrameNumber = 1;
        }

        public override void MouseLeaveAnimation() {
            if (!IsSelected)
                visual.FrameNumber = 0;
        }

        /// <summary>
        /// Sets the Status, a text indicating what is currently loading
        /// </summary>
        private string GetDescription(Construction construction)
        {
            string text = "";
            FieldInfo fi = construction.GetType().GetField(construction.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                text = attributes[0].Description;

            return text;
        }

        #region ISelectable Members

        public bool IsSelected {
            get { return selected; }
        }

        public void Select() {
            selected = true;
            visual.FrameNumber = 1;
        }

        public void Unselect() {
            selected = false;
            visual.FrameNumber = 0;
        }

        #endregion
    }
}