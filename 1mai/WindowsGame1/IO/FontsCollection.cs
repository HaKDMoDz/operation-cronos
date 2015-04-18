using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Xml.Linq;
using System.Threading;


namespace Operation_Cronos.IO {
    public class FontsCollection : Microsoft.Xna.Framework.GameComponent {

        #region Events
        public event EventHandler<IOEventArgs> OnStart = delegate { };
        public event EventHandler<IOEventArgs> OnProgress = delegate { };
        public event EventHandler<IOEventArgs> OnComplete = delegate { };
        #endregion

        #region Fields
        private List<FontPack> fonts;
        private Hashtable links;
        #endregion

        #region Properties
        public FontPack this[int index] {
            get { return fonts[index]; }
        }
        #endregion

        #region Constructors
        public FontsCollection(Game game)
            : base(game) {
            fonts = new List<FontPack>();
            links = new Hashtable();
            Game.Components.Add(this);
        }

        public FontsCollection(Game game, XElement xml, IOOperation op, IOContent type)
            : base(game) {
            fonts = new List<FontPack>();
            links = new Hashtable();
            Game.Components.Add(this);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Load), new IODataPack(xml, op, type));
        }
        #endregion

        public void AddFontsFromXml(XElement xml, IOOperation op, IOContent type) {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Load), new IODataPack(xml, op, type));
        }

        public FontPack GetPack(int id) {
            return fonts[(int)links[id]];
        }

        public FontPack GetPack(string name) {
            return fonts[(int)links[name]];
        }

        private void Load(object state) {
            IODataPack ioDataPack = (IODataPack)state;
            LoadFonts(ioDataPack);
        }

        private void LoadFonts(IODataPack ioDataPack) {
            XElement xml = ioDataPack.Xml;
            IOContent contentType = ioDataPack.Type;
            IOOperation operation = ioDataPack.Operation;

            int fileCount = xml.Elements("font").Count();
            int filesLoaded = 0;
            double progress = (double)filesLoaded / (double)fileCount * 100;

            OnStart(this, new IOEventArgs(0, ioDataPack.Operation, ioDataPack.Type));
            foreach (XElement element in xml.Elements("font")) {
                SpriteFont font = Game.Content.Load<SpriteFont>(element.Element("resource").Value);
                int id = Convert.ToInt32(element.Element("id").Value);
                string name = element.Element("name").Value;
                fonts.Add(new FontPack(font, id, name));
                links.Add(id, fonts.Count - 1);
                links.Add(name, fonts.Count - 1);
                filesLoaded++;
                progress = (double)filesLoaded / (double)fileCount * 100;
                OnProgress(this, new IOEventArgs(progress, ioDataPack.Operation, ioDataPack.Type));
            }
            OnComplete(this, new IOEventArgs(progress, ioDataPack.Operation, ioDataPack.Type));
        }

        #region Overrides

        public override void Initialize() {
            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        #endregion
    }
}