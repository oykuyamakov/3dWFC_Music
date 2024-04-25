using UnityEngine;

namespace Events.UnityEventBinding
{
	public class UnityOnCollisionExitEvent : Event<UnityOnCollisionExitEvent>
	{
		public Collision collision;

		public static UnityOnCollisionExitEvent Get(Collision collision)
		{
			var evt = GetPooledInternal();
			evt.collision = collision;
			return evt;
		}
	}
}
