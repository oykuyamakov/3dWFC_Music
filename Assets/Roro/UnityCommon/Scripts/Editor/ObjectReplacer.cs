using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCommon.Editor
{
	public class ObjectReplacer : EditorWindow
	{
		public static ObjectReplacer instance;

		[MenuItem("Tools/Replace GameObject")]
		private static void ShowWindow()
		{
			var window = GetWindow<ObjectReplacer>();
			window.titleContent = new GUIContent("GameObject Replacer");
			window.Show();

			instance = window;
		}


		[MenuItem("GameObject/Replace...", false, 0)]
		public static void ReplaceExplicit()
		{
			ShowWindow();

			instance.objectToBeReplaced = Selection.activeGameObject;
		}

		[MenuItem("GameObject/Replace...", true)]
		public static bool ReplaceExplicitValidation()
		{
			// if (Selection.gameObjects.Length > 1)
			// 	return false;

			var obj = Selection.activeGameObject;
			return obj != null && AssetDatabase.IsMainAsset(obj) == false;
		}

		private GameObject objectToBeReplaced;
		private GameObject replacement;


		private GameObject Replace(GameObject objectToBeReplaced, GameObject replacement)
		{
			var t = objectToBeReplaced.transform;

			var name = objectToBeReplaced.name;
			var layer = objectToBeReplaced.layer;
			var tag = objectToBeReplaced.tag;
			var parent = objectToBeReplaced.transform.parent;
			int sibling = objectToBeReplaced.transform.GetSiblingIndex();

			var newObj = PrefabUtility.InstantiatePrefab(replacement) as GameObject;

			Undo.RegisterCreatedObjectUndo(newObj, "Replacement");

			var newT = newObj.transform;
			newT.position = t.position;
			newT.rotation = t.rotation;
			newObj.name = name;
			newObj.layer = layer;
			newObj.tag = tag;
			newObj.transform.parent = parent;
			newT.localScale = t.localScale;

			newT.SetSiblingIndex(sibling);


			Undo.DestroyObjectImmediate(objectToBeReplaced);


			return newObj;
		}

		private void OnGUI()
		{
			replacement =
				EditorGUILayout.ObjectField("Replacement", replacement, typeof(GameObject), true) as GameObject;

			objectToBeReplaced =
				EditorGUILayout.ObjectField("Object to be replaced", objectToBeReplaced, typeof(GameObject), true) as
					GameObject;

			GUILayout.Space(30);

			if (GUILayout.Button("Replace"))
			{
				bool sure = EditorUtility.DisplayDialog("GameObject Replacer",
				                                        $"This will replace {Selection.gameObjects.Length} GameObjects. Are you sure?",
				                                        "Yes", "No");
				if (sure)
				{
					List<Object> gos = new List<Object>();

					Undo.SetCurrentGroupName("Object Replacement");

					var obj = Replace(objectToBeReplaced, replacement);
					gos.Add(obj);

					foreach (var gameObject in Selection.gameObjects.Where(g => g.scene.IsValid()))
					{
						//Debug.Log(gameObject.name);
						obj = Replace(gameObject, replacement);
						gos.Add(obj);
					}

					Selection.objects = gos.ToArray();

					this.Close();
				}
			}
		}
	}
}
