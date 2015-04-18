using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    public interface IDraggable {
        Point MousePositionInside {
            get;
        }

        event EventHandler OnPress;
        event EventHandler OnRelease;

        void Press(Point mouseWorldPos);
        void Release(Point mouseWorldPos);
    }
}
