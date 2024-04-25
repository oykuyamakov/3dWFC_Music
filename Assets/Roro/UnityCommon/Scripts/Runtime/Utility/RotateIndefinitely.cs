using Sirenix.OdinInspector;
using UnityEngine;

public class RotateIndefinitely : MonoBehaviour
{
	public bool randomVelocity = false;

	[ShowIf("$randomVelocity")]
	public float randomMagnitude = 120f;

	[HideIf("$randomVelocity")]
	public Vector3 velocity = Vector3.forward * 540f;

	public Space space = Space.Self;

	private Transform t;

	private bool isVisible = true;

	private void Awake()
	{
		t = transform;

		if (randomVelocity)
			velocity = UnityEngine.Random.insideUnitSphere * randomMagnitude;

		isVisible = true;
	}

	private void Update()
	{
		if (!isVisible)
			return;

		t.Rotate(velocity * Time.deltaTime, space);
	}

	private void OnBecameInvisible()
	{
		isVisible = false;
	}

	private void OnBecameVisible()
	{
		isVisible = true;
	}
}
