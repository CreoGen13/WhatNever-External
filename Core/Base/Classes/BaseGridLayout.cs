using UnityEngine;
using UnityEngine.UI;

namespace Core.Base.Classes
{
    public class BaseGridLayout : LayoutGroup
    {
        private enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }
        [SerializeField] private FitType fitType;
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector2 spacing;

        private bool fitX = false;
        private bool fitY = false;
        
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.Height || fitType == FitType.Width || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                float sqrt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrt);
                columns = Mathf.CeilToInt(sqrt);
            }
            
            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            }
            else if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);
            }
            
            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = parentWidth / columns - spacing.x * 2 / columns - padding.left / (float) columns - padding.right / (float) columns;
            float cellHeight = parentHeight / rows- spacing.y * 2 / rows - padding.top / (float) rows - padding.bottom / (float) rows;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];
                
                var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
                var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;
                
                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }
        
        public override void CalculateLayoutInputVertical()
        {
            //throw new System.NotImplementedException();
        }

        public override void SetLayoutHorizontal()
        {
            //throw new System.NotImplementedException();
        }

        public override void SetLayoutVertical()
        {
            //throw new System.NotImplementedException();
        }
    }
}