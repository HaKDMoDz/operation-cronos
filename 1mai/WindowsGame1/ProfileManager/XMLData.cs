using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using Operation_Cronos.Display;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Profiles
{
    public class XMLData : GameComponent
    {
        XElement xmlLoader;
        XmlDocument xmlSaver;

        String SettingsFile = "Settings.xml";
        String TimeGateLevelsFile = "TimeGateLevels.xml";
        String ZonesFile = "Zones.xml";
        String ZonesHistoryFile = "History.xml";
        String ResearchFile = "Research.xml";

        List<CommandCenterEventArgs> zoneEventArgs;

        #region Properties
        private GameMap GameMap
        {
            get { return (GameMap)Game.Services.GetService(typeof(GameMap)); }
        }
        #endregion

        public XMLData(Game game)
            :base(game)
        {            
            game.Components.Add(this);
        }

        #region Settings
        /// <summary>
        /// Reads the 'Settings.xml' file from the given profile and returns the content
        /// </summary>
        public CommandCenterEventArgs LoadSettings(string profilePath)
        {
            xmlLoader = XElement.Load(profilePath + "\\" + SettingsFile);

            int TimeValueIndex = Convert.ToInt32(xmlLoader.Element("TimeValueIndex").Value);
            string DifficultyLevel = xmlLoader.Element("DifficultyLevel").Value;
            int ResolutionValueIndex = Convert.ToInt32(xmlLoader.Element("ResolutionValueIndex").Value);
            string FullScreenState = xmlLoader.Element("FullScreenState").Value;
            int VolumeValue = Convert.ToInt32(xmlLoader.Element("VolumeValue").Value);
            string SoundState = xmlLoader.Element("SoundState").Value;
            int CameraSpeed = Convert.ToInt32(xmlLoader.Element("CameraSpeed").Value);

            CommandCenterEventArgs args = new CommandCenterEventArgs(ResolutionValueIndex, FullScreenState, TimeValueIndex, DifficultyLevel, VolumeValue,SoundState,CameraSpeed);
            return args;
        }

        /// <summary>
        /// Saves the settings to the 'Settings.xml' file from the given profile
        /// </summary>
        public void SaveSettings(string profilePath, CommandCenterEventArgs settings)
        {
            xmlSaver = new XmlDocument();
            XmlNode node;

            XmlNode root = xmlSaver.CreateElement("root");
            xmlSaver.AppendChild(root);

            node = xmlSaver.CreateElement("TimeValueIndex");
            node.AppendChild(xmlSaver.CreateTextNode(settings.TimeValueIndex.ToString()));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("DifficultyLevel");
            node.AppendChild(xmlSaver.CreateTextNode(settings.DifficultyLevel));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("ResolutionValueIndex");
            node.AppendChild(xmlSaver.CreateTextNode(settings.ResolutionIndex.ToString()));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("FullScreenState");
            node.AppendChild(xmlSaver.CreateTextNode(settings.FullScreen));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("VolumeValue");
            node.AppendChild(xmlSaver.CreateTextNode(settings.VolumeValue.ToString()));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("SoundState");
            node.AppendChild(xmlSaver.CreateTextNode(settings.SoundState));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("CameraSpeed");
            node.AppendChild(xmlSaver.CreateTextNode(settings.CameraSpeed.ToString()));
            root.AppendChild(node);

            xmlSaver.Save(profilePath + "\\" + SettingsFile);
        }
        #endregion

        #region Zone Parameters
        /// <summary>
        /// Reads the 'Zones.xml' files from the given profile and returns the content
        /// </summary>
        public List<CommandCenterEventArgs> LoadZonesParameters(string profilePath)
        {
            zoneEventArgs = new List<CommandCenterEventArgs>();

            xmlLoader = XElement.Load(profilePath + "\\" + ZonesFile);
            IEnumerable<XElement> zones = from element in xmlLoader.Elements() select element;

            foreach (XElement zone in zones)
            {
                double Energy_Quantum = Convert.ToDouble(zone.Element("Energy_Quantum").Value);
                Energy_Quantum = Energy_Quantum * 100;
                if (Energy_Quantum > 100)
                    Energy_Quantum = 100;
                if (Energy_Quantum < 0)
                    Energy_Quantum = 0;
                double Education_Quantum = Convert.ToDouble(zone.Element("Education_Quantum").Value);
                Education_Quantum = Education_Quantum * 100;
                if (Education_Quantum > 100)
                    Education_Quantum = 100;
                if (Education_Quantum < 0)
                    Education_Quantum = 0;
                double Economy_Quantum = Convert.ToDouble(zone.Element("Economy_Quantum").Value);
                Economy_Quantum = Economy_Quantum * 100;
                if (Economy_Quantum > 100)
                    Economy_Quantum = 100;
                if (Economy_Quantum < 0)
                    Economy_Quantum = 0;
                double Environment_Quantum = Convert.ToDouble(zone.Element("Environment_Quantum").Value);
                Environment_Quantum = Environment_Quantum * 100;
                if (Environment_Quantum > 100)
                    Environment_Quantum = 100;
                if (Environment_Quantum < 0)
                    Environment_Quantum = 0;
                double Health_Quantum = Convert.ToDouble(zone.Element("Health_Quantum").Value);
                Health_Quantum = Health_Quantum * 100;
                if (Health_Quantum > 100)
                    Health_Quantum = 100;
                if (Health_Quantum < 0)
                    Health_Quantum = 0;
                double Food_Quantum = Convert.ToDouble(zone.Element("Food_Quantum").Value);
                Food_Quantum = Food_Quantum * 100;
                if (Food_Quantum > 100)
                    Food_Quantum = 100;
                if (Food_Quantum < 0)
                    Food_Quantum = 0;

                CommandCenterEventArgs zoneArgs = new CommandCenterEventArgs(Energy_Quantum,Education_Quantum,Economy_Quantum,Environment_Quantum,Health_Quantum,Food_Quantum);
                zoneEventArgs.Add(zoneArgs);
            }
            return zoneEventArgs;
        }

        public void SaveZoneParameters(string profilePath, MilleniumGoalsSet mg)
        {
            zoneEventArgs = new List<CommandCenterEventArgs>();

            xmlSaver = new XmlDocument();
            XmlNode nodeZone;
            XmlNode node;

            XmlNode root = xmlSaver.CreateElement("root");
            xmlSaver.AppendChild(root);

            for (int i = 0; i < 3; i++)
            {
                nodeZone = xmlSaver.CreateElement("Zone"+i.ToString());
                root.AppendChild(nodeZone);

                node = xmlSaver.CreateElement("Energy_Quantum");
                node.AppendChild(xmlSaver.CreateTextNode(mg.Energy.ToString()));
                nodeZone.AppendChild(node);

                node = xmlSaver.CreateElement("Education_Quantum");
                node.AppendChild(xmlSaver.CreateTextNode(mg.Education.ToString()));
                nodeZone.AppendChild(node);

                node = xmlSaver.CreateElement("Economy_Quantum");
                node.AppendChild(xmlSaver.CreateTextNode(mg.Economy.ToString()));
                nodeZone.AppendChild(node);

                node = xmlSaver.CreateElement("Environment_Quantum");
                node.AppendChild(xmlSaver.CreateTextNode(mg.Environment.ToString()));
                nodeZone.AppendChild(node);

                node = xmlSaver.CreateElement("Health_Quantum");
                node.AppendChild(xmlSaver.CreateTextNode(mg.Health.ToString()));
                nodeZone.AppendChild(node);

                node = xmlSaver.CreateElement("Food_Quantum");
                node.AppendChild(xmlSaver.CreateTextNode(mg.Food.ToString()));
                nodeZone.AppendChild(node);
            }

            xmlSaver.Save(profilePath + "\\" + ZonesFile);
        }
        #endregion

        #region TimeGate Upgrade Levels
        /// <summary>
        /// Reads the 'TimeGateLevels.xml' files from the given profile and returns the content
        /// </summary>
        public CommandCenterEventArgs LoadTimeGateUpgradeLevels(string profilePath)
        {
            xmlLoader = XElement.Load(profilePath + "\\" + TimeGateLevelsFile);

            //<TimeGate1>0</TimeGate1>
            //means that there is no unlocked level in the first TimeGateUpgrade Category
            //<TimeGate1>1</TimeGate1>
            //means that the first unlocked level in the first TimeGateUpgrade Category is level1
            //but no level is bought in this category, as level 1 is the first level
            //<TimeGate1>2</TimeGate1>
            //means that the first unlocked level in the first TimeGateUpgrade Category is level2
            //and that level1 (the previous level) is bought
            //...
            //<TimeGate1>5</TimeGate1>
            //means that all levels have been bought in the first TimeGateUpgrade Category 

            int level1 = Convert.ToInt32(xmlLoader.Element("TimeGate1").Value);
            int level2 = Convert.ToInt32(xmlLoader.Element("TimeGate2").Value);
            int level3 = Convert.ToInt32(xmlLoader.Element("TimeGate3").Value);
            int level4 = Convert.ToInt32(xmlLoader.Element("TimeGate4").Value);

            CommandCenterEventArgs timeGateArgs = new CommandCenterEventArgs(level1, level2, level3, level4);
            return timeGateArgs;
        }

        public void SaveTimeGateUpgradeLevels(string profilePath, CommandCenterEventArgs levels)
        {
            xmlSaver = new XmlDocument();
            XmlNode node;

            XmlNode root = xmlSaver.CreateElement("root");
            xmlSaver.AppendChild(root);

            node = xmlSaver.CreateElement("TimeGate1");
            node.AppendChild(xmlSaver.CreateTextNode(levels.TimeGateUpgradeLevels[0].ToString()));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("TimeGate2");
            node.AppendChild(xmlSaver.CreateTextNode(levels.TimeGateUpgradeLevels[1].ToString()));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("TimeGate3");
            node.AppendChild(xmlSaver.CreateTextNode(levels.TimeGateUpgradeLevels[2].ToString()));
            root.AppendChild(node);

            node = xmlSaver.CreateElement("TimeGate4");
            node.AppendChild(xmlSaver.CreateTextNode(levels.TimeGateUpgradeLevels[3].ToString()));
            root.AppendChild(node);          

            xmlSaver.Save(profilePath + "\\" + TimeGateLevelsFile);
        }
        #endregion

        #region History
        public void SaveZoneHistory(String profilePath, List<Slot> _slots)
        {
            xmlSaver = new XmlDocument();
            XmlNode slot;
            XmlNode slotData;
            XmlNode res;
            XmlNode resData;

            XmlNode root = xmlSaver.CreateElement("root");
            xmlSaver.AppendChild(root);

            List<Slot> slotList = _slots;//GameMap.SlotList;

            for (int i = 0; i < slotList.Count; i++)
            {
                bool atLeastOneReservation = false;
                slot = null;

                for (int year = (int)HistoricalPeriod.HP1_FirstYear; year <= (int)HistoricalPeriod.HP2_LastYear; year++)
                {
                    Reservation reservation = slotList[i].GetReservation(year);

                    if (reservation != null)
                    {
                        if (reservation.StartingYear == year)
                        {
                            if (atLeastOneReservation == false)
                            {
                                slot = xmlSaver.CreateElement("slot");
                                root.AppendChild(slot);

                                slotData = xmlSaver.CreateElement("X");
                                slotData.AppendChild(xmlSaver.CreateTextNode(slotList[i].XSlotCenter.ToString()));
                                slot.AppendChild(slotData);

                                slotData = xmlSaver.CreateElement("Y");
                                slotData.AppendChild(xmlSaver.CreateTextNode(slotList[i].YSlotCenter.ToString()));
                                slot.AppendChild(slotData);

                                slotData = xmlSaver.CreateElement("constructionType");
                                slotData.AppendChild(xmlSaver.CreateTextNode(slotList[i].ConstructionType.ToString()));
                                slot.AppendChild(slotData);


                                atLeastOneReservation = true;
                            }
                            res = xmlSaver.CreateElement("reservation");
                            if (slot != null)
                                slot.AppendChild(res);

                            resData = xmlSaver.CreateElement("constructionName");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.ConstructionName.ToString()));
                            res.AppendChild(resData);

                            resData = xmlSaver.CreateElement("startingYear");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.StartingYear.ToString()));
                            res.AppendChild(resData);

                            resData = xmlSaver.CreateElement("duration");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.Duration.ToString()));
                            res.AppendChild(resData);

                            resData = xmlSaver.CreateElement("upgradeYear");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.UpgradeYear.ToString()));
                            res.AppendChild(resData);

                            resData = xmlSaver.CreateElement("isUpgraded");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.IsUpgraded.ToString()));
                            res.AppendChild(resData);

                            resData = xmlSaver.CreateElement("isPopulationUpgraded");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.IsPopulationUpgraded.ToString()));
                            res.AppendChild(resData);

                            resData = xmlSaver.CreateElement("isDestroyed");
                            resData.AppendChild(xmlSaver.CreateTextNode(reservation.IsDestroyed.ToString()));
                            res.AppendChild(resData);
                        }
                    }
                }
            }

            xmlSaver.Save(profilePath + "\\" + ZonesHistoryFile);
        }

        public List<Slot> LoadZoneHistory(String profilePath)
        {
            List<Slot> slotList = new List<Slot>();

            xmlLoader = XElement.Load(profilePath + "\\" + ZonesHistoryFile);
            IEnumerable<XElement> slotListData = from element in xmlLoader.Elements() select element;

            int xSlotCenter = 0;
            int ySlotCenter = 0;
            string constructionType = "";
            Slot currentSlot = null;
            Reservation currentReservation = null;

            foreach (XElement slot in slotListData)
            {
                xSlotCenter = Convert.ToInt32(slot.Element("X").Value);
                ySlotCenter = Convert.ToInt32(slot.Element("Y").Value);
                constructionType = slot.Element("constructionType").Value.ToString();
                currentSlot = new Slot(this.Game, (ConstructionType)Enum.Parse(typeof(ConstructionType), constructionType));
                currentSlot.XSlotCenter = xSlotCenter;
                currentSlot.YSlotCenter = ySlotCenter;
                IEnumerable<XElement> slotReservations = from element in slot.Elements("reservation") select element;

                foreach (XElement reservation in slotReservations)
                {
                    string constructionName = reservation.Element("constructionName").Value.ToString();
                    Construction construction = (Construction)Enum.Parse(typeof(Construction), constructionName);
                    int startingYear = Convert.ToInt32(reservation.Element("startingYear").Value);
                    int duration = Convert.ToInt32(reservation.Element("duration").Value);
                    int upgradeYear = Convert.ToInt32(reservation.Element("upgradeYear").Value);
                    bool isUpgraded = Convert.ToBoolean(reservation.Element("isUpgraded").Value);
                    bool isPopulationUpgraded = Convert.ToBoolean(reservation.Element("isPopulationUpgraded").Value);
                    bool isDestroyed = Convert.ToBoolean(reservation.Element("isDestroyed").Value);

                    currentReservation = new Reservation(startingYear, duration, construction);
                    currentReservation.UpgradeYear = upgradeYear;
                    currentReservation.IsUpgraded = isUpgraded;
                    currentReservation.IsPopulationUpgraded = isPopulationUpgraded;
                    currentReservation.IsDestroyed = isDestroyed;

                    currentSlot.MakeReservation(currentReservation);
                    slotList.Add(currentSlot);
                }
            }
                
            return slotList;
        }
        #endregion

        #region Research
        public void SaveZoneResearch(string profilePath, List<Research> researchList)
        {
            xmlSaver = new XmlDocument();
            XmlNode nodeResearch;
            XmlNode node;

            XmlNode root = xmlSaver.CreateElement("root");
            xmlSaver.AppendChild(root);

            for (int i = 0; i < researchList.Count;i++)
            {
                nodeResearch = xmlSaver.CreateElement("research");
                root.AppendChild(nodeResearch);

                node = xmlSaver.CreateElement("year");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].YearAvailable.ToString()));
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("completionYear");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].YearCompleted.ToString()));
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("bonus");
                ConstructionType c = researchList[i].ResearchType;
                switch (c)
                {
                    case ConstructionType.Economy:
                        node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Bonus.Economy.ToString()));
                        break;
                    case ConstructionType.Education:
                        node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Bonus.Education.ToString()));                        
                        break;
                    case ConstructionType.Environment:
                        node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Bonus.Environment.ToString()));            
                        break;
                    case ConstructionType.Food:
                        node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Bonus.Food.ToString()));                                    
                        break;
                    case ConstructionType.Health:
                        node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Bonus.Health.ToString()));                                                            
                        break;
                    case ConstructionType.Energy:
                        node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Bonus.Energy.ToString()));                                                                                    
                        break;
                }
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("cost");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Cost.ToString()));
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("name");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Name.ToString()));
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("description");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Description.ToString()));
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("ResearchType");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].ResearchType.ToString()));
                nodeResearch.AppendChild(node);

                node = xmlSaver.CreateElement("done");
                node.AppendChild(xmlSaver.CreateTextNode(researchList[i].Completed.ToString()));
                nodeResearch.AppendChild(node);
            }

            xmlSaver.Save(profilePath + "\\" + ResearchFile);
        }

        public List<Research> LoadResearchList(string profilePath)
        {
            List<Research> researchList = new List<Research>();

            xmlLoader = XElement.Load(profilePath + "\\" + ResearchFile);
            IEnumerable<XElement> researchListData = from element in xmlLoader.Elements() select element;

            Research currentResearch = null;

            foreach (XElement researchElement in researchListData)
            {
                currentResearch = new Research();
                currentResearch.YearAvailable = Convert.ToInt32(researchElement.Element("year").Value);
                currentResearch.YearCompleted = Convert.ToInt32(researchElement.Element("completionYear").Value);
                currentResearch.ResearchType = (ConstructionType)Enum.Parse(typeof(ConstructionType),researchElement.Element("ResearchType").Value);
                currentResearch.Bonus = new MilleniumGoalsSet(1, 1, 1, 1, 1, 1);
                float bonus = (float)Convert.ToDouble(researchElement.Element("bonus").Value);
                switch (currentResearch.ResearchType)
                {
                    case ConstructionType.Economy:
                        currentResearch.Bonus.Economy = bonus;
                        break;
                    case ConstructionType.Education:
                        currentResearch.Bonus.Education = bonus;
                        break;
                    case ConstructionType.Environment:
                        currentResearch.Bonus.Environment = bonus;
                        break;
                    case ConstructionType.Food:
                        currentResearch.Bonus.Food = bonus;
                        break;
                    case ConstructionType.Health:
                        currentResearch.Bonus.Health = bonus;
                        break;
                    case ConstructionType.Energy:
                        currentResearch.Bonus.Energy = bonus;
                        break;
                }
                currentResearch.Cost = Convert.ToInt32(researchElement.Element("cost").Value);
                currentResearch.Name = researchElement.Element("name").Value.ToString();
                currentResearch.Description = Convert.ToString(researchElement.Element("description").Value);
                currentResearch.Completed = Convert.ToBoolean(researchElement.Element("done").Value);

                researchList.Add(currentResearch);
            }
            return researchList;
        }
        #endregion
    }
}
