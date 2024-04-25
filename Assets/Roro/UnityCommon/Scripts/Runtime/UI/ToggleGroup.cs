using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.UI
{
	public class ToggleGroup : MonoBehaviour
	{
		public Toggle[] toggles;

		private void Start()
		{
			for (var i = 0; i < toggles.Length; i++)
			{
				var tog = toggles[i];
				int j = i;
				tog.onValueChanged.AddListener(e =>
				{
					if (!e)
					{
						tog.SetIsOnWithoutNotify(true);
						return;
					}
					
					DisableAllExcept(j);
				});
			}
		}


		private void DisableAllExcept(int j)
		{
			for (var i = 0; i < toggles.Length; i++)
			{
				toggles[i].SetIsOnWithoutNotify(i == j);
			}
		}
	}
}
