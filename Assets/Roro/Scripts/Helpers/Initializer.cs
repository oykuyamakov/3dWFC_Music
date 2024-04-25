using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Roro.Scripts.Serialization;
using Roro.Scripts.SettingImplementations;
using SceneManagement;
using UnityCommon.Variables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roro.Scripts.Helpers
{
	public class Initializer : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup m_IntroCanvas;
		
		[SerializeField]
		private List<SceneId> m_ScenesToLoad = new()
		{  SceneId.Shared, SceneId.Loading };
		
		[SerializeField] 
		private BoolVariable m_StartedFromIntro;
		
		public void Start()
		{
			StartCoroutine(InitializeAsync());

			m_StartedFromIntro.Value = true;
		}


		private IEnumerator InitializeAsync()
		{
			yield return null;
			
			yield return new WaitForSeconds(1.5f);

			var ctx = SerializationWizard.Default;

			yield return LoadScenes();
		}

		private IEnumerator LoadScenes()
		{
			float p0 = 0f;
			float ps = 1f / m_ScenesToLoad.Count;
		
			Application.backgroundLoadingPriority = ThreadPriority.High;
		
			for (var i = 0; i < m_ScenesToLoad.Count; i++)
			{
				var sceneId = m_ScenesToLoad[i];
				var sceneName = sceneId.ToString();
		
				yield return null;
		
				var asyncOp = SceneManager.LoadSceneAsync(
					sceneName, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));
		
				float p = p0;
				//progress = p;
				while (!asyncOp.isDone)
				{
					p = p0 + asyncOp.progress * ps;
					//progress = p;
					yield return null;
				}
		
				yield return null;
				yield return null;
		
				p = p0 + 1f * ps;
				//progress = p;
		
				p0 = p;
			}
		
			yield return null;
			yield return null;

			var sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
			sceneTransitionManager.OnAllInitialScenesLoaded();
			
			yield return new WaitForSeconds(GeneralSettings.Get().IntroWaitDuration);
			
			m_IntroCanvas.DOFade(0, GeneralSettings.Get().IntroWaitDuration/3).SetEase(Ease.InQuad);
			
			yield return new WaitForSeconds(GeneralSettings.Get().IntroWaitDuration/2);

			Application.backgroundLoadingPriority = ThreadPriority.Low;
			
			Resources.UnloadUnusedAssets();
			System.GC.Collect();

			yield return null;
			yield return null;			
			yield return null;
			
			SceneManager.UnloadSceneAsync("Intro");
			
		}
	}
}
