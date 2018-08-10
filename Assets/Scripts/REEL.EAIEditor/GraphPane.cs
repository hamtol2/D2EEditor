using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class GraphPane : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Transform blockPane;

        public void AddNodeItem(GameObject itemPrefab, Vector3 itemPosition, NodeType nodeType)
        {
            // Create Block on to block pane.
            GameObject newObj = Instantiate(itemPrefab);
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.SetParent(blockPane);
            rectTransform.position = itemPosition;
            rectTransform.localScale = Vector3.one;

            // Add Block Information to Block Diagram Manager.
            NodeBlock newBlock = new NodeBlock() { nodeType = nodeType, position = itemPosition };
            newObj.GetComponent<DragItem>().SubscribeOnUpdate(newBlock.UpdatePosition);
            int blockID = BlockDiagramManager.Instance.AddBlock(newBlock);
            newObj.GetComponent<GraphItem>().BlockID = blockID;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BlockDiagramManager.Instance.SetSelectedGraphItem(null);
        }
    }
}