using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
	// TODO: Use for pooling when setting a factory method is needed
	
	// Örnek: FactoryPool<GameObject>.FactoryMethod = ()=> {var obj = new GameObject(); obj.SetActive(false); return obj;}
	// var pooledObj = FactoryPool<GameObject>.Get()
	// FactoryPool<GameObject>.Release(pooledObj);
	public class FactoryPool<T>
	{
		private static readonly Queue<T> Queue = new Queue<T>(4);

		public static Func<T> FactoryMethod;

		public static T Get()
		{
			if (Queue.Count < 1)
			{
				if (FactoryMethod is null)
					throw new Exception($"Pool of type {typeof(T).Name} has no factory method");

				Queue.Enqueue(FactoryMethod());
			}

			return Queue.Dequeue();
		}


		public static void Release(T item)
		{
			Queue.Enqueue(item);
		}
	}
}
