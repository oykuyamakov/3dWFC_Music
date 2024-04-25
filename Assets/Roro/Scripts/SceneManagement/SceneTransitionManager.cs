using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using Roro.Scripts.Serialization;
using Roro.Scripts.SettingImplementations;
using Roro.Scripts.Utility;
using SceneManagement.EventImplementations;
using Sirenix.OdinInspector;
using UnityCommon.Modules;
using UnityCommon.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    [System.Serializable]
    public enum SceneId
    {
        None = 0,
        Intro = 1,
        Menu = 2,
        Shared = 4,
        Loading = 64,
    }

    public static class SceneExtensions
    {
        public static Scene GetScene(this SceneId id)
        {
            return SceneManager.GetSceneByName(id.ToString());
        }
        
        public static string GetName(this SceneId id)
        {
            return id.ToString();
        }
    }

    public static class Shared
    {
        public static Camera MainCamera => m_Cam == null ? m_Cam = Camera.main : m_Cam;
        private static Camera m_Cam;
    }

    [DefaultExecutionOrder(ExecOrder.SceneManager)]
    public class SceneTransitionManager : SingletonBehaviour<SceneTransitionManager>
    {
        [SerializeField]
        private SceneId m_InitialScene = SceneId.Menu;

        private List<SceneController> m_SceneControllersList = new();
        private Dictionary<Scene, SceneController> m_SceneControllers = new();

        [ShowInInspector] 
        public SceneId CurrentSceneId => m_CurrentSceneId;
        
        private SceneId m_CurrentSceneId;
        private SceneId m_NextSceneId;
        private SceneId m_SceneToUnload;
        private SceneId m_CurrentTempSceneId;

        private SerializationWizard m_SerializationContext;

        public bool debug = false;

        private List<Action<SceneId>> m_SceneChangeListeners = new List<Action<SceneId>>();
        private List<SceneController> m_PermanentSceneControllers = new();
        
        private bool m_NextTempSceneLoaded;
        private bool m_LoadingTimePassed;
        private bool m_NextSceneActivated;

        private GeneralSettings m_Settings;
        
        public static float CurrentLoadProgress;

        private void Awake()
        {
            if (!SetupInstance(false))
                return;

            m_Settings = GeneralSettings.Get();
            m_NextTempSceneLoaded = false;
            m_SerializationContext = SerializationWizard.Default;

            GEM.AddListener<SceneChangeRequestEvent>(OnSceneChangeRequest);

            if (debug)
            {
                Conditional.WaitFrames(2).Do(OnAllInitialScenesLoaded);
            }
        }

        public void OnAllInitialScenesLoaded()
        {
            m_SceneControllersList = FindObjectsOfType<SceneController>().ToList();
            m_PermanentSceneControllers = FindObjectsOfType<SceneController>().ToList();
            m_SceneControllers = m_SceneControllersList.ToDictionary(act => act.gameObject.scene);

            ChangeScene(m_InitialScene, false);

            // var ctrl = m_SceneControllers[m_InitialScene.GetScene()];
            // ctrl.SetActiveState(true);
        }

        private void OnSceneChangeRequest(SceneChangeRequestEvent evt)
        {
            CurrentLoadProgress = 0;
            
            m_SerializationContext.Push();

            bool changed = false;

            changed = ChangeScene(evt.sceneId); 
            
            evt.result = changed ? EventResult.Positive : EventResult.Negative;
        }
        
        public bool ActivateScene(SceneId id, bool enableLoading)
        {
            if (TogglePermanentSceneControllers(id))
            {
                return true;
            }

            StartLoadingScene(id, enableLoading);

            return true;
        }

        private bool TogglePermanentSceneControllers(SceneId id)
        {
            for (int i = 0; i < m_PermanentSceneControllers.Count; i++)
            {
                var controller = m_PermanentSceneControllers[i];

                if (!controller.IsPermanent)
                {
                    continue;
                }

                if (controller.SceneId != id)
                    controller.TogglePermanentScene(false);
                else
                {
                    //if the scene to activate is permanent scene, set it active and return
                    controller.TogglePermanentScene(true);

                    var sceneName = controller.SceneName;
                    var sceneToLoad = SceneManager.GetSceneByName(sceneName);
                    SceneManager.SetActiveScene(sceneToLoad);

                    return true;
                }
            }

            return false;
        }

        private void StartLoadingScene(SceneId id, bool loadingEnabled)
        {
            if (loadingEnabled)
            {
                m_NextSceneId = id;
                StartCoroutine(EnableLoadingScene());

                //unload previous temp scene
                //Debug.Log($"Unload if there is a temp scene enabled : {m_CurrentTempSceneId.ToString()}");
                m_SceneToUnload = m_CurrentTempSceneId;
                StartCoroutine(UnLoadScene(m_SceneToUnload));
            }

            //Debug.Log( $"Next scene is  {id.ToString()} ");
            //load requested temp scene
            StartCoroutine(LoadScene(id, loadingEnabled));
        }
        
        public IEnumerator EnableLoadingScene()
        {
            ActivateScene(SceneId.Loading, false);

            yield return new WaitForSeconds(m_Settings.SceneTransitionDuration + 1);

            m_LoadingTimePassed = true;

            while (!m_NextTempSceneLoaded)
            {
                yield return null;
            }
            
            ActivateScene(m_NextSceneId, false);

            using var evt = NewSceneActivatedEvent.Get(m_NextSceneId);
            evt.SendGlobal();

            //if (m_NextSceneLoaded && m_NextSceneActivated)

            m_NextTempSceneLoaded = false;
        }

        
        public IEnumerator LoadScene(SceneId sceneId, bool waitForLoadingScene)
        {
            var sceneToLoad = sceneId.GetScene();
            
            if (sceneToLoad.IsValid())
            {
                StartCoroutine(OnSceneLoaded(sceneId, waitForLoadingScene));
                yield break; 
            }

            yield return null;

            yield return new WaitForSeconds(m_Settings.SceneTransitionDuration / 3f);

            var sceneName = sceneId.GetName();
            var asyncOp = SceneManager.LoadSceneAsync(
                sceneName, new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None));

            while (!asyncOp.isDone)
            {
                CurrentLoadProgress = asyncOp.progress;
                yield return null;
            }

            CurrentLoadProgress = 1f;
            yield return null;

            //Debug.Log($"{sceneId.ToString()} scene is loaded and will be activated after loading time passes" );

            yield return StartCoroutine(OnSceneLoaded(sceneId, waitForLoadingScene));
        }

        private IEnumerator OnSceneLoaded(SceneId sceneId, bool waitForLoadingScene)
        {
            m_NextTempSceneLoaded = true;

            m_CurrentTempSceneId = sceneId;

            if (waitForLoadingScene)
                yield break;

            SceneManager.SetActiveScene(sceneId.GetScene());
            
            var evt2 = GetSceneControllerEvent.Get(sceneId).SendGlobal();
            if (evt2.Controller != null)
            {
                evt2.Controller.TogglePermanentScene(true);
            }
        }
        
        public IEnumerator UnLoadScene(SceneId sceneId)
        {
            if (sceneId != SceneId.None)
            {
                var sceneToLoad = sceneId.GetScene();
                if (!sceneToLoad.IsValid())
                {
                    //Debug.Log($"Current temp scene : {sceneName} is not loaded?");
                }
                else
                {
                    yield return null;

                    var evt = GetSceneControllerEvent.Get(sceneId).SendGlobal();
                    if (evt.Controller != null)
                    {
                        evt.Controller.TogglePermanentScene(false);
                    }

                    SceneManager.UnloadSceneAsync(sceneToLoad);

                    yield return null;

                    //Debug.Log($"Unloaded : {sceneId.ToString()}" );
                }
            }
            else
            {
                Debug.Log("Scene ID cannot be \"None\", nothing to unload");

                yield return null;
            }
        }
        
        public bool ChangeScene(SceneId sceneId, bool enableLoading = true)
        {
            for (var i = 0; i < m_SceneChangeListeners.Count; i++)
            {
                var listener = m_SceneChangeListeners[i];
                listener?.Invoke(m_CurrentSceneId);
            }

            bool success = ActivateScene(sceneId, enableLoading);

            if (success)
            {
                using var sceneChangedEvt = OnSceneChangedEvent.Get(sceneId, m_CurrentSceneId).SendGlobal();
                m_CurrentSceneId = sceneId;
            }

            return success;
        }

        public void AddSceneChangeListener(Action<SceneId> action)
        {
            m_SceneChangeListeners.Add(action);
        }

        public void RemoveSceneChangeListener(Action<SceneId> action)
        {
            m_SceneChangeListeners.Remove(action);
        }
    }
}