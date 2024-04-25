using Sirenix.OdinInspector;
using UnityCommon.Runtime.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityCommon.Runtime.Utility
{
	public class SceneLoader : MonoBehaviour
	{
		public bool useBuildIndex = false;

		[HideIf("$useBuildIndex")]
		public string sceneName = "Main";

		[ShowIf("$useBuildIndex")]
		public int buildIndex = 1;

		public bool doFade = true;

		[SerializeField] private Button b;
		
		public void Load()
		{
			b.enabled = false;
			if (doFade)
			{
				FadeInOut.Instance.DoTransition(() =>
				{
					if (useBuildIndex)
					{
						SceneManager.LoadScene(buildIndex);
						SceneManager.LoadScene(buildIndex + 1,LoadSceneMode.Additive);
					}
					else
					{
						SceneManager.LoadScene(sceneName);
					}

					b.enabled = true;
				});
			}
			else
			{
				if (useBuildIndex)
				{
					SceneManager.LoadScene(buildIndex);
				}
				else
				{
					SceneManager.LoadScene(sceneName);
				}
			}
		}
	}
}
