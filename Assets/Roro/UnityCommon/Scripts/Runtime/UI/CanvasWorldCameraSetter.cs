using UnityEngine;

namespace UnityCommon.Runtime.UI
{
	public class CanvasWorldCameraSetter : MonoBehaviour
	{
		[HideInInspector]
		[SerializeField]
		private Canvas canvas;

		[Tooltip("If empty or not found, uses Camera.main")]
		public string overrideCameraName = "";

#if UNITY_EDITOR
		private void Reset()
		{
			canvas = GetComponent<Canvas>();
		}
#endif


		private void Start()
		{
			if (string.IsNullOrEmpty(overrideCameraName))
			{
				canvas.worldCamera = Camera.main;
			}
			else
			{
				GameObject camObj = GameObject.Find(overrideCameraName);
				if (camObj == null)
				{
					canvas.worldCamera = Camera.main;
				}
				else
				{
					canvas.worldCamera = camObj.GetComponent<Camera>();
				}
			}
		}
	}
}
