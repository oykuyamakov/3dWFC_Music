using UnityEngine.UI;

namespace UnityCommon.Runtime.UI
{
	public class SilentCanvasScaler : CanvasScaler
	{
		private bool didScale = false;

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		protected override void Handle()
		{
			if (didScale)
				return;

			didScale = true;

			base.Handle();
		}
	}
}
