using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display
{
    public class ResearchPanel : InputVisualComponent
    {
        Sprite timeline;
        SpriteText name;
        SpriteText description;
        SpriteText price;
        SpriteText year;
        ControlPanelButton ok;
        Research selectedResearch;
        ConstructionType selectedMG;

        List<ControlPanelButton> icons;

        const int LeftPositioningLimit = 102;
        const int RightPositioningLimit = 404;

        #region Properties
        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }

        private GameManager GameManager
        {
            get { return (GameManager)Game.Services.GetService(typeof(GameManager)); }
        }
        #endregion

        public ResearchPanel(Game game)
            : base(game)
        {
            selectedResearch = null;
            selectedMG = ConstructionType.None;

            timeline = new Sprite(game, GraphicsCollection.GetPack("control-research-timeline"));
            timeline.XRelative = 91;
            timeline.YRelative = 197;
            AddChild(timeline);

            ok = new ControlPanelButton(game, ControlPanelButtonType.ResearchOK);
            ok.XRelative = 413;
            ok.YRelative = 64;
            ok.IsVisible = false;
            ok.Enabled = false;
            ok.OnPress += new EventHandler<ButtonEventArgs>(ok_OnPress);
            AddChild(ok);

            name = new SpriteText(game, FontsCollection.GetPack("Calibri 11").Font);
            name.Tint = Color.White;
            name.XRelative = 101;
            name.YRelative = 64;
            AddChild(name);

            description = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            description.Tint = Color.White;
            description.XRelative = 101;
            description.YRelative = 82;
            AddChild(description);

            price = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            price.Tint = Color.Lime;
            price.XRelative = 101;
            price.YRelative = 178;
            AddChild(price);

            year = new SpriteText(game, FontsCollection.GetPack("Calibri 10").Font);
            year.Tint = Color.Lime;
            year.XRelative = 347;
            year.YRelative = 178;
            AddChild(year);

            icons = new List<ControlPanelButton>();
        }

        void ok_OnPress(object sender, ButtonEventArgs e)
        {
            GameManager.CompleteResearch(selectedResearch);
            Refresh(selectedMG);
            Refresh(selectedResearch);
        }

        private void Refresh(Research research)
        {
            name.Text = research.Name;
            description.Text = research.Description;
            description.SplitTextToRows(RightPositioningLimit-LeftPositioningLimit);
            if (research.Completed == false || (research.Completed && research.YearCompleted > GameManager.CurrentYear))
            {
                price.Text = "$" + research.Cost.ToString();
                if (research.Cost <= GameManager.GetMoney(GameManager.CurrentYear))
                {
                    ok.IsVisible = this.IsVisible;
                    ok.Enabled = true;
                }
            }
            else
            {
                price.Text = "Researched: "+research.YearCompleted.ToString();
                ok.IsVisible = false;
                ok.Enabled = false;
            }
            year.Text = "Available: " + research.YearAvailable.ToString();
            selectedResearch = research;
        }

        public void Clear()
        {
            name.Text = "";
            description.Text = "";
            price.Text = "";
            ok.IsVisible = false;
            ok.Enabled = false;
            year.Text = "";
        }

        public void Refresh()
        {
            Refresh(selectedMG);
        }

        public void Refresh(ConstructionType mg)
        {
            Clear();
            selectedMG = mg;
            List<Research> list = GameManager.GetResearchList(mg);
            while (icons.Count > 0)
            {
                RemoveChild(icons[0]);
                icons.RemoveAt(0);
            }

            foreach (Research r in list)
            {
                //int range = ResearchPanel.RightPositioningLimit - ResearchPanel.LeftPositioningLimit;

                ////int travelRange = GameManager.EndTravelYear - GameManager.StartingTravelYear;
                ////float yearRatio = (float)(r.YearAvailable - GameManager.StartingTravelYear) / travelRange;

                //int travelRange = GameManager.EndTravelYear - 1800;
                //float yearRatio = (float)(r.YearAvailable - 1800) / travelRange;

                //float pos = yearRatio * range + LeftPositioningLimit;
                int pos = list.IndexOf(r) * (300 / (list.Count - 1)) + ResearchPanel.LeftPositioningLimit;
                if (r.YearAvailable <= GameManager.CurrentYear)
                {
                    ControlPanelButton icon;
                    if (!r.Completed || (r.Completed && r.YearCompleted > GameManager.CurrentYear))
                        icon = new ControlPanelButton(this.Game, ControlPanelButtonType.ResearchIcon);
                    else
                        icon = new ControlPanelButton(this.Game, ControlPanelButtonType.ResearchOK);
                    icon.StackOrder = 3;
                    icon.XRelative = (int)pos;
                    icon.YRelative = 206;
                    icon.Data = r;
                    icon.IsVisible = this.IsVisible;
                    icon.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(icon_OnMousePress);
                    AddChild(icon);
                    icons.Add(icon);
                }
                ok.IsVisible = this.IsVisible;
            }
            if (selectedResearch != null && selectedResearch.ResearchType == selectedMG)
            {
                Refresh(selectedResearch);
            }
            else
            {
                if (icons.Count > 0)
                {
                    Refresh((Research)icons[0].Data);
                }
            }
        }

        void icon_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e)
        {
            ControlPanelButton icon = (ControlPanelButton)sender;
            Refresh((Research)icon.Data);
        }
    }
}
