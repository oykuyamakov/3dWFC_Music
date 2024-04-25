using UnityCommon.Runtime.Variables;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityCommon.DataBinding
{
	public class IntObserver : DataObserver<IntReference, IntVariable, IntEvent, int>
	{
		public int multiplier = 1;
		public int offset = 0;

		protected override void OnValueChanged(ValueChangedEvent<int> evt)
		{
			evt.value = evt.value * multiplier + offset;
			base.OnValueChanged(evt);
#if UNITY_EDITOR
			if (stackTrace)
				Debug.Log("Modified: " + evt.value, this.gameObject);
#endif
		}
	}
}
