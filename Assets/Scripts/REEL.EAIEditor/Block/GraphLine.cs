using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    public class GraphLine : MonoBehaviour
    {
        public struct LinePoint
        {
            public Vector2 start;
            public Vector2 end;

            public LinePoint(Vector2 start, Vector2 end)
            {
                this.start = start;
                this.end = end;
            }

            public override string ToString()
            {
                return start.ToString() + ", " + end.ToString();
            }
        }

        public struct ExecutePointInfo
        {
            public int blockID;
            public int executePointID;

            public ExecutePointInfo(int blockID, int executePointID)
            {
                this.blockID = blockID;
                this.executePointID = executePointID;
            }
        }

        private ExecutePoint left;
        private ExecutePoint right;

        private ExecutePointInfo leftExecutePointInfo;
        private ExecutePointInfo rightExecutePointInfo;

        private LinePoint linePoint;
        private Image lineImage;

        private bool isSet = false;

        [SerializeField] private float lineHeight = 5f;

        private Color normalColor;
        [SerializeField] private Color selectedColor = Color.green;

        private void Awake()
        {
            lineImage = GetComponent<Image>();
            normalColor = lineImage.color;
            BlockDiagramManager.Instance.AddLine(this);
        }

        private void UpdateLine()
        {
            if (isSet)
            {
                linePoint.start = left.transform.position;
                linePoint.end = right.transform.position;
                SetLinePoint(linePoint);
            }
        }

        public void SetExecutePoints(ExecutePoint left, ExecutePoint right)
        {
            this.left = left;
            this.right = right;
            isSet = true;

            leftExecutePointInfo = new ExecutePointInfo(
                left.GetTargetDragItem().GetComponent<GraphItem>().BlockID,
                left.GetExecutePointID
            );

            rightExecutePointInfo = new ExecutePointInfo(
                right.GetTargetDragItem().GetComponent<GraphItem>().BlockID,
                right.GetExecutePointID
            );

            // Subscribe dragitem's OnChanged event to update line.
            left.GetTargetDragItem().SubscribeOnChanged(UpdateLine);
            right.GetTargetDragItem().SubscribeOnChanged(UpdateLine);
        }

        private void OnDestroy()
        {
            if (left != null && left.GetTargetDragItem() != null)
            {
                left.GetTargetDragItem().UnsubscribeOnChanged(UpdateLine);
                left.SetHasLine(false);
            }
                
            if (right != null && right.GetTargetDragItem() != null)
            {
                right.GetTargetDragItem().UnsubscribeOnChanged(UpdateLine);
                right.SetHasLine(false);
            }

            if (BlockDiagramManager.Instance != null) BlockDiagramManager.Instance.RemoveLine(this);
        }

        public void SetLinePoint(LinePoint linePoint)
        {
            this.linePoint = linePoint;
            SetLineAngle();
            SetLineLength();
        }

        public void SetSelected()
        {
            lineImage.color = selectedColor;
        }

        public void SetUnselected()
        {
            lineImage.color = normalColor;
        }

        void SetLineAngle()
        {
            float angle = Util.GetAngleBetween(linePoint.start, linePoint.end);
            lineImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        void SetLineLength()
        {
            float length = Util.GetDistanceBetween(linePoint.start, linePoint.end);
            lineImage.rectTransform.sizeDelta = new Vector2(length, lineHeight);
        }

        public ExecutePointInfo GetLeftExecutePointInfo { get { return leftExecutePointInfo; } }
        public ExecutePointInfo GetRightExecutePointInfo { get { return rightExecutePointInfo; } }

        public LinePoint GetLinePoint {  get { return linePoint; } }
    }
}