using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.Sound;

namespace Operation_Cronos.Display {

    public class BuildingTypeEventArgs : EventArgs {
        ConstructionType type;

        public ConstructionType Category {
            get { return type; }
        }

        public BuildingTypeEventArgs(ConstructionType categ) {
            type = categ;
        }
    }
    
    public class Minimap : InputVisualComponent {

        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }

        int buttonsY = 124;
        Sprite frame;
        Map map;
        BuildingTypeButton population;
        BuildingTypeButton environment;
        BuildingTypeButton economy;
        BuildingTypeButton health;
        BuildingTypeButton education;
        BuildingTypeButton power;
        BuildingTypeButton food;
        PanelButton minimize;
        List<BuildingTypeButton> buttons;
        ConstructionType selectedCategory;

        Tooltip tooltip = null;

        public event EventHandler<BuildingTypeEventArgs> OnCategorySelected = delegate { };
        public event EventHandler<BuildingTypeEventArgs> OnCategoryUnselected = delegate { };
        public event EventHandler OnMinimize = delegate { };

        #region Properties
        public override int Width {
            get {
                return frame.Width;
            }
        }

        public override int Height {
            get {
                return frame.Height;
            }
        }
        public ConstructionType BuildingCategory {
            get { return selectedCategory; }
        }
        #endregion

        public Minimap(Game game)
            : base(game) {
            frame = new Sprite(game, GraphicsCollection.GetPack("minimap-frame"));
            frame.StackOrder = 0;
            AddChild(frame);

            buttons = new List<BuildingTypeButton>(6);

            #region Buttons
            population = new BuildingTypeButton(game, ConstructionType.Population);
            population.StackOrder = 1;
            population.XRelative = 31;
            population.YRelative = buttonsY;
            population.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            population.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            
            economy = new BuildingTypeButton(game, ConstructionType.Economy);
            economy.StackOrder = 1;
            economy.XRelative = 65;
            economy.YRelative = buttonsY;
            economy.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            economy.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            health = new BuildingTypeButton(game, ConstructionType.Health);
            health.StackOrder = 1;
            health.XRelative = 94;
            health.YRelative = buttonsY+1;
            health.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            health.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            education = new BuildingTypeButton(game, ConstructionType.Education);
            education.StackOrder = 1;
            education.XRelative = 125;
            education.YRelative = buttonsY+2;
            education.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            education.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            power = new BuildingTypeButton(game, ConstructionType.Energy);
            power.StackOrder = 1;
            power.XRelative = 160;
            power.YRelative = buttonsY;
            power.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            power.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            food = new BuildingTypeButton(game, ConstructionType.Food);
            food.StackOrder = 1;
            food.XRelative = 187;
            food.YRelative = buttonsY-1;
            food.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            food.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            environment = new BuildingTypeButton(game, ConstructionType.Environment);
            environment.StackOrder = 1;
            environment.XRelative = 218;
            environment.YRelative = buttonsY;
            environment.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            environment.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);

            buttons.Add(population);
            buttons.Add(environment);
            buttons.Add(health);
            buttons.Add(education);
            buttons.Add(power);
            buttons.Add(food);
            buttons.Add(economy);

            foreach (BuildingTypeButton  button in buttons) {
                button.OnRelease += new EventHandler<ButtonEventArgs>(button_OnRelease);
                AddChild(button);
            }
            #endregion

            minimize = new PanelButton(game, PanelButtonType.Minimize);
            minimize.XRelative = 38;
            minimize.YRelative = 148;
            minimize.StackOrder = 2;
            minimize.OnPress += new EventHandler<ButtonEventArgs>(minimize_OnPress);

            AddChild(minimize);

            map = new Map(game);
            map.YRelative = 19;
            map.XRelative = 25;
            AddChild(map);

            tooltip = new Tooltip(game, 2);
            tooltip.XRelative = 0;
            tooltip.StackOrder = 2;
            tooltip.YRelative = buttonsY + 20;
            tooltip.Visible = false;
            AddChild(tooltip);
        }

        void minimize_OnPress(object sender, ButtonEventArgs e) {
            OnMinimize(this, new EventArgs());
            SoundManager.PlaySound(Sounds.SlidingSoundShort);
        }

        void button_OnRelease(object sender, ButtonEventArgs e) {
            BuildingTypeButton button = (BuildingTypeButton)sender;
            if (button.IsSelected) {
                button.Unselect();
                selectedCategory = ConstructionType.None;
                OnCategoryUnselected(this, new BuildingTypeEventArgs(button.Category));
            } else {
                foreach (BuildingTypeButton  b in buttons) {
                    b.Unselect();
                }
                button.Select();
                selectedCategory = button.Category;
                OnCategorySelected(this, new BuildingTypeEventArgs(button.Category));
            }
        }

        public void SelectBuildingCategory(ConstructionType category) {
            foreach (BuildingTypeButton b in buttons) {
                b.Unselect();
            }
            foreach (BuildingTypeButton  b in buttons) {
                if (b.Category == category) {
                    b.Select();
                    selectedCategory = category;
                    OnCategorySelected(this, new BuildingTypeEventArgs(category));
                    break;
                }
            }
        }

        public void UpdateSize(int width, int height)
        {
            map.UpdateSize(width, height);
            map.XRelative = (220 - map.Width) / 2 + 25;
        }

        public void UpdateCamera(Point position, Point size)
        {
            map.UpdateCamera(position, size);
        }

        public void DisplaySlots(List<Point> pos)
        {
            map.DisplaySlots(pos);
        }

        public void EraseSlots()
        {
            map.EraseSlots();
        }

        public void Button_OnMouseOver(object sender, ButtonEventArgs args)
        {
            BuildingTypeButton eventSender = (BuildingTypeButton)sender;

            tooltip.Text = eventSender.Category.ToString();
            if (eventSender.XRelative <= 125) //third button in row
            {
                tooltip.XRelative = eventSender.XRelative + eventSender.Width;
                tooltip.AlignTextToRight();
            }
            else
                tooltip.XRelative = (eventSender.XRelative + eventSender.Width + 15) - tooltip.Width;

            tooltip.IsVisible = true;
        }

        public void Button_OnMouseLeave(object sender, ButtonEventArgs args)
        {
            tooltip.Text = "";
            tooltip.IsVisible = false;
        }
    }
}
