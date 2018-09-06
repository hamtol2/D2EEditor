using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
    public class ExecutePoint : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        public enum PointPosition
        {
            ExecutePoint_Left, ExecutePoint_Right
        }

        [SerializeField] protected PointPosition pointPosition = PointPosition.ExecutePoint_Left;
        [SerializeField] protected GameObject lineBasePrefab = null;
        [SerializeField] protected GameObject executePointRoot;

        protected GraphLine graphLine;
        protected GraphLine.LinePoint linePoint;
        protected RectTransform refRectTransform;

        private DragItem dragItem;

        [SerializeField] protected bool hasLine = false;
        [SerializeField] private int executePointID = 0;

        protected virtual void Awake()
        {
            dragItem = GetComponentInParent<DragItem>();
            refRectTransform = GetComponent<RectTransform>();
            linePoint = new GraphLine.LinePoint(Vector2.zero, Vector2.zero);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            List<RaycastResult> result = new List<RaycastResult>();
            EditorManager.Instance.UIRaycaster.Raycast(eventData, result);

            if (pointPosition == PointPosition.ExecutePoint_Left)
            {
                if (SetLine(result, PointPosition.ExecutePoint_Right)) return;
            }
            else if (pointPosition == PointPosition.ExecutePoint_Right)
            {
                if (SetLine(result, PointPosition.ExecutePoint_Left)) return;
            }

            if (!hasLine && graphLine)
                Destroy(graphLine.gameObject);
        }

        public GraphLine SetLineData(ExecutePoint rightExecutePoint)
        {
            if (graphLine == null)
            {
                graphLine = CreateLineGO().GetComponent<GraphLine>();
            }

            linePoint.start = SelfPosition;
            linePoint.end = rightExecutePoint.GetComponent<RectTransform>().position;
            graphLine.SetLinePoint(linePoint);
            graphLine.SetExecutePoints(this, rightExecutePoint);

            SetHasLine(true);
            rightExecutePoint.SetHasLine(true);

            return graphLine;
        }

        private bool SetLine(List<RaycastResult> result, PointPosition position)
        {
            GameObject targetLinePosition = IsOnExecutePoint(result, position);
            if (targetLinePosition && graphLine)
            {
                SetLineData(targetLinePosition.GetComponent<ExecutePoint>());
                return true;
            }

            return false;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (KeyInputManager.Instance.isAltPressed && graphLine)
            {
                WorkspaceManager.Instance.RemoveLine(graphLine);
                return;
            }

            if (hasLine) return;

            // create and set graph line.
            if (HasDragItem) dragItem.SetEnableDrag(false);

            GameObject newObj = CreateLineGO();
            graphLine = newObj.GetComponent<GraphLine>();
        }

        private GameObject CreateLineGO()
        {
            GameObject newObj = Instantiate(lineBasePrefab);
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = Vector3.zero;
            newObj.GetComponent<Image>().canvas.sortingLayerName = "GraphLine";

            return newObj;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (hasLine) return;

            if (graphLine)
            {
                linePoint.start = SelfPosition;
                linePoint.end = eventData.position;
                graphLine.SetLinePoint(linePoint);
            }
        }

        protected bool HasDragItem { get { return dragItem != null; } }
        protected Vector2 SelfPosition { get { return refRectTransform.position; } }

        public PointPosition GetPointPosition { get { return pointPosition; } }
        public int GetExecutePointID { get { return executePointID; } }

        public DragItem GetTargetDragItem()
        {
            return dragItem;
        }

        public void SetHasLine(bool hasLine)
        {
            this.hasLine = hasLine;
        }

        public bool GetHasLineState {  get { return hasLine; } }

        public void SetExecutePointEnabled(bool isEnable)
        {
            
            if (executePointRoot == null)
            {
                gameObject.SetActive(isEnable);
            }   
            else
            {
                executePointRoot.SetActive(isEnable);
            }   
        }

        protected GameObject IsOnExecutePoint(List<RaycastResult> rayResults, PointPosition point)
        {
            for (int ix = 0; ix < rayResults.Count; ++ix)
            {
                if (Util.CompareTwoStrings(rayResults[ix].gameObject.tag, point.ToString()))
                    return rayResults[ix].gameObject;
            }

            return null;
        }
    }
}