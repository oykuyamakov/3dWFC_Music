using UnityCommon.Runtime.Variables;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.DataBinding
{
	public class BoolObserver : DataObserver<BoolReference, BoolVariable, BoolEvent, bool>
	{
		[SerializeField]
		protected bool negate = false;

		[SerializeField]
		private UnityEvent onTrue = null;

		[SerializeField]
		private UnityEvent onFalse = null;


		protected override void OnValueChanged(ValueChangedEvent<bool> evt)
		{
			evt.value = negate ? !evt.value : evt.value;

			base.OnValueChanged(evt);

			if (evt.value)
			{
				onTrue.Invoke();
			}
			else
			{
				onFalse.Invoke();
			}
		}
	}
}
