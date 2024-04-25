using UnityEngine;

namespace Events.UnityEventBinding
{
	public class UnityOnCollisionEnterEvent : Event<UnityOnCollisionEnterEvent>
	{
		public Collision collision;

		public static UnityOnCollisionEnterEvent Get(Collision collision)
		{
			var evt = GetPooledInternal();
			evt.collision = collision;
			return evt;
		}
	}
}
