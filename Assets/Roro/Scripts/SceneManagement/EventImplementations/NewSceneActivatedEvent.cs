using Events;
using UnityEngine;

namespace SceneManagement.EventImplementations
{
    public class NewSceneActivatedEvent : Event<NewSceneActivatedEvent>
    {
        public SceneId NewSceneId;
        public static NewSceneActivatedEvent Get(SceneId newSceneId)
        {
            var evt = GetPooledInternal();
            evt.NewSceneId = newSceneId;
            return evt;
        }
    }
}