using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class VariableNodeWindow : MonoBehaviour, IShowProperty
	{
        [SerializeField] private Text nodeTypeText;
        [SerializeField] private Text nodeIDText;
        [SerializeField] private InputField nodeTitleInput;
        [SerializeField] private InputField nodeNameInput;
        [SerializeField] private Dropdown nodeOperatorDropdown;
        [SerializeField] private InputField nodeValueInput;
        [SerializeField] private Text nextIDText;

        [SerializeField] private VariableItem variableNode;

        private void OnDisable()
        {
            variableNode = null;
            nodeTitleInput.onValueChanged.RemoveAllListeners();
            nodeNameInput.onValueChanged.RemoveAllListeners();
            nodeOperatorDropdown.onValueChanged.RemoveAllListeners();
            nodeValueInput.onValueChanged.RemoveAllListeners();
        }

        public void ShowProperty(GraphItem node)
        {
            gameObject.SetActive(true);

            variableNode = node as VariableItem;
            nodeTitleInput.text = variableNode.GetBlockTitle;
            nodeNameInput.text = variableNode.GetBlockName;

            nodeTitleInput.onValueChanged.AddListener(variableNode.SetBlockTitle);
            nodeNameInput.onValueChanged.AddListener(variableNode.SetBlockName);

            nodeOperatorDropdown.value = (int)variableNode.GetOperatorType;
            nodeOperatorDropdown.onValueChanged.AddListener(variableNode.SetOperatorType);

            nodeTypeText.text = node.GetNodeType.ToString();
            nodeIDText.text = node.BlockID.ToString();

            nodeValueInput.text = node.GetItemData() != null ? node.GetItemData().ToString() : "";
            nodeValueInput.onValueChanged.AddListener(variableNode.SetItemData);

            nextIDText.text = node.GetNextBlockID.ToString();
        }
    }
}