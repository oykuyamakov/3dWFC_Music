using Sirenix.OdinInspector;
using UnityCommon.Variables;
using UnityEngine;

namespace UnityCommon.Runtime.Animators
{


	[CreateAssetMenu(menuName = "Variables/Animator Parameter Hash", fileName = "Hash")]
	public class AnimatorParameterHash : Variable<string>
	{

		[ReadOnly] [SerializeField] private int animatorHash;
		public int Hash { get => animatorHash; private set => animatorHash = value; }

		private bool isUsedOnce = false;

		private bool boolValue;

		public override bool CanBeBoundToPlayerPrefs() => false;

		public override void OnInspectorChanged()
		{
			base.OnInspectorChanged();

			if (string.IsNullOrEmpty(this.value))
				return;


			Hash = Animator.StringToHash(this.value);
		}


		public void Reset()
		{
			isUsedOnce = false;
		}


		public void SetTrigger(Animator animator)
		{
			animator.SetTrigger(this.animatorHash);
		}

		public void SetBool(Animator animator, bool value)
		{

			if (isUsedOnce)
			{
				if (boolValue == value)
				{
					return;
				}
			}

			animator.SetBool(this.animatorHash, value);

			boolValue = value;

			isUsedOnce = true;
		}


	}

}
