using TMPro;
using UnityEngine;

namespace Roro.Scripts.UI.UITemplates.UITemplateImplementations
{
    [RequireComponent(typeof(TextMeshProUGUI))]

    public class TextMeshProTemplate : UITemplate<string>
    {
        
        private TextMeshProUGUI m_Text => GetComponent<TextMeshProUGUI>();

        public override void Set(string val)
        {
            m_Text.text = val;
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
            throw new System.NotImplementedException();
        }
    }
}
