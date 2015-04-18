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


namespace Operation_Cronos.Input
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class KeyboardManager : Microsoft.Xna.Framework.GameComponent
    {
        #region Fields
        KeyboardState oldKey;
        KeyboardState thisKey;
        #endregion

        #region Events
        /// <summary>
        /// Invoked when a key is pressed.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> OnPress;
        /// <summary>
        /// Invoked when a key is released.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> OnRelease;
        
        #endregion

        #region Constructor
        public KeyboardManager(Game game)
            : base(game)
        {
            Game.Components.Add(this);
            oldKey = Keyboard.GetState();
        }
        #endregion

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            thisKey = Keyboard.GetState();
            PollButtons();
            oldKey = thisKey;
            base.Update(gameTime);
        }

        public void PollButtons()
        {
            if (thisKey.GetPressedKeys().Length > 0)
            {
                if (oldKey.GetPressedKeys().Length == 0)
                {
                    //Debug.AddToLog("Tasta apasata" + thisKey.GetPressedKeys()[0]);
                    OnPress(this, new KeyboardEventArgs(thisKey.GetPressedKeys()[0]));
                }
                    //daca in ambele stari sunt taste apasate
                else
                {
                    Keys[] oldK = oldKey.GetPressedKeys();
                    Keys[] newK = thisKey.GetPressedKeys();
                    //daca exista vreo diferenta intre cele doua stari
                    //daca a fost apasata o noua tasta
                    if (oldK.Length<newK.Length)
                    {
                        //Debug.AddToLog("Pressed!!! " + Difference(oldK, newK));
                        OnPress(this, new KeyboardEventArgs(Difference(oldK, newK)));
                    }
                    //daca a fost ridicata o noua tasta
                    if (oldK.Length > newK.Length)
                    {
                        //Debug.AddToLog("Released!!! " + Difference(oldK, newK));
                        OnRelease(this, new KeyboardEventArgs(Difference(oldK, newK)));
                        //Debug.AddToLog("Pressed!!! " + newK[0].ToString());
                        //daca a ramas o tasta apasata 
                        //si daca tasta care a ramas apasata este o sageata, genereaza OnPress
                        if (newK[0] == Keys.Up || newK[0] == Keys.Down || newK[0] == Keys.Left || newK[0] == Keys.Right)
                        {
                            OnPress(this, new KeyboardEventArgs(newK[0]));
                        }
                    }
                }
            }
            else
            {
                if (oldKey.GetPressedKeys().Length > 0)
                {
                    //Debug.AddToLog("Released Key" + oldKey.GetPressedKeys()[0]);
                    OnRelease(this, new KeyboardEventArgs(oldKey.GetPressedKeys()[0]));
                }
            }
        }

        private Keys Difference(Keys[] oldK, Keys[] newK)
        {
            for (int i = 0; i < oldK.Length; i++)
            {
                if (!newK.Contains(oldK[i]))
                {
                    return oldK[i];
                }
            }
            for (int i = 0; i < newK.Length; i++)
            {
                if (!oldK.Contains(newK[i]))
                {
                    return newK[i];
                }
            }
            return Keys.None;
        }
    }
}