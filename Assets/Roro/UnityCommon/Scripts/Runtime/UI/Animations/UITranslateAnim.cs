using System;
using Sirenix.OdinInspector;
using UnityCommon.Modules;
using UnityEngine;

namespace UnityCommon.Runtime.UI.Animations
{
	public class UITranslateAnim : UIAnimation
	{
		[SerializeField]
		private AnimationCurve curve = null;


		[SerializeField]
		private Vector3 offset = default;

		[SerializeField]
		private bool initiallyVisible = false;

		[SerializeField]
		[HideInInspector]
		private Vector3 inPos = default;

		[SerializeField, HideInInspector]
		private RectTransform _t = null;

		private Animation<Vector3> anim = null;


		private Conditional cond;

		private void OnValidate()
		{
			if (Application.isPlaying == false)
			{
				_t = GetComponent<RectTransform>();
				inPos = _t.anchoredPosition;
			}
		}


		private void Awake()
		{
			if (!initiallyVisible)
			{
				_t.anchoredPosition = inPos + offset;
			}
		}


		public void Cancel()
		{
			cond?.Cancel();
		}

		[Button]

		public override void FadeIn(Action onComplete = null)
		{
			if (delay.Value < 0)
			{
				if (anim != null)
					anim.Stop();

				anim = new Animation<Vector3>(val => _t.anchoredPosition = val)
				       .From(_t.anchoredPosition).To(inPos)
				       .For(duration)
				       .With(new Interpolator(curve)).OnCompleted(() => onComplete?.Invoke());

				if (useUnscaledTime)
					anim.UnscaledTime();

				anim.Start();
			}
			else
			{
				cond?.Cancel();
				cond = Conditional.Wait(delay.Value).Do(() =>
				{
					if (anim != null)
						anim.Stop();

					anim = new Animation<Vector3>(val => _t.anchoredPosition = val)
					       .From(_t.anchoredPosition).To(inPos)
					       .For(duration)
					       .With(new Interpolator(curve)).OnCompleted(() => onComplete?.Invoke());

					if (useUnscaledTime)
					{
						anim.UnscaledTime();
					}

					anim.Start();
				}).OnComplete(() =>
				{
					cond = null;
				});
			}
		}

		[Button]
		public override void FadeOut(Action onComplete = null)
		{
			cond?.Cancel();

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.anchoredPosition = val)
			       .From(_t.anchoredPosition).To(inPos + offset)
			       .For(duration)
			       .With(new Interpolator(curve)).OnCompleted(() => onComplete?.Invoke());

			anim.Start();

			if (useUnscaledTime)
			{
				anim.UnscaledTime();
			}

			anim.Start();
		}

	}
}
