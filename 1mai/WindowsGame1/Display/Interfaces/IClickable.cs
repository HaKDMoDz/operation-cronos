using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display {
    /// <summary>
    /// Work in progress. Please ignore.
    /// </summary>
    public interface IClickable {
        void Press(MouseButton button);
        void Release(MouseButton button);
        void MouseOver();
        void MouseLeave();

        event EventHandler<ButtonEventArgs> OnPress;
        event EventHandler<ButtonEventArgs> OnRelease;
        event EventHandler<ButtonEventArgs> OnMouseOver;
        event EventHandler<ButtonEventArgs> OnMouseLeave;
    }
}
