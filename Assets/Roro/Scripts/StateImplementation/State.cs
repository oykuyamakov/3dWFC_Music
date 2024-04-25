using System;
using System.Collections.Generic;
using System.Linq;
using UnityCommon.Modules;
using UnityEngine;

namespace Roro.Scripts.StateImplementation
{
    public abstract class State<T>
    {
        protected State(T def, SpecialCondition<T> specialCondition = null, Action onChange= null)
        {
            Initialize(def, specialCondition, onChange);
        }
        public T DefaultValue { get ; set ; }
        public T CurrentValue { get; protected set; }
        public Conditional ToggleCond { get; set; }

        protected List<Conditional> Conditions = new List<Conditional>();
        public Action OnChange { get; set; }
        public float Remaining => ToggleCond?.remaining ?? 0;

        protected List<SpecialCondition<T>> SpecialConditions = new List<SpecialCondition<T>>();
        public bool Conditioned => SpecialConditions.Any();
        public abstract void CheckConditions();
        private void Initialize(T def, SpecialCondition<T> specialCondition, Action onChange = null)
        {
            DefaultValue = def;
            OnChange = onChange;
            CurrentValue = DefaultValue;
            
            AddCondition(specialCondition);
        }
        public virtual void Toggle(T value, float dur, Action onOnceComplete = null, Action onToggleDur = null)
        {
            if(ToggleCond != null)
            {  
                if (dur < Remaining)
                {
                    Debug.Log("tried to toggle while on toggling, reset will be ignored");

                    var cond = Conditional.For(dur).Do(() => onToggleDur?.Invoke()).OnComplete(()=>
                    {
                        onOnceComplete?.Invoke();
                    });
                    Conditions.Add(cond);
                
                    return;
                }
            }
            
            
            Change(value);
            //
            ToggleCond = Conditional.For(dur).Do(() => onToggleDur?.Invoke()).OnComplete(()=>
            {
                Reset();
                onOnceComplete?.Invoke();
            });
        }
        public bool Compare(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }
        public virtual void Change(T value)
        {
            if (Compare(CurrentValue,value))
            {
                //Debug.Log("tried to update with the same value");
                return;
            }
            
            OnChange?.Invoke();

            if(Conditioned)
                CheckConditions();
            
            CurrentValue = value;
        }
        public virtual void Reset()
        {
            ToggleCond?.Cancel();
            
            Conditions.ForEach(cond => cond.Cancel());
            
            //CHANGE SHOULD BE CALLED BEFORE CORRECTING SPECIAL CONDITIONS
            Change(DefaultValue);
            
            SpecialConditions.ForEach(sCon => sCon.OnceChecked = false);
            
        }
        public void AddCondition(T val, Action action, ComparisionMethod method = ComparisionMethod.Equal)
        {
            var cond = new SpecialCondition<T>(val, action);
            AddCondition(cond);
        }
        public void AddCondition(SpecialCondition<T> specialCondition)
        {
            if (specialCondition == null)
            {
                return;
            }
            SpecialConditions.Add(specialCondition);
        }
    }
    public class SpecialCondition<T>
    {
        public SpecialCondition(T conditional, Action onCond, ComparisionMethod method = ComparisionMethod.Equal)
        {
            ConditionValue = conditional;
            OnCondition = onCond;
            Method = method;
        }

        public readonly T ConditionValue;
        public readonly Action OnCondition;
        public readonly ComparisionMethod Method;
        public bool OnceChecked;
    }
    public enum ComparisionMethod
    {
        BiggerOnce,
        Bigger,
        Equal,
        Lesser,
        LesserOnce,
        NonEqual,
        LesserOrEqualOnce,
    } 
}
