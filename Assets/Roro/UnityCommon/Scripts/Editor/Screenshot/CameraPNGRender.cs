using UnityEditor;
#if UNITY_EDITOR
using System.IO;
using UnityEngine;

#endif

[RequireComponent(typeof(Camera))]
public class CameraPNGRender : MonoBehaviour
{
#if UNITY_EDITOR

	
	public void RenderPNG(int width = 1024, int height = 1024, string path = "Assets/RenderTextures/Render")
	{
		var cam = GetComponent<Camera>();

		var rt = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
		rt.antiAliasing = 8;
		var oldActiveCam = cam.targetTexture;
		var oldClearFlags = cam.clearFlags;

		cam.targetTexture = rt;
		cam.clearFlags = CameraClearFlags.Nothing;

		cam.Render();
		
		var tex = new Texture2D(width, height, TextureFormat.ARGB32, false, false);
		var oldActive = RenderTexture.active;
		RenderTexture.active = rt;
		tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		tex.Apply();

		var bytes = tex.EncodeToPNG();
		path += ".png";
		path = AssetDatabase.GenerateUniqueAssetPath(path);
		path = Application.dataPath + path.Replace("Assets", "");

		var dir = Path.GetDirectoryName(path);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(dir);
		}

		File.WriteAllBytes(path, bytes);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		

		cam.targetTexture = oldActiveCam;
		cam.clearFlags = oldClearFlags;
		RenderTexture.active = oldActive;
		
		rt.Release();

	}


	[MenuItem("Assets/Textures/Bake RenderTexture To Asset", false, 0)]
	public static void BakeTexture2D()
	{
		var rt = Selection.activeObject as RenderTexture;

		var old = RenderTexture.active;
		RenderTexture.active = rt;

		var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBAFloat, false);
		tex.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
		tex.Apply();

		var path = AssetDatabase.GetAssetPath(rt).Replace("\\", "/");
		path = path.Substring(0, path.LastIndexOf("/")) + $"/{rt.name}_2D.asset";

		AssetDatabase.CreateAsset(tex, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		RenderTexture.active = old;
	}

	[MenuItem("Assets/Textures/Bake RenderTexture To Asset", true)]
	public static bool BakeTexture2DValidate()
	{
		return Selection.activeObject is RenderTexture;
	}

	[MenuItem("Assets/Textures/Bake RenderTexture To PNG", false, 0)]
	public static void BakeTexture2DPNG()
	{
		var rt = Selection.activeObject as RenderTexture;

		var old = RenderTexture.active;
		RenderTexture.active = rt;

		var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBAFloat, false, true);
		tex.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
		tex.Apply();

		var path = AssetDatabase.GetAssetPath(rt).Replace("\\", "/");
		path = path.Substring(0, path.LastIndexOf("/")) + $"/{rt.name}_2D.png";

		path = AssetDatabase.GenerateUniqueAssetPath(path);

		path = Application.dataPath + path.Replace("Assets", "");

		var bytes = tex.EncodeToPNG();
		File.WriteAllBytes(path, bytes);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		RenderTexture.active = old;
	}

	[MenuItem("Assets/Textures/Bake RenderTexture To PNG", true)]
	public static bool BakeTexture2DPNGValidate()
	{
		return Selection.activeObject is RenderTexture;
	}


#endif
}
