using UnityEngine;

namespace UnityCommon.Runtime.Utility
{

	public class TargetFrameRateSetter : MonoBehaviour
	{

		[SerializeField] private int value = 60;

		private void Awake()
		{
			Application.targetFrameRate = value;
		}


	}

}
