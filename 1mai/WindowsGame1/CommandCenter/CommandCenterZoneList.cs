using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Operation_Cronos.IO;

namespace Operation_Cronos
{
    class CommandCenterZoneList : InputVisualComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;

        List<CommandCenterZoneButton> zones = new List<CommandCenterZoneButton>(15);

        CommandCenterZoneButton btnZone1;
        CommandCenterZoneButton btnZone2;
        CommandCenterZoneButton btnZone3;

        string zonePressed = "none";

        bool isHidden = false;

        Timer tmr_sleepTime;
        #endregion

        #region Events
        public event EventHandler<CommandCenterEventArgs> OnNewZoneActivated = delegate { };
        public event EventHandler<CommandCenterEventArgs> OnZoneDeactivated = delegate { };
        #endregion

        #region Properties
        public bool IsHidden
        {
            get
            {
                return isHidden;
            }
        }

        /// <summary>
        /// Shows if a zone is currently selected
        /// </summary>
        public bool ZoneIsSelected
        {
            get
            {
                if (zonePressed.Equals("none"))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Gets the Currently pressed Zone
        /// </summary>
        public CommandCenterZoneButton SelectedZone
        {
            get
            {
                if (ZoneIsSelected)
                {
                    int i = 0;
                    for (i = 0; i < zones.Count; i++)
                        if (zones[i].Name.Equals(zonePressed))
                            break;
                    return zones[i];
                }
                else
                    return null;
            }
        }
        #endregion

        #region Constructors
        public CommandCenterZoneList(Game game, int DrawOrder)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));

            //-----Zone Buttons-------
            btnZone1 = new CommandCenterZoneButton(game, new Sprite(game, graphicsCollection.GetPack("Zone")), new Sprite(game, graphicsCollection.GetPack("SelectedZone")), DrawOrder);
            btnZone1.Position = new Point(470, 190);
            btnZone1.Name = "ZONE 1";
            btnZone1.OnPress += new EventHandler<ButtonEventArgs>(Do_ZoneOnPress);
            btnZone1.Locked = false;
            btnZone1.DescriptionString = btnZone1.Name+ "\n\n\n\n  Tutorial zone for familiarizing with the game.";

            btnZone1.MissionBriefingString = " Mission briefing \n\n\n   You are in the year 2010 and you are able to travel through time between the years 1960 and 2010. You will be given some small tasks, in order to understand the game mechanics.";
            btnZone1.RewardsString = " Rewards \n\n\n\n This is a tutorial, there are no rewards for this zone.";
            btnZone1.ParametersString = " Parameters \n\n\n\n This is a tutorial, parameters are not yet implemented";
            AddChild(btnZone1);
            zones.Add(btnZone1);

            btnZone2 = new CommandCenterZoneButton(game, new Sprite(game, graphicsCollection.GetPack("Zone")), new Sprite(game, graphicsCollection.GetPack("SelectedZone")), DrawOrder);
            btnZone2.Position = new Point(210, 390);
            btnZone2.Name = "ZONE 2";
            btnZone2.Locked = false; //needs to be unlocked for the Strings to be set
            btnZone2.DescriptionString = btnZone2.Name+"\n Description";
            btnZone2.MissionBriefingString = btnZone2.Name + "\n Mission briefing";
            btnZone2.RewardsString = btnZone2.Name + "\n Rewards";
            btnZone2.Locked = true; //it is set back to locked==false;
            btnZone2.OnPress += new EventHandler<ButtonEventArgs>(Do_ZoneOnPress);
            AddChild(btnZone2);
            zones.Add(btnZone2);

            btnZone3 = new CommandCenterZoneButton(game, new Sprite(game, graphicsCollection.GetPack("Zone")), new Sprite(game, graphicsCollection.GetPack("SelectedZone")), DrawOrder);
            btnZone3.Position = new Point(500, 330);
            btnZone3.Name = "ZONE 3";
            btnZone3.Locked = false; //needs to be unlocked for the Strings to be set
            btnZone3.DescriptionString = btnZone3.Name + "\n Description";
            btnZone3.MissionBriefingString = btnZone3.Name + "\n Mission briefing";
            btnZone3.RewardsString = btnZone3.Name + "\n Rewards";
            btnZone3.Locked = true; //it is set back to locked==false;
            btnZone3.OnPress += new EventHandler<ButtonEventArgs>(Do_ZoneOnPress);
            AddChild(btnZone3);
            zones.Add(btnZone3);
            //-----------------------

            this.StackOrder = DrawOrder;

            //timer is used for the Submenu Close-then-Open effect, when switching between 2 different Zones
            tmr_sleepTime = new Timer(game);
            tmr_sleepTime.IntervalType = TimerIntervalType.Miliseconds;
            tmr_sleepTime.Interval = 250;
            tmr_sleepTime.OnTick += new EventHandler(tmr_sleepTime_OnTick);
        }
        #endregion

        #region Event Handlers

            #region Zone Buttons
            void Do_ZoneOnPress(object sender, ButtonEventArgs e)
            {
                if (!zonePressed.Equals(((CommandCenterZoneButton)sender).Name))
                {//the previosly pressed zone is not this zone (that generated the ZonePressEvent)

                    DeactivatePrevioslyPressedZone();

                    if (!zonePressed.Equals("none"))//this zone was not pressed first (when all zones were deactivated)
                    {
                        //launches event
                        OnZoneDeactivated(this, new CommandCenterEventArgs());

                        //on Tick, the timer will stop and the OnNewZoneActivated Event will be Launched
                        tmr_sleepTime.Start();
                    }
                    else //no other zone was active before this one was pressed, so the event launches right away
                    {
                        //launches event
                        OnNewZoneActivated(this, new CommandCenterEventArgs());
                    }
                    zonePressed = ((CommandCenterZoneButton)sender).Name;
                }
                else//this zone was previosly pressed
                {
                    zonePressed = "none";
                    ((CommandCenterZoneButton)sender).ReleaseZoneButton();

                    //launches event
                    OnZoneDeactivated(this, new CommandCenterEventArgs());
                }
            }
            #endregion

            #region Timer
            void tmr_sleepTime_OnTick(object sender, EventArgs e)
            {
                //launches event
                OnNewZoneActivated(this, new CommandCenterEventArgs());

                ((Timer)sender).Stop();
            }
            #endregion

        #endregion

        #region Methods
        /// <summary>
        /// HideSprites all the zones and deactivates the pressed one (if such)
        /// </summary>
        public void Hide()
        {
            for (int i = 0; i < zones.Count; i++)
            {
                zones[i].Hide();
            }

            DeactivatePrevioslyPressedZone();//in this order!
            zonePressed = "none";

            this.Enabled = false;
        }

        /// <summary>
        /// Shows all the zones and deactivates the pressed one (if such)
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < zones.Count; i++)
            {
                zones[i].Show();
            }

            DeactivatePrevioslyPressedZone();//in this order!
            zonePressed = "none";

            this.Enabled = true;
        }

        //Simulates a MouseLeave effect for the Zone that was previosly pressed,
        //when another zone was pressed
        void DeactivatePrevioslyPressedZone()
        {
            for (int i = 0; i < zones.Count; i++)
            {
                if (zonePressed.Equals(zones[i].Name))
                {
                    zones[i].DeactivateZoneButton();
                }
            }
        }

        /// <summary>
        /// Sets the text displayed after clicking the Parameters Button for a zone
        /// </summary>
        public void SetZoneParameters(List<CommandCenterEventArgs> zonesParameters)
        {
            for (int i = 0; i < zonesParameters.Count; i++)
            {
                CommandCenterEventArgs zone = zonesParameters[i];
                zones[i].ParametersString = zones[i].Name + "\n Parameters\n\n";
                zones[i].ParametersString += "\n      Energy                " + ((int)zone.Energy_Quantum).ToString() + "%";
                zones[i].ParametersString += "\n      Education          " + ((int)zone.Education_Quantum).ToString() + "%";
                zones[i].ParametersString += "\n      Economy            " + ((int)zone.Economy_Quantum).ToString() + "%";
                zones[i].ParametersString += "\n      Environment     " + ((int)zone.Environment_Quantum).ToString() + "%";
                zones[i].ParametersString += "\n      Health                " + ((int)zone.Health_Quantum).ToString() + "%";
                zones[i].ParametersString += "\n      Food                   " + ((int)zone.Food_Quantum).ToString() + "%";
            }
        }
        #endregion

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion
    }
}
