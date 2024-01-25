using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.StandardCore;
using System;
using System.Collections.Generic;

namespace MonoGameEngine.ParticleEngine
{
    internal class ParticleEmitter : GameObject
    {
        private GameObject _parent;

        private LinkedList<Particle> _activeParticles = new LinkedList<Particle>();
        private LinkedList<Particle> _inactiveParticles = new LinkedList<Particle>();

        private EmitterParticleState _emitterParticleState;
        private IEmitterType _emitterType;
        private int _particleEmittedPerUpdate = 0;
        private int _maxParticles = 0;

        private Rectangle _sourceRectangle;

        public ParticleEmitter(EmitterParticleState particleState, IEmitterType emitterType, int particlesEmittedPerUpdate, int maxParticles)
        {
            _emitterParticleState = particleState;
            _emitterType = emitterType;
            _particleEmittedPerUpdate = particlesEmittedPerUpdate;
            _maxParticles = maxParticles;
        }

        private void EmitNewParticle(Particle particle)
        {
            var lifespan = _emitterParticleState.GenerateLifespan();
            var velocity = _emitterParticleState.GenerateVelocity();
            var scale = _emitterParticleState.GenerateScale();
            var rotation = _emitterParticleState.GenerateRotation();
            var opacity = _emitterParticleState.GenerateOpacity();
            var gravity = _emitterParticleState.Gravity;
            var acceleration = _emitterParticleState.Acceleration;
            var opacityFadeRate = _emitterParticleState.OpacityFadeRate;

            var direction = _emitterType.GetParticleDirection();
            var position = _emitterType.GetParticlePosition(GetPosition() + (_parent == null ? Vector2.Zero : _parent.GetCenter()));

            particle.Activate(lifespan, position, direction, gravity, velocity, acceleration, scale, rotation, opacity, opacityFadeRate);
            _activeParticles.AddLast(particle);
        }

        private void EmitParticles()
        {
            // Ensure we haven't hit the particle limit
            if (_activeParticles.Count >= _maxParticles)
                return;

            var maxCreationAmount = _maxParticles - _activeParticles.Count;
            var neededParticles = Math.Min(maxCreationAmount, _particleEmittedPerUpdate);

            // Reuse inactive particles first before creating new ones
            var numberToReuse = Math.Min(_inactiveParticles.Count, neededParticles);
            var numberToCreate = neededParticles - numberToReuse;

            for(int i = 0; i < numberToReuse; i++)
            {
                var particleNode = _inactiveParticles.First;

                EmitNewParticle(particleNode.Value);
                _inactiveParticles.Remove(particleNode);
            }

            for(int i = 0; i < numberToCreate; i++)
            {
                EmitNewParticle(new Particle());
            }
        }

        public override void Update(float deltaTime)
        {
            EmitParticles();

            var particleNode = _activeParticles.First;
            while(particleNode != null)
            {
                var nextNode = particleNode.Next;
                var stillAlive = particleNode.Value.Update(deltaTime);
                if(!stillAlive)
                {
                    _activeParticles.Remove(particleNode);
                    _inactiveParticles.AddLast(particleNode.Value);
                }

                particleNode = nextNode;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if(_sourceRectangle.Width == 0 || _sourceRectangle.Height == 0)
                if(GetSprite() is AnimatedSprite)
                    _sourceRectangle = GetAnimatedSprite().GetAnimationFrame();
                else
                    _sourceRectangle = new Rectangle(0, 0, GetSprite().GetWidth(), GetSprite().GetHeight());

            var sprite = GetSprite();

            foreach (var particle in _activeParticles)
            {
                spriteBatch.Draw(sprite.GetTexture(), 
                    particle.Position, 
                    _sourceRectangle, 
                    Color.White * particle.Opacity, 
                    particle.Rotation,
                    sprite.GetOrigin(), 
                    particle.Scale, 
                    SpriteEffects.None,
                    sprite.GetLayerDepth());
            }
        }

        public void AttachTo(GameObject parent)
        {
            _parent = parent;
        }
    }
}
