using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
	public delegate void EventListener<in T>(T evt) where T : Event;

	[System.Serializable]
	public class Event : IDisposable
	{
		public EventResult result = EventResult.None;

		public object target = null;

		public bool IsConsumed { get; protected set; }

		public Event()
		{
		}

		public void Consume()
		{
			IsConsumed = true;
		}

		public virtual void Dispose()
		{
		}
	}

	public abstract class Event<T> : Event where T : Event<T>, new()
	{
		private const int PoolSizeWarningThreshold = 3;

		private static readonly Stack<T> poolStack = new Stack<T>();
		private static int createdObjectCount = 0;

		public static T Get() => GetPooledInternal();

		protected static T GetPooledInternal()
		{
			if (poolStack.Count > 0)
			{
				var evt = poolStack.Pop();
				evt.Reset();

				return evt;
			}
			else
			{
				var evt = new T();

				createdObjectCount++;
				if (createdObjectCount > PoolSizeWarningThreshold)
				{
					// Debug.LogError(
					// 	$"{createdObjectCount} instances of {typeof(T).Name} have been created in total.");
				}

				return evt;
			}
		}

		private static void Return(T evt)
		{
			poolStack.Push(evt);
		}


		protected virtual void Reset()
		{
			result = EventResult.None;
			target = null;
			IsConsumed = false;
		}

		public override void Dispose()
		{
			Return((T) this);
		}
	}
}
