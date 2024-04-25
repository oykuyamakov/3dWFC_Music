using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Roro.UnityCommon.Scripts.Runtime.Extensions
{
	public static class Helpers
	{
		public static int RoundTo5(this int x)
		{
			if (x < 0)
				return x;

			int r = x % 5;
			return r >= 3 ? x + (5 - r) : x - r;
		}

		public static int RoundTo5(this float x)
		{
			return RoundTo5((int) x);
		}

		public static Vector2 GetPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
		{
			Canvas.ForceUpdateCanvases();
			Vector2 viewportLocalPosition = instance.viewport.localPosition;
			Vector2 childLocalPosition = child.localPosition;
			Vector2 result = new Vector2(
				0 - (viewportLocalPosition.x + childLocalPosition.x),
				0 - (viewportLocalPosition.y + childLocalPosition.y)
			);
			return result;
		}

		public static void SetColor(this Image image, Color color)
		{
			var col = image.color;
			var dr = col.r - color.r;
			var dg = col.g - color.g;
			var db = col.b - color.b;
			var da = col.a - color.a;
			if (dr * dr + dg * dg + db * db + da * da < 1e-4f)
				return;

			image.color = color;
		}
	}
}
