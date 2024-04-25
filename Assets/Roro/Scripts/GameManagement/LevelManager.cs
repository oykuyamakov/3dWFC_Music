using Events;
using Roro.Scripts.GameManagement.EventImplementations;
using Roro.Scripts.Serialization;
using UnityCommon.Variables;
using UnityEngine;

namespace Roro.Scripts.GameManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private BoolVariable m_GameIsRunning;
        
        private static SerializationWizard m_SerializationWizard => SerializationWizard.Default;

        private static int m_Level
        {
            get => m_SerializationWizard.GetInt("level", 0);
            set => m_SerializationWizard.SetInt("level", value);
        }

        private float m_Duration;

        private bool m_StartedTimer = false;

        private long m_FinishTime;

        private Coroutine m_TimerRoutine;

        private void Awake()
        {
            GEM.AddListener<LevelEndEvent>(OnLevelEnd);
            GEM.AddListener<LevelStartEvent>(OnLevelStart);

        }

        private void InitializeLevel()
        {
        }

        private void OnDestroy()
        {
            GEM.RemoveListener<LevelEndEvent>(OnLevelEnd);
            GEM.RemoveListener<LevelStartEvent>(OnLevelStart);
        }

        private void OnLevelEnd(LevelEndEvent evt)
        {
        }

        private void OnLevelStart(LevelStartEvent evt)
        {

        }
    }
}