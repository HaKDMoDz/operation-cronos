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


namespace Operation_Cronos.Input {
    /// <summary>
    /// Generates mouse events.
    /// </summary>
    public class MouseManager : Microsoft.Xna.Framework.GameComponent {

        const int MoveTreshold = 0;

        #region Fields
        /// <summary>
        /// The previous state of the mouse.
        /// </summary>
        private MouseState oldMouse;
        /// <summary>
        /// The current state of the mouse.
        /// </summary>
        private MouseState thisMouse;
        /// <summary>
        /// The positions of the mouse.
        /// </summary>
        private Point position;
        /// <summary>
        /// The mouse button which is currently pressed.
        /// </summary>
        private MouseButton pressedButton;
        #endregion

        #region Properties
        /// <summary>
        /// The previous state of the mouse.
        /// </summary>
        public MouseState OldMouseState {
            get { return oldMouse; }
        }

        /// <summary>
        /// The current state of the mouse.
        /// </summary>
        public MouseState CurrentMouseState {
            get { return thisMouse; }
        }

        /// <summary>
        /// The current mouse positions.
        /// </summary>
        public Point MousePosition {
            get { return position; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Invoked when a button is pressed.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnPress = delegate { };
        /// <summary>
        /// Invoked when a mouse button is released.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnRelease = delegate { };
        /// <summary>
        /// Invoked when the mouse wheel is rolled.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnWheel = delegate { };
        /// <summary>
        /// Invoked when the mouse is moved.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnMouseMove = delegate { };
        #endregion

        #region Constructors
        public MouseManager(Game game)
            : base(game) {
            Game.Components.Add(this);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize() {
            pressedButton = MouseButton.None;
            oldMouse = Mouse.GetState();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            UpdateCurrentMouse();
            PollButtons();
            PollWheel();
            PollPosition();
            UpdateOldMouse();
        }


        #endregion

        #region Methods
        /// <summary>
        /// Called after all the polling has been made, 
        /// keeps the current state of the mouse for the next polling.
        /// </summary>
        private void UpdateOldMouse() {
            oldMouse = Mouse.GetState();
        }

        /// <summary>
        /// Called before polling, takes the current mouse state
        /// to be able to compare it with the previous.
        /// </summary>
        private void UpdateCurrentMouse() {
            thisMouse = Mouse.GetState();
            position = new Point(thisMouse.X, thisMouse.Y);
        }
        /// <summary>
        /// Polls the buttons of the mouse for changes in their states.
        /// </summary>
        private void PollButtons() {
            #region Left Button
            if (OldMouseState.LeftButton == ButtonState.Released
                && CurrentMouseState.LeftButton == ButtonState.Pressed) {
                pressedButton = MouseButton.LeftButton;
                OnPress(this, new MouseEventArgs(MousePosition, MouseButton.LeftButton));
            }
            if (OldMouseState.LeftButton == ButtonState.Pressed
                && CurrentMouseState.LeftButton == ButtonState.Released) {
                pressedButton = MouseButton.None;
                OnRelease(this, new MouseEventArgs(MousePosition, MouseButton.LeftButton));
            }
            #endregion

            #region Right Button
            if (OldMouseState.RightButton == ButtonState.Released
                && CurrentMouseState.RightButton == ButtonState.Pressed) {
                pressedButton = MouseButton.RightButton;
                OnPress(this, new MouseEventArgs(MousePosition, MouseButton.RightButton));
            }
            if (OldMouseState.RightButton == ButtonState.Pressed
                && CurrentMouseState.RightButton == ButtonState.Released) {
                pressedButton = MouseButton.None;
                OnRelease(this, new MouseEventArgs(MousePosition, MouseButton.RightButton));
            }
            #endregion

            #region Middle Button
            if (OldMouseState.MiddleButton == ButtonState.Released
                && CurrentMouseState.MiddleButton == ButtonState.Pressed) {
                pressedButton = MouseButton.MiddleButton;
                OnPress(this, new MouseEventArgs(MousePosition, MouseButton.MiddleButton));
            }
            if (OldMouseState.MiddleButton == ButtonState.Pressed
                && CurrentMouseState.MiddleButton == ButtonState.Released) {
                pressedButton = MouseButton.None;
                OnRelease(this, new MouseEventArgs(MousePosition, MouseButton.MiddleButton));
            }
            #endregion

            #region Extra Button 1
            if (OldMouseState.XButton1 == ButtonState.Released
                && CurrentMouseState.XButton1 == ButtonState.Pressed) {
                pressedButton = MouseButton.ExtraButton1;
                OnPress(this, new MouseEventArgs(MousePosition, MouseButton.ExtraButton1));
            }
            if (OldMouseState.XButton1 == ButtonState.Pressed
                && CurrentMouseState.XButton1 == ButtonState.Released) {
                pressedButton = MouseButton.None;
                OnRelease(this, new MouseEventArgs(MousePosition, MouseButton.ExtraButton1));
            }
            #endregion

            #region Extra Button 2
            if (OldMouseState.XButton2 == ButtonState.Released
                && CurrentMouseState.XButton2 == ButtonState.Pressed) {
                pressedButton = MouseButton.ExtraButton2;
                OnPress(this, new MouseEventArgs(MousePosition, MouseButton.ExtraButton2));
            }
            if (OldMouseState.XButton2 == ButtonState.Pressed
                && CurrentMouseState.XButton2 == ButtonState.Released) {
                pressedButton = MouseButton.None;
                OnRelease(this, new MouseEventArgs(MousePosition, MouseButton.ExtraButton2));
            }
            #endregion
        }
        /// <summary>
        /// Checks if the wheel value is changed and 
        /// generates an OnWheel event if that is true.
        /// </summary>
        private void PollWheel() {
            int value = CurrentMouseState.ScrollWheelValue - OldMouseState.ScrollWheelValue;
            if (value < 0) {
                OnWheel(this, new MouseEventArgs(MousePosition, MouseButton.WheelBackward));
            } else if (value > 0) {
                OnWheel(this, new MouseEventArgs(MousePosition, MouseButton.WheelForward));
            }
        }
        /// <summary>
        /// Checks if the mouse has been moved from the last
        /// polling, and if it's true, generates an MouseMove event,
        /// sending the pressed button too.
        /// </summary>
        private void PollPosition() {
            if (Math.Abs(CurrentMouseState.X - OldMouseState.X) > MouseManager.MoveTreshold
                || Math.Abs(CurrentMouseState.Y - OldMouseState.Y) > MouseManager.MoveTreshold) {
                OnMouseMove(this, new MouseEventArgs(MousePosition, pressedButton));
            }
        }
        #endregion
    }
}