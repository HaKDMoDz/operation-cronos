using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Input;
using Microsoft.Xna.Framework.Input;

/* * * * * * * * InputVisualComponent * * * * How it works * * *  * * * * * *  * * * * **  * ** 
 * 
 * The role of the InputVisualComponent is to propagate the mouse and keyboard events to all the
 * components which can receive such events.
 * 
 * MOUSE EVENTS
 * 
 * The functionality of the mouse events (MousePress, MouseRelease, MouseWheel, MouseMove) has very
 * few changes from one event to another.
 *
 * MousePress
 * 
 * Esentially, when the MousePress method is called, it checks to see if the Point supplied as parameter
 * collides with the component and if it does, it generates the OnMousePress event, but it also checks to 
 * see if the any of the children collides with the mouse. If one of them does collide, its MousePress 
 * method is called and the process stops. The reason is that even if the mouse collides with other children,
 * they will have a lower StackOrder than the first colliding child (meaning they are under this child, 
 * maybe entirely covered by its surface).
 * 
 * NOTICE: If the first colliding child is not IMouseInteractable (it doesn't accept mouse interaction),
 * the process still stops, from the same reason: if there are other children, even IMouseInteractable, 
 * which are covered by the first colliding child, the OnMousePress event must not happen.
 * 
 * 
 * MouseMove
 * 
 * Because this event will be used to generate OnMouseOver and OnMouseLeave events for buttons, the components
 * must receive it even when the mouse doesn't collide with them.
 * */

namespace Operation_Cronos.Display
{
    public class InputVisualComponent : VisualComponent, IMouseInteractable
    {
        public InputVisualComponent(Game game)
            : base(game)
        {
            strictMouseCollision = false;
        }

        #region IMouseInteractable Members
        protected Boolean strictMouseCollision;
        public Boolean StrictMouseCollision
        {
            get { return strictMouseCollision; }
        }

        public event EventHandler<MouseEventArgs> OnMousePress = delegate { };

        public event EventHandler<MouseEventArgs> OnMouseRelease = delegate { };

        public event EventHandler<MouseEventArgs> OnMouseMove = delegate { };

        public event EventHandler<MouseEventArgs> OnMouseWheel = delegate { };

        public void MousePress(MouseButton button, Point mouseWorldPosition)
        {
            if (Enabled && Visible)
            {
                if (StrictMouseCollision)
                {
                    if (Collides(mouseWorldPosition))
                    {
                        OnMousePress(this, new MouseEventArgs(mouseWorldPosition, button));
                        for (int i = SortedComponents.Count - 1; i >= 0; i--)
                        {
                            GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                            if (item.Enabled)
                                if (item is VisualComponent)
                                {
                                    if (((IPositionable)item).Collides(mouseWorldPosition))
                                    {
                                        if (item is IMouseInteractable)
                                        {
                                            ((IMouseInteractable)item).MousePress(button, mouseWorldPosition);
                                        }
                                        return;
                                    }
                                }
                                else
                                {
                                    Point positionForSprite = new Point();
                                    positionForSprite.X = mouseWorldPosition.X - ((IPositionable)item).X;
                                    positionForSprite.Y = mouseWorldPosition.Y - ((IPositionable)item).Y;
                                    if (((IPositionable)item).Collides(positionForSprite))
                                    {
                                        return;
                                    }
                                }
                        }
                    }
                }
                else
                {
                    if (CollidesTile(mouseWorldPosition))
                    {
                        OnMousePress(this, new MouseEventArgs(mouseWorldPosition, button));
                        //Debug.AddToLog("[DEBUG] Mouse Press: " + this.ToString());
                        for (int i = SortedComponents.Count - 1; i >= 0; i--)
                        {
                            GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                            if (item.Enabled)
                            {
                                if (item is VisualComponent)
                                {
                                    if (((IPositionable)item).CollidesTile(mouseWorldPosition))
                                    {
                                        if (item is IMouseInteractable)
                                        {
                                            ((IMouseInteractable)item).MousePress(button, mouseWorldPosition);
                                        }
                                        return;
                                    }
                                }
                                else
                                {
                                    Point positionForSprite = new Point();
                                    positionForSprite.X = mouseWorldPosition.X - ((IPositionable)item).X;
                                    positionForSprite.Y = mouseWorldPosition.Y - ((IPositionable)item).Y;
                                    if (((IPositionable)item).CollidesTile(positionForSprite))
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void MouseRelease(MouseButton button, Point mouseWorldPosition)
        {
            if (Enabled && Visible)
            {
                if (StrictMouseCollision)
                {
                    if (Collides(mouseWorldPosition))
                    {
                        OnMouseRelease(this, new MouseEventArgs(mouseWorldPosition, button));
                        for (int i = SortedComponents.Count - 1; i >= 0; i--)
                        {
                            GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                            if (item.Enabled)
                                if (item is VisualComponent)
                                {
                                    if (((IPositionable)item).Collides(mouseWorldPosition))
                                    {
                                        if (item is IMouseInteractable)
                                        {
                                            ((IMouseInteractable)item).MouseRelease(button, mouseWorldPosition);
                                        }
                                        return;
                                    }
                                }
                                else
                                {
                                    Point positionForSprite = new Point();
                                    positionForSprite.X = mouseWorldPosition.X - ((IPositionable)item).X;
                                    positionForSprite.Y = mouseWorldPosition.Y - ((IPositionable)item).Y;
                                    if (((IPositionable)item).Collides(positionForSprite))
                                    {
                                        return;
                                    }
                                }
                        }
                    }
                }
                else
                {
                    if (CollidesTile(mouseWorldPosition))
                    {
                        OnMouseRelease(this, new MouseEventArgs(mouseWorldPosition, button));
                        for (int i = SortedComponents.Count - 1; i >= 0; i--)
                        {
                            GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                            if (item.Enabled)
                                if (item is VisualComponent)
                                {
                                    if (((IPositionable)item).CollidesTile(mouseWorldPosition))
                                    {
                                        if (item is IMouseInteractable)
                                        {
                                            ((IMouseInteractable)item).MouseRelease(button, mouseWorldPosition);
                                        }
                                        return;
                                    }
                                }
                                else
                                {
                                    Point positionForSprite = new Point();
                                    positionForSprite.X = mouseWorldPosition.X - ((IPositionable)item).X;
                                    positionForSprite.Y = mouseWorldPosition.Y - ((IPositionable)item).Y;
                                    if (((IPositionable)item).CollidesTile(positionForSprite))
                                    {
                                        return;
                                    }
                                }
                        }
                    }
                }
            }
        }

        public void MouseMove(MouseButton button, Point mouseWorldPosition)
        {
            if (Enabled && Visible)
            {
                OnMouseMove(this, new MouseEventArgs(mouseWorldPosition, button));
                for (int i = SortedComponents.Count - 1; i >= 0; i--)
                {
                    GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                    if (item is IMouseInteractable)
                    {
                        ((IMouseInteractable)item).MouseMove(button, mouseWorldPosition);
                    }
                }
            }
        }

        public void MouseWheel(MouseButton button, Point mouseWorldPosition)
        {
            if (Enabled && Visible)
            {
                if (StrictMouseCollision)
                {
                    if (Collides(mouseWorldPosition))
                    {
                        OnMouseWheel(this, new MouseEventArgs(mouseWorldPosition, button));
                        for (int i = SortedComponents.Count - 1; i >= 0; i--)
                        {
                            GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                            if (item.Enabled)
                                if (item is VisualComponent)
                                {
                                    if (((IPositionable)item).Collides(mouseWorldPosition))
                                    {
                                        if (item is IMouseInteractable)
                                        {
                                            ((IMouseInteractable)item).MouseWheel(button, mouseWorldPosition);
                                        }
                                        return;
                                    }
                                }
                                else
                                {
                                    Point positionForSprite = new Point();
                                    positionForSprite.X = mouseWorldPosition.X - ((IPositionable)item).X;
                                    positionForSprite.Y = mouseWorldPosition.Y - ((IPositionable)item).Y;
                                    if (((IPositionable)item).Collides(positionForSprite))
                                    {
                                        return;
                                    }
                                }
                        }
                    }
                }
                else
                {
                    if (CollidesTile(mouseWorldPosition))
                    {
                        OnMouseWheel(this, new MouseEventArgs(mouseWorldPosition, button));
                        for (int i = SortedComponents.Count - 1; i >= 0; i--)
                        {
                            GameComponent item = (GameComponent)SortedComponents.ElementAt(i);
                            if (item.Enabled)
                                if (item is VisualComponent)
                                {
                                    if (((IPositionable)item).CollidesTile(mouseWorldPosition))
                                    {
                                        if (item is IMouseInteractable)
                                        {
                                            ((IMouseInteractable)item).MouseWheel(button, mouseWorldPosition);
                                        }
                                        return;
                                    }
                                }
                                else
                                {
                                    Point positionForSprite = new Point();
                                    positionForSprite.X = mouseWorldPosition.X - ((IPositionable)item).X;
                                    positionForSprite.Y = mouseWorldPosition.Y - ((IPositionable)item).Y;
                                    if (((IPositionable)item).CollidesTile(positionForSprite))
                                    {
                                        return;
                                    }
                                }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
