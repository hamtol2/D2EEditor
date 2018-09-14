using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class GeneralNodeWindow : MonoBehaviour, IShowProperty
	{
        [SerializeField] private Text nodeTypeText;
        [SerializeField] private Text nodeIDText;
        [SerializeField] private InputField nodeTitleInput;
        [SerializeField] private InputField nodeValueInput;
        [SerializeField] private Text nextIDText;

        [SerializeField] private GraphItem normalNode;

        private void OnDisable()
        {
            normalNode = null;
            nodeTitleInput.onValueChanged.RemoveAllListeners();
            nodeValueInput.onValueChanged.RemoveAllListeners();
        }

        public void ShowProperty(GraphItem node)
        {
            gameObject.SetActive(true);

            nodeTitleInput.text = node.GetBlockTitle;

            normalNode = node;
            nodeTitleInput.onValueChanged.AddListener(normalNode.SetBlockTitle);

            nodeTypeText.text = node.GetNodeType.ToString();
            nodeIDText.text = node.BlockID.ToString();

            nodeValueInput.text = node.GetItemData() != null ? node.GetItemData() as string : string.Empty;
            nodeValueInput.onValueChanged.AddListener(normalNode.SetItemData);

            nextIDText.text = node.GetNextBlockID.ToString();
        }
    }
}