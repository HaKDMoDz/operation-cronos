using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos.Display
{
    public class CameraRectangle:VisualComponent
    {
        List<Sprite> rectangle;
        public CameraRectangle(Game game)
            : base(game)
        {
            rectangle = new List<Sprite>(4);
            rectangle.Add(new Sprite(game, GraphicsCollection.GetPack("pixel")));
            rectangle.Add(new Sprite(game, GraphicsCollection.GetPack("pixel")));
            rectangle.Add(new Sprite(game, GraphicsCollection.GetPack("pixel")));
            rectangle.Add(new Sprite(game, GraphicsCollection.GetPack("pixel")));

            rectangle[0].Tint = Color.White;
            rectangle[1].Tint = Color.White;
            rectangle[2].Tint = Color.White;
            rectangle[3].Tint = Color.White;

            rectangle[0].XRelative = 0;
            rectangle[0].YRelative = 0;
            rectangle[1].YRelative = 0;
            rectangle[2].XRelative = 0;
            rectangle[3].YRelative = 0;
            rectangle[3].XRelative = 0;

            AddChild(rectangle[0]);
            AddChild(rectangle[1]);
            AddChild(rectangle[2]);
            AddChild(rectangle[3]);
        }

        public void UpdateSize(int w, int h)
        {
            rectangle[0].Width = w;
            rectangle[2].Width = w;
            rectangle[1].Height = h;
            rectangle[3].Height = h;
            rectangle[1].XRelative = w - 1;
            rectangle[2].YRelative = h - 1;
            UpdateComponentsX();
            UpdateComponentsY();
        }
    }

    public class Map : InputVisualComponent
    {
        Sprite background;
        List<Sprite> slots;
        CameraRectangle rectangle;
        int w, h;
        float ratio;

        public override int Width
        {
            get
            {
                return background.Width;
            }
        }

        public Map(Game game)
            : base(game)
        {
            background = new Sprite(game, GraphicsCollection.GetPack("pixel"));
            background.Tint = new Color(157, 125, 76);
            background.Width = 95;
            background.Height = 95;
            AddChild(background);

            slots = new List<Sprite>();

            rectangle = new CameraRectangle(game);
            rectangle.StackOrder = 2;
            AddChild(rectangle);
        }

        public void UpdateSize(int width, int height)
        {
            w = width;
            h = height;
            ratio = (float)background.Height / (float)height;
            background.Width = (int)((float)width * ratio);
        }

        public void UpdateCamera(Point position, Point size)
        {
            rectangle.XRelative = (int)((float)position.X * ratio);
            rectangle.YRelative = (int)((float)position.Y * ratio);
            rectangle.UpdateSize((int)((float)size.X * ratio), (int)((float)size.Y * ratio));
        }

        public void DisplaySlots(List<Point> pos)
        {
            foreach (Point p in pos)
            {
                Sprite dot = new Sprite(this.Game, GraphicsCollection.GetPack("pixel"));
                dot.Tint = Color.Lime;
                dot.Width = 3;
                dot.Height = 3;
                dot.XRelative = (int)((float)p.X * ratio)-1;
                dot.YRelative = (int)((float)p.Y * ratio)-1;
                dot.StackOrder = 1;
                slots.Add(dot);
                AddChild(slots[slots.Count-1]);
            }
        }

        public void EraseSlots()
        {
            while (slots.Count > 0)
            {
                RemoveChild(slots[0]);
                slots[0] = null;
                slots.RemoveAt(0);
            }
        }
    }
}


