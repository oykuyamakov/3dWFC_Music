using System;
using UnityCommon.Modules;
using UnityEngine;

namespace UnityCommon.Runtime.UI.Animations
{

	public class ScaleAnim : UIAnimation
	{

		[SerializeField] private AnimationCurve curve = null;


		[SerializeField] private Vector3 outScale = default;

		[SerializeField] private bool initiallyVisible = false;

		[SerializeField]
		[HideInInspector]
		private Vector3 inScale = default;

		[SerializeField, HideInInspector]
		private Transform _t = null;

		private Animation<Vector3> anim;


		private void OnValidate()
		{
			if (Application.isPlaying == false)
			{
				_t = transform;
				inScale = _t.localScale;
			}
		}


		private void Awake()
		{

			if (!initiallyVisible)
			{
				_t.localScale = outScale;
			}

		}


		public override void FadeIn(Action onComplete = null)
		{

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.localScale = val)
				.From(outScale).To(inScale)
				.For(duration)
				.With(new Interpolator(curve)).OnCompleted(() => onComplete?.Invoke());

			if (useUnscaledTime)
			{
				anim.UnscaledTime();
			}
			
			anim.Start();

		}

		public override void FadeOut(Action onComplete = null)
		{

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.localScale = val)
				.From(inScale).To(outScale)
				.For(duration)
				.With(new Interpolator(curve)).OnCompleted(() => onComplete?.Invoke());

			if (useUnscaledTime)
			{
				anim.UnscaledTime();
			}

			anim.Start();
		}

		public void FadeOutAndDo(Action act)
		{

			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.localScale = val)
				.From(inScale).To(outScale)
				.For(duration)
				.With(new Interpolator(curve)).OnCompleted(() => act.Invoke());

			if (useUnscaledTime)
			{
				anim.UnscaledTime();
			}

			anim.Start();
		}
		public void FadeInAndDo(Action act)
		{
			if (anim != null)
				anim.Stop();

			anim = new Animation<Vector3>(val => _t.localScale = val)
				.From(outScale).To(inScale)
				.For(duration)
				.With(new Interpolator(curve)).OnCompleted(() => act.Invoke());

			if (useUnscaledTime)
			{
				anim.UnscaledTime();
			}

			anim.Start();
		}

	}

}
