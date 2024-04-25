using System;
using Roro.Scripts.StateImplementation;

namespace Utility
{
    public class FloatState : State<float>
    {
        public FloatState(float def = 0, SpecialCondition<float> condition = null, Action onChange = null) : base(def)
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
                switch (cond.Method)
                {
                    case ComparisionMethod.Bigger:
                    {
                        if (cond.ConditionValue < CurrentValue)
                        {
                            cond.OnCondition.Invoke();
                        }
                    }
                        break;
                    case ComparisionMethod.Equal:
                    {
                        if (cond.ConditionValue == CurrentValue)
                        {
                            cond.OnCondition.Invoke();
                        }
                    }
                        break;
                    case ComparisionMethod.Lesser:
                    {
                        if (cond.ConditionValue > CurrentValue)
                        {
                            cond.OnCondition.Invoke();
                        }
                    }
                        break;
                    case ComparisionMethod.BiggerOnce:
                        if (cond.ConditionValue < CurrentValue)
                        {
                            if(cond.OnceChecked)
                                break;
                            cond.OnCondition.Invoke();
                            cond.OnceChecked = true;
                            //SpecialConditions.Remove(cond);
                        }
                        else
                        {
                            cond.OnceChecked = false;
                        }

                        break;
                    case ComparisionMethod.LesserOnce:
                        if (cond.ConditionValue > CurrentValue)
                        {
                            if(cond.OnceChecked)
                                break;
                            cond.OnceChecked = true;
                            cond.OnCondition.Invoke();
                            //SpecialConditions.Remove(cond);
                        }
                        else
                        {
                            cond.OnceChecked = false;
                        }

                        break; 
                    case ComparisionMethod.LesserOrEqualOnce:
                        if (cond.ConditionValue >= CurrentValue)
                        {
                            if(cond.OnceChecked)
                                break;
                            cond.OnceChecked = true;
                            cond.OnCondition.Invoke();
                            //SpecialConditions.Remove(cond);
                        }
                        else
                        {
                            cond.OnceChecked = false;
                        }

                        break;
                    case ComparisionMethod.NonEqual:
                        if (cond.ConditionValue != CurrentValue)
                        {
                            cond.OnCondition.Invoke();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}