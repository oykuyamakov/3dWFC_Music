using UnityEngine;

namespace Roro.Scripts.Utility
{
    public static class GOExtensions
    {
        public static bool TryGetComponentInChildren<T>(this GameObject g, out T var)
        {
            return (var = g.GetComponentInChildren<T>()) == null;
        }
    }
}
