using UnityEngine;
using UnityEngine.UI;

namespace UnityCommon.UI
{
	[RequireComponent(typeof(UnityEngine.UI.Text))]
	public class TextSetterMultiVar : UIBinder<Text>
	{
		[TextArea]
		[SerializeField]
		private string format = "";


		private object val1, val2;


		public void SetFirst(float val)  => SetFirstObject(val);
		public void SetFirst(int val)    => SetFirstObject(val);
		public void SetFirst(string val) => SetFirstObject(val);
		public void SetFirst(bool val)   => SetFirstObject(val);

		public void SetSecond(float val)  => SetSecondObject(val);
		public void SetSecond(int val)    => SetSecondObject(val);
		public void SetSecond(string val) => SetSecondObject(val);
		public void SetSecond(bool val)   => SetSecondObject(val);

		private void SetFirstObject(object val)
		{
			val1 = val;
			string value;
			if (format == "")
			{
				value = val.ToString();
			}
			else
			{
				value = string.Format(format, val1, val2);
			}

			uiElement.text = value;
		}

		private void SetSecondObject(object val)
		{
			val2 = val;
			string value;
			if (format == "")
			{
				value = val.ToString();
			}
			else
			{
				value = string.Format(format, val1, val2);
			}

			uiElement.text = value;
		}
	}
}
