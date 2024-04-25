
namespace Events
{
	// Global Event Manager
	// ReSharper disable once InconsistentNaming
	public static class GEM
	{
		public static void AddListener<T>(EventListener<T> handler, Priority priority = Priority.Normal,
		                                  int channel = EventManager.DefaultChannel)
			where T : Event, new() => EventManager.AddListener(handler, priority, null, channel);

		public static void RemoveListener<T>(EventListener<T> handler, int channel = EventManager.DefaultChannel)
			where T : Event, new() => EventManager.RemoveListener(handler, null, channel);

		public static T SendEvent<T>(T evt, int channel = EventManager.DefaultChannel) where T : Event, new() =>
			EventManager.SendEvent(evt, null, channel);
	}
}
