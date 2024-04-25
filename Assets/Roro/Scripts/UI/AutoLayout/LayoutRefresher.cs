using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class LayoutRefresher : MonoBehaviour
	{
		public LayoutGroup LayoutGroup;

		public void Refresh()
		{
			LayoutGroup.enabled = false;
			LayoutGroup.enabled = true;

			Conditional.WaitFrames(2).Do(() =>
			{
				LayoutGroup.enabled = false;
				LayoutGroup.enabled = true;

				Conditional.WaitFrames(2).Do(() =>
				{
					LayoutGroup.enabled = false;
					LayoutGroup.enabled = true;
				});
			});
		}
	}
}
