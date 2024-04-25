using UnityEngine;

namespace Events.UnityEventBinding
{
	public class UnityOnTriggerExitEvent : Event<UnityOnTriggerExitEvent>
	{
		public Collider collider;

		public static UnityOnTriggerExitEvent Get(Collider collider)
		{
			var evt = GetPooledInternal();
			evt.collider = collider;
			return evt;
		}
	}
}
