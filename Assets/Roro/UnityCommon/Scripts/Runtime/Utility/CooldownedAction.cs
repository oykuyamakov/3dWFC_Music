using System;
using UnityEngine;

namespace Roro.UnityCommon.Scripts.Runtime.Utility
{
    public class CooldownedAction : IAction
    {
        public float Remaining { get => execDur; set => execDur = value; }
        public float Period { get => cooldown; set => cooldown = value; }

        private Action action;
        private Action onStartCooldown;
        private float countdown;
        private float cooldown;
        private float execDur;
        private float execInitial;
        private float initialDelay;

        private bool onPause;
        
        public CooldownedAction(Action action, float initialDelay,float cooldown, float execDur, Action onStartCooldown)
        {
            this.action = action;
            this.onStartCooldown = onStartCooldown;
            this.initialDelay = initialDelay;
            this.countdown = initialDelay;
            this.cooldown = cooldown;
            this.execInitial = execDur;
            this.execDur = execDur;
        }
        
        public void Update(float dt)
        {
            while (countdown < 0f)
            { 
                while (execDur > 0f)
                {
                    execDur -= dt;
                    action.Invoke();
                    
                    return;
                }
                
                execDur += execInitial;
                countdown += cooldown;
                
                onStartCooldown.Invoke();

                return;
            }
            
            countdown -= dt;

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
            countdown = initialDelay;
        }

        public void ResetToPeriod()
        {
            countdown = cooldown;
        }

        public void ResetToInitialDelay()
        {
            countdown = initialDelay;
        }
    }
}