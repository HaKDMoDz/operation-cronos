using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Operation_Cronos.IO {
    public class XMLLoader {
        /// <summary>
        /// The XML document.
        /// </summary>
        private XElement document;

        /// <summary>
        /// The XElement containing all the information.
        /// </summary>
        public XElement Document {
            get { return document; }
        }

        /// <summary>
        /// Creates an empty XMLLoader.
        /// </summary>
        public XMLLoader() {
            document = null;
        }

        /// <summary>
        /// Creates an XMLLoader, loading the specified file.
        /// </summary>
        public XMLLoader(string file) {
            document = XElement.Load(file);
        }

        /// <summary>
        /// Loads an XML document from a file.
        /// </summary>
        public void Load(string file) {
            document = XElement.Load(file);
        }

        /// <summary>
        /// Returns the number of files from an xml file.
        /// </summary>
        /// <param name="xml">The XElement which holds the data about the files.</param>
        /// <returns>The number of files.</returns>
        public static int FileCount(XElement xml) {
            int count = 0;
            foreach (XElement o in xml.Elements("object")) {
                foreach (XElement f in o.Elements("frames")) {
                    count += Convert.ToInt32(f.Value);
                }
            }
            return count;
        }
    }
}
