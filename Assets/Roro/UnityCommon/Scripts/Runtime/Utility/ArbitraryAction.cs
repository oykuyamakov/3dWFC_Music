using System;
using UnityCommon.Runtime.Utility;

namespace Roro.UnityCommon.Scripts.Runtime.Utility
{
    public class ArbitraryAction : IAction
    {
        public float Remaining { get => countdown; set => countdown = value; }
        public float Period { get => period; set => period = value; }

        private Action action;
        private float countdown;
        private float period;

        private bool onPause;

        private int actionCount;
        
        protected float initialDelay;

        public RepeatValues repeatValues;

        public ArbitraryAction(Action action, RepeatValues repeatValues)
        {
            this.action = action;
            this.repeatValues = repeatValues;
            this.initialDelay = repeatValues.InitialDelay;
            this.countdown = repeatValues.InitialDelay;
            this.period = repeatValues.Rate;
            this.onPause = false;
            actionCount = 0;
        }
        
        public void Update(float dt)
        {
            if(onPause)
                return;
            
            countdown -= dt;
            while (countdown < 0f)
            {
                countdown += period;
                action.Invoke();
                actionCount++;
            }

            if (actionCount > repeatValues.ArbitraryCount - 1)
            {
                period = repeatValues.Cooldown;
                actionCount = 0;
            }
            else
            {
                period = repeatValues.Rate;
            }
        }

        public void Pause()
        {
            onPause = true;
        }

        public void Resume()
        {
            onPause = false;
        }

        public void Execute()
        {
            action.Invoke();
        }

        public void Reset()
        {
            actionCount = 0;
            countdown = initialDelay;
            period = repeatValues.Rate;
        }

        public void ResetToPeriod()
        {
            countdown = period;
        }

        public void ResetToInitialDelay()
        {
            countdown = initialDelay;
        }
    }
}