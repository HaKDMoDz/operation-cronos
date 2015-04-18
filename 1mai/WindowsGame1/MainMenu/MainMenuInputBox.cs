using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.IO;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Operation_Cronos
{
    public class MainMenuInputBox : InputVisualComponent
    {
        #region Fields
        private Sprite background;
        private Sprite cursor;

        private SpriteText text;
        SpriteFont textFont;

        private SpriteText message;
        SpriteFont messageFont;

        private int sidePadding;
        private int verticalPadding;
        #endregion

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
            }
        }

        public override int Height
        {
            get { return background.Height; }
            set
            {
                background.Height = value;
            }
        }

        public String Text
        {
            get { return text.Text; }
            set
            {
                text.Text = value;
                cursor.X = background.X + SidePadding + text.Width + 2;
                cursor.XRelative = background.XRelative + SidePadding + text.Width + 2;
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

        public Color BackgroundColor
        {
            get { return background.Tint; }
            set { background.Tint = value; }
        }

        public int SidePadding
        {
            get { return sidePadding; }
            set { sidePadding = value; }
        }

        public int VerticalPadding
        {
            get { return verticalPadding; }
            set { verticalPadding = value; }
        }
        #endregion

        #region Constructor
        public MainMenuInputBox(Game game, Sprite Background, Sprite Cursor,int DrawOrder,int[] position)
            : base(game)
        {
            VerticalPadding = 5;
            SidePadding = 5;

            background = Background;
            background.X = position[0];
            background.Y = position[1];
            background.XRelative = position[0];
            background.YRelative = position[1];
            background.Width = position[2];
            background.DrawOrder = DrawOrder;
            background.Tint = Color.Black;


            //adaptation for classes of the main menu, for they do not derive from VisualComponent
            textFont = ((FontsCollection)Game.Services.GetService(typeof(FontsCollection))).GetPack("MainMenu").Font;  
            //textFont = Game.Content.LoadSettings<SpriteFont>("MainMenu"); old loading version  
            //-----------------------------------             
            text = new SpriteText(game, textFont);
            text.DrawOrder = DrawOrder + 3;
            text.X = background.X + SidePadding;
            text.Y = background.Y + VerticalPadding;
            text.XRelative = background.XRelative + SidePadding;
            text.YRelative = background.YRelative + VerticalPadding;
            TextColor = Color.MediumSeaGreen;
            MaxLength = 18;

            //text used only for initialization, needed to measure text height
            text.Text = "Init";

            Height = text.Height + 2 * VerticalPadding;

            cursor = Cursor;
            cursor.Tint = Color.Chartreuse;
            cursor.Height = text.Height;
            cursor.Width = 2;
            cursor.Y = background.Y+VerticalPadding;
            cursor.X = background.X+SidePadding;
            cursor.YRelative = background.YRelative + VerticalPadding;
            cursor.XRelative = background.XRelative + SidePadding;
            cursor.DrawOrder = DrawOrder + 1;
            cursor.AnimationSpeed = 10;
            cursor.Visible = false;
            cursor.OnFirstFrame += new EventHandler(cursor_OnFirstFrame);            
            
            messageFont = ((FontsCollection)Game.Services.GetService(typeof(FontsCollection))).GetPack("MainMenuNewUserMessage").Font;  
            //messageFont = Game.Content.LoadSettings<SpriteFont>("MainMenuNewUserMessage"); old loading version        
            message = new SpriteText(game, messageFont);
            message.DrawOrder = DrawOrder + 3;
            message.X = background.X + SidePadding;
            message.Y = background.Y + VerticalPadding + Height;
            message.XRelative = background.XRelative + SidePadding;
            message.YRelative = background.YRelative + VerticalPadding + Height;
            message.Tint = Color.Chartreuse;
            message.Text = "Init";
                                   
            Components.Add(background);
            Components.Add(cursor);
            Components.Add(text);

            text.Text = String.Empty;
            Select();
        }
        #endregion

        #region Cursor Event Handler
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
        #endregion

        #region Methods

        #region Keyboard Methods
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

            if (message.Visible)
            {
                ClearMessage();
            }
        }

        private String TranslateKey(Keys key)
        {
            switch (key)
            {
                //-------space si semne de punctuatie---------
                case Keys.Space:
                    return " ";
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

        #endregion

        #region Select Methods

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

        public void Hide()
        {
            background.Visible = false;
            background.Enabled = false;
            text.Visible = false;
            text.Enabled = false;
            Text = String.Empty;
            cursor.Visible = false;
            cursor.Enabled = false;
            ClearMessage();
        }

        public void Show()
        {
            background.Visible = true;
            background.Enabled = true;
            text.Visible = true;
            text.Enabled = true;
            Text = String.Empty;
            cursor.Visible = true;
            cursor.Enabled = true;
        }

        public void PostMessage(String msg)
        {
            message.Text = msg;
            message.Visible = true;
            message.Enabled = true;
        }

        public void ClearMessage()
        {
            message.Visible = false;
            message.Enabled = false;
        }

        #endregion
     }
}