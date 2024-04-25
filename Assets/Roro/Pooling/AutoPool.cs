using System.Collections.Generic;

namespace Pooling
{
	// TODO: Use for pooling when new T() is enough
	public static class AutoPool<T> where T : new()
	{
		private static readonly Queue<T> Queue = new Queue<T>(4);

		public static T Get()
		{
			if (Queue.Count < 1)
			{
				Queue.Enqueue(new T());
			}

			return Queue.Dequeue();
		}

		public static void Release(T item)
		{
			Queue.Enqueue(item);
		}
	}
}
