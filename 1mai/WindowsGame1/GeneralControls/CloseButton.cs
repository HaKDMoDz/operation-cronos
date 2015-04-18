using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    public class CloseButton : Button {
        public Sprite visual;

        public override int Height {
            get {
                return visual.Height;
            }
        }

        public CloseButton(Game game)
            : base(game) {
            visual = new Sprite(game, GraphicsCollection.GetPack("close_button"));
            visual.XRelative = 0;
            visual.YRelative = 0;
            visual.StackOrder = 0;

            AddChild(visual);

            UpdateComponentsX();
            UpdateComponentsY();
        }

        public override void MouseOverAnimation() {
            visual.FrameNumber = 1;
        }

        public override void MouseLeaveAnimation() {
            visual.FrameNumber = 0;
        }

        public override void PressAnimation() {
            visual.FrameNumber = 2;
        }

        public override void ReleaseAnimation() {
            visual.FrameNumber = 0;
        }
    }
}
