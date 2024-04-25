using System.Linq;

namespace Roro.Scripts.Serialization.ContextImplementation
{
    /// <summary>
    /// Tries to get data in the order of sub-contexts. If data is found, it does not look further into other sub-contexts.
    /// </summary>
    public class PrioritySerializationWizard : SerializationWizard
    {
        private readonly SerializationWizard[] m_SubContexts;

        public PrioritySerializationWizard(params SerializationWizard[] subContexts)
        {
            this.m_SubContexts = subContexts;
        }

        public override bool IsReadOnly() => false;

        public override bool Contains(string key)
        {
            return m_SubContexts.Any(ctx => ctx.Contains(key));
        }

        public override void SetString(string key, string value, bool writeToReadonly = false)
        {
            for (var i = 0; i < m_SubContexts.Length; i++)
            {
                m_SubContexts[i].SetString(key, value, writeToReadonly);
            }
        }

        public override string GetString(string key, string fallback = null)
        {
            for (var i = 0; i < m_SubContexts.Length; i++)
            {
                if (m_SubContexts[i].Contains(key))
                {
                    return m_SubContexts[i].GetString(key, fallback);
                }
            }

            return fallback;
        }

        public override void Push()
        {
            for (var i = 0; i < m_SubContexts.Length; i++)
            {
                m_SubContexts[i].Push();
            }
        }

        public override void Clear()
        {
            for (var i = 0; i < m_SubContexts.Length; i++)
            {
                m_SubContexts[i].Clear();
            }
        }
    }
}