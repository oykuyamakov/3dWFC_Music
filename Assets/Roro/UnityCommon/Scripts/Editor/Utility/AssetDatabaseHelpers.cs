using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCommon.Editor.Utility
{
	public static class AssetDatabaseHelpers
	{
		public static string AssetPathToResourcePath(string path)
		{
			var resIndex = path.LastIndexOf("/Resources/", StringComparison.Ordinal);
			var resPath = path.Substring(resIndex + 11, path.Length - 11 - resIndex);
			resPath = Path.GetDirectoryName(resPath) + "/" + Path.GetFileNameWithoutExtension(resPath);
			return resPath;
		}

		public static T LoadOrCreateResourceSO<T>(string path) where T : ScriptableObject
		{
			var asset = Resources.Load<T>(path);
			if (!asset)
			{
				asset = ScriptableObject.CreateInstance<T>();
				AssetDatabaseHelpers.CreateAssetMkdir(asset, $"Assets/Resources/{path}.asset");
			}

			return asset;
		}

		public static void CreateAssetMkdir(UnityEngine.Object asset, string path)
		{
			var fullPath = Path.GetFullPath(path);
			var fullDir = Path.GetDirectoryName(fullPath);

			if (!Directory.Exists(fullDir))
			{
				Directory.CreateDirectory(fullDir);
			}

			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();
		}

		public static List<T> LoadAll<T>() where T : UnityEngine.Object
		{
			return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
			                    .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
			                    .ToList();
		}

		public static void Rename(UnityEngine.Object item, string newName)
		{
			var path = AssetDatabase.GetAssetPath(item);
			var dir = Path.GetDirectoryName(path);
			var name = Path.GetExtension(path);

			if (string.Equals(name, newName, StringComparison.Ordinal))
			{
				return;
			}

			var extension = Path.GetExtension(path);

			var newPath = Path.Combine(dir, newName + extension);

			if (string.CompareOrdinal(path, newPath) == 0)
			{
				return;
			}

			var uniquePath = AssetDatabase.GenerateUniqueAssetPath(newPath);

			string result = null;
			if (!string.IsNullOrEmpty(result = AssetDatabase.MoveAsset(path, newPath)))
			{
				return;
			}
		}
	}
}
