using Events;
using UnityEngine;

namespace SceneManagement.EventImplementations
{
    public class GetSceneControllerEvent : Event<GetSceneControllerEvent>
    {
        public SceneController Controller;
        public SceneId SceneId;
        public static GetSceneControllerEvent Get(SceneId sceneId)
        {
            var evt = GetPooledInternal();
            evt.SceneId = sceneId;
            return evt;
        }
    }
}
