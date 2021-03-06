﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class PropertyWindowManager : Singleton<PropertyWindowManager>
	{
        [SerializeField] private GeneralNodeWindow generalNodeWindow;
        [SerializeField] private SwitchNodeWindow switchNodeWindow;
        [SerializeField] private VariableNodeWindow variableNodeWindow;
        [SerializeField] private IfNodeWindow ifNodeWindow;

        private List<GameObject> windows = new List<GameObject>();

        private void OnEnable()
        {
            windows.Add(generalNodeWindow.gameObject);
            windows.Add(switchNodeWindow.gameObject);
            windows.Add(variableNodeWindow.gameObject);
            windows.Add(ifNodeWindow.gameObject);
        }

        public void ShowProperty(GraphItem node)
        {
            TurnOffAll();

            switch (node.GetNodeType)
            {
                case NodeType.SWITCH: switchNodeWindow.ShowProperty(node); break;
                case NodeType.VARIABLE: variableNodeWindow.ShowProperty(node); break;
                case NodeType.IF: ifNodeWindow.ShowProperty(node); break;
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