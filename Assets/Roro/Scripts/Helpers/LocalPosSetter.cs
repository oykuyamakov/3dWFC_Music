using Sirenix.OdinInspector;
using UnityEngine;

namespace Roro.Scripts.Helpers
{
    public class LocalPosSetter : MonoBehaviour
    {
        [Button]
        private void Set()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
