using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using EPaneType = REEL.EAIEditor.EditorManager.EPaneType;

namespace REEL.EAIEditor
{
    //public enum NodeType
    //{
    //    START, EVENT, HEARING, SAY, IF, SWITCH, STT, FACIAL, MOTION, VARIABLE, TTS, Length
    //}

    public enum NodeType
    {
        START, SWITCH, STT, TTS, FACIAL, MOTION, VARIABLE, Length
    }

    public class NodeBlockItem : DragItem
    {
        public NodeType nodeType;
        public GameObject itemButtonPrefab;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!canDrag) return;

            WorkspaceManager.Instance.SetAllUnselected();

            originPosition = refRectTransform.position;
            SetDragOffset(eventData.position);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;

            ChangePosition(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (WorkspaceManager.Instance.GetTabManager.CurrentTabCount == 0)
            {
                SetToOrigin();
                return;
            }   

            List<RaycastResult> result = new List<RaycastResult>();
            EditorManager.Instance.UIRaycaster.Raycast(eventData, result);

            if (IsOnGraphPane(result) && itemButtonPrefab)
            {
                GameObject paneObj = EditorManager.Instance.GetPaneObject(EPaneType.Graph_Pane);
                GraphPane pane = paneObj.GetComponent<GraphPane>();
                pane.AddNodeItem(itemButtonPrefab, eventData.position, nodeType);
            }

            SetToOrigin();
        }

        private bool IsOnGraphPane(List<RaycastResult> rayResults)
        {
            for (int ix = 0; ix < rayResults.Count; ++ix)
            {
                if (Util.CompareTwoStrings(rayResults[ix].gameObject.tag, EPaneType.Graph_Pane.ToString()))
                    return true;
            }

            return false;
        }
    }
}