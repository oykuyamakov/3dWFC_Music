using Events;
using UnityEngine;

namespace Roro.Scripts.GameManagement.EventImplementations
{
    public class LevelWinEvent : Event<LevelWinEvent>
    {
        public static LevelWinEvent Get(bool result)
        {
            var evt = GetPooledInternal();
            return evt;
        }
    }
}