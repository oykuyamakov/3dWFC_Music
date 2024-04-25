using System.Collections.Generic;
using System.Linq;
using Events;
using Pooling.EventImplementations;
using Roro.Scripts.Misc;
using Roro.Scripts.Misc.EventImplementations;
using UnityEngine;

namespace Roro.Scripts.GameManagement
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField]
        private List<Particle> m_ParticlePrefabs;

        [SerializeField]
        private Transform m_ParticleHolder;

        private Dictionary<ParticleType, Pooling.ObjectPool<Particle>>
            m_ProjectilePoolsByTypes = new Dictionary<ParticleType, Pooling.ObjectPool<Particle>>();

        private Dictionary<ParticleType, Particle> m_ProjectilePrefabsByTypes =>
            m_ParticlePrefabs.ToDictionary(key => key.ParticleType, k => k);


        private void Awake()
        {
            GEM.AddListener<ParticleSpawnEvent>(OnSpawnParticle);
            GEM.AddListener<PoolReleaseEvent<Particle>>(DequeueParticle);
        }

        private void OnDestroy()
        {
            GEM.RemoveListener<PoolReleaseEvent<Particle>>(DequeueParticle);
            GEM.RemoveListener<ParticleSpawnEvent>(OnSpawnParticle);
        }

        private Particle GetParticle(ParticleType type)
        {
            if (!m_ProjectilePoolsByTypes.ContainsKey(type))
            {
                m_ProjectilePoolsByTypes.Add(type, new Pooling.ObjectPool<Particle>(100));
            }

            return m_ProjectilePoolsByTypes[type]
                .GetPoolable(m_ProjectilePrefabsByTypes[type].gameObject, m_ParticleHolder).Get();
        }

        private void OnSpawnParticle(ParticleSpawnEvent evt)
        {
            var p = GetParticle(evt.ParticleType);
            p.gameObject.SetActive(true);
            evt.Particle = p;
        }

        public void DequeueParticle(PoolReleaseEvent<Particle> evt)
        {
            var pr = evt.Poolable.Return(m_ParticleHolder);
            pr.SelfTransform().SetParent(null);

            m_ProjectilePoolsByTypes[pr.ParticleType].ReleasePoolable(evt.Poolable);
        }
    }

    public enum ParticleType
    {
      
    }
}