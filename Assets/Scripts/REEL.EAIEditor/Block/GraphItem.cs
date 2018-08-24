using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class GraphItem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] private NodeType nodeType;

        [SerializeField] private EditorManager.ETargetMenuType targetMenuType;

        private RectTransform refRectTransform;
        private Vector3 originPosition;

        protected MenuPopup targetPopup;

        [SerializeField]
        private object data;

        // block placed id.
        [SerializeField] private int blockID = -1;

        private Image image;
        private Color normalColor;
        [SerializeField] private Color selectedColor = Color.yellow;

        protected void Awake()
        {
            Init();
        }

        private void Init()
        {
            refRectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            normalColor = image.color;
            targetPopup = EditorManager.Instance.GetTargetMenuObject(targetMenuType);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            originPosition = refRectTransform.position;

            // Set Selected Node Block.
            if (BlockDiagramManager.Instance.GetCurrentSelectedCount == 0 
                || KeyInputManager.Instance.shouldMultiSelect)
            {
                BlockDiagramManager.Instance.SetSelectedGraphItem(this);
            }

            //else if (!KeyInputManager.Instance.shouldMultiSelect 
            //    && BlockDiagramManager.Instance.GetCurrentSelectedCount > 0)
            //{
            //    BlockDiagramManager.Instance.SetOneSelected(this);
            //}
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IfMoved)
            {
                return;
            }

            if (!KeyInputManager.Instance.shouldMultiSelect
                && BlockDiagramManager.Instance.GetCurrentSelectedCount > 0)
            {
                BlockDiagramManager.Instance.SetOneSelected(this);
            }

            if (!KeyInputManager.Instance.shouldMultiSelect 
                && eventData.clickCount == 2)
            {
                if (targetPopup != null) targetPopup.ShowPopup(this);
            }
        }

        public void SetSelected()
        {
            image.color = selectedColor;
        }

        public void SetUnselected()
        {
            image.color = normalColor;
        }

        public int BlockID
        {
            get { return blockID; }
            set { blockID = value; }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (IfMoved) return;
            //if (targetPopup != null) targetPopup.ShowPopup(this);
        }

        public virtual void SetItemData(object data)
        {
            this.data = data;
            //Debug.Log("item name: " + data);
        }

        public virtual object GetItemData()
        {
            return this.data;
        }

        bool IfMoved { get { return refRectTransform.position != originPosition; } }
        public Rect GetRect { get { return refRectTransform.rect; } }
        public NodeType GetNodeType {  get { return nodeType; } }

        public ExecutePoint GetExecutePoint(ExecutePoint.PointPosition pointPosition)
        {
            int childCount = transform.childCount;
            for (int ix = 0; ix < childCount; ++ix)
            {
                ExecutePoint executePoint = transform.GetChild(ix).GetComponent<ExecutePoint>();
                if (executePoint != null && executePoint.GetPointPosition == pointPosition)
                    return executePoint;
            }

            return null;
        }

        public ExecutePoint GetExecutePoint(ExecutePoint.PointPosition pointPosition, int executePointID)
        {
            int childCount = transform.childCount;
            for (int ix = 0; ix < childCount; ++ix)
            {
                ExecutePoint[] executePoints = transform.GetChild(ix).GetComponentsInChildren<ExecutePoint>(true);

                for (int jx = 0; jx < executePoints.Length; ++jx)
                {
                    ExecutePoint current = executePoints[jx];
                    if (current != null &&  current.GetPointPosition == pointPosition && current.GetExecutePointID.Equals(executePointID))
                    {
                        current.SetExecutePointEnabled(true);
                        return current;
                    }
                }
            }

            return null;
        }
    }
}