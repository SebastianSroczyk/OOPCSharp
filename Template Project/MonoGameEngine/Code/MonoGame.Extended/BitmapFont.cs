﻿/*Code belongs to craftworkgames' MonoGame.Extended library, with minor tweaks by me.*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameEngine.Extended
{
    internal class BitmapFont
    {
        private readonly Dictionary<int, BitmapFontRegion> _characterMap = new Dictionary<int, BitmapFontRegion>();

        internal BitmapFont(string name, IEnumerable<BitmapFontRegion> regions, int lineHeight)
        {
            foreach (var region in regions)
                _characterMap.Add(region.Character, region);

            Name = name;
            LineHeight = lineHeight;
        }

        internal string Name { get; }
        internal int LineHeight { get; }
        internal int LetterSpacing { get; set; }
        internal static bool UseKernings { get; set; } = true;

        internal BitmapFontRegion GetCharacterRegion(int character)
        {
            return _characterMap.TryGetValue(character, out var region) ? region : null;
        }

        internal Vector2 MeasureString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Vector2.Zero;

            var stringRectangle = GetStringRectangle(text);
            return new Vector2(stringRectangle.Width, stringRectangle.Height);
        }

        internal Vector2 MeasureString(StringBuilder text)
        {
            if (text == null || text.Length == 0)
                return Vector2.Zero;

            var stringRectangle = GetStringRectangle(text);
            return new Vector2(stringRectangle.Width, stringRectangle.Height);
        }

        internal Rectangle GetStringRectangle(string text)
        {
            return GetStringRectangle(text, Vector2.Zero);
        }

        internal Rectangle GetStringRectangle(string text, Vector2 position)
        {
            var glyphs = GetGlyphs(text, position);
            var rectangle = new Rectangle((int)position.X, (int)position.Y, 0, LineHeight);

            foreach (var glyph in glyphs)
            {
                if (glyph.FontRegion != null)
                {
                    var right = glyph.Position.X + glyph.FontRegion.Width;

                    if (right > rectangle.Right)
                        rectangle.Width = (int)(right - rectangle.Left);
                }

                if (glyph.Character == '\n')
                    rectangle.Height += LineHeight;
            }

            return rectangle;
        }

        internal Rectangle GetStringRectangle(StringBuilder text, Vector2? position = null)
        {
            var position1 = position ?? new Vector2();
            var glyphs = GetGlyphs(text, position1);
            var rectangle = new Rectangle((int)position1.X, (int)position1.Y, 0, LineHeight);

            foreach (var glyph in glyphs)
            {
                if (glyph.FontRegion != null)
                {
                    var right = glyph.Position.X + glyph.FontRegion.Width;

                    if (right > rectangle.Right)
                        rectangle.Width = (int)(right - rectangle.Left);
                }

                if (glyph.Character == '\n')
                    rectangle.Height += LineHeight;
            }

            return rectangle;
        }

        internal StringGlyphEnumerable GetGlyphs(string text, Vector2? position = null)
        {
            return new StringGlyphEnumerable(this, text, position);
        }

        internal StringBuilderGlyphEnumerable GetGlyphs(StringBuilder text, Vector2? position)
        {
            return new StringBuilderGlyphEnumerable(this, text, position);
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        internal struct StringGlyphEnumerable : IEnumerable<BitmapFontGlyph>
        {
            private readonly StringGlyphEnumerator _enumerator;

            internal StringGlyphEnumerable(BitmapFont font, string text, Vector2? position)
            {
                _enumerator = new StringGlyphEnumerator(font, text, position);
            }

            public StringGlyphEnumerator GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator<BitmapFontGlyph> IEnumerable<BitmapFontGlyph>.GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _enumerator;
            }
        }

        internal struct StringGlyphEnumerator : IEnumerator<BitmapFontGlyph>
        {
            private readonly BitmapFont _font;
            private readonly string _text;
            private int _index;
            private readonly Vector2 _position;
            private Vector2 _positionDelta;
            private BitmapFontGlyph _currentGlyph;
            private BitmapFontGlyph? _previousGlyph;

            object IEnumerator.Current
            {
                get
                {
                    // casting a struct to object will box it, behaviour we want to avoid...
                    throw new InvalidOperationException();
                }
            }

            public BitmapFontGlyph Current => _currentGlyph;

            public StringGlyphEnumerator(BitmapFont font, string text, Vector2? position)
            {
                _font = font;
                _text = text;
                _index = -1;
                _position = position ?? new Vector2();
                _positionDelta = new Vector2();
                _currentGlyph = new BitmapFontGlyph();
                _previousGlyph = null;
            }

            public bool MoveNext()
            {
                if (++_index >= _text.Length)
                    return false;

                var character = GetUnicodeCodePoint(_text, ref _index);
                _currentGlyph.Character = character;
                _currentGlyph.FontRegion = _font.GetCharacterRegion(character);
                _currentGlyph.Position = _position + _positionDelta;

                if (_currentGlyph.FontRegion != null)
                {
                    _currentGlyph.Position.X += _currentGlyph.FontRegion.XOffset;
                    _currentGlyph.Position.Y += _currentGlyph.FontRegion.YOffset;
                    _positionDelta.X += _currentGlyph.FontRegion.XAdvance + _font.LetterSpacing;
                }

                if (UseKernings && _previousGlyph?.FontRegion != null)
                {
                    if (_previousGlyph.Value.FontRegion.Kernings.TryGetValue(character, out var amount))
                    {
                        _positionDelta.X += amount;
                        _currentGlyph.Position.X += amount;
                    }
                }

                _previousGlyph = _currentGlyph;

                if (character != '\n')
                    return true;

                _positionDelta.Y += _font.LineHeight;
                _positionDelta.X = 0;
                _previousGlyph = null;

                return true;
            }

            private static int GetUnicodeCodePoint(string text, ref int index)
            {
                return char.IsHighSurrogate(text[index]) && ++index < text.Length
                    ? char.ConvertToUtf32(text[index - 1], text[index])
                    : text[index];
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                _positionDelta = new Vector2();
                _index = -1;
                _previousGlyph = null;
            }
        }

        internal struct StringBuilderGlyphEnumerable : IEnumerable<BitmapFontGlyph>
        {
            private readonly StringBuilderGlyphEnumerator _enumerator;

            internal StringBuilderGlyphEnumerable(BitmapFont font, StringBuilder text, Vector2? position)
            {
                _enumerator = new StringBuilderGlyphEnumerator(font, text, position);
            }

            public StringBuilderGlyphEnumerator GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator<BitmapFontGlyph> IEnumerable<BitmapFontGlyph>.GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _enumerator;
            }
        }

        internal struct StringBuilderGlyphEnumerator : IEnumerator<BitmapFontGlyph>
        {
            private readonly BitmapFont _font;
            private readonly StringBuilder _text;
            private int _index;
            private readonly Vector2 _position;
            private Vector2 _positionDelta;
            private BitmapFontGlyph _currentGlyph;
            private BitmapFontGlyph? _previousGlyph;

            object IEnumerator.Current
            {
                get
                {
                    // casting a struct to object will box it, behaviour we want to avoid...
                    throw new InvalidOperationException();
                }
            }

            public BitmapFontGlyph Current => _currentGlyph;

            public StringBuilderGlyphEnumerator(BitmapFont font, StringBuilder text, Vector2? position)
            {
                _font = font;
                _text = text;
                _index = -1;
                _position = position ?? new Vector2();
                _positionDelta = new Vector2();
                _currentGlyph = new BitmapFontGlyph();
                _previousGlyph = null;
            }

            public bool MoveNext()
            {
                if (++_index >= _text.Length)
                    return false;

                var character = GetUnicodeCodePoint(_text, ref _index);
                _currentGlyph = new BitmapFontGlyph
                {
                    Character = character,
                    FontRegion = _font.GetCharacterRegion(character),
                    Position = _position + _positionDelta
                };

                if (_currentGlyph.FontRegion != null)
                {
                    _currentGlyph.Position.X += _currentGlyph.FontRegion.XOffset;
                    _currentGlyph.Position.Y += _currentGlyph.FontRegion.YOffset;
                    _positionDelta.X += _currentGlyph.FontRegion.XAdvance + _font.LetterSpacing;
                }

                if (UseKernings && _previousGlyph.HasValue && _previousGlyph.Value.FontRegion != null)
                {
                    int amount;
                    if (_previousGlyph.Value.FontRegion.Kernings.TryGetValue(character, out amount))
                    {
                        _positionDelta.X += amount;
                        _currentGlyph.Position.X += amount;
                    }
                }

                _previousGlyph = _currentGlyph;

                if (character != '\n')
                    return true;

                _positionDelta.Y += _font.LineHeight;
                _positionDelta.X = _position.X;
                _previousGlyph = null;

                return true;
            }

            private static int GetUnicodeCodePoint(StringBuilder text, ref int index)
            {
                return char.IsHighSurrogate(text[index]) && ++index < text.Length
                    ? char.ConvertToUtf32(text[index - 1], text[index])
                    : text[index];
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                _positionDelta = new Vector2();
                _index = -1;
                _previousGlyph = null;
            }
        }
    }

    internal struct BitmapFontGlyph
    {
        internal int Character;
        internal Vector2 Position;
        internal BitmapFontRegion FontRegion;
    }
}