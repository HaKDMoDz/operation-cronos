using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Operation_Cronos {
    /// <summary>
    /// This is a class that implements the base properties and methods of an object which will be placed on the map.
    /// </summary>
    public class Debug : Microsoft.Xna.Framework.DrawableGameComponent {
        SpriteFont font;
        static String instantMessage = "";
        static String log="";
        static String aditional="";
        Vector2 instantPos;
        Vector2 logPos;
        private static int lineCount;
        //int lifeLength = 3000;
        //int secondsPassed = 0;
        public Vector2 Position = new Vector2(0,0);

        ContentManager Content;

        #region Public Properties
        public static String InstantMessage {
            get { return instantMessage;  }
            set { instantMessage = value; }
        }
        public static String AditionalText{
            get { return aditional;  }
            set { aditional = value; }
        }
        public static String Log {
            get { return log; }
        }
#endregion

        public Debug(Game game)
            : base(game) {
            Game.Components.Add(this);
            lineCount = 0;
        }
        public override void Initialize() {
            DrawOrder = 1000;
            Content = new ContentManager(Game.Services);
            Content.RootDirectory = "Content";
            base.Initialize();
        }
        protected override void LoadContent() {
            base.LoadContent();
            font = Content.Load<SpriteFont>("courier");
        }
        public override void Update(GameTime gameTime) {
            //secondsPassed = gameTime.TotalRealTime.Milliseconds;
            //if (secondsPassed % lifeLength == 0) {
            //    log = "";
            //}
            instantPos = new Vector2(300,300);
            logPos = Position+(new Vector2(20, 600 - 100)) - (new Vector2(0, font.MeasureString(log).Y));
            if (log.Split('\n').Length > 30) {
                log = log.Substring(log.IndexOf("\n")+1);
                log = log.Substring(log.IndexOf("\n")+1);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime) {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //sBatch.DrawString(font, instantMessage, instantPos, Color.White);
            sBatch.DrawString(font, log, logPos, Color.White);
            base.Draw(gameTime);
        }
        public static void AddToLog(String text){
            log += text + "\n";
            lineCount++;
            if (lineCount > 50)
            {
                Clear();
                lineCount = 0;
            }
        }
        public static void AddToLog(object text)
        {
            log += text.ToString() + "\n";
            lineCount++;
            if (lineCount > 50)
            {
                Clear();
                lineCount = 0;
            }
        }
        public static void Clear() {
            log = "";
        }
    }
}
