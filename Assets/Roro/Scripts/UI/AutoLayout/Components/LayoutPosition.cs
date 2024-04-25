using Roro.Scripts.UI.AutoLayout;
using UnityEngine;

namespace UI.Components
{
	public class LayoutPosition : LayoutConstraint
	{
		public Vector2 SelfAnchor = Vector2.one * 0.5f;
		public ReferencePoint Point;

		public override void Apply()
		{
			if (string.IsNullOrEmpty(Point.Id))
				return;

			if (!LayoutEngine.TryGetPosition(Point, out var pos))
			{
				Debug.Log("ReferencePoint id not found: " + Point.Id, this);
				return;
			}

			var pivot = RectTransform.pivot;
			RectTransform.pivot = SelfAnchor;
			RectTransform.position = pos;
		}
	}
}
