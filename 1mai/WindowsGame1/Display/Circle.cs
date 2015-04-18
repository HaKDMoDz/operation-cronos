using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    public class Circle {
        Point center;
        int radius;

        public int Radius {
            get { return radius; }
            set { radius = value; }
        }

        public Point Center {
            get { return center; }
            set { center = value; }
        }

        public Circle() {
        }

        public Circle(Point center, int radius) {
            Radius = radius;
            Center = center;
        }

        public Boolean Contains(Point point) {
            if (Vector2.Distance(new Vector2((float)Center.X,(float)Center.Y),
                new Vector2((float)point.X, (float)point.Y)) <= (float)radius) {
                return true;
            }
            return false;
        }

        public override string ToString() {
            return "Circle with center" + Center.ToString() + " and radius " + radius.ToString();
        }
    }
}
