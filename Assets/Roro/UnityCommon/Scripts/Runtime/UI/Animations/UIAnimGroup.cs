using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityCommon.Modules;
using UnityEngine;

namespace UnityCommon.Runtime.UI.Animations
{
	public class UIAnimGroup : MonoBehaviour
	{
		[SerializeField]
		private bool onStart = false;

		[SerializeField]
		private bool includeChildrenGroups = false;

		public List<UIAnimation> animations;

		[HideInInspector]
		[SerializeField]
		private Canvas canvas;

		[HideInInspector]
		public List<UIAnimGroup> childrenGroups;

		private float duration = 0f;

#if UNITY_EDITOR
		private void OnValidate()
		{
			animations = new List<UIAnimation>();

			CacheChildren(transform);

			canvas = GetComponent<Canvas>();

			childrenGroups = GetComponentsInChildren<UIAnimGroup>().ToList();
			childrenGroups.Remove(this);
		}

		private void CacheChildren(Transform t)
		{
			var anim = t.GetComponent<UIAnimation>();
			if (anim)
			{
				animations.Add(anim);
				duration = Mathf.Max(duration, anim.duration);
			}

			for (int i = 0; i < t.childCount; i++)
			{
				var child = t.GetChild(i);

				if (!child.GetComponent<UIAnimGroup>())
				{
					CacheChildren(child);
				}
			}
		}

#endif

		private void OnEnable()
		{
			if (onStart)
			{
				FadeIn();
			}
		}

		public void Fade(bool e)
		{
			if (e)
			{
				FadeIn();
			}
			else
			{
				FadeOut();
			}
		}

		[Button]
		public void FadeIn()
		{
			if (canvas)
				canvas.enabled = true;

			for (int i = 0; i < animations.Count; i++)
			{
				animations[i].FadeIn();
			}

			if (includeChildrenGroups)
			{
				for (var i = 0; i < childrenGroups.Count; i++)
				{
					childrenGroups[i].FadeIn();
				}
			}
		}

		[Button]
		public void FadeOut()
		{
			for (int i = 0; i < animations.Count; i++)
			{
				animations[i].FadeOut();
			}

			if (canvas)
			{
				Conditional.Wait(duration).Do(() =>
				{
					if (canvas)
						canvas.enabled = false;
				});
			}

			if (includeChildrenGroups)
			{
				for (var i = 0; i < childrenGroups.Count; i++)
				{
					childrenGroups[i].FadeOut();
				}
			}
		}
	}
}
