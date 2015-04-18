using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public partial class GameManager : GameComponent
    {
        List<float> moneyLevels;
        public const int TaxPerPerson = 1;

        public int GetMoney(int year)
        {
            return (int)moneyLevels[year - MinimumYear - 1];
        }

        public void SetMoney(int year, float value)
        {
            moneyLevels[year - MinimumYear - 1] = value;
        }

        public void SetMoney(int year, int value)
        {
            moneyLevels[year - MinimumYear - 1] = (float)value;
        }

        public void SpendMoney(int year, float value)
        {
            moneyLevels[year - MinimumYear - 1] -= value;
        }

        public void AddMoney(int year, float value)
        {
            moneyLevels[year - MinimumYear - 1] += value;
        }

        public void UpdateMoney(int year)
        {
            int pop = (int)popManager.GetPopulation(year);
            SetMoney(year, GetMoney(year - 1) + pop * TaxPerPerson);

            if (year == 1850)
                SetMoney(year, (float)4000);
        }

        private void AllocateMemoryForMoney()
        {
            moneyLevels = new List<float>(MaximumTimeRange);
            for (int i = 0; i < MaximumTimeRange; i++)
            {
                moneyLevels.Add(0);
            }
        }

        private void UpdateMonetaryHistory()
        {
            for (int i = StartingTravelYear; i <= EndTravelYear; i++)
            {
                UpdateMoney(i);
                List<Reservation> res = GameMap.GetReservationsForYear(i);

                foreach (Reservation r in res)
                {
                    if (r.StartingYear == i) //construire
                    {
                        int price = BuildingProperties.BuildingBaseProperties(r.ConstructionName, r.StartingYear, r.UpgradeYear, i).Price;
                        if (GetMoney(i) >= price)
                        {
                            SpendMoney(i, (float)price);
                        }
                        else
                        {
                            GameMap.RemoveReservation(r);
                            Debug.AddToLog("Building canceled, not enough cashcaval");
                        }
                    }
                    if (r.IsUpgraded && r.UpgradeYear == i)
                    {
                        int price = BuildingProperties.BuildingBaseProperties(r.ConstructionName, r.StartingYear, r.UpgradeYear, i).Price;
                        if (GetMoney(i) >= price / 10)
                        {
                            SpendMoney(i, (float)price / 10);
                        }
                        else
                        {
                            r.IsUpgraded = false;
                            r.UpgradeYear = 0;
                        }
                    }
                }

                //mai trebuie pt upgrade si destroy in timpul construirii (undo)
            }

            List<Research> rlist = researchHistory.ResearchList;
            foreach (Research r in rlist)
            {
                if (r.Completed)
                {
                    if (GetMoney(r.YearCompleted) >= r.Cost)
                    {
                        SpendMoney(r.YearCompleted, r.Cost);
                    }
                    else
                    {
                        r.CancelResearch();
                        Debug.AddToLog("Research canceled, not enough cashcaval");
                    }
                }
            }
        }
    }
}