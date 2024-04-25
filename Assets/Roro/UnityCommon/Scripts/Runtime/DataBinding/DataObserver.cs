using System.Collections.Generic;
using Events;
using UnityCommon.Runtime.Variables;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.DataBinding
{
	[System.Serializable]
	public class FloatEvent : UnityEvent<float>
	{
	}

	[System.Serializable]
	public class IntEvent : UnityEvent<int>
	{
	}

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool>
	{
	}

	[System.Serializable]
	public class StringEvent : UnityEvent<string>
	{
	}

	[System.Serializable]
	public class SpriteEvent : UnityEvent<Sprite>
	{
	}

	[System.Serializable]
	public class ColorEvent : UnityEvent<Color>
	{
	}

	[DefaultExecutionOrder(-5)]
	public class DataObserver<TRef, TVar, TEvent, T> : MonoBehaviour where TRef : Reference<T, TVar>, new()
	                                                                 where TVar : Variable<T>
	                                                                 where TEvent : UnityEvent<T>
	{
		public bool stackTrace = false;

		[SerializeField]
		protected bool onStart = true;

		[SerializeField]
		protected TRef data;

		[SerializeField]
		protected TEvent onModified;

		[SerializeField]
		private float delay = -1f;

		public float Delay
		{
			get => delay;
			set => delay = value;
		}

		private bool isInitialized = false;

		private Queue<T> dataQueue;
		private Queue<float> timeQueue;


		private void Reset()
		{
			data = new TRef();
			data.type = Reference.Type.GlobalVariable;
		}

		protected virtual void OnValidate()
		{
			if (data.type == Reference.Type.Constant)
			{
				Debug.Log("Observed data can not be 'Constant', switching to 'Global'");
				data.type = Reference.Type.GlobalVariable;
			}
		}


		protected virtual void Start()
		{
			isInitialized = true;

			if (data.type == Reference.Type.Constant)
			{
				//
			}
			else
			{
				var variable = data.GetVariable();

				variable.AddListener<ValueChangedEvent<T>>(OnValueChanged);
			}

			if (onStart)
			{
				OnDataModifiedNoDelay(data.Value);
			}

			if (delay > 0)
			{
				dataQueue = new Queue<T>(32);
				timeQueue = new Queue<float>(32);
			}
		}

		protected virtual void OnEnable()
		{
			if (isInitialized == false)
				return;


			if (data.type == Reference.Type.Constant)
			{
				//
			}
			else
			{
				var variable = data.GetVariable();

				if (variable == null)
					return;

				variable.AddListener<ValueChangedEvent<T>>(OnValueChanged);
			}

			OnDataModifiedNoDelay(data.Value);
		}

		protected virtual void OnDisable()
		{
			if (data.type == Reference.Type.Constant)
			{
				//
			}
			else
			{
				var variable = data.GetVariable();

				if (variable == null)
					return;

				variable.RemoveListener<ValueChangedEvent<T>>(OnValueChanged);
			}
		}


		private void Update()
		{
			if (timeQueue == null)
				return;

			while (timeQueue.Count > 0 && Time.time >= timeQueue.Peek())
			{
				timeQueue.Dequeue();
				var delayedData = dataQueue.Dequeue();
				onModified.Invoke(delayedData);
			}
		}

		protected virtual void OnDataModifiedNoDelay(T value)
		{
			var oldDelay = delay;
			delay = 0f;
			UseValue(value);
			delay = oldDelay;
		}

		protected virtual void OnValueChanged(ValueChangedEvent<T> evt) => UseValue(evt.value);


		private void UseValue(T value)
		{
			if (delay <= 0)
			{
				onModified.Invoke(value);
				return;
			}

			if (dataQueue == null)
			{
				dataQueue = new Queue<T>(32);
				timeQueue = new Queue<float>(32);
			}

			dataQueue.Enqueue(value);
			timeQueue.Enqueue(Time.time + this.delay);
		}
	}
}
