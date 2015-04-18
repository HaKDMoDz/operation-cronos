using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.IO;
using Operation_Cronos.Display;

namespace Operation_Cronos {
    public enum AnimationDirection {
        Forward = 'f',
        Backward = 'b',
    }
    public class Sprite : DrawableGameComponent, IStackable, IPositionable {
         
        /// <summary>
        /// The maximum number of updates until the next frame.
        /// </summary>
        public const int TimeFrame = 100;
        /// <summary>
        /// Used to modify the speed for every sprite in the game.
        /// </summary>
        public static float GlobalAnimationSpeedModifier = 1f;

        #region Fields
        /// <summary>
        /// The bounding box of the texture.
        /// </summary>
        private Rectangle bounds;
        private int xrel;
        private int yrel;
        /// <summary>
        /// The texture displayed on the screen.
        /// </summary>
        private Texture2D texture;
        /************************   Eliminat  ************************
        /// <summary>
        /// The angle of rotation, in radians.
        /// </summary>
        private float angle;
        /// <summary>
        /// The origin of the texture and the rotation center.
        /// </summary>
        private Vector2 origin;
        /*************************************************************/
        /// <summary>
        /// The portion of the texture to be displayed.
        /// </summary>
        private Rectangle sourceRectangle;
        
        private Rectangle collisionTile;
        /// <summary>
        /// The tint of the texture (Color.White for no change).
        /// </summary>
        private Color tintColor = Color.White;
        /// <summary>
        /// The color used to highlight the texture.
        /// </summary>
        public Color highlightColor = new Color(Color.White, 150);
        /// <summary>
        /// The userList of the textures used in the animation.
        /// </summary>
        private List<Texture2D> framesList;
        private int frameNumber;
        private int framesCount;
        private int baseAnimationSpeed = 5;
        private int counter=0;
        private AnimationDirection animationDirection = AnimationDirection.Forward;
        private Boolean generateEvents;
        private Boolean generateFrameOfInterestEvent;
        private int frameOfInterest;

        /// <summary>
        /// The order of the sprite in the visual stack of components.
        /// </summary>
        private int stackOrder;

        private Boolean isVisible;
        #endregion

        #region Properties

        #region Location and Size
        /// <summary>
        /// The absolute X location of the texture.
        /// </summary>
        public int X {
            get { return bounds.X; }
            set { bounds.X = value; }
        }

        /// <summary>
        /// The X location of the sprite relative to its parent.
        /// </summary>
        public int XRelative {
            get { return xrel; }
            set { xrel = value; }
        }
        /// <summary>
        /// The absolute Y location of the texture.
        /// </summary>
        public int Y {
            get { return bounds.Y; }
            set { bounds.Y = value; }
        }

        /// <summary>
        /// The Y location of the sprite relative to its parent.
        /// </summary>
        public int YRelative {
            get { return yrel; }
            set { yrel = value; }
        }
        public Point Position {
            get { return new Point(bounds.X, bounds.Right); }
            set { bounds.X = value.X; bounds.Y = value.Y; }
        }

        /// <summary>
        /// The location of the texture inf Vector2 format.
        /// </summary>
        public Vector2 Location {
            get { return new Vector2(bounds.X, bounds.Y); }
        }

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width {
            get { return bounds.Width; }
            set { bounds.Width = value; collisionTile.Width = value; }
        }

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height {
            get { return bounds.Height; }
            set { bounds.Height = value; collisionTile.Height = value; }
        }

        /// <summary>
        /// The bounding box.
        /// </summary>
        public Rectangle Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        /// <summary>
        /// The collision area. By default, is the same as the bounding box.
        /// </summary>
        public Rectangle CollisionTile {
            get { return collisionTile; }
            set { collisionTile = value; }
        }
        /// <summary>
        /// The portion of the texture to be displayed.
        /// </summary>
        public Rectangle SourceRectangle {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }
        #endregion

        #region Obsolete
        /*******************  Eliminat   *****************************
        /// <summary>
        /// The origin of the texture.
        /// </summary>
        public Vector2 Origin {
            get { return origin; }
            set { origin = value; }
        }

        /// <summary>
        /// The angle of rotation in radians.
        /// </summary>
        public float Angle {
            get { return angle; }
            set { angle = value; }
        }
        *************************************************************/
        #endregion

        /// <summary>
        /// Used to set the highlight on/off.
        /// </summary>
        public Boolean Highlight {
            set {
                if (value == true) {
                    tintColor = highlightColor;
                } else {
                    tintColor = Color.White;
                }
            }
        }

        public Color Tint {
            get { return tintColor; }
            set { tintColor = value; }
        }

        #region Animation
        public Texture2D CurrentFrame {
            get { return texture; }
        }

        /// <summary>
        /// The current frame number.
        /// </summary>
        public int FrameNumber {
            get { return frameNumber; }
            set {
                if (value >= 0 && value <= framesCount) {
                    frameNumber = value;
                    texture = framesList[value];
                } else
                    throw (new IndexOutOfRangeException("The specified frame number doesn't exist."));
            }
        }
        public int NextFrameNumber {
            get { return frameNumber < framesCount ? frameNumber + 1 : 0; }
        }
        public int PreviousFrameNumber {
            get { return frameNumber > 0 ? frameNumber - 1 : framesCount; }
        }
        /// <summary>
        /// The animation speed, in counter increments.
        /// It is added to the counter until the counter reaches a certain value,
        /// after which the animation goes to the next frame.
        /// </summary>
        public int AnimationSpeed {
            get { return baseAnimationSpeed; }
            set {
                if ((int)(value*Sprite.GlobalAnimationSpeedModifier) <= Sprite.TimeFrame && (int)(value*Sprite.GlobalAnimationSpeedModifier) >= 0)
                    baseAnimationSpeed = value;
                else
                    throw (new ArgumentOutOfRangeException("The speed is either too great or too small."));
            }
        }
        public AnimationDirection AnimDirection {
            get { return animationDirection; }
            set { animationDirection = value; }
        }

        /// <summary>
        /// A flag to set events ON/OFF.
        /// </summary>
        public Boolean GenerateEvents {
            get { return generateEvents; }
            set { generateEvents = value; }
        }

        /// <summary>
        /// A flag to set event for a certain frame ON/OFF
        /// </summary>
        public Boolean GenerateFrameOfInterestEvent
        {
            get { return generateFrameOfInterestEvent; }
            set { generateFrameOfInterestEvent = value; }
        }

        /// <summary>
        /// Gets or sets the frame number which generates the OnFrameOfInterest event
        /// </summary>
        public int FrameOfInterest
        {
            get { return frameOfInterest; }
            set { frameOfInterest = value; }
        }
        #endregion


        public int StackOrder {
            get { return stackOrder; }
            set { stackOrder = value; }
        }

        public Boolean IsVisible {
            get { return isVisible; }
            set { isVisible = value; Visible = value; }
        }
        #endregion

        #region Events
        public event EventHandler OnLastFrame = delegate { };
        public event EventHandler OnFirstFrame = delegate { };
        public event EventHandler OnFrameOfInterest = delegate { };
        #endregion

        #region Constructors
        public Sprite(Game game, Texture2D texture)
            : base(game) {
            this.texture = texture;
            //animation
            framesList = new List<Texture2D>();
            framesList.Add(texture);
            frameNumber = 0;
            framesCount = 0;
            counter = 0;
            this.texture = framesList[0];
            //size
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            
            //defaults
            sourceRectangle = new Rectangle(0, 0, Width, Height);
            CollisionTile = sourceRectangle;
            Visible = true;
            //
            AnimationSpeed = 0;
            Game.Components.Add(this);
            StackOrder = 0;
            DrawOrder = 0;
            IsVisible = true;
        }

        public Sprite(Game game, List<Texture2D> list)
            : base(game) {
            //animation
            framesList = list;
            texture = framesList[0];
            frameNumber = 0;
            framesCount = framesList.Count-1;
            counter = 0;
            //size
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            //defaults
            sourceRectangle = new Rectangle(0, 0, Width, Height);
            CollisionTile = sourceRectangle;
            Visible = true;
            //
            AnimationSpeed = 0;
            GenerateEvents = true;
            GenerateFrameOfInterestEvent = false;
            FrameOfInterest = 0;
            Game.Components.Add(this);
            StackOrder = 0;
            DrawOrder = 0;
            IsVisible = true;
        }

        public Sprite(Game game, TexturePack pack)
            : base(game) {
            //animation
            framesList = pack.Frames;
            texture = framesList[0];
            frameNumber = 0;
            framesCount = framesList.Count - 1;
            counter = 0;
            //size
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            //defaults
            sourceRectangle = new Rectangle(0, 0, Width, Height);
            CollisionTile = sourceRectangle;
            Visible = true;
            //
            AnimationSpeed = 0;
            GenerateEvents = true;
            GenerateFrameOfInterestEvent = false;
            FrameOfInterest = 0;
            Game.Components.Add(this);
            StackOrder = 0;
            DrawOrder = 0;
            IsVisible = true;
        }
        #endregion

        #region Override
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            HandleAnimation();
        }
        public override void Draw(GameTime gameTime) {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //spriteBatch.Draw(texture, Bounds, sourceRectangle, Color.White, angle, Origin, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, Bounds, sourceRectangle, tintColor);
            base.Draw(gameTime);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifies if the given positions is inside the texture bounds,
        /// and if it is, verifies if the pixel in that positions is transparent
        /// or not. If it's not transparent (i.e. alpha > 0), returns true.
        /// </summary>
        /// <param name="positions">The positions to be checked, relative to the origin of the texture.</param>
        /// <returns></returns>
        public Boolean Collides(Point position) {
            if (sourceRectangle.Contains(position)) {
                if (SelectedPixelColor(position).A > 0) {
                    return true;
                }
            }
            return false;
        }

        public Boolean CollidesTile(Point position) {
            if (CollisionTile.Contains(position)) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the color of the pixel at a certain positions
        /// in a texture.
        /// </summary>
        /// <param name="positions">The positions of the pixel, relative to the origin of the texture.</param>
        /// <returns></returns>
        private Color SelectedPixelColor(Point position) {
            GraphicsDevice.Textures[0] = null;
            Color[] pixelColor = new Color[1];
            Rectangle pixel = new Rectangle(position.X, position.Y, 1, 1);
            try {
                this.texture.GetData<Color>(0, pixel, pixelColor, 0, 1);
            } catch { }
            return pixelColor[0];
        }

        /// <summary>
        /// Increases the counter until it hits the Sprite timeframe,
        /// then goes to the next frame.
        /// The increment used is obtained through multiplying the base speed
        /// by the global animation speed modifier.
        /// </summary>
        private void HandleAnimation() {
            counter += (int)(AnimationSpeed * Sprite.GlobalAnimationSpeedModifier);
            if (counter >= Sprite.TimeFrame) 
            {
                counter = 0;
                if (AnimDirection == AnimationDirection.Forward) {
                    FrameNumber = NextFrameNumber;
                } else {
                    FrameNumber = PreviousFrameNumber;
                }
                if (GenerateEvents) {
                    if (FrameNumber == framesCount && AnimationSpeed > 0) {
                        OnLastFrame(this, new EventArgs());
                    }
                    if (FrameNumber == 0 && AnimationSpeed > 0) {
                        OnFirstFrame(this, new EventArgs());
                    }
                }
                if (GenerateFrameOfInterestEvent)
                {
                    if (FrameOfInterest == FrameNumber && AnimationSpeed > 0)
                    {
                        OnFrameOfInterest(this, new EventArgs());
                    }
                }
            }

        }

        public void PauseAnimation(){
            AnimationSpeed = 0;
        }
        public void StopAnimation() {
            AnimationSpeed = 0;
            ResetAnimation();
        }
        public void ResetAnimation(){
            FrameNumber = 0;
        }
        #endregion
    }
}