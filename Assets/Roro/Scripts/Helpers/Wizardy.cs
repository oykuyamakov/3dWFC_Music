using System.Collections.Generic;
using UnityEngine;

namespace Roro.Scripts.Helpers
{
    public static class Wizardy 
    {
        public static int GetRandomRangeExcept(int max, params int[] excepts)
        {
            List<int> vars = new List<int>();
            for (int i = 0; i < max; i++)
            {
                if (!CheckIfExistsIn(i, excepts))
                {
                    vars.Add(i);
                }
            }
            return vars[Random.Range(0, vars.Count)];
        }
        public static int GetRandomRangeExcept(int min, int max, int[] excepts)
        {
            List<int> vars = new List<int>();
            for (int i = min; i < max; i++)
            {
                if (!CheckIfExistsIn(i, excepts))
                {
                    vars.Add(i);
                }
            }
            return vars.Count == 0 ? 0 : vars[Random.Range(0, vars.Count)];
        } 
        public static int GetRandomRangeExcept(List<int> values, params int[] excepts)
        {
            List<int> vars = new List<int>();
            for (int i = 0; i < values.Count; i++)
            {
                if (!CheckIfExistsIn(values[i], excepts))
                {
                    vars.Add(values[i]);
                }
            }
            return vars[Random.Range(0, vars.Count)];
        }

        public static bool CheckIfExistsIn(int isIn, params int[] inside)
        {
            for (int i = 0; i < inside.Length; i++)
            {
                if (inside[i] == isIn)
                    return true;
            }

            return false;
        }
    }
}
