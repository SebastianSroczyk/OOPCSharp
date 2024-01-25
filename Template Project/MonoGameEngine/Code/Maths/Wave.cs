using System;

namespace MonoGameEngine.Maths
{
    /// <summary>A static helper class which can be used to perform some useful Sin and Cos wave functionality.</summary>
    public static class Wave
    {
        /// <summary>
        /// A simple wrapper method for generating a y-axis position value along a sine wave.
        /// </summary>
        /// <param name="amplitude">The 'height' of the wave.</param>
        /// <param name="frequency">The number of 'cycles' per second.</param>
        /// <param name="time">The current time, in seconds. Specifies the y-position along the wave that we want.</param>
        /// <param name="offset">The amount to offset the generate y-position by.</param>
        /// <returns>The y-axis value of the given sine waveform at the requested time.</returns>
        public static float Sin(float amplitude, float frequency, float time, float offset = 0)
        {
            return offset + (float)Math.Sin((2.0f * Math.PI) * time * frequency) * amplitude;
        }

        /// <summary>
        /// A simple wrapper method for generating a y-axis position value along a cosine wave.
        /// </summary>
        /// <param name="amplitude">The 'height' of the wave.</param>
        /// <param name="frequency">The number of 'cycles' per second.</param>
        /// <param name="time">The current time, in seconds. Specifies the y-position along the wave that we want.</param>
        /// <param name="offset">The amount to offset the generate y-position by.</param>
        /// <returns>The y-axis value of the given cosine waveform at the requested time.</returns>
        public static float Cos(float amplitude, float frequency, float time, float offset = 0)
        {
            return offset + (float)Math.Cos((2.0f * Math.PI) * time * frequency) * amplitude;
        }
    }
}
