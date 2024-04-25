using System;
using UnityCommon.Modules;
using UnityCommon.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.Runtime.UI
{
	public class FadeInOut : SingletonBehaviour<FadeInOut>
	{
		private static readonly int HASH_TRANSITION = Shader.PropertyToID("_Transition");
		private static readonly int HASH_IN = Shader.PropertyToID("_In");

		private Canvas canvas;
		private Image image;

		private void Start()
		{
			DontDestroyOnLoad(gameObject);

			SetupCanvas();
		}

		private void SetupCanvas()
		{
			if (canvas != null)
				return;

			if ((canvas = gameObject.GetComponent<Canvas>()) != null)
			{
				// Assuming it's setup in the editor
				canvas.enabled = false;
				image = canvas.GetComponentInChildren<Image>();
				image.color = Color.clear;
				return;
			}

			canvas = gameObject.AddComponent<Canvas>();
			canvas.sortingOrder = 10000;
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;

			var scaler = gameObject.AddComponent<CanvasScaler>();
			scaler.referenceResolution = new Vector2(1080, 1920);
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

			canvas.enabled = true;

			var rootRect = canvas.GetComponent<RectTransform>();
			var imageObj = new GameObject("Image");
			imageObj.transform.parent = transform;
			imageObj.transform.localScale = Vector3.one;
			imageObj.transform.localRotation = Quaternion.identity;
			image = imageObj.AddComponent<Image>();
			image.color = Color.clear;
			var rect = image.GetComponent<RectTransform>();

			Conditional.WaitFrames(1).Do(() =>
			{
				rect.anchorMin = new Vector2(0, 0);
				rect.anchorMax = new Vector2(1, 1);
				rect.anchoredPosition = new Vector2(0.5f, 0.5f);
				rect.pivot = new Vector2(0.5f, 0.5f);
				rect.sizeDelta = Vector2.one;
			});
		}

		public void DoTransition(Action onOut)                 => DoTransition(onOut, 0.7f);
		public void DoTransition(Action onOut, float duration) => DoTransition(onOut, duration, Color.black);

		public void DoTransition(Action onOut, float duration, Color color, float durationOutPercent = 0.6f)
		{
			if (duration < 1e-2f)
			{
				onOut.Invoke();
				return;
			}

			if (canvas == null)
			{
				SetupCanvas();
			}

			var c0 = color;
			c0.a = 0f;

			canvas.enabled = true;
			image.material = null;
			image.color = color;

			float outDuration = duration * durationOutPercent;
			float inDuration = duration * (1f - durationOutPercent);

			var fadeIn = new Animation<Color>(val => image.color = val)
			             .From(color).To(c0)
			             .For(inDuration)
			             .With(Interpolator.Smooth())
			             .OnCompleted(() => { canvas.enabled = false; });


			var fadeOut = new Animation<Color>(val => image.color = val)
			              .From(c0).To(color)
			              .For(outDuration)
			              .With(Interpolator.Accelerate())
			              .OnCompleted(() =>
			              {
				              onOut?.Invoke();
				              Conditional.WaitFrames(3).Do(() => { fadeIn.Start(); });
			              })
			              .Start();
		}

		public void DoTransition(Action onOut, float duration, Color color, float durationOutPercent, Material mat)
		{
			if (canvas == null)
			{
				SetupCanvas();
			}

			mat.SetFloat(HASH_TRANSITION, 0f);

			var c0 = color;
			c0.a = 0f;


			image.material = mat;
			image.color = color;
			canvas.enabled = true;

			float outDuration = duration * durationOutPercent;
			float inDuration = duration * (1f - durationOutPercent);

			var fadeIn = new Animation<float>(val => mat.SetFloat(HASH_TRANSITION, val))
			             .From(1f).To(0f)
			             .For(inDuration)
			             .With(Interpolator.Decelerate())
			             .OnCompleted(() =>
			             {
				             canvas.enabled = false;
				             //Destroy(mat);
			             });

			mat.SetFloat(HASH_IN, 1f);

			var fadeOut = new Animation<float>(val => mat.SetFloat(HASH_TRANSITION, val))
			              .From(0f).To(1f)
			              .For(outDuration)
			              .With(Interpolator.Linear())
			              .OnCompleted(() =>
			              {
				              onOut?.Invoke();
				              mat.SetFloat(HASH_IN, -1f);
				              Conditional.WaitFrames(3).Do(() => { fadeIn.Start(); });
			              })
			              .Start();
		}
	}
}
