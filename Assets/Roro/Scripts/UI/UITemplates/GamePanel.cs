using System;
using Events;
using Roro.Scripts.GameManagement.EventImplementations;
using UnityCommon.Runtime.UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.UI.UITemplates
{
    [RequireComponent(typeof(UIAnimation))]
    public class GamePanel : MonoBehaviour
    {
        [SerializeField]
        protected GameState m_GameState;

        private UIAnimation m_Anim => GetComponent<UIAnimation>();

        private void Awake()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {
            switch (m_GameState)
            {
                case GameState.Intro:
                    OnShow(null);
                    break;
                case GameState.Win:
                    GEM.AddListener<LevelWinEvent>(OnShow);
                    break;
                case GameState.Fail:                    
                    GEM.AddListener<LevelFailEvent>(OnShow);
                    break;
                case GameState.Start:
                    GEM.AddListener<LevelStartEvent>(OnShow);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            switch (m_GameState)
            {
                case GameState.Intro:
                    break;
                case GameState.Win:
                    GEM.RemoveListener<LevelWinEvent>(OnShow);
                    break;
                case GameState.Fail:                    
                    GEM.RemoveListener<LevelFailEvent>(OnShow);
                    break;
                case GameState.Start:
                    GEM.RemoveListener<LevelStartEvent>(OnShow);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnShow(object obj)
        {
            if (TryGetComponent<Image>(out var im))
            {
                im.raycastTarget = true;
            }
            
            m_Anim.FadeIn();
        }

        protected virtual void OnHide()
        {
            m_Anim.FadeOut();
            
            if (TryGetComponent<Image>(out var im))
            {
                im.raycastTarget = false;
            }

            if (m_GameState != GameState.Start)
            {
                using var gameStart = LevelStartEvent.Get().SendGlobal();
            }
        }
    }
    
    public enum GameState
    {
        Intro,
        Win,
        Fail,
        Start
    }
}
