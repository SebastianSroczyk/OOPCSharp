using Microsoft.Xna.Framework.Input;

namespace MonoGameEngine.ComponentCore.GameObjects.Components
{
    internal abstract class InputComponent : Component
    {
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        /// <summary>
        /// Automatically updates this Component's Keyboard state and tries to handle any relevant inputs. Can be overridden.
        /// </summary>
        /// <param name="deltaTime">The amount of time that has passed since the last frame, in seconds. Taken from MonoGame's GameTime object.</param>
        public override sealed void Update(float deltaTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            HandleInput(deltaTime);
        }

        /// <summary>
        /// Allows the component to recognise key presses and respond as appropriate.
        /// </summary>
        /// /// <param name="deltaTime">The amount of time that has passed since the last frame, in seconds. Taken from MonoGame's GameTime object.</param>
        public abstract void HandleInput(float deltaTime);

        /// <summary>
        /// Determines whether or not this is the first frame that the given key has been pushed.
        /// </summary>
        /// <param name="key">The specific keyboard key of interest.</param>
        /// <returns>Returns 'true' if the given key was not down last frame; otherwise returns 'false'.</returns>
        protected bool IsKeyPressed(Keys key)
        {
            return _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines whether or not the given key is being pushed, and was also pushed last frame.
        /// </summary>
        /// <param name="key">The specific keyboard key of interest.</param>
        /// <returns>Returns 'true' if the given key is currently down, and was also down last frame; otherwise returns 'false'.</returns>
        protected bool IsKeyHeld(Keys key)
        {
            return _currentState.IsKeyDown(key) && _previousState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines whether or not the given key is currently pushed, irrespective of its previous state.
        /// </summary>
        /// <param name="key">The specific keyboard key of interest.</param>
        /// <returns>Returns 'true' if the given key is currently down, regardless of if it was down last frame or not; otherwise returns 'false'.</returns>
        protected bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines whether or not the given key was released within the last frame.
        /// </summary>
        /// <param name="key">The specific keyboard key of interest.</param>
        /// <returns>Returns 'true' if the given key was down last frame, but is currently released; otherwise returns 'false'.</returns>
        protected bool IsKeyReleased(Keys key)
        {
            return !_currentState.IsKeyDown(key) && _previousState.IsKeyDown(key);
        }
    }
}
