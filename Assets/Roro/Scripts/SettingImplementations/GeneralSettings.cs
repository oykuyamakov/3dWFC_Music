using System;
using SceneManagement;
using UnityEngine;

namespace Roro.Scripts.SettingImplementations
{
    [CreateAssetMenu(fileName =" GeneralSettings" )]
    public class GeneralSettings : ScriptableObject
    {
        private static GeneralSettings _GeneralSettings;

        private static GeneralSettings generalSettings
        {
            get
            {
                if (!_GeneralSettings)
                {
                    _GeneralSettings = Resources.Load<GeneralSettings>($"Settings/GeneralSettings");

                    if (!_GeneralSettings)
                    {
#if UNITY_EDITOR
                        Debug.Log("General Settings not found AND NOT creating and a new one");
                        //_GeneralSettings = CreateInstance<GeneralSettings>();
                        // var path = "Assets/Resources/Settings/GeneralSettings.asset";
                        // AssetDatabaseHelpers.CreateAssetMkdir(_GeneralSettings, path);
#else
 				//		throw new Exception("Global settings could not be loaded");
#endif
                    }
                }

                return _GeneralSettings;
            }
        }
        
        public static GeneralSettings Get()
        {
            return generalSettings;
        }
        
        public float SceneTransitionDuration = 2f;
        
        public float IntroWaitDuration = 2f;
        
        public float LoadingFadeInDuration = 0.8f;
        public float LoadingFadeOutDuration = 0.8f;

        #region InGame

        public float PlatformSpeed = 1f;
        
        public float GravityChangeSpeed = 1f;

        public float GravityMagnitude = 2f;

        #endregion



    }

    [Serializable]
    public class ScreenShakeValues
    {
        public float Duration = 0;
        public float Magnitude = 1;
    }

    [Serializable]
    public struct SceneToScene
    {
        public SceneId From;
        public SceneId To;
    }
}
