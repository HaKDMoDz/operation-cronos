using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    interface IPositionable {
        int X { get; set; }
        int Y { get; set; }
        int XRelative { get; set; }
        int YRelative { get; set; }
        int Width { get; }
        int Height { get; }
        Boolean Collides(Point position);
        Boolean CollidesTile(Point position);
    }
}
