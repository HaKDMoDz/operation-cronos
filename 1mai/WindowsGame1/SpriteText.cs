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
using Operation_Cronos.Display;



namespace Operation_Cronos
{
    public enum Align
    {
        Right,
        Left
    }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteText : DrawableGameComponent, IStackable, IPositionable
    {
        private SpriteFont font;
        private string text = string.Empty;
        private int x;
        private int y;
        private int xrel;
        private int yrel;
        private int stackOrder;
        private Color tint = Color.White;
        /// <summary>
        /// Default value = 100
        /// </summary>
        private int maxLength = 500;

        private Boolean isVisible;
        private Align align;


        #region Properties
        public Align TextAlignment
        {
            set { align = value; }
            get { return align; }
        }
        public int MaxLength 
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value.Length <= maxLength)
                {
                    text = value;
                }
            }
        }

        //-----------------------properties used to implement IStackable and IPositionable
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width
        {
            get { return (int)font.MeasureString(Text).X; }
            set { }
        }

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height
        {
            get { return (int)font.MeasureString(Text).Y; }
            set { }
        }

        /// <summary>
        /// The X location of the SpriteText relative to its parent.
        /// </summary>
        public int XRelative
        {
            get { return xrel; }
            set { xrel = value; }
        }

        /// <summary>
        /// The Y location of the SpriteText relative to its parent.
        /// </summary>
        public int YRelative
        {
            get { return yrel; }
            set { yrel = value; }
        }

        public Boolean Collides(Point position)
        {
            return false;
        }

        public Boolean CollidesTile(Point position)
        {
            return false;
        }

        public int StackOrder
        {
            get { return stackOrder; }
            set { stackOrder = value; }
        }

        public Color Tint {
            get { return tint; }
            set { tint = value; }
        }

        public Boolean IsVisible {
            get { return isVisible; }
            set { isVisible = value; Visible = value; }
        }
        //--------------------------------------------------------------
        #endregion

        /// <summary>
        /// SpriteText Constructor.
        /// </summary>
        /// <param name="game">The current game</param>
        /// <param name="_font">The font associated with the text.</param>
        public SpriteText(Game game, SpriteFont _font)
            : base(game) {
            font = _font;
            IsVisible = true;
            Game.Components.Add(this);
            align = Align.Left;
        }
        /// <summary>
        /// SpriteText Constructor.
        /// </summary>
        /// <param name="game">The current game.</param>
        /// <param name="x">The x coordonate of the SpriteText.</param>
        /// <param name="y">The y coordonate of the SpriteText.</param>
        /// <param name="_font">The font asociated to the text</param> 
        /// <param name="_text">The text that will be written</param> 
        public SpriteText(Game game, int _x, int _y, SpriteFont _font, string _text)
            : base(game)
        {
            x = _x;
            y = _y;
            this.font = _font;
            this.text = _text;
            Game.Components.Add(this);
            align = Align.Left;
        }

        #region SplitTextToRows

        /// <summary>
        /// Distributes the Text to rows, so that the SpriteText will fit into 
        /// a given width
        /// </summary>
        public void SplitTextToRows(int width)
        {
            //will contain the final string, in rows
            string initialString = this.Text;
            this.Text = "";

            //will contain the final string, split into rows, to fit the given width
            string finalString = "";
            //will contain a word from the initialString
            string wordString = "";
            //will contain a row of words from the initialString, fitting into the given width
            string rowString = "";

            //---counting the words of the initial text
            int wordCount = 0;

            for (int i = 0; i < initialString.Length; i++)
                if (initialString[i] == ' ')
                    wordCount++;

            if (initialString.Length > initialString.LastIndexOf(' '))
                //the text does not end with ' ', so there is one
                //more word left after the last ' '.
                wordCount++;
            //----------------------------------------

            //for each word in the initial text..
            for (int i = 0; i < wordCount; i++)
            {
                if (i < (wordCount - 1)) //if the current word is not the last word
                {
                    wordString = initialString.Substring(0, initialString.IndexOf(' '));
                }
                else//the current word is the last word
                {
                    wordString = initialString;
                }

                //if the word can be added to rowString without exceding the given width
                //then it is added and also removed from the initial string, along with the
                //' ' that precedes it
                this.Text = rowString + " " + wordString;

                if (this.Width <= width)
                {
                    if (wordString != "")
                        rowString += " ";
                    rowString += wordString;

                    if (i < (wordCount - 1)) //this is not the last word
                    {
                        //the word is removed from the initial string
                        initialString = initialString.Substring(initialString.IndexOf(' ') + 1);
                    }
                    else //this is the last word
                    {
                        //it is added to the final String
                        if (finalString != "")
                            finalString += "\n";
                        finalString += rowString;
                    }
                }
                //by adding the word, the rowString excedes the given width.
                //the word is not added to rowString, but rowString 
                //is added to the end of finalString
                else
                {
                    if (finalString != "")
                        finalString += "\n";
                    finalString += rowString;

                    //because the current word needs to be processed again
                    //and added to the next rowString
                    i--;

                    rowString = "";
                }
                this.Text = "";
            }
            this.Text = finalString;
        }
        #endregion

        #region Override
        public override void Draw(GameTime gameTime)
        {   
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            if (align == Align.Left)
            {
                spriteBatch.DrawString(font, text, new Vector2(x, y), tint);
            }
            else
            {
                spriteBatch.DrawString(font, text, new Vector2(x-Width, y), tint);
            }
            base.Draw(gameTime);
        }
        #endregion
    }
}