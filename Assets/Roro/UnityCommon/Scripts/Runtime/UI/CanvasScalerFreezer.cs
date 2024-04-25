using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10)]
public class CanvasScalerFreezer : MonoBehaviour
{
	public Canvas canvas;
	public CanvasScaler canvasScaler;

	private float scaleFactor;
	private float referencePixelsPerUnit;

#if UNITY_EDITOR
	private void Reset()
	{
		canvas = GetComponent<Canvas>();
		canvasScaler = GetComponent<CanvasScaler>();
	}
#endif

	private void Start()
	{
		// scaleFactor = canvas.scaleFactor;
		// referencePixelsPerUnit = canvas.referencePixelsPerUnit;
		//
		// canvasScaler.enabled = false;
		// //Destroy(canvasScaler);
		//
		// canvas.scaleFactor = scaleFactor;
		// canvas.referencePixelsPerUnit = referencePixelsPerUnit;
	}
}
