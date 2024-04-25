using System;

namespace Events.UnityEventBinding
{
	[Flags]
	public enum UnityEventCallbackType
	{
		Awake = 1 << 0,
		OnEnable = 1 << 1,
		Start = 1 << 2,
		OnCollisionEnter = 1 << 3,
		OnCollisionExit = 1 << 4,
		OnTriggerEnter = 1 << 5,
		OnTriggerExit = 1 << 6,
		OnDisable = 1 << 7,
		OnDestroy = 1 << 8,
		OnBecameVisible = 1 << 9,
		OnBecameInvisible = 1 << 10,
		OnMouseEnter = 1 << 11,
		OnMouseExit = 1 << 12,
		OnMouseDown = 1 << 13,
		OnMouseUp = 1 << 14,
	}
}
