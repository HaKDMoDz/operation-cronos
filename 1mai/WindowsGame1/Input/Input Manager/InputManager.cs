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
using Operation_Cronos.Profiles;
using Operation_Cronos.Display;


namespace Operation_Cronos.Input {
    public class InputManager : Microsoft.Xna.Framework.GameComponent {

        MouseManager mouseManager;
        KeyboardManager keyboardManager;


        private DisplayManager DisplayManager {
            get { return (DisplayManager)Game.Services.GetService(typeof(DisplayManager)); }
        }

        public InputManager(Game game)
            : base(game) {
            mouseManager = new MouseManager(game);
            keyboardManager = new KeyboardManager(game);
            InitializeMouseEvents();
            Game.Components.Add(this);
            Game.Services.AddService(typeof(InputManager), this);
        }

        private void InitializeMouseEvents() {
            mouseManager.OnPress += new EventHandler<MouseEventArgs>(mouseManager_OnPress);
            mouseManager.OnRelease += new EventHandler<MouseEventArgs>(mouseManager_OnRelease);
            mouseManager.OnWheel += new EventHandler<MouseEventArgs>(mouseManager_OnWheel);
            mouseManager.OnMouseMove += new EventHandler<MouseEventArgs>(mouseManager_OnMouseMove);

            keyboardManager.OnPress += new EventHandler<KeyboardEventArgs>(keyboardManager_OnPress);
            keyboardManager.OnRelease += new EventHandler<KeyboardEventArgs>(keyboardManager_OnRelease);
        }

        #region Keyboard Event Handlers
        void keyboardManager_OnRelease(object sender, KeyboardEventArgs e) {
            DisplayManager.KeyRelease(sender, e);
        }

        void keyboardManager_OnPress(object sender, KeyboardEventArgs e) {
            DisplayManager.KeyPress(sender, e);
        }
        #endregion

        #region Mouse Event Handlers
        void mouseManager_OnMouseMove(object sender, MouseEventArgs e) {
            DisplayManager.MouseMove(sender, e);
        }

        void mouseManager_OnWheel(object sender, MouseEventArgs e) {
            DisplayManager.MouseWheel(sender, e);
        }

        void mouseManager_OnRelease(object sender, MouseEventArgs e) {
            DisplayManager.MouseRelease(sender, e);
        }

        void mouseManager_OnPress(object sender, MouseEventArgs e) {
            DisplayManager.MousePress(sender, e);
        }
        #endregion

        public override void Initialize() {

            base.Initialize();
        }

        public override void Update(GameTime gameTime) {

            base.Update(gameTime);
        }
    }
}