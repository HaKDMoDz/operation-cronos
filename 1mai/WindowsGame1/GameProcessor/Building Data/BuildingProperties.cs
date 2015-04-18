using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operation_Cronos.GameProcessor
{
    public class BuildingProperties
    {
        #region Fields
        int constructingPeriod;
        int destructionPeriod;
        int lifetime;
        int cost;
        #endregion

        #region Properties
        public int ConstructingPeriod
        {
            get { return constructingPeriod; }
            set { constructingPeriod = value; }
        }

        public int DestructionPeriod
        {
            get { return destructionPeriod; }
            set { destructionPeriod = value; }
        }
        public int Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }
        public int Price
        {
            get { return cost; }
            set { cost = value; }
        }
        public int UpgradePrice
        {
            get { return (int)(0.08f * ((float)cost)); }
        }
        public int UpgradePricePopulation//0.6 of half the cost of the next stage population construction
        {
            get { return (int)(0.6f * ((float)cost)); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Uses the default values (50 years lifetime, 2 years constructing period)
        /// </summary>
        public BuildingProperties()
        {
            lifetime = 50;
            constructingPeriod = 2;
        }
        public BuildingProperties(int life, int period, int destruction, int price)
        {
            lifetime = life;
            constructingPeriod = period;
            destructionPeriod = destruction;
            cost = price;
        }
        #endregion

        #region Operators
        public static BuildingProperties operator +(BuildingProperties a, BuildingProperties b)
        {
            BuildingProperties c = new BuildingProperties();
            c.Lifetime = a.lifetime + b.lifetime;
            c.constructingPeriod = a.constructingPeriod + b.constructingPeriod;
            return c;
        }
        public static BuildingProperties operator -(BuildingProperties a, BuildingProperties b)
        {
            BuildingProperties c = new BuildingProperties();
            c.Lifetime = a.lifetime - b.lifetime;
            c.constructingPeriod = a.constructingPeriod - b.constructingPeriod;
            return c;
        }
        #endregion
    }
}
