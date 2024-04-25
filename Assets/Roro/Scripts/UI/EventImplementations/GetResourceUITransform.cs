using Based.ResourceManagement;
using Events;
using UnityEngine;

namespace UI.EventImplementations
{
    public class GetResourceUITransform : Event<GetResourceUITransform>
    {
        public ResourceType type;
        public Transform transform;
        
        public static GetResourceUITransform Get(ResourceType type)
        {
            var evt = GetPooledInternal();
            evt.type = type;
            return evt;
        }
    }
}
