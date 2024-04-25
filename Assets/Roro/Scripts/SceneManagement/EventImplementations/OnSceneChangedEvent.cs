using Events;
using UnityEngine;

namespace SceneManagement.EventImplementations
{
    public class OnSceneChangedEvent : Event<OnSceneChangedEvent>
    {
        public SceneId newScene;
        public SceneId transitionScene;
        public bool animate = true;
        public static OnSceneChangedEvent Get(SceneId newScene, SceneId transitionScene)
        {
            var evt = GetPooledInternal();
            evt.newScene = newScene;
            evt.transitionScene = transitionScene;
            return evt;
        }
    }
}
