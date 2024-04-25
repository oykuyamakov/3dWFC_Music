using System.Collections.Generic;
using DG.Tweening;
using UnityCommon.Modules;
using UnityEngine;

namespace UI
{
	[System.Serializable]
	public class ToggleMenuItem
	{
		public RectTransform transform;
		public Vector2 targetPosition;
	}

	public class ToggleMenu : MonoBehaviour
	{
		public RectTransform gear;

		public RectTransform bg;

		public List<ToggleMenuItem> items;

		public float height0;
		public float height1;

		public float duration = 0.3f;

		private bool isVisible = false;

		private Conditional m_CancelConditional;

		private void Start()
		{
			isVisible = false;

			foreach (var item in items)
			{
				item.transform.localScale = Vector3.zero;
				item.transform.anchoredPosition = gear.anchoredPosition;
			}

			var size = bg.sizeDelta;
			size.y = height0;
			bg.sizeDelta = size;
		}

		public void OnClick()
		{
			ToggleState();
		}

		private void ToggleState()
		{
			var s0 = isVisible ? Vector3.one : Vector3.zero;
			var s1 = isVisible ? Vector3.zero : Vector3.one;

			var g1 = isVisible ? Vector3.zero : Vector3.forward * 180f;

			var gearPos = gear.anchoredPosition;

			foreach (var item in items)
			{
				item.transform.DOKill();

				var p1 = isVisible ? gearPos : item.targetPosition;
				item.transform.anchoredPosition = isVisible ? item.targetPosition : gearPos;
				item.transform.DOAnchorPos(p1, duration).SetEase(Ease.OutQuad);

				item.transform.localScale = s0;
				item.transform.DOScale(s1, duration).SetEase(Ease.OutQuad);
			}

			gear.DOKill();
			gear.DORotate(g1, duration);

			var size = bg.sizeDelta;
			size.y = isVisible ? height0 : height1;

			bg.DOKill();
			bg.DOSizeDelta(size, duration).SetEase(Ease.OutQuad);

			isVisible = !isVisible;

			m_CancelConditional?.Cancel();
			if (isVisible)
			{
				m_CancelConditional = Conditional.Wait(4f).Do(() =>
				{
					if (isVisible)
						ToggleState();
				});
			}
		}
	}
}
