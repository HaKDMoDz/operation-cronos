using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class ResearchHistory
    {
        List<Research> researchList;

        public List<Research> ResearchList
        {
            get { return researchList; }
        }

        public ResearchHistory()
        {
            researchList = new List<Research>();
        }

        public ResearchHistory(List<Research> list)
        {
            researchList = list;
        }

        public MilleniumGoalsSet GetModifierByResearch(int year){
            MilleniumGoalsSet modifier = new MilleniumGoalsSet(1,1,1,1,1,1);
            foreach (Research r in researchList)
            {
                if (r.YearAvailable <= year && r.Completed)
                {
                    modifier += r.Bonus;
                }
            }
            return modifier;
        }

        public List<Research> GetResearchList(ConstructionType type)
        {
            List<Research> list = new List<Research>();
            foreach (Research r in researchList)
            {
                if (r.ResearchType == type)
                {
                    list.Add(r);
                }
            }
            return list;
        }

        public MilleniumGoalsSet CompleteResearch(Research r, int year)
        {
            foreach (Research re in researchList)
            {
                if (re == r)
                {
                    return re.CompleteResearch(year);
                }
            }
            return null;
        }

        public void CancelResearch(Research r)
        {
            r.CancelResearch();
        }

        public void AddResearchToList(Research r)
        {
            researchList.Add(r);
        }
    }
}
