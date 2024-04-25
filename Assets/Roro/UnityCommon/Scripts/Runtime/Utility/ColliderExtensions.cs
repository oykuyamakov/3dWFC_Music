using UnityCommon.Utilities;
using UnityEngine;

namespace UnityCommon.Runtime.Utility
{
    public static class ColliderExtensions
    {
        public static bool GetMouseDown(this Collider collider)
        {
            return InputUtility.GetMouseDown() &&
                   collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000f);
        }

        public static bool GetMouseHold(this Collider collider)
        {
            return Input.GetMouseButton(0) &&
                   collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000f);
        }

        public static bool GetMouseOver(this Collider collider)
        {
            return collider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000f);
        }

        public static Vector3 GetRandomPointInBounds(this Collider collider)
        {
            var bounds = collider.bounds;

            float minX = bounds.size.x * -0.5f;
            float minY = bounds.size.y * -0.5f;
            float minZ = bounds.size.z * -0.5f;

            return collider.transform.TransformPoint(
                new Vector3(Random.Range(minX, -minX),
                    Random.Range(minY, -minY),
                    Random.Range(minZ, -minZ))
            );
        }
    }
}