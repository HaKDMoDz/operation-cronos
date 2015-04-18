using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display {
    public class SelectableComponent : InputVisualComponent{

        public event EventHandler OnSelected = delegate { };
        public event EventHandler OnCleared = delegate { };

        public SelectableComponent(Game game)
            : base(game)
        {
            OnMouseRelease += new EventHandler<MouseEventArgs>(SelectableComponent_OnMouseRelease);
        }

        void SelectableComponent_OnMouseRelease(object sender, MouseEventArgs e) {
            Select();
        }

        protected virtual void SelectionAnimation() {
        }
        protected virtual void ClearSelectionAnimation() {
        }

        public void ClearSelection() {
            OnCleared(this, new EventArgs());
            ClearSelectionAnimation();
        }

        public void Select() {
            OnSelected(this, new EventArgs());
            SelectionAnimation();
        }
    }
}
