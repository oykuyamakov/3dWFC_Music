using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Roro.Scripts.Serialization
{
	public static class JsonSerialization
	{
		private static StringBuilder stringBuilder;
		private static StringWriter stringWriter;
		private static JsonWriter jsonWriter;

		private static JsonSerializerSettings settings = new JsonSerializerSettings
		                                                 {
			                                                 TypeNameHandling = TypeNameHandling.Auto,
			                                                 ReferenceLoopHandling =
				                                                 ReferenceLoopHandling.Ignore,
			                                                 Culture = CultureInfo.InvariantCulture,
			                                                 TypeNameAssemblyFormatHandling =
				                                                 TypeNameAssemblyFormatHandling.Simple,
			                                                 ConstructorHandling = ConstructorHandling
				                                                 .AllowNonPublicDefaultConstructor,
			                                                 MissingMemberHandling = MissingMemberHandling.Ignore,
			                                                 NullValueHandling = NullValueHandling.Include,
			                                                 ObjectCreationHandling = ObjectCreationHandling.Auto,
		                                                 };

#if UNITY_EDITOR
		static JsonSerialization()
		{
			Initialize();
		}
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Initialize()
		{
			if (stringBuilder != null)
				return;

			stringBuilder = new StringBuilder();
			stringWriter = new StringWriter(stringBuilder);
			jsonWriter = new JsonTextWriter(stringWriter);
		}

		private static string GetJsonString()
		{
			stringWriter.Flush();
			var data = stringBuilder.ToString();
			stringBuilder.Clear();
			return data;
		}

		public static string Serialize(IJsonSerializable serializable)
		{
			serializable.Serialize(jsonWriter);
			return GetJsonString();
		}

		public static string Serialize(IEnumerable<IJsonSerializable> collection)
		{
			jsonWriter.WriteStartArray();
			foreach (var item in collection)
			{
				item.Serialize(jsonWriter);
			}

			jsonWriter.WriteEndArray();

			return GetJsonString();
		}


		public static T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, settings);
		}

		public static T Instantiate<T>(T obj) where T : IJsonSerializable
		{
			return Deserialize<T>(Serialize(obj));
		}
	}
}
