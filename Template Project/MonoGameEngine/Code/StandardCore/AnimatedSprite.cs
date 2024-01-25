using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace MonoGameEngine.StandardCore
{
    /// <summary>Represents the style of loop that is desired for an animation.
    /// <br/>- <strong>Standard</strong> loops the animation from the start to the end, before starting from the beginning again.
    /// <br/>- <strong>Bounce</strong> bounces the animation loop forwards and backwards when it runs out of frames in its current direction.
    /// <br/>- <strong>None</strong> will only play the animation once before stopping on the final frame.
    /// </summary>
    public enum LoopType 
    {
        /// <summary>Loops the animation from the start to the end, before starting from the beginning again.</summary>
        Standard,
        /// <summary>Bounces the animation loop forwards and backwards when it runs out of frames in its current direction.</summary>
        Bounce,
        /// <summary>Will only play the animation once before stopping on the final frame.</summary>
        None
    }

    /// <summary>A class that can handle sprite animation using a uniform spritesheet.</summary>
    public sealed class AnimatedSprite : Sprite
    {
        private float _animationSpeed;
        private int _currentAnimation;

        private int[] _framesOfAnimation;

        private int _currentFrame;
        private float _currentFrameTime;

        private float _timePerFrame = 1.0f;

        private int _frameWidth;
        private int _frameHeight;

        private Rectangle _sourceRectangle;

        private LoopType _loopType;
        private bool _animateForwards = true;
        private bool _paused = false;

        /// <summary>
        /// The constructor for this class.
        /// </summary>
        /// <param name="texture">The spritesheet that this AnimatedSprite should use.</param>
        /// <param name="colour">[Optional] The colour that should be used when rendering the frames of this AnimatedSprite. White by default.</param>
        /// <param name="layerDepth">[Optional] The rendering depth of this AnimatedSprite. '5' by default.</param>
        internal AnimatedSprite(Texture2D texture, Color? colour = null, int layerDepth = 5) 
            : base(texture, colour, layerDepth)
        {
        }

        /// <summary>
        /// Function that should be called when setting up this AnimatedSprite's attributes. Must be called before any animation can take place.
        /// </summary>
        /// <param name="frameWidth">The width of each frame of animation in the spritesheet (in pixels).</param>
        /// <param name="frameHeight">The height of each frame of animation in the spritesheet (in pixels).</param>
        /// <param name="animationSpeed">The speed at which the animation should cycle between its frames (in seconds).</param>
        /// <param name="framesPerAnimation">An array of integers, with each integer representing the frame count of the corresponding animation sequence.</param>
        /// <param name="loopType">[Optional] The type of animation looping that should be used. 'LoopType.Standard' by default.</param>
        internal void Initialise(int frameWidth, int frameHeight, float animationSpeed, int[] framesPerAnimation, LoopType loopType = LoopType.Standard)
        {
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _animationSpeed = animationSpeed;
            _loopType = loopType;

            _framesOfAnimation = new int[framesPerAnimation.Length];
            for (int i = 0; i < framesPerAnimation.Length; i++)
                _framesOfAnimation[i] = framesPerAnimation[i];

            _sourceRectangle.Width = _frameWidth;
            _sourceRectangle.Height = _frameHeight;
        }

        /// <summary>
        /// Function used to begin a new animation. 
        /// </summary>
        /// <param name="animation">The number of the animation in the spritesheet. The first row is at index '0'.</param>
        /// <param name="animationSpeed">[Optional] The speed at which the animation should cycle between its frames (in seconds).</param>
        /// <param name="loopType">[Optional] The type of animation looping that should be used. 'LoopType.Standard' by default.</param>
        public void StartAnimation(int animation, float? animationSpeed = null, LoopType loopType = LoopType.Standard)
        {
            _currentAnimation = animation;
            _loopType = loopType;

            if (animationSpeed != null)
                _animationSpeed = (float)animationSpeed;

            _currentFrame = 0;
            _currentFrameTime = 0;
        }

        /// <summary>
        /// [Overload] Function used to begin a new animation. 
        /// </summary>
        /// <param name="animationEnum">The user-defined Enum which represents the desired animation row in the spritesheet. <br/>Enums should be written starting from '0', as per their default behaviour.</param>
        /// <param name="animationSpeed">[Optional] The speed at which the animation should cycle between its frames (in seconds).</param>
        /// <param name="loopType">[Optional] The type of animation looping that should be used. 'LoopType.Standard' by default.</param>
        public void StartAnimation(Enum animationEnum, float? animationSpeed = null, LoopType loopType = LoopType.Standard)
        {
            StartAnimation(Convert.ToInt32(animationEnum), animationSpeed, loopType);
        }

        /// <summary>
        /// The rendering function of this AnimatedSprite object.
        /// </summary>
        /// <param name="spriteBatch">The common SpriteBatch responsible for handling the sprites within the current game.</param>
        /// <param name="position">The position onscreen to draw this Sprite at.</param>
        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            _position = position;
            Draw(spriteBatch);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if(IsVisible)
                spriteBatch.Draw(_texture, _inWorldSpace == true ? _position : Camera.Instance.WorldToScreen(_position), _sourceRectangle, _tint, (_rotation * (float)Math.PI) / 180.0f, _origin, _scale, _spriteEffects, _layerDepth);
        }

        /// <summary>
        /// The function responsible for updating the current frame of animation for this AnimatedSprite object. <b>Called automatically from the game's Screen</b>.
        /// </summary>
        /// <param name="deltaTime">The elapsed time since the last frame.</param>
        internal void Animate(float deltaTime)
        {
            if(!_paused)
            {
                if (_currentFrame < _framesOfAnimation[_currentAnimation])
                {
                    _currentFrameTime += deltaTime;

                    if (_currentFrameTime >= _timePerFrame * _animationSpeed)
                    {
                        if(_animateForwards)
                            _currentFrame++;
                        else
                        {
                            _currentFrame--;
                            if (_currentFrame == 0)
                                _animateForwards = true;
                        }
                            

                        _currentFrameTime = 0;
                    }

                    if (_currentFrame == _framesOfAnimation[_currentAnimation])
                    {
                        if (_loopType == LoopType.Standard)
                        {
                            _currentFrame = 0;
                        }
                        else if(_loopType == LoopType.Bounce)
                        {
                            _currentFrame--;
                            _animateForwards = false;
                        }
                        else
                        {
                            _currentFrame = _framesOfAnimation[_currentAnimation] - 1;
                            _currentFrameTime = _timePerFrame;
                        }
                    }
                }

                UpdateSourceRectangle();
            }
        }

        private void UpdateSourceRectangle()
        {
            _sourceRectangle.X = _currentFrame * _frameWidth;
            _sourceRectangle.Y = _currentAnimation * _frameHeight;
        }

        /// <summary>
        /// [Override] A getter function which returns the width of this AnimatedSprite's image.
        /// </summary>
        /// <returns>Returns an integer value representing the width of the current frame of animation.</returns>
        public override int GetWidth()
        {
            return _frameWidth;
        }

        /// <summary>
        /// [Override] A getter function which returns the height of this AnimatedSprite's image.
        /// </summary>
        /// <returns>Returns an integer value representing the height of the current frame of animation.</returns>
        public override int GetHeight()
        {
            return _frameHeight;
        }

        /// <summary>
        /// A getter function which returns the current 'paused' state of this AnimatedSprite's animation.
        /// </summary>
        /// <returns>Returns a boolean value representing whether or not this AnimatedSprite is currently animating or not.</returns>
        public bool IsPaused()
        {
            return _paused;
        }

        /// <summary>
        /// A setter function which can either pause or resume this AnimatedSprite's animation updates.
        /// </summary>
        /// <param name="paused">A boolean value representing whether or not this AnimatedSprite should be paused.</param>
        public void SetPaused(bool paused)
        {
            _paused = paused;
        }

        /// <summary>
        /// A setter function which can manually assign the current frame of animation in the current sequence.
        /// </summary>
        /// <param name="newFrame">An integer value representing the frame of animation to switch to.</param>
        public void SetFrameNumber(int newFrame)
        {
            _currentFrame = newFrame;
            UpdateSourceRectangle();
        }

        /// <summary>
        /// A setter function which can assign a new animation speed multiplier to this AnimatedSprite's current animation.
        /// </summary>
        /// <param name="animationSpeed">A floating-point value representing the new animation speed multiplier to be used.</param>
        public void SetAnimationSpeed(float animationSpeed)
        {
            _animationSpeed = animationSpeed;
        }

        /// <summary>
        /// A getter function which returns the rectangle used to draw from the currently loaded spritesheet.
        /// </summary>
        /// <returns>A Rectangle object representing the spritesheet cell of the current frame of animation.</returns>
        public Rectangle GetAnimationFrame()
        {
            return _sourceRectangle;
        }

        /// <summary>
        /// A getter function which returns a new Rectangle representing a specific frame within the AnimatedSprite's spritesheet.
        /// </summary>
        /// <param name="frameIndex">An integer value representing the index within in the animation frames (the column of the spritesheet).</param>
        /// <param name="animationIndex">An integer value representing the index within in the available animations (the row of the spritesheet).</param>
        /// <returns>Returns a Rectangle object representing the requested spritesheet cell using the specified row and column.</returns>
        internal Rectangle GetAnimationFrameByIndex(int frameIndex, int animationIndex)
        {
            return new Rectangle(frameIndex * _frameWidth, animationIndex * _frameHeight, _frameWidth, _frameHeight);
        }

        /// <summary>
        /// A getter function which returns the frame index of the current frame of animation within the sequence.
        /// </summary>
        /// <returns>An integer value representing the index of the animation frame within the current sequence.</returns>
        public int GetFrameNumber()
        {
            return _currentFrame;
        }

        /// <summary>
        /// A getter function which returns the index of the current animation within the established array of animations.
        /// </summary>
        /// <returns>An integer value representing the row of current animation frames in this AnimatedSprite's spritesheet.</returns>
        public int GetAnimationIndex()
        {
            return _currentAnimation;
        }

        /// <summary>
        /// A getter function which returns the state of this AnimatedSprite. Returns 'true' if the animation is not set to loop AND the last frame of animation is being displayed.
        /// </summary>
        /// <returns>A boolean value representing whether or not the current animation has finished.</returns>
        public bool IsFinished()
        {
            return _loopType == LoopType.None && _currentFrame == _framesOfAnimation[_currentAnimation] - 1;
        }
    }
}
