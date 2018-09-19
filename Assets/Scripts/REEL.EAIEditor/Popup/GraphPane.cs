using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class GraphPane : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField]
        private Transform blockPane = null;

        public void AddNodeItem(GameObject itemPrefab, GraphItem node, Vector3 nodePos, int blockID)
        {
            // Create Block on to block pane.
            GameObject newObj = Instantiate(itemPrefab);
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.SetParent(blockPane);
            rectTransform.position = nodePos;
            rectTransform.localScale = Vector3.one;
            GraphItem graphItem = newObj.GetComponent<GraphItem>();
            graphItem.BlockID = blockID;
            graphItem.SetBlockTitle(node.GetBlockTitle);
            graphItem.SetItemData(node.GetItemData());

            if (node.GetNodeType == NodeType.SWITCH)
            {
                SwitchBranchItem switchNode = graphItem as SwitchBranchItem;
                SwitchBranchItem givenSwitchNode = node as SwitchBranchItem;
                switchNode.SetBlockCount(givenSwitchNode.GetBlockCount);
                switchNode.SetSwitchType(givenSwitchNode.GetSwitchType);
                switchNode.SetSwitchName(givenSwitchNode.GetSwitchName);

                for (int ix = 1; ix < givenSwitchNode.GetBlockCount + 1; ++ix)
                {
                    if (switchNode.executePoints[ix] is ExecuteCasePoint)
                    {
                        ExecuteCasePoint casePoint = switchNode.executePoints[ix] as ExecuteCasePoint;
                        casePoint.CaseValue = (givenSwitchNode.executePoints[ix] as ExecuteCasePoint).CaseValue;
                    }
                }
            }

            else if (node.GetNodeType == NodeType.VARIABLE)
            {
                VariableItem variableNode = graphItem as VariableItem;
                VariableItem givenVariableNode = node as VariableItem;
                variableNode.SetBlockName(givenVariableNode.GetBlockName);
                variableNode.SetOperatorType(givenVariableNode.GetOperatorType);
            }

            // Add Block Information to Block Diagram Manager.
            WorkspaceManager.Instance.AddBlock(graphItem);
        }

        public void AddNodeItem(GameObject itemPrefab, NodeBlock nodeData)
        {
            // Create Block on to block pane.
            GameObject newObj = Instantiate(itemPrefab);
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.SetParent(blockPane);
            rectTransform.position = nodeData.position;
            rectTransform.localScale = Vector3.one;
            GraphItem graphItem = newObj.GetComponent<GraphItem>();
            graphItem.BlockID = nodeData.id;
            graphItem.SetBlockTitle(nodeData.title);
            graphItem.SetItemData(nodeData.value);

            if (nodeData.nodeType == NodeType.SWITCH)
            {
                SwitchBranchItem switchNode = graphItem as SwitchBranchItem;
                switchNode.SetSwitchName(nodeData.name);
                switchNode.SetBlockCount(nodeData.switchBlockCount);

                for (int ix = 1; ix < nodeData.switchBlockCount + 1; ++ix)
                {
                    if (switchNode.executePoints[ix] is ExecuteCasePoint)
                    {
                        ExecuteCasePoint casePoint = switchNode.executePoints[ix] as ExecuteCasePoint;
                        casePoint.CaseValue = nodeData.switchBlockValues[ix - 1];
                    }
                }
            }

            else if (nodeData.nodeType == NodeType.VARIABLE)
            {
                VariableItem variableNode = graphItem as VariableItem;
                variableNode.SetBlockName(nodeData.name);
                variableNode.SetBlockName(nodeData.name);
                variableNode.SetOperatorType(nodeData.variableOperator);
            }

            else if (nodeData.nodeType == NodeType.IF)
            {
                IFBranchItem ifNode = graphItem as IFBranchItem;
                ifNode.SetBlockName(nodeData.name);
                ifNode.SetOpParamType((int)ifNode.GetOpTypeFromString(nodeData.variableOperator));
                ifNode.SetRParamValue(nodeData.value);
                ifNode.SetLParamType((int)nodeData.ifBranchType);
            }

            // Add Block Information to Block Diagram Manager.
            WorkspaceManager.Instance.AddBlock(graphItem);
        }

        public void AddNodeItem(GameObject itemPrefab, Vector3 itemPosition, NodeType nodeType, int nodeID = -1)
        {
            // Create Block on to block pane.
            GameObject newObj = Instantiate(itemPrefab);
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.SetParent(blockPane);
            rectTransform.position = itemPosition;
            rectTransform.localScale = Vector3.one;
            GraphItem graphItem = newObj.GetComponent<GraphItem>();
            graphItem.BlockID = nodeID;

            // Add Block Information to Block Diagram Manager.
            WorkspaceManager.Instance.AddBlock(graphItem);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //BlockDiagramManager.Instance.SetAllUnselected();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (KeyInputManager.Instance.isShiftPressed) return;

            WorkspaceManager.Instance.SetAllUnselected();
        }
    }
}