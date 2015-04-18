using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.Display;
using Operation_Cronos.GameProcessor;
using Operation_Cronos.Input;
using Operation_Cronos.IO;
using Operation_Cronos.Profiles;
using Operation_Cronos.Sound;

namespace Operation_Cronos
{
    public class OperationCronos : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Debug debug;
        InputManager inputManager;
        DisplayManager displayManager;
        IOManager ioManager;
        GameManager gameManager;
        ProfileManager profileManager;
        SoundManager soundManager;

        Video video;
        VideoPlayer videoplayer;
        bool gameStarted;

        public OperationCronos()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            Services.AddService(typeof(GraphicsDeviceManager), graphics);
            Content.RootDirectory = "Content";
            debug = new Debug(this);
            IsMouseVisible = false;
            gameStarted = false;

            //hides the Form's ControlBox
            Form MyGameForm = (Form)Form.FromHandle(this.Window.Handle);
            MyGameForm.ControlBox = false;
            MyGameForm.Text = "Operation Cronos";
            //---------------------------

            inputManager = new InputManager(this);

            displayManager = new DisplayManager(this);

            ioManager = new IOManager(this);

            gameManager = new GameManager(this);

            profileManager = new ProfileManager(this);

            soundManager = new SoundManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            video = Content.Load<Video>("Video\\Intro");
            videoplayer = new VideoPlayer();
            videoplayer.Play(video);
        }

        protected override void Draw(GameTime gameTime)
        {
            Texture2D texture = null;
            if (!gameStarted)
            {
                texture = videoplayer.GetTexture();
            }
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, displayManager.CameraPosition);
            base.Draw(gameTime);
            if (!gameStarted)
            {
                spriteBatch.Draw(texture, new Rectangle((graphics.PreferredBackBufferWidth - texture.Width) / 2, (graphics.PreferredBackBufferHeight - texture.Height) / 2, texture.Width, texture.Height), Color.White);
            }
            spriteBatch.End();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();
            if (currentKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && !gameStarted)
            {
                videoplayer.Stop();
                videoplayer.Dispose();
            }
            if ((videoplayer.IsDisposed || videoplayer.State == MediaState.Stopped) && !gameStarted)
            {
                gameStarted = true;
                StartGame();
            }
            base.Update(gameTime);
        }

        private void StartGame()
        {
            ioManager.LoadMainMenuGraphics();
        }

    }
}