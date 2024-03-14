using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace MonoGameEngine.StandardCore
{
    /// <summary>A class which can handle a static sprite image.</summary>
    public class Sprite : DrawableElement
    {
        protected Texture2D _texture;
        protected readonly string _filename;

        protected Color _tint;
        protected Color _fillColour;
        protected float _fillEffectStrength = 0f;

        protected float _layerDepth = 0.5f;
        protected float _rotation = 0;

        protected bool _inWorldSpace = true;

        protected SpriteEffects _spriteEffects = SpriteEffects.None;

        protected Vector2 _scale;
        protected Vector2 _origin;
        protected Vector2 _position;

        /// <summary>
        /// The constructor for this class.
        /// </summary>
        /// <param name="texture">The image that should be used by this Sprite object.</param>
        /// <param name="tint">[Optional] The tint with which this Sprite's image should be rendered. White by default.</param>
        /// <param name="layerDepth">[Optional] The depth at which this Sprite should be rendered. '5' by default.</param>
        internal Sprite(Texture2D texture, Color? tint = null, int layerDepth = 5)
        {
            _texture = texture;
            _filename = texture.Name;
            _scale = Vector2.One;
            _origin = Vector2.Zero;
            SetLayerDepth(layerDepth);

            if (tint.HasValue)
                _tint = (Color)tint;
            else
                _tint = Color.White;

            _fillColour = Color.Transparent;
        }

        /// <summary>
        /// A getter method which returns the Rectangle representing the bounding box of this Sprite.
        /// </summary>
        /// <returns>Returns a Rectangle object which represents the edges of this Sprite's image.</returns>
        public virtual Rectangle GetBounds()
        {
            return _texture.Bounds;
        }

        /// <summary>
        /// A getter method which returns the width (in pixels) of this Sprite's image.
        /// </summary>
        /// <returns>Returns an integer value representing this Sprite's width.</returns>
        public virtual int GetWidth()
        {
            return _texture.Width;
        }

        /// <summary>
        /// A getter method which returns the height (in pixels) of this Sprite's image.
        /// </summary>
        /// <returns>Returns an integer value representing this Sprite's height.</returns>
        public virtual int GetHeight()
        {
            return _texture.Height;
        }

        /// <summary>
        /// A getter method which returns the position of this Sprite's origin.
        /// </summary>
        /// <returns>A Vector2 object containing the coordinates of this Sprite's center point.</returns>
        public Vector2 GetOrigin()
        {
            return _origin;
        }

        /// <summary>
        /// A getter method which returns the current rendering tint used by this Sprite.
        /// </summary>
        /// <returns>A Color object containing the RGBA values used when tinting this Sprite's image.</returns>
        public Color GetTint()
        {
            return _tint;
        }

        /// <summary>
        /// A getter method which returns the current rendering depth of this Sprite. The lower the number, the earlier this Sprite's image will be drawn in the render order.
        /// </summary>
        /// <returns>An integer value representing the layer depth of this Sprite.</returns>
        public int GetLayerDepth()
        {
            return (int)(_layerDepth * 10);
        }

        /// <summary>
        /// A getter method which returns the current rotation (in degrees) that this Sprite's image will be drawn at.
        /// </summary>
        /// <returns>A floating-point value representing the rotation of this Sprite's image.</returns>
        public float GetRotation()
        {
            return _rotation;
        }

        /// <summary>
        /// A getter method which returns the colour used to fill all the pixels of this Sprite, ignoring alpha (transparency).
        /// </summary>
        /// <returns>A Color object containing the RGBA values used when filling this Sprite's image.</returns>
        public Color GetFillColour()
        {
            return _fillColour;
        }

        /// <summary>
        /// A getter method which returns the strength of the 'fill' pixel effect's blending. 
        /// <br/>1.0f will completely fill the Sprite with the fill colour, and 0.0f will ignore the fill colour.
        /// </summary>
        /// <returns>A floating point number which represents the amount of blending the fill colour has with the Sprite's original colours.</returns>
        public float GetFillEffectStrength()
		{
            return _fillEffectStrength;
		}

        /// <summary>
        /// A setter method for setting the rotation (in degrees) of this Sprite.
        /// </summary>
        /// <param name="rotation">The rotation (in degrees) to render this Sprite at.</param>
        public void SetRotation(float rotation)
        {
            _rotation = rotation;
        }

        /// <summary>
        /// A setter method for changing the render layer depth of this Sprite. '5' by default.
        /// </summary>
        /// <param name="layerDepth">The layer depth to render this sprite at. The higher the number, the closer to the background the sprite will be rendered.</param>
        public void SetLayerDepth(int layerDepth)
        {
            //Ensure the parameter is between 0 and 10, then divide it by 10 to convert the value to the scale of 0.0f -> 1.0f.
            _layerDepth = Math.Clamp(layerDepth, 0, 10) * 0.1f; 
            
        }

        /// <summary>
        /// A setter method which sets the fractional scale for this Sprite, per axis. Scale values are (1,1) by default.
        /// </summary>
        /// <param name="scaleX">Decimal value to scale the x-axis by.</param>
        /// <param name="scaleY">Decimal value to scale the y-axis by.</param>
        public void SetScale(float scaleX, float scaleY)
        {
            _scale = new Vector2(scaleX, scaleY);
        }

        /// <summary>
        /// A getter method which gets the current fractional scale for this Sprite.
        /// </summary>
        /// <returns>A Vector2 object containing the floating point fractional value used for scaling this Sprite.</returns>
        public Vector2 GetScale()
        {
            return _scale;
        }

        /// <summary>
        /// A setter method which sets the origin point for this Sprite. Values should be between 0.0f and 1.0f. Origin point is (0.0f, 0.0f) by default.
        /// </summary>
        /// <param name="originX">Fraction of the width of the sprite. Value should be between 0.0f and 1.0f.</param>
        /// <param name="originY">Fraction of the height of the sprite. Value should be between 0.0f and 1.0f.</param>
        public void SetOrigin(float originX, float originY)
        {
            originX = Math.Clamp(originX, 0.0f, 1.0f);
            originY = Math.Clamp(originY, 0.0f, 1.0f);

            _origin = new Vector2(GetWidth() * originX, GetHeight() * originY);
        }

        /// <summary>
        /// A setter method which sets the render tint for this Sprite. Tint is White by default.
        /// </summary>
        /// <param name="tint">The Colour with which to tint the rendered Sprite image.</param>
        public void SetTint(Color tint)
        {
            _tint = tint;
        }

        /// <summary>
        /// [Override] A setter method which sets the render tint for this Sprite using separate colour values. Tint is 255, 255, 255 by default.
        /// </summary>
        /// <param name="r">An integer value representing the red colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="b">An integer value representing the blue colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="g">An integer value representing the green colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="a">[Optional] An integer value representing the alpha (transparency) channel for the new Color. Value is clamped between 0 and 255. Set to 255 (fully opaque) by default.</param>
        public void SetTint(int r, int g, int b, int a = 255)
        {
            // Clamp the values to ensure they remain within acceptable bounds.
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            a = Math.Clamp(a, 0, 255);

            SetTint(new Color(r, g, b, a));
        }

        /// <summary>
        /// A method which sets the fill colour for this Sprite, ignoring base transparency. Fill is 0, 0, 0, 0 by default.
        /// </summary>
        /// <param name="fillColour">The Colour with which to fill the rendered Sprite image.</param>
        /// <param name="effectStrength">[Optional] The amount (0.0f - 1.0f) that the fill colour should blend with the original Sprite's colours. Values are clamped automatically.</param>
        public void FillWithColour(Color fillColour, float effectStrength = 1.0f)
		{
            _fillColour = fillColour;
            _fillEffectStrength = Math.Clamp(effectStrength, 0.0f, 1.0f);
		}

        /// <summary>
        /// [Override] A method which sets the fill colour for this Sprite, ignoring base transparency. Fill is 0, 0, 0, 0 by default.
        /// </summary>
        /// <param name="r">An integer value representing the red colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="b">An integer value representing the blue colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="g">An integer value representing the green colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="a">[Optional] An integer value representing the alpha (transparency) channel for the new Color. Value is clamped between 0 and 255. Set to 255 (fully opaque) by default.</param>
        /// <param name="effectStrength">[Optional] The amount (0.0f - 1.0f) that the fill colour should blend with the original Sprite's colours. Values are clamped automatically.</param>
        public void FillWithColour(int r, int g, int b, int a = 255, float effectStrength = 1.0f)
        {
            // Clamp the values to ensure they remain within acceptable bounds.
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            a = Math.Clamp(a, 0, 255);

            FillWithColour(new Color(r, g, b, a), effectStrength);
        }

        /// <summary>
        /// A getter method which returns the filename of this Sprite's current image.
        /// </summary>
        /// <returns>A string value with the name of the current image of this Sprite.</returns>
        public string GetName()
        {
            return _filename;
        }

        /// <summary>
        /// A setter method which sets whether or not this Sprite should be drawn in world space or screen space.
        /// </summary>
        /// <param name="inWorldSpace">A boolean which represents if this Sprite is in world space or not.</param>
        public void SetInWorldSpace(bool inWorldSpace)
        {
            _inWorldSpace = inWorldSpace;
        }

        /// <summary>
        /// A setter method which sets the image texture for this Sprite to use.
        /// </summary>
        /// <param name="texture">The texture which should be used as an image for this Sprite.</param>
        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
        }

        /// <summary>
        /// A getter method that returns the image texture being used by this Sprite.
        /// </summary>
        /// <returns>The Texture2D which represents the image used by this Sprite.</returns>
        public Texture2D GetTexture()
        {
            return _texture;
        }

        /// <summary>
        /// A getter method which returns the position on-screen where this Sprite will be rendered. 
        /// </summary>
        /// <returns>A Vector2 object representing the on-screen pixel location of this Sprite's origin.</returns>
        public Vector2 GetPosition()
        {
            return _position;
        }

        /// <summary>
        /// A setter method which allocates a new on-screen position for this Sprite to render at.
        /// </summary>
        /// <param name="position">The new on-screen position for this Sprite, in pixel co-ordinates.</param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// The rendering function of this Sprite object. 
        /// </summary>
        /// <param name="spriteBatch">The current batch of sprites to be rendered this frame of the game.</param>
        /// <param name="position">The position onscreen to draw this Sprite at.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            _position = position;
            Draw(spriteBatch);
            
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if(IsVisible)
                spriteBatch.Draw(_texture, _position, null, _tint, (_rotation * (float)Math.PI) / 180.0f, _origin, _scale, _spriteEffects, _layerDepth);
        }


        internal bool IsInWorldSpace()
        {
            return _inWorldSpace;
        }

        internal void UpdatePosition(Vector2 position)
        {
            _position = position;
        }

        /// <summary>
        /// A method which checks if this Sprite has been set to be rendered with a flipped horizontal axis.
        /// </summary>
        /// <returns>Returns 'true' if this Sprite is being drawn with a flipped horizontal axis. Otherwise, returns 'false'.</returns>
        public bool IsHorizontallyFlipped()
        {
            return _spriteEffects == SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// A method which checks if this Sprite has been set to be rendered with a flipped vertical axis.
        /// </summary>
        /// <returns>Returns 'true' if this Sprite is being drawn with a flipped vertical axis. Otherwise, returns 'false'.</returns>
        public bool IsVerticallyFlipped()
        {
            return _spriteEffects == SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// A method which will allow the Sprite to be rendered with a flipped horizontal axis. Useful for rendering sprites facing left or right.
        /// </summary>
        /// <param name="flip">Represents whether or not the Sprite should be flipped horizontally or not.</param>
        public void FlipHorizontally(bool flip)
        {
            if (flip)
            {
                if (_spriteEffects.HasFlag(SpriteEffects.FlipVertically))
                    _spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                else
                    _spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                if (_spriteEffects.HasFlag(SpriteEffects.FlipVertically))
                    _spriteEffects = SpriteEffects.FlipVertically;
                else
                    _spriteEffects = SpriteEffects.None;
            }
        }

        /// <summary>
        /// A method which will allow the Sprite to be rendered with a flipped vertical axis. Useful for rendering sprites facing up or down.
        /// </summary>
        /// <param name="flip">Represents whether or not the Sprite should be flipped vertically or not.</param>
        public void FlipVertically(bool flip)
        {
            if(flip)
            {
                if (_spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
                    _spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                else
                    _spriteEffects = SpriteEffects.FlipVertically;
            }
            else
            {
                if (_spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
                    _spriteEffects = SpriteEffects.FlipHorizontally;
                else
                    _spriteEffects = SpriteEffects.None;
            }
        }

        /// <summary>
        /// Rotates the Sprite around its origin by a given amount, in degrees.
        /// </summary>
        /// <param name="rotateAmount">The amount, in degrees, to rotate the sprite by.</param>
        public void Rotate(float rotateAmount)
        {
            _rotation += rotateAmount;
            if (_rotation >= 360.0f || _rotation <= -360.0f) // cap the value
                _rotation = 0;
        }
    }
}
