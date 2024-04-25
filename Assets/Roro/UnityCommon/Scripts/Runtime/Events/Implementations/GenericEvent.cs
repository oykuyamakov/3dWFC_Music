
namespace Events
{
	public class GenericEvent<T> : Event<GenericEvent<T>>
	{
		public T value;

		public static GenericEvent<T> Get(T value)
		{
			var evt = GetPooledInternal();
			evt.value = value;
			return evt;
		}
	}
}
