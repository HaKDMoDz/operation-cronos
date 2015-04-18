using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display
{
    public class ControlPanel:InputVisualComponent
    {
        Sprite frame;
        Sprite backgroundResearch;
        Sprite backgroundGraph;
        ControlPanelButton research;
        ControlPanelButton graph;
        ControlPanelButton close;
        ControlPanelButton environment;
        ControlPanelButton energy;
        ControlPanelButton education;
        ControlPanelButton economy;
        ControlPanelButton food;
        ControlPanelButton health;
        ResearchPanel researchPanel;
        GraphPanel graphPanel;
        List<ControlPanelButton> MGButtons;

        Tooltip tooltip = null;

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

        public ControlPanel(Game game)
            : base(game)
        {
            #region Base

            frame = new Sprite(game, GraphicsCollection.GetPack("control-frame"));
            AddChild(frame);
            
            backgroundResearch = new Sprite(game, GraphicsCollection.GetPack("control-button-background"));
            backgroundResearch.StackOrder = 1;
            backgroundResearch.XRelative = 18;
            backgroundResearch.YRelative = 65;
            AddChild(backgroundResearch);

            backgroundGraph = new Sprite(game, GraphicsCollection.GetPack("control-button-background"));
            backgroundGraph.StackOrder = 1;
            backgroundGraph.XRelative = 18;
            backgroundGraph.YRelative = 135;
            AddChild(backgroundGraph);
            #endregion

            MGButtons = new List<ControlPanelButton>();

            research = new ControlPanelButton(game, ControlPanelButtonType.Research);
            research.StackOrder = 3;
            research.XRelative = 31;
            research.YRelative = 142;
            research.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(research_OnMousePress);
            research.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            research.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(research);

            graph = new ControlPanelButton(game, ControlPanelButtonType.Graph);
            graph.StackOrder = 3;
            graph.XRelative = 24;
            graph.YRelative = 70;
            graph.OnMousePress+=new EventHandler<Operation_Cronos.Input.MouseEventArgs>(research_OnMousePress);
            graph.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            graph.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(graph);

            close = new ControlPanelButton(game, ControlPanelButtonType.Close);
            close.StackOrder = 3;
            close.XRelative = 430;
            close.YRelative = 7;
            close.OnMouseRelease += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(close_OnMousePress);
            AddChild(close);

            #region MGs
            economy = new ControlPanelButton(game, ControlPanelButtonType.Economy);
            economy.XRelative = 139;
            economy.YRelative = 15;
            economy.StackOrder = 3;
            economy.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            economy.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(economy);
            MGButtons.Add(economy);

            health = new ControlPanelButton(game, ControlPanelButtonType.Health);
            health.XRelative = 185;
            health.YRelative = 15;
            health.StackOrder = 3;
            health.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            health.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(health);
            MGButtons.Add(health);

            education = new ControlPanelButton(game, ControlPanelButtonType.Education);
            education.XRelative = 231;
            education.YRelative = 15;
            education.StackOrder = 3;
            education.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            education.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(education);
            MGButtons.Add(education);

            energy = new ControlPanelButton(game, ControlPanelButtonType.Energy);
            energy.XRelative = 278;
            energy.YRelative = 15;
            energy.StackOrder = 3;
            energy.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            energy.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(energy);
            MGButtons.Add(energy);

            food = new ControlPanelButton(game, ControlPanelButtonType.Food);
            food.XRelative = 324;
            food.YRelative = 15;
            food.StackOrder = 3;
            food.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            food.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(food);
            MGButtons.Add(food);

            environment = new ControlPanelButton(game, ControlPanelButtonType.Environment);
            environment.XRelative = 370;
            environment.YRelative = 15;
            environment.StackOrder = 3;
            environment.OnMouseOver += new EventHandler<ButtonEventArgs>(Button_OnMouseOver);
            environment.OnMouseLeave += new EventHandler<ButtonEventArgs>(Button_OnMouseLeave);
            AddChild(environment);
            MGButtons.Add(environment);

            economy.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(mgButton_OnMousePress);
            education.OnMousePress+=new EventHandler<Operation_Cronos.Input.MouseEventArgs>(mgButton_OnMousePress);
            energy.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(mgButton_OnMousePress);
            environment.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(mgButton_OnMousePress);
            food.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(mgButton_OnMousePress);
            health.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(mgButton_OnMousePress);
            #endregion

            researchPanel = new ResearchPanel(game);
            researchPanel.StackOrder = 1;
            researchPanel.XRelative = 0;
            AddChild(researchPanel);

            graphPanel = new GraphPanel(game);
            graphPanel.StackOrder = 1;
            AddChild(graphPanel);

            HideAll();
        }

        public void FirstView()
        {
            research_OnMousePress(graph, null);
        }

        void research_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            switch (((ControlPanelButton)sender).ButtonType)
            {
                case ControlPanelButtonType.Research:
                    HideAll();
                    researchPanel.IsVisible = true;
                    research.IsSelected = true;
                    graph.IsSelected = false;
                    researchPanel.Refresh(ConstructionType.Food);
                    mgButton_OnMousePress(food, null);
                    researchPanel.Enabled = true;
                    break;
                case ControlPanelButtonType.Graph:
                    HideAll();
                    graphPanel.IsVisible = true;
                    graphPanel.Enabled = true;
                    graphPanel.Refresh(ConstructionType.Food);
                    mgButton_OnMousePress(food, null);
                    research.IsSelected = false;
                    graph.IsSelected = true;
                    break;
            }
        }

        private void HideAll()
        {
            researchPanel.IsVisible = false;
            researchPanel.Enabled = false;
            graphPanel.IsVisible = false;
            graphPanel.Enabled = false;
        }

        void mgButton_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            for (int i = 0; i < MGButtons.Count; i++)
            {
                MGButtons[i].IsSelected = false;
            }
            ChangeView(((ControlPanelButton)sender).ButtonType);
            ((ControlPanelButton)sender).IsSelected = true;
        }

        void close_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            this.IsVisible = false;
            this.Enabled = false;
        }

        private void ChangeView(ControlPanelButtonType controlPanelButtonType)
        {
            switch (controlPanelButtonType)
            {
                case ControlPanelButtonType.Economy:
                    researchPanel.Refresh(ConstructionType.Economy);
                    graphPanel.Refresh(ConstructionType.Economy);
                    break;
                case ControlPanelButtonType.Environment:
                    researchPanel.Refresh(ConstructionType.Environment);
                    graphPanel.Refresh(ConstructionType.Environment);
                    break;
                case ControlPanelButtonType.Energy:
                    researchPanel.Refresh(ConstructionType.Energy);
                    graphPanel.Refresh(ConstructionType.Energy);
                    break;
                case ControlPanelButtonType.Education:
                    researchPanel.Refresh(ConstructionType.Education);
                    graphPanel.Refresh(ConstructionType.Education);
                    break;
                case ControlPanelButtonType.Food:
                    researchPanel.Refresh(ConstructionType.Food);
                    graphPanel.Refresh(ConstructionType.Food);
                    break;
                case ControlPanelButtonType.Health:
                    researchPanel.Refresh(ConstructionType.Health);
                    graphPanel.Refresh(ConstructionType.Health);
                    break;
            }
        }

        public void Close()
        {
            researchPanel.Clear();
            researchPanel.Refresh();
            close_OnMousePress(null, null);
        }

        public void UpdateYear()
        {
            Close();
        }

        private void Button_OnMouseOver(object sender, ButtonEventArgs args)
        {
            ControlPanelButton senderButton =  ((ControlPanelButton)sender);
            if (tooltip == null && senderButton.TooltipText != "")
            {
                tooltip = new Tooltip(this.Game, 2);
                tooltip.StackOrder = 3;
                tooltip.Visible = true;
                AddChild(tooltip);
            }
            if (senderButton.TooltipText != "")
            {
                tooltip.Text = " " + senderButton.TooltipText;
                if (senderButton.ButtonType == ControlPanelButtonType.Graph ||
                    senderButton.ButtonType == ControlPanelButtonType.Research)
                    tooltip.XRelative = backgroundGraph.XRelative + 5;
                else
                    tooltip.XRelative = senderButton.XRelative;

                tooltip.YRelative = senderButton.YRelative + ((ControlPanelButton)sender).Height + 7;
                tooltip.IsVisible = true;
            }
        }

        private void Button_OnMouseLeave(object sender, ButtonEventArgs args)
        {
            if (tooltip != null)
            {
                tooltip.IsVisible = false;
                tooltip.Text = "";
            }
        }
    }
}
