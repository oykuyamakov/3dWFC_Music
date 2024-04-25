using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.Helpers
{
    public class TextFontUnifier : MonoBehaviour
    {
        [SerializeField][Required]
        private Font m_Font;
         
        [SerializeField][Required]
        private TMP_FontAsset m_TmpFont;    
        
        [Button]
        public void ChangeAllTextFonts()
        {
            if (m_Font != null)
            {
                var txts = GameObject.FindObjectsOfType<Text>();

                for (int i = 0; i < txts.Length; i++)
                {
                    txts[i].font = m_Font;
                } 
            }

            if(m_TmpFont == null)
                return;
            
            var txtsPr = GameObject.FindObjectsOfType<TextMeshProUGUI>();

            for (int i = 0; i < txtsPr.Length; i++)
            {
                txtsPr[i].font = m_TmpFont;
            }
        }
    }
}