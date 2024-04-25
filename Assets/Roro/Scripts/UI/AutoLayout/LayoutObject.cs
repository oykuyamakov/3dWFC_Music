using System;
using Roro.Scripts.UI.AutoLayout;
using UnityEngine;

namespace UI
{
	[ExecuteAlways]
	[DefaultExecutionOrder(-2)]
	public class LayoutObject : MonoBehaviour
	{
		[NonSerialized]
		public RectTransform RectTransform;

		public string Id = "id";

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (Application.isPlaying)
				return;
			
			LayoutEngine.Run();
		}
#endif

		private void OnEnable()
		{
			RectTransform = GetComponent<RectTransform>();
			LayoutEngine.OnLayoutObjectCreated(this);
		}

		private void OnDisable()
		{
			LayoutEngine.OnLayoutObjectDestroyed(this);
		}
	}
}
