using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operation_Cronos.Display {
    public interface ISliding {
        Boolean IsOpen { get; }
        void SlideIn();
        void SlideOut();
    }
}
