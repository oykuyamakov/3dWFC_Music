using UnityEngine;

namespace UnityCommon.Runtime.Inputs
{
	[System.Serializable]
	public class SwipeInput
	{
		public SwipeInput(float sensitivity = 9)
		{
			this.sensitivity = sensitivity;
		}

		public Vector3 direction = Vector3.right;

		[Min(1f)]
		public float sensitivity = 9f;

		public InputScaleMode inputScaleMode;

		public bool requireRelease = true;

		private bool canSwipe = false;

		private Vector3 mouseAnchor;

		public int Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				mouseAnchor = Input.mousePosition;
				canSwipe = true;
				return 0;
			}

			if (Input.GetMouseButtonUp(0))
			{
				canSwipe = false;
				return 0;
			}

			if (Input.GetMouseButton(0) && canSwipe)
			{
				var mousePos = Input.mousePosition;
				var dMousePos = mousePos - mouseAnchor;
				var ds = Vector3.Dot(dMousePos, direction);
				var dsAbs = Mathf.Abs(ds);
				if (dsAbs >= (inputScaleMode == InputScaleMode.SensitivityPerWidth ? Screen.width : Screen.height) /
					sensitivity)
				{
					mouseAnchor = mousePos;
					canSwipe = !requireRelease;
					if (ds < 0)
					{
						return 1;
					}
					else
					{
						return -1;
					}
				}
			}

			return 0;
		}
	}
}
