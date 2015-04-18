using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display {


    public class Slot : SelectableComponent{
        
        Sprite visual;
        List<Reservation> reservationList;
        ConstructionType type;
        BaseConstruction construction;
        int XCenter = 0;
        int YCenter = 0;

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }

        public int XSlotCenter
        {
            get
            {
                return XCenter;
            }
            set
            {
                XCenter = value;
                this.XRelative = XCenter - this.Width / 2;
                UpdateComponentsX();
            }
        }

        public int YSlotCenter
        {
            get
            {
                return YCenter;
            }
            set
            {
                YCenter = value;
                this.YRelative = YCenter - this.Height / 2;
                UpdateComponentsX();
            }
        }
        
        public override int Width {
            get {
                return visual.Width;
            }
            set {
                base.Width = value;
            }
        }

        public override int Height {
            get {
                return visual.Height;
            }
            set {
                base.Height = value;
            }
        }

        public List<Reservation> ReservationList
        {
            get
            {
                return reservationList;
            }
            set
            {
                reservationList = value;
            }            
        }

        public ConstructionType ConstructionType {
            get { return type; }
        }

        public BaseConstruction CurrentConstruction {
            get { return construction; }
            set { construction = value; }
        }

        public Slot(Game game, ConstructionType category):base(game) {
            type = category;
            visual = new Sprite(game, GraphicsCollection.GetPack("flag"));
            reservationList = new List<Reservation>();
            AddChild(visual);
        }

        public void MakeReservation(int year, int duration, Construction building) {
            Reservation newRes = new Reservation(year, duration, building);
            
            for (int i = 0; i < reservationList.Count; i++) {
                if (newRes.OverlapsReservation(reservationList[i])) {
                    reservationList.Remove(reservationList[i]);
                    i--;
                }
            }
            reservationList.Add(newRes);
        }

        public void MakeReservation(Reservation reservation)
        {
            for (int i = 0; i < reservationList.Count; i++)
            {
                if (reservation.OverlapsReservation(reservationList[i]))
                {
                    reservationList.Remove(reservationList[i]);
                    i--;
                }
            }
            reservationList.Add(reservation);
        }

        public void UndoReservation(int year) 
        {
            reservationList.Remove(GetReservation(year)); 
        }

        public void ShortenReservation(int year)
        {
            GetReservation(year).RemoveReservationFromYear(year);            
        }

        public void ProlongReservation(int year)
        {
            BuildingsBaseProperties properties = new BuildingsBaseProperties();

            Reservation prolongedReservation = GetReservation(year);
            prolongedReservation.Duration = (year - GetReservation(year).StartingYear) + properties.BuildingBaseProperties(this.CurrentConstruction.ConstructionName,this.CurrentConstruction.ConstructionYear,this.CurrentConstruction.UpgradeYear, year).Lifetime;
            MakeReservation(prolongedReservation);
        }

        public void Upgrade(int upgradeYear)
        {
            GetReservation(upgradeYear).UpgradeYear = upgradeYear;
            GetReservation(upgradeYear).IsUpgraded = true;
        }

        public void CancelReservation(Reservation r)
        {
            reservationList.Remove(r);
        }

        public Boolean HasReservation(Reservation r)
        {
            return reservationList.Contains(r);
        }

        public Reservation GetReservation(int year) {
            foreach (Reservation res in reservationList) {
                if (res.IsYearReserved(year)) {
                    return res;
                }
            }
            return null;
        }

        public Boolean IsFree(int year) {
            if (GetReservation(year) != null) {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return "Slot: " + this.ConstructionType.ToString() + ", Free: " + this.IsFree(GameManager.CurrentYear).ToString();
        }
    }
}
