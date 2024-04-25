using Events;
using UnityEngine;

namespace Roro.Scripts.GameManagement.EventImplementations
{
    public class LevelFailEvent : Event<LevelFailEvent>
    {
        public static LevelFailEvent Get(bool result)
        {
            var evt = GetPooledInternal();
            return evt;
        }
    }
}