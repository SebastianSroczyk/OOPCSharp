// Base NoiseMap generation based on the work by ven0maus on their blog - https://code2d.wordpress.com/2020/07/21/perlin-noise/
// Supplemented by the seamless noise patching based on the work by Mina Pêcheux on her blog - https://blog.devgenius.io/making-a-seamless-perlin-noise-in-c-970f831ca878 
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGameEngine.Maths
{
    /// <summary>Noise is unfinished and currently does not work due to the ComputePerlinNoise method.</summary>
    internal static class Noise
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="seed"></param>
        /// <param name="scale"></param>
        /// <param name="octaves"></param>
        /// <param name="persistance"></param>
        /// <param name="lacunarity"></param>
        /// <param name="offset"></param>
        /// <param name="seamless"></param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, bool seamless = true)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];
            var random = new Random(seed); // Have to create a new Random object to allow a specific seed to be used

            // We need at least one octave
            if(octaves < 1)
            {
                octaves = 1;
            }

            Vector2[] octavesOffsets = new Vector2[octaves];
            for(int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.X;
                float offsetY = random.Next(-100000, 100000) + offset.Y;
                octavesOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if(scale <= 0f)
            {
                scale = 0.0001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            // When changing noise scale, it zooms from the top-right corner
            // We will use this to make it zoom from the center instead
            float halfWidth = mapWidth * 0.5f;
            float halfHeight = mapHeight * 0.5f;

            for(int x = 0, y; x < mapWidth; x++)
            {
                for(y = 0; y < mapHeight; y++)
                {
                    // Define base values for amplitude, frequency and noiseHeight
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    // Calculate the noise for each octave
                    for(int i = 0; i < octaves; i++)
                    {
                        // We sample a point (x,y)
                        float sampleX = (x - halfWidth) / scale * frequency + octavesOffsets[i].X;
                        float sampleY = (y - halfHeight) / scale * frequency + octavesOffsets[i].Y;

                        float perlinValue = ComputePerlinNoise(sampleX, sampleY) * 2 - 1;

                        // noiseHeight is our final noise, we add all octaves together here
                        noiseHeight += perlinValue * amplitude;
                        amplitude += persistance;
                        frequency *= lacunarity;
                    }

                    // We need to find the min and max noise height in our noisemap
                    // So that we can later interpolate the min and max values between 0 and 1 again
                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    // Assign our noise
                    noiseMap[y, x] = noiseHeight;
                }
            }

            for(int x = 0, y; x < mapWidth; x++)
            {
                for(y = 0; y < mapHeight; y++)
                {
                    // Returns a value between 0f and 1f based on noiseMap value
                    // minNoiseHeight being 0f, and maxNoiseHeight being 1f
                    noiseMap[y, x] = Interpolation.InverseLerp(0f, 1f, noiseMap[y, x]);
                }
            }

            if(seamless)
            {
                MakeSeamlessHorizontally(noiseMap, mapWidth / 16);
                MakeSeamlessVertically(noiseMap, mapHeight / 16);
            }

            return noiseMap;
        }

        /// <summary>
        /// [Overload] 
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="seed">[Optional] </param>
        /// <param name="seamless">[Optional] </param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int? seed = null, bool seamless = true)
        {
            return GenerateNoiseMap(mapWidth, mapHeight, seed ?? Core.GetRandomNumber(Int32.MaxValue), 0, 0, 0.2f, 2.5f, Vector2.Zero, seamless);
        }

        /// <summary>
        /// Based on Ken Perlin's original noise algorithm, converted to C#.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static float ComputePerlinNoise(float x, float y)
        {
            // Determine grid cell coordinates
            int x0 = (int)MathF.Floor(x);
            int x1 = x0 + 1;
            int y0 = (int)MathF.Floor(y);
            int y1 = y0 + 1;

            // Determine interpolation weights
            // Could also use higher order polynomial/s-curve here
            float sx = x - (float)x0;
            float sy = y - (float)y0;

            // Interpolate between grid point gradients
            float n0, n1, ix0, ix1, value;

            n0 = DotGridGradient(x0, y0, x, y);
            n1 = DotGridGradient(x1, y0, x, y);
            ix0 = MathHelper.Lerp(n0, n1, sx);

            n0 = DotGridGradient(x0, y1, x, y);
            n1 = DotGridGradient(x1, y1, x, y);
            ix1 = MathHelper.Lerp(n0, n1, sx);

            value = MathHelper.Lerp(ix0, ix1, sy);
            return value; // Will return in range -1 to 1. To make it in range 0 to 1, multiply by 0.5 and add 0.5
        }

        /// <summary>
        /// Computes the dot product of the distance and gradient vectors.
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static float DotGridGradient(int ix, int iy, float x, float y)
        {
            // Get gradient from integer coordinates
            Vector2 gradient = RandomGradient(ix, iy);

            // Compute the distance vector
            float dx = x - ix;
            float dy = y - iy;

            // Compute the dot-product
            return (dx * gradient.X + dy * gradient.Y);
        }

        /// <summary>
        /// Create pseudorandom direction vector
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <returns></returns>
        private static Vector2 RandomGradient(int ix, int iy)
        {
            // No precomputed gradients mean this works for any number of grid coordinates
            const uint w = 8u * sizeof(uint);
            const uint s = w / 2u; // rotation width
            uint a = (uint)ix, b = (uint)iy;
            a *= 3284157443; b ^= (a << (int)s) | (a >> (int)(w - s));
            b *= 1911520717; a ^= b << (int)s | b >> (int)(w - s);
            a *= 2048419325;
            float random = a * (3.14159265f / ~(~0u >> 1)); // in [0, 2*Pi]
            Vector2 v;
            v.X = MathF.Cos(random); v.Y = MathF.Sin(random);
            return v;
        }

        private static void MakeSeamlessHorizontally(float[,] noiseMap, int stitchWidth)
        {
            int width = noiseMap.GetUpperBound(0) + 1;
            int height = noiseMap.GetUpperBound(1) + 1;

            // Iterate on the stitch band (on the left of the noise)
            for(int x = 0, y; x < stitchWidth; x++)
            {
                // Get the transparency value from a linear gradient
                float v = x / (float)stitchWidth;
                for(y = 0; y < height; y++)
                {
                    // Compute the 'mirrored x position':
                    // The far left is copied on the right and the far right on the left
                    int o = width - stitchWidth + x;

                    // Copy the value on the right of the noise
                    noiseMap[o, y] = MathHelper.Lerp(noiseMap[o, y], noiseMap[stitchWidth - x, y], v);
                }
            }
        }

        private static void MakeSeamlessVertically(float[,] noiseMap, int stitchWidth)
        {
            int width = noiseMap.GetUpperBound(0) + 1;
            int height = noiseMap.GetUpperBound(1) + 1;

            // Iterate through the stitch band (both top and bottom sides are treated simultaneously because it is mirrored)
            for(int y = 0, x; y < stitchWidth; y++)
            {
                // Number of neighbour pixels to consider for the average (= kernel size)
                int k = stitchWidth - y;

                // Go through the entire row
                for(x = 0; x < width; x++)
                {
                    // Compute the sum of pixel values in the top and bottom bands
                    float s1 = 0.0f, s2 = 0.0f;
                    int c = 0;
                    for(int o = x - k; o < x + k; o++)
                    {
                        if(o < 0 || o >= width)
                            continue;

                        s1 += noiseMap[o, y];
                        s2 += noiseMap[o, height - y - 1];
                        c++;
                    }

                    // Compute the means and assign them to the pixels
                    // in the top and bottom rows
                    noiseMap[x, y] = s1 / (float)c;
                    noiseMap[x, height - y - 1] = s2 / (float)c;
                }
            }
        }
    }
}
