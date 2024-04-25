using System;

namespace Roro.UnityCommon.Scripts.Runtime.Utility
{
    [Serializable]
    public class RepeatValues
    {
        public RepeatValues( float cooldown,float arbitraryCount,float rate ,float initialDelay )
        {
            this.Cooldown = cooldown;
            this.ArbitraryCount = arbitraryCount;
            this.Rate = rate;
            this.InitialDelay = initialDelay;
        }

        public float Cooldown = 1;
        public float ArbitraryCount = 1;
        public float Rate = 1;
        public float InitialDelay = 1;
    }
}