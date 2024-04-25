using System.Collections;
using UnityCommon.Modules;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommon.Runtime.Utility
{
	public class DelayedInvoke : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent onExecute = null;

		[SerializeField]
		private FloatReference delay = default;

		[SerializeField]
		private bool onAwake = true;

		private void Awake()
		{
			if (onAwake)
			{
				Invoke();
			}
		}

		public void Invoke()
		{
			if (delay.Value < 1e-3f)
			{
				onExecute.Invoke();
			}
			else
			{
				Conditional.Wait(delay.Value).Do(() => { onExecute?.Invoke(); });
			}
		}


		private IEnumerator WaitAndExecute()
		{
			yield return new WaitForSeconds(delay.Value);

			onExecute.Invoke();
		}
	}
}
