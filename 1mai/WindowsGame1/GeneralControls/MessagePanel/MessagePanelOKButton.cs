using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;

namespace Operation_Cronos
{
    public class MessagePanelOKButton : Button 
    {
        
        public Sprite btnSprite;

        #region Properties
        /// <summary>
        /// Gets or sets the Width of the MessagePanelOKButton's Sprite
        /// </summary>
        public override int Width
        {
            get
            {
                return btnSprite.Width;
            }
            set
            {
                btnSprite.Width = value;
                base.Width = value;
            }
        }
        #endregion

        public MessagePanelOKButton(Game game)
            : base(game) 
        {
            
            btnSprite = new Sprite(game, GraphicsCollection.GetPack("ok_button"));
            btnSprite.XRelative = 0;
            btnSprite.YRelative = 0;
            btnSprite.StackOrder = 0;

            AddChild(btnSprite);
        }
        
        #region Overrides
        public override void MouseOverAnimation() 
        {
            btnSprite.FrameNumber = 1;
        }

        public override void MouseLeaveAnimation() 
        {
            btnSprite.FrameNumber = 0;
        }
        
        public override void PressAnimation() 
        {
            btnSprite.FrameNumber = 1;
        }

        public override void ReleaseAnimation() 
        {
            btnSprite.FrameNumber = 0;
        }
        #endregion
    }
}
