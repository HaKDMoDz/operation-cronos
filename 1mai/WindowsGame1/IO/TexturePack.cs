using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos.IO {
    public struct TexturePack {
        private String name;
        private int id;
        private List<Texture2D> textures;

        public String Name {
            get { return name; }
        }
        public int Id {
            get { return id; }
        }

        public List<Texture2D> Frames {
            get { return textures; }
        }
        public TexturePack(String _name, int _id, List<Texture2D> _textures) {
            name = _name;
            id = _id;
            textures = _textures;
        }
    }
}