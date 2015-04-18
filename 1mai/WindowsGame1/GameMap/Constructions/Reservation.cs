using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display {
    public class Reservation {

        private int startingYear;
        private int duration;
        private int upgradeYear;
        bool isUpgraded;
        bool isPopulationUpgraded;
        bool isDestroyed;
        BuildingsBaseProperties buildingProperties;
        ConstructionStatus constructionStatus;

        private Construction construction;

        #region Properties
        public int StartingYear {
            get { return startingYear; }
        }

        public int Duration {
            get { return duration; }
            set { duration = value; }
        }

        public int EndingYear {
            get { return startingYear + duration; }
        }

        public int UpgradeYear
        {
            get { return upgradeYear; }
            set { upgradeYear = value; }
        }

        public bool IsUpgraded
        {
            get { return isUpgraded; }
            set { isUpgraded = value;}
        }

        public bool IsPopulationUpgraded
        {
            get { return isPopulationUpgraded; }
            set { isPopulationUpgraded = value; }
        }

        public bool IsDestroyed
        {
            get { return isDestroyed; }
            set { isDestroyed = value; }
        }

        public Construction ConstructionName {
            get { return construction; }
            set { construction = value; }
        }
        #endregion

        public Reservation(int year, int d, Construction constructionName) {
            startingYear = year;
            duration = d;
            construction = constructionName;

            upgradeYear = 0;
            isUpgraded = false;
            isPopulationUpgraded = false;
            isDestroyed = false;

            buildingProperties = new BuildingsBaseProperties();

            constructionStatus = ConstructionStatus.None;
        }

        public Boolean IsYearReserved(int year) {
            return year >= startingYear && year <= EndingYear;
        }

        public Boolean OverlapsReservation(Reservation reservation) {
            if (startingYear < reservation.StartingYear) {
                if (EndingYear >= reservation.startingYear) {
                    return true;
                }
                return false;
            }
            if (startingYear == reservation.StartingYear) {
                return true;
            }
            if (startingYear > reservation.StartingYear) {
                if (reservation.EndingYear >= startingYear) {
                    return true;
                }
                return false;
            }
            return false;
        }

        public void RemoveReservationFromYear(int year)
        {
            duration = year - startingYear - 1;
            isDestroyed = true;
        }

        public ConstructionStatus Status(int year)
        {

            int ConstructingPeriod = buildingProperties.BuildingBaseProperties(construction, startingYear, upgradeYear, year).ConstructingPeriod;
            int DestructionPeriod = buildingProperties.BuildingBaseProperties(construction, startingYear, upgradeYear, year).DestructionPeriod;

            if ((year >= startingYear) && //construction stage 2
                 (year < startingYear + ConstructingPeriod))
                return ConstructionStatus.InConstruction;
            if ((year >= (startingYear + ConstructingPeriod)) &&
                (year <= (EndingYear - DestructionPeriod)))
                return ConstructionStatus.InProduction;
            if ((year > (EndingYear - DestructionPeriod) &&
                (year <= EndingYear)))
                return ConstructionStatus.InDegradation;
            return ConstructionStatus.None;
        }

        public override string ToString() {
            return construction.ToString()+" Starting Year: " + startingYear.ToString() + " EndingYear: " + EndingYear.ToString();
        }
    }
}
