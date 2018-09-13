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

        public void ShowProperty(GraphItem node)
        {
            gameObject.SetActive(true);

            nodeTypeText.text = node.GetNodeType.ToString();
            nodeIDText.text = node.BlockID.ToString();
            nodeValueInput.text = node.GetItemData() != null ? node.GetItemData().ToString() : "";
            nextIDText.text = node.GetNextBlockID.ToString();
        }
    }
}