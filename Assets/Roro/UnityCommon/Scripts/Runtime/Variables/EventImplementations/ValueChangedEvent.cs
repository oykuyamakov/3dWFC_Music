using Events;

namespace UnityCommon.Runtime.Variables
{
	public class ValueChangedEvent<T> : Event<ValueChangedEvent<T>>
	{
		public T value;
		
		public static ValueChangedEvent<T> Get(T value)
		{
			var evt = GetPooledInternal();
			evt.value = value;
			return evt;
		}

	}
}
