using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display
{
    public class MissionPanel : InputVisualComponent
    {
        Sprite frame;
        ControlPanelButton close;
        public SpriteText text;

        #region Properties
        public override int Width
        {
            get
            {
                return frame.Width;
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
                return frame.Height;
            }
            set
            {
                base.Height = value;
            }
        }
        #endregion

        public MissionPanel(Game game)
            : base(game)
        {
            frame = new Sprite(game, GraphicsCollection.GetPack("mission-frame"));
            AddChild(frame);

            close = new ControlPanelButton(game, ControlPanelButtonType.Close);
            close.StackOrder = 3;
            close.XRelative = 428;
            close.YRelative = 7;
            close.OnMousePress+=new EventHandler<Operation_Cronos.Input.MouseEventArgs>(close_OnMousePress);
            AddChild(close);

            text = new SpriteText(game, FontsCollection.GetPack("Calibri 8").Font);
            text.StackOrder = 3;
            text.MaxLength = 350;
            text.XRelative = 30;
            text.YRelative = 50;
            AddChild(text);
        }

        public string Text
        {
            get{ return text.Text;}
            set{ text.Text = value;}
        }

        public void SplitTextToRows(int width)
        {
            text.SplitTextToRows(width);
        }

        void close_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            this.IsVisible = false;
            this.Enabled = false;
        }

        public void Close()
        {
            close_OnMousePress(null, null);
        }

        public void UpdateYear()
        {
            Close();
        }
    }
}
