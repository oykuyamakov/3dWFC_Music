using System;
using DG.Tweening;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace Utility
{
	public static class DOTweenExtensions
	{
		public static Tweener DOMoveDynamic(this Transform tr, Transform target, float duration)
		{
			float t = 0f;
			var p0 = tr.position;
			return DOTween.To(() => t, val =>
			{
				t = val;
				tr.position = Vector3.Lerp(p0, target.position, t);
			}, 1f, duration);
		}


		public static Tweener DOSquashAndStretchAreaBased(this Transform tr, float targetScale,
		                                                  float duration, float frequency = 12,
		                                                  float disturbance = 0.3f,
		                                                  float threshold = 0.01f, float sharpness = 15f)
		{
			var s0 = tr.localScale;

			float k = -Mathf.Log(threshold) / duration;
			Func<float, float> e = time => Mathf.Exp(-k * time);

			float area0 = s0.x * s0.y;
			float targetArea = targetScale * targetScale;
			float f0 = targetScale * disturbance;
			var y = s0.y;

			float t = 0f;
			var tweener = DOTween.To(() => t, val =>
			                     {
				                     t = val;
				                     var eVal = e(t);
				                     float area = Mathf.Lerp(area0, targetArea, 1 - eVal);

				                     var targetY = Mathf.Cos(frequency * t) * f0 * eVal + targetScale;
				                     y = Mathf.Lerp(y, targetY, Time.deltaTime * sharpness);
				                     var x = area / y;

				                     tr.SetScaleXY(x, y);
			                     }, duration, duration).OnComplete(() => { tr.SetScaleXY(targetScale, targetScale); })
			                     .SetTarget(tr);
			return tweener;
		}

		public static Tweener DOSquashAndStretch(this Transform tr, float targetScale, float duration,
		                                         float frequency = 20, float disturbance = 0.5f,
		                                         float threshold = 1e-3f, float introSmoothing = 0.1f) =>
			DOSquashAndStretch(tr, Vector2.one * targetScale, duration, frequency, disturbance, threshold,
			                   introSmoothing);

		public static Tweener DOSquashAndStretch(this Transform tr, Vector2 targetScale,
		                                         float duration, float frequency = 20, float disturbance = 0.5f,
		                                         float threshold = 1e-3f, float introSmoothing = 0.1f)
		{
			var s0 = tr.localScale;

			float k = -Mathf.Log(threshold) / duration;
			Func<float, float> e = time => Mathf.Exp(-k * time);

			Vector2 f0 = targetScale * disturbance;
			var x = s0.x;
			var y = s0.y;
			var dx = targetScale.x - x;
			var dy = targetScale.y - y;
			bool yMax = dy > dx;

			var smoothTime = duration * introSmoothing;

			float t = 0f;
			var tweener = DOTween.To(() => t, val =>
			                     {
				                     t = val;
				                     var eVal = e(t);

				                     var sin = Mathf.Sin(frequency * t);
				                     var cos = Mathf.Cos(frequency * t + Mathf.PI / 2);

				                     float xm = yMax ? cos : sin;
				                     float ym = yMax ? sin : cos;

				                     var targetX = xm * f0.x * eVal +
				                                   targetScale.x;
				                     var targetY = ym * f0.y * eVal + targetScale.y;

				                     float a = Mathf.SmoothStep(0f, 1f, t / smoothTime);

				                     y = Mathf.Lerp(y, targetY, a);
				                     x = Mathf.Lerp(x, targetX, a);

				                     tr.SetScaleXY(x, y);
			                     }, duration, duration)
			                     .SetTarget(tr);
			return tweener;
		}
	}
}
