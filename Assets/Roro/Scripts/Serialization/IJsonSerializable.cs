using Newtonsoft.Json;

namespace Roro.Scripts.Serialization
{
	public interface IJsonSerializable
	{
		public void Serialize(JsonWriter jw);
		
	}
}
