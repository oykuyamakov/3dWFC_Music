using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;

namespace UI
{
    public class FlexibleLayoutGroup : LayoutGroup
    {
        public enum FitType
        {
            Height,
            Width,
            Uniform,
            FixedRows,
            FixedColumns
        }

        public FitType fitType;
        [Min(1)]
        public int rows;
        [Min(1)]
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        [ShowIf("@fitType == FitType.FixedColumns || fitType == FitType.FixedRows")]
        public bool fitY;
        [ShowIf("@fitType == FitType.FixedColumns || fitType == FitType.FixedRows")]
        public bool fitX;
        

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
        
            float sqrt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrt);
            columns = Mathf.CeilToInt(sqrt);
        
            if(fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }
        
            if(fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }
            if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }
            
            var rect = rectTransform.rect;
            var parentWidth = rect.width;
            var parentHeight = rect.height;
        
            var p = padding;
            var cellWidth = parentWidth / columns - ((spacing.x / columns) * (columns-1)) - (p.left / (float)columns) - (p.right / (float)columns);
            var cellHeight = parentHeight / rows - ((spacing.y / rows) * (rows-1)) - (p.top / (float) rows) - (p.bottom / (float) rows);
        
            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;
        
            var columnCount = 0;
            var rowCount = 0;
        
            for (var i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;
        
                var item = rectChildren[i];
        
                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + p.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + p.top;
                
                SetChildAlongAxis(item,0,xPos,cellSize.x);
                SetChildAlongAxis(item,1,yPos,cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            throw new NotImplementedException();
        }

        public override void SetLayoutHorizontal()
        {
           
        }

        public override void SetLayoutVertical()
        {
        }
    }
}
