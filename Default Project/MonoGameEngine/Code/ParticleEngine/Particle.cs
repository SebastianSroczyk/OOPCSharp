using Microsoft.Xna.Framework;

namespace MonoGameEngine.ParticleEngine
{
    internal sealed class Particle
    {
        public Vector2 Position { get; private set; }
        public float Scale { get; private set; }
        public float Rotation { get; private set; }
        public float Opacity { get; private set; }

        private float _age;
        private float _lifespan;
        private Vector2 _direction;
        private Vector2 _gravity;
        private float _velocity;
        private float _acceleration;
        private float _opacityFadeRate;

        internal Particle() { }

        internal void Activate(float lifespan, Vector2 position, Vector2 direction, Vector2 gravity, 
            float velocity, float acceleration, float scale, float rotation, float opacity, float opacityFadeRate)
        {
            _age = 0;

            _lifespan = lifespan;
            _direction = direction;
            _velocity = velocity;
            _gravity = gravity;
            _acceleration = acceleration;
            _opacityFadeRate = opacityFadeRate;

            Position = position;
            Opacity = opacity;
            Scale = scale;
            Rotation = rotation;
        }

        internal bool Update(float deltaTime)
        {
            _velocity *= _acceleration;
            _direction += _gravity;

            var positionDelta = _direction * _velocity;
            Position += positionDelta;

            Opacity *= _opacityFadeRate;

            // returns true if the particle is still considered alive
            _age += deltaTime;
            return _age < _lifespan;
        }

    }
}
