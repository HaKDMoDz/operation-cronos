using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operation_Cronos.Display {
    interface IStackable {
        Boolean Visible {
            get;
            set;
        }
        int StackOrder {
            get;
            set;
        }
    }
}
