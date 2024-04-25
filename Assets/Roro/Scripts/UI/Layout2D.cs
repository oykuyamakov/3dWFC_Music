using System.Linq;
using Sirenix.OdinInspector;
using UnityCommon.Runtime.Extensions;
using UnityEngine;

namespace Roro.Scripts.Helpers
{
    public class Layout2D : MonoBehaviour
    {
        [SerializeField]
        private float offset;

        [SerializeField]
        private float offsetX;
        [SerializeField]
        private float offsetStart;
        
        private Transform t;
        
        [Button]
        private void OnLayout()
        {
            var w = transform.localScale.x;
            var h = transform.localScale.y;

            var children = transform.Children();
            var k = 1;
            var pos = transform.position;

            var cW = (w / children.Count())/w;
            var xStart = pos.x - w / 2f;

            Debug.Log(cW);
            Debug.Log(w);
            Debug.Log(children.Count());

            var cH = (h - offset) / h;
            foreach (var child in children)
            {
                child.transform.localScale = new Vector2(cW, cH);
                var sl = child.transform.localScale;
                child.transform.position = new Vector3(xStart + (cW/2  * k), pos.y);
                //child.transform.localPosition = new Vector3( -(sl.x/2f ) +sl.x/4 +(k * (sl.x/4)), 0);
                k++;
            }

        }
    }
}
