using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine.ComponentCore.GameObjects.Components
{
    internal abstract class DrawableComponent : Component
    {
        protected bool _visible = true;
        protected float _layerDepth = 0f;
        protected float _rotation = 0f;
        protected float _scale = 1.0f;
        protected Vector2 _origin = new Vector2(0, 0);

        public abstract void Render(SpriteBatch spriteBatch);

        /// <summary>
        /// A simple Draw method to quickly get a sprite onto the game screen.
        /// </summary>
        /// <param name="spriteBatch">The utility used to render sprites within MonoGame.</param>
        /// <param name="position">The position, in pixels, to draw the sprite on-screen.</param>
        /// <param name="image">The desired sprite image.</param>
        /// <param name="color">[Optional] The base colour of the image.</param>
        protected void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D image, Color? color = null)
        {
            if(_visible && image != null)
            {
                spriteBatch.Draw(image, 
                    position,
                    null, 
                    color == null ? Color.White : (Color)color,
                    _rotation,
                    _origin,
                    _scale,
                    SpriteEffects.None,
                    _layerDepth);
            } 
        }

        /// <summary>
        /// A simple Draw method to quickly get text onto the game screen.
        /// </summary>
        /// <param name="spriteBatch">The utility used to render sprites within MonoGame.</param>
        /// <param name="position">The position, in pixels, to draw the sprite on-screen.</param>
        /// <param name="font">The desired SpriteFont to use.</param>
        /// <param name="text">The string of text that should be rendered.</param>
        /// <param name="color">[Optional] The base colour of the text.</param>
        protected void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, string text, Color? color = null)
        {
            if (_visible && font != null)
            {
                spriteBatch.DrawString(font,
                    text,
                    position,
                    color == null ? Color.White : (Color)color,
                    _rotation,
                    _origin,
                    _scale, SpriteEffects.None,
                    _layerDepth);
            }  
        }

        /// <summary>
        /// Draw method for rendering a specific frame of animation within a spritesheet.
        /// </summary>
        /// <param name="spriteBatch">The utility used to render sprites within MonoGame.</param>
        /// <param name="drawLocation">Represents where on screen to draw the animation frame. Handles position, width and height.</param>
        /// <param name="animationFrame">The dimensions, in pixels, of the animation frame within the spritesheet</param>
        /// <param name="spritesheet">The full source spritesheet image.</param>
        /// <param name="color">[Optional] The base colour of the image.</param>
        protected void Draw(SpriteBatch spriteBatch, Rectangle drawLocation, Rectangle animationFrame, Texture2D spritesheet, Color? color = null)
        {
            if (_visible && spritesheet != null)
            {
                spriteBatch.Draw(spritesheet, 
                    drawLocation,
                    animationFrame, 
                    color == null ? Color.White : (Color)color,
                    _rotation,
                    _origin,
                    SpriteEffects.None,
                    _layerDepth);
            }
        }

        public bool IsVisible()
        {
            return _visible;
        }

        public float GetLayerDepth()
        {
            return _layerDepth;
        }

        public float GetRotation()
        {
            return _rotation;
        }

        public void SetVisible(bool visible) => _visible = visible;

        public void SetLayerDepth(float depth) => _layerDepth = depth;

        public void SetRotation(float rotation) => _rotation = rotation;
    }
}
