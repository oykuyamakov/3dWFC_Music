using UnityEngine;

namespace UI
{
	public class RectTransformSizeToggler : MonoBehaviour
	{
		public Vector2 OnSize = Vector2.up * 40f;
		public Vector2 OffSize = Vector2.zero;

		public void Toggle(bool e)
		{
			var val = OffSize;
			if (e)
			{
				val = OnSize;
			}

			GetComponent<RectTransform>().sizeDelta = val;
		}
	}
}
