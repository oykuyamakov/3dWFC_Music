#if HAS_EDITOR_COROUTINES

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR


/*
 * Add 1242x2688 and 1242x2208 fixed resolutions to game view resolution presets
 * Shortcut :  Alt+Shift+S 
 * Saves in {Project Path}/Screenshots
 *
 */


namespace ScreenshotPro
{
	[InitializeOnLoad]
	public class Screenshot : EditorWindow
	{
		private const string KEY = "SSPRO_RESOLUTIONS";

		static Screenshot()
		{
			// if (EditorPrefs.HasKey(KEY) == false)
			// {
			// }
		}

		[MenuItem("Screenshot/Settings")]
		public static void OpenSettings()
		{
			var window = ScriptableObject.CreateInstance<Screenshot>();

			window.titleContent = new GUIContent("Screenshot Settings");

			var size = new Vector2(300, 100);
			window.maxSize = size;
			window.minSize = size;

			window.ShowUtility();
		}

		private void OnGUI()
		{
			var s = EditorPrefs.GetString(KEY);

			GUILayout.Space(12);

			var l = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = l / 2;

			EditorGUI.BeginChangeCheck();

			s = EditorGUILayout.TextField(new GUIContent("Resolutions"), s);

			if (EditorGUI.EndChangeCheck())
			{
				EditorPrefs.SetString(KEY, s);
			}

			EditorGUIUtility.labelWidth = l;

			GUILayout.Space(12);

			GUILayout.Label("Example: '1440x2960, 720x1280'");
		}

		private static bool ExtractResolutions(string s, out List<Vector2Int> resolutions)
		{
			var s0 = s.Split(',').Select(nontrimmed => nontrimmed.Trim());

			List<Vector2Int> list = new List<Vector2Int>();

			foreach (var s1 in s0)
			{
				var s2 = s1.Split('x');
				if (s2.Length != 2 || !int.TryParse(s2[0], out int w) || !int.TryParse(s2[1], out int h))
				{
					resolutions = null;
					return false;
				}

				var v = new Vector2Int(w, h);
				list.Add(v);
			}

			resolutions = list;
			return true;
		}


		[MenuItem("Screenshot/Take #&s")]
		public static void TakeAndSave()
		{
			ExtractResolutions(EditorPrefs.GetString(KEY), out var resos);
			EditorCoroutineUtility.StartCoroutineOwnerless(TakeAndSaveAll(resos));
		}

		private static IEnumerator TakeAndSaveAll(List<Vector2Int> resos)
		{
			var dir = Application.dataPath.Replace("Assets", "Screenshots");
			if (Directory.Exists(dir) == false)
			{
				Directory.CreateDirectory(dir);
			}

			var wasPaused = EditorApplication.isPaused;


			var ts = Time.timeScale;
			Time.timeScale = 0f;

			foreach (var reso in resos)
			{
				yield return TakeAndSave_(reso.x, reso.y);
			}

			Time.timeScale = ts;
		}

		private static IEnumerator TakeAndSave_(int w, int h)
		{
			GameViewSizeGroupType type;

			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
				type = GameViewSizeGroupType.Android;
			else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
				type = GameViewSizeGroupType.iOS;
			else
				throw new Exception(
					$"Target platform must be Android or iOS, it is {EditorUserBuildSettings.activeBuildTarget}");


			int size = GameViewUtils.FindSize(type, w, h);
			if (size < 0)
			{
				GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.FixedResolution, type,
				                            w, h, $"SS_{w}x{h}");

				Debug.Log("Add size " + $"SS_{w}x{h}");

				EditorApplication.Step();

				size = GameViewUtils.FindSize(type, w, h);
			}

			if (size < 0)
				throw new Exception("Cannot find size (1)");

			GameViewUtils.SetSize(size);

			yield return null;
			AssetDatabase.Refresh();


			var path = Application.dataPath.Replace("Assets", "") +
			           $"Screenshots/{w}x{h}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}" + ".png";
			ScreenCapture.CaptureScreenshot(path, 1);

			yield return null;
			AssetDatabase.Refresh();


			/*
			yield return null;

			w = _w;
			h = _h;
			Debug.Log(size);
			size = GameViewUtils.FindSize(type, w, h);
			GameViewUtils.SetSize(size);
			*/
		}
	}
}


#endif
#endif