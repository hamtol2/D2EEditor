﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class GraphPane : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField]
        private Transform blockPane;

        public void AddNodeItem(GameObject itemPrefab, Vector3 itemPosition, NodeType nodeType, int nodeID = -1)
        {
            // Create Block on to block pane.
            GameObject newObj = Instantiate(itemPrefab);
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.SetParent(blockPane);
            rectTransform.position = itemPosition;
            rectTransform.localScale = Vector3.one;

            // Add Block Information to Block Diagram Manager.
            BlockDiagramManager.Instance.AddBlock(newObj.GetComponent<GraphItem>());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //BlockDiagramManager.Instance.SetAllUnselected();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            BlockDiagramManager.Instance.SetAllUnselected();
        }
    }
}