using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.UI.UITemplates
{
    [RequireComponent(typeof(Text))]
    public class TextTemplate : UITemplate<string>
    {
        private Text m_Text => GetComponent<Text>();

        [SerializeField]
        private UIElementType m_UIElementType;
        
        public override void Set(string val)
        {
            m_Text.text = val.ToString();
        }

        public override void Enable()
        {
            m_Text.enabled = true;
        }

        public override void Disable()
        {
            m_Text.enabled = false;
        }

        public override UIElementType GetElementType()
        {
            return m_UIElementType;
        }
    }
}
