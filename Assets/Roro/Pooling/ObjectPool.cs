using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pooling
{
    public class ObjectPool<T> where T : Component, IPoolable<T>, new()
    {
        private Queue<IPoolable<T>> Pool = new Queue<IPoolable<T>>();
        private int count = 0;
        private int max;

        public virtual IPoolable<T> GetPoolable(GameObject prefab, Transform parent)
        {
            if (Pool.Any())
            {
                return Pool.Dequeue();
            }
            else
            {
                var obj = GameObject.Instantiate(prefab,parent).GetComponent<T>();
                return obj;
            }
        } 
        
        public virtual IPoolable<T> GetPoolable()
        {
            if (Pool.Any())
            {
                return Pool.Dequeue();
            }
            else
            {
                IPoolable<T> obj = new T();
                return obj;
            }
        }

        public void ReleasePoolable(IPoolable<T> obj)
        {

            if (count < max || max == 0)
            {
                Pool.Enqueue(obj);
                // count++;
                //obj.Return();
            }
            else
            {
                Debug.LogError(this + " Exceeds max poolables");
            }
        }

        public ObjectPool(int Max)
        {
            this.max = Max;
            count = 0;
            Pool = new Queue<IPoolable<T>>();
        }
        /// <summary>
        /// Maximum amount of Objects in Pool
        /// </summary>
        public virtual int Max
        {
            set { max = value; }
        }
        /// <summary>
        /// Current amount of Objects in Pool
        /// </summary>
        public virtual int Count
        {
            get { return count; }
        }
    }
}
