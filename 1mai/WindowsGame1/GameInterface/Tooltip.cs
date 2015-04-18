using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.IO;

namespace Operation_Cronos.Display {
    public class Tooltip : VisualComponent
    {
        #region Fields
        SpriteText text;

        Sprite Background;
        #endregion

        #region Properties
        //The text to be displayed in the tooltip
        public String Text 
        {
            get { return text.Text; }
            set { text.Text = value;
            }
        }

        public override int Width
        {
            get
            {
                return Background.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        public override int Height
        {
            get
            {
                return Background.Height;
            }
            set
            {
                base.Height = value;
            }
        }
        #endregion
        
        public Tooltip(Game game, int OneOfThreeSize)
            : base(game) 
        {
            if (OneOfThreeSize==3)
                Background = new Sprite(game, GraphicsCollection.GetPack("tooltip_large"));
            if (OneOfThreeSize==2)
                Background = new Sprite(game, GraphicsCollection.GetPack("tooltip_small"));
            if (OneOfThreeSize==1)
                Background = new Sprite(game, GraphicsCollection.GetPack("tooltip_mini"));

            Background.StackOrder = 0;
            AddChild(Background);

            text = new SpriteText(game, FontsCollection.GetPack("Calibri 8").Font);
            text.StackOrder = 1;
            text.XRelative = 5;
            text.YRelative = 3;
            text.MaxLength = 350;

            AddChild(text);
        }

        public void AlignTextToRight()
        {
            if (text.Text != "")
            {
                string spaces = "";
                while (text.Width < Background.Width - 20)
                {
                    spaces += " ";
                    text.Text = spaces + text.Text;
                }
            }
        }
    }
}
