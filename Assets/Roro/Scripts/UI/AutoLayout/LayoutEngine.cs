using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.Components;
using UnityCommon.Singletons;
using UnityEngine;

namespace Roro.Scripts.UI.AutoLayout
{
	[ExecuteAlways]
	[DefaultExecutionOrder(-8)]
	public class LayoutEngine : SingletonBehaviour<LayoutEngine>
	{
		private static Vector3[] Corners = new Vector3[4];


		public bool AlwaysUpdate = true;

		public int Iterations = 5;

		private int m_LastWidth;
		private int m_LastHeight;

		private bool m_IsDirty;

		private void Awake()
		{
			if (!SetupInstance())
			{
				return;
			}
		}

		private void Update()
		{
#if UNITY_EDITOR
			if (AlwaysUpdate)
			{
				RunInternal();
				return;
			}
#endif

			if (m_LastWidth != Screen.width || m_LastHeight != Screen.height)
			{
				m_LastWidth = Screen.width;
				m_LastHeight = Screen.height;

				RunInternal();
			}

			if (m_IsDirty)
			{
				m_IsDirty = false;
				RunInternal();
			}
		}

		public static Dictionary<string, LayoutObject> LayoutObjectsById = new Dictionary<string, LayoutObject>();
		public static List<LayoutConstraint> LayoutConstraints = new List<LayoutConstraint>();

		public static void Run()
		{
			if (!HasInstance())
				return;

			Instance.m_IsDirty = true;
		}

		private static void RunInternal()
		{
			if (!HasInstance())
				return;

			for (var i = 0; i < Instance.Iterations; i++)
			{
				for (var j = 0; j < LayoutConstraints.Count; j++)
				{
					var layoutConstraint = LayoutConstraints[j];
					layoutConstraint.Apply();
				}
			}
		}

		public static void OnLayoutConstraintCreated(LayoutConstraint constraint)
		{
			if (!LayoutConstraints.Contains(constraint))
			{
				LayoutConstraints.Add(constraint);
				Run();
			}
		}

		public static void OnLayoutConstraintDestroyed(LayoutConstraint constraint)
		{
			LayoutConstraints.Remove(constraint);
			Run();
		}

		public static void OnLayoutObjectCreated(LayoutObject obj)
		{
			foreach (var kv in LayoutObjectsById.Where(kv => kv.Value == obj).ToList())
			{
				LayoutObjectsById.Remove(kv.Key);
			}

			if (!LayoutObjectsById.ContainsKey(obj.Id))
			{
				LayoutObjectsById.Add(obj.Id, obj);
			}
			else
			{
				Debug.LogException(
					new Exception($"LayoutObject with the same id already added. {obj.Id}, {obj.ToString()}"));
			}

			Run();
		}

		public static void OnLayoutObjectDestroyed(LayoutObject obj)
		{
			foreach (var kv in LayoutObjectsById.Where(kv => kv.Value == obj).ToList())
			{
				LayoutObjectsById.Remove(kv.Key);
			}

			Run();
		}

		public static Vector3 GetPosition(ReferencePoint point)
		{
			if (!LayoutObjectsById.ContainsKey(point.Id))
			{
				Debug.Log($"LayoutObject with id {point.Id} does not exist");
				return Vector3.zero;
			}

			var obj = LayoutObjectsById[point.Id];
			obj.RectTransform.GetWorldCorners(Corners);

			var rect = new Rect(
				Corners[0].x,
				Corners[0].y,
				Corners[2].x - Corners[0].x,
				Corners[2].y - Corners[0].y);

			var pos = rect.min + Vector2.Scale(rect.max - rect.min, point.Anchor);

			// var oldPivot = obj.RectTransform.pivot;
			// obj.RectTransform.SetPivot(point.Anchor);
			// var pos = obj.RectTransform.position;
			// obj.RectTransform.SetPivot(oldPivot);
			return pos;
		}

		public static bool TryGetPosition(ReferencePoint point, out Vector3 pos)
		{
			if (string.IsNullOrEmpty(point.Id) || !LayoutObjectsById.ContainsKey(point.Id))
			{
				pos = default;
				return false;
			}

			pos = GetPosition(point);

			if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z)
			    || float.IsInfinity(pos.x) || float.IsInfinity(pos.y) || float.IsInfinity(pos.z))
			{
				return false;
			}

			return true;
		}
	}
}
