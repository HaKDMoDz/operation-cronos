using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Operation_Cronos.Display
{
    public class InputBox : InputVisualComponent, ISelectable
    {

        private Sprite background;
        private Sprite cursor;
        private Sprite leftBorder;
        private Sprite rightBorder;
        private Sprite topBorder;
        private Sprite bottomBorder;

        private SpriteText text;

        #region Properties
        public override int Width
        {
            get
            {
                return background.Width;
            }
            set
            {
                background.Width = value;
                rightBorder.XRelative = value;
                topBorder.Width = bottomBorder.Width = value + BorderWidth;
                UpdateComponentsX();
            }
        }

        public override int Height
        {
            get { return background.Height; }
            set
            {
                background.Height = value;
                bottomBorder.YRelative = value;
                leftBorder.Height = rightBorder.Height = value;
                UpdateComponentsY();
            }
        }

        public String Text
        {
            get { return text.Text; }
            set
            {
                text.Text = value;
                cursor.XRelative = SidePadding + text.Width + 2;
                UpdateComponentsX();
            }
        }

        /// <summary>
        /// The maximum number of characters that can be written.
        /// CAUTION! Some characters are wider than others, so the width of the inputbox
        /// needs to be big enough.
        /// </summary>
        public int MaxLength
        {
            get { return text.MaxLength; }
            set { text.MaxLength = value; }
        }
        #endregion

        #region Style Properties

        public Color TextColor
        {
            get { return text.Tint; }
            set { text.Tint = value; }
        }

        public Color BorderColor
        {
            get { return leftBorder.Tint; }
            set
            {
                leftBorder.Tint = value;
                rightBorder.Tint = value;
                topBorder.Tint = value;
                bottomBorder.Tint = value;
            }
        }

        public int BorderWidth
        {
            get { return leftBorder.Width; }
            set
            {
                leftBorder.Width = value;
                rightBorder.Width = value;
                topBorder.Height = value;
                bottomBorder.Height = value;

                bottomBorder.Width = Width + BorderWidth;
            }
        }

        public Color BackgroundColor
        {
            get { return background.Tint; }
            set { background.Tint = value; }
        }

        private int sidePadding;
        public int SidePadding
        {
            get { return sidePadding; }
            set { sidePadding = value; }
        }

        private int verticalPadding;
        public int VerticalPadding
        {
            get { return verticalPadding; }
            set { verticalPadding = value; }
        }
        #endregion

        public InputBox(Game game)
            : base(game)
        {

            background = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            cursor = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            leftBorder = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            leftBorder.XRelative = 0;
            rightBorder = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            topBorder = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            topBorder.YRelative = 0;
            bottomBorder = new Sprite(game, GraphicsCollection.GetPack("pixel"));

            text = new SpriteText(game, FontsCollection.GetPack("Courier New 14").Font);


            #region Default style
            VerticalPadding = 5;
            SidePadding = 5;
            TextColor = Color.Black;
            BorderColor = Color.Black;
            BorderWidth = 1;
            #endregion

            background.StackOrder = 0;
            leftBorder.StackOrder = rightBorder.StackOrder = topBorder.StackOrder = bottomBorder.StackOrder = 1;
            text.StackOrder = 3;
            text.XRelative = SidePadding;
            text.YRelative = VerticalPadding;

            //text used only for initialization, needed to measure text height
            text.Text = "Init";

            Height = text.Height + 2 * VerticalPadding;

            cursor.Tint = Color.Black;
            cursor.Height = text.Height;
            cursor.Width = 2;
            cursor.YRelative = VerticalPadding;
            cursor.XRelative = SidePadding;
            cursor.StackOrder = 4;
            cursor.AnimationSpeed = 10;
            cursor.Visible = false;
            cursor.OnFirstFrame += new EventHandler(cursor_OnFirstFrame);

            #region Component init
            Components.Add(background);
            Components.Add(cursor);
            //Components.Add(leftBorder);
            //Components.Add(rightBorder);
            Components.Add(topBorder);
            Components.Add(bottomBorder);
            Components.Add(text);

            UpdateComponentsX();
            UpdateComponentsY();
            #endregion

            text.Text = String.Empty;
        }

        void cursor_OnFirstFrame(object sender, EventArgs e)
        {
            if (this.IsSelected)
            {
                if (cursor.Visible)
                    cursor.Visible = false;
                else
                    cursor.Visible = true;
            }
        }

        public void BackSpace()
        {
            if (Text.Length > 0)
                Text = text.Text.Substring(0, text.Text.Length - 1);
        }

        public void InputKeyCode(Keys key)
        {
            if (key == Keys.Back)
            {
                BackSpace();
            }
            else
            {
                Text += TranslateKey(key);
            }
        }

        private String TranslateKey(Keys key)
        {
            switch (key)
            {
                //-------space si semne de punctuatie---------
                case Keys.Space:
                    return " ";
                case Keys.OemComma:
                    return ",";
                case Keys.OemPeriod:
                    return ";";
                case Keys.OemMinus:
                    return "-";
                //------cifre----------------
                case Keys.D0:
                    return "0";
                case Keys.D1:
                    return "1";
                case Keys.D2:
                    return "2";
                case Keys.D3:
                    return "3";
                case Keys.D4:
                    return "4";
                case Keys.D5:
                    return "5";
                case Keys.D6:
                    return "6";
                case Keys.D7:
                    return "7";
                case Keys.D8:
                    return "8";
                case Keys.D9:
                    return "9";
                //-------NumLock------------------------------
                case Keys.NumPad0:
                    return "0";
                case Keys.NumPad1:
                    return "1";
                case Keys.NumPad2:
                    return "2";
                case Keys.NumPad3:
                    return "3";
                case Keys.NumPad4:
                    return "4";
                case Keys.NumPad5:
                    return "5";
                case Keys.NumPad6:
                    return "6";
                case Keys.NumPad7:
                    return "7";
                case Keys.NumPad8:
                    return "8";
                case Keys.NumPad9:
                    return "9";
                //--------Letters-----------------------------------------------------
                case Keys.A:
                    return "A";
                case Keys.B:
                    return "B";
                case Keys.C:
                    return "C";
                case Keys.D:
                    return "D";
                case Keys.E:
                    return "E";
                case Keys.F:
                    return "F";
                case Keys.G:
                    return "G";
                case Keys.H:
                    return "H";
                case Keys.I:
                    return "I";
                case Keys.J:
                    return "J";
                case Keys.K:
                    return "K";
                case Keys.L:
                    return "L";
                case Keys.M:
                    return "M";
                case Keys.N:
                    return "N";
                case Keys.O:
                    return "O";
                case Keys.P:
                    return "P";
                case Keys.Q:
                    return "Q";
                case Keys.R:
                    return "R";
                case Keys.S:
                    return "S";
                case Keys.T:
                    return "T";
                case Keys.U:
                    return "U";
                case Keys.V:
                    return "V";
                case Keys.W:
                    return "W";
                case Keys.X:
                    return "X";
                case Keys.Y:
                    return "Y";
                case Keys.Z:
                    return "Z";
            }
            return String.Empty;
        }

        #region ISelectable Members

        private bool selected = false;

        public bool IsSelected
        {
            get { return selected; }
        }

        public void Select()
        {
            selected = true;
            cursor.Visible = true;
            cursor.AnimationSpeed = 10;
        }

        public void Unselect()
        {
            selected = false;
            cursor.Visible = false;
            cursor.AnimationSpeed = 0;
        }

        #endregion
    }
}
