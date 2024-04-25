using Events;
using UnityEngine;

namespace Roro.Scripts.GameManagement.EventImplementations
{
    public class LevelEndEvent : Event<LevelEndEvent>
    {
        public bool Win;
        public int LevelPoint;
        public static LevelEndEvent Get(bool result)
        {
            var evt = GetPooledInternal();
            evt.Win = result;
            return evt;
        }
    }
}
