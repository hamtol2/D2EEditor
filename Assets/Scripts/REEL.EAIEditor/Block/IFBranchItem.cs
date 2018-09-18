using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REEL.EAIEditor
{
    public class IFBranchItem : GraphItem
    {
        [SerializeField]
        private BranchCondition ifBranchData = null;

        [SerializeField]
        private ExecuteIFPoint executeTrue;
        [SerializeField]
        private ExecuteIFPoint executeFalse;

        public XMLIFNode GetXMLIFData()
        {
            XMLIFNode xmlNode = new XMLIFNode();
            xmlNode.nodeID = BlockID;
            xmlNode.nodeType = NodeType.IF;
            xmlNode.nodeTitle = "액션장르?";

            xmlNode.ifProperty = new IFProperty();
            xmlNode.ifProperty.ifType = NodeType.VARIABLE;
            xmlNode.ifProperty.name = "genre";
            xmlNode.ifProperty.operatorValue = "==";
            xmlNode.ifProperty.compareValue = "액션";
            xmlNode.ifProperty.trueValue = new IFTrue();
            xmlNode.ifProperty.trueValue.nextID = 128;
            xmlNode.ifProperty.falseValue = new IFFalse();
            xmlNode.ifProperty.falseValue.nextID = 129;

            return xmlNode;
        }

        public void SetParameters(BranchCondition branchCondition)
        {
            if (ifBranchData == null) ifBranchData = new BranchCondition(branchCondition);
            else ifBranchData.SetValues(branchCondition);
        }

        public BranchCondition GetIFBranchData()
        {
            return ifBranchData;
        }
    }
}