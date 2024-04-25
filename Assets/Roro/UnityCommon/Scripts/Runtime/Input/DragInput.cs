using UnityEngine;

public enum InputScaleMode
{
	SensitivityPerWidth = 0,
	SensitivityPerHeight = 1,
}

[System.Serializable]
public class DragInput
{
	public float sensitivity = 10f;

	public InputScaleMode inputScaleMode;

	private Vector3 lastMousePos;
	private Vector3 currentMousePos;

	public Vector3 MouseDelta => currentMousePos - lastMousePos;

	public void Reset()
	{
		lastMousePos = Input.mousePosition;
		currentMousePos = lastMousePos;
	}

	public float GetAxis(Vector3 axis)
	{
		return Vector3.Dot(MouseDelta, axis.normalized) * sensitivity /
		       (inputScaleMode == InputScaleMode.SensitivityPerWidth ? Screen.width : Screen.height);
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			lastMousePos = Input.mousePosition;
			currentMousePos = lastMousePos;
			return;
		}

		if (Input.GetMouseButton(0))
		{
			lastMousePos = currentMousePos;
			currentMousePos = Input.mousePosition;
		}
		else
		{
			currentMousePos = lastMousePos;
		}
	}
}
