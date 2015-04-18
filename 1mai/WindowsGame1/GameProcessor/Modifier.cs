using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class Modifier : GameComponent
    {

        #region Properties
        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }
        #endregion

        public Modifier(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Building production modifier. Is determined by research.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public MilleniumGoalsSet Production(int year)
        {
            MilleniumGoalsSet p = new MilleniumGoalsSet(1, 1, 1, 1, 1, 1);
            foreach (Research r in GameManager.ResearchList)
            {
                if (r.Completed && r.YearCompleted <= year)
                {
                    p += r.Bonus;
                }
            }
            return p;
        }

        /// <summary>
        /// Is determined by the Environment need coverage
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public MilleniumGoalsSet EnvironmentBonus(int year)
        {
            return GameMap.MapProduction * GameManager.GetConsumptionCoverage(year).Environment;
        }

        public MilleniumGoalsSet Consumption(int year)
        {

            if (year < (int)HistoricalPeriod.HP1_LastYear)
            {
                return new MilleniumGoalsSet(1, 1, 1, 1, 1, 0);
            }
            else
            {
                return new MilleniumGoalsSet(1, 1, 1, 1, 1, 1);
            }
        }
    }
}
