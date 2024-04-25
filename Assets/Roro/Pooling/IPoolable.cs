using UnityEngine;

namespace Pooling
{
    public interface IPoolable<T>
    {
        public T Return(Transform parent = null);
        public T Get();
    }
}
