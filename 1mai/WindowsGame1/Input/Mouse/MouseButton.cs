using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Cronos.Input {
    public enum MouseButton {
        /// <summary>
        /// Used when a no-button concept is needed.
        /// </summary>
        None = -1,
        /// <summary>
        /// The left button of the mouse.
        /// </summary>
        LeftButton = 0,
        /// <summary>
        /// The right button of the mouse.
        /// </summary>
        RightButton = 1,
        /// <summary>
        /// The wheel as middle button.
        /// </summary>
        MiddleButton = 4,
        /// <summary>
        /// Wheel rolled forward.
        /// </summary>
        WheelForward = 2,
        /// <summary>
        /// Wheel rolled backward.
        /// </summary>
        WheelBackward = 3,
        /// <summary>
        /// Extra button
        /// </summary>
        ExtraButton1 = 5,
        /// <summary>
        /// Extra button
        /// </summary>
        ExtraButton2 = 6,
    }
}
