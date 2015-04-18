using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos.Display
{
    public class GraphBar : InputVisualComponent
    {
        Sprite visual;

        public int Percent
        {
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                visual.Height = value;
                visual.YRelative = 0 - value;
            }
        }

        public Color Tint
        {
            set { visual.Tint = value; }
        }

        public GraphBar(Game game)
            : base(game)
        {
            visual = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            AddChild(visual);
        }

        public GraphBar(Game game, int percent)
            : base(game)
        {
            visual = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            Percent = percent;
            AddChild(visual);
        }
    }
}
