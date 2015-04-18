using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Operation_Cronos.IO {
    public enum IOOperation
    {
        First,
        [DescriptionAttribute("Entering main menu")]
        MainMenu,
        InputBoxDefault,
        [DescriptionAttribute("Logging in to command center")]
        CommandCenter,
        [DescriptionAttribute("Preparing time machine for first use")]
        GameInterface_Graphics,
        [DescriptionAttribute("Loading zone history")]
        GameInterface_Instance,
        [DescriptionAttribute("Preparing for first zone visit")]
        GameMap_Graphics,
        [DescriptionAttribute("Entering zone")]
        GameMap_Instance,
        [DescriptionAttribute("Saving game")]
        SavingGame
    }
    public enum IOContent {
        Graphics,
        Sounds,
        Fonts
    }
    public class IOEventArgs : EventArgs{
        double progressPercent;
        IOOperation operation;
        IOContent content;

        /// <summary>
        /// How much of the content is loaded.
        /// </summary>
        public double Percent {
            get { return progressPercent; }
        }

        /// <summary>
        /// What is being loaded.
        /// </summary>
        public IOContent ContentType {
            get { return content; }
        }

        /// <summary>
        /// Where will the content be used.
        /// </summary>
        public IOOperation Operation {
            get { return operation; }
        }

        public IOEventArgs(double progress, IOOperation op,IOContent c) {
            progressPercent = progress;
            content = c;
            operation = op;
        }
    }
}
