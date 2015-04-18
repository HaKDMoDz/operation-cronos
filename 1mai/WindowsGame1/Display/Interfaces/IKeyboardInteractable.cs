using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display {
    interface IKeyboardInteractable {
        void KeyPress(Keys key);
        void KeyRelease(Keys key);

        event EventHandler<KeyboardEventArgs> KeyPressed;
        event EventHandler<KeyboardEventArgs> KeyReleased;
    }
}
