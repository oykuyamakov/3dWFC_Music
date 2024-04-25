using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]
public class LayerCullData
{
	public LayerMask layers = LayerMask.GetMask();
	public float distance = 100f;
}


public class LayerCullDistances : MonoBehaviour
{
	public Camera cam;

	public bool layerCullSpherical = false;

	public List<LayerCullData> data;

	[ReadOnly]
	public float[] layerCullDistances;

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (!cam)
			cam = GetComponent<Camera>();

		layerCullDistances = new float[32];
		for (var i = 0; i < 32; i++)
		{
			uint m = 1u << i;
			float distance = 0;
			foreach (var layerCullData in data)
			{
				if ((layerCullData.layers.value & m) != 0)
				{
					distance = layerCullData.distance;
					break;
				}
			}

			layerCullDistances[i] = distance;
		}
	}
#endif

	private void Start()
	{
		cam.layerCullDistances = layerCullDistances;
		cam.layerCullSpherical = layerCullSpherical;

#if UNITY_EDITOR
		var sceneCam = SceneView.GetAllSceneCameras().FirstOrDefault();
		if (sceneCam != null)
		{
			sceneCam.layerCullDistances = layerCullDistances;
			sceneCam.layerCullSpherical = layerCullSpherical;
		}
#endif
	}
}
