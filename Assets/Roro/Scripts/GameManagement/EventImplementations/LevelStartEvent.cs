using Events;
using UnityEngine;

namespace Roro.Scripts.GameManagement.EventImplementations
{
    public class LevelStartEvent : Event<LevelStartEvent>
    {
        public bool Result;

        public static LevelStartEvent Get(bool result)
        {
            var evt = GetPooledInternal();
            evt.Result = result;
            return evt;
        }
    }
}