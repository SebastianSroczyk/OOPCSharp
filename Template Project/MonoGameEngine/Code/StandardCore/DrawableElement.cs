using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine.StandardCore
{
    public abstract class DrawableElement
    {
        //public Vector2 Position { get; set; }
        //public Vector2 Scale { get; set; }
        //public float Rotation { get; set; }

        //public bool InWorldSpace { get; set; }
        //public Color Colour { get; set; }
        internal bool IsVisible { get; set; } = true;

        protected DrawableElement() { }

        internal abstract void Draw(SpriteBatch spriteBatch);
    }
}
