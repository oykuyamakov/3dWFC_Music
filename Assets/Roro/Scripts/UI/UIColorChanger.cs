using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.UI
{
    public class UIColorChanger : MonoBehaviour
    {
        public void ChangeColor(Color color)
        {
            if (TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
            {
                textMeshProUGUI.color = color;
            }
            else if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = color;
            }
            else if (TryGetComponent<Text>(out var txt))
            {
                txt.color = color;
            }
        }
        
        public void ColorBlack()
        {
            if (TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
            {
                textMeshProUGUI.color = Color.black;
            }
            else if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = Color.black;
            }
            else if (TryGetComponent<Text>(out var txt))
            {
                txt.color = Color.black;
            }
        }
        
        public void ColorWhite()
        {
            if (TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
            {
                textMeshProUGUI.color = Color.white;
            }
            else if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = Color.white;
            }
            else if (TryGetComponent<Text>(out var txt))
            {
                txt.color = Color.white;
            }
        }
    }
}
