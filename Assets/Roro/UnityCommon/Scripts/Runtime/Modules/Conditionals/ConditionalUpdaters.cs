using UnityEngine;

namespace UnityCommon.Modules
{
	internal interface IConditionalUpdater
	{
		void Update(Conditional c);
	}


	internal class WaitTimeUpdater : IConditionalUpdater
	{
		public void Update(Conditional c)
		{
			c.f -= c.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
			if (c.f <= 0f)
			{
				c.act?.Invoke();
				c.isDone = true;
			}
		}
	}

	internal class WaitFrameUpdater : IConditionalUpdater
	{
		public void Update(Conditional c)
		{
			c.i -= 1;

			if (c.i <= 0)
			{
				c.act?.Invoke();
				c.isDone = true;
			}
		}
	}


	internal class IfUpdater : IConditionalUpdater
	{
		public void Update(Conditional c)
		{
			if (c.cond.Invoke())
			{
				c.act?.Invoke();
				c.isDone = true;
			}
		}
	}

	internal class WhileUpdater : IConditionalUpdater
	{
		public void Update(Conditional c)
		{
			if (c.cond.Invoke())
			{
				c.act?.Invoke();
			}
			else
			{
				c.isDone = true;
			}
		}
	}

	internal class RepeatUpdater : IConditionalUpdater
	{
		public void Update(Conditional c)
		{
			c.f -= c.unscaledTime ? Time.unscaledTime : Time.deltaTime;
			if (c.f <= 0f)
			{
				c.act.Invoke();
				c.i--;
				if (c.i <= 0)
				{
					c.isDone = true;
					return;
				}

				c.f = c.f1;
			}
		}
	}
}
