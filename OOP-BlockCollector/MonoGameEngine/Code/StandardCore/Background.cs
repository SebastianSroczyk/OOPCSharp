// Based on the camera work of RB Whitaker: http://rbwhitaker.wikidot.com/monogame-basic-matrices
// Also utilises code from Monogame's learner materials: https://learn-monogame.github.io/tutorial/infinite-background-shader/ 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoGameEngine.StandardCore
{
    /// <summary>Enum used to control Background rendering behaviour.
    /// <br/>- <strong>Stretch</strong> will fill the current window with the held image.
    /// <br/>- <strong>Wrap</strong> will tile the background image across the current window without stretching. Does not allow scrolling.
    /// <br/>- <strong>HorizontalScroll</strong> wraps the background image across the current window, but only allows scrolling horizontally.
    /// <br/>- <strong>VerticalScroll</strong> wraps the background image across the current window, but only allows scrolling vertically.
    /// <br/>- <strong>FullScroll</strong> wraps the background image across the current window, and allows scrolling in all directions.
    /// </summary>
    public enum BackgroundType { Stretch, Fill, Wrap, HorizontalScroll, VerticalScroll, FullScroll };

    /// <summary>A class which represents the background visual element of a given Screen.</summary>
    public sealed class Background
    {
        private readonly Core _core;
        private Effect _backgroundEffect;
        private BackgroundType _backgroundType;
        private Texture2D _image;

        private Vector2 _position;
        private float _rotation = 0.0f;
        private float _scale = 1.0f;

        private float _layerDepth = 1.0f;
        private float _parallaxStrength = 1.0f;

        private readonly Dictionary<string, Vector2> _panelPositions;

        /// <summary>
        /// The constructor of this Background class.
        /// </summary>
        /// <param name="core">The Core instance at the center of this game project.</param>
        internal Background(Core core)
        {
            _position = Vector2.Zero;
            _core = core;
            _panelPositions = new Dictionary<string, Vector2>();
            _panelPositions.Add("Center", Vector2.Zero);
        }

        /// <summary>
        /// A method used for the initial setup of this Background object. 
        /// </summary>
        /// <param name="backgroundName"></param>
        /// <param name="layerDepth"></param>
        /// <param name="type"></param>
        internal void Initialise(string backgroundName, float layerDepth, BackgroundType type = BackgroundType.Stretch)
        {
            _backgroundType = type;
            _layerDepth = layerDepth;

            try
            {
                _image = Core.GetResource<Texture2D>("images/" + backgroundName);
            }catch(IOException ex)
            {
                throw ex;
            }

            InitialisePanelPositions(type);
        }

        /// <summary>
        /// A method which allows this object to remain up-to-date.
        /// </summary>
        /// <param name="deltaTime">The time (in seconds) since the last frame of the game.</param>
        internal void Update(float deltaTime)
        {
            
        }

        /// <summary>
        /// A method which allows this Background object to be drawn to the game window.
        /// </summary>
        /// <param name="spriteBatch">The current batch of sprites to be rendered this frame of the game.</param>
        internal void Render(SpriteBatch spriteBatch)
        {
            var panelWidth = (int)Settings.GameResolution.X;
            var panelHeight = (int)Settings.GameResolution.Y;
            var camTranslation = Matrix.CreateTranslation(Camera.Instance.GetViewMatrix(Vector2.One).Translation);
            _position = Vector2.Transform(_position, Matrix.Invert(camTranslation));

            if (_backgroundType == BackgroundType.Stretch)
            {
                spriteBatch.Draw(_image, Camera.Instance.ViewBounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth);
            }
            else if(_backgroundType == BackgroundType.Wrap)
            {
                for(int i = 0; i < Settings.ScreenDimensions.X; i += _image.Width)
                    for(int j = 0; j < Settings.ScreenDimensions.Y; j += _image.Height)
                        spriteBatch.Draw(_image, new Rectangle(i, j, _image.Width, _image.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth);
            }
            else
            {
                foreach(Vector2 panelPos in _panelPositions.Values)
                    spriteBatch.Draw(_image, new Rectangle((int)(_position.X + panelPos.X), (int)(_position.Y + panelPos.Y), panelWidth, panelHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _layerDepth);
            }

            _position = Vector2.Transform(_position, camTranslation);
        }

        /// <summary>
        /// A setter method for changing the rotation of the Background's viewport, in degrees. 
        /// </summary>
        /// <param name="degrees">A floating-point value representing the z-axis rotation of this Background object, in degrees.</param>
        public void SetRotation(float degrees)
        {
            _rotation = MathHelper.ToRadians(degrees);
        }

        /// <summary>
        /// A getter method for accessing the rotation of this Background's viewport, in degrees.
        /// </summary>
        /// <returns>A floating-point value representing the current rotation of this Background, in degrees.</returns>
        public float GetRotation()
        {
            return MathHelper.ToDegrees(_rotation);
        }

        /// <summary>
        /// A getter method for accessing the position of this Background's viewport.
        /// </summary>
        /// <returns>A Vector2 object representing the co-ordinates of this Background's on-screen position.</returns>
        public Vector2 GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// A setter method for changing the position of this Background's viewport. <br></br>Can only change the position of Backgrounds which have scrolling enabled.
        /// </summary>
        /// <param name="position">The on-screen position that this Background should move to.</param>
        public void SetPosition(Vector2 position)
        {
            if (_backgroundType != BackgroundType.Stretch || _backgroundType != BackgroundType.Wrap) // prevents stretched or static wrapped backgrounds from moving
            {
                _position = position;
            }
            else
            {
                Console.WriteLine("Backgrounds using a BackgroundType of Stretch or Wrap cannot be moved.");
            }
        }

        /// <summary>
        /// [Overload] A setter method for changing the position of this Background's viewport. <br></br>Can only change the position of Backgrounds which have scrolling enabled.
        /// </summary>
        /// <param name="x">The on-screen horizontal co-ordinate that this Background should move to.</param>
        /// <param name="y">The on-screen vertical co-ordinate that this Background should move to.</param>
        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        internal void SetParallaxStrength(float parallaxStrength)
        {
            _parallaxStrength = parallaxStrength;
        }

        /// <summary>
        /// A method which allows the user to move this Background object by a given amount. <br></br>Can only change the position of Backgrounds which have scrolling enabled.
        /// </summary>
        /// <param name="moveBy">A Vector2 object containing the number of pixels to move this Background object by, in the x and y axis respectively.</param>
        public void Move(Vector2 moveBy)
        {
            if(_backgroundType != BackgroundType.Stretch) // prevents stretched backgrounds from moving
            {
                if (_backgroundType == BackgroundType.HorizontalScroll) // prevent vertical movement
                    moveBy.Y = 0;
                else if (_backgroundType == BackgroundType.VerticalScroll) // prevent horizontal movement
                    moveBy.X = 0;
                else if (_backgroundType == BackgroundType.Wrap) // prevent any movement
                    moveBy = Vector2.Zero;

                    _position -= moveBy * (1.0f / _layerDepth) * _parallaxStrength;
                WrapPositions();
            }
            else
                Console.WriteLine("Backgrounds using a BackgroundType of Stretch or Wrap cannot be moved.");
        }

        internal void WrapPositions()
        {
            var panelWidth = (int)Settings.GameResolution.X;
            var panelHeight = (int)Settings.GameResolution.Y;

            if (_position.X + panelWidth <= 0)
                _position += _panelPositions["Right"]; 
            if(_position.X >= panelWidth)
                _position += _panelPositions["Left"];
            if(_position.Y + panelHeight <= 0)
                _position += _panelPositions["Down"];
            if(_position.Y >= panelHeight)
                _position += _panelPositions["Up"];
        }

        internal bool IsInitialised()
        {
            return _image != null;
        }

        //TODO: Implement parallaxing using layer depth modifying the displacement of the camera since last frame.
        private void CalculateVelocity(float deltaTime)
        {
            var _prevOffset = Vector2.Zero;
            var _layer = 0.9f;
            var distance = Camera.Instance.Position - _prevOffset;
            var velocity = distance / deltaTime;
            var parallaxAmount = velocity * _layer;
        }

        internal Effect GetShaderEffect()
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Settings.ScreenDimensions.X, Settings.ScreenDimensions.Y, 0, 0, 1);
            Matrix uv_transform = GetUVTransform(_image, new Vector2(0, 0), 1f, _core.GraphicsDevice.Viewport);
            _backgroundEffect.Parameters["view_projection"].SetValue(Matrix.Identity * projection);
            _backgroundEffect.Parameters["uv_transform"].SetValue(Matrix.Invert(uv_transform));

            return _backgroundEffect;
        }

        private void InitialisePanelPositions(BackgroundType type)
        {
            var gameBounds = Settings.GameResolution;
            if (type == BackgroundType.FullScroll || type == BackgroundType.HorizontalScroll)
            {
                _panelPositions.Add("Left", new Vector2(-gameBounds.X, 0));
                _panelPositions.Add("Right", new Vector2(gameBounds.X, 0));
            }
            if (type == BackgroundType.FullScroll || type == BackgroundType.VerticalScroll)
            {
                _panelPositions.Add("Up", new Vector2(0, -gameBounds.Y));
                _panelPositions.Add("Down", new Vector2(0, gameBounds.Y));
            }
            if (type == BackgroundType.FullScroll)
            {
                _panelPositions.Add("UpLeft", new Vector2(-gameBounds.X, -gameBounds.Y));
                _panelPositions.Add("UpRight", new Vector2(gameBounds.X, -gameBounds.Y));
                _panelPositions.Add("DownLeft", new Vector2(-gameBounds.X, gameBounds.Y));
                _panelPositions.Add("DownRight", new Vector2(gameBounds.X, gameBounds.Y));
            }
        }

        /// <summary>
        /// Poor man's tweening function.
        /// If the result is stored in the value, it will create a nice interpolation over multiple frames.
        /// </summary>
        /// <param name="start">The value to start from.</param>
        /// <param name="target">The value to reach.</param>
        /// <param name="speed">A value between 0f and 1f.</param>
        /// <param name="snapNear">
        /// When the difference between the target and the result is smaller than this value, the target will be returned.
        /// </param>
        /// <returns></returns>
        private float Interpolate(float start, float target, float speed, float snapNear)
        {
            float result = MathHelper.Lerp(start, target, speed);

            if (start < target)
            {
                result = MathHelper.Clamp(result, start, target);
            }
            else
            {
                result = MathHelper.Clamp(result, target, start);
            }

            if (MathF.Abs(target - result) < snapNear)
            {
                return target;
            }
            else
            {
                return result;
            }
        }

        private Matrix GetView()
        {
            int width = Camera.Instance.GetRenderRectangle().Width;
            int height = Camera.Instance.GetRenderRectangle().Height;
            Vector2 translation = _position;

            Vector2 origin = new Vector2(width / 2f, height / 2f);

            return
                Matrix.CreateTranslation(-origin.X, -origin.Y, 0f) *
                Matrix.CreateTranslation(-translation.X, -translation.Y, 0f) *
                Camera.Instance.GetViewMatrix(Vector2.One) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_scale, _scale, 1f) *
                Matrix.CreateTranslation(origin.X, origin.Y, 0f);
        }

        private Matrix GetUVTransform(Texture2D t, Vector2 offset, float scale, Viewport v)
        {
            return
                Matrix.CreateScale(t.Width, t.Height, 1f) *
                Matrix.CreateScale(scale, scale, 1f) *
                Matrix.CreateTranslation(offset.X, offset.Y, 0f) *
                GetView() *
                Matrix.CreateScale(1f / v.Width, 1f / v.Height, 1f);
        }
    }
}
