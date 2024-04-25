using System;
using Roro.Scripts.StateImplementation;
using UnityCommon.Modules;
using UnityEngine;

namespace Utility
{
    public class BoolState : State<bool>
    {
        public BoolState(bool def = false, Action onChange = null, SpecialCondition<bool> condition = null) : base(def)
        {
            DefaultValue = def;
            CurrentValue = DefaultValue;
            OnChange = onChange;
            
            AddCondition(condition);
        }

        public override void CheckConditions()
        {
            for (int i = 0; i < SpecialConditions.Count; i++)
            {
                var cond = SpecialConditions[i];
                Debug.Log(cond.Method);
                switch (cond.Method)
                {
                    case ComparisionMethod.Equal:
                    {
                        if (cond.ConditionValue == CurrentValue)
                        {
                            cond.OnCondition?.Invoke();
                        }
                    }
                        break;
                    case ComparisionMethod.NonEqual:
                    {
                        if (cond.ConditionValue != CurrentValue)
                        {
                            cond.OnCondition?.Invoke();
                        }
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
