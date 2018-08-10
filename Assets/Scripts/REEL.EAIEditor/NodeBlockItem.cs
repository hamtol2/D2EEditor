using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using EPaneType = REEL.EAIEditor.EditorManager.EPaneType;

namespace REEL.EAIEditor
{
    public enum NodeType
    {
        Event, Hearing, Say, If, Switch, Length
    }

    public class NodeBlockItem : DragItem
    {
        public NodeType nodeType;
        public GameObject itemButtonPrefab;

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

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