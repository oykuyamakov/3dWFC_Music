using Events;

namespace Roro.Scripts.Helpers.EventImplementations
{
	public class AllScenesLoadedEvent : Event<AllScenesLoadedEvent>
	{

		public static AllScenesLoadedEvent Get()
		{
			var evt = GetPooledInternal();
			return evt;
		}
	}
}
