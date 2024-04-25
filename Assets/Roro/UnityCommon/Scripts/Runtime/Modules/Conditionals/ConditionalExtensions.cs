using System;

namespace UnityCommon.Modules
{
	public static class ConditionalExtensions
	{
		public static Conditional ThenWait(this Conditional c, float seconds)
		{
			var c1 = Conditional.GetWaitTime(seconds);
			c.next = c1;
			return c1;
		}

		public static Conditional ThenWaitFrames(this Conditional c, int frames)
		{
			var c1 = Conditional.GetWaitFrame(frames);
			c.next = c1;
			return c1;
		}

		public static Conditional ThenIf(this Conditional c, Func<bool> cond)
		{
			var c1 = Conditional.GetIf(cond);
			c.next = c1;
			return c1;
		}

		public static Conditional ThenWhile(this Conditional c, Func<bool> cond)
		{
			var c1 = Conditional.GetWhile(cond);
			c.next = c1;
			return c1;
		}

		public static Conditional ThenFor(this Conditional c, float seconds)
		{
			var c1 = Conditional.For(seconds);
			c.next = c1;
			return c1;
		}

		// public static Conditional Restart(this Conditional conditional)
		// {
		// 	conditional.isDone = false;
		// 	conditional.Reset();
		// 	ConditionalsModule.Instance.conditionals.Add(conditional);
		// 	return conditional;
		// }
	}
}
