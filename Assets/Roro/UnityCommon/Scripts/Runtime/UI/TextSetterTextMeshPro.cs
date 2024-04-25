using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.UI
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextSetterTextMeshPro : UIBinder<TextMeshProUGUI>
    {


        [TextArea]
        [SerializeField] private string format = "";

		

        public void SetText(float val) => SetTextObject(val);
        public void SetText(int val) => SetTextObject(val);
        public void SetText(string val) => SetTextObject(val);
        public void SetText(bool val) => SetTextObject(val);

        private void SetTextObject(object val)
        {
            string value;
            if (format == "")
            {
                value = val.ToString();
            }
            else
            {
                value = string.Format(format, val);
            }

            uiElement.text = value;


        }



    }

}