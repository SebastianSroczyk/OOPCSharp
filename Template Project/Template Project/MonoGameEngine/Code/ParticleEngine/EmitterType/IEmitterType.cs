using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGameEngine.ParticleEngine
{
    internal interface IEmitterType
    {
        Vector2 GetParticleDirection();
        Vector2 GetParticlePosition(Vector2 emitterPosition);
    }
}
