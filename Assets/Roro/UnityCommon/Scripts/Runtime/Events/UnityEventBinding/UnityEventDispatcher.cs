using Sirenix.OdinInspector;
using UnityEngine;

namespace Events.UnityEventBinding
{
	public class UnityEventDispatcher : MonoBehaviour
	{
		[SerializeField]
		private UnityEventCallbackType m_EventMask;

		public UnityEventCallbackType EventMask
		{
			get => m_EventMask;
			set => m_EventMask = value;
		}

		[SerializeField]
		private bool m_UseExplicitEventContext = false;

		[ShowIf("$m_UseExplicitEventContext")]
		[SerializeField]
		private UnityEngine.Object m_ExplicitEventContext;

		private bool ShouldSendEvent(UnityEventCallbackType type) => ((int) m_EventMask & (int) type) != 0;

		private void SendEvent<T>(T evt) where T : Event, new()
		{
			evt.target = gameObject;
			(m_UseExplicitEventContext ? m_ExplicitEventContext : evt.target).SendEvent(evt);
		}


		private void Awake()
		{
			if (ShouldSendEvent(UnityEventCallbackType.Awake))
			{
				using var evt = UnityAwakeEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnEnable()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnEnable))
			{
				using var evt = UnityOnEnableEvent.Get();
				SendEvent(evt);
			}
		}

		private void Start()
		{
			if (ShouldSendEvent(UnityEventCallbackType.Start))
			{
				using var evt = UnityStartEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnDisable()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnDisable))
			{
				using var evt = UnityOnDisableEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnDestroy()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnDestroy))
			{
				using var evt = UnityOnDestroyEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnBecameVisible()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnBecameVisible))
			{
				using var evt = UnityOnBecameVisibleEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnBecameInvisible()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnBecameInvisible))
			{
				using var evt = UnityOnBecameInvisibleEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnCollisionEnter))
			{
				using var evt = UnityOnCollisionEnterEvent.Get(other);
				SendEvent(evt);
			}
		}


		private void OnCollisionExit(Collision other)
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnCollisionExit))
			{
				using var evt = UnityOnCollisionExitEvent.Get(other);
				SendEvent(evt);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnTriggerEnter))
			{
				using var evt = UnityOnTriggerEnterEvent.Get(other);
				SendEvent(evt);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnTriggerExit))
			{
				using var evt = UnityOnTriggerExitEvent.Get(other);
				SendEvent(evt);
			}
		}

		private void OnMouseEnter()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnMouseEnter))
			{
				using var evt = UnityOnMouseEnterEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnMouseExit()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnMouseEnter))
			{
				using var evt = UnityOnMouseEnterEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnMouseDown()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnMouseDown))
			{
				using var evt = UnityOnMouseDownEvent.Get();
				SendEvent(evt);
			}
		}

		private void OnMouseUp()
		{
			if (ShouldSendEvent(UnityEventCallbackType.OnMouseUp))
			{
				using var evt = UnityOnMouseUpEvent.Get();
				SendEvent(evt);
			}
		}
	}
}
