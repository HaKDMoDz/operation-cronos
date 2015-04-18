using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.GameProcessor
{
    public enum TimeGateUpgradeLevelsValues
    {
        //Number of years
        UpgradeCategory1_Level1 = 20,
        UpgradeCategory1_Level2 = 30,
        UpgradeCategory1_Level3 = 40,
        UpgradeCategory1_Level4 = 50,
        //Number of years
        UpgradeCategory2_Level1 = 80,
        UpgradeCategory2_Level2 = 100,
        UpgradeCategory2_Level3 = 130,
        UpgradeCategory2_Level4 = 160,
        //Bonus percentage
        UpgradeCategory3_Level1 = 5,
        UpgradeCategory3_Level2 = 10,
        UpgradeCategory3_Level3 = 20,
        UpgradeCategory3_Level4 = 30,
        //Bonus percentage
        UpgradeCategory4_Level1 = 40,
        UpgradeCategory4_Level2 = 60,
        UpgradeCategory4_Level3 = 80,
        UpgradeCategory4_Level4 = 100,
    }

    public class TimeMachine : GameComponent
    {
        private int startYear;
        private int endYear;
        private int level;

        #region Properties
        public int CurrentLevel
        {
            get { return level; }
        }

        public int StartYear
        {
            get { return startYear; }
        }

        public int EndYear
        {
            get { return endYear; }
        }

        private GameInterface GUI
        {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        #endregion

        public TimeMachine(Game game)
            : base(game)
        {
            startYear = 2000;
            endYear = 2010;
        }

        public void UpgradeTimeMachine()
        {
            if (level < 4)
            {
                level++;
                startYear -= 50;
                GUI.ChangeTravelYears(startYear, endYear);
            }
        }
    }
}
