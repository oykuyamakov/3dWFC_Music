using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.UI
{
    public class UIImageAnimator : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] m_AptalSprites;

        [SerializeField]
        private float m_Duration = 1f;

        [SerializeField]
        private float m_Gap = 0.01f;

        private Image m_Image;
        private int index = 0;
        private float timer = 0;

        private void Awake()
        {
            m_Image = GetComponent<Image>();
        }

        public void EnableSelf()
        {
            m_Image.enabled = true;
        }
        public void DisableSelf()
        {
            m_Image.enabled = false;
            index = 0;
        }

        public IEnumerator AnimateSplash(Action oncomplete)
        {
            m_Image.enabled = true;
            for (int i = 0; i < m_AptalSprites.Length; i++)
            {
                m_Image.sprite = m_AptalSprites[i];
                yield return new WaitForSeconds(m_Gap);
            }
            
            m_Image.enabled = false;
            oncomplete.Invoke();
        }

        public void LoopAnim()
        {
            if((timer+=Time.deltaTime) >= (m_Duration / m_AptalSprites.Length))
            {
                timer = 0;
                m_Image.sprite = m_AptalSprites[index];
                index = (index + 1) % m_AptalSprites.Length;
            }
        }
    }
}
