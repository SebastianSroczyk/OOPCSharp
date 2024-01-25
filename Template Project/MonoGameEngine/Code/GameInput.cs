using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

namespace MonoGameEngine
{
    /// <summary>Enum representing the three main mouse buttons. Used when handling mouse click inputs.
    /// <br/>- <strong>Left</strong> represents a left mouse button click.
    /// <br/>- <strong>Right</strong> represents a right mouse button click.
    /// <br/>- <strong>Middle</strong> represents a middle mouse button click.
    /// </summary>
    public enum MouseButton { Left, Right, Middle };

    /// <summary>A class which gives access to low-level input capturing for keyboard and mouse.</summary>
    public static class GameInput
    {
        private static KeyboardState _lastState;
        private static KeyboardState _currentState;

        private static MouseState _lastMouseState;
        private static MouseState _currentMouseState;

        private static GamePadState[] _lastGamePadStates;
        private static GamePadState[] _currentGamePadStates;

        private const int MAX_GAMEPADS = 4;


        /// <summary>
        /// Check to see if key has been held for more than one frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns 'true' if the chosen key has been pressed for at least this frame and the last. Otherwise, returns 'false'.</returns>
        public static bool IsKeyHeld(Keys key)
        {
            return _currentState.IsKeyDown(key) && _lastState.IsKeyDown(key);
        }

        /// <summary>
        /// [Overload] Check to see if key has been held for more than one frame.
        /// </summary>
        /// <param name="key">A string representing the name of the key to check.</param>
        /// <returns>Returns 'true' if the chosen key has been pressed for at least this frame and the last. Otherwise, returns 'false'.</returns>
        public static bool IsKeyHeld(string key)
        {
            key = key.Capitalise();
            if (Enum.TryParse(typeof(Keys), key, out object keyEnum))
                return IsKeyHeld((Keys)keyEnum);
            else
                Console.Error.WriteLine("Invalid key name [" + key + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Check to see if this is the first frame the key has been pressed down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns 'true' if the chosen key was first pressed this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsKeyPressed(Keys key)
        {
            return _currentState.IsKeyDown(key) && !_lastState.IsKeyDown(key);
        }

        /// <summary>
        /// [Overload] Check to see if this is the first frame the key has been pressed down.
        /// </summary>
        /// <param name="key">A string representing the name of the key to check.</param>
        /// <returns>Returns 'true' if the chosen key was first pressed this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsKeyPressed(string key)
        {
            key = key.Capitalise();
            if (Enum.TryParse(typeof(Keys), key, out object keyEnum))
                return IsKeyPressed((Keys)keyEnum);
            else
                Console.Error.WriteLine("Invalid key name [" + key + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Check to see if the key was released this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns 'true' if the chosen key was first released on this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsKeyReleased(Keys key)
        {
            return _currentState.IsKeyUp(key) && _lastState.IsKeyDown(key);
        }

        /// <summary>
        /// [Overload] Check to see if the key was released this frame.
        /// </summary>
        /// <param name="key">A string representing the name of the key to check.</param>
        /// <returns>Returns 'true' if the chosen key was first released on this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsKeyReleased(string key)
        {
            key = key.Capitalise();
            if (Enum.TryParse(typeof(Keys), key, out object keyEnum))
                return IsKeyReleased((Keys)keyEnum);
            else
                Console.Error.WriteLine("Invalid key name [" + key + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Set the vibration strength of the chosen player's gamepad. 
        /// </summary>
        /// <param name="vibrationStrength">The strength with which to vibrate the gamepad. Should be between 0.0f and 1.0f.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        public static void SetGamePadVibration(float vibrationStrength, int player = 1)
        {
            GamePad.SetVibration(player, vibrationStrength, vibrationStrength);
        }

        /// <summary>
        /// [Overload] Set the vibration strength of the chosen player's gamepad. 
        /// </summary>
        /// <param name="leftVibration">The strength with which to vibrate the gamepad's left motor. Should be between 0.0f and 1.0f.</param>
        /// <param name="rightVibration">The strength with which to vibrate the gamepad's right motor. Should be between 0.0f and 1.0f.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        public static void SetGamePadVibration(float leftVibration, float rightVibration, int player = 1)
        {
            GamePad.SetVibration(player, leftVibration, rightVibration);
        }

        /// <summary>
        /// Check to see if this is the first frame the button on the specified gamepad has been pressed down.
        /// </summary>
        /// <param name="button">The specified button to check for.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns 'true' if the chosen button was first pressed this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsGamePadButtonPressed(Buttons button, int player = 1)
        {
            if (player > 0 && player <= MAX_GAMEPADS)
            {
                return _currentGamePadStates[player - 1].IsButtonDown(button) && !_lastGamePadStates[player - 1].IsButtonDown(button);
            }
            else
            {
                Console.Error.WriteLine("Invalid gamepad index [" + player + "] given. Gamepad index should be between 1 and 4.");
                return false;
            }
        }

        /// <summary>
        /// [Overload] Check to see if this is the first frame the button on the specified gamepad has been pressed down.
        /// </summary>
        /// <param name="button">A string representing the name of the button to check for.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns 'true' if the chosen button was first pressed this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsGamePadButtonPressed(string button, int player = 1)
        {
            button = button.Capitalise();
            if (button.Equals("Up") || button.Equals("Down") || button.Equals("Left") || button.Equals("Right"))
                button = "DPad" + button;

            if (Enum.TryParse(typeof(Buttons), button, out object buttonEnum))
                return IsGamePadButtonPressed((Buttons)buttonEnum, player);
            else
                Console.Error.WriteLine("Invalid button name [" + button + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Check to see if button on the specified gamepad has been held for more than one frame.
        /// </summary>
        /// <param name="button">The specified button to check for.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns 'true' if the chosen button has been pressed for at least this frame and the last. Otherwise, returns 'false'.</returns>
        public static bool IsGamePadButtonHeld(Buttons button, int player = 1)
        {
            if (player > 0 && player <= MAX_GAMEPADS)
            {
                return _currentGamePadStates[player - 1].IsButtonDown(button) && _lastGamePadStates[player - 1].IsButtonDown(button);
            }
            else
            {
                Console.Error.WriteLine("Invalid gamepad index [" + player + "] given. Gamepad index should be between 1 and 4.");
                return false;
            }
        }

        /// <summary>
        /// [Overload] Check to see if button on the specified gamepad has been held for more than one frame.
        /// </summary>
        /// <param name="button">A string representing the name of the button to check for.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns 'true' if the chosen button has been pressed for at least this frame and the last. Otherwise, returns 'false'.</returns>
        public static bool IsGamePadButtonHeld(string button, int player = 1)
        {
            button = button.Capitalise();
            if (button.Equals("Up") || button.Equals("Down") || button.Equals("Left") || button.Equals("Right"))
                button = "DPad" + button;

            if (Enum.TryParse(typeof(Buttons), button, out object buttonEnum))
                return IsGamePadButtonHeld((Buttons)buttonEnum, player);
            else
                Console.Error.WriteLine("Invalid button name [" + button + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Check to see if the button on the specified gamepad was released this frame.
        /// </summary>
        /// <param name="button">The specified button to check for.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns 'true' if the chosen button was first released on this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsGamePadButtonReleased(Buttons button, int player = 1)
        {
            if (player > 0 && player <= MAX_GAMEPADS)
            {
                return _currentGamePadStates[player - 1].IsButtonUp(button) && _lastGamePadStates[player - 1].IsButtonDown(button);
            }
            else
            {
                Console.Error.WriteLine("Invalid gamepad index [" + player + "] given. Gamepad index should be between 1 and 4.");
                return false;
            }
        }

        /// <summary>
        /// [Overload] Check to see if the button on the specified gamepad was released this frame.
        /// </summary>
        /// <param name="button">A string representing the name of the button to check for.</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns 'true' if the chosen button was first released on this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsGamePadButtonReleased(string button, int player = 1)
        {
            button = button.Capitalise();
            if (button.Equals("Up") || button.Equals("Down") || button.Equals("Left") || button.Equals("Right"))
                button = "DPad" + button;

            if (Enum.TryParse(typeof(Buttons), button, out object buttonEnum))
                return IsGamePadButtonReleased((Buttons)buttonEnum, player);
            else
                Console.Error.WriteLine("Invalid button name [" + button + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Checks the position of the specified stick on the requested gamepad.
        /// </summary>
        /// <param name="stick">A string representing the name of the stick to check for. Should be either "Left" or "Right".</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns the normalised direction vector of the specified stick. Otherwise, returns (0,0) if stick is untouched or not found.</returns>
        public static Vector2 GetGamePadStickAxis(string stick, int player = 1)
        {
            if (player > 0 && player <= MAX_GAMEPADS)
            {
                stick = stick.Capitalise();

                if (stick.Equals("Left"))
                    return _currentGamePadStates[player - 1].ThumbSticks.Left;
                else if (stick.Equals("Right"))
                    return _currentGamePadStates[player - 1].ThumbSticks.Right;
                else
                    Console.Error.WriteLine("Invalid stick name [" + stick + "] given. Please double-check your spelling.");
            }
            else
            {
                Console.Error.WriteLine("Invalid gamepad index [" + player + "] given. Gamepad index should be between 1 and 4.");
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// Checks how far the specified trigger has been pressed. '1.0f' would be a full press.
        /// </summary>
        /// <param name="trigger">A string representing the name of the trigger to check for. Should be either "Left" or "Right".</param>
        /// <param name="player">[Optional] The index of the desired gamepad. Should be between 1 and 4.</param>
        /// <returns>Returns a floating-point value representing the strength of the press on the requested trigger. '1.0f' would be a full press.</returns>
        public static float GetGamePadTriggerPress(string trigger, int player = 1)
        {
            if(player > 0 && player <= MAX_GAMEPADS)
            {
                trigger = trigger.Capitalise();

                if (trigger.Equals("Left"))
                    return _currentGamePadStates[player - 1].Triggers.Left;
                else if (trigger.Equals("Right"))
                    return _currentGamePadStates[player - 1].Triggers.Right;
                else
                    Console.Error.WriteLine("Invalid stick name [" + trigger + "] given. Please double-check your spelling.");
            }
            else
            {
                Console.Error.WriteLine("Invalid gamepad index [" + player + "] given. Gamepad index should be between 1 and 4.");
            }

            return 0.0f;
        }

        /// <summary>
        /// Check to see if mouse button has been held for more than one frame.
        /// </summary>
        /// <param name="button">An enum representing the mouse button to check.</param>
        /// <returns>Returns 'true' if the chosen mouse button has been pressed for at least this frame and the last. Otherwise, returns 'false'.</returns>
        public static bool IsMouseButtonHeld(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _currentMouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return _currentMouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return _currentMouseState.MiddleButton == ButtonState.Pressed && _lastMouseState.MiddleButton == ButtonState.Pressed;
            }

            return false;
        }

        /// <summary>
        /// Check to see if mouse button has been held for more than one frame.
        /// </summary>
        /// <param name="button">A string representing the name of the button to check for.</param>
        /// <returns>Returns 'true' if the chosen mouse button has been pressed for at least this frame and the last. Otherwise, returns 'false'.</returns>
        public static bool IsMouseButtonHeld(string button)
        {
            if (Enum.TryParse(typeof(MouseButton), button, out object buttonEnum))
                return IsMouseButtonHeld((MouseButton)buttonEnum);
            else
                Console.Error.WriteLine("Invalid button name [" + button + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Check to see if this is the first frame the mouse button has been pressed down.
        /// </summary>
        /// <param name="button">An enum representing the mouse button to check.</param>
        /// <returns>Returns 'true' if the chosen mouse button was first pressed this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _currentMouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return _currentMouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released;
                case MouseButton.Middle:
                    return _currentMouseState.MiddleButton == ButtonState.Pressed && _lastMouseState.MiddleButton == ButtonState.Released;
            }

            return false;
        }

        /// <summary>
        /// [Overload] Check to see if this is the first frame the mouse button has been pressed down.
        /// </summary>
        /// <param name="button">A string representing the name of the button to check for.</param>
        /// <returns>Returns 'true' if the chosen mouse button was first pressed this current frame. Otherwise, returns 'false'.</returns>
        public static bool IsMouseButtonPressed(string button)
        {
            if (Enum.TryParse(typeof(MouseButton), button.Capitalise(), out object buttonEnum))
                return IsMouseButtonPressed((MouseButton)buttonEnum);
            else
                Console.Error.WriteLine("Invalid button name [" + button + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Check to see if the mouse button was released this frame.
        /// </summary>
        /// <param name="button">An enum representing the mouse button to check.</param>
        /// <returns>Returns 'true' if the chosen mouse button was released this frame, otherwise returns 'false'.</returns>
        public static bool IsMouseButtonReleased(MouseButton button)
        {
            switch(button)
            {
                case MouseButton.Left:
                    return _currentMouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return _currentMouseState.RightButton == ButtonState.Released && _lastMouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return _currentMouseState.MiddleButton == ButtonState.Released && _lastMouseState.MiddleButton == ButtonState.Pressed;
            }

            return false;
        }

        /// <summary>
        /// [Overload] Check to see if the mouse button was released this frame.
        /// </summary>
        /// <param name="button">A string representing the name of the button to check for.</param>
        /// <returns>Returns 'true' if the chosen mouse button was released this frame, otherwise returns 'false'.</returns>
        public static bool IsMouseButtonReleased(string button)
        {
            if (Enum.TryParse(typeof(MouseButton), button, out object buttonEnum))
                return IsMouseButtonReleased((MouseButton)buttonEnum);
            else
                Console.Error.WriteLine("Invalid button name [" + button + "] given. Please double-check your spelling.");

            return false;
        }

        /// <summary>
        /// Get the position co-ordinates of the mouse cursor, relative to the primary window.
        /// </summary>
        /// <returns>A Vector2 object containing the co-ordinates of the mouse cursor.</returns>
        public static Vector2 GetMousePosition()
        {
            return Camera.Instance.CalculateMouseScreenPosition(Mouse.GetState().Position.ToVector2());
        }

        /// <summary>
        /// Set the mouse cursor position, relative to the active window. (0,0) is the top-left corner.
        /// </summary>
        /// <param name="mousePosition">The co-ordinates to set the mouse cursor to.</param>
        public static void SetMousePosition(Vector2 mousePosition)
        {
            Mouse.SetPosition((int)mousePosition.X, (int)mousePosition.Y);
        }

        /// <summary>
        /// Get the number of active, connected gamepads.
        /// </summary>
        /// <returns>An integer value representing the number of active gamepads.</returns>
        public static int GetConnectedGamePadCount()
        {
            int connected = 0;

            for(int i = 0; i < _currentGamePadStates.Length; i++)
            {
                if (_currentGamePadStates[i].IsConnected)
                    connected++;
            }

            return connected;
        }

        /// <summary>
        /// Check to see if the mouse cursor is still currently on the displayed Screen.
        /// </summary>
        /// <returns>Returns 'true' if the mouse cursor is within the bounds of the current Screen. Otherwise, returns 'false'.</returns>
        public static bool IsMouseOnScreen()
        {
            Vector2 mousePos = GetMousePosition();
            return ((mousePos.X > 0 && mousePos.X < Settings.ScreenDimensions.X) && (mousePos.Y > 0 && mousePos.Y < Settings.ScreenDimensions.Y));
        }

        /// <summary>
        ///  Method used to refresh the keyboard states used when handling key presses. <b>Automatically handled by the game's Core</b>.
        /// </summary>
        internal static void Update(float deltaTime)
        {
            // Refresh the keyboard states used by the system
            _lastState = _currentState;
            _currentState = Keyboard.GetState();

            // Refresh the mouse states used by the system
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // Refresh the gamepad states used by the system
            UpdateGamePads();

            /* CURRENTLY NOT IN USE
            
            if (_currentState.GetPressedKeys().Length > 0)
                _scrollTime += deltaTime;
            else
                _scrollTime = 0;
            
             */
        }

        /// <summary>
        /// Initialise support for input from GamePads. Called automatically by the game's Core.
        /// </summary>
        internal static void InitialiseGamePads()
        {
            _lastGamePadStates = new GamePadState[MAX_GAMEPADS];
            _currentGamePadStates = new GamePadState[MAX_GAMEPADS];
        }

        /// <summary>
        /// Update each GamePad in turn.
        /// </summary>
        private static void UpdateGamePads()
        {
            for(int i = 0; i <= _currentGamePadStates.Length; i++)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                {
                    _lastGamePadStates[i] = _currentGamePadStates[i];
                    _currentGamePadStates[i] = GamePad.GetState(i);
                }
            }
        }
    }
}
