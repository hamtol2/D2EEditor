using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
	public class VariableItem : GraphItem
	{
        [SerializeField] private string blockName;
        [SerializeField] private XMLVariableOperatorType operatorType;

        public XMLVariableNode GetXMLVariableData()
        {
            XMLVariableNode xmlNode = new XMLVariableNode();
            xmlNode.nodeID = BlockID;
            xmlNode.nodeTitle = GetBlockTitle;
            xmlNode.nodeName = GetBlockName;
            xmlNode.nodeType = GetNodeType;
            xmlNode.nodeValue = GetItemData() == null ? "" : GetItemData().ToString();
            xmlNode.operatorType = GetOperatorType;

            if (executePoints != null || executePoints.Length > 0)
            {
                foreach (ExecutePoint executePoint in executePoints)
                {
                    if (executePoint.GetPointPosition == ExecutePoint.PointPosition.ExecutePoint_Right)
                        if (executePoint.GetLineData)
                            xmlNode.nextID = executePoint.GetLineData.GetRightExecutePointInfo.blockID;
                }
            }

            return xmlNode;
        }

        public void SetOperatorType(string type)
        {
            operatorType = (XMLVariableOperatorType)Enum.Parse(typeof(XMLVariableOperatorType), type);
        }

        public void SetOperatorType(int type)
        {
            operatorType = (XMLVariableOperatorType)type;
        }

        public void SetOperatorType(XMLVariableOperatorType type)
        {
            operatorType = type;
        }

        public XMLVariableOperatorType GetOperatorType {  get { return operatorType; } }

        public void SetBlockName(string name)
        {
            blockName = name;
        }

        public string GetBlockName { get { return blockName; } }
    }
}