using System.Collections.Generic;
using UnityEngine;

namespace Events
{
	public static class EventManager
	{
		public static readonly object GlobalContext = new object();

		public static object CurrentContext = GlobalContext;

		public const int DefaultChannel = -1;

		private static Dictionary<object, Dictionary<int, EventListenerCollection>> eventChannelsByContext;

		static EventManager()
		{
			eventChannelsByContext = new Dictionary<object, Dictionary<int, EventListenerCollection>>();
		}

		private static bool TryGetEventListenerCollection(object context, int channel,
		                                                  out EventListenerCollection listeners,
		                                                  bool createIfNotExists = true)
		{
			if (context == null)
				context = GlobalContext;

			if (!eventChannelsByContext.TryGetValue(context, out var listenersByChannel))
			{
				if (!createIfNotExists)
				{
					listeners = null;
					return false;
				}

				listenersByChannel = new Dictionary<int, EventListenerCollection>();
				eventChannelsByContext[context] = listenersByChannel;
			}

			if (!listenersByChannel.TryGetValue(channel, out listeners))
			{
				if (!createIfNotExists)
				{
					listeners = null;
					return false;
				}

				listeners = new EventListenerCollection();
				listenersByChannel[channel] = listeners;
			}

			return true;
		}

		public static void AddListener<T>(EventListener<T> listener, Priority priority = Priority.Normal,
		                                  object context = null, int channel = DefaultChannel)
			where T : Event, new()
		{
			TryGetEventListenerCollection(context, channel, out var listeners);
			listeners.AddListener(listener, priority);
		}

		public static void RemoveListener<T>(EventListener<T> listener, object context = null,
		                                     int channel = DefaultChannel)
			where T : Event, new()
		{
			if (!TryGetEventListenerCollection(context, channel, out var listeners, false))
			{
				// Debug.Log($"Trying to remove non-existent listener of type {typeof(T)}");
				return;
			}

			listeners.RemoveListener(listener);
		}

		public static T SendEvent<T>(T evt, object context = null, int channel = DefaultChannel)
			where T : Event, new()
		{
			if (!TryGetEventListenerCollection(context, channel, out var listeners, false))
			{
				// Debug.Log($"Trying to send event of type {typeof(T)} to non-existent listeners");
				return evt;
			}

			var oldContext = CurrentContext;

			CurrentContext = context;

			listeners.SendEvent(evt);

			CurrentContext = oldContext;

			return evt;
		}
	}
}
