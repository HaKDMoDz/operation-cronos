using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Input;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    interface IMouseInteractable {

        Boolean StrictMouseCollision {
            get;
        }

        event EventHandler<MouseEventArgs> OnMousePress;
        event EventHandler<MouseEventArgs> OnMouseRelease;
        event EventHandler<MouseEventArgs> OnMouseMove;
        event EventHandler<MouseEventArgs> OnMouseWheel;

        void MousePress(MouseButton button, Point mouseWorldPosition);
        void MouseRelease(MouseButton button, Point mouseWorldPosition);
        void MouseMove(MouseButton button, Point mouseWorldPosition);
        void MouseWheel(MouseButton button, Point mouseWorldPosition);
    }
}
