/*Code belongs to craftworkgames' MonoGame.Extended library, with minor tweaks by me.*/
using System;
using System.Collections.Generic;

namespace MonoGameEngine.Extended
{
    internal class BitmapFontRegion
    {
        internal BitmapFontRegion(TextureRegion2D textureRegion, int character, int xOffset, int yOffset, int xAdvance)
        {
            TextureRegion = textureRegion;
            Character = character;
            XOffset = xOffset;
            YOffset = yOffset;
            XAdvance = xAdvance;
            Kernings = new Dictionary<int, int>();
        }

        internal int Character { get; }
        internal TextureRegion2D TextureRegion { get; }
        internal int XOffset { get; }
        internal int YOffset { get; }
        internal int XAdvance { get; }
        internal int Width => TextureRegion.Width;
        internal int Height => TextureRegion.Height;
        internal Dictionary<int, int> Kernings { get; }

        public override string ToString()
        {
            return $"{Convert.ToChar(Character)} {TextureRegion}";
        }
    }
}