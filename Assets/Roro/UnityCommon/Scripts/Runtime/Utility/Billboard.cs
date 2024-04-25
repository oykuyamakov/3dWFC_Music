using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	[ExecuteInEditMode]
	public class Billboard : MonoBehaviour
	{
		private static readonly Vector3 v3up = Vector3.up;

		[SerializeField]
		private bool switchDirection = true;

		private Transform myTransform;

		private Transform camTransform;


		private void Awake()
		{
			myTransform = transform;

			camTransform = Camera.main.transform;
		}


		private void LateUpdate()
		{
#if UNITY_EDITOR
			if (camTransform == null)
				Awake();
#endif

			var direction = switchDirection ? camTransform.forward : -camTransform.forward;

			myTransform.rotation = Quaternion.LookRotation(direction, myTransform.up);
		}
	}
}
