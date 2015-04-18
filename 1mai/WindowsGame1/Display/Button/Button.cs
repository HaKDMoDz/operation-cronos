using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Operation_Cronos.Display;
using Operation_Cronos.Input;


namespace Operation_Cronos.Display {

    /// <summary>
    /// A class that implements the IClickable interface.
    /// It is the base class for every type of button.
    /// </summary>
    public class Button : InputVisualComponent, IClickable {

        public event EventHandler<ButtonEventArgs> OnMouseOver = delegate { };
        public event EventHandler<ButtonEventArgs> OnMouseLeave = delegate { };
        public event EventHandler<ButtonEventArgs> OnPress = delegate { };
        public event EventHandler<ButtonEventArgs> OnRelease = delegate { };

        #region Fields
        /// <summary>
        /// Position of the button.
        /// </summary>
        private Point position;
        private bool mouseIsOver = false;
        #endregion

        #region Properties
        /// <summary>
        /// The positions of the button. To be overriden in the extended class.
        /// </summary>
        public virtual Point Position {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Indicates if the mouse is over the button surface. 
        /// </summary>
        public virtual bool MouseIsOver {
            get { return mouseIsOver; }
        }
        #endregion

        public Button(Game game)
            : base(game) {
            OnMousePress += new EventHandler<MouseEventArgs>(Button_OnMousePress);
            OnMouseRelease += new EventHandler<MouseEventArgs>(Button_OnMouseRelease);
            OnMouseMove += new EventHandler<MouseEventArgs>(Button_OnMouseMove);
        }

        void Button_OnMouseMove(object sender, MouseEventArgs e) {
            if (MouseIsOver) {
                if (StrictMouseCollision) {
                    if (!Collides(e.ScreenPosition)) {
                        MouseLeave();
                    }
                } else {
                    if (CollidesTile(e.ScreenPosition) == false) {
                        MouseLeave();
                    }
                }
            } else {
                if (StrictMouseCollision) {
                    if (Collides(e.ScreenPosition)) {
                        MouseOver();
                    }
                } else {
                    if (CollidesTile(e.ScreenPosition)) {
                        MouseOver();
                    }
                }
            }
            if (StrictMouseCollision) {
                if (Collides(e.ScreenPosition)) {
                    mouseIsOver = true;
                } else {
                    mouseIsOver = false;
                }
            } else {
                if (CollidesTile(e.ScreenPosition)) {
                    mouseIsOver = true;
                } else {
                    mouseIsOver = false;
                }
            }
            
        }

        #region MouseEvent Handlers
        void Button_OnMouseRelease(object sender, MouseEventArgs e) {
            Release(e.Button);
        }

        void Button_OnMousePress(object sender, MouseEventArgs e) {
            Press(e.Button);
        }
        #endregion

        #region IClickable Members

        /// <summary>
        /// If the button is enabled, generates the OnPress event and activates the mouse press animation.
        /// </summary>
        public void Press(MouseButton button) {
            if (Enabled) {
                PressAnimation();
                OnPress(this, new ButtonEventArgs(button));
            }
        }

        /// <summary>
        /// If the button is enabled, generates the OnRelease event and activates the mouse release animation.
        /// </summary>
        public void Release(MouseButton button) {
            if (Enabled) {
                ReleaseAnimation();
                OnRelease(this, new ButtonEventArgs(button));
            }
        }

        /// <summary>
        /// If the button is enabled, generates the OnMouseOver event and activates the mouse over animation.
        /// </summary>
        public void MouseOver() {
            if (Enabled) {
                mouseIsOver = true;
                MouseOverAnimation();
                OnMouseOver(this, new ButtonEventArgs());
            }
        }

        /// <summary>
        /// If the button is enabled, generates the OnMouseLeave event and activates the mouse leave animation.
        /// </summary>
        public void MouseLeave() {
            if (Enabled) {
                mouseIsOver = false;
                MouseLeaveAnimation();
                OnMouseLeave(this, new ButtonEventArgs());
            }
        }

        #endregion

        /// <summary>
        /// To be overriden in the extended class.
        /// </summary>
        public virtual void PressAnimation() { }

        /// <summary>
        /// To be overriden in the extended class.
        /// </summary>
        public virtual void ReleaseAnimation() { }

        /// <summary>
        /// To be overriden in the extended class.
        /// </summary>
        public virtual void MouseOverAnimation() { }

        /// <summary>
        /// To be overriden in the extended class.
        /// </summary>
        public virtual void MouseLeaveAnimation() { }
    }
}