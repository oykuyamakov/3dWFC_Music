using UnityEngine;
using UnityEngine.UI;

namespace Based.UI
{
    public static class ScrollRectExtensions
    {
        public static void DisableHiddenCanvases(this ScrollRect scrollRect, float DistanceToRecalcVisibility,
            float DistanceMarginForLoad)
        {
            var lastPos = scrollRect.content.transform.localPosition.y;
            scrollRect.onValueChanged.AddListener((newValue) =>
            {
                if (Mathf.Abs(lastPos - scrollRect.content.transform.localPosition.y) >= DistanceToRecalcVisibility)
                {
                    RectTransform scrollTransform = scrollRect.GetComponent<RectTransform>();
                    float checkRectMinY = scrollTransform.rect.yMin - DistanceMarginForLoad;
                    float checkRectMaxY = scrollTransform.rect.yMax + DistanceMarginForLoad;

                    foreach (Transform child in scrollRect.content)
                    {
                        if(!child.GetComponent<Canvas>())
                            return;
                        
                        RectTransform childTransform = child.GetComponent<RectTransform>();
                        Vector3 positionInWord = childTransform.parent.TransformPoint(childTransform.localPosition);
                        Vector3 positionInScroll = scrollTransform.InverseTransformPoint(positionInWord);
                        float childMinY = positionInScroll.y + childTransform.rect.yMin;
                        float childMaxY = positionInScroll.y + childTransform.rect.yMax;

                        if (childMaxY >= checkRectMinY && childMinY <= checkRectMaxY)
                        {
                            child.GetComponent<Canvas>().enabled = true;
                        }
                        else
                        {
                            child.GetComponent<Canvas>().enabled = false;
                        }
                    }
                }
            });
        }  
        public static void DisableHiddenContentChild(this ScrollRect scrollRect, float DistanceToRecalcVisibility,
            float DistanceMarginForLoad)
        {
            var lastPos = scrollRect.content.transform.localPosition.y;
            scrollRect.onValueChanged.AddListener((newValue) =>
            {
                if (Mathf.Abs(lastPos - scrollRect.content.transform.localPosition.y) >= DistanceToRecalcVisibility)
                {
                    RectTransform scrollTransform = scrollRect.GetComponent<RectTransform>();
                    float checkRectMinY = scrollTransform.rect.yMin - DistanceMarginForLoad;
                    float checkRectMaxY = scrollTransform.rect.yMax + DistanceMarginForLoad;

                    foreach (Transform child in scrollRect.content)
                    {
                        if(child.childCount < 1)
                            return;
                        
                        RectTransform childTransform = child.GetComponent<RectTransform>();
                        Vector3 positionInWord = childTransform.parent.TransformPoint(childTransform.localPosition);
                        Vector3 positionInScroll = scrollTransform.InverseTransformPoint(positionInWord);
                        float childMinY = positionInScroll.y + childTransform.rect.yMin;
                        float childMaxY = positionInScroll.y + childTransform.rect.yMax;

                        if (childMaxY >= checkRectMinY && childMinY <= checkRectMaxY)
                        {
                            if (child.GetComponent<ContentSizeFitter>())
                                child.GetComponent<ContentSizeFitter>().enabled = true;
                            child.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        { 
                            if (child.GetComponent<ContentSizeFitter>())
                                child.GetComponent<ContentSizeFitter>().enabled = false;
                            child.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                }
            });
        }
    }
}