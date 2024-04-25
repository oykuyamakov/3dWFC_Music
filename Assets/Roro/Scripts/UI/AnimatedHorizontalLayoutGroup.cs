using System;
using System.Collections.Generic;
using UnityCommon.Runtime.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class AnimatedHorizontalLayoutGroup : LayoutGroup
	{
		public float Spacing = 6f;

		public float Sharpness = 6f;

		private RectTransform m_Self;

		private float m_Width;
		public override float minWidth       => m_Width;
		public override float preferredWidth => m_Width;

		private bool m_IsDirty = false;

		protected override void Awake()
		{
			base.Awake();
			m_Self = GetComponent<RectTransform>();
		}

		public override void CalculateLayoutInputHorizontal()
		{
			// RefreshTargets();
		}

		public override void CalculateLayoutInputVertical()
		{
			// RefreshTargets();
		}

		public override void SetLayoutHorizontal()
		{
		}

		public override void SetLayoutVertical()
		{
		}

		public void Refresh()
		{
			// Debug.Log("Refreshing orders layout");
			const float Threshold = 0.5f;
			var tr = m_Self;
			var height = tr.rect.height;
			float currentPos = padding.left;
			float percent = Time.deltaTime * Sharpness;
			m_IsDirty = false;
			for (var i = 0; i < tr.childCount; i++)
			{
				var child = (RectTransform) tr.GetChild(i);

				if (!child.gameObject.activeSelf)
				{
					continue;
				}

				var ancPos = child.anchoredPosition;
				var oldPos = ancPos.x;

				if (Mathf.Abs(oldPos - currentPos) > Threshold)
				{
					ancPos.x = Mathf.Lerp(ancPos.x, currentPos, percent);
					child.anchoredPosition = ancPos;
					m_IsDirty = true;
				}

				var sizeDelta = child.sizeDelta;
				sizeDelta.y = height;
				currentPos += child.localScale.x * sizeDelta.x + Spacing;
				child.sizeDelta = sizeDelta;
			}

			m_Width = currentPos - Spacing + padding.right;
			m_Self.sizeDelta = m_Self.sizeDelta.WithX(m_Width);
		}


		private void Update()
		{
			if (!m_IsDirty)
				return;
			Refresh();
		}

		public void Dirty()
		{
			m_IsDirty = true;
		}
	}
}
