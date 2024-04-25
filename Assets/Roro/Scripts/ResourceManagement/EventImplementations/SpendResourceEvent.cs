using Based.ResourceManagement;
using Events;
using UnityEngine;

namespace Roro.Scripts.ResourceManagement.EventImplementations
{
    public class SpendResourceEvent : Event<SpendResourceEvent>
    {
        public ResourceType ResourceType;
        public int Amount;
        
        public static SpendResourceEvent Get(ResourceType type, int amount)
        {
            var evt = GetPooledInternal();
            evt.Amount = amount;
            evt.ResourceType = type;
            return evt;
        }
    }
}
