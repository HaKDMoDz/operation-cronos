using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Xml.Linq;
using System.Collections;


namespace Operation_Cronos.IO {
    /// <summary>
    /// Data strucutre used to pack the necessary parameters 
    /// used to start an I/O operation.
    /// </summary>
    struct IODataPack {
        XElement xml;
        IOContent type;
        IOOperation operation;
        public XElement Xml {
            get { return xml; }
        }
        public IOContent Type {
            get { return type; }
        }
        public IOOperation Operation {
            get { return operation; }
        }
        public IODataPack(XElement _xml, IOOperation op,IOContent _type){
            xml = _xml;
            type = _type;
            operation = op;
        }
    }

    public class GraphicsCollection : Microsoft.Xna.Framework.GameComponent {

        #region Fields
        /// <summary>
        /// Holds the texturepacks.
        /// </summary>
        private List<TexturePack> packs;
        /// <summary>
        /// Holds the indexes of the texturepacks. The keys are the name and the id of the texturepack.
        /// </summary>
        private Hashtable links;

        #endregion

        #region Properties
        /// <summary>
        /// Indexer that returns the texturepack at the specified index.
        /// </summary>
        /// <param name="index">The index of the requested texturepack.</param>
        /// <returns>A TexturePack.</returns>
        public TexturePack this[int index] {
            get { return packs[index]; }
        }

        #endregion

        #region Events
        /// <summary>
        /// Generated when the collection starts loading.
        /// </summary>
        public event EventHandler<IOEventArgs> OnStart = delegate { };
        /// <summary>
        /// Generated when a new texture is loaded.
        /// </summary>
        public event EventHandler<IOEventArgs> OnProgress = delegate { };
        /// <summary>
        /// Generated when a new pack is completed.
        /// </summary>
        public event EventHandler<IOEventArgs> OnNewPack = delegate { };
        /// <summary>
        /// Generated when the collection finishes loading.
        /// </summary>
        public event EventHandler<IOEventArgs> OnComplete = delegate { };
        #endregion


        public GraphicsCollection(Game game)
            : base(game) {
            packs = new List<TexturePack>();
            links = new Hashtable();
            Game.Components.Add(this);
        }

        /// <summary>
        /// Creates a new GraphicsCollection and starts loading the textures from the received XML.
        /// </summary>
        public GraphicsCollection(Game game, XElement _xml, IOContent type, IOOperation op)
            : base(game) {
            packs = new List<TexturePack>();
            links = new Hashtable();
            Game.Components.Add(this);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Load), new IODataPack(_xml, op, type));
        }

        #region Overrides
        public override void Initialize() {
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Loads a new collection of texture packs from an XML file.
        /// </summary>
        /// <param name="_xml"></param>
        public void AddCollectionFromXml(XElement _xml, IOOperation op, IOContent type) {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Load), new IODataPack(_xml,op,type));
        }
        /// <summary>
        /// A wrapper of the LoadTexturePacks method, called by the Thread.QueueUserWorkItem method.
        /// </summary>
        private void Load(Object data) {
            LoadTexturePacks((IODataPack)data);
        }
        /// <summary>
        /// Returns the index of the TexturePack with the specified id.
        /// </summary>
        public int GetPackIndex(int id) {
            return (int)links[id];
        }
        /// <summary>
        /// Returns the index of the TexturePack with the specified name.
        /// </summary>
        public int GetPackIndex(string name) {
            return (int)links[name];
        }

        /// <summary>
        /// Returns the TexturePack with the specified id
        /// </summary>
        public TexturePack GetPack(int id) {
            return this[id];
        }

        /// <summary>
        /// Returns the TexturePack with the specified name
        /// </summary>
        public TexturePack GetPack(string name) {
            return this[GetPackIndex(name)];
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the number of files from an xml file.
        /// </summary>
        /// <param name="xml">The XElement which holds the data about the files.</param>
        /// <returns>The number of files.</returns>
        private int FileCount(XElement xml) {
            int count = 0;
            foreach (XElement o in xml.Elements("object")) {
                foreach (XElement f in o.Elements("frames")) {
                    count += Convert.ToInt32(f.Value);
                }
            }
            return count;
        }

        /// <summary>
        /// Creates texturepacks and adds them to the packs list.
        /// </summary>
        private void LoadTexturePacks(IODataPack ioDataPack) {
            XElement xml = ioDataPack.Xml;
            IOContent contentType = ioDataPack.Type;
            IOOperation operation = ioDataPack.Operation;

            int totalFiles = FileCount(xml);
            int loadedFiles = 0;
            int progress = 0;
            OnStart(this, new IOEventArgs(progress, operation, contentType));
            IEnumerable<XElement> xmlPacks = from element in xml.Elements() select element;
            
            foreach (XElement xmlData in xmlPacks) {
                int id = Convert.ToInt32(xmlData.Element("id").Value);
                int count = Convert.ToInt32(xmlData.Element("frames").Value);
                string folder = xmlData.Element("folder").Value;
                string name = xmlData.Element("name").Value;
                packs.Add(CreateTexturePack(id, name, folder, count, ref loadedFiles, ref totalFiles, contentType, operation));
                links.Add(name, packs.Count-1);
                links.Add(id, packs.Count-1);
                OnNewPack(this, new IOEventArgs(progress, operation, contentType));
            }
            OnComplete(this, new IOEventArgs(progress, operation, contentType));
        }

        /// <summary>
        /// Loads a pack of textures and returns a new TexturePack.
        /// </summary>
        /// <param name="id">The id of the TexturePack returned.</param>
        /// <param name="name">The name of the TexturePack returned.</param>
        /// <param name="folder">The path of the TexturePack returned.</param>
        /// <param name="count">The number of frames.</param>
        private TexturePack CreateTexturePack(int id, string name, string folder, int count, ref int loadedFiles, ref int totalFiles, IOContent contentType, IOOperation operation) {
            List<Texture2D> temp = new List<Texture2D>(count);
            for (int i = 0; i < count; i++) {
                string path = folder + "//" + (10000 + i).ToString();
                Texture2D tex = Game.Content.Load<Texture2D>(path);
                //Thread.Sleep(500);
                loadedFiles++;
                double progress = (double)loadedFiles/(double)totalFiles*100;
                OnProgress(this, new IOEventArgs(progress,operation, contentType));
                temp.Add(tex);
            }
            return new TexturePack(name, id, temp);
        }
        #endregion

    }
}