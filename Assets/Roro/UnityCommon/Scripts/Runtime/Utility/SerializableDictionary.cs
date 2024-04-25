using System.Collections.Generic;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	[System.Serializable]
	public class SerializableDictionary<TKey, TVal> : ISerializationCallbackReceiver
	{
		[System.Serializable]
		public class Entry
		{
			public TKey key;
			public TVal value;
		}

		[SerializeField]
		private List<Entry> keyValuePairs;

		private Dictionary<TKey, TVal> dict;

		public TVal this[TKey key]
		{
			get => dict[key];
			set => dict[key] = value;
		}

		public bool ContainsKey(TKey key) => dict.ContainsKey(key);

		public bool TryGetValue(TKey key, out TVal val) => dict.TryGetValue(key, out val);

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			dict = new Dictionary<TKey, TVal>();

			for (var i = 0; i < keyValuePairs.Count; i++)
			{
				var pair = keyValuePairs[i];
				dict[pair.key] = pair.value;
			}
		}
	}
}
