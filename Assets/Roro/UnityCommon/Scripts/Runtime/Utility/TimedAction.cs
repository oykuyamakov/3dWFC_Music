using System;
using Roro.UnityCommon.Scripts.Runtime.Utility;
using UnityEngine;


namespace UnityCommon.Runtime.Utility
{
    public class TimedAction : IAction
    {
        public float Remaining { get => countdown; set => countdown = value; }
        public float Period { get => period; set => period = value; }

        private Action action;
        private float countdown;
        private float period;

        private bool onPause;
        
        protected float initialDelay;

        public TimedAction(Action action, float countdown, float period)
        {
            this.action = action;
            this.initialDelay = countdown;
            this.countdown = countdown;
            this.period = period;
            this.onPause = false;
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
            }
        }

        public void Pause()
        {
            onPause = true;
        }

        public void Resume()
        {
            ResetToPeriod();
            onPause = false;
        }

        public void Execute()
        {
            action.Invoke();
        }

        public void ResetToPeriod()
        {
            countdown = period;
        }

        public void Reset()
        {
            countdown = initialDelay;
        }
        public void ResetToInitialDelay()
        {
            countdown = initialDelay;
        }

    }
}