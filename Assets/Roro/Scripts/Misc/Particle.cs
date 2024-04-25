using DG.Tweening;
using Events;
using Pooling;
using Pooling.EventImplementations;
using Roro.Scripts.GameManagement;
using Roro.Scripts.GameManagement.EventImplementations;
using Unity.VisualScripting;
using UnityCommon.Modules;
using UnityEngine;

namespace Roro.Scripts.Misc
{
    public class Particle : MonoBehaviour, IPoolable<Particle>
    {
        [SerializeField]
        private ParticleType m_ParticleType;

        public ParticleType ParticleType => m_ParticleType;
        public Transform SelfTransform() => GetComponent<Transform>();
        
        public ParticleSystem SelfParticleSystem() => GetComponentInChildren<ParticleSystem>();

        private bool m_Disabled;
        private bool m_Enabled;

        private Conditional Cond;

        // TODO: testing
        private Vector3 m_OriginalScale;

        private void Awake()
        {
            GEM.AddListener<LevelEndEvent>(OnReset);
        }
        
        protected void OnReset(LevelEndEvent evt)
        {
            DisableSelf();
        }


        public Particle Return(Transform parent = null)
        {
            SelfTransform().position = Vector3.zero;
            SelfTransform().SetParent(parent);
            m_Enabled = false;
            m_Disabled = true;
            return this;
        }

        public Particle Get()
        {
            m_Disabled = false;
            m_Enabled = true;
            gameObject.SetActive(true);

            return this;
        }

    
        public void MoveToPos(Vector3 pos)
        {
            if(!m_Enabled)
                return;
            
            transform.DOMove(pos, 0.35f);
        }
        
        public void Initialize(Vector3 targetPos, Vector3 dir = default, float dur = 0.54f, float scaleMult = 1)
        {
            m_OriginalScale = SelfTransform().localScale * scaleMult;
            SelfTransform().SetParent(null);
            SelfTransform().position = targetPos;
            SelfTransform().forward = dir;
            
            SelfParticleSystem().Play();

            Cond = Conditional.Wait(dur).Do(DisableSelf);
        }
        
        public void Initialize(Transform parent, bool selfDisable = true)
        {
            m_OriginalScale = SelfTransform().localScale;
            SelfTransform().SetParent(parent);
            SelfTransform().position = parent.position;
            
            SelfParticleSystem().Play();

            if(!selfDisable)
                return;
            
            Cond = Conditional.Wait(SelfParticleSystem().main.startLifetime.constant).Do(() => DisableSelf());
        }

        public void Disable()
        {
            DisableSelf();
        }

        private void OnDestroy()
        {
            Cond?.Cancel();
        }

        private void DisableSelf()
        {
            if (m_Disabled || !m_Enabled)
                return;
            
            m_Disabled = true;

            transform.DOKill();
            
            this.GameObject().SetActive(false);
            Cond?.Cancel();
            
            SelfTransform().localScale = m_OriginalScale;

            using var evt = PoolReleaseEvent<Particle>.Get(this);
            evt.SendGlobal();
        }
    }
}
