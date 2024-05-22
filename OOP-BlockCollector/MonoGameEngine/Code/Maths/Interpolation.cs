using Microsoft.Xna.Framework;
using System;

namespace MonoGameEngine.Maths
{
    /// <summary>Enum used to trigger different methods of interpolation.
    /// <br/>- <strong>Lerp</strong> uses the standard linear interpolation formula. Has no easing applied.
    /// <br/>- <strong>SmoothLerp</strong> uses the standard linear interpolation formula, with easing applied.
    /// <br/>- <strong>Slerp</strong> uses a spherical interpolation formula, which creates a different kind of easing effect to SmoothLerp.
    /// </summary>
    public enum InterpolationType { Lerp, SmoothLerp, Slerp };

    /// <summary>A static helper class which can be used to call upon several useful interpolation methods.</summary>
    public static class Interpolation
    {
        /// <summary>
        /// Linear interpolation with smoothing applied.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="time">The time along the interpolation curve.</param>
        /// <returns>The floating-point value between point1 and point2, at the given time.</returns>
        public static float SmoothLerp(float point1, float point2, float time)
        {
            return point1 + PerlinSmoothStep(time) * (point2 - point1);
        }

        /// <summary>
        /// [Overload] Standard linear interpolation with smoothing applied.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="time">The time along the interpolation curve.</param>
        /// <returns>The integer value between point1 and point2, at the given time.</returns>
        public static int SmoothLerp(int point1, int point2, float time)
        {
            return (int)(point1 + PerlinSmoothStep(time) * (point2 - point1));
        }

        /// <summary>
        /// Standard linear interpolation calculator.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="time">The time along the interpolation curve.</param>
        /// <returns>The floating-point value between point1 and point2, at the given time.</returns>
        public static float Lerp(float point1, float point2, float time)
        {
            return point1 + time * (point2 - point1);
        }

        /// <summary>
        /// [Overload] Standard linear interpolation calculator.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="time">The time along the interpolation curve.</param>
        /// <returns>The integer value between point1 and point2, at the given time.</returns>
        public static int Lerp(int point1, int point2, float time)
        {
            return (int)(point1 + time * (point2 - point1));
        }

        /// <summary>
        /// Spherical linear interpolation calculator, with smoothing applied.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="time">The time along the interpolation curve.</param>
        /// <returns>The floating-point value between point1 and point2, at the given time.</returns>
        public static float Slerp(float point1, float point2, float time)
        {
            return Lerp(point1, point2, CubicScurve3(PerlinSmoothStep(time)));
        }

        /// <summary>
        /// [Overload] Spherical linear interpolation calculator, with smoothing applied.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="time">The time along the interpolation curve.</param>
        /// <returns>The integer value between point1 and point2, at the given time.</returns>
        public static int Slerp(int point1, int point2, float time)
        {
            return Lerp(point1, point2, CubicScurve3(PerlinSmoothStep(time)));
        }

        private static float CubicScurve3(float alpha)
        {
            return alpha * alpha * (3 - 2 * alpha);
        }

        // Smooth Step Interpolations
        private static float PerlinSmoothStep(float time)
        {
            // Ken Perlin's version
            return time * time * time * ((time * ((6 * time) - 15)) + 10);
        }
        /*
        private static float SmoothStep(float time)
        {
            return time * time * (3 - (2 * time));
        }
        */

        /// <summary>
        /// Determines where a given value lies within a range.
        /// </summary>
        /// <param name="point1">The start of the range.</param>
        /// <param name="point2">The end of the range.</param>
        /// <param name="value">The point within the range you want to calculate.</param>
        /// <param name="clamp">[Optional] Sets whether or not to clamp the returned value between 0 and 1.</param>
        /// <returns>The floating-point value representing where the value parameter falls within the given range.<br/>This value can be clamped or extrapolated based on the clamp parameter.</returns>
        public static float InverseLerp(float point1, float point2, float value, bool clamp = true)
        {
            if (clamp)
                return MathHelper.Clamp((value - point1) / (point2 / point1), 0, 1);
            else
                return (value - point1) / (point2 / point1);
        }
    }

    /// <summary>
    /// Class intended to represent every aspect of Linear Interpolation, in one handy object.<br></br>
    /// Please ensure the number of StartValues and EndValues are the identical.
    /// </summary>
    internal class InterpolationData
    {
        /// <summary>The type of interpolation that should be performed.</summary>
        public InterpolationType Type { get; set; } = InterpolationType.Lerp;
        /// <summary>How far along the interpolation timeline this data is, generally between 0.0 and 1.0.</summary>
        public float LerpTime { get; set; } = 1.0f;
        /// <summary>How quickly the data reaches 1.0f LerpTime.</summary>
        public float SpeedMultiplier { get; set; } = 1.0f;
        /// <summary>A collection of 'current' values, based on the current LerpTime and the StartValues and EndValues provided.</summary>
        public float[] CurrentValues { get; }
        /// <summary>A collection of floating-point 'start' values.</summary>
        public float[] StartValues { get; }
        /// <summary>A collection of floating-point 'end' values.</summary>
        public float[] EndValues { get; }

        /// <summary>
        /// The main constructor for the class. Uses raw float arrays of values that can handle any number of entries.
        /// </summary>
        /// <param name="startValues">A generic array of float values.</param>
        /// <param name="endValues">A generic array of float values.</param>
        /// <param name="speed">A float value which defaults to 1.0f (the interpolation will take exactly 1 second).</param>
        /// <param name="type">An enum which dictates the type of interpolation to perform.</param>
        internal InterpolationData(float[] startValues, float[] endValues, float speed = 1.0f, InterpolationType type = InterpolationType.Lerp)
        {
            StartValues = startValues;
            EndValues = endValues;
            Type = type;
            CurrentValues = new float[StartValues.Length];
            for (int i = 0; i < StartValues.Length; i++)
                CurrentValues[i] = StartValues[i]; // 'Current' values should logically begin at the starting values

            LerpTime = 0.0f;
            SpeedMultiplier = speed;
        }

        /// <summary>
        /// The overloaded constructor for the class. Accepts the MonoGame-specific Vector2 class objects. Useful for moving 2d objects.
        /// </summary>
        /// <param name="startValue">A Vector2 object starting values.</param>
        /// <param name="endValue">A Vector2 object ending values.</param>
        /// <param name="speed">A float value which defaults to 1.0f (the interpolation will take exactly 1 second).</param>
        /// <param name="type">An enum which dictates the type of interpolation to perform.</param>
        internal InterpolationData(Vector2 startValue, Vector2 endValue, float speed = 1.0f, InterpolationType type = InterpolationType.Lerp)
            : this(new float[] { startValue.X, startValue.Y }, new float[] { endValue.X, endValue.Y }, speed, type)
        {

        }

        /// <summary>
        /// Performs the interpolation between the start and end positions given. Also updates the current lerp time using the deltaTime parameter.
        /// </summary>
        /// <param name="deltaTime">A generic float value is used, to decouple from the Monogame framework's GameTime construct.</param>
        internal void UpdateValues(float deltaTime)
        {
            if (LerpTime < 1.0f) // Only perform this update if the lerp is not already finished
            {
                // Update lerp time
                LerpTime += deltaTime / SpeedMultiplier;
                if (LerpTime >= 1.0f) // Ensure the lerp time does not go above 1.0f
                {
                    LerpTime = 1.0f;
                }

                // Update the current values using lerp time, and the start and end values
                for (int i = 0; i < CurrentValues.Length; i++)
                {
                    if (Type == InterpolationType.Lerp)
                        CurrentValues[i] = Interpolation.Lerp(StartValues[i], EndValues[i], LerpTime);
                    else if (Type == InterpolationType.SmoothLerp)
                        CurrentValues[i] = Interpolation.SmoothLerp(StartValues[i], EndValues[i], LerpTime);
                    else if (Type == InterpolationType.Slerp)
                        CurrentValues[i] = Interpolation.Slerp(StartValues[i], EndValues[i], LerpTime);
                }
            }
        }
    }
}
