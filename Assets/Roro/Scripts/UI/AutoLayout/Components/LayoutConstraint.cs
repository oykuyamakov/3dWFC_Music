using System;
using Roro.Scripts.UI.AutoLayout;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
	[ExecuteAlways]
	public abstract class LayoutConstraint : MonoBehaviour
	{
		[HideInInspector]
		public RectTransform RectTransform;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (Application.isPlaying)
				return;
			
			LayoutEngine.Run();
		}
		
#endif

		protected virtual void OnEnable()
		{
			RectTransform = GetComponent<RectTransform>();
			LayoutEngine.OnLayoutConstraintCreated(this);
		}

		protected void OnDisable()
		{
			LayoutEngine.OnLayoutConstraintDestroyed(this);
		}

		public abstract void Apply();
	}
}
