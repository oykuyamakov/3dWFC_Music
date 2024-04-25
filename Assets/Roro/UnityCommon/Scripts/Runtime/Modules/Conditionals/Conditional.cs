using System;
using UnityEngine;

namespace UnityCommon.Modules
{
	public class Conditional
	{
		internal static ConditionalsModule I => ConditionalsModule.Instance;

		public int id;

#if UNITY_EDITOR && CONDITIONAL_STACK_TRACE
		internal string stackTrace = "NULL";
#endif

		private bool _isDone;
		public bool isDone
		{
			get => _isDone;
			set
			{
				_isDone = value;

				if(debug)
					UnityEngine.Debug.Log("IsDone: " + value);
			}
		}

		public float remaining => f;

		internal bool suppressExceptions = false;


		internal IConditionalUpdater updater;

		internal Conditional next;

		internal Func<bool> cond;
		internal Action act;
		internal Func<bool> cancelCond;

		internal Action onComplete;

		internal float f;
		internal float f1;
		internal int i;

		internal bool unscaledTime;

		internal bool debug;

		public Conditional(int id)
		{
			this.id = id;
		}

		public override int GetHashCode()
		{
			return id;
		}

		public void Reset()
		{
			f = -1f;
			f1 = -1f;
			i = -1;
			updater = null;
			next = null;
			cond = null;
			act = null;
			cancelCond = null;
			onComplete = null;
			isDone = false;
			suppressExceptions = false;
			unscaledTime = false;
			debug = false;
			//stackTrace = "";
		}

		public void Update()
		{
			if (cancelCond != null && cancelCond.Invoke())
			{
				isDone = true;
				return;
			}

			updater.Update(this);
		}

		public Conditional Cancel()
		{
			I.StopConditional(this);
			return this;
		}

		public Conditional Debug(bool debug = true)
		{
			this.debug = debug;
			return this;
		}

		public Conditional SuppressExceptions(bool suppress = true)
		{
			suppressExceptions = suppress;
			return this;
		}

		public Conditional UnscaledTime(bool unscaled = true)
		{
			unscaledTime = unscaled;
			return this;
		}

		public Conditional CancelIf(Func<bool> cancelCond)
		{
			this.cancelCond = cancelCond;
			return this;
		}

		public Conditional Do(Action act)
		{
			this.act = act;
			return this;
		}

		public Conditional OnComplete(Action act)
		{
			onComplete = act;
			return this;
		}

		internal static Conditional GetWaitTime(float time)
		{
			var c = I.Dequeue();
			c.updater = I.waitTimeUpdater;
			c.f = time;
			return c;
		}

		internal static Conditional GetWaitFrame(int frame)
		{
			var c = I.Dequeue();
			c.updater = I.waitFrameUpdater;
			c.i = frame;
			return c;
		}

		internal static Conditional GetIf(Func<bool> cond)
		{
			var c = I.Dequeue();
			c.updater = I.ifUpdater;
			c.cond = cond;
			return c;
		}

		internal static Conditional GetWhile(Func<bool> cond)
		{
			var c = I.Dequeue();
			c.updater = I.whileUpdater;
			c.cond = cond;
			return c;
		}

		internal static Conditional GetRepeat(float timeStep, int repetitions, Action act)
		{
			var c = I.Dequeue();
			c.updater = I.repeatUpdater;
			c.f = timeStep;
			c.f1 = timeStep;
			c.i = repetitions;
			c.act = act;
			return c;
		}


		public static Conditional Wait(float seconds)
		{
			var c = GetWaitTime(seconds);
			I.StartConditional(c);
			return c;
		}

		public static Conditional WaitFrames(int frames)
		{
			var c = GetWaitFrame(frames);
			I.StartConditional(c);
			return c;
		}

		public static Conditional If(Func<bool> cond)
		{
			var c = GetIf(cond);
			I.StartConditional(c);
			return c;
		}

		public static Conditional While(Func<bool> cond)
		{
			var c = GetWhile(cond);
			I.StartConditional(c);
			return c;
		}


		public static Conditional Repeat(float timeStep, int repetitions, Action act)
		{
			var c = GetRepeat(timeStep, repetitions, act);
			I.StartConditional(c);
			return c;
		}

		public static Conditional RepeatNow(float timeStep, int repetitions, Action act)
		{
			act.Invoke();
			if (repetitions > 1)
			{
				return Repeat(timeStep, repetitions - 1, act);
			}

			return Conditional.WaitFrames(0); // Dummy conditional to not break the chain
		}

		public static Conditional For(float seconds)
		{
			var time = Time.time + seconds;
			var cond = While(() => Time.time < time);
			return cond;
		}
	}
}
