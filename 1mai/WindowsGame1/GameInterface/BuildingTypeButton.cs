using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Input;

namespace Operation_Cronos.Display {
    
    public class BuildingTypeButton : Button, ISelectable {

        Sprite background;
        Boolean selected;
        ConstructionType category;

        public ConstructionType Category {
            get { return category; }
        }


        public BuildingTypeButton(Game game, ConstructionType categ)
            : base(game) {

            #region Decode
            category = categ;
            switch (categ) {
                case ConstructionType.Economy:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-dollar"));
                    break;
                case ConstructionType.Education:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-book"));
                    break;
                case ConstructionType.Environment:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-tree"));
                    break;
                case ConstructionType.Food:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-food"));
                    break;
                case ConstructionType.Health:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-cross"));
                    break;
                case ConstructionType.Energy:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-lightning"));
                    break;
                case ConstructionType.Population:
                    background = new Sprite(game, GraphicsCollection.GetPack("minimap-house"));
                    break;
            }
            #endregion

            AddChild(background);

            selected = false;
        }

        public override void MouseOverAnimation() {
            if (!IsSelected)
                background.FrameNumber = 1;
        }

        public override void MouseLeaveAnimation() {
            if (!IsSelected)
                background.FrameNumber = 0;
        }

        #region ISelectable Members

        public bool IsSelected {
            get { return selected; }
        }

        public void Select() {
            background.FrameNumber = 1;
            selected = true;
        }

        public void Unselect() {
            background.FrameNumber = 0;
            selected = false;
        }

        #endregion
    }
}