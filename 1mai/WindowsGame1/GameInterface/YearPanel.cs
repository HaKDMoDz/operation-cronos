using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos.Display
{
    public class YearPanel : InputVisualComponent
    {
        Sprite frame;
        PanelButton reset;
        SpriteText year;
        SpriteText population;
        SpriteText money;
        SpriteText historicalPeriod;
        Sprite gologan;
        Sprite smiley;

        public event EventHandler OnYearReset = delegate { };

        public override int Width
        {
            get
            {
                return frame.Width;
            }
        }

        public int Year
        {
            get { return Convert.ToInt32(year.Text); }
            set
            {
                year.Text = value.ToString();
                SetHistoricalPeriod();
            }
        }

        public int Population
        {
            get { return Convert.ToInt32(population.Text); }
            set { population.Text = value.ToString(); }
        }

        public int Money
        {
            set { money.Text = " " + value.ToString(); }
        }

        public YearPanel(Game game)
            : base(game)
        {
            frame = new Sprite(game, GraphicsCollection.GetPack("year-panel-frame"));
            frame.YRelative = -7;
            AddChild(frame);

            reset = new PanelButton(game, PanelButtonType.YearReset);
            reset.XRelative = 245;
            reset.YRelative = 9;
            reset.OnRelease += new EventHandler<ButtonEventArgs>(reset_OnRelease);
            AddChild(reset);

            year = new SpriteText(game, FontsCollection.GetPack("Trebuchet MS 14").Font);
            year.XRelative = 173;
            year.YRelative = 5;
            year.Text = "2010";
            AddChild(year);

            population = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            population.XRelative = 400;
            population.YRelative = 5;
            population.TextAlignment = Align.Right;
            AddChild(population);

            money = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            money.YRelative = 5;
            money.XRelative = 25;
            AddChild(money);

            historicalPeriod = new SpriteText(game, FontsCollection.GetPack("Calibri 8").Font);
            historicalPeriod.XRelative = 170;
            historicalPeriod.YRelative = 30;
            AddChild(historicalPeriod);

            gologan = new Sprite(game, GraphicsCollection.GetPack("gologan"));
            gologan.YRelative = 5;
            gologan.XRelative = 10;
            gologan.StackOrder = 1;

            AddChild(gologan);

            smiley = new Sprite(game, GraphicsCollection.GetPack("smiley"));
            smiley.XRelative = 280;
            smiley.YRelative = 5;
            AddChild(smiley);
        }

        void reset_OnRelease(object sender, ButtonEventArgs e)
        {
            if (e.Button == Operation_Cronos.Input.MouseButton.LeftButton)
            {
                OnYearReset(sender, e);
            }
        }

        private void SetHistoricalPeriod()
        {
            if (Year < (int)HistoricalPeriod.HP2_FirstYear)
                historicalPeriod.Text = "INDUSTRIAL AGE";
            else
                historicalPeriod.Text = "MODERN AGE";
        }

        public void ShowIfOverpopulated(int overpopulationValue)
        {
            if (overpopulationValue > 0)
                population.Tint = Color.Red;
            else
                population.Tint = Color.White;
        }
    }
}
