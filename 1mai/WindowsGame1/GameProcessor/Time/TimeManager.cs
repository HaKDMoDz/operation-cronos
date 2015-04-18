using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor {
    public class TimeManager : GameComponent {
        int year;

        public int Year {
            get { return year; }
            set { ChangeYear(value); }
        }

        private GameInterface GUI {
            get { return (GameInterface)Game.Services.GetService(typeof(GameInterface)); }
        }

        private GameMap GameMap {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        public TimeManager(Game game)
            : base(game) {

            //Game.Services.AddService(typeof(TimeManager), this);
            Game.Components.Add(this);
        }

        private void ChangeYear(int newYear) {
            year = newYear;
            GUI.UpdateYear(year);
            GameMap.UpdateYear(year);
        }
    }
}
