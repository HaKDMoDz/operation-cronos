using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display {
    public class ParametersPanel : VisualComponent, ISliding{
        public const int OpenPosition = 98;
        public const int ClosedPosition = -60;

        private int gap;
        private int direction = 1;
        private int speed = 10;

        ResourceBar food;
        ResourceBar health;
        ResourceBar economy;
        ResourceBar education;
        ResourceBar environment;
        ResourceBar power;

        public bool IsOpen {
            get { return XRelative > ClosedPosition; }
        }

        public ParametersPanel(Game game)
            : base(game) {
            gap = 31;

            economy = new ResourceBar(game);

            health = new ResourceBar(game);
            health.YRelative = gap;

            education = new ResourceBar(game);
            education.YRelative = 2 * gap;

            power = new ResourceBar(game);
            power.YRelative = 3 * gap;

            food = new ResourceBar(game);
            food.YRelative = 4 * gap;

            environment = new ResourceBar(game);
            environment.YRelative = 5 * gap;

            AddChild(economy);
            AddChild(health);
            AddChild(education);
            AddChild(power);
            AddChild(food);
            AddChild(environment);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            XRelative += speed * direction;

            if (direction == 1) {
                if (XRelative + speed * direction > OpenPosition) {
                    direction = 0;
                    XRelative = OpenPosition;
                }
            }
            if (direction == -1) {
                if (XRelative + speed * direction < ClosedPosition) {
                    direction = 0;
                    XRelative = ClosedPosition;
                }
            }
        }

        public void SlideIn() {
            direction = -1;
        }

        public void SlideOut() {
            direction = 1;
        }

        public void SetBars(MilleniumGoalsSet data)
        {
            environment.Percent = (int)(data.Environment * 100);
            economy.Percent = (int)(data.Economy * 100);
            education.Percent = (int)(data.Education * 100);
            power.Percent = (int)(data.Energy * 100);
            health.Percent = (int)(data.Health * 100);
            food.Percent = (int)(data.Food * 100);
        }
    }
}
