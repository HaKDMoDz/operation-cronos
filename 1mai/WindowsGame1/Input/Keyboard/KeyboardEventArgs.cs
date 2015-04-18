using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Operation_Cronos.Input
{
    public class KeyboardEventArgs:EventArgs
    {
        private Keys key;
        public KeyboardEventArgs(Keys _key)
        {
            this.key = _key;
        }
        public Keys Key
        {
            get { return key; }
        }
    }
    
}
