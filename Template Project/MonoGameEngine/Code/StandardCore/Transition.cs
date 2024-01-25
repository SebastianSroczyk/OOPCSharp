using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Maths;
using System;

namespace MonoGameEngine
{
    /// <summary>Enum used to represent the animation style desired for a transition.
    /// <br/>- <strong>Fade</strong> will perform a fullscreen fade in to/out from a solid colour by adjusting the transparency level.
    /// <br/>- <strong>Fill</strong> will perform a scaling box fill in/out using a solid colour.
    /// <br/>- <strong>SwipeLeft</strong> will perform a swipe transition in a leftward motion, using a single colour panel.
    /// <br/>- <strong>SwipeRight</strong> will perform a swipe transition in a rightward motion, using a single colour panel.
    /// <br/>- <strong>SwipeUp</strong> will perform a swipe transition in a upward motion, using a single colour panel.
    /// <br/>- <strong>SwipeDown</strong> will perform a swipe transition in a downward motion, using a single colour panel.
    /// </summary>
    public enum TransitionType { Fade, Fill, SwipeLeft, SwipeRight, SwipeUp, SwipeDown}

    /// <summary>A class which represents a transition special effect.</summary>
    public sealed class Transition
    {
        private Core _core;
        private Texture2D _panel;
        private Color _panelColour;
        private Color _colourDestination;
        private Color _colourSource;

        private TransitionType _type = TransitionType.Fade;
        private InterpolationData _movementData;
        private Vector2 _position;
        private Vector2 _origin;
        private InterpolationData _scale;

        private Schedulable _screenSwapSchedulable;

        private float _transitionTime;
        private float _durationOfTransition = 1.0f;

        private bool _active = false;

        /// <summary>
        /// Private constructor necessary to instantiate the Transition singleton.
        /// </summary>
        static Transition()
        {
            
        }

        /// <summary>
        /// Provides access to this Transition object. The main way to use the built-in screen transition functionality.
        /// </summary>
        public static Transition Instance { get; } = new Transition();

        /// <summary>
        /// Provides essential setup for the Transition object. <b>Automatically called by the game's Core</b>.
        /// </summary>
        /// <param name="core">The Core of the game.</param>
        internal void Setup(Core core) 
        {
            _core = core;
            _position = Vector2.Zero;
            _origin = new Vector2(0.5f, 0.5f);
            _scale = new InterpolationData(new float[] { 1.0f }, new float[]{ 1.0f });
            _panel = Core.GetResource<Texture2D>("misc/Pixel");
            _colourSource = Color.Black;
            _colourDestination = Color.Black;
            _panelColour.A = 0;
        }

        /// <summary>
        /// Ensures the Transition object is kept up-to-date. <b>Automatically called by the game's Core</b>.
        /// </summary>
        /// <param name="deltaTime">Time since the last frame.</param>
        internal void Update(float deltaTime)
        {
            if (_active)
            {
                if (_movementData != null)
                {
                    _movementData.UpdateValues(deltaTime);
                    _position = new Vector2(_movementData.CurrentValues[0], _movementData.CurrentValues[1]);
                    if (_movementData.LerpTime >= 1.0f)
                        _movementData = null;
                }

                _transitionTime += deltaTime / _durationOfTransition;
                if (_type == TransitionType.Fade)
                    _panelColour = Color.Lerp(_colourSource, _colourDestination, _transitionTime);
                else if (_type == TransitionType.Fill)
                    _scale.UpdateValues(deltaTime);

                if(_transitionTime >= 1.0f)
                {
                    _active = false;
                    _transitionTime = 0;

                    if(_panelColour.A == 255 && _scale.CurrentValues[0] == 1)
                    {
                        _core.SwapScreen();
                    }
                }
            }
        }

        private void InitialiseTransitionStart(TransitionType type)
        {
            _type = type;
            switch(_type)
            {
                case TransitionType.Fade:
                    _colourSource = Color.Transparent;
                    break;
                case TransitionType.SwipeUp:
                    _position = new Vector2(0, Settings.ScreenDimensions.Y);
                    _movementData = new InterpolationData(_position, Vector2.Zero, _durationOfTransition);
                    break;
                case TransitionType.SwipeDown:
                    _position = new Vector2(0, -Settings.ScreenDimensions.Y);
                    _movementData = new InterpolationData(_position, Vector2.Zero, _durationOfTransition);
                    break;
                case TransitionType.SwipeLeft:
                    _position = new Vector2(-Settings.ScreenDimensions.X, 0);
                    _movementData = new InterpolationData(_position, Vector2.Zero, _durationOfTransition);
                    break;
                case TransitionType.SwipeRight:
                    _position = new Vector2(Settings.ScreenDimensions.X, 0);
                    _movementData = new InterpolationData(_position, Vector2.Zero, _durationOfTransition);
                    break;
                case TransitionType.Fill:
                    _scale = new InterpolationData(new float[] { 0.0f }, new float[] { 1.0f }, _durationOfTransition);
                    break;
            }
        }

        private void InitialiseTransitionEnd(TransitionType type)
        {
            _type = type;
            switch (_type)
            {
                case TransitionType.Fade:
                    _colourDestination = Color.Transparent;
                    break;
                case TransitionType.SwipeUp:
                    _position = Vector2.Zero;
                    _movementData = new InterpolationData(_position, new Vector2(0, -Settings.ScreenDimensions.Y), _durationOfTransition);
                    break;
                case TransitionType.SwipeDown:
                    _position = Vector2.Zero;
                    _movementData = new InterpolationData(_position, new Vector2(0, Settings.ScreenDimensions.Y), _durationOfTransition);
                    break;
                case TransitionType.SwipeLeft:
                    _position = Vector2.Zero;
                    _movementData = new InterpolationData(_position, new Vector2(-Settings.ScreenDimensions.X, 0), _durationOfTransition);
                    break;
                case TransitionType.SwipeRight:
                    _position = Vector2.Zero;
                    _movementData = new InterpolationData(_position, new Vector2(Settings.ScreenDimensions.X, 0), _durationOfTransition);
                    break;
                case TransitionType.Fill:
                    _scale = new InterpolationData(new float[] { 1.0f }, new float[] { 0.0f }, _durationOfTransition);
                    break;
            }
        }

        /// <summary>
        /// Draws the Transition panel texture above the rest of the game. <b>Automatically called by the game's Core</b>.
        /// </summary>
        /// <param name="spriteBatch">The common SpriteBatch used by the game.</param>
        internal void Render(SpriteBatch spriteBatch)
        {
            Rectangle panelRect = Camera.Instance.GetRenderRectangle();
            if (_panel != null)
                spriteBatch.Draw(_panel, new Rectangle((int)(_position.X + panelRect.X + (_origin.X * panelRect.Width)), (int)(_position.Y + panelRect.Y + (_origin.Y * panelRect.Height)), (int)(panelRect.Width * _scale.CurrentValues[0]), (int)(panelRect.Height * _scale.CurrentValues[0])), null, _panelColour, 0, _origin, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Start a transition, then swap to the desired Screen once complete.
        /// </summary>
        /// <typeparam name="TScreen">The Screen that should start next.</typeparam>
        /// <param name="type">[Optional] The type of transition that is desired. 'Fade' by default.</param>
        /// <param name="fadeColour">[Optional] The colour that the transition should use.</param>
        /// <param name="transitionDuration">[Optional] The time, in seconds, that the transition fadeout should take. 0.25 seconds by default.</param>
        public void ToScreen<TScreen>(TransitionType type = TransitionType.Fade, Color? fadeColour = null, float transitionDuration = 0.25f) where TScreen : IScreen, new()
        {
            if(!_active && _core.GetNextScreen() == null)
            {
                StartTransition(type, fadeColour, transitionDuration);
                _core.StartScreen<TScreen>();
                //Core.Schedule(_core, "SwapScreen", null, transitionDuration);
                _active = true;
            }
            else
			{
                if(_core.GetNextScreen() != null)
                    throw new TransitionOverloadException("Transition to screen (" + typeof(TScreen) + ") already in progress.");
			}
        }

        /// <summary>
        /// Start a transition, without swapping to another Screen.
        /// </summary>
        /// <param name="type">[Optional] The type of transition that is desired. 'Fade' by default.</param>
        /// <param name="fadeColour">[Optional] The colour that the transition should use.</param>
        /// <param name="transitionDuration">[Optional] The time, in seconds, that the transition fadeout should take. 0.25 seconds by default.</param>
        public void StartTransition(TransitionType type = TransitionType.Fade, Color? fadeColour = null, float transitionDuration = 0.25f)
        {
            if (!_active)
            {
                if (type != TransitionType.Fade)
                    _panelColour = fadeColour == null ? Color.Black : (Color)fadeColour;
                else
                    _colourDestination = fadeColour == null ? Color.Black : (Color)fadeColour;

                _durationOfTransition = transitionDuration;

                InitialiseTransitionStart(type);
                _active = true;
            }
        }

        /// <summary>
        /// Begins the 'end' animation of the chosen Transition. Should be called in the Start() method of any Screen that you have transitioned to.
        /// </summary>
        /// <param name="type">[Optional] The type of transition that is desired. 'Fade' by default.</param>
        /// <param name="transitionDuration">[Optional] The time, in seconds, that the transition fadein should take. 0.25 seconds by default.</param>
        public void EndTransition(TransitionType? type = null, float? transitionDuration = null)
        {
            if(!_active)
            {
                // Ensures a transition ending effect doesn't start transparent
                _colourSource = _colourDestination != Color.Transparent ? _colourDestination : Color.Black;

                // Assigns a duration of at least 0 seconds after a null check
                _durationOfTransition = Math.Max(0f, transitionDuration ?? (_durationOfTransition == 0 ? 0.25f : _durationOfTransition));

                InitialiseTransitionEnd(type == null ? _type : (TransitionType)type);
                _active = true;
            }
        }

        /// <summary>
        /// A getter method to check if a Transition effect is currently running.
        /// </summary>
        /// <returns>Returns 'true' if an effect is running. Otherwise, returns 'false'.</returns>
        public bool IsActive() { return _active; }
    }
}
