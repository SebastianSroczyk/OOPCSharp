using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Maths;
using MonoGameEngine.StandardCore;
using System;

namespace MonoGameEngine
{
    /// <summary> </summary>
    public sealed class Camera
    {
        private FixedRatioRenderer _fixedRenderer;
        private Viewport _viewport; // Change this to use the FixedRatioRenderer's RenderTexture bounds to calculate camera position
        private Rectangle? _limits;

        private Vector2 _position;
        private Vector2 _targetPosition;
        private float _zoom;
        private float _easing;

        private Vector2 _shakeOffset;
        private float _shakeStrength = 0;
        private float _shakeMaxStrength = 0;
        private float _shakeDuration = 0;
        private float _shakeStartDuration = 0;
        private float _shakeStartAngle = 0;


        /// <summary>Provides access to this Camera object.</summary>
        public static Camera Instance { get; } = new Camera();

        private Camera() 
        {
            ClampWithinWorld = false;
        }

        internal void Initialise(Core core)
        {
            _fixedRenderer = new FixedRatioRenderer(core);
            _viewport = _fixedRenderer.GetRenderViewport();

            Origin = new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f);
            Zoom = 1.0f;
            _shakeOffset = Vector2.Zero;
            
        }

        internal void Update(float deltaTime)
        {
            Ease(deltaTime);
            PerformShake(deltaTime);
            ViewBounds = CalculateViewBounds();
        }

        internal void Ease(float deltaTime)
        {
            if (Easing > 0)
            {
                var direction = _targetPosition - _position;

                if (direction.Length() < 1)
                {
                    _position = _targetPosition;
                    if (ClampWithinWorld)
                    {
                        ValidatePosition();
                    }
                }
                else
                {
                    var velocity = direction.Length() * Easing;
                    direction.Normalize();

                    var moveTo = Vector2.SmoothStep(_position, _position + (direction * velocity * deltaTime), Easing);
                    if (!HorizontalLock)
                        _position.X = moveTo.X;
                    if (!VerticalLock)
                        _position.Y = moveTo.Y;

                    if (ClampWithinWorld)
                    {
                        ValidatePosition();
                    }
                }
            }
        }

        private void PerformShake(float deltaTime)
        {
            // Screenshake help from Jason Lautzenheiser: https://lautzofdotnet.wordpress.com/2016/07/31/screen-camera-shake-in-monogame/
            if (_shakeDuration > 0)
            {
                _shakeStrength = Wave.Cos(-_shakeMaxStrength / 2, 1.0f / _shakeStartDuration, _shakeDuration, _shakeMaxStrength / 2);

                _shakeStartAngle += 150 + Core.GetRandomNumber(60);
                _shakeOffset.X = (float)Math.Sin(_shakeStartAngle) * _shakeStrength;
                _shakeOffset.Y = (float)Math.Cos(_shakeStartAngle) * _shakeStrength;

                
                _shakeDuration -= deltaTime;

                // If the shake is over, reset all variables
                if (_shakeDuration < 0)
                {
                    _shakeDuration = _shakeStrength = _shakeStartAngle = 0;
                    _shakeOffset = Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if(Easing != 0f)
                {
                    _targetPosition = value;
                }
                else
                {
                    _position = value;
                }

                if (ClampWithinWorld)
                {
                    ValidatePosition();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Origin
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = Math.Clamp(value, 0.25f, 2.5f);
                if (ClampWithinWorld)
                {
                    ValidateZoom();
                    ValidatePosition();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Rotation
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Easing
        {
            get
            {
                return _easing;
            }
            set
            {
                _easing = Math.Max(0, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HorizontalLock
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool VerticalLock
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle? Limits
        {
            get
            {
                return _limits;
            }
            set
            {
                _limits = value;
                if(ClampWithinWorld)
                {
                    ValidateZoom();
                    ValidatePosition();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ClampWithinWorld
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle ViewBounds
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix(Vector2.One));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix(Vector2.One)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parallaxStrength"></param>
        /// <returns></returns>
        public Matrix GetViewMatrix(Vector2 parallaxStrength) 
        {
            return Matrix.CreateTranslation(new Vector3((-Position + _shakeOffset) * parallaxStrength, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void LookAt(Vector2 position)
        {
            Position = position - new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displacement"></param>
        /// <param name="respectRotation"></param>
        public void Move(Vector2 displacement, bool respectRotation = false)
        {
            if(respectRotation) 
            {
                displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(-Rotation));
            }

            Position += displacement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="respectRotation"></param>
        public void Move(float x, float y, bool respectRotation = false)
        {
            Move(new Vector2(x, y), respectRotation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strength"></param>
        /// <param name="duration"></param>
        public void Shake(float strength, float duration)
        {
            _shakeStartDuration = _shakeDuration = Math.Max(0, duration);
            _shakeStrength = Math.Clamp(strength, 0, 50);
            _shakeMaxStrength = _shakeStrength;
        }

        private void ValidateZoom()
        {
            // Sourced from David Gouveia's blog - https://www.david-gouveia.com/limiting-2d-camera-movement-with-zoom
            if (_limits.HasValue)
            {
                float minZoomX = (float)_viewport.Width / _limits.Value.Width;
                float minZoomY = (float)_viewport.Height / _limits.Value.Height;
                _zoom = Math.Max(_zoom, Math.Max(minZoomX, minZoomY));
            }
        }

        private void ValidatePosition()
        {
            // Sourced from David Gouveia's blog - https://www.david-gouveia.com/limiting-2d-camera-movement-with-zoom
            if (_limits.HasValue)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(GetViewMatrix(Vector2.One)));
                Vector2 cameraSize = new Vector2(_viewport.Width, _viewport.Height) / Zoom;
                Vector2 limitWorldMin = new Vector2(_limits.Value.Left, _limits.Value.Top);
                Vector2 limitWorldMax = new Vector2(_limits.Value.Right, _limits.Value.Bottom);
                Vector2 positionOffset = _position - cameraWorldMin;

                _position = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
            }
        }

        private Rectangle CalculateViewBounds()
        {
            // Code posted by Spool on the MonoGame forums - https://community.monogame.net/t/simple-2d-camera/9135
            var inverseViewMatrix = Matrix.Invert(GetViewMatrix(Vector2.One));
            _viewport = _fixedRenderer.GetRenderViewport();

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(_viewport.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, _viewport.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(_viewport.Width, _viewport.Height), inverseViewMatrix);

            // Find the minimum valued corner
            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));

            // Find the maximum valued corner
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        internal bool WithinFrustum(GameObject obj)
        {
            // Checks the collision bounds to ensure it is onscreen (even slightly)
            if (obj.GetSprite().IsInWorldSpace())
                return ViewBounds.Intersects(obj.GetBounds());
            // Always update screen space objects
            else
                return true;
        }

        internal Vector2 CalculateMouseScreenPosition(Vector2 rawPosition)
        {
            return _fixedRenderer.ScaleMouseToScreenCoordinates(rawPosition);
        }

        internal void BeginDraw()
        {
            _fixedRenderer.BeginDraw();
        }

        internal void EndDraw()
        {
            _fixedRenderer.EndDraw();
        }

        internal RenderTarget2D GetRender()
        {
            return _fixedRenderer.RenderTexture;
        }

        internal Rectangle GetRenderRectangle()
        {
            return _fixedRenderer.DestinationRect;
        }

    }
}
