using UnityCommon.Variables;
using UnityEngine;

namespace UnityCommon.DataBinding
{
	public class ColorObserver : DataObserver<ColorReference, ColorVariable, ColorEvent, Color>
	{
// 		protected override void OnValueChanged(Color val)
// 		{
// 			base.OnValueChanged(val);
// #if UNITY_EDITOR
// 			if (stackTrace)
// 				Debug.Log("Modified: " + val, this.gameObject);
// #endif
// 		}
	}
}
