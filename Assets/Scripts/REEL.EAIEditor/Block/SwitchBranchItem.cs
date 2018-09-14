using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class SwitchBranchItem : GraphItem
    {
        [SerializeField] private BranchCondition[] switchBranchDatas;
        [SerializeField] private int currentBlockNumber = 2;
        [SerializeField] private GameObject[] caseBlocks = null;

        [SerializeField] private NodeType switchType;
        [SerializeField] private string switchName;

        public XMLSwitchNode GetXMLSwitchData()
        {
            XMLSwitchNode xmlNode = new XMLSwitchNode();
            xmlNode.nodeID = BlockID;
            xmlNode.nodeType = GetNodeType;
            xmlNode.nodeTitle = GetBlockTitle;
            xmlNode.xmlSwitch = new XMLSwitch();
            xmlNode.xmlSwitch.comparerType = GetSwitchType;
            xmlNode.xmlSwitch.name = GetSwitchName;

            xmlNode.xmlSwitch.switchCase = new List<XMLSwitchCase>();

            foreach (ExecuteSwitchPoint point in executePoints)
            {
                if (point.GetPointPosition == ExecutePoint.PointPosition.ExecutePoint_Left) continue;

                if (point.GetHasLineState)
                {
                    if (point.GetSwitchPointType == ExecuteSwitchPoint.SwitchPointType.Case)
                    {
                        ExecuteCasePoint casePoint = point as ExecuteCasePoint;
                        XMLCase caseItem = new XMLCase()
                        {
                            caseValue = casePoint.CaseValue,
                            nextID = point.GetLineData.GetRightExecutePointInfo.blockID
                        };

                        xmlNode.xmlSwitch.switchCase.Add(caseItem);
                    }
                    else if (point.GetSwitchPointType == ExecuteSwitchPoint.SwitchPointType.Default)
                    {
                        XMLDefault defaultItem = new XMLDefault()
                        {
                            nextID = point.GetLineData.GetRightExecutePointInfo.blockID
                        };

                        xmlNode.xmlSwitch.switchCase.Add(defaultItem);
                    }
                }
            }

            return xmlNode;
        }

        public void SetSwitchName(string name)
        {
            switchName = name;
        }

        public string GetSwitchName { get { return switchName; } }

        public void SetSwitchType(int type)
        {
            switchType = (NodeType)type;
        }

        public void SetSwitchType(string type)
        {
            switchType = (NodeType)Enum.Parse(typeof(NodeType), type);
        }

        public void SetSwitchType(NodeType type)
        {
            switchType = type;
        }

        public NodeType GetSwitchType { get { return switchType; } }

        public void SetBlockCount(int blockCount)
        {
            if (IsBlockCountOK(blockCount))
            {
                DisableAllBlock();
                currentBlockNumber = blockCount;

                for (int ix = 0; ix < blockCount - 2; ++ix)
                {
                    caseBlocks[ix].SetActive(true);
                }
            }
        }

        public void SetBlockData(int blockCount, BranchConditionComponent[] switchBlocks)
        {
            if (IsBlockCountOK(blockCount))
            {
                DisableAllBlock();
                currentBlockNumber = blockCount;
                
                for (int ix = 0; ix < blockCount - 2; ++ix)
                {   
                    caseBlocks[ix].SetActive(true);
                }

                SetConditionData(switchBlocks);
            }
        }

        private void SetConditionData(BranchConditionComponent[] switchBlocks)
        {
            switchBranchDatas = new BranchCondition[currentBlockNumber];
            for (int ix = 0; ix < currentBlockNumber; ++ix)
            {
                switchBranchDatas[ix] = switchBlocks[ix].GetBranchCondition();
            }
        }

        public int GetBlockCount
        {
            get { return currentBlockNumber; }
        }

        public BranchCondition[] GetConditionData()
        {
            return switchBranchDatas;
        }

        private void DisableAllBlock()
        {
            for (int ix = 0; ix < caseBlocks.Length; ++ix)
            {
                caseBlocks[ix].SetActive(false);
            }
        }

        bool IsBlockCountOK(int blockCount)
        {
            return blockCount >= 2 && blockCount <= 5;
        }
    }
}