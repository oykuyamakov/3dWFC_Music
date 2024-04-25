using UnityEngine;

namespace UnityCommon.Runtime.Extensions
{
    public static class TransformExtensions
    {
        public static void SetPositionX(this Transform t, float val)
        {
            t.position = t.position.WithX(val);
        }

        public static void SetPositionY(this Transform t, float val)
        {
            t.position = t.position.WithY(val);
        }

        public static void SetPositionZ(this Transform t, float val)
        {
            t.position = t.position.WithZ(val);
        }

        public static void SetLocalPositionX(this Transform t, float val)
        {
            t.localPosition = t.localPosition.WithX(val);
        }

        public static void SetLocalPositionY(this Transform t, float val)
        {
            t.localPosition = t.localPosition.WithY(val);
        }

        public static void SetLocalPositionZ(this Transform t, float val)
        {
            t.localPosition = t.localPosition.WithZ(val);
        }

        public static void AddPositionX(this Transform t, float val)
        {
            t.position = t.position.WithXRelative(val);
        }

        public static void AddPositionY(this Transform t, float val)
        {
            t.position = t.position.WithYRelative(val);
        }

        public static void AddPositionZ(this Transform t, float val)
        {
            t.position = t.position.WithZRelative(val);
        }

        public static void SetScaleX(this Transform t, float val)
        {
            t.localScale = t.localScale.WithX(val);
        }

        public static void SetScaleY(this Transform t, float val)
        {
            t.localScale = t.localScale.WithY(val);
        }

        public static void SetScaleZ(this Transform t, float val)
        {
            t.localScale = t.localScale.WithZ(val);
        }

        public static void SetScaleXY(this Transform t, float x, float y) => t.localScale = t.localScale.WithXY(x, y);
        public static void SetScaleYZ(this Transform t, float y, float z) => t.localScale = t.localScale.WithYZ(y, z);
        public static void SetScaleXZ(this Transform t, float x, float z) => t.localScale = t.localScale.WithXZ(x, z);

        public static void AddScaleX(this Transform t, float val)
        {
            t.localScale = t.localScale.WithXRelative(val);
        }

        public static void AddScaleY(this Transform t, float val)
        {
            t.localScale = t.localScale.WithYRelative(val);
        }

        public static void AddScaleZ(this Transform t, float val)
        {
            t.localScale = t.localScale.WithZRelative(val);
        }

        public static void AddScaleXClamped(this Transform t, float val, float clampVal)
        {
            var scale = t.localScale;
            var localScale = scale;
            scale = localScale.WithYRelative(val);
            scale = localScale.WithY(Mathf.Clamp(scale.y, 0, clampVal));
            t.localScale = scale;
        }
        
        public static void AddScaleYClamped(this Transform t, float val, float clampVal)
        {
            var scale = t.localScale;
            var localScale = scale;
            scale = localScale.WithYRelative(val);
            scale = localScale.WithY(Mathf.Clamp(scale.y, 0, clampVal));
            t.localScale = scale;
        }
        
        public static void AddScaleZClamped(this Transform t, float val, float clampVal)
        {
            var scale = t.localScale;
            var localScale = scale;
            scale = localScale.WithYRelative(val);
            scale = localScale.WithY(Mathf.Clamp(scale.y, 0, clampVal));
            t.localScale = scale;
        }
    }
}