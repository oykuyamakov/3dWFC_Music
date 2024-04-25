using Roro.Scripts.UI.AutoLayout;
using Unity.Mathematics;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace UI.Components
{
	public class LayoutFitBetween : LayoutConstraint
	{
		private static readonly Vector3[] Corners = new Vector3[4];

		public Vector2 SelfMinAnchor = Vector2.zero;
		public Vector2 SelfMaxAnchor = Vector2.one;

		public ReferencePoint Point1;
		public ReferencePoint Point2;

		public LayoutOrientation Orientation = LayoutOrientation.Vertical;

		public float MaxHeight = 2000;

		public override void Apply()
		{
			if (!LayoutEngine.TryGetPosition(Point1, out var pos1))
			{
				return;
			}

			if (!LayoutEngine.TryGetPosition(Point2, out var pos2))
			{
				return;
			}

			if (Orientation == LayoutOrientation.Vertical)
			{
				ApplyVertical(pos1, pos2);
			}
			else if (Orientation == LayoutOrientation.Horizontal)
			{
				ApplyHorizontal(pos1, pos2);
			}
			else
			{
				ApplyVertical(pos1, pos2);
				ApplyHorizontal(pos1, pos2);
			}
		}

		private void ApplyVertical(Vector3 pos1, Vector3 pos2)
		{
			var parent = RectTransform.parent.GetComponent<RectTransform>();

			parent.GetWorldCorners(Corners);

			var rect = new Rect(
				Corners[0].x,
				Corners[0].y,
				Corners[2].x - Corners[0].x,
				Corners[2].y - Corners[0].y);

			var p1 = (Vector2) ((new float2(pos1.x, pos1.y) - (float2) rect.min) / (float2) rect.size);
			var p2 = (Vector2) ((new float2(pos2.x, pos2.y) - (float2) rect.min) / (float2) rect.size);

			var yMin = Mathf.Min(p1.y, p2.y) - SelfMinAnchor.y;
			var yMax = Mathf.Max(p1.y, p2.y) + (1f - SelfMaxAnchor.y);

			RectTransform.anchorMin = RectTransform.anchorMin.WithY(yMin);
			RectTransform.anchorMax = RectTransform.anchorMax.WithY(yMax);
			RectTransform.offsetMin = RectTransform.offsetMin.WithY(0f);
			RectTransform.offsetMax = RectTransform.offsetMax.WithY(0f);

			var dy = RectTransform.rect.height - MaxHeight;
			if (dy > 0)
			{
				RectTransform.sizeDelta = RectTransform.sizeDelta.WithY(-dy);
			}
		}

		private void ApplyHorizontal(Vector3 pos1, Vector3 pos2)
		{
		}
	}
}
