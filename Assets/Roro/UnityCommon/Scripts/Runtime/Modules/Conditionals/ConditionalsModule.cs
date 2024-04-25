using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityCommon.Modules
{
	public class ConditionalsModule : Module<ConditionalsModule>
	{
		internal const int POOL_CAPACITY = 128;

		internal List<Conditional> conditionals = new List<Conditional>(256);

		internal Queue<Conditional> pool;
		// internal HashSet<Conditional> hash;

		[ShowInInspector]
		public int PoolCount => pool.Count;


		internal WaitTimeUpdater waitTimeUpdater = new WaitTimeUpdater();
		internal WaitFrameUpdater waitFrameUpdater = new WaitFrameUpdater();
		internal IfUpdater ifUpdater = new IfUpdater();
		internal WhileUpdater whileUpdater = new WhileUpdater();
		internal RepeatUpdater repeatUpdater = new RepeatUpdater();

		internal int currentCapacity;

		public override void OnEnable()
		{
			Profiler.BeginSample("Conditional Pool Generation");
			if (conditionals == null)
				conditionals = new List<Conditional>(256);

			currentCapacity = POOL_CAPACITY;

			pool = new Queue<Conditional>(POOL_CAPACITY);
			// hash = new HashSet<Conditional>();
			for (var i = 0; i < POOL_CAPACITY; i++)
			{
				var c = new Conditional(i);
				pool.Enqueue(c);
				// hash.Add(c);
			}

			Profiler.EndSample();
		}

		internal void ExpandPool()
		{
			for (int i = 0; i < currentCapacity; i++)
			{
				var c = new Conditional(i + currentCapacity);
				pool.Enqueue(c);
			}

			currentCapacity *= 2;
		}

		internal Conditional Dequeue()
		{
			if (pool.Count < 1)
			{
				// Debug.LogError($"Conditional Pool Empty! Resizing to capacity: {currentCapacity * 2}");
				// ExpandPool();
				Debug.Log(
					$"Conditional pool is empty! Creating new conditional. New size: {currentCapacity + 1}");
				var c_ = new Conditional(currentCapacity);
				pool.Enqueue(c_);
				currentCapacity++;
			}

			var c = pool.Dequeue();
			// hash.Remove(c);
			c.Reset();
#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
			c.stackTrace = StackTraceUtility.ExtractStackTrace();
#endif
			return c;
		}

		internal void Enqueue(Conditional c)
		{
			if (!pool.Contains(c))
				pool.Enqueue(c);
		}


		internal void StartConditional(Conditional c)
		{
			c.isDone = false;
			conditionals.Add(c);
		}


		internal void StopConditional(Conditional c)
		{
			c.isDone = true;
			// Enqueue(c);
			// conditionals.Remove(c);
		}

		public override void OnDisable()
		{
			conditionals?.Clear();
			conditionals = null;
		}

		private void FinishConditional(Conditional cond, int i)
		{
#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
			if (cond.debug)
			{
				Debug.Log("Conditional Done\n" + cond.stackTrace);
			}
#endif

			if (cond.onComplete != null)
			{
#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
				if (cond.debug)
				{
					Debug.Log("Conditional On Complete Invocation\n" + cond.stackTrace);
				}
#endif

				cond.onComplete.Invoke();
			}

			conditionals.RemoveAt(i);

			if (cond.next != null)
			{
#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
				if (cond.debug)
				{
					Debug.Log("Conditional Chaining Next\n" + cond.stackTrace);
				}
#endif

				StartConditional(cond.next);
			}

			Enqueue(cond);
		}

		public override void Update()
		{
			for (int i = conditionals.Count - 1; i >= 0; i--)
			{
				if (i >= conditionals.Count)
					continue;

				var cond = conditionals[i];
				if (cond == null)
				{
					conditionals.RemoveAt(i);
					continue;
				}

				try
				{
					if (cond.isDone)
					{
						FinishConditional(cond, i);
						continue;
					}

#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
					if(cond.debug)
						Debug.Log("Conditional Update stack trace:\n" + cond.stackTrace);
#endif
					cond.Update();

					if (cond.isDone)
					{
						FinishConditional(cond, i);
					}
				}
				catch (Exception ex)
				{
#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
					Debug.Log("Conditional stack trace:\n" + cond.stackTrace);
#endif
					Enqueue(cond);

					if (i >= 0 && i < conditionals.Count)
						conditionals.RemoveAt(i);

					UnityEngine.Debug.Log($"Exception encountered in conditional, removing chain.\n{ex}");
					if (cond.suppressExceptions)
					{
						Debug.Log(ex.ToString());
						// UnityEngine.Debug.Log($"Exception encountered in conditional, removing chain. {ex}");
					}
					else
					{
						throw;
					}
				}
			}
		}

		public override void LateUpdate()
		{
		}
	}
}
