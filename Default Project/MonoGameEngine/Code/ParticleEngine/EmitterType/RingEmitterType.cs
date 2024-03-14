using Microsoft.Xna.Framework;
using System;

namespace MonoGameEngine.ParticleEngine
{
    internal sealed class RingEmitterType : IEmitterType
    {
        public Vector2 Direction { get; private set; }
        public float Radius { get; private set; }
        private float Angle { get;  set; }

        public RingEmitterType(Vector2 direction, float radius)
        {
            Direction = direction;
            Radius = radius;
        }

        public Vector2 GetParticleDirection()
        {
            if (Direction == Vector2.Zero)
                return Direction;

            Angle = (float)Math.Atan2(Direction.Y, Direction.X);
            //var newAngle = Core.GetRandomNumber(Angle * 0.5f, Angle * 0.5f);

            var particleDirection = new Vector2(MathF.Cos(Angle), MathF.Sin(Angle));
            particleDirection.Normalize();
            return particleDirection;
        }

        public Vector2 GetParticlePosition(Vector2 emitterPosition)
        {
            var angle = MathHelper.ToRadians(Core.GetRandomNumber(360));
            var perimeterPosition = new Vector2(Radius * MathF.Sin(angle), Radius * MathF.Cos(angle));
            return new Vector2(emitterPosition.X + perimeterPosition.X, emitterPosition.Y + perimeterPosition.Y);
        }
    }
}
