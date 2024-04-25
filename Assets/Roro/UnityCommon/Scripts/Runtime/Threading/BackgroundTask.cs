using System;

namespace UnityCommon.Threading
{
	public class BackgroundTask
	{

		public bool IsCompleted { get; internal set; } = false;

		public bool Aborted { get; internal set; } = false;

		public float Progress { get; set; } = 0.0f;


		private Action onCompleted;

		public BackgroundTask(Action onCompleted = null)
		{
			this.onCompleted = onCompleted;
		}

	}
}
