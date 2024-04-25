using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCommon.Editor.Utility
{
	public static class MeshMerger
	{
		[MenuItem("GameObject/Merge Children Meshes", false, 0)]
		public static void MergeChildren()
		{
			var go = Selection.activeGameObject;
			var filters = go.GetComponentsInChildren<MeshFilter>(false);
			var yes = EditorUtility.DisplayDialog("Mesh Merger",
			                                      $"This will merge {filters.Length} meshes into a single mesh. Are you sure?",
			                                      "Yes", "No");

			if (!yes)
				return;

			MergeInternal(go, filters);
		}

		[MenuItem("GameObject/Merge Children Meshes", true)]
		public static bool MergeChildrenValidation()
		{
			var sel = Selection.gameObjects;
			if (sel.Length == 1 && sel.First().GetComponentsInChildren<MeshFilter>().Length > 1)
				return true;

			return false;
		}

		private static void MergeInternal(GameObject root, MeshFilter[] filters)
		{
			Undo.SetCurrentGroupName("Merge Meshes");

			try
			{
				foreach (var f in filters)
				{
					f.gameObject.SetActive(false);
				}

				var rootFilter = root.GetComponent<MeshFilter>();
				var rootRenderer = root.GetComponent<MeshRenderer>();

				if (!rootFilter)
					rootFilter = root.AddComponent<MeshFilter>();
				if (!rootRenderer)
					rootRenderer = root.AddComponent<MeshRenderer>();

				var rootT = root.transform;
				var rootW2L = rootT.worldToLocalMatrix;

				var ts = filters.Select(f => f.transform).ToArray();

				var material = filters.Select(f => f.GetComponent<MeshRenderer>()).FirstOrDefault(r => r != null)
				                      ?.sharedMaterial;

				var meshes = filters.Select(f => f.sharedMesh).ToArray();

				var combineInstances = new CombineInstance[filters.Length];

				for (var i = 0; i < filters.Length; i++)
				{
					var matrix = rootW2L * ts[i].localToWorldMatrix;
					combineInstances[i] = new CombineInstance
					                      {
						                      mesh = meshes[i], transform = matrix, subMeshIndex = 0
					                      };
				}

				var mesh = new Mesh();
				// mesh.indexFormat = IndexFormat.UInt32;
				mesh.CombineMeshes(combineInstances, true, true);

				rootFilter.sharedMesh = mesh;
				rootRenderer.sharedMaterial = material;

				var dir = "MergedMeshes";
				var dir_ = Application.dataPath + "/" + dir;

				if (Directory.Exists(dir_) == false)
					Directory.CreateDirectory(dir_);

				AssetDatabase.Refresh();

				var file = $"/{root.name}.asset";

				var path = "Assets/" + dir + file;
				path = AssetDatabase.GenerateUniqueAssetPath(path);
				AssetDatabase.CreateAsset(mesh, path);
				AssetDatabase.SaveAssets();

				root.SetActive(true);
			}
			catch (Exception e)
			{
				Undo.PerformUndo();
				throw e;
			}
		}
	}
}
