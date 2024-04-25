using Events;

namespace Roro.Scripts.Sounds
{
    public class SoundStopEvent : Event<SoundStopEvent>
    {
        public int LoopIndex;
        public static SoundStopEvent Get(int loopIndex)
        {
            var evt = GetPooledInternal();
            evt.LoopIndex = loopIndex;
            return evt;
        }
    }
}