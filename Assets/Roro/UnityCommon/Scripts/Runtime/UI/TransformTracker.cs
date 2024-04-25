using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace UnityCommon.Runtime.UI
{
	public class TransformTracker : MonoBehaviour
	{
		public Transform target;

		public bool useSmoothdamp = true;

		[HideIf("$useSmoothdamp")]
		public float sharpness = 20f;

		[ShowIf("$useSmoothdamp")]
		public float smoothness = 0.2f;

		[Space]
		public float margin = 0.05f;

		[Space]
		public Vector3 viewportOffset = Vector3.zero;

		private Transform t;
		
		private RectTransform rect;

		private Vector3 velocity = Vector3.zero;

		private Vector3 position;

		private Camera cam;

		private Vector2 screenSize;

		private Vector3 rectSize;
		

		private void Start()
		{
			screenSize = new Vector2(Screen.width, Screen.height);
			cam = Camera.main;

			t = this.transform;
			rect = this.GetComponent<RectTransform>();

			rectSize = rect.sizeDelta;
			
			position = cam.ScreenToViewportPoint(t.position);
		}

		private void Update()
		{
			var wPos = target.position;
			var dot = Vector3.Dot(cam.transform.forward, cam.transform.position - wPos);
			if (dot >= -0.1f)
			{
				wPos += cam.transform.forward * (dot);
			}

			if (useSmoothdamp)
			{
				position = Vector3.SmoothDamp(position, cam.WorldToViewportPoint(wPos), ref velocity,
				                              smoothness);
			}
			else
			{
				position = Vector3.Lerp(position, cam.WorldToViewportPoint(wPos),
				                        Time.deltaTime * sharpness);
			}

			var boundary = Vector3.one * margin;
			boundary.y *= screenSize.x / screenSize.y;
			position = math.clamp(position, boundary, Vector3.one - boundary);

			t.position = cam.ViewportToScreenPoint(position + viewportOffset);

			rect.sizeDelta = rectSize * target.localScale.x * (1f / Vector3.Distance(cam.transform.position, target.position));
		}
	}
}
