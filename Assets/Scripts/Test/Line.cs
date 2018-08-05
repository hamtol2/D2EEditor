using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace REEL.Test
{
    public class Line : MonoBehaviour
    {
        public enum LineType
        {
            Top, Left, Right, Bottom
        }

        public class LinePoint
        {
            public Vector2 start;
            public Vector2 end;
        }

        public LineType lineType = LineType.Top;

        private Image image;
        private RectTransform rectTransform;
        private LinePoint linePoint;

        private void Awake()
        {
            linePoint = new LinePoint();
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        public Vector2 StartPoint
        {
            get { return linePoint.start; }
        }

        public Vector2 EndPoint
        {
            get { return linePoint.end; }
        }

        public void SetStartPoint(Vector2 start)
        {
            linePoint.start = start;
        }

        public void SetEndPoint(Vector2 end)
        {
            Vector2 length = linePoint.end - linePoint.start;
            if (lineType == LineType.Top || lineType == LineType.Left)
            {
                linePoint.end = end;

                if (lineType == LineType.Top)
                    rectTransform.sizeDelta = new Vector2(length.magnitude, rectTransform.sizeDelta.y);
                else if (lineType == LineType.Left)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, length.magnitude);
            }
            else if (lineType == LineType.Bottom|| lineType == LineType.Right)
            {
                transform.position = Camera.main.ScreenToWorldPoint(end);

                if (lineType == LineType.Bottom)
                    rectTransform.sizeDelta = new Vector2(length.magnitude, rectTransform.sizeDelta.y);
                else if (lineType == LineType.Right)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, length.magnitude);
            }
        }
    }
}