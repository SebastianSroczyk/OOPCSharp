using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Extended;
using System;

namespace MonoGameEngine.StandardCore
{
    /// <summary>A class which can represent some text for displaying onscreen.</summary>
    public sealed class Text : DrawableElement
    {
        //TODO: Read up on FontStashSharp (https://github.com/FontStashSharp/FontStashSharp/wiki/Using-FontStashSharp-in-MonoGame-or-FNA)
        private SpriteFont _font;
        private BitmapFont _bmFont;
        private readonly float _standardScale = 0.25f;

        private readonly bool _inScreenSpace = true;

        private string _message;
        private Color _colour;
        private Vector2 _position;
        private float _scale = 0.5f;
        private Vector2 _origin;
        private string _fontName;

        private Vector2 _size;

        /// <summary>
        /// The constructor for this class.
        /// </summary>
        /// <param name="message">The text which will be printed to the screen.</param>
        /// /// <param name="colour">[Optional] The colour applied to the text. Black by default.</param>
        /// /// <param name="fontName">[Optional] The font used for the text rendering. Arial by default.</param>
        /// <param name="inScreenSpace">[Optional] Should the Text be drawn in world space or screen space?</param>W
        public Text(string message, Color? colour = null, string fontName = "misc/Arial", bool inScreenSpace = true)
        {
            IsVisible = true;
            _inScreenSpace = inScreenSpace;
            _message = message;
            _colour = colour == null ? Color.Black : (Color)colour;

            SetScale(1.0f);
            SetFont(fontName);
        }

        /// <summary>
        /// A setter method for changing the scale factor of this rendered text.
        /// </summary>
        /// <param name="scale">The amount of scaling that should be applied to this Text.</param>
        public void SetScale(float scale)
        {
            _scale = _standardScale * scale;
        }

        /// <summary>
        /// A getter method for returning the current scale factor for this Text object.
        /// </summary>
        /// <returns>Returns the current scale factor.</returns>
        public float GetScale()
        {
            return _scale;
        }

        /// <summary>
        /// A setter method which sets the origin point for this Text. Values should be between 0.0f and 1.0f. Origin point is (0.0f, 0.0f) by default.
        /// </summary>
        /// <param name="originX">Fraction of the width of the sprite. Value should be between 0.0f and 1.0f.</param>
        /// <param name="originY">Fraction of the height of the sprite. Value should be between 0.0f and 1.0f.</param>
        public void SetOrigin(float originX, float originY)
        {
            originX = Math.Clamp(originX, 0.0f, 1.0f);
            originY = Math.Clamp(originY, 0.0f, 1.0f);

            _origin = new Vector2(_size.X * originX, _size.Y * originY);
        }

        /// <summary>
        /// A setter method for changing the string being displayed.
        /// </summary>
        /// <param name="newMessage">The string that should be displayed onscreen.</param>
        public void SetMessage(string newMessage)
        {
            _message = newMessage;
            Remeasure();
        }

        /// <summary>
        /// A setter method for changing the rendering colour of this Text. Colour is White by default.
        /// </summary>
        /// <param name="newColour">The colour that should be used for rendering.</param>
        public void SetColour(Color newColour)
        {
            _colour = newColour;
        }

        /// <summary>
        /// [Override] A setter method for changing the rendering colour of this Text using separate colour values. Colour is 255, 255, 255 by default.
        /// </summary>
        /// <param name="r">An integer value representing the red colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="b">An integer value representing the blue colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="g">An integer value representing the green colour channel for the new Color. Value is clamped between 0 and 255.</param>
        /// <param name="a">[Optional] An integer value representing the alpha (transparency) channel for the new Color. Value is clamped between 0 and 255. Set to 255 (fully opaque) by default.</param>
        public void SetColour(int r, int g, int b, int a = 255)
        {
            // Clamp the values to ensure they remain within acceptable bounds.
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            a = Math.Clamp(a, 0, 255);

            SetColour(new Color(r, g, b, a));
        }

        /// <summary>
        /// A setter method for changing the drawing location of this Text object.
        /// </summary>
        /// <param name="newPosition">The position that this Text object should be drawn at.</param>
        public void SetPosition(Vector2 newPosition)
        {
            _position = newPosition;
        }
      
        /// <summary>
        /// A setter method for applying a new font to this Text object's string.
        /// </summary>
        /// <param name="fontName">The new font that should be applied.</param>
        public void SetFont(string fontName)
        {
            _fontName = fontName;
            try
            {
                _font = Core.GetResource<SpriteFont>(_fontName);
            }
            catch (InvalidCastException)
            {
                try
                {
                    _bmFont = Core.GetResource<BitmapFont>(_fontName);
                }
                catch
                {
                    throw;
                }
            }
            Remeasure();
        }

        /// <summary>
        /// The rendering function for this Text object. <b>Called automatically by the game's Screen</b>.
        /// </summary>
        /// <param name="spriteBatch">The current batch of sprites for rendering in the MonoGame pipeline.</param>
        internal override void Draw(SpriteBatch spriteBatch)
        {
            if(IsVisible == true)
            {
                if(_font != null)
                    spriteBatch.DrawString(_font, _message, _position, _colour, 0, _origin, _scale, SpriteEffects.None, 0);
                else if(_bmFont != null)
                    spriteBatch.DrawString(_bmFont, _message, _position, _colour, 0, _origin, _scale, SpriteEffects.None, 0);
            }
        }

        internal bool IsInWorldSpace()
        {
            return !_inScreenSpace;
        }

        private void Remeasure()
        {
            if(_font != null)
            {
                _size = _font.MeasureString(_message) * _scale;
                SetOrigin(0.0f, 0.0f);
            }
            
        }
    }
}
