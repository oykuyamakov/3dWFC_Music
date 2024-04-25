using DG.Tweening;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace Utility.Extensions
{
    public static class UtilExtensions
    {
        public static void Toggle(this CanvasGroup canvasGroup, bool visible, float duration)
        {
            canvasGroup.DOFade(visible ? 1 : 0, duration);
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }

        /// <summary>
        /// if denominator is 0, returns numerator
        /// </summary>
        /// <param name="Numerator"></param>
        /// <param name="Denominator"></param>
        /// <returns></returns>
        public static float SafeDivision(this float Numerator, float Denominator)
        {
            return (Denominator == 0) ? Numerator : Numerator / Denominator;
        }

        public static void DoScaleAnim(this Transform t, float scale, float duration)
        {
            var originalScale = t.localScale;

            t.DOScale(scale, duration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                t.DOScale(originalScale, duration).SetEase(Ease.OutBack);
            });
        }


        public static float GetDistanceFromPoint(this Transform t, Vector3 origin)
        {
            Vector3 directionToTarget = t.position.WithY(origin.y) - origin;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            return dSqrToTarget;
        }

        public static Transform CompareDistanceToPoint(Transform t1, Transform t2, Vector3 origin)
        {
            var t1Dist = t1.GetDistanceFromPoint(origin);
            var t2Dist = t1.GetDistanceFromPoint(origin);

            return t1Dist < t2Dist ? t1 : t2;
        }

        public static Transform GetClosestToPoint(Vector3 origin, float closestDistanceSqr, Transform transform)
        {
            return transform.GetDistanceFromPoint(origin) < closestDistanceSqr ? transform : null;
        }

        public static void AddMaterial(this MeshRenderer renderer, Material material)
        {
            var materials = renderer.sharedMaterials;
            var newMaterials = new Material[materials.Length + 1];

            materials.CopyTo(newMaterials, 0);
            newMaterials[^1] = material;
            renderer.sharedMaterials = newMaterials;
        }

        public static void RemoveMaterial(this MeshRenderer renderer, Material material)
        {
            var materials = renderer.sharedMaterials;
            var newMaterials = new Material[materials.Length - 1];
            
            var index = 0;
            foreach (var mat in materials)
            {
                if (mat == material) continue;
                newMaterials[index] = mat;
                index++;
            }

            renderer.sharedMaterials = newMaterials;
        }
    }
}