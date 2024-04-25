using System;
using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.UI.UITemplates.UITemplateImplementations
{
    [RequireComponent(typeof(Image))]
    public class ImageTemplate : UITemplate<Sprite>
    {
        private Image m_Image => GetComponent<Image>();

        public override void Set(Sprite val)
        {
            m_Image.sprite = val;
        }

        public override void Enable()
        {
            m_Image.enabled = true;
        }

        public override void Disable()
        {
            m_Image.enabled = false;
        }

        public override UIElementType GetElementType()
        {
            throw new NotImplementedException();
        }
    }
}