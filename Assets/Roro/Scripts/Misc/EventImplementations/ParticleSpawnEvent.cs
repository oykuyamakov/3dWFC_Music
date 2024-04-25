using Events;
using Roro.Scripts.GameManagement;

namespace Roro.Scripts.Misc.EventImplementations
{
    public class ParticleSpawnEvent : Event<ParticleSpawnEvent>
    {
        public ParticleType ParticleType;
        public Particle Particle;

        public static ParticleSpawnEvent Get(ParticleType particleType)
        {
            var evt = GetPooledInternal();
            evt.ParticleType = particleType;
            return evt;
        }
    }
}
