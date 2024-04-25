using Based.ResourceManagement;
using Events;
using UnityEngine;

namespace Roro.Scripts.ResourceManagement.EventImplementations
{
    public class EarnResourceEvent : Event<EarnResourceEvent>
    {
        public ResourceType Type;
        public int Amount;
        
        public static EarnResourceEvent Get(ResourceType type, int amount)
        {
            var evt = GetPooledInternal();
            evt.Amount = amount;
            evt.Type = type;
            return evt;
        }
    }
}
