using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {
    public class ResourceBar : VisualComponent{
        private const float MaxCoverWidth = 136f;
        private const int ZeroXPos = 146;

        Sprite fullBar;
        Sprite cover;
        int percent = 0;

        public int Percent {
            get { return percent; }
            set {
                if (value >= 0 && value <= 100)
                {
                    percent = value;
                    cover.Width = (int)(MaxCoverWidth / 100 * (100 - percent));
                    cover.XRelative = ZeroXPos - cover.Width;
                    UpdateComponentsX();
                }
                else
                {
                    if (value < 0)
                    {
                        percent = 0;
                        cover.Width = (int)(MaxCoverWidth / 100 * (100 - percent));
                        cover.XRelative = ZeroXPos - cover.Width;
                        UpdateComponentsX();
                    }
                    if (value > 100)
                    {
                        percent = 100;
                        cover.Width = (int)(MaxCoverWidth / 100 * (100 - percent));
                        cover.XRelative = ZeroXPos - cover.Width;
                        UpdateComponentsX();
                    }
                }
            }
        }

        public ResourceBar(Game game)
            : base(game) {
            fullBar = new Sprite(game, GraphicsCollection.GetPack("left-menu-bar"));
            fullBar.StackOrder = 0;
            AddChild(fullBar);

            cover = new Sprite(game, GraphicsCollection.GetPack("left-menu-barcover"));
            cover.YRelative = 4;
            cover.StackOrder = 1;
            AddChild(cover);
        }
    }
}
