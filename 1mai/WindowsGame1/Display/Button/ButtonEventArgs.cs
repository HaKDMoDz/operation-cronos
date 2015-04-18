using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display {
    public class ButtonEventArgs : EventArgs {
        MouseButton b;

        public MouseButton Button {
            get { return b; }
        }

        public ButtonEventArgs() {
            b = MouseButton.None;
        }

        public ButtonEventArgs(MouseButton button) {
            b = button;
        }
    }
}
