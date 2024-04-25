using UnityCommon.Variables;
using UnityEngine;

namespace UnityCommon.Runtime.Animators
{

	public class AnimatorParameterController : MonoBehaviour
	{


		[SerializeField] private AnimatorReference animator = null;


		public void SetTrigger(AnimatorParameterHash hash)
		{
			animator.Value.SetTrigger(hash.Hash);
		}

		public void SetFloat(AnimatorParameterHash hash, float value)
		{
			animator.Value.SetFloat(hash.Hash, value);
		}

		public void SetBool(AnimatorParameterHash hash, bool value)
		{
			animator.Value.SetBool(hash.Hash, value);
		}


	}

}
