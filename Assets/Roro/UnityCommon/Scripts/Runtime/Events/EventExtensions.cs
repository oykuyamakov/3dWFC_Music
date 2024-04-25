
using UnityEngine;

namespace Events
{
	public static class EventExtensions
	{
		public static T SendGlobal<T>(this T evt, int channel = EventManager.DefaultChannel)
			where T : Event, new() => GEM.SendEvent(evt, channel);

		public static void AddListener<T>(this object context, EventListener<T> listener,
		                                  Priority priority = Priority.Normal,
		                                  int channel = EventManager.DefaultChannel) where T : Event, new() =>
			EventManager.AddListener(listener, priority, context, channel);

		public static void RemoveListener<T>(this object context, EventListener<T> listener,
		                                     int channel = EventManager.DefaultChannel) where T : Event, new() =>
			EventManager.RemoveListener(listener, context, channel);

		public static void SendEvent(this object context, int channel = EventManager.DefaultChannel) =>
			SendEvent<VoidEvent>(context, channel);

		public static void SendEvent<T>(this object context, int channel = EventManager.DefaultChannel)
			where T : Event<T>, new()
		{
			using (var evt = Event<T>.Get())
				SendEvent(context, evt, channel);
		}

		public static T SendEvent<T>(this object context, T evt, int channel = EventManager.DefaultChannel)
			where T : Event, new()
		{
			if (evt == null)
			{
				using (var voidEvt = VoidEvent.Get())
					EventManager.SendEvent(voidEvt, context, channel);
				return null;
			}

			return EventManager.SendEvent(evt, context, channel);
		}
	}
}
