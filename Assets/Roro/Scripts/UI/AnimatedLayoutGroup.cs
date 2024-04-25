using System.Collections.Generic;
using UnityCommon.Modules;
using UnityCommon.Runtime.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[ExecuteAlways]
	public class AnimatedLayoutGroup : LayoutGroup
	{
		[SerializeField]
		private float m_Spacing = 4f;

		[SerializeField]
		private float m_Sharpness = 10f;

		private Dictionary<RectTransform, float> m_Positions;

		private List<RectTransform> m_Children;
		private RectTransform m_Self;

		private bool m_ShouldUpdate = false;

		private Canvas m_Canvas;
		private bool m_CanvasWasEnabled;


		private float m_PreferredWidth = 1f;

		public override float preferredWidth
		{
			get => m_PreferredWidth;
		}

		protected override void Awake()
		{
			m_Self = GetComponent<RectTransform>();

			m_Canvas = GetComponentInParent<Canvas>();

			m_Positions = new Dictionary<RectTransform, float>();

			m_Children = new List<RectTransform>();
		}


		public override void SetLayoutHorizontal()
		{
			Refresh();
		}

		public override void SetLayoutVertical()
		{
			Refresh();
		}

		// protected override void OnTransformChildrenChanged()
		// {
		// 	Refresh();
		// }
		//
		// protected override void OnTransformParentChanged()
		// {
		// 	Refresh();
		// }
		//
		// protected override void OnCanvasHierarchyChanged()
		// {
		// 	Refresh();
		// }

		// public override void CalculateLayoutInputHorizontal()
		// {
		// 	Refresh();
		// }

		public override void CalculateLayoutInputVertical()
		{
			Refresh();
		}

		// protected override void OnRectTransformDimensionsChange()
		// {
		// 	Refresh();
		// }


		private void Refresh()
		{
			if (m_Children == null)
				m_Children = new List<RectTransform>();

			m_Children.Clear();
			foreach (RectTransform child in transform)
			{
				m_Children.Add(child);
				child.anchorMin = Vector2.zero;
				child.anchorMax = Vector2.zero;
				child.pivot = Vector2.zero;
			}

			// CalculateTargetPositions();

			// m_ShouldUpdate = true;
		}

		// private void CalculateTargetPositions()
		// {
		// 	if (m_Positions == null)
		// 		m_Positions = new Dictionary<RectTransform, float>();
		//
		// 	m_Positions.Clear();
		//
		// 	var currentPosition = (float) padding.left;
		// 	for (var i = 0; i < m_Children.Count; i++)
		// 	{
		// 		var rt = m_Children[i];
		// 		rt.anchorMin = Vector2.zero;
		// 		rt.anchorMax = Vector2.zero;
		// 		rt.pivot = Vector2.zero;
		// 		m_Positions[rt] = currentPosition;
		// 		var minWidth = rt.sizeDelta.x * rt.localScale.x; //LayoutUtility.GetMinWidth(rt);
		// 		currentPosition += minWidth + m_Spacing;
		// 	}
		//
		// 	// Self sizing
		// 	// var size = currentPosition - m_Spacing;
		// 	// m_Self.CenterPivot();
		// 	// var sd = m_Self.sizeDelta;
		// 	// m_Self.sizeDelta = new Vector2(size, sd.y);
		// }


		private void Update()
		{
			// if (!m_ShouldUpdate)
			// 	return;


			var wasEnabled = m_Canvas.enabled;

			if (m_CanvasWasEnabled != wasEnabled)
			{
				Refresh();
#if UNITY_EDITOR
				if (Application.isPlaying)
				{
#endif
					this.enabled = false;
					Conditional.WaitFrames(4).Do(() =>
					{
						this.enabled = true;
						Refresh();
					});

#if UNITY_EDITOR
				}
#endif
			}

			var currentPos = (float) padding.left;
			float t = Time.deltaTime * m_Sharpness;
			bool isDirty = false;
			for (var i = 0; i < m_Children.Count; i++)
			{
				var child = m_Children[i];

				child.anchorMin = Vector2.zero;
				child.anchorMax = Vector2.zero;
				child.pivot = Vector2.zero;

				child.sizeDelta = child.sizeDelta.WithY(m_Self.rect.height);

				var pos = child.anchoredPosition;
				var oldPos = pos.x;
				var newPos = currentPos;


				currentPos += child.sizeDelta.x * child.localScale.x + m_Spacing;

				if (Mathf.Abs(oldPos - newPos) < 1e-3f)
				{
					continue;
				}

				isDirty = true;
				pos.x = Mathf.Lerp(oldPos, newPos, t);

				m_Canvas.enabled = false;

				child.anchorMin = Vector2.zero;
				child.anchorMax = Vector2.zero;
				child.pivot = Vector2.zero;
				child.anchoredPosition = pos;
			}

			if (isDirty)
			{
				m_PreferredWidth = currentPos - m_Spacing;
				// m_Self.sizeDelta = new Vector2(, m_Self.sizeDelta.y);
			}

			m_Canvas.enabled = wasEnabled;

			m_CanvasWasEnabled = wasEnabled;
		}
	}
}
