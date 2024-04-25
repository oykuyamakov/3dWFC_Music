#if UNITY_EDITOR

using UnityCommon.Editor.Utility;
#endif

using UnityEditor;
using UnityEngine;

namespace Roro.Scripts.Serialization.ContextImplementation
{
    public class ScriptableObjectWizard : SerializationWizard
    {

        private readonly SOSerializationData m_Data;

        public ScriptableObjectWizard(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("ScriptableObjectWizard: Path was null or empty.");
                path = "Saves/AutoFallbackSave";
            }
            m_Data = Resources.Load<SOSerializationData>(path);

#if UNITY_EDITOR
            if (m_Data == null)
            {
                m_Data = ScriptableObject.CreateInstance<SOSerializationData>();
                AssetDatabaseHelpers.CreateAssetMkdir(m_Data, $"Assets/Resources/{path}.asset");
                AssetDatabase.SaveAssets();
            }
#endif

            m_Data.PrepareDictionary();
        }
        
        public override bool IsReadOnly() => m_Data.IsReadOnly();
        public override bool Contains(string key)
        {
            return m_Data.Dicti.ContainsKey(key);
        }
        
        public override void SetString(string key, string value, bool writeToReadonly = false)
        {
            if (!IsReadOnly() || (writeToReadonly && Application.isEditor))
                m_Data.Dicti[key] = value;
        }

        public override string GetString(string key, string fallback = null)
        {
            if (m_Data.Dicti.TryGetValue(key, out var value))
            {
                return value;
            }

            return fallback;
        }

        public override void Push()
        {
#if UNITY_EDITOR
            m_Data.PushDict();
            EditorUtility.SetDirty(m_Data);
#endif
        }

        public override void Clear()
        {
            Debug.Log("Not clearing scriptable object context");
        }
    }
}
