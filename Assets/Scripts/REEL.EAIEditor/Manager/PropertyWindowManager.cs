using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class PropertyWindowManager : Singleton<PropertyWindowManager>
	{
        [SerializeField] private GeneralNodeWindow generalNodeWindow;
        [SerializeField] private SwitchNodeWindow switchNodeWindow;
        [SerializeField] private VariableNodeWindow variableNodeWindow;

        private List<GameObject> windows = new List<GameObject>();

        private void OnEnable()
        {
            windows.Add(generalNodeWindow.gameObject);
            windows.Add(switchNodeWindow.gameObject);
            windows.Add(variableNodeWindow.gameObject);
        }

        public void ShowProperty(GraphItem node)
        {
            //Debug.Log("ShowProperty: " + node);

            TurnOffAll();

            switch (node.GetNodeType)
            {
                case NodeType.SWITCH: switchNodeWindow.ShowProperty(node); break;
                case NodeType.VARIABLE: variableNodeWindow.ShowProperty(node); break;
                default: generalNodeWindow.ShowProperty(node); break;
            }
        }

        public void TurnOffAll()
        {
            foreach (GameObject obj in windows)
            {
                obj.SetActive(false);
            }
        }
    }

    public interface IShowProperty
    {
        void ShowProperty(GraphItem node);
    }
}