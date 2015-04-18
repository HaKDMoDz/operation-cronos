using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display {
    public partial class BuildingPanel : InputVisualComponent, ISliding {
        private List<BuildingIcon> selectedIcons;
        private List<BuildingIcon> energyIcons;
        private List<BuildingIcon> educationIcons;
        private List<BuildingIcon> foodIcons;
        private List<BuildingIcon> populationIcons;
        private List<BuildingIcon> healthIcons;
        private List<BuildingIcon> environmentIcons;
        private List<BuildingIcon> economyIcons;
        private List<Point> iconPositions;

        #region Properties
        public GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        #endregion

        private void CreateIcons() {
            iconPositions = new List<Point>(6);
            iconPositions.Add(new Point(26, 68));
            iconPositions.Add(new Point(99, 68));
            iconPositions.Add(new Point(26, 133));
            iconPositions.Add(new Point(99, 133));
            iconPositions.Add(new Point(26, 199));
            iconPositions.Add(new Point(99, 199));

            #region Energy
            energyIcons = new List<BuildingIcon>();

            BuildingIcon icon = new BuildingIcon(this.Game, ConstructionType.Energy, Construction.EnergyGeo);
            icon.OnRelease+=new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            energyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Energy, Construction.EnergyNuclear);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            energyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Energy, Construction.EnergySolar);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            energyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Energy, Construction.EnergySolid);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            energyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Energy, Construction.EnergyWind);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            energyIcons.Add(icon);

            for (int i = 0; i < energyIcons.Count; i++)
            {
                energyIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                energyIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

            #region Education
            educationIcons = new List<BuildingIcon>();

            icon = new BuildingIcon(this.Game, ConstructionType.Education, Construction.EducationSchool);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            educationIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Education, Construction.EducationHighSchool);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            educationIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Education, Construction.EducationUniversity);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            educationIcons.Add(icon);

            for (int i = 0; i < educationIcons.Count; i++)
            {
                educationIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                educationIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

            #region Food
            foodIcons = new List<BuildingIcon>();

            icon = new BuildingIcon(this.Game, ConstructionType.Food, Construction.FoodAnimalFarm);   
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            foodIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Food, Construction.FoodCropFarm);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            foodIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Food, Construction.FoodOrchard);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            foodIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Food, Construction.FoodFarmedFisherie);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            foodIcons.Add(icon);

            for (int i = 0; i < foodIcons.Count; i++)
            {
                foodIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                foodIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

            #region Population
            populationIcons = new List<BuildingIcon>();

            icon = new BuildingIcon(this.Game, ConstructionType.Population, Construction.PopulationVillage);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            populationIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Population, Construction.PopulationTown);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            populationIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Population, Construction.PopulationCity);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            populationIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Population, Construction.PopulationMetropolis);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            populationIcons.Add(icon);

            for (int i = 0; i < populationIcons.Count; i++)
            {
                populationIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                populationIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

            #region Health
            healthIcons = new List<BuildingIcon>();

            icon = new BuildingIcon(this.Game, ConstructionType.Health, Construction.HealthHospital);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            healthIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Health, Construction.HealthClinic);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            healthIcons.Add(icon);

            for (int i = 0; i < healthIcons.Count; i++)
            {
                healthIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                healthIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

            #region Environment
            environmentIcons = new List<BuildingIcon>();

            icon = new BuildingIcon(this.Game, ConstructionType.Environment, Construction.EnvironmentNursery);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            environmentIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Environment, Construction.EnvironmentRecycling);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            environmentIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Environment, Construction.EnvironmentWaterPurification);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            environmentIcons.Add(icon);

            for (int i = 0; i < environmentIcons.Count; i++)
            {
                environmentIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                environmentIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

            #region Economy
            economyIcons = new List<BuildingIcon>();

            icon = new BuildingIcon(this.Game, ConstructionType.Economy, Construction.EconomyFactory);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            economyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Economy, Construction.EconomyMine);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            economyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Economy, Construction.EconomySawMill);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            economyIcons.Add(icon);

            icon = new BuildingIcon(this.Game, ConstructionType.Economy, Construction.EconomyOilWell);
            icon.OnRelease += new EventHandler<ButtonEventArgs>(icon_OnRelease);
            icon.Visible = false;
            icon.Enabled = false;
            AddChild(icon);
            economyIcons.Add(icon);

            for (int i = 0; i < economyIcons.Count; i++)
            {
                economyIcons[i].OnMouseOver += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseOver);
                economyIcons[i].OnMouseLeave += new EventHandler<ButtonEventArgs>(BuildingPanelIcon_OnMouseLeave);
            }
            #endregion

        }

        void BuildingPanelIcon_OnMouseOver(object sender, ButtonEventArgs e)
        {
            BuildingIcon icon = ((BuildingIcon)sender);
            tooltip.YRelative = icon.YRelative + 45;
            tooltip.Text = icon.TooltipText;
            tooltip.IsVisible = true;
        }

        void BuildingPanelIcon_OnMouseLeave(object sender, ButtonEventArgs e)
        {
            tooltip.Text = "";
            tooltip.IsVisible = false;
        }

        public void DisplayIcons() 
        {
            for (int i = 0; i < iconPositions.Count; i++)
            {
                if (i < selectedIcons.Count)
                {
                    selectedIcons[i].XRelative = iconPositions[i].X;
                    selectedIcons[i].YRelative = iconPositions[i].Y;
                    selectedIcons[i].IsVisible = true;
                    selectedIcons[i].Enabled = true;
                }
                HideHistoricalPeriod1UnavailableIcons();
            }
            for (int i = 0; i < selectedIcons.Count; i++)
            {
                selectedIcons[i].RefreshTooltip();
            }
        }

        private void HideIcons()
        {
            if (selectedIcons != null)
                foreach (BuildingIcon icon in selectedIcons)
                {
                    icon.IsVisible = false;
                    icon.Enabled = false;
                }
        }

        public void HideHistoricalPeriod1UnavailableIcons()
        {
            if (GameManager.GetCurrentYear() < (int)HistoricalPeriod.HP2_FirstYear)
            {
                if ((selectedIcons[0].BuildingType == ConstructionType.Energy) || (selectedIcons[0].BuildingType == ConstructionType.Environment))
                {
                    HideIcons();
                }
                if (selectedIcons[0].BuildingType == ConstructionType.Economy)
                {
                    for (int i = 0; i < selectedIcons.Count; i++)
                        if (selectedIcons[i].BuildingName == Construction.EconomyOilWell)
                        {
                            selectedIcons[i].IsVisible = false;
                            selectedIcons[i].Enabled = false;
                        }
                }
            }
        }
    }
}
