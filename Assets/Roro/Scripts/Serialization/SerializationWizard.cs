using Roro.Scripts.Serialization.ContextImplementation;

namespace Roro.Scripts.Serialization
{
	public abstract class SerializationWizard
	{
		private static SerializationWizard m_DefaultSerializationWizard;

		public static SerializationWizard Default
		{
			get
			{
				if (m_DefaultSerializationWizard == null)
				{
					string prefix = null;
					
					m_DefaultSerializationWizard = new PrioritySerializationWizard(
						new PlayerPrefsWizard(prefix));
				}

				return m_DefaultSerializationWizard;
			}
		}

		public static void Reset()
		{
			m_DefaultSerializationWizard = null;
		}

		public abstract bool IsReadOnly();

		public abstract bool Contains(string key);


		public abstract void Clear();

		public abstract void SetString(string key, string value, bool writeToReadonly = false);

		public abstract string GetString(string key, string fallback = null);

		public abstract void Push();

		public virtual int GetInt(string key, int fallback = -1)
		{
			return int.TryParse(GetString(key, ""), out var parsedVal) ? parsedVal : fallback;
		}

		public virtual void SetInt(string key, int value, bool writeToReadonly = false)
		{
			SetString(key, value.ToString(), writeToReadonly);
		}

		public virtual long GetLong(string key, long fallback = -1)
		{
			return long.TryParse(GetString(key, "-1"), out var parsedVal) ? parsedVal : fallback;
		}

		public virtual void SetLong(string key, long value, bool writeToReadonly = false)
		{
			SetString(key, value.ToString(), writeToReadonly);
		}

		public virtual bool GetBool(string key, bool fallback)
		{
			if (!Contains(key))
				return fallback;

			var intVal = GetInt(key, -1);
			if (intVal < 0)
				return fallback;

			return intVal != 0;
		}


		public virtual void SetBool(string key, bool value)
		{
			SetInt(key, value ? 1 : 0);
		}
	}
}
