using System;
using UnityCommon.Modules;
using UnityCommon.Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Roro.Scripts.UI.AutoLayout
{
	[ExecuteAlways]
	[RequireComponent(typeof(Canvas))]
	public class SafeArea : MonoBehaviour
	{
		public static UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();

		private static bool screenChangeVarsInitialized = false;
		private static ScreenOrientation lastOrientation = ScreenOrientation.LandscapeLeft;
		private static Vector2 lastResolution = Vector2.zero;
		private static Rect lastSafeArea = Rect.zero;

		[SerializeField]
		private RectTransform m_SafeAreaTransform;

		[SerializeField]
		private bool m_AlwaysUpdate = true;

		[SerializeField]
		private bool m_SimulateBannerInEditor = true;

		private TimedAction m_Timer;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (!m_SafeAreaTransform)
			{
				m_SafeAreaTransform = transform.Find("SafeArea")?.GetComponent<RectTransform>();
			}
		}
#endif

		private Canvas m_Canvas;
		private RectTransform m_RectTransform;

		void Awake()
		{


			m_Canvas = GetComponent<Canvas>();
			m_RectTransform = GetComponent<RectTransform>();

			if (!screenChangeVarsInitialized)
			{
				lastOrientation = Screen.orientation;
				lastResolution.x = Screen.width;
				lastResolution.y = Screen.height;
				lastSafeArea = Screen.safeArea;

				screenChangeVarsInitialized = true;
			}

			if (Application.isPlaying)
			{
				// Debug.Log("Safe area: Listening to banner state changes");
				//
				// var bannerVisible = Var.Get<BoolVariable>("BannerVisible");
				// var bannerEnabled = Var.Get<BoolVariable>("BannerEnabled");
				//
				// bannerVisible.OnModified.AddListener(b => ApplySafeArea());
				// bannerEnabled.OnModified.AddListener(b => ApplySafeArea());
			}


			ApplySafeArea();

			m_Timer = new TimedAction(CheckForChanges, 0.5f, 1f);
		}

		void Update()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				ApplySafeArea();
			}
#endif

			if (m_AlwaysUpdate)
			{
				ApplySafeArea();
				return;
			}

#if UNITY_EDITOR
			if (Application.isPlaying)
#endif
				m_Timer.Update(Time.deltaTime);
		}

		private void CheckForChanges()
		{
			if (Application.isMobilePlatform && Screen.orientation != lastOrientation)
			{
				ApplySafeArea();
				lastOrientation = Screen.orientation;
				lastResolution.x = Screen.width;
				lastResolution.y = Screen.height;
			}

			if (Screen.safeArea != lastSafeArea)
			{
				ApplySafeArea();
				lastSafeArea = Screen.safeArea;
			}

			const float TOLERANCE = 1f;
			if (Math.Abs(Screen.width - lastResolution.x) > TOLERANCE ||
			    Math.Abs(Screen.height - lastResolution.y) > TOLERANCE)
			{
				ApplySafeArea();
				lastResolution.x = Screen.width;
				lastResolution.y = Screen.height;
			}
		}

		public void ApplySafeArea()
		{
			if (m_SafeAreaTransform == null)
				return;

			var safeArea = Screen.safeArea;

			bool isEditor = false;

#if UNITY_EDITOR
			isEditor = !Application.isPlaying;
#endif

			// var banner = (isEditor && m_SimulateBannerInEditor) ||
			//              (Application.isPlaying && Var.Get<BoolVariable>("BannerVisible").Value &&
			//               Var.Get<BoolVariable>("BannerEnabled").Value);
			

			// var anchorMin = banner
			// 	? safeArea.position.WithY(safeArea.position.y + BannerUtility.CalculateBannerHeight())
			// 	: safeArea.position;
			var anchorMin = safeArea.position;
			var anchorMax = safeArea.position + safeArea.size;
			var pixelRect = m_Canvas.pixelRect;
			anchorMin.x /= pixelRect.width;
			anchorMin.y /= pixelRect.height;
			anchorMax.x /= pixelRect.width;
			anchorMax.y /= pixelRect.height;

			m_SafeAreaTransform.anchorMin = anchorMin;
			m_SafeAreaTransform.anchorMax = anchorMax;
			m_SafeAreaTransform.offsetMin = Vector2.zero;
			m_SafeAreaTransform.offsetMax = Vector2.zero;

			if (Application.isPlaying)
			{
				Conditional.WaitFrames(2).Do(() => { LayoutEngine.Run(); });
			}
			else
			{
				LayoutEngine.Run();
			}
		}
	}
}
