using System;
using Microsoft.Xna.Framework;

namespace Operation_Cronos.Display {

    public enum CameraDirection {
        None,
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public enum CameraStatus
    {
        Stopped,
        Dragged,
        MouseScrolled,
        KeyboardScrolled
    }

    public class Camera : Microsoft.Xna.Framework.GameComponent {

        public event EventHandler OnMove = delegate { };

        #region Fields
        /// <summary>
        /// The moving speed of the camera.
        /// </summary>
        public static float Speed = 8f;

        private Matrix worldProjection;
        private Vector2 position;
        private Vector2 direction;
        private CameraDirection cameraDirection;
        private Rectangle screenSize;
        private bool frozen;
        private CameraStatus status;
        private Point draggingMousePos;
        #endregion

        #region Constructor
        public Camera(Game game)
            : base(game) {
            Game.Components.Add(this);
            position = new Vector2(20, 0);
            direction = Vector2.Zero;
            frozen = false;
        }
        #endregion

        #region Properties
        public Point MouseDraggingPosition
        {
            get { return draggingMousePos; }
            set { draggingMousePos = value; }
        }
        public CameraStatus CameraStatus
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// The world positions of the camera.
        /// </summary>
        public Vector2 Position {
            get {
                return position;
            }
            set {
                position = new Vector2((float)Math.Round(value.X), (float)Math.Round(value.Y));
                worldProjection = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
            }
        }

        /// <summary>
        /// Wraps up the process of setting the camera moving direction.
        /// </summary>
        public CameraDirection Direction {
            get {
                return cameraDirection;
            }
            set {
                cameraDirection = value;
                switch (cameraDirection) {
                    case CameraDirection.None:
                        direction = Vector2.Zero;
                        break;
                    case CameraDirection.Left:
                        direction = Vector2.Normalize(new Vector2(-1, 0));
                        break;
                    case CameraDirection.Right:
                        direction = Vector2.Normalize(new Vector2(1, 0));
                        break;
                    case CameraDirection.Up:
                        direction = Vector2.Normalize(new Vector2(0, -1));
                        break;
                    case CameraDirection.Down:
                        direction = Vector2.Normalize(new Vector2(0, 1));
                        break;
                    case CameraDirection.UpRight:
                        direction = Vector2.Normalize(new Vector2(1, -1));
                        break;
                    case CameraDirection.UpLeft:
                        direction = Vector2.Normalize(new Vector2(-1, -1));
                        break;
                    case CameraDirection.DownRight:
                        direction = Vector2.Normalize(new Vector2(1, 1));
                        break;
                    case CameraDirection.DownLeft:
                        direction = Vector2.Normalize(new Vector2(-1, 1));
                        break;
                }
            }
        }

        /// <summary>
        /// The view of the camera.
        /// </summary>
        public Matrix WorldProjection {
            get {
                return worldProjection;
            }
            set {
            }
        }

        public Rectangle Screen {
            get { return new Rectangle((int)Position.X, (int)Position.Y, screenSize.Width, screenSize.Height); }
            set { screenSize = value; }
        }
        #endregion

        #region Overrides
        public override void Initialize() {
            base.Initialize();
            Screen = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
        }

        public override void Update(GameTime gameTime) 
        {
            HandleCameraMovement();
            base.Update(gameTime);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Makes all the transformations regarding camera movemement.
        /// </summary>
        private void HandleCameraMovement()
        {
            if (frozen == false)
            {
                    if ((cameraDirection == CameraDirection.Left ||
                        cameraDirection == CameraDirection.UpLeft ||
                        cameraDirection == CameraDirection.DownLeft) && position.X <= Speed)
                    {
                        Position = new Vector2(0, position.Y);
                        Direction = CameraDirection.None;
                        OnMove(this, new EventArgs());
                    }
                    if ((cameraDirection == CameraDirection.Up ||
                        cameraDirection == CameraDirection.UpLeft ||
                        cameraDirection == CameraDirection.UpRight) && position.Y <= Speed)
                    {
                        Position = new Vector2(position.X, 0);
                        Direction = CameraDirection.None;
                        OnMove(this, new EventArgs());
                    }
                        if ((cameraDirection == CameraDirection.Down ||
                            cameraDirection == CameraDirection.DownLeft ||
                            cameraDirection == CameraDirection.DownRight) && position.Y >= ((GameMap)Game.Services.GetService(typeof(GameMap))).Height - Speed - Screen.Height)
                        {
                            Position = new Vector2(position.X, ((GameMap)Game.Services.GetService(typeof(GameMap))).Height - Screen.Height);
                            Direction = CameraDirection.None;
                            OnMove(this, new EventArgs());
                        }
                    if ((cameraDirection == CameraDirection.Right ||
                        cameraDirection == CameraDirection.DownRight ||
                        cameraDirection == CameraDirection.UpRight) && position.X >= ((GameMap)Game.Services.GetService(typeof(GameMap))).Width - Speed - Screen.Width)
                    {
                        Position = new Vector2(((GameMap)Game.Services.GetService(typeof(GameMap))).Width - Screen.Width, position.Y);
                        Direction = CameraDirection.None;
                        OnMove(this, new EventArgs());
                    }

                    Position += Camera.Speed * direction;
                    if (direction != Vector2.Zero)
                    {
                        OnMove(this, new EventArgs());
                    }
                    if (CameraStatus == CameraStatus.Dragged)
                    {
                        OnMove(this, new EventArgs());
                    }
            }
        }
        /*
        public CameraDirection AddDirection(CameraDirection first, CameraDirection second)
        {
            switch (first)
            {
                case CameraDirection.None:
                    return second;
                case CameraDirection.Up:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Up;
                        case CameraDirection.Left:
                            return CameraDirection.UpLeft;
                        case CameraDirection.Right:
                            return CameraDirection.UpRight;
                        case CameraDirection.Up:
                            return CameraDirection.Up;
                        case CameraDirection.Down:
                            return CameraDirection.Down;
                    }
                case CameraDirection.Down:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Down;
                        case CameraDirection.Left:
                            return CameraDirection.DownLeft;
                        case CameraDirection.Right:
                            return CameraDirection.DownRight;
                        case CameraDirection.Up:
                            return CameraDirection.Up;
                        case CameraDirection.Down:
                            return CameraDirection.Down;
                    }
                case CameraDirection.Left:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Left;
                        case CameraDirection.Left:
                            return CameraDirection.Left;
                        case CameraDirection.Right:
                            return CameraDirection.Right;
                        case CameraDirection.Up:
                            return CameraDirection.UpLeft;
                        case CameraDirection.Down:
                            return CameraDirection.DownLeft;
                    }
                case CameraDirection.Right:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Right;
                        case CameraDirection.Left:
                            return CameraDirection.Left;
                        case CameraDirection.Right:
                            return CameraDirection.Right;
                        case CameraDirection.Up:
                            return CameraDirection.UpRight;
                        case CameraDirection.Down:
                            return CameraDirection.DownRight;
                    }
                default:
                    return CameraDirection.None;
            }
        }

        public CameraDirection SubstractDirection(CameraDirection first, CameraDirection second)
        {
            switch (first)
            {
                case CameraDirection.None:
                    return second;
                case CameraDirection.Up:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Up;
                        case CameraDirection.Left:
                            return CameraDirection.UpLeft;
                        case CameraDirection.Right:
                            return CameraDirection.UpRight;
                        case CameraDirection.Up:
                            return CameraDirection.Up;
                        case CameraDirection.Down:
                            return CameraDirection.Down;
                    }
                case CameraDirection.Down:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Down;
                        case CameraDirection.Left:
                            return CameraDirection.DownLeft;
                        case CameraDirection.Right:
                            return CameraDirection.DownRight;
                        case CameraDirection.Up:
                            return CameraDirection.Up;
                        case CameraDirection.Down:
                            return CameraDirection.Down;
                    }
                case CameraDirection.Left:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Left;
                        case CameraDirection.Left:
                            return CameraDirection.Left;
                        case CameraDirection.Right:
                            return CameraDirection.Right;
                        case CameraDirection.Up:
                            return CameraDirection.UpLeft;
                        case CameraDirection.Down:
                            return CameraDirection.DownLeft;
                    }
                case CameraDirection.Right:
                    switch (second)
                    {
                        case CameraDirection.None:
                            return CameraDirection.Right;
                        case CameraDirection.Left:
                            return CameraDirection.Left;
                        case CameraDirection.Right:
                            return CameraDirection.Right;
                        case CameraDirection.Up:
                            return CameraDirection.UpRight;
                        case CameraDirection.Down:
                            return CameraDirection.DownRight;
                    }
                default:
                    return CameraDirection.None;
            }
        }
        */

        public Boolean ContainsObject(Rectangle bounds) {
            return Screen.Intersects(bounds);
        }

        public Boolean ContainsPoint(Point position) {
            return Screen.Contains(position);
        }

        public void ChangeResolution(int width, int height) {
            GraphicsDeviceManager graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            Screen = new Rectangle(0, 0, width, height);
            graphics.ApplyChanges();
        }

        public Boolean Fullscreen {
            get {
                GraphicsDeviceManager graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
                return graphics.IsFullScreen;
            }
            set {
                GraphicsDeviceManager graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
                graphics.IsFullScreen = value;
                graphics.ApplyChanges();
            }
        }

        public void Freeze() {
            frozen = true;
        }

        public void Unfreeze() {
            frozen = false;
        }
        #endregion
    }
}