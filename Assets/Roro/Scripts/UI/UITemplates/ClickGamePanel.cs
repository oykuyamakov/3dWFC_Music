using Sirenix.Utilities;
using UnityCommon.Runtime.UI.Animations;
using UnityEngine.EventSystems;

namespace Roro.Scripts.UI.UITemplates
{
    public class ClickGamePanel : GamePanel, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            OnHide();
        }

        protected override void OnHide()
        {
            base.OnHide();
            GetComponentsInChildren<UITranslateAnim>().ForEach(i => i.FadeOut());
        }

        protected override void OnShow(object obj)
        {
            GetComponentsInChildren<UITranslateAnim>().ForEach(i => i.FadeIn());
            base.OnShow(obj);
        }
    }
}