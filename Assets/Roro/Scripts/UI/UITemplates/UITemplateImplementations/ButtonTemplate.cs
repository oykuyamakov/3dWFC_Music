using System;
using System.Collections.Generic;
using Events;
using Roro.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.UI;
using Event = Events.Event;

namespace Roro.Scripts.UI.UITemplates.UITemplateImplementations
{
    [RequireComponent(typeof(Button))]
    public class ButtonTemplate : UITemplate<Action>
    {
        [SerializeField]
        private UIElementType m_UIElementType;
        private Button m_Button => GetComponent<Button>();

        private bool m_Pressed;

        private Action m_OnPressedAction;

        private void OnPressed()
        {
            if(m_Pressed)
                return;
            
            m_Pressed = true;
            //HapticManager.GiveHaptic(0.3f,0.5f);
            
            m_OnPressedAction.Invoke();
            m_Pressed = false;
        }

        public override void Set(Action val)
        {
            m_Button.onClick.RemoveAllListeners();
            m_OnPressedAction = val as Action;
            m_Button.onClick.AddListener(OnPressed);
        }

        public override void Enable()
        {
            m_Button.enabled = true;
            m_Button.transform.localScale = Vector3.one;
        }

        public override void Disable()
        {
            m_Button.enabled = false;
            m_Button.transform.localScale = Vector3.zero;
        }

        public override UIElementType GetElementType()
        {
            return m_UIElementType;
        }
    }
}
