using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos.Display
{
    public class GraphPanel:InputVisualComponent
    {
        List<GraphBar> bars;
        SpriteText selectedYear;
        Sprite topBar;
        Sprite bottomBar;
        SpriteText startYear;
        SpriteText endYear;

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }

        public GraphPanel(Game game)
            : base(game)
        {
            topBar = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            topBar.Width = 300;
            topBar.Height = 2;
            topBar.XRelative = 120;
            topBar.YRelative = 99;
            topBar.Tint = Color.Orange;
            AddChild(topBar);

            bottomBar = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            bottomBar.Width = 300;
            bottomBar.Height = 2;
            bottomBar.XRelative = 120;
            bottomBar.YRelative = 201;
            bottomBar.Tint = Color.Orange;
            AddChild(bottomBar);

            startYear = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            startYear.XRelative = 120;
            startYear.YRelative = 180;
            AddChild(startYear);

            endYear = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            endYear.XRelative = 390;
            endYear.YRelative = 180;
            AddChild(endYear);

            //90, 440
            bars = new List<GraphBar>(300);
            for (int year = 0; year < 300; year++)
            {
                GraphBar bar = new GraphBar(game, 0);
                bar.XRelative = 200 + year;
                bar.YRelative = 200;
                bars.Add(bar);
                AddChild(bar);
            }

            selectedYear = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            selectedYear.XRelative = 130;
            selectedYear.YRelative = 55;
            
            AddChild(selectedYear);
        }

        private void Clear()
        {
            foreach (GraphBar bar in bars)
            {
                bar.Tint = Color.White;
            }
        }

        public void Refresh(ConstructionType mg)
        {
            startYear.Text = GameManager.StartingTravelYear.ToString();
            endYear.Text = GameManager.EndTravelYear.ToString();
            for (int year = GameManager.StartingTravelYear; year < GameManager.EndTravelYear; year++)
            {
                MilleniumGoalsSet production = GameManager.GetConsumptionCoverage(year);
                bars[year - GameManager.StartingTravelYear].Percent = (int)(production.GetValue(mg) * 100);
            }
            Clear();
            int prod = (int)(GameManager.GetConsumptionCoverage(GameManager.CurrentYear).GetValue(mg) * 100);
            if (prod > 100) prod = 100;
            if (prod < 0) prod = 0;
            bars[GameManager.CurrentYear - GameManager.StartingTravelYear].Tint = Color.Red;
            bars[GameManager.CurrentYear - GameManager.StartingTravelYear].Percent = 100;
            selectedYear.Text = mg + " Coverage in "
                + GameManager.CurrentYear + ": " +
                 +prod +
                " %";
        }
    }
}
