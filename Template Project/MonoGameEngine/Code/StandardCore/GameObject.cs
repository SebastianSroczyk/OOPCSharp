using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Maths;
using System;
using System.Collections.Generic;

namespace MonoGameEngine.StandardCore
{
    /// <summary>A class which can be used as the base of any object needed within a game project.</summary>
    public abstract class GameObject
    {
        /// <summary>A reference to the current Screen, allowing access to some higher-level functionality.</summary>
        private Screen _currentScreen;

        private readonly AudioEmitter _audioEmitter = new AudioEmitter();

        /// <summary>A boolean property which allows velocity to automatically be added to the GameObject's position when set to 'true'.<br/> Value set to 'true' by default.</summary>
        public bool IsKinematic { get; protected set; } = true;

        /*CLASS VARIABLES*/
        private Vector2 _position;
        private Vector2 _previousPosition;
        private Vector2 _velocity;

        private float _pauseDuration = 0;
        private float _orbitTime = 0;

        private Sprite _sprite;
        private Texture2D _debugTexture;
        private Color _debugColour;

        private Rectangle _bounds;

        /*CLASS FLAGS*/
        private bool _isSolid = true;
        private bool _isVisible = true;
        private bool _isActive = true;
        private bool _drawDebug = false;

        /// <summary>
        /// Default constructor for this class.
        /// </summary>
        public GameObject() 
        {
        }

        /// <summary>
        /// Method which is automatically called once, immediately after the GameObject has been first added to the Screen.
        /// </summary>
        public virtual void OnceAdded() { }

        /// <summary>
        /// Must be overloaded by child classes. Allows code to be run every frame of the game. <b>Called automatically by the current Screen</b>.
        /// </summary>
        /// <param name="deltaTime">The elapsed time since the last frame.</param>
        public abstract void Update(float deltaTime);

        internal void PhysicsUpdate(float deltaTime)
        {
            if(_velocity != Vector2.Zero)
            {
                var direction = new Vector2(_velocity.X, _velocity.Y);
                direction.Normalize();

                direction.X *= Math.Abs(_velocity.X);
                direction.Y *= Math.Abs(_velocity.Y);

                _position += direction * Settings.GameSpeed;
                ResetBounds();
            }  
        }

        /// <summary>
        /// Performs the drawing of this GameObject. Can be overloaded by child classes as needed. <b>Called automatically by the current Screen.</b>.
        /// </summary>
        /// <param name="spriteBatch">The common SpriteBatch responsible for handling the sprites within the current game.</param>
        public virtual void Render(SpriteBatch spriteBatch)
        {
            if(_isVisible)
                _sprite.Draw(spriteBatch, _position);

            if (_drawDebug)
            {
                spriteBatch.Draw(_debugTexture, GetBounds(), _debugColour);
            }
                
        }

        /// <summary>
        /// This method sends out a RayCast in the given direction, and will check against all GameObjects in the current Screen.
        /// </summary>
        /// <param name="startPoint">The initial point of the raycast.</param>
        /// <param name="direction">The direction for the raycast to follow. Should be normalised (values between -1 and 1).</param>
        /// <param name="length">How far the ray should cast, in screen pixels.</param>
        /// <returns>Returns an array of objects hit by the projected raycast.</returns>
        protected GameObject[] Raycast(Vector2 startPoint, Vector2 direction, float length)
        {
            direction.Normalize(); // Normalise the direction just in-case.

            RayCast ray = new RayCast(startPoint, direction, length);
            List<GameObject> hitObjects = new List<GameObject>();

            foreach (GameObject obj in GetScreen().GetAllObjects())
            {
                if (obj.IsActive() && obj != this)
                {
                    if (ray.CheckBoundHit(obj.GetBounds()))
                    {
                        hitObjects.Add(obj);
                        obj.ReceiveRaycast(ray, this);
                    }
                }
            }

            return hitObjects.ToArray();
        }

        /// <summary>
        /// Virtual method that should be overloaded when looking for the GameObject to react to a received RayCast. <b>Called automatically by the game</b>.
        /// </summary>
        /// <param name="rayCast">The RayCast object that has been received.</param>
        /// <param name="sender">The GameObject which has sent out this RayCast.</param>
        protected virtual void ReceiveRaycast(RayCast rayCast, GameObject sender) { }

        /// <summary>
        /// Performs an immediate check for an intersection between this GameObject and other GameObjects of the requested TClass type.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <returns>Returns an array of other GameObjects if collisions are detected. Otherwise returns an empty array. <br/>Check the .Length property to see if any elements actually exist.</returns>
        protected GameObject[] GetAllIntersectingObjects<TClass>() where TClass : class
        {
            GameObject[] objects = _currentScreen.GetAllObjectsOfType<TClass>();
            List<GameObject> collided = new List<GameObject>();
            foreach (GameObject gameObject in objects)
            {
                if (gameObject != this && gameObject.IsSolid())
                {
                    if (GetBounds().Intersects(gameObject.GetBounds()))
                        collided.Add(gameObject);
                }
            }

            return collided.ToArray();
        }

        /// <summary>
        /// Performs an immediate check for an intersection between the given GameObject and another GameObject of the requested TClass type.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <returns>Returns the other GameObject if a collision is found. Otherwise returns 'null'.</returns>
        protected GameObject GetOneIntersectingObject<TClass>() where TClass : class
        {
            foreach(GameObject gameObject in _currentScreen.GetAllObjectsOfType<TClass>())
            {
                if (gameObject != this && gameObject.IsSolid())
                {
                    if (GetBounds().Intersects(gameObject.GetBounds()))
                        return gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Performs an immediate check for other GameObjects of the requested TClass type at the given offset from this GameObject's current position.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <param name="offset">The offset coordinates.</param>
        /// <returns>Returns an array of the other GameObjects if a collision is found at the offset. Otherwise returns an empty array.</returns>
        protected GameObject[] GetAllObjectsAtOffset<TClass>(Vector2 offset) where TClass : class
        {
            GameObject[] objects = _currentScreen.GetAllObjectsOfType<TClass>();
            List<GameObject> collided = new List<GameObject>();
            foreach (GameObject gameObject in objects)
            {
                if (gameObject != this && gameObject.IsSolid())
                {
                    if (gameObject.GetBounds().Contains(GetCenter() + offset))
                        collided.Add(gameObject);
                }
            }

            return collided.ToArray();
        }

        /// <summary>
        /// [Overload] Performs an immediate check for other GameObjects of the requested TClass type at the given offset from this GameObject's current position.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <param name="offsetX">The offset x-axis coordinate.</param>
        /// <param name="offsetY">The offset y-axis coordinate.</param>
        /// <returns>Returns an array of the other GameObjects if a collision is found at the offset. Otherwise returns an empty array.</returns>
        protected GameObject[] GetAllObjectsAtOffset<TClass>(int offsetX, int offsetY) where TClass : class
        {
            return GetAllObjectsAtOffset<TClass>(new Vector2(offsetX, offsetY));
        }

        /// <summary>
        /// Performs an immediate check for a GameObject of the requested TClass type at the given offset from this GameObject's current position.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <param name="offset">The offset coordinates.</param>
        /// <returns>Returns the other GameObject if a collision is found. Otherwise returns 'null'.</returns>
        protected GameObject GetOneObjectAtOffset<TClass>(Vector2 offset) where TClass : class
        {
            foreach (GameObject gameObject in _currentScreen.GetAllObjectsOfType<TClass>())
            {
                if (gameObject is TClass && gameObject != this && gameObject.IsSolid())
                {
                    if (gameObject.GetBounds().Contains(GetCenter() + offset))
                        return gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// [Overload] Performs an immediate check for a GameObject of the requested TClass type at the given offset from this GameObject's current position.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <param name="offsetX">The offset x-axis coordinate.</param>
        /// <param name="offsetY">The offset y-axis coordinate.</param>
        /// <returns>Returns the other GameObject if a collision is found. Otherwise returns 'null'.</returns>
        protected GameObject GetOneObjectAtOffset<TClass>(int offsetX, int offsetY) where TClass : class
        {
            return GetOneObjectAtOffset<TClass>(new Vector2(offsetX, offsetY));
        }

        /// <summary>
        /// Performs an immediate check for a GameObject of the requested TClass type within a circle boundary represented by the center point of this GameObject and its largest axis.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <param name="radius">[Optional] The radius length of the circle that should be used. If left null, the larger of the width/height parameters will be used.</param>
        /// <returns>Returns the other GameObject if a collision is found. Otherwise returns 'null'.</returns>
        protected GameObject GetOneObjectInRadius<TClass>(float? radius = null)
        {
            foreach (GameObject gameObject in _currentScreen.GetAllObjects())
            {
                if (gameObject is TClass && gameObject != this && gameObject.IsSolid())
                {
                    var difference = GetCenter() - gameObject.GetCenter();

                    // Ensure the radius is greater than 0
                    if (radius.HasValue && radius.Value <= 0)
                        radius = 1;

                    // Calculate the radii of the two circles
                    var thisRadius = radius ?? Math.Max(GetSprite().GetWidth(), GetSprite().GetHeight()) * 0.5f;
                    var otherRadius = Math.Max(gameObject.GetSprite().GetWidth(), gameObject.GetSprite().GetHeight()) * 0.5f;
                    var totalRadius = thisRadius + otherRadius;

                    // Square the numbers for the calculation
                    difference.X *= difference.X;
                    difference.Y *= difference.Y;
                    totalRadius *= totalRadius;

                    if (difference.X + difference.Y <= totalRadius) 
                        return gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Performs an immediate check for an intersection between the given GameObject and any another GameObject of the requested TClass type.
        /// </summary>
        /// <typeparam name="TClass">The subclass of GameObject that should be checked against.</typeparam>
        /// <returns>Returns 'true' if a collision has taken place between this GameObject and another of the given subclass. Otherwise, returns 'false'.</returns>
        protected bool IsTouching<TClass>()
        {
            foreach (GameObject gameObject in _currentScreen.GetAllObjects())
            {
                if (gameObject is TClass && gameObject != this && gameObject.IsSolid())
                {
                    if (GetBounds().Intersects(gameObject.GetBounds()))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Performs a check on this GameObject's current position against the boundaries of the game window.
        /// </summary>
        /// <returns>Returns 'true' if an overlap with the screen boundaries is detected. Otherwise, returns 'false'.</returns>
        protected bool IsAtScreenEdge()
        {
            Rectangle bounds = GetBounds();
            Rectangle viewBounds = Camera.Instance.ViewBounds;

            // Check for collision with the boundaries of the world in each direction
            if (bounds.Left <= viewBounds.X 
                || bounds.Right >= viewBounds.Width 
                || bounds.Top <= viewBounds.Y 
                || bounds.Bottom >= viewBounds.Height)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs a check to see if this GameObject is actually visible on the current Screen.
        /// </summary>
        /// <returns>Returns 'true' if this GameObject is completely off-screen in any direction. Otherwise, returns 'false'.</returns>
        protected bool IsOffscreen()
        {
            Rectangle bounds = GetBounds();
            Rectangle viewBounds = Camera.Instance.ViewBounds;

            // Check for collision with the boundaries of the world in each direction
            if (bounds.Right <= viewBounds.X 
                || bounds.Left >= viewBounds.Width 
                || bounds.Bottom <= viewBounds.Y 
                || bounds.Top >= viewBounds.Height)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Incrementally moves the GameObject towards a target position. Uses deltaTime to provide a smoother movement effect.
        /// <br/>Only works when IsKinematic property is set to 'false' for this GameObject.
        /// </summary>
        /// <param name="target">The on-screen position that the GameObject should move towards.</param>
        /// <param name="deltaTime">The time (in seconds) since the last frame of the game.</param>
        /// <param name="speed">[Optional] The speed at which the GameObject should move towards target. 1.0f by default. Clamped between 1.0f and 100.0f.</param>
        protected void MoveTowards(Vector2 target, float deltaTime, float speed = 1.0f)
        {
            if(IsKinematic)
            {
                speed = Math.Clamp(speed, 1.0f, 100.0f);
                speed *= 10f; // Scale all speeds so that the base speed (1.0f) feels reasonably quick.

                var direction = (target - _position);
                if (direction.Length() >= 0.05f) // If there is some amount of distance between the GameObject and the target
                {
                    direction.Normalize();
                    var prevPosition = _position;
                    var newPosition = _position;
                    newPosition += direction * deltaTime * speed;

                    if ((prevPosition.X < target.X && newPosition.X > target.X)
                        || (prevPosition.X > target.X && newPosition.X < target.X))
                        newPosition.X = target.X;

                    if ((prevPosition.Y < target.Y && newPosition.Y > target.Y)
                        || (prevPosition.Y > target.Y && newPosition.Y < target.Y))
                        newPosition.Y = target.Y;

                    SetPosition(newPosition);
                }
            }
        }

        /// <summary>
        /// Immediately rotates this GameObject to look towards the given target.
        /// </summary>
        /// <param name="targetPosition">The screen position that this GameObject should turn to face.</param>
        protected void LookAtPosition(Vector2 targetPosition)
        {
            try
            {
                var direction = targetPosition - GetCenter();
                GetSprite().SetRotation(MathHelper.ToDegrees((float)Math.Atan2(direction.Y, direction.X)));
            }
            catch(NullReferenceException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Orbits around a given point in a circular motion.
        /// </summary>
        /// <param name="center">The on-screen position that the GameObject should orbit around.</param>
        /// <param name="loopsPerSecond">The number of times an orbit cycle will be completed per second. Automatically clamped between 0.05f and 5.0f.</param>
        /// <param name="radius">The distance at which the GameObject should orbit around the given center.</param>
        /// <param name="deltaTime">The time (in seconds) since the last frame of the game.</param>
        /// <param name="orbitClockwise">[Optional] Control to choose which direction the orbit should travel. 'true' by default.</param>
        protected void OrbitAround(Vector2 center, float loopsPerSecond, float radius, float deltaTime, bool orbitClockwise = true)
        {
            _orbitTime += deltaTime;
            _previousPosition = GetPosition();

            loopsPerSecond = Math.Clamp(loopsPerSecond, 0.05f, 5.0f);
            if(orbitClockwise)
                SetPosition(center.X + Wave.Sin(radius, loopsPerSecond, _orbitTime), center.Y - Wave.Cos(radius, loopsPerSecond, _orbitTime));
            else
                SetPosition(center.X - Wave.Sin(radius, loopsPerSecond, _orbitTime), center.Y - Wave.Cos(radius, loopsPerSecond, _orbitTime));
        }

        /// <summary>
        /// Pauses updates for this GameObject for the specified duration. Updates will resume automatically after the pause duration has elapsed.
        /// </summary>
        /// <param name="pauseDuration">The time, in seconds, that this GameObject should pause for.</param>
        public void Pause(float pauseDuration)
        {
            _pauseDuration = Math.Max(0, pauseDuration);
            if(_pauseDuration > 0)
                _isActive = false;
        }

        internal void UpdatePause(float deltaTime)
        {
            if(_pauseDuration > 0)
            {
                _pauseDuration -= deltaTime;
                if (_pauseDuration <= 0)
                {
                    _pauseDuration = 0;
                    _isActive = true;
                }
            }    
        }

        internal bool IsDebug()
        {
            return _drawDebug;
        }

        /// <summary>
        /// A getter function for this GameObject's screen position.
        /// </summary>
        /// <returns>A Vector2 object representing the on-screen position of this GameObject.</returns>
        public Vector2 GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// A getter function which returns this GameObject's x-axis screen position.
        /// </summary>
        /// <returns>A floating-point value representing this GameObject's horizontal screen position, in pixels.</returns>
        public float GetX()
        {
            return _position.X;
        }

        /// <summary>
        /// A getter function which returns this GameObject's y-axis screen position.
        /// </summary>
        /// <returns>A floating-point value representing this GameObject's vertical screen position, in pixels.</returns>
        public float GetY()
        {
            return _position.Y;
        }

        /// <summary>
        /// A getter function for this GameObject's velocity.
        /// </summary>
        /// <returns>A Vector2 object representing the current velocity of this GameObject.</returns>
        public Vector2 GetVelocity()
        {
            return _velocity;
        }

        /// <summary>
        /// A getter function for this GameObject's sprite or animated sprite.
        /// </summary>
        /// <returns>The Sprite currently representing this GameObject on-screen.</returns>
        public Sprite GetSprite()
        {
            try
            {
                return _sprite;
            }
            catch(NullReferenceException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// A getter function for this GameObject's bounding box.
        /// </summary>
        /// <returns>The bounding box used by this GameObject.</returns>
        public Rectangle GetBounds()
        {
            //_bounds.Location = new Point((int)_position.X - (int)GetSprite().GetOrigin().X, (int)_position.Y - (int)(GetSprite().GetOrigin().Y));
            return _bounds;
        }

        /// <summary>
        /// A getter function for this GameObject's 'solid' flag, for collision purposes.
        /// </summary>
        /// <returns>A boolean value representing whether or not this GameObject should be considered 'solid'.</returns>
        public bool IsSolid()
        {
            return _isSolid;
        }

        /// <summary>
        /// A getter function for this GameObject's 'visible' flag, for rendering purposes.
        /// </summary>
        /// <returns>A boolean value representing whether or not this GameObject should be considered 'visible'.</returns>
        public bool IsVisible()
        {
            return GetSprite() == null ? false : GetSprite().IsVisible;
        }

        /// <summary>
        /// A getter function for this GameObject's 'active' flag, for updating purposes.
        /// </summary>
        /// <returns>A boolean value representing whether or not this GameObject is currently receiving updates.</returns>
        public bool IsActive()
        {
            return _isActive;
        }

        /// <summary>
        /// Calculates the center point of this GameObject, using its bounding box.
        /// </summary>
        /// <returns>A Vector2 containing the coordinates of the center point of this GameObject.</returns>
        public Vector2 GetCenter()
        {
            _bounds.Location = new Point((int)_position.X - (int)GetSprite().GetOrigin().X, (int)_position.Y - (int)(GetSprite().GetOrigin().Y));
            Vector2 center = new Vector2()
            {
                X = _bounds.Center.X,
                Y = _bounds.Center.Y
            };
            return center;
        }

        /// <summary>
        /// A getter function which will return the GameObject's sprite, if it currently uses an animated sprite.
        /// </summary>
        /// <returns>The sprite being used by the GameObject (cast as an AnimatedSprite) if appropriate. Otherwise returns 'null'.</returns>
        public AnimatedSprite GetAnimatedSprite()
        {
            if (_sprite is AnimatedSprite sprite)
                return sprite;
            else
                return null;
        }

        /// <summary>
        /// A getter function to access the current Screen.
        /// </summary>
        /// <returns>A reference to the current Screen object.</returns>
        protected Screen GetScreen()
        {
            return _currentScreen;
        }

        /// <summary>
        /// A setter function to reposition this GameObject onscreen.
        /// </summary>
        /// <param name="position">A Vector2 object containing the x and y co-ordinates for the new position.</param>
        public void SetPosition(Vector2 position)
        {
            _previousPosition = _position;
            _position = position;
            GetSprite().UpdatePosition(_position);
            _audioEmitter.Position = new Vector3(position.X, position.Y, 0.0f);
            _audioEmitter.Forward = new Vector3(0, 0, 1);
            ResetBounds();
        }

        /// <summary>
        /// [Overload] A setter function to reposition this GameObject onscreen.
        /// </summary>
        /// <param name="x">An integer value representing the new position for this GameObject in the x-axis.</param>
        /// <param name="y">An integer value representing the new position for this GameObject in the y-axis.</param>
        public void SetPosition(int x, int y)
        {
            SetPosition(new Vector2(x, y));
        }

        /// <summary>
        /// [Overload] A setter function to reposition this GameObject onscreen.
        /// </summary>
        /// <param name="x">A floating-point value representing the new position for this GameObject in the x-axis.</param>
        /// <param name="y">A floating-point value representing the new position for this GameObject in the y-axis.</param>
        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        /// <summary>
        /// A function which reverts this GameObject's position to the co-ordinates from before the most recent call to SetPosition().
        /// </summary>
        protected void RevertPosition()
        {
            _position = _previousPosition;
            ResetBounds();
        }

        /// <summary>
        /// [Overload] A function which reverts this GameObject's position to the co-ordinates from before the most recent call to SetPosition().
        /// </summary>
        /// <param name="revertToX">A boolean value representing whether or not to revert the x co-ordinates.</param>
        /// <param name="revertToY">A boolean value representing whether or not to revert the y co-ordinates.</param>
        protected void RevertPosition(bool revertToX, bool revertToY)
        {
            if (revertToX)
                _position.X = _previousPosition.X;
            if (revertToY)
                _position.Y = _previousPosition.Y;
        }

        /// <summary>
        /// A function which adds the given deltaVelocity to the current velocity of this GameObject.
        /// </summary>
        /// <param name="deltaVelocity">A Vector2 object representing the amount of velocity to add to the existing velocity.</param>
        public void AddVelocity(Vector2 deltaVelocity)
        {
            _velocity += deltaVelocity;
        }

        /// <summary>
        /// [Overload] A function which adds the given deltaVelocity to the current velocity of this GameObject.
        /// </summary>
        /// <param name="xVelocity">A floating point value representing the change desired to this GameObject's horizontal velocity.</param>
        /// <param name="yVelocity">A floating point value representing the change desired to this GameObject's vertical velocity.</param>
        public void AddVelocity(float xVelocity, float yVelocity)
        {
            AddVelocity(new Vector2(xVelocity, yVelocity));
        }

        /// <summary>
        /// A setter function which allows the velocity of this GameObject to be directly changed.
        /// </summary>
        /// <param name="velocity">A Vector2 object representing the new horizontal and vertical velocities of this GameObject.</param>
        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        /// <summary>
        /// [Overload] A setter function which allows the velocity of this GameObject to be directly changed.
        /// </summary>
        /// <param name="xVelocity">A floating point value representing the GameObject's horizontal velocity.</param>
        /// <param name="yVelocity">A floating point value representing the GameObject's vertical velocity.</param>
        public void SetVelocity(float xVelocity, float yVelocity)
        {
            SetVelocity(new Vector2(xVelocity, yVelocity));
        }

        /// <summary>
        /// A setter function which allows this GameObject to start or stop actively updating (i.e. automatic calls to the Update() method). Rendering will still take place regardless of whether or not this GameObject is currently updating.
        /// </summary>
        /// <param name="isActive">A boolean value representing whether or not this GameObject should be actively updated.</param>
        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// A setter function which allows this GameObject to start or stop rendering to the screen. Collisions and other functional updates will still occur regardless of whether or not rendering takes place.
        /// </summary>
        /// <param name="isVisible">A boolean value representing whether or not this GameObject should be rendered to the screen.</param>
        public void SetVisible(bool isVisible)
        {
            if (GetSprite() != null)
                GetSprite().IsVisible = isVisible;
        }

        /// <summary>
        /// A setter function which enables or disables the drawing of a rectangle representing this GameObject's bounding box.
        /// </summary>
        /// <param name="drawDebug">A boolean value representing whether or not to render this GameObject's bounds.</param>
        /// <param name="drawColour">[Optional] A Color object representing the colour desired for rendering the debug bounds. Automatically sets Alpha value to 128.</param>
        protected void SetDrawDebug(bool drawDebug, Color? drawColour = null)
        {
            _drawDebug = drawDebug;

            if (drawColour != null)
                SetDebugColour((Color)drawColour);
            else if (_debugColour.A == 0) // If a colour hasn't already been set, use the default.
                SetDebugColour(Color.White);

            if (_debugTexture == null) // Only assigns the texture if needed (by calling this function).
                _debugTexture = Core.GetResource<Texture2D>("misc/Pixel");
        }

        /// <summary>
        /// A setter function which can adjust the render colour of the debug bounds rendering for this GameObject.
        /// </summary>
        /// <param name="colour">A Color object representing the colour desired for rendering the debug bounds. Automatically sets Alpha value to 128.</param>
        private void SetDebugColour(Color colour)
        {
            _debugColour = colour;
            _debugColour.A = 128;
        }

        /// <summary>
        /// A setter function which can adjust the bounding box of this GameObject.
        /// </summary>
        /// <param name="bounds">A Rectangle object representing the desired bounding box dimensions.</param>
        public void SetBounds(Rectangle bounds)
        {
            _bounds = bounds;
            ResetBounds();
        }

        /// <summary>
        /// [Overload] A setter function which can adjust the bounding box of this GameObject.
        /// </summary>
        /// <param name="width">The desired width of the bounding box.</param>
        /// <param name="height">The desired height of the bounding box.</param>
        public void SetBounds(int width, int height)
        {
            _bounds.Width = width;
            _bounds.Height = height;
            ResetBounds();
        }

        /// <summary>
        /// A setter function for assigning a new Sprite to visually represent this GameObject.
        /// </summary>
        /// <param name="sprite">The Sprite instance that should be used by this GameObject.</param>
        /// <param name="inWorldSpace">[Optional] A boolean representing whether to draw this Sprite in world space or screen space.</param>
        public void SetSprite(Sprite sprite, bool inWorldSpace = true)
        {
            _sprite = sprite;
            _sprite.SetInWorldSpace(inWorldSpace);
            ResetBounds();
            _bounds.Width = _sprite.GetWidth();
            _bounds.Height = _sprite.GetHeight();
        }

        /// <summary>
        /// [Overload] A setter function for assigning a new Sprite to visually represent this GameObject.
        /// </summary>
        /// <param name="spriteImage">The image file that should be used by this GameObject's new Sprite.</param>
        /// <param name="inWorldSpace">[Optional] A boolean representing whether to draw this Sprite in world space or screen space.</param>
        public void SetSprite(Texture2D spriteImage, bool inWorldSpace = true)
        {
            SetSprite(new Sprite(spriteImage), inWorldSpace);
        }

        /// <summary>
        /// [Overload] A setter function for assigning a new Sprite to visually represent this GameObject.
        /// </summary>
        /// <param name="spriteName">The name of an image file that should be used by this GameObject's new Sprite.</param>
        /// <param name="inWorldSpace">[Optional] A boolean representing whether to draw this Sprite in world space or screen space.</param>
        public void SetSprite(string spriteName, bool inWorldSpace = true)
        {
            SetSprite(new Sprite(Core.GetResource<Texture2D>("images/" + spriteName)), inWorldSpace);
        }

        /// <summary>
        /// [Overload] A setter function for assigning a new AnimatedSprite to visually represent this GameObject.
        /// </summary>
        /// <param name="spritesheet">The name of the spritesheet that should be used for rendering.</param>
        /// <param name="frameWidth">The width of each frame of animation in the spritesheet (in pixels).</param>
        /// <param name="frameHeight">The height of each frame of animation in the spritesheet (in pixels).</param>
        /// <param name="animationSpeed">The speed at which the animation should cycle between its frames (in seconds).</param>
        /// <param name="framesPerAnimation">An array of integers, with each integer representing the frame count of the corresponding animation sequence.</param>
        /// <param name="loopType">[Optional] The type of animation looping that should be used. 'LoopType.Standard' by default.</param>
        /// <param name="inWorldSpace">[Optional] A boolean representing whether to draw this Sprite in world space or screen space.</param>
        public void SetSprite(string spritesheet, int frameWidth, int frameHeight, float animationSpeed, int[] framesPerAnimation, LoopType loopType = LoopType.Standard, bool inWorldSpace = true)
        {
            _sprite = new AnimatedSprite(Core.GetResource<Texture2D>("images/" + spritesheet));
            _sprite.SetInWorldSpace(inWorldSpace);
            ((AnimatedSprite)_sprite).Initialise(frameWidth, frameHeight, animationSpeed, framesPerAnimation, loopType);
            _bounds.Width = _sprite.GetWidth();
            _bounds.Height = _sprite.GetHeight();
        }

        /// <summary>
        /// [Overload] A setter function for assigning a new AnimatedSprite to visually represent this GameObject.
        /// </summary>
        /// <param name="spritesheet">The name of the spritesheet that should be used for rendering.</param>
        /// <param name="frameSize">The width and height of each frame of animation in the spritesheet. This requires cells of matching widths and heights.</param>
        /// <param name="animationSpeed">The speed at which the animation should cycle between its frames (in seconds).</param>
        /// <param name="framesPerAnimation">An array of integers, with each integer representing the frame count of the corresponding animation sequence.</param>
        /// <param name="loopType">[Optional] The type of animation looping that should be used. 'LoopType.Standard' by default.</param>
        /// <param name="inWorldSpace">[Optional] A boolean representing whether to draw this Sprite in world space or screen space.</param>
        public void SetSprite(string spritesheet, int frameSize, float animationSpeed, int[] framesPerAnimation, LoopType loopType = LoopType.Standard, bool inWorldSpace = true)
        {
            SetSprite(spritesheet, frameSize, frameSize, animationSpeed, framesPerAnimation, loopType, inWorldSpace);
        }

        /// <summary>
        /// A setter function which can assign a new parent Screen to this GameObject. <b>Called automatically by the current Screen</b>.
        /// </summary>
        /// <param name="screen">A Screen object representing the new parent object of this GameObject.</param>
        internal void SetScreen(Screen screen)
        {
            _currentScreen = screen;
        }

        internal AudioEmitter GetEmitter()
        {
            _audioEmitter.Velocity = new Vector3(_velocity, 0);
            _audioEmitter.Up = new Vector3(0, -1, 0);
            _audioEmitter.DopplerScale = 1000f;
            return _audioEmitter;
        }

        private void ResetBounds()
        {
            // Match the GameObject position to the Rectangle position.
            //if (GetSprite().IsInWorldSpace())
            //{
                _bounds.X = (int)_position.X;
                _bounds.Y = (int)_position.Y;
            //}
            //else
            //{
            //    var worldPosition = Camera.Instance.ScreenToWorld(_position);
            //    _bounds.X = (int)worldPosition.X;
            //    _bounds.Y = (int)worldPosition.Y;
            //}

            // Apply offset if necessary to match both center points
            _bounds.X -= (_bounds.Width - ((int)(GetSprite().GetWidth() * GetSprite().GetScale().X))) / 2;
            _bounds.Y -= (_bounds.Height - ((int)(GetSprite().GetHeight() * GetSprite().GetScale().Y))) / 2;
        }
    }
}
