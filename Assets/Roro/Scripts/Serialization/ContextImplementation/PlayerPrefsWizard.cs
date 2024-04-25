using System.Text;
using UnityEngine;

namespace Roro.Scripts.Serialization.ContextImplementation
{
    public class PlayerPrefsWizard : SerializationWizard
    {
        private string m_Prefix;

        private StringBuilder StringBuilder;

        public PlayerPrefsWizard(string prefix)
        {
            m_Prefix = prefix;
            StringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(prefix))
            {
                PlayerPrefs.SetString("%pp_prefix%", prefix);
                PlayerPrefs.Save();
            }
        }

        private string GetPrefixedKey(string key) => string.IsNullOrEmpty(m_Prefix)
            ? key
            : StringBuilder.Clear().Append(m_Prefix).Append('_').Append(key).ToString();


        public override bool IsReadOnly() => false;

        public override bool Contains(string key) => PlayerPrefs.HasKey(GetPrefixedKey(key));

        public override void SetString(string key, string value, bool writeToReadonly = false) =>
            PlayerPrefs.SetString(GetPrefixedKey(key), value);

        public override string GetString(string key, string fallback = null) =>
            PlayerPrefs.GetString(GetPrefixedKey(key), fallback);

        public override void Push() => PlayerPrefs.Save();

        public override void Clear() => PlayerPrefs.DeleteAll();
    }
}