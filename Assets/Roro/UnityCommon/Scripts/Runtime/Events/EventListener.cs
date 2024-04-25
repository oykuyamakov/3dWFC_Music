using System.Collections.Generic;
using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
	public class EventListener : MonoBehaviour
	{
		public string listenerName = "Unnamed Listener";

		public List<EventContext> events;

		public UnityEvent response;
		public float responseDelay = -1f;

		public void OnEnable()
		{
			if (Application.isPlaying)
			{
				foreach (var ev in events)
				{
					ev.AddListener<VoidEvent>(OnEventFired);
				}
			}
		}

		public void OnDisable()
		{
			foreach (var ev in events)
			{
				ev.RemoveListener<VoidEvent>(OnEventFired);
			}
		}


		public void OnEventFired(VoidEvent evt)
		{
			if (responseDelay <= 1e-2f)
			{
				response?.Invoke();
			}
			else
			{
				Conditional.Wait(responseDelay).Do(() => { response?.Invoke(); });
			}
		}

		public string GetName() => listenerName;
	}
}
