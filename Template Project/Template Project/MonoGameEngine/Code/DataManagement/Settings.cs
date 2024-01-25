using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace MonoGameEngine
{
    /// <summary>A handler class which manages several key settings for a game project.</summary>
    public static class Settings
    {
        private static GraphicsDeviceManager _graphics;
        private static Core _core;

        private static Vector2 _gameResolution = new Vector2(1280, 720);
        private static Vector2 _screenDimensions = new Vector2(1280, 720);

        private static float _gameSpeed = 1.0f;

        /// <summary>Property representing the internal resolution for the game to be rendered at (1280x720px by default). This is not the same as the game's window dimensions.</summary>
        public static Vector2 GameResolution { get { return _gameResolution; } set { _gameResolution = value; } }

        /// <summary>Property representing the game window's current dimensions. 1280x720px by default.</summary>
        public static Vector2 ScreenDimensions { get { return _screenDimensions; } set => SetScreenDimensions(value); }

        /// <summary>Property representing the game's fullscreen state. Set to 'false' by default.<br/><b>Note: Proper fullscreen rendering is only supported in Release builds.</b></summary>
        public static bool IsFullscreen { get => _graphics.IsFullScreen; set => EnableFullscreen(value); }

        /// <summary>Property representing whether or not the mouse will appear above the game window.</summary>
        public static bool IsMouseVisible { get => _core.IsMouseVisible; set => _core.IsMouseVisible = value; }

        /// <summary>Property representing the game's vsync flag. True by default.</summary>
        public static bool Vsync { get => _graphics.SynchronizeWithVerticalRetrace; set => EnableVsync(value); }

        /// <summary>Property representing the desired framerate for the game. Change this if the game is being run on weaker hardware. 60 by default. Values of below 0 will be clamped.</summary>
        public static int TargetFrameRate { get => _core.GetFrameRate(); set => _core.SetFrameRate(value); }

        /// <summary>Property representing the game's window border flag. False by default.</summary>
        public static bool IsBorderless { get => _core.Window.IsBorderless; set => _core.Window.IsBorderless = value; }

        /// <summary>Property representing the game window's background colour. CornflowerBlue by default.</summary>
        public static Color BackgroundFill { get; set; }

        /// <summary>Property representing the game window's letterboxing colour. Black by default.</summary>
        public static Color LetterboxFill { get; set; }

        /// <summary>Property representing whether or not anti-aliasing (MSAA) is being applied to the game window.</summary>
        public static bool UseAntiAliasing { get => _graphics.PreferMultiSampling; set => EnableMSAA(value); }

        /// <summary>Property representing the current BGM' volume, which is clamped between 0.0f and 1.0f. '1.0f' by default.</summary>
        public static float BGMVolume { get => AudioManager.Instance.GetCurrentBGMVolume(); set => AudioManager.Instance.SetBGMVolume(value); }

        /// <summary>Property representing a global multiplier applied to deltaTime. 1.0f by default. Property is automatically clamped between 0.25f and 4.0f.<br/><u>Does not affect framerate/FPS. Only natively affects values relating to deltaTime.</u></summary>
        public static float GameSpeed { get => _gameSpeed; set => _gameSpeed = Math.Clamp(value, 0.25f, 4f); }

        /// <summary>Property representing whether or not the game should pan and scale the volume of sound effects in the game based on the position of the camera. False by default.</summary>
        public static bool UsePositionalAudio { get; set; }

        /// <summary>Function for initial setup of the game's Settings. <b>Can only be called once, and this is done automatically by the game's Core</b>.</summary>
        /// <param name="graphics">The graphics manager for the game. Instance should be created by Core.</param>
        /// <param name="core">The Core instance at the center of this game project.</param>
        internal static void Initialise(Core core, GraphicsDeviceManager graphics)
        {
            if(_core == null) // Ensures initialisation is only performed once.
            {
                _core = core;
                _graphics = graphics;

                GameResolution = new Vector2(1920, 1080);
                _screenDimensions = new Vector2(1920, 1080);
                IsFullscreen = false;
                IsBorderless = false;
                BackgroundFill = Color.CornflowerBlue;
                LetterboxFill = Color.Black;
            }
        }

        private static void EnableFullscreen(bool enabled)
        {
#if !DEBUG
            _graphics.IsFullScreen = enabled;

            if (_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                _graphics.PreferredBackBufferWidth = (int)_screenDimensions.X;
                _graphics.PreferredBackBufferHeight = (int)_screenDimensions.Y;
            }

            _graphics.ApplyChanges();

#else
            if(enabled)
            {
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 4;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 4;
            }
            else
            {
                _graphics.PreferredBackBufferWidth = (int)_screenDimensions.X;
                _graphics.PreferredBackBufferHeight = (int)_screenDimensions.Y;
            }

            _graphics.ApplyChanges();
#endif
        }

        private static void EnableMSAA(bool enabled)
        {
            _graphics.PreferMultiSampling = enabled;
            _graphics.ApplyChanges();
        }

        private static void EnableVsync(bool enabled)
        {
            _graphics.SynchronizeWithVerticalRetrace = enabled;
            _graphics.ApplyChanges();
        }

        private static void SetScreenDimensions(Vector2 dimensions)
        {
            _screenDimensions.X = dimensions.X;
            _screenDimensions.Y = dimensions.Y;

            if (!IsFullscreen)
            {
                _gameResolution.X = dimensions.X;
                _gameResolution.Y = dimensions.Y;
                _graphics.PreferredBackBufferWidth = (int)dimensions.X;
                _graphics.PreferredBackBufferHeight = (int)dimensions.Y;
                _graphics.ApplyChanges();
            }

            _core.RefreshWindow();
        }
    }
}
