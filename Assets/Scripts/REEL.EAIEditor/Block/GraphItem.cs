using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class GraphItem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] private NodeType nodeType = NodeType.START;

        [SerializeField] private EditorManager.ETargetMenuType targetMenuType = EditorManager.ETargetMenuType.Event;

        private RectTransform refRectTransform;
        private Vector3 originPosition;

        protected MenuPopup targetPopup;

        [SerializeField]
        private object data;

        // block placed id.
        [SerializeField] private int blockID = -1;

        [SerializeField] private string blockTitle;
        

        private Image image;
        private Color normalColor;
        [SerializeField] private Color selectedColor = Color.yellow;

        public ExecutePoint[] executePoints;

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
            if (WorkspaceManager.Instance.GetCurrentSelectedBlockCount == 0 
                || KeyInputManager.Instance.shouldMultiSelect)
            {
                WorkspaceManager.Instance.SetSelectedGraphItem(this);
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
                && WorkspaceManager.Instance.GetCurrentSelectedBlockCount > 0)
            {
                WorkspaceManager.Instance.SetOneSelected(this);

                // Test.
                PropertyWindowManager.Instance.ShowProperty(this);
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

        public XMLNode GetXMLNormalData()
        {
            XMLNode xmlNode = new XMLNode();
            xmlNode.nodeType = GetNodeType;
            xmlNode.nodeID = BlockID;
            xmlNode.nodeValue = GetItemData() == null ? "" : GetItemData().ToString();
            xmlNode.nodeTitle = GetBlockTitle;
            xmlNode.nextID = -1;

            if (executePoints != null || executePoints.Length > 0)
            {
                foreach (ExecutePoint executePoint in executePoints)
                {
                    if (executePoint.GetPointPosition == ExecutePoint.PointPosition.ExecutePoint_Right)
                    {
                        if (executePoint.GetLineData)
                        {
                            xmlNode.nextID = executePoint.GetLineData.GetRightExecutePointInfo.blockID;
                        }
                    }
                }
            }

            return xmlNode;
        }

        public void SetBlockTitle(string title)
        {
            blockTitle = title;
        }

        public string GetBlockTitle { get { return blockTitle; } }

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

        public int GetNextBlockID
        {
            get
            {
                foreach (ExecutePoint point in executePoints)
                {
                    if (point.GetPointPosition == ExecutePoint.PointPosition.ExecutePoint_Right && point.GetHasLineState)
                        return point.GetLineData.GetRightExecutePointInfo.blockID;
                }

                return -1;
            }
        }
    }
}