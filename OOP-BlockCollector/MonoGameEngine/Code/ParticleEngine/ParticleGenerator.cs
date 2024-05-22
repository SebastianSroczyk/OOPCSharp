namespace MonoGameEngine.ParticleEngine
{
    internal class ParticleGenerator // Under construction
    {
        public static ParticleGenerator Instance { get; private set; } = new ParticleGenerator();

        static ParticleGenerator()
        {
        }

        private ParticleGenerator()
        {
        }

        public ParticleEmitter GenerateEmitter(EmitterParticleState particleState, IEmitterType emitterType, int particlesPerUpdate, int maxParticles)
        {
            /*
            IEmitterType emitter;
            switch(emitterType)
            {
                case EmitterType.Cone:
                    emitter = new ConeEmitterType();
                    break;
                case EmitterType.Ring:
                    emitter = new RingEmitterType();
                    break;
                case EmitterType.Point:
                    break;
            }
            */

            return new ParticleEmitter(particleState, emitterType, particlesPerUpdate, maxParticles);
        }
    }
}
