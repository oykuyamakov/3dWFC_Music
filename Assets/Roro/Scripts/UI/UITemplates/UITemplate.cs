using System;
using UnityEngine;

namespace Roro.Scripts.UI.UITemplates
{
    [Serializable]
    public abstract class UITemplate<T>: MonoBehaviour
    {
        public abstract void Set(T val);
        public abstract void Enable();
        public abstract void Disable();
        
        public abstract UIElementType GetElementType();
    }
}
