using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display
{
    public enum ConstructionPanelButtonType
    {
        Close,
        Destroy,
        Upgrade,
        Repair,
    }
    public class ConstructionPanelButton : Button
    {
        #region Fields
        Sprite buttonSprite;
        ConstructionPanelButtonType type;
        #endregion

        #region Properties
        public ConstructionPanelButtonType ButtonType
        {
            get { return type; }
        }
        #endregion

        public ConstructionPanelButton(Game game, ConstructionPanelButtonType type)
            : base(game)
        {
            this.type = type;

            switch (type)
            {
                case ConstructionPanelButtonType.Close:
                    buttonSprite = new Sprite(game, GraphicsCollection.GetPack("control-button-close"));
                    AddChild(buttonSprite);
                    break;
                case ConstructionPanelButtonType.Destroy:
                    buttonSprite = new Sprite(game, GraphicsCollection.GetPack("construction-panel-destroy"));
                    AddChild(buttonSprite);
                    break;
                case ConstructionPanelButtonType.Upgrade:
                    buttonSprite = new Sprite(game, GraphicsCollection.GetPack("construction-panel-upgrade"));
                    AddChild(buttonSprite);
                    break;
                case ConstructionPanelButtonType.Repair:
                    buttonSprite = new Sprite(game, GraphicsCollection.GetPack("construction-panel-repair"));
                    AddChild(buttonSprite);
                    break;
            }
        }
        public override void MouseOverAnimation()
        {
            buttonSprite.FrameNumber = 1;
        }
        public override void MouseLeaveAnimation()
        {
            buttonSprite.FrameNumber = 0;
        }
    }
}
