using UnityEngine;

namespace REEL.EAIEditor
{
    public class GraphPane : MonoBehaviour
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
            BlockDiagramManager.Instance.AddBlock(newBlock);
        }
    }
}