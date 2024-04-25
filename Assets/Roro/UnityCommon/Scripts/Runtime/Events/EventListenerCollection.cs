using System;
using System.Collections.Generic;
using UnityCommon.Runtime.Collections;
using UnityEngine;

namespace Events
{
	public class EventListenerCollection
	{
		private Dictionary<Type, PriorityList<object>> events;

		public EventListenerCollection()
		{
			events = new Dictionary<Type, PriorityList<object>>();
		}


		public PriorityList<object> GetListenersForType(Type type)
		{
			events.TryGetValue(type, out var list);
			return list;
		}

		public void AddListener<T>(EventListener<T> handler, Priority priority = Priority.Normal)
			where T : Event
		{
			var eventType = typeof(T);
			if (!events.TryGetValue(eventType, out var priorityList))
			{
				priorityList = new PriorityList<object>();
				events[eventType] = priorityList;
			}

			priorityList.Add(handler, (int) priority);
		}

		public void RemoveListener<T>(EventListener<T> handler) where T : Event
		{
			var eventType = typeof(T);
			if (!events.TryGetValue(eventType, out var priorityList))
			{
				return;
			}

			priorityList.Remove(handler);
		}


		public void SendEvent<T>(T evt) where T : Event
		{
			var evtType = evt.GetType();
			if (!events.TryGetValue(evtType, out var evtTypeHandlers))
			{
				// UnityEngine.Debug.Log($"No event handler for event type {evtType.Name} exists");
				return;
			}

			for (var i = 0; i < evtTypeHandlers.Count; i++)
			{
				var handler = evtTypeHandlers[i] as EventListener<T>;
				if (handler == null)
				{
					throw new Exception(
						$"Event types do not match. Expected {typeof(EventListener<T>).FullName}, encountered {evtTypeHandlers[i].GetType().FullName}");
				}

				try
				{
					handler(evt);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}

				if (evt.IsConsumed)
				{
					break;
				}
			}
		}
	}
}
