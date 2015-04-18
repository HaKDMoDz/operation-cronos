using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operation_Cronos.Profiles {
    public class User {

        #region Fields
        /// <summary>
        /// The user's id.
        /// </summary>
        int id;
        /// <summary>
        /// The user's name.
        /// </summary>
        string name;
        #endregion

        #region Properties
        /// <summary>
        /// The user's id.
        /// </summary>
        public int Id {
            get {
                return id;
            }
            set {
                id = value;
            }
        }

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        /// <summary>
        /// The user's folder.
        /// </summary>
        public string Folder {
            get {
                return name;
            }
        }
        #endregion

        #region Constructors
        public User() {
        }

        public User(int id, string name) {
            Id = id;
            Name = name;
        }
        #endregion

        #region Methods
        public override string ToString() {
            return "ID: " + Id.ToString() + ", Name: " + Name;
        }
        #endregion

    }
}
