namespace Roro.UnityCommon.Scripts.Runtime.Utility
{
    public interface IAction
    {
        public float Remaining { get; set; }
        public float Period { get; set; }
        
        public void Update(float dt);

        public void Execute();
        
        public void ResetToPeriod();

        public void ResetToInitialDelay();

        public void Pause();

        public void Resume();

        public void Reset();
    }
}