using System.Collections.Generic;

namespace Pooling
{
	// TODO: Use for pooling when List<T> is needed
	public static class ListPool<T>
	{
		private static readonly Queue<List<T>> Queue = new Queue<List<T>>(4);

		public static List<T> Get()
		{
			if (Queue.Count < 1)
			{
				Queue.Enqueue(new List<T>());
			}

			return Queue.Dequeue();
		}

		public static void Release(List<T> list)
		{
			list.Clear();
			Queue.Enqueue(list);
		}
	}
}
