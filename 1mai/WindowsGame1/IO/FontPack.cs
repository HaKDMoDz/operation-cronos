using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Operation_Cronos.IO {
    public struct FontPack {
        private SpriteFont font;
        private int id;
        private String name;

        public SpriteFont Font {
            get { return font; }
        }

        public int Id {
            get { return id; }
        }

        public String Name {
            get { return name; }
        }

        public FontPack(SpriteFont _font, int _id, String _name) {
            font = _font;
            id = _id;
            name = _name;
        }
    }
}
