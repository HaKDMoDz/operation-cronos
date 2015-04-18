using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Operation_Cronos.Input {
    public class MouseEventArgs : EventArgs {
        private Point screenPosition;
        private MouseButton button;
        private Boolean allow;
        

        /// <summary>
        /// The screen coordinates of the mouse.
        /// </summary>
        public Point ScreenPosition {
            get { return screenPosition; }
        }

        /// <summary>
        /// The mouse button that generated the event.
        /// </summary>
        public MouseButton Button {
            get { return button; }
        }

        /// <summary>
        /// Tells if this MouseMoveEvent can generate MouseOver.
        /// </summary>
        public Boolean AllowMouseOverEvent {
            get { return allow; }
        }

        public MouseEventArgs(Point screenPos, MouseButton sourceButton) {
            screenPosition = screenPos;
            button = sourceButton;
        }

        public MouseEventArgs(Point screenPos, MouseButton sourceButton, Boolean allowMouseOverEvent) {
            screenPosition = screenPos;
            button = sourceButton;
            allow = allowMouseOverEvent;
        }
    }
}
