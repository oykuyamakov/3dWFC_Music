using System;
using UnityCommon.Runtime.UI.Animations;
using UnityEngine;

namespace Roro.Scripts.Helpers
{
    [Serializable][RequireComponent(typeof(UITranslateAnim))]
    public abstract class PopupPanel : MonoBehaviour
    {
        public UITranslateAnim m_Anim => GetComponent<UITranslateAnim>();

        public virtual void Open()
        {
            m_Anim.FadeIn();
        }

        public virtual void Close()
        {
            m_Anim.FadeOut();
        }
    }
}
