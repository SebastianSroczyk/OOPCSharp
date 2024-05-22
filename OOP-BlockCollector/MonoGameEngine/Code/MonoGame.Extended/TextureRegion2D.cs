/*Code belongs to craftworkgames' MonoGame.Extended library, with minor tweaks by me.*/
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine.Extended
{
    internal sealed class TextureRegion2D
    {
        internal TextureRegion2D(Texture2D texture, int x, int y, int width, int height)
            : this(null, texture, x, y, width, height)
        {
        }

        internal TextureRegion2D(Texture2D texture, Rectangle region)
            : this(null, texture, region.X, region.Y, region.Width, region.Height)
        {
        }

        internal TextureRegion2D(string name, Texture2D texture, Rectangle region)
            : this(name, texture, region.X, region.Y, region.Width, region.Height)
        {
        }

        internal TextureRegion2D(Texture2D texture)
            : this(texture.Name, texture, 0, 0, texture.Width, texture.Height)
        {
        }

        internal TextureRegion2D(string name, Texture2D texture, int x, int y, int width, int height)
        {
            Name = name;
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        internal string Name { get; }
        internal Texture2D Texture { get; private set; }
        internal int X { get; }
        internal int Y { get; }
        internal int Width { get; }
        internal int Height { get; }
        internal Vector2 Size => new Vector2(Width, Height);
        internal object Tag { get; set; }
        internal Rectangle Bounds => new Rectangle(X, Y, Width, Height);

        public override string ToString()
        {
            return $"{Name ?? string.Empty} {Bounds}";
        }
    }
}
