using System.Collections.Generic;

namespace Pooling
{
    public static class DictionaryPool<T1,T2> 
    {
        private static readonly Queue<Dictionary<T1,T2>> Queue = new Queue<Dictionary<T1,T2>>(4);

        public static Dictionary<T1,T2> Get()
        {
            if (Queue.Count < 1)
            {
                Queue.Enqueue(new Dictionary<T1, T2>());
            }

            return Queue.Dequeue();
        }

        public static void Release(Dictionary<T1, T2> dict)
        {
            dict.Clear();
            Queue.Enqueue(dict);
        }
    }
}