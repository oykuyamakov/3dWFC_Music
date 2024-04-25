using UnityEngine;

namespace Events.UnityEventBinding
{
	public class UnityOnTriggerEnterEvent : Event<UnityOnTriggerEnterEvent>
	{
		public Collider collider;

		public static UnityOnTriggerEnterEvent Get(Collider collider)
		{
			var evt = GetPooledInternal();
			evt.collider = collider;
			return evt;
		}
	}
}
