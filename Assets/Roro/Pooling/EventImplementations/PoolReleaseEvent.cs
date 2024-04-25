using Events;
using UnityEngine;

namespace Pooling.EventImplementations
{
    public class PoolReleaseEvent<T> : Event<PoolReleaseEvent<T>>
    {
        public IPoolable<T> Poolable;
        public static PoolReleaseEvent<T> Get(IPoolable<T> poolable)
        {
            var evt = GetPooledInternal();
            evt.Poolable = poolable;
            return evt;
        }
    }
}
