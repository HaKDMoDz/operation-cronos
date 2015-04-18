using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Operation_Cronos.Profiles
{
    public class UsersList
    {
        const string XmlFileName = "profiles.xml";
        private static int MaxId = 0;

        #region Fields
        /// <summary>
        /// Contains identification data for the user's list.
        /// </summary>
        Hashtable links;
        /// <summary>
        /// The list of all the users.
        /// </summary>
        private List<User> users;
        /// <summary>
        /// The xml code representing the users list.
        /// </summary>
        private XElement xml;
        #endregion

        #region Properties

        public int Count
        {
            get { return users.Count; }
        }

        /// <summary>
        /// Returns the xml code created based on the users list.
        /// </summary>
        public XElement Xml
        {
            get
            {
                return xml;
            }
        }

        /// <summary>
        /// Indexer used to get/set the user at the specified index in the list.
        /// </summary>
        /// <param name="index">The zero-based index of the user.</param>
        /// <returns>The user.</returns>
        public User this[int index]
        {
            get { return users[index]; }
            set { users[index] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an empty users list.
        /// </summary>
        public UsersList()
        {
            links = new Hashtable();
            users = new List<User>(10);
            xml = new XElement("users");
        }

        /// <summary>
        /// Creates a users list from xml code.
        /// </summary>
        /// <param name="_xml">The XElement used to create the users list.</param>
        public UsersList(XElement _xml)
        {
            xml = _xml;
            links = new Hashtable();
            users = new List<User>(xml.Elements("user").Count());
            ParseXml();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a newly created user to the list.
        /// </summary>
        public void AddUser(User user)
        {
            if (user.Id > MaxId)
            {
                MaxId = user.Id;
            }
            users.Add(user);
            links.Add(user.Id, users.Count - 1);
            links.Add(user.Name, users.Count - 1);

            //adds xml code
            XElement userXmlCode = new XElement("user");
            XElement idXml = new XElement("id");
            idXml.Add(user.Id);
            XElement nameXml = new XElement("name");
            nameXml.Add(user.Name);
            userXmlCode.Add(idXml, nameXml);
            xml.Add(userXmlCode);

            //write to disk
            SaveToFile("Profiles\\" + UsersList.XmlFileName);
        }

        /// <summary>
        /// Creates a new user and adds it to the list.
        /// </summary>
        public void AddUser(int id, string name)
        {
            if (id > UsersList.MaxId)
            {
                MaxId = id;
            }
            User user = new User(id, name);
            users.Add(user);
            links.Add(user.Id, users.Count - 1);
            links.Add(user.Name, users.Count - 1);

            //adds xml code
            XElement userXmlCode = new XElement("user");
            XElement idXml = new XElement("id");
            idXml.Add(user.Id);
            XElement nameXml = new XElement("name");
            nameXml.Add(user.Name);
            userXmlCode.Add(idXml, nameXml);
            xml.Add(userXmlCode);

            //write to disk
            SaveToFile("Profiles\\" + UsersList.XmlFileName);
        }

        /// <summary>
        /// Creates a new user and adds it to the list.
        /// </summary>
        public void AddUser(string name)
        {
            int id = ++MaxId;
            AddUser(id, name);
        }
        /// <summary>
        /// Returns the user with the specified id.
        /// </summary>
        public User GetUser(int id)
        {
            return this[(int)links[id]];
        }

        /// <summary>
        /// Returns the user with the specified name.
        /// </summary>
        public User GetUser(string name)
        {
            return this[(int)links[name]];
        }


        /// <summary>
        /// Deletes the user with the specified id.
        /// </summary>
        public void DeleteUser(int id)
        {
            string name = String.Empty;
            foreach (XElement element in Xml.Nodes())
            {
                if (Convert.ToInt32(element.Element("id").Value) == id)
                {
                    name = element.Element("name").Value;
                    element.Remove();
                    break;
                }
            }
            SaveToFile("Profiles\\" + UsersList.XmlFileName);
            Directory.Delete("Profiles\\" + name, true);
            users.Clear();
            links.Clear();
            ParseXml();
        }

        /// <summary>
        /// Deletes the user with the specified name.
        /// </summary>
        public void DeleteUser(string name)
        {

            foreach (XElement element in Xml.Nodes())
            {
                if (element.Element("name").Value == name)
                {
                    element.Remove();
                    break;
                }
            }

            SaveToFile("Profiles\\" + UsersList.XmlFileName);
            Directory.Delete("Profiles\\" + name, true);

            users.Clear();
            links.Clear();
            ParseXml();
        }

        public override string ToString()
        {
            String str = String.Empty;
            foreach (User user in users)
            {
                str += user.ToString() + "\n";
            }
            return str;
        }
        #endregion

        /// <summary>
        /// Translates the xml code into an users list.
        /// </summary>
        private void ParseXml()
        {
            foreach (XElement user in xml.Elements("user"))
            {
                int id = Convert.ToInt32(user.Element("id").Value);
                if (id > MaxId)
                {
                    MaxId = id;
                }
                string name = user.Element("name").Value;
                User newUser = new User(id, name);
                users.Add(newUser);
                links.Add(newUser.Id, users.Count - 1);
                links.Add(newUser.Name, users.Count - 1);
            }
        }

        /// <summary>
        /// Writes the xml code on disk.
        /// </summary>
        /// <param name="path"></param>
        private void SaveToFile(string path)
        {
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(path, xws))
            {
                xml.WriteTo(xw);
            }
            foreach (User user in users)
            {
                if (!Directory.Exists("Profiles\\" + user.Name))
                {
                    Directory.CreateDirectory("Profiles\\" + user.Name);
                }
            }
        }
    }
}
