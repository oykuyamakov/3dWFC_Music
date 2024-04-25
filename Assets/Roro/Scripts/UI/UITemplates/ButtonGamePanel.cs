using System;
using Roro.Scripts.UI.UITemplates.UITemplateImplementations;
using Roro.Scripts.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Roro.Scripts.UI.UITemplates
{
    public class ButtonGamePanel : GamePanel
    {

        [Required][SerializeField]
        private ButtonTemplate m_ButtonT;

        public override void OnAwake()
        {
            Action act = OnDone;

            if (m_ButtonT == null)
            {
                if (this.gameObject.TryGetComponentInChildren(out m_ButtonT))
                {
                    m_ButtonT.Set(act);
                }
            }
            else
            {
                m_ButtonT.Set(act);
            }
            base.OnAwake();
        }

        public void OnDone()
        {
            OnHide();
        }

        private bool TryGetDoneButtonTemplate(out ButtonTemplate button)
        {
            return (button = GetComponentInChildren<ButtonTemplate>()) != null;
        }
    }
}
