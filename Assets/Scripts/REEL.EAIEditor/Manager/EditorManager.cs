using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace REEL.EAIEditor
{
    public class EditorManager : Singleton<EditorManager>
    {
        public enum EPaneType
        {
            Graph_Pane, Length
        }

        public enum ETargetMenuType
        {
            Event, Hearing, SayTextBox, HearingTextBox, IFBranch, SwitchBranch, Length
        }

        [SerializeField] private GameObject[] paneObjects;
        [SerializeField] private MenuPopup[] targetMenuObjects;
        [SerializeField] private GraphicRaycaster uiRaycaster;
        [SerializeField] private GraphItem[] nodePrefabs;

        private Dictionary<NodeType, GraphItem> prefabDictionary = null;

        public GraphicRaycaster UIRaycaster { get { return uiRaycaster; } }

        private void Awake()
        {
            InitPrefabDictionary();
        }

        public GraphItem GetNodePrefab(NodeType nodeType)
        {
            GraphItem retItem;
            if (prefabDictionary.TryGetValue(nodeType, out retItem))
                return retItem;

            return null;
        }

        private void InitPrefabDictionary()
        {
            prefabDictionary = new Dictionary<NodeType, GraphItem>();

            for (int ix = 0; ix < nodePrefabs.Length; ++ix)
            {
                prefabDictionary.Add(nodePrefabs[ix].GetNodeType, nodePrefabs[ix]);
            }
        }

        public GameObject GetPaneObject(EPaneType paneType)
        {
            int length = (int)EPaneType.Length;
            for (int ix = 0; ix < length; ++ix)
            {
                if (Util.CompareTwoStrings(paneObjects[ix].tag, ((EPaneType)(ix)).ToString()))
                {
                    return paneObjects[ix];
                }
            }

            return null;
        }

        public MenuPopup GetTargetMenuObject(ETargetMenuType targetMenuType)
        {
            int length = (int)ETargetMenuType.Length;
            for (int ix = 0; ix < length; ++ix)
            {
                if (Util.CompareTwoStrings(targetMenuObjects[ix].tag, targetMenuType.ToString()))
                    return targetMenuObjects[ix];
            }

            return null;
        }
    }
}