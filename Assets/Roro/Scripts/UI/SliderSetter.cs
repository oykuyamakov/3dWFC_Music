using DG.Tweening;
using UnityCommon.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Based.Utility
{	
    [RequireComponent(typeof(UnityEngine.UI.Slider))]

    public class SliderSetter : UIBinder<Slider>
    {
        [SerializeField]
        private bool m_Animate;

        [SerializeField]
        private float m_Duration;
        
        public void SetSlider(float val) => SetTextObject(val);
        public void SetSlider(int val) => SetTextObject(val);
        public void SetSlider(string val) => SetTextObject(val);

        private void SetTextObject(object val)
        {
            float value;

            if (m_Animate)
            {
                float val2 = (float)(val);
                var start = uiElement.value;
                value = start;
                DOTween.To(bs => value = (bs) ,start , val2, m_Duration).
                    OnUpdate(() => uiElement.value = value);
                return;
            }

            else
            {
                value = (float)val;
            }

            uiElement.value = value;
        }
       
    }
}
