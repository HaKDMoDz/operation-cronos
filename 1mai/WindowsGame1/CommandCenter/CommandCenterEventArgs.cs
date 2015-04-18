using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos
{
    public class CommandCenterEventArgs:EventArgs
    {
        #region Fields

            #region Options Panel Settings

            private int resolutionIndex;
            private string fullScreen; //"On" or "Off"
            private int timeValueIndexSelected;//Autosave Time (index)
            private string difficultyLevel;
            private int volumeValue;//the volume Sprite frame number
            private string soundState; //"On" or "Off"
            private int cameraSpeed;//the camera Speed Sprite frame number

            private Point resolutionValue; //used when the OnEnterZone event launches
            private int timeValue;
            #endregion

            #region TimeGate Panel Settings
            private int[] timeGateUpgradeLevels;

            private int[] tgUpgradeCategories_Level;
            #endregion

            #region Zones

            private string zoneName = "";
            private double Energy;
            private double Education;
            private double Economy;
            private double Environment;
            private double Health;
            private double Food;
            #endregion

        #endregion

        #region Properties

            #region Options Panel
            /// <summary>
            /// Gets the Screen Resolution Index
            /// </summary>
            public int ResolutionIndex
            {
                get
                {
                    return resolutionIndex;
                }
            }

            /// <summary>
            /// Gets or sets the resolution value
            /// </summary>
            public Point ResolutionValue
            {
                get
                {
                    return resolutionValue;
                }
                set
                {
                    resolutionValue = value;
                }
            }

            /// <summary>
            /// Gets or sets the autosave time value
            /// </summary>
            public int AutosaveTimeValue
            {
                get
                {
                    return timeValue;
                }
                set
                {
                    timeValue = value;
                }
            }

            /// <summary>
            /// Gets the FullScreen option
            /// </summary>
            public string FullScreen
            {
                get
                {
                    return fullScreen;//return fullScreen.Equals("On") ? true : false;
                }
            }

            /// <summary>
            /// Gets the Index for the Time Values available for the Autosave section
            /// </summary>
            public int TimeValueIndex
            {
                get
                {
                    return timeValueIndexSelected;
                }
            }

            /// <summary>
            /// Gets the name of the radio button selected in the DifficultyLevel section
            /// </summary>
            public string DifficultyLevel
            {
                get
                {
                    return difficultyLevel;
                }
            }

            /// <summary>
            /// Gets the volume value (frame number) for the Audio section
            /// </summary>
            public int VolumeValue
            {
                get
                {
                    return volumeValue;
                }
            }

            /// <summary>
            /// Gets the Sound On/Off status for the Audio section
            /// </summary>
            public string SoundState
            {
                get
                {
                    return soundState;
                }
            }

            /// <summary>
            /// Gets the speed value (frame number) of the camera for the Video section
            /// </summary>
            public int CameraSpeed
            {
                get
                {
                    return cameraSpeed;
                }
            }
            #endregion

            #region TimeGate Panel
            /// <summary>
            /// Gets or sets the levels of the TimeGate upgrades in the TimeGate panel
            /// </summary>
            public int[] TimeGateUpgradeLevels
            {
                get
                {
                    return timeGateUpgradeLevels;
                }
                set
                {
                    timeGateUpgradeLevels = value;
                    SetTimeGateUpgradeLevels(value);
                }
            }

            /// <summary>
            /// Gets the int value of the current upgrade level for the TimeGate UpgradeCategory1
            /// </summary>
            public int TimeGateUpgradeCategory1_Level
            {
                get
                {
                    return tgUpgradeCategories_Level[0];
                }
            }

            /// <summary>
            /// Gets the int value of the current upgrade level for the TimeGate UpgradeCategory2
            /// </summary>
            public int TimeGateUpgradeCategory2_Level
            {
                get
                {
                    return tgUpgradeCategories_Level[1];
                }
            }

            /// <summary>
            /// Gets the int value of the current upgrade level for the TimeGate UpgradeCategory3
            /// </summary>
            public int TimeGateUpgradeCategory3_Level
            {
                get
                {
                    return tgUpgradeCategories_Level[2];
                }
            }

            /// <summary>
            /// Gets the int value of the current upgrade level for the TimeGate UpgradeCategory4
            /// </summary>
            public int TimeGateUpgradeCategory4_Level
            {
                get
                {
                    return tgUpgradeCategories_Level[3];
                }
            }
            #endregion

            #region Zones

            /// <summary>
            /// Gets or sets the entered zone's name
            /// </summary>
            public string ZoneName
            {
                get
                {
                    return zoneName;
                }
                set
                {
                    zoneName = value;
                }
            }

            public double Energy_Quantum
            {
                get
                {
                    return Energy;
                }
            }

            public double Education_Quantum
            {
                get
                {
                    return Education;
                }
            }

            public double Economy_Quantum
            {
                get
                {
                    return Economy;
                }
            }

            public double Environment_Quantum
            {
                get
                {
                    return Environment;
                }
            }

            public double Health_Quantum
            {
                get
                {
                    return Health;
                }
            }

            public double Food_Quantum
            {
                get
                {
                    return Food;
                }
            }
    
            #endregion

        #endregion

        #region Options Constructor
        /// <summary>
        /// Everithing is set to a given value
        /// </summary>
        /// <param name="_fullScreen">"On" or "Off"</param>
        /// <param name="_difficultyLevel">"Easy" or "Medium" or "Hard"</param>
        /// <param name="_soundState">"On" or "Off"</param>
        public CommandCenterEventArgs(int _resolutionIndex, string _fullScreen, int _timeValueIndex, string _difficultyLevel, int _volumeValue, string _soundState, int _cameraSpeed)
        {           
            fullScreen = _fullScreen;
            resolutionIndex = _resolutionIndex;
            timeValueIndexSelected = _timeValueIndex;
            difficultyLevel = _difficultyLevel;
            volumeValue = _volumeValue;
            soundState = _soundState;
            cameraSpeed = _cameraSpeed;
        }
            #endregion

        #region TimeGate Constructor
        public CommandCenterEventArgs(int timeGate1UpgradeLevel, int timeGate2UpgradeLevel, int timeGate3UpgradeLevel, int timeGate4UpgradeLevel)
        {
            timeGateUpgradeLevels = new int[4];
            timeGateUpgradeLevels[0] = timeGate1UpgradeLevel;
            timeGateUpgradeLevels[1] = timeGate2UpgradeLevel;
            timeGateUpgradeLevels[2] = timeGate3UpgradeLevel;
            timeGateUpgradeLevels[3] = timeGate4UpgradeLevel;
        }
        #endregion

        #region Zones Constructors

        public CommandCenterEventArgs(double _Energy_Quantum, double _Education_Quantum, double _Economy_Quantum, double _Environment_Quantum, double _Health_Quantum, double _Food_Quantum)
        {
            Energy = _Energy_Quantum;
            Education = _Education_Quantum;
            Economy = _Economy_Quantum;
            Environment = _Environment_Quantum;
            Health = _Health_Quantum;
            Food = _Food_Quantum;
        }
        #endregion

        /// <summary>
        /// Everithing is set to a default
        /// </summary>
        public CommandCenterEventArgs(int DefaultCategory)
        {
            if (DefaultCategory == 1)//uses the constructor to save Option Panel Settings
            {
                fullScreen = "Off";
                resolutionIndex = 0; //the first resolution in the list (OptionsPanel list)
                timeValueIndexSelected = 2;//by default, the game will be save every 30 minutes
                difficultyLevel = "Easy";
                volumeValue = 0;
                soundState = "On";
                cameraSpeed = 0;
            }
        }

        public CommandCenterEventArgs()
        {
        }

        /// <summary>
        /// Sets the actual values of the time gate upgrade categories levels
        /// </summary>
        private void SetTimeGateUpgradeLevels(int[] levels)
        {
            tgUpgradeCategories_Level = new int[4];
            for (int i = 0; i < 4; i++)
            {
                switch (levels[i])//Time Gate Upgrades Category i
                {
                    case 0://no level unlocked
                        tgUpgradeCategories_Level[i] = 0;
                        break;
                    case 1://level 1 unlocked
                        tgUpgradeCategories_Level[i] = 0;
                        break;
                    case 2://level 2 unlocked, level 1 bought
                        tgUpgradeCategories_Level[i] = (int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level1;
                        break;
                    case 3://level 3 unlocked, levels 1 and 2 bought
                        tgUpgradeCategories_Level[i] = (int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level2;
                        break;
                    case 4://level 4 unlocked, levels 1, 2 and 3 bought
                        tgUpgradeCategories_Level[i] = (int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level3;
                        break;
                    case 5://all levels bought
                        tgUpgradeCategories_Level[i] = (int)TimeGateUpgradeLevelsValues.UpgradeCategory1_Level4;
                        break;
                }
            }
        }
    }       
}
