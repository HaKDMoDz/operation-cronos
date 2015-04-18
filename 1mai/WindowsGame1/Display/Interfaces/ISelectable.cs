using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operation_Cronos.Display {
    public interface ISelectable {
        bool IsSelected {
            get;
        }
        void Select();
        void Unselect();
    }
}
