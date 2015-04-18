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


namespace Operation_Cronos.Display
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Slot_old : SelectableComponent
    {
        Sprite img;
        public bool free = true;
        int[] year_of_rezervation = new int[100];
        int[] duration_of_rezervation = new int[100];

        public Slot_old(Game game)
            : base(game)
        {
            img = new Sprite(game, GraphicsCollection.GetPack("steag"));
            img.StackOrder = 6;
            AddChild(img);
        }

        #region Methods
        public void MakeRezervation(int year, int duration)
        {
            for (int i = 0; i < year_of_rezervation.Length; i++)
                if (year_of_rezervation[i] == 0)
                {
                    year_of_rezervation[i] = year;
                    duration_of_rezervation[i] = duration;
                    break;
                }
            free = false;
        }

        public void CancelRezervation(int year)
        {
            for (int i = 0; i < year_of_rezervation.Length; i++)
                if (year_of_rezervation[i] == year)
                {
                    year_of_rezervation[i] = 0;
                    duration_of_rezervation[i] = 0;
                }
        }

        public string ViewRezervations()
        {
            string str = "Anul: ";
            for (int i = 0; i < year_of_rezervation.Length; i++)
            {
                if (year_of_rezervation[i] != 0)
                    str += year_of_rezervation[i].ToString() + ", ";
            }
            str += "\nDurata: ";
            for (int i = 0; i < duration_of_rezervation.Length; i++)
            {
                if (duration_of_rezervation[i] != 0)
                    str += duration_of_rezervation[i].ToString() + ", ";
            }
            return str;
        }
        #endregion

    }
}