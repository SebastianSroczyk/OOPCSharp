using Microsoft.Xna.Framework;

namespace MonoGameEngine.ParticleEngine
{
    internal abstract class EmitterParticleState
    {
        public abstract float MinLifespan { get; }
        public abstract float MaxLifespan { get; }

        public abstract float Velocity { get; }
        public abstract float VelocityDeviation { get; }
        public abstract float Acceleration { get; }

        public abstract Vector2 Gravity { get; }

        public abstract float Opacity { get; }
        public abstract float OpacityDeviation { get; }
        public abstract float OpacityFadeRate { get; }

        public abstract float Rotation { get; }
        public abstract float RotationDeviation { get; }

        public abstract float Scale { get; }
        public abstract float ScaleDeviation { get; }

        public float GenerateLifespan()
        {
            return Core.GetRandomNumber(MinLifespan, MaxLifespan);
        }

        public float GenerateVelocity()
        {
            return GenerateFloat(Velocity, VelocityDeviation);
        }

        public float GenerateOpacity()
        {
            return GenerateFloat(Opacity, OpacityDeviation);
        }

        public float GenerateRotation()
        {
            return GenerateFloat(Rotation, RotationDeviation);
        }

        public float GenerateScale()
        {
            return GenerateFloat(Scale, ScaleDeviation);
        }

        protected float GenerateFloat(float baseNumber, float deviation)
        {
            var halfDeviation = deviation * 0.5f;
            return Core.GetRandomNumber(baseNumber - halfDeviation, baseNumber + halfDeviation);
        }
    }
}
