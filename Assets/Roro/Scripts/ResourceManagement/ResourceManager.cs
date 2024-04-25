using Events;
using Roro.Scripts.ResourceManagement.EventImplementations;
using UnityEngine;

namespace Roro.Scripts.ResourceManagement
{
    public class ResourceManager : MonoBehaviour
    {
        
        private void Awake()
        {
            GEM.AddListener<SpendResourceEvent>(OnSpendResourceEvent);
            GEM.AddListener<EarnResourceEvent>(OnEarnResourceEvent);
        }

        private void OnDestroy()
        {
            GEM.RemoveListener<SpendResourceEvent>(OnSpendResourceEvent);
            GEM.RemoveListener<EarnResourceEvent>(OnEarnResourceEvent);
        }

        private void OnSpendResourceEvent(SpendResourceEvent evt)
        {
        }

        private void OnEarnResourceEvent(EarnResourceEvent evt)
        {
           
        }
    }
}