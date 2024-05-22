using Microsoft.Xna.Framework;
using System;

namespace MonoGameEngine.ParticleEngine
{
    internal sealed class ConeEmitterType : IEmitterType
    {
        public Vector2 Direction { get; private set; }
        public float Spread { get; private set; }

        public ConeEmitterType(Vector2 direction, float spread)
        {
            Direction = direction;
            Spread = spread;
        }

        public Vector2 GetParticleDirection()
        {
            if (Direction == null)
                return Vector2.Zero;

            var angle = (float)Math.Atan2(Direction.Y, Direction.X);
            var newAngle = Core.GetRandomNumber(angle - Spread * 0.5f, angle + Spread * 0.5f);

            var particleDirection = new Vector2(MathF.Cos(newAngle), MathF.Sin(newAngle));
            particleDirection.Normalize();
            return particleDirection;
        }

        public Vector2 GetParticlePosition(Vector2 emitterPosition)
        {
            return new Vector2(emitterPosition.X, emitterPosition.Y);
        }
    }
}
