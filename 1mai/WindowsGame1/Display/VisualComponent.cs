using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Operation_Cronos.IO;

/*************  Visual Component - How it works?  *******************************************************
 * 
 * A VisualComponent is, esentially, a container for Sprites, SpriteTexts or other child VisualComponents.
 * Its two main features are given by the two interfaces it implements: IStackable and IPositionable. 
 * 
 * Let's start with IPositionable. First, this interface brings the X and Y properties. These coordinates
 * are the absolute coordinates of the VisualComponent, while XRelative and YRelative are the relative
 * coordinates (relative to the VisualComponent's parent). That means that a VisualComponent with an XRelative
 * of 50px will be positioned 50px to the right of its parent left margin. 
 * Then, IPositionable brings Width and Height, which return 0, by default, but are marked as virtual, so they
 * can be overwritten in derived classes. The methods Collides and CollidesTile return true if the Point 
 * supplied as parameter is contained by the VisualComponent. Both of these methods are marked virtual,
 * and by default they check subsequently for collisions the child components which implement IPositionable.
 * If one of the children collides with the Point, the method returns TRUE. The children are checked 
 * according to their StackOrder (explained below).
 * 
 * The IStackable interface brings two properties: Visible and StackOrder. The Visible property is obvious,
 * with one mention: if a VisualComponent is set invisible, all the child components are set invisible.
 * Next comes StackOrder. The idea behind is simple: the VisualComponent is considered a stack of child 
 * components. StackOrder shows the order of the children in this stack (0 - bottom). Only the order is 
 * important so, if there are gaps between StackOrders, there is no problem.
 * To implement this feature, there are two GameComponentCollections: one two keep the references to the
 * children, and another to sort temporarily the children. Also, there is a StartingDrawOrder value, which
 * can be considered the "DrawOrder" of the VisualComponent. In fact it is the DrawOrder of the bottom child.
 * Another property, NeededDrawOrders, is a number equal to the number of children. It is needed to compute
 * the StartingDrawOrder of the next VisualComponent on the stack. (Example: inside a VisualComponent, the 
 * bottom child has the StartingDrawOrder = 25 and NeededDrawOrders = 10. The StartingDrawOrder of the
 * child on top of it will be 25+10+1 = 36.)
 * The catch is that these children can have children too... What to do?
 * 
 * The solution is simple. It is easy to see that this structure of VisualComponents is nothing else than
 * a tree. The leaves of this tree are Sprites and SpriteTexts. It is easy to get the NeededDrawOrders for 
 * the parents of these leaves, it's actually the number of Sprites and SpriteTexts contained. And if the
 * NeededDrawOrders for the parents of the leaves are known, it's easy to calculate the NeededDrawOrders of
 * the next level of VisualComponents. This leads, obviously, to recursive algorithms.
 * 
 * So, the number of children can be computed only if the count starts from the bottom of the tree (from 
 * the leaves). When a component is inside a VisualComponent, an event is generated. In the handler, the 
 * new child is tested if it implements IDrawable (meaning it's either Sprite or SpriteText) or not. At
 * the bottom of the tree, the first VisualComponents contain only Sprites or SpriteTexts. When a new 
 * IDrawable item is added, the NeededDrawOrders value is incremented with 1. That's how the NeededDrawOrders
 * is computed for the bottom VisualComponents. For those in higher levels, which have children that are also
 * VisualComponents, the NeededDrawOrders is already computed.
 * 
 * Now comes the real catch: after a new component is added into a VisualComponent, and after NeededDrawOrders
 * is incremented in the handler of the event, a new event is generated, which tells the parent of the
 * VisualComponent that its stack was changed. When the parent receives that event it also generates the 
 * OnStackChanged event, and the event travels all the way to the top (the root of the tree). The parent
 * of them all updates the DrawOrders of its children, calling the UpdateComponentsDrawOrders() method.
 * When a child's StackOrder or StartingDrawOrder is changed, it calls it's own UpdateComponentsDrawOrders()
 * method, so the tree rearranges itself, recursively. It is a resource-consuming process, but it happens
 * only when a component is created or removed, which is not very often (speaking in CPU terms).
 * 
 * 
 * The only thing remaining which needs explaining is the colliding mechanism. When the CollidesTile or the
 * Collides method is called, it calls the same method of the children components. Now, this is really expensive
 * for the CPU. To optimize this process, the child components are checked in top-to-bottom order, and when
 * one collision is found, the method returns TRUE, also stopping the process.
 * The most favorable case is when the topmost component collides with the point, and the least favorable, 
 * when the colliding component is at the bottom.
 * 
 * NOTICE: If the Collides method is called, the processing time increases tremendously, because this method
 * does a pixel-by-pixel search (in a 100x100 pixels texture, which is a medium size, there can be as much 
 * as 10.000 tests, while a CollidesTile call will make just 1 test).
 * That's why the StrictCollision property exists: it is FALSE by default, which means that the called method 
 * will be CollidesTile. If it's TRUE, the Collides method will be called. It should be used only when it's
 * absolutely necessary.
 * 
 * */


namespace Operation_Cronos.Display
{

    /// <summary>
    /// This is a game component that contains a userList of sprites, text sprites and/or other VisualComponents.
    /// It implements the IStackable and IPositionable interfaces.
    /// </summary>
    public class VisualComponent : GameComponent, IStackable, IPositionable
    {

        public event EventHandler OnStackChanged = delegate { };
        public event EventHandler OnStackOrderChanged = delegate { };
        public event EventHandler OnRelativePositionChanged = delegate { };

        #region Fields
        private int x;
        private int y;
        private int xrel;
        private int yrel;
        private int stackOrder;
        private int startingDrawOrder;
        private Boolean visible;
        private Boolean isVisible;
        private GameComponentCollection components;
        private GameComponentCollection sortedComponents;
        private int neededDrawOrders;
        #endregion

        #region Properties

        #region IPositionable Members

        /// <summary>
        /// The absolute X positions.
        /// </summary>
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                UpdateComponentsX();
            }
        }


        /// <summary>
        /// The absolute Y positions.
        /// </summary>
        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                UpdateComponentsY();
            }
        }

        public int XRelative
        {
            get { return xrel; }
            set
            {
                xrel = value;
                OnRelativePositionChanged(this, new EventArgs());
                UpdateComponentsX();
            }
        }
        public int YRelative
        {
            get { return yrel; }
            set
            {
                yrel = value;
                OnRelativePositionChanged(this, new EventArgs());
                UpdateComponentsY();
            }
        }

        public virtual int Width
        {
            get { return 0; }
            set { }
        }

        public virtual int Height
        {
            get { return 0; }
            set { }
        }

        #endregion

        #region IStackable
        /// <summary>
        /// The order of this VisualComponent in its layer's visual stack.
        /// </summary>
        public int StackOrder
        {
            get { return stackOrder; }
            set
            {
                stackOrder = value;
                OnStackOrderChanged(this, new EventArgs());
            }
        }

        public Boolean Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                UpdateComponentsVisibility();
            }
        }

        public Boolean IsVisible {
            get { return isVisible; }
            set {
                isVisible = value;
                Visible = value;
                visible = value;
                UpdateComponentsVisibility();
            }
        }

        
        #endregion

        public int StartingDrawOrder
        {
            get { return startingDrawOrder; }
            set
            {
                startingDrawOrder = value;
                UpdateComponentsDrawOrder();
            }
        }

        public int NeededDrawOrders
        {
            get { return neededDrawOrders; }
        }

        /// <summary>
        /// Contains all the components of this VisualComponent.
        /// </summary>
        public GameComponentCollection Components
        {
            get { return components; }
        }

        public GameComponentCollection SortedComponents
        {
            get { return sortedComponents; }
        }

        #endregion

        #region Services
        protected GraphicsCollection GraphicsCollection
        {
            get { return (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection)); }
        }

        protected FontsCollection FontsCollection
        {
            get { return (FontsCollection)Game.Services.GetService(typeof(FontsCollection)); }
        }
        #endregion

        public VisualComponent(Game game)
            : base(game)
        {
            Game.Components.Add(this);
            components = new GameComponentCollection();
            sortedComponents = new GameComponentCollection();
            components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(components_ComponentAdded);
            components.ComponentRemoved += new EventHandler<GameComponentCollectionEventArgs>(components_ComponentRemoved);
            StackOrder = 0;
            neededDrawOrders = 0;
            Visible = true;
            isVisible = true;
        }


        #region Event Handlers
        void components_ComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {

            UpdateComponentsX();
            UpdateComponentsY();

            SortComponentsByStackOrder();
            if (e.GameComponent is VisualComponent)
            {
                neededDrawOrders += ((VisualComponent)e.GameComponent).NeededDrawOrders;
                ((VisualComponent)e.GameComponent).OnStackOrderChanged += new EventHandler(VisualComponent_OnStackOrderChanged);
                ((VisualComponent)e.GameComponent).OnStackChanged += new EventHandler(VisualComponent_OnStackChanged);
                ((VisualComponent)e.GameComponent).OnRelativePositionChanged += new EventHandler(VisualComponent_OnRelativePositionChanged);

            }
            else
            {
                if (e.GameComponent is IDrawable)
                {
                    neededDrawOrders++;
                }
            }

            OnStackChanged(this, new EventArgs());
        }

        void VisualComponent_OnRelativePositionChanged(object sender, EventArgs e)
        {
            UpdateComponentsX();
            UpdateComponentsY();
        }

        void VisualComponent_OnStackChanged(object sender, EventArgs e)
        {
            OnStackChanged(this, new EventArgs());
        }

        void VisualComponent_OnStackOrderChanged(object sender, EventArgs e)
        {
            SortComponentsByStackOrder();
            UpdateComponentsDrawOrder();
        }

        void components_ComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {

            SortComponentsByStackOrder();
            if (e.GameComponent is VisualComponent)
            {
                neededDrawOrders -= ((VisualComponent)e.GameComponent).NeededDrawOrders;
            }
            else
            {
                if (e.GameComponent is IDrawable)
                {
                    neededDrawOrders--;
                }
            }

            OnStackChanged(this, new EventArgs());
        }

        #endregion

        #region Methods

        #region Public
        public virtual Boolean Collides(Point mouseWorldPosition)
        {
            Point relativeMousePosition = new Point();
            relativeMousePosition.X = mouseWorldPosition.X - this.X;
            relativeMousePosition.Y = mouseWorldPosition.Y - this.Y;
            if (this.Enabled)
            {
                foreach (IPositionable component in components)
                {
                    if (component is IDrawable)
                    {
                        Point transformedPosition = new Point();
                        transformedPosition.X = relativeMousePosition.X - component.XRelative;
                        transformedPosition.Y = relativeMousePosition.Y - component.YRelative;
                        if (component.Collides(transformedPosition)) { return true; }
                    }
                    else if (component is VisualComponent)
                    {
                        if (component.Collides(mouseWorldPosition)) { return true; }
                    }
                }
            }
            return false;
        }

        public virtual Boolean CollidesTile(Point mouseWorldPosition)
        {
            Point relativeMousePosition = new Point();
            relativeMousePosition.X = mouseWorldPosition.X - this.X;
            relativeMousePosition.Y = mouseWorldPosition.Y - this.Y;
            if (this.Enabled)
            {
                foreach (IPositionable component in components)
                {
                    if (component is IDrawable)
                    {
                        Point transformedPosition = new Point();
                        transformedPosition.X = relativeMousePosition.X - component.XRelative;
                        transformedPosition.Y = relativeMousePosition.Y - component.YRelative;
                        if (component.CollidesTile(transformedPosition)) { return true; }
                    }
                    else if (component is VisualComponent)
                    {
                        if (component.CollidesTile(mouseWorldPosition)) { return true; }
                    }
                }
            }
            return false;
        }

        public void AddChild(GameComponent component)
        {
            components.Add(component);
        }

        public void RemoveChild(GameComponent component)
        {
            if (component is VisualComponent)
            {
                ((VisualComponent)component).Remove();
            }
            components.Remove(component);
            Game.Components.Remove(component);
        }

        public void Remove()
        {
            foreach (GameComponent component in Components)
            {
                Game.Components.Remove(component);
                if (component is VisualComponent)
                    ((VisualComponent)component).Remove();
            }
            Game.Components.Remove(this);
        }
        #endregion

        #region Protected

        protected void SortComponentsByStackOrder()
        {
            IEnumerable<IGameComponent> query = components.OrderBy(comp => ((IStackable)comp).StackOrder);

            sortedComponents.Clear();
            for (int i = 0; i < query.Count(); i++)
            {
                sortedComponents.Add(query.ElementAt(i));
            }
        }

        protected void UpdateComponentsDrawOrder()
        {
            int nextDrawOrder = StartingDrawOrder;
            foreach (GameComponent item in sortedComponents)
            {
                if (item is IDrawable)
                {
                    ((DrawableGameComponent)item).DrawOrder = nextDrawOrder++;
                }
                else
                {
                    if (item is VisualComponent)
                    {
                        ((VisualComponent)item).StartingDrawOrder = nextDrawOrder;
                        nextDrawOrder += ((VisualComponent)item).NeededDrawOrders;
                    }
                }
            }
        }

        protected virtual void UpdateComponentsX()
        {
            foreach (IPositionable component in components)
            {
                component.X = this.X + component.XRelative;
            }
        }

        protected virtual void UpdateComponentsY()
        {
            foreach (IPositionable component in components)
            {
                component.Y = this.Y + component.YRelative;
            }
        }

        protected void UpdateComponentsVisibility()
        {
            foreach (IStackable component in components)
            {
                if (Visible == true) {
                    if (IsVisible == true)
                    {
                        component.Visible = true;
                    }
                    else
                    {
                        component.Visible = false;
                    }
                } else {
                    component.Visible = false;
                }
                //component.Visible = Visible;
            }
        }

        #endregion

        #endregion
    }
}