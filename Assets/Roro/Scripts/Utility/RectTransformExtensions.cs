using System.Collections.Generic;
using UnityEngine;

namespace Roro.Scripts.Utility
{
    public static class RectTransformExtensions
    {
        public static bool Intersects(this Rect r1, Rect r2, out Rect area)
        {
            area = new Rect();
 
            if (r2.Overlaps(r1))
            {
                float x1 = Mathf.Min(r1.xMax, r2.xMax);
                float x2 = Mathf.Max(r1.xMin, r2.xMin);
                float y1 = Mathf.Min(r1.yMax, r2.yMax);
                float y2 = Mathf.Max(r1.yMin, r2.yMin);
                area.x = Mathf.Min(x1, x2);
                area.y = Mathf.Min(y1, y2);
                area.width = Mathf.Max(0.0f, x1 - x2);
                area.height = Mathf.Max(0.0f, y1 - y2);
           
                return true;
            }
 
            return false;
        } 
        public static bool Intersects2(this Rect r1, Rect r2, out Rect area)
        {
            area = new Rect();
            if (r2.Overlaps(r1))
            {
                List<Vector2> container = new List<Vector2>();

                Debug.Log(r2.xMax + " " +r2.xMin);
                List<Vector2> points = new List<Vector2>();
                List<Vector2> points2 = new List<Vector2>();
                var r2MaxX = r2.x + r2.width;
                var r2MinX = r2.x;  
                var r2MaxY = r2.y + r2.width;
                var r2MinY = r2.y;

                var r2DR = new Vector2(r2MaxX, r2MinY);
                var r2DL = new Vector2(r2MinX, r2MinY);
                var r2UR = new Vector2(r2MaxX, r2MaxY);
                var r2UL = new Vector2(r2MinX, r2MaxY);
                
                var r1MaxX = r1.x + r1.width;
                var r1MinX = r1.x;
                var r1MaxY = r1.y + r1.width;
                var r1MinY = r1.y;
                
                var r1DR = new Vector2(r1MaxX, r1MinY);
                var r1DL = new Vector2(r1MinX, r1MinY);
                var r1UR = new Vector2(r1MaxX, r1MaxY);
                var r1UL = new Vector2(r1MinX, r1MaxY);
                
                points.Add(r2DR);
                points.Add(r2DL);
                points.Add(r2UR);
                points.Add(r2UL);
                
                points2.Add(r1DR);
                points2.Add(r1DL);
                points2.Add(r1UR);
                points2.Add(r1UL);
                Debug.Log(points.Count);
                Debug.Log(points2.Count);

                var realPoints = new List<Vector2>();
                for (int i = 0; i < points.Count; i++)
                {
                    Debug.Log(i);
                    if(r1.Contains(points[i]))
                        realPoints.Add(points[i]);
                   
                    if(r2.Contains(points2[i]))
                        realPoints.Add(points2[i]);   
                }
                
                
                area.x = Mathf.Min(realPoints[0].x, realPoints[1].x);
                area.y = Mathf.Min(realPoints[0].y, realPoints[1].y);
                area.width = Mathf.Abs(Mathf.Max(0.0f, realPoints[0].x - realPoints[1].x));
                area.height = Mathf.Abs(Mathf.Max(0.0f, realPoints[0].y - realPoints[1].y));
                
                return true;
            }
            Debug.Log("falses");
 
            
            return false;
            
        }
        
        public static void StretchToParent(this RectTransform rt)
        {
            rt.pivot = Vector2.one * 0.5f;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public static void CenterPivot(this RectTransform rt)
        {
            var size = rt.rect.size;
            rt.pivot = Vector2.one * 0.5f;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
        }
		
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            if (rectTransform == null) return;
 
            Vector2 size = rectTransform.rect.size;
            Vector2 deltaPivot = rectTransform.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }
        
    }
}