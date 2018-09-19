using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class IfNodeWindow : MonoBehaviour, IShowProperty
    {
        [SerializeField] private Text nodeTypeText;
        [SerializeField] private Text nodeIDText;
        [SerializeField] private InputField nodeTitleInput;
        [SerializeField] private Dropdown ifTypeDrowpdown;
        [SerializeField] private InputField nodeNameInput;
        [SerializeField] private Dropdown operatorDropdown;
        [SerializeField] private InputField nodeValueInput;
        [SerializeField] private Text trueNextIDText;
        [SerializeField] private Text falseNextIDText;

        private IFBranchItem ifNode;

        private void OnDisable()
        {   
            nodeTitleInput.onValueChanged.RemoveAllListeners();
            ifTypeDrowpdown.onValueChanged.RemoveAllListeners();
            nodeNameInput.onValueChanged.RemoveAllListeners();
            operatorDropdown.onValueChanged.RemoveAllListeners();
            nodeValueInput.onValueChanged.RemoveAllListeners();
            trueNextIDText.text = falseNextIDText.text = "-1";
        }

        public void ShowProperty(GraphItem node)
        {   
            gameObject.SetActive(true);
            ifNode = node as IFBranchItem;
            BranchCondition data = ifNode.GetIFBranchData();

            nodeTitleInput.text = ifNode.GetBlockTitle;
            ifTypeDrowpdown.value = (int)data.lParamType;
            nodeNameInput.text = ifNode.GetBlockName;

            nodeTitleInput.onValueChanged.AddListener(ifNode.SetBlockTitle);
            ifTypeDrowpdown.onValueChanged.AddListener(ifNode.SetLParamType);
            nodeNameInput.onValueChanged.AddListener(ifNode.SetBlockName);

            operatorDropdown.value = (int)data.opParameter;
            operatorDropdown.onValueChanged.AddListener(ifNode.SetOpParamType);

            nodeTypeText.text = node.GetNodeType.ToString();
            nodeIDText.text = node.BlockID.ToString();
            nodeValueInput.text = data.rParameter;
            nodeValueInput.onValueChanged.AddListener(ifNode.SetRParamValue);

            int nextID = ifNode.GetTrueExecutePoint.GetLineData != null ? ifNode.GetTrueExecutePoint.GetLineData.GetRightExecutePointInfo.blockID : -1;
            trueNextIDText.text = nextID.ToString();

            nextID = ifNode.GetFalseExecutePoint.GetLineData != null ? ifNode.GetFalseExecutePoint.GetLineData.GetRightExecutePointInfo.blockID : -1;
            falseNextIDText.text = nextID.ToString();
        }
    }
}