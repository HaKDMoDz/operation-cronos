using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class Research
    {
        int year;
        int completionYear;
        MilleniumGoalsSet bonus;
        int cost;
        String name;
        String description;
        ConstructionType researchType;
        Boolean done;

        #region Properties
        public Boolean Completed
        {
            get { return done; }
            set
            {
                done = value;
            }
        }
        public ConstructionType ResearchType
        {
            get { return researchType; }
            set { researchType=value; }
        }
        public MilleniumGoalsSet Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        /// <summary>
        /// The year this research becomes available
        /// </summary>
        public int YearAvailable
        {
            get { return year; }
            set { year = value; }
        }

        /// <summary>
        /// The year this research has been completed
        /// </summary>
        public int YearCompleted
        {
            get { return completionYear; }
            set { completionYear = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion

        public Research()
        {
        }
        public Research(MilleniumGoalsSet bonus, int price, ConstructionType type)
        {
            done = false;
            this.bonus = bonus;
            cost = price;
            researchType = type;
        }

        public MilleniumGoalsSet CompleteResearch(int year)
        {
            completionYear = year;
            done = true;
            return bonus;
        }

        public void CancelResearch()
        {
            completionYear = 0;
            done = false;
        }
    }
}
