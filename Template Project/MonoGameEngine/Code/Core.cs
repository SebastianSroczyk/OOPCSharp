using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameEngine.Extended;
using MonoGameEngine.ComponentCore;
using MonoGameEngine.StandardCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameEngine
{
    /// <summary>A class which represents the highest-level functionality of a game project.</summary>
    public abstract class Core : Game
    {
        private static ContentManager _content;
        private static Random _randomGenerator;
        private static ShapeBatcher _shapeBatcher;
        private static GameTime _gameTime;

        internal readonly GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;  
        
        private Queue<KeyValuePair<string, string>> _queuedMessages;
        private static List<Schedulable> _scheduledInvocations;

        private Effect _screenEffect = null;

        private IScreen _currentScreen;
        private IScreen _nextScreen;

        private static bool _gameRunning = true;
        private static float _updateDelay = 0;

        /// <summary>The constructor for this class.</summary>
        protected Core()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Settings.Initialise(this, _graphics);
            
            Content.RootDirectory = "Content";
            _content = Content;

            IsMouseVisible = false;
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += UpdateCameraResolution;

            _randomGenerator = new Random();
            _scheduledInvocations = new List<Schedulable>();
            _queuedMessages = new Queue<KeyValuePair<string, string>>();

            Transition.Instance.Setup(this);
        }

        private void UpdateCameraResolution(object sender, EventArgs e)
        {
            Window.ClientSizeChanged -= UpdateCameraResolution;

            Window.ClientSizeChanged += UpdateCameraResolution;
            
            Camera.Instance.Initialise(this);
        }

        /// <summary>Performs initial setup for various systems. <br/><b>Should always call base.Initialize() in derived classes.</b></summary>
        protected override void Initialize()
        {
            base.Initialize();

            //Camera2D.Instance.Initialise(this);
            Camera.Instance.Initialise(this);

            // Timing and FPS settings
            _graphics.HardwareModeSwitch = false; // changes how the fullscreen toggle works
            _graphics.ApplyChanges();
            IsFixedTimeStep = true;

            GameInput.InitialiseGamePads();
            

            _shapeBatcher = new ShapeBatcher(this);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

			try
			{
                _screenEffect = Content.Load<Effect>("misc/GrayscaleEffect");
            }
            catch
			{
                Console.Error.WriteLine("Error loading shader: GrayscaleEffect shader missing from \"misc\" content folder.");
			}
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * Settings.GameSpeed;
            if (_gameRunning && _updateDelay <= 0)
            {               
                GameInput.Update(deltaTime);
                Transition.Instance.Update(deltaTime);
                AudioManager.Instance.Update(deltaTime);

                HandleMessages();

                UpdateScreen(deltaTime);

                //Camera2D.Instance.Update(deltaTime);
                Camera.Instance.Update(deltaTime);

                UpdateSchedulables(deltaTime);

                base.Update(gameTime);
            }
            if(_updateDelay > 0)
            {
                _updateDelay -= deltaTime;
                if (_updateDelay < 0)
                    _updateDelay = 0;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _currentScreen.End();
                Exit();
            }

            if (GameInput.IsKeyPressed(Keys.F11))
            {
                Settings.IsFullscreen = !Settings.IsFullscreen;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _currentScreen.PreRender(_spriteBatch);

            _currentScreen.Render(_spriteBatch);

            _currentScreen.PostRender(_spriteBatch);

            if (_screenEffect != null)
                _screenEffect.Parameters["grayscaleStrength"].SetValue(((Screen)_currentScreen).GetColourSaturation());

            _spriteBatch.Begin(samplerState: SamplerState.AnisotropicClamp, effect: _screenEffect); // Render game buffer from camera
            _spriteBatch.Draw(Camera.Instance.GetRender(), Camera.Instance.GetRenderRectangle(), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);
            Transition.Instance.Render(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        [Obsolete("Message handling has not been fully implemented. Please consider using parameter passing instead.")]
        internal void Receive(MessageType messageType, string message)
        {
            _queuedMessages.Enqueue(new KeyValuePair<string, string>(messageType.ToString(), message));
        }

        private void UpdateSchedulables(float deltaTime)
        {
            List<Schedulable> completed = new List<Schedulable>();
            foreach(Schedulable scheduled in _scheduledInvocations)
            {
                scheduled.UpdateTime(deltaTime);
                if (scheduled.IsReadyToInvoke())
                {
                    scheduled.Invoke();
                    completed.Add(scheduled);
                } 
            }

            for (int i = 0; i < completed.Count; i++)
            {
                _scheduledInvocations.Remove(completed[i]);
            }
            completed.Clear();
        }

        // METHOD INCOMPLETE
        protected virtual void HandleMessages() 
        {
            for (int i = 0; i < _queuedMessages.Count; i++)
            {
                var message = _queuedMessages.Dequeue();

                if (message.Key == MessageType.SCREEN_SWAP.ToString())
                {
                    //SwapScreen(message.Value);
                }
            }
        }

        /// <summary>
        /// Performs the actual hand-off of an instantiated new Screen to the Core's managed reference.
        /// </summary>
        internal void SwapScreen()
        {
            if (_currentScreen != null)
                _currentScreen.End();

            _currentScreen = _nextScreen;
            _nextScreen = null;

            _currentScreen.Start(this);
        }

        /// <summary>
        /// This method allows a new Screen derivative to be created and start running in place of the current one, if one exists.
        /// </summary>
        /// <typeparam name="TScreen">The Type of Screen that you would like to start.</typeparam>
        public void StartScreen<TScreen>() where TScreen : IScreen, new()
        {
            if (_currentScreen == null || !_currentScreen.GetType().Equals(typeof(TScreen)))
                _nextScreen = new TScreen();
            else
                throw new ArgumentException("Requested Screen of type " + typeof(TScreen) + " is already active.");

            if (_currentScreen == null) // If there is no current screen already running, set it up straight away
                SwapScreen();
        }

        /// <summary>
        /// Handles the top-level Screen instance operation.
        /// </summary>
        /// <param name="deltaTime">The time (in seconds) since the last frame of the game.</param>
        private void UpdateScreen(float deltaTime)
        {
            _currentScreen.Update(deltaTime);            
            //if(_nextScreen != null && !Transition.Instance.IsActive())
                //SwapScreen();
        }

        internal void RefreshWindow()
        {
            Camera.Instance.Initialise(this);
        }

        internal IScreen GetNextScreen()
        {
            return _nextScreen;
        }

        internal IScreen GetScreen()
        {
            return _currentScreen;
        }

        /// <summary>
        /// A getter method for this Core's ContentManager.
        /// </summary>
        /// <returns>Returns the game's ContentManager instance.</returns>
        internal static ContentManager GetContent()
        {
            return _content;
        }

        internal static ShapeBatcher GetShapeBatcher()
        {
            return _shapeBatcher;
        }

        /// <summary>
        /// Handles the loading of a desired resource. 
        /// </summary>
        /// <typeparam name="T">The Type of the resource required.</typeparam>
        /// <param name="resourceName">The name of the desired resource. Filetype suffix is not required.</param>
        /// <returns>Returns the requested media resource if found, otherwise throws an error.</returns>
        public static T GetResource<T>(string resourceName)
        {
            try
            {
                T resource = _content.Load<T>(resourceName);
                return resource;
            }
            catch (ContentLoadException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// A getter method for this Core's GameWindow.
        /// </summary>
        /// <returns>Returns the game's GameWindow instance.</returns>
        internal GameWindow GetWindow()
        {
            return Window;
        }

        /// <summary>
        /// A method which generates a pseudo-random integer value from 0 to the upper bound provided.
        /// </summary>
        /// <param name="max">The upper bound to the random number generation (non-inclusive). If a value below 0 is entered, the maximum bound will be set to 0.</param>
        /// <returns>Returns a whole number within the established range.</returns>
        public static int GetRandomNumber(int max)
        {
            if (max < 0) max = 0;
            return _randomGenerator.Next(max);
        }

        /// <summary>
        /// [Overload] A method which generates a pseudo-random floating-point value from 0 to the upper bound provided.
        /// </summary>
        /// <param name="max">The upper bound to the random number generation (non-inclusive). If a value below 0 is entered, the maximum bound will be set to 0.</param>
        /// <returns>Returns a floating-point number within the established range.</returns>
        public static float GetRandomNumber(float max)
        {
            if (max < 0) max = 0;
            return (float)_randomGenerator.NextDouble() * max;
        }

        /// <summary>
        /// [Overload] A method which generates a pseudo-random floating-point value from the lower bound to the upper bound provided.<br/>If the min is greater than the max provided, the two values will be swapped.
        /// </summary>
        /// <param name="min">The lower bound of the random number generation (inclusive).</param>
        /// <param name="max">The upper bound to the random number generation (non-inclusive). If a value below 0 is entered, the maximum bound will be set to 0.</param>
        /// <returns>Returns a floating-point number within the established range.</returns>
        public static float GetRandomNumber(float min, float max)
        {
            if(min > max)
            {
                (max, min) = (min, max);
            }

            return ((float)_randomGenerator.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// The game will stop updating once this method is called.
        /// </summary>
        public static void EndGame()
        {
            _gameRunning = false;
        }

        /// <summary>
        /// An estimation of the current FPS (frames per second) will be printed to the program's output window.
        /// </summary>
        public static void PrintFPS()
        {
            Console.WriteLine("FPS = " + (int)(1 / _gameTime.ElapsedGameTime.TotalSeconds));
        }

        internal void SetFrameRate(int fps)
        {
            TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / Math.Max(fps, 0)));
        }

        internal int GetFrameRate()
        {
            return (int)Math.Round((1 / TargetElapsedTime.TotalSeconds));
        }

        /// <summary>
        /// The Core will call the method belonging to the specified object after the specified time has elapsed. <br/><b>Note:</b> This is an expensive way to call the desired method, and should only be used as a last resort.
        /// </summary>
        /// <param name="callingObject">The object that the desired method belongs to.</param>
        /// <param name="methodName">The name of the method that should be called, which belongs to the given object.</param>
        /// <param name="arguments">A generic array of values that should be handed to the desired method when invoked. The values should be in the same order and of the same type as the method would normally receive.</param>
        /// <param name="scheduleFor">The time, in seconds, that should be waited before the method is invoked.</param>
        public static void Schedule(object callingObject, string methodName, object[] arguments, float scheduleFor)
        {
            scheduleFor = Math.Max(0, scheduleFor); // Ensures a negative time cannot be used
            _scheduledInvocations.Add(new Schedulable(callingObject, methodName, arguments, scheduleFor));
        }

        /// <summary>
        /// [Overload] The Core will call the method belonging to the specified object after the specified time has elapsed. <br/><b>Note:</b> This is an expensive way to call the desired method, and should only be used as a last resort.
        /// </summary>
        /// <param name="callingObject">The object that the desired method belongs to.</param>
        /// <param name="methodName">The name of the method that should be called, which belongs to the given object.</param>
        /// <param name="argument">A generic value that should be handed to the desired method when invoked. The value supplied should be of the same type as the desired method would normally expect.</param>
        /// <param name="scheduleFor">The time, in seconds, that should be waited before the method is invoked.</param>
        public static void Schedule(object callingObject, string methodName, object argument, float scheduleFor)
        {
            Schedule(callingObject, methodName, new object[] { argument }, scheduleFor);
        }

        /// <summary>
        /// The game's entire update will pause for the requested duration (in seconds). Duration should be greater than 0. <br/>
        /// <strong>NOTE:</strong> This will also pause AudioManager, Transition and Camera, along with GameInput.
        /// </summary>
        /// <param name="duration">The timeframe (in seconds) that the game should pause for.</param>
        public static void Pause(float duration)
        {
            if (_updateDelay == 0)
                _updateDelay = Math.Max(duration, 0);
            else
                Console.Error.WriteLine("Delay already in effect. New delay request has been ignored.");
        }

        /// <summary>
        /// Property which provides the directory to the root Content folder of this project.
        /// </summary>
        public static string RootContentDirectory
        {
            get
            {
                return _content.RootDirectory;
            }
        }
    }
    

    /* EXTENSION METHODS for Engine */
    /// <summary>Class which provides additional (extended) functionality for several other classes.</summary>
    public static class Extensions
    {
        /// <summary>
        /// Draws a filled rectangle at the given position with the dimensions requested.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch currently being used to batch for rendering.</param>
        /// <param name="position">The position on-screen that the rectangle should be drawn at.</param>
        /// <param name="width">The desired width of the rectangle to be drawn.</param>
        /// <param name="height">The desired height of the rectangle to be drawn.</param>
        /// <param name="colour">[Optional] The desired rendering colour.</param>
        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, float width, float height, Color? colour = null)
        {
            Core.GetShapeBatcher().DrawRectangle(position.X, position.Y, width, height, colour);
        }

        /// <summary>
        /// [Overload] Draws a filled rectangle using the given dimensions.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch currently being used to batch for rendering.</param>
        /// <param name="rectangle">The dimensions of the rectangle to be drawn.</param>
        /// <param name="color">[Optional] The desired rendering colour.</param>
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color? color = null)
        {
            spriteBatch.DrawRectangle(rectangle.Location.ToVector2(), rectangle.Width, rectangle.Height, color);
        }

        /// <summary>
        /// Draws a line segment from point1 to point 2.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch currently being used to batch for rendering.</param>
        /// <param name="point1">The starting point of the desired line segment.</param>
        /// <param name="point2">The ending point of the desired line segment.</param>
        /// <param name="thickness">[Optional] The thickness of the line. 1.0f by default.</param>
        /// <param name="colour">[Optional] The desired rendering colour.</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, float thickness = 1.0f, Color? colour = null)
        {
            Core.GetShapeBatcher().DrawLine(point1, point2, thickness, colour);
        }

        /// <summary>
        /// Draws a circle at a given origin with the requested radius. The circle will look smoother depending on how many points are requested.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch currently being used to batch for rendering.</param>
        /// <param name="origin">The center point that the circle should use for rendering.</param>
        /// <param name="radius">The radius of the desired circle to be rendered.</param>
        /// <param name="points">[Optional] The number of points that make up the circle. 24 by default. The minimum acceptable number of points is 3.</param>
        /// <param name="thickness">[Optional] The thickness of the circle outline. 1.0f by default.</param>
        /// <param name="colour">[Optional] The desired rendering colour.</param>
        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 origin, float radius, int points = 16, float thickness = 1.0f, Color? colour = null)
        {
            points = Math.Max(points, 3); // Ensure there are always at least 3 points
            Core.GetShapeBatcher().DrawCircle(origin.X, origin.Y, radius, points, thickness, colour);
        }

        /// <summary>
        /// Converts a given string value into a form where the first character is uppercase, and the rest are lowercase.
        /// </summary>
        /// <param name="before">The string value before the capitalisation process begins.</param>
        /// <returns>Returns a string value where the first char has been capitalised and the rest converted to lowercase.</returns>
        public static string Capitalise(this string before)
        {
            if (before != null && before.Length > 0) // Only perform the conversion if the string supplied isn't empty
                return char.ToUpper(before.First()) + before[1..].ToLower();
            else
                return before; // return the value unaltered
        }

        /// <summary>
        /// Converts the given value into a new range from an old one, maintaining the previous ratio.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="oldMin">The old range's minimum value.</param>
        /// <param name="oldMax">The old range's maximum value.</param>
        /// <param name="newMin">The new range's minimum value.</param>
        /// <param name="newMax">The new range's maximum value.</param>
        /// <returns>Returns a whole number in the new range, based on the ratio of the old range and the provided value.</returns>
        public static int LinearConversion(this int value, int oldMin, int oldMax, int newMin, int newMax)
		{
            if(oldMax == oldMin)
			{
                throw new DivideByZeroException("oldMax and oldMin cannot be the same value.");
			}
            int oldRange = oldMax - oldMin;
            int newRange = newMax - newMin;
            return ((value - oldMin) * newRange / oldRange) + newMin;
        }

        /// <summary>
        /// [Overload] Converts the given value into a new range from an old one, maintaining the previous ratio.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="oldMin">The old range's minimum value.</param>
        /// <param name="oldMax">The old range's maximum value.</param>
        /// <param name="newMin">The new range's minimum value.</param>
        /// <param name="newMax">The new range's maximum value.</param>
        /// <returns>Returns a decimal number in the new range, based on the ratio of the old range and the provided value.</returns>
        public static float LinearConversion(this float value, float oldMin, float oldMax, float newMin, float newMax)
		{
            if (oldMax == oldMin)
            {
                throw new DivideByZeroException("oldMax and oldMin cannot be the same value.");
            }
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            return ((value - oldMin) * newRange / oldRange) + newMin;
        }

        /// <summary>
        /// [Overload] Converts the given value into a new range from an old one, maintaining the previous ratio.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="oldMin">The old range's minimum value.</param>
        /// <param name="oldMax">The old range's maximum value.</param>
        /// <param name="newMin">The new range's minimum value.</param>
        /// <param name="newMax">The new range's maximum value.</param>
        /// <returns>Returns a decimal number in the new range, based on the ratio of the old range and the provided value.</returns>
        public static double LinearConversion(this double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            if (oldMax == oldMin)
            {
                throw new DivideByZeroException("oldMax and oldMin cannot be the same value.");
            }
            double oldRange = oldMax - oldMin;
            double newRange = newMax - newMin;
            return ((value - oldMin) * newRange / oldRange) + newMin;
        }
    }
}
