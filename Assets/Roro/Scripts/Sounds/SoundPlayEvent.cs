using Events;
using Roro.Scripts.Sounds.Data;

namespace Roro.Scripts.Sounds
{
    public class SoundPlayEvent : Event<SoundPlayEvent>
    {
        public Sound Sound;
        public bool Loop;
        public bool MainSong;
        public static SoundPlayEvent Get(Sound sound, bool loop = false, bool mainSong = false)
        {
            var evt = GetPooledInternal();
            evt.Sound = sound;
            evt.Loop = loop;
            evt.MainSong = mainSong;
            return evt;
        }
    }
}
