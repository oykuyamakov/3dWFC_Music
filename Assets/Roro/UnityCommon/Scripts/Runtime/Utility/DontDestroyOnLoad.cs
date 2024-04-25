using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
