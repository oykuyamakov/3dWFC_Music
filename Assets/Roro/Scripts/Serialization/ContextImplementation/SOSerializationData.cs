using System;
using System.Collections.Generic;
using UnityEngine;

namespace Roro.Scripts.Serialization.ContextImplementation
{
    public class SOSerializationData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private bool m_IsReadOnly = true;

        [SerializeField]
        private List<string> m_Keys = new List<string>();

        [SerializeField]
        private List<string> m_Values = new List<string>();

        [NonSerialized]
        public Dictionary<string, string> Dicti = new Dictionary<string, string>();

        public bool IsReadOnly() => m_IsReadOnly;

        private void Reset()
        {
            m_Keys = new List<string>();
            m_Values = new List<string>();
            Dicti = new Dictionary<string, string>();
        }
        
        public void OnBeforeSerialize()
        {
            PushDict();
        }

        public void OnAfterDeserialize()
        {
            for (var i = 0; i < m_Keys.Count; i++)
            {
                Dicti[m_Keys[i]] = m_Values[i];
            }
        }
        
        public void PushDict()
        {
            m_Keys = new List<string>();
            m_Values = new List<string>();
            foreach (KeyValuePair<string, string> kv in Dicti)
            {
                m_Keys.Add(kv.Key);
                m_Values.Add(kv.Value);
            }
        }
        
        public void PrepareDictionary()
        {
            Dicti = new Dictionary<string, string>();
            for (var i = 0; i < m_Keys.Count; i++)
            {
                Dicti.Add(m_Keys[i], m_Values[i]);
            }
        }
    }
}
