using Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SceneManagement
{
	public class SceneTransitionHelper : MonoBehaviour
	{
		public SceneId targetScene = SceneId.None;

		[Button]
		public void DoTransition()
		{
			using (var evt = SceneChangeRequestEvent.Get(targetScene))
			{
				evt.SendGlobal();
			}
		}
	}
}
