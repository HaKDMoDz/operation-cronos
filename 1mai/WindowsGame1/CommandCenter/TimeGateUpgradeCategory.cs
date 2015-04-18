using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Operation_Cronos.Display;
using Operation_Cronos.IO;

namespace Operation_Cronos
{
    /// <summary>
    /// A collection of 4 TimeGateUpgradeLevel buttons
    /// </summary>
    class TimeGateUpgradeCategory: InputVisualComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;

        List<TimeGateUpgradeLevel> levels = new List<TimeGateUpgradeLevel>(4);

        TimeGateUpgradeLevel btnLevel1;
        TimeGateUpgradeLevel btnLevel2;
        TimeGateUpgradeLevel btnLevel3;
        TimeGateUpgradeLevel btnLevel4;

        #endregion

        #region Events
        public event EventHandler<ButtonEventArgs> OnLastLevelBought = delegate { };
        public event EventHandler<ButtonEventArgs> OnLevelBought = delegate { };
        #endregion

        #region Properties
        #endregion

        #region Constructors
        //positions is an 4-size Point vector containing the (x,y) positions of the 4 level buttons
        public TimeGateUpgradeCategory(Game game, int DrawOrder,Point[] positions)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));

            //-----Level Buttons-------
            btnLevel1 = new TimeGateUpgradeLevel(game, new Sprite(game, graphicsCollection.GetPack("Level1")), DrawOrder);
            btnLevel1.Position = positions[0];
            btnLevel1.Name = "Level1";
            btnLevel1.Status = TimeGateUpgradeStatus.Locked; 
            btnLevel1.OnPress += new EventHandler<ButtonEventArgs>(Do_LevelOnPress);
            AddChild(btnLevel1);
            levels.Add(btnLevel1);

            btnLevel2 = new TimeGateUpgradeLevel(game, new Sprite(game, graphicsCollection.GetPack("Level2")), DrawOrder);
            btnLevel2.Position = positions[1];
            btnLevel2.Name = "Level2";
            btnLevel2.Status = TimeGateUpgradeStatus.Locked; 
            btnLevel2.OnPress += new EventHandler<ButtonEventArgs>(Do_LevelOnPress);
            AddChild(btnLevel2);
            levels.Add(btnLevel2);

            btnLevel3 = new TimeGateUpgradeLevel(game, new Sprite(game, graphicsCollection.GetPack("Level3")), DrawOrder);
            btnLevel3.Position = positions[2];
            btnLevel3.Name = "Level3";
            btnLevel3.Status = TimeGateUpgradeStatus.Locked; 
            btnLevel3.OnPress += new EventHandler<ButtonEventArgs>(Do_LevelOnPress);
            AddChild(btnLevel3);
            levels.Add(btnLevel3);

            btnLevel4 = new TimeGateUpgradeLevel(game, new Sprite(game, graphicsCollection.GetPack("Level4")), DrawOrder);
            btnLevel4.Position = positions[3];
            btnLevel4.Name = "Level4";
            btnLevel4.Status = TimeGateUpgradeStatus.Locked; 
            btnLevel4.OnPress += new EventHandler<ButtonEventArgs>(Do_LevelOnPress);
            AddChild(btnLevel4);
            levels.Add(btnLevel4);
            //-----------------------

            this.StackOrder = DrawOrder;
        }
        #endregion

        #region Event Handlers

        void Do_LevelOnPress(object sender, ButtonEventArgs e)
        {
            switch (((TimeGateUpgradeLevel)sender).Name)
            {
                case "Level1":
                    if (btnLevel1.Status == TimeGateUpgradeStatus.Unlocked)
                    {
                        //daca isi permite sa cumpere
                        btnLevel1.Status = TimeGateUpgradeStatus.Bought;
                        OnLevelBought(this, new ButtonEventArgs());
                        btnLevel2.Status = TimeGateUpgradeStatus.Unlocked;
                    }
                    break;
                case "Level2":
                    if (btnLevel2.Status == TimeGateUpgradeStatus.Unlocked)
                    //si daca isi permite sa cumpere
                    {
                        btnLevel2.Status = TimeGateUpgradeStatus.Bought;
                        OnLevelBought(this, new ButtonEventArgs());
                        btnLevel3.Status = TimeGateUpgradeStatus.Unlocked;
                    }
                    break;
                case "Level3":
                    if (btnLevel3.Status == TimeGateUpgradeStatus.Unlocked)
                    //si daca isi permite sa cumpere
                    {
                        btnLevel3.Status = TimeGateUpgradeStatus.Bought;
                        OnLevelBought(this, new ButtonEventArgs());
                        btnLevel4.Status = TimeGateUpgradeStatus.Unlocked;
                    }
                    break;
                case "Level4":
                    if (btnLevel4.Status == TimeGateUpgradeStatus.Unlocked)
                    //si daca isi permite sa cumpere
                    {
                        btnLevel4.Status = TimeGateUpgradeStatus.Bought;
                        OnLevelBought(this, new ButtonEventArgs());
                        //Launches the OnLastLevelBought Event
                        OnLastLevelBought(this, new ButtonEventArgs());
                    }
                    break;
            }
        }          
        #endregion

        #region Methods
        /// <summary>
        /// HideSprites all the levels
        /// </summary>
        public void Hide()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].Hide();
            }
            this.Enabled = false;
        }

        /// <summary>
        /// Shows all the levels
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].Show();
            }
            this.Enabled = true;
        }

        /// <summary>
        /// Sets the status of the levels as desired (usefull when the upgrade levels status are loaded
        /// from a XML file)
        /// </summary>
        public void SetLevelsStatus(TimeGateUpgradeStatus level1, TimeGateUpgradeStatus level2, TimeGateUpgradeStatus level3, TimeGateUpgradeStatus level4)
        {
            btnLevel1.Status = level1;
            btnLevel2.Status = level2;
            btnLevel3.Status = level3;
            btnLevel4.Status = level4;
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
