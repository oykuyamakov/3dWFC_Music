using System.Collections;
using Sirenix.OdinInspector;
using UnityCommon.Variables;
using UnityEngine;

namespace Events
{
	public class EventRaiser : MonoBehaviour
	{

		public EventContext eventContext;

		public FloatReference delay;

		[Button(ButtonSizes.Medium, ButtonStyle.CompactBox)]
		public void Raise()
		{
			if (delay.Value < Mathf.Epsilon)
			{
				RaiseNow();
			}
			else
			{
				StartCoroutine(WaitAndRaise());
			}
		}


		[Button(ButtonSizes.Small, ButtonStyle.CompactBox)]
		public void RaiseNow()
		{
			eventContext.SendEvent();
		}


		private IEnumerator WaitAndRaise()
		{
			yield return new WaitForSecondsRealtime(delay.Value);

			RaiseNow();
		}


	}
}
