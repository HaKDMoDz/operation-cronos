using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;
using Operation_Cronos.Sound;

namespace Operation_Cronos.Display {
    public class LeftMenu : InputVisualComponent {

        Sprite frame;
        PanelButton minimize;
        ParametersPanel parameters;
        PanelButton alert;
        PanelButton mission;
        PanelButton control;
        PanelButton save;
        PanelButton exit;
        ControlPanel controlPanel;
        MissionPanel missionPanel;
        AlertPanel alertPanel;

        Tooltip tooltip = null;
        bool BlinkOn = true;

        private int buttonsX = 26;

        #region Properties
        private SoundManager SoundManager
        {
            get { return (SoundManager)Game.Services.GetService(typeof(SoundManager)); }
        }

        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }
        #endregion

        #region Events
        public event EventHandler On_SaveGame = delegate { };
        #endregion	

        public LeftMenu(Game game)
            : base(game) {
            frame = new Sprite(game, GraphicsCollection.GetPack("left-menu-frame"));
            frame.StackOrder = 1;

            AddChild(frame);

            minimize = new PanelButton(game, PanelButtonType.ResourcesMinimize);
            minimize.StackOrder = 2;
            minimize.XRelative = 60;
            minimize.YRelative = 11;
            minimize.OnMousePress += new EventHandler<Operation_Cronos.Input.MouseEventArgs>(minimize_OnMousePress);
            AddChild(minimize);

            parameters = new ParametersPanel(game);
            parameters.XRelative = ParametersPanel.ClosedPosition;
            parameters.YRelative = 15;
            parameters.StackOrder = 0;
            AddChild(parameters);

            alert = new PanelButton(game, PanelButtonType.ResourcesAlert);
            alert.XRelative = buttonsX;
            alert.YRelative = 25;
            alert.StackOrder = 3;
            alert.OnMouseOver += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseOver);
            alert.OnMouseLeave += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseLeave);
            alert.OnRelease += new EventHandler<ButtonEventArgs>(button_OnRelease);
            AddChild(alert);

            mission = new PanelButton(game, PanelButtonType.ResourcesMission);
            mission.StackOrder = 3;
            mission.XRelative = buttonsX;
            mission.YRelative = 60;
            mission.OnMouseOver += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseOver);
            mission.OnMouseLeave += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseLeave);
            mission.OnRelease += new EventHandler<ButtonEventArgs>(button_OnRelease);
            AddChild(mission);

            control = new PanelButton(game, PanelButtonType.ResourcesControl);
            control.StackOrder = 3;
            control.XRelative = buttonsX;
            control.YRelative = 89;
            control.OnMouseOver += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseOver);
            control.OnMouseLeave += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseLeave);
            control.OnRelease += new EventHandler<ButtonEventArgs>(button_OnRelease);
            AddChild(control);

            save = new PanelButton(game, PanelButtonType.ResourcesSave);
            save.StackOrder = 3;
            save.XRelative = buttonsX + 3;
            save.YRelative = 123;
            save.OnMouseOver += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseOver);
            save.OnMouseLeave += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseLeave);
            save.OnRelease += new EventHandler<ButtonEventArgs>(button_OnRelease);
            AddChild(save);

            exit = new PanelButton(game, PanelButtonType.ResourcesExit);
            exit.StackOrder = 3;
            exit.XRelative = buttonsX+2;
            exit.YRelative = 155;
            exit.OnMouseOver += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseOver);
            exit.OnMouseLeave += new EventHandler<ButtonEventArgs>(LeftButton_OnMouseLeave);
            exit.OnRelease+=new EventHandler<ButtonEventArgs>(button_OnRelease);
            AddChild(exit);

            #region ControlPanel
            controlPanel = new ControlPanel(game);
            controlPanel.StackOrder = 3;
            controlPanel.Visible = false;
            controlPanel.Enabled = false;
            AddChild(controlPanel);
            #endregion

            #region MissionPannel
            missionPanel = new MissionPanel(game);
            missionPanel.StackOrder = 3;
            missionPanel.XRelative = 300;
            missionPanel.YRelative = 200;
            missionPanel.Visible = false;
            missionPanel.Enabled = false;
            AddChild(missionPanel);
            #endregion

            #region AlertPannel
            alertPanel = new AlertPanel(game);
            alertPanel.StackOrder = 3;
            alertPanel.XRelative = 300;
            alertPanel.YRelative = 200;
            alertPanel.Visible = false;
            alertPanel.Enabled = false;
            alertPanel.On_Blink += new EventHandler(alertPanel_On_Blink);
            AddChild(alertPanel);
            #endregion

            tooltip = new Tooltip(game, 2);
            tooltip.XRelative = buttonsX + exit.Width + 5;
            tooltip.StackOrder = 2;
            tooltip.YRelative = 0;
            AddChild(tooltip);
            tooltip.Visible = false;
        }

        void button_OnRelease(object sender, ButtonEventArgs e)
        {
            GameMap.RemoveConstructionPanel();
            if (((PanelButton)sender).ButtonType == PanelButtonType.ResourcesExit)
            {
                ((DisplayManager)Game.Services.GetService(typeof(DisplayManager))).CameraFreeze();
                ((DisplayManager)Game.Services.GetService(typeof(DisplayManager))).ReturnToCommandCenter();
            }
            if (((PanelButton)sender).ButtonType == PanelButtonType.ResourcesMission)
            {
                controlPanel.IsVisible = false;
                controlPanel.Enabled = false;
                alertPanel.IsVisible = false;
                alertPanel.Enabled = false;
                

                ((GameInterface)Game.Services.GetService(typeof(GameInterface))).ShowCurrentMission();
                missionPanel.Text = "MISSION1: \nGhitza tre' sa mearga la Maria, sa o intrebe de sanatate, de moartea bunicii, sa-i arate ca-i pasa de moartea bunicii, dupa care sa o 'consoleze'.";
                missionPanel.SplitTextToRows(missionPanel.Width - 50);
                missionPanel.IsVisible = true;
                missionPanel.Enabled = true;
            }
            if (((PanelButton)sender).ButtonType == PanelButtonType.ResourcesAlert)
            {
                missionPanel.IsVisible = false;
                missionPanel.Enabled = false;
                controlPanel.IsVisible = false;
                controlPanel.Enabled = false;
                
                alertPanel.SplitTextToRows(alertPanel.Width - 50);
                alertPanel.IsVisible = true;
                alertPanel.Enabled = true;

                StopBlinking();
            }
            if (((PanelButton)sender).ButtonType == PanelButtonType.ResourcesControl)
            {
                missionPanel.IsVisible = false;
                missionPanel.Enabled = false;
                alertPanel.IsVisible = false;
                alertPanel.Enabled = false;

                controlPanel.IsVisible = true;
                controlPanel.Enabled = true;

                controlPanel.FirstView();
            }         
           
            if (((PanelButton)sender).ButtonType == PanelButtonType.ResourcesSave)
            {
                On_SaveGame(this, new EventArgs());
            }
        }

        void minimize_OnMousePress(object sender, Operation_Cronos.Input.MouseEventArgs e) {
            if (parameters.IsOpen)
            {
                parameters.SlideIn();
                SoundManager.PlaySound(Sounds.SlidingSoundShort);
            }
            else
            {
                parameters.SlideOut();
                SoundManager.PlaySound(Sounds.SlidingSoundShort);
            }
        }

        public void SetParameters(MilleniumGoalsSet data)
        {
            parameters.SetBars(data);
            controlPanel.UpdateYear();
        }

        public void UpdateLeftPanelsPosition(Rectangle screen)
        {
            controlPanel.XRelative = (int)(screen.Width / 2) - (controlPanel.Width / 2);
            controlPanel.YRelative = (int)(screen.Height / 2) - (controlPanel.Height / 2);

            missionPanel.XRelative = (int)(screen.Width / 2) - (missionPanel.Width / 2);
            missionPanel.YRelative = (int)(screen.Height / 2) - (missionPanel.Height / 2);

            alertPanel.XRelative = (int)(screen.Width / 2) - (alertPanel.Width / 2);
            alertPanel.YRelative = (int)(screen.Height / 2) - (alertPanel.Height / 2);

        }

        private void LeftButton_OnMouseOver(object sender, ButtonEventArgs args)
        {
            SoundManager.PlaySound(Sounds.LeftMenuButtons);

            switch (((PanelButton)sender).ButtonType)
            {
                case PanelButtonType.ResourcesAlert:
                    tooltip.Text = "Alert messages";
                    tooltip.YRelative = ((PanelButton)sender).YRelative - 3;
                    tooltip.IsVisible = true;
                    break;
                case PanelButtonType.ResourcesControl:
                    tooltip.Text = "Control panel";
                    tooltip.YRelative = ((PanelButton)sender).YRelative - 3;
                    tooltip.IsVisible = true;
                    break;
                case PanelButtonType.ResourcesMission:
                    tooltip.Text = "Mission briefing";
                    tooltip.YRelative = ((PanelButton)sender).YRelative - 3;
                    tooltip.IsVisible = true;
                    break;
                case PanelButtonType.ResourcesSave:
                    tooltip.Text = "Save game";
                    tooltip.YRelative = ((PanelButton)sender).YRelative - 3;
                    tooltip.IsVisible = true;
                    break;
                case PanelButtonType.ResourcesExit:
                    tooltip.Text = "Exit zone";
                    tooltip.YRelative = ((PanelButton)sender).YRelative - 3;
                    tooltip.IsVisible = true;
                    break;
            }
        }

        private void LeftButton_OnMouseLeave(object sender, ButtonEventArgs args)
        {
            tooltip.Text = "";
            tooltip.IsVisible = false;
        }

        public void SetAlertPanelText(string text)
        {
            alertPanel.SetText(text);
        }

        void alertPanel_On_Blink(object sender, EventArgs e)
        {
            if (BlinkOn)
            {
                alert.MouseOverAnimation();
                BlinkOn = false;
            }
            else
            {
                alert.MouseLeaveAnimation();
                BlinkOn = true;
            }
        }

        public void StopBlinking()
        {
            alert.MouseLeaveAnimation();
            alertPanel.StopBlinking();
        }

    }
}
