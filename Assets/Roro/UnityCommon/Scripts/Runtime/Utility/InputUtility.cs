using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityCommon.Utilities
{
	public static class InputUtility
	{
		public static bool GetMouseDown()
		{
			return Input.GetMouseButtonDown(0) && EventSystem.current != null &&
#if UNITY_EDITOR
			       !EventSystem.current.IsPointerOverGameObject();
#elif UNITY_IOS || UNITY_ANDROID
			       !EventSystem.current.IsPointerOverGameObject(0);
#else
			       EventSystem.current.currentSelectedGameObject == null;
#endif
		}

		public static bool GetMouseHold()
		{
			return Input.GetMouseButton(0) && EventSystem.current != null &&
#if UNITY_EDITOR
			       !EventSystem.current.IsPointerOverGameObject();
#elif UNITY_IOS || UNITY_ANDROID
			       !EventSystem.current.IsPointerOverGameObject(0);
#else
			       EventSystem.current.currentSelectedGameObject == null;
#endif
		}

		public static bool GetMouseUp()
		{
			return Input.GetMouseButtonUp(0);
		}
	}
}
